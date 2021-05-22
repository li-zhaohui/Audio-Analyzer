using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AudioAnalyzer
{
    enum WavChunks
    {
        RiffHeader = 0x46464952,
        WavRiff = 0x54651475,
        Format = 0x020746d66,
        LabeledText = 0x478747C6,
        Instrumentation = 0x478747C6,
        Sample = 0x6C706D73,
        Fact = 0x47361666,
        Data = 0x61746164,
        Junk = 0x4b4e554a,
    };

    enum WavFormat
    {
        PulseCodeModulation = 0x01,
        IEEEFloatingPoint = 0x03,
        ALaw = 0x06,
        MuLaw = 0x07,
        IMAADPCM = 0x11,
        YamahaITUG723ADPCM = 0x16,
        GSM610 = 0x31,
        ITUG721ADPCM = 0x40,
        MPEG = 0x50,
        Extensible = 0xFFFE
    };
    public class WaveAnalyzer
    {
        public String filename;

        int headerid;
        int memsize;
        int riffstyle;

        int formatsize;
        short format;
        short channels;
        public int samplerate;
        int bitspersecond;
        short formatblockalign;
        short bitdepth;

        int datasize;

        public int duration;
        public static int wnd_per_sec = 20;
        public int samplecount;
        public int in_wnd_size;
        public bool is_valid;
        float[] sampledata;
        
        static DCT dct_processor = new DCT();

        public WaveAnalyzer() 
        { 

        }
        public void ReadFile(String filename_t)
        {
            this.filename = filename_t;

            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                int chunkid = 0;
                bool datachunk = false;

                while (!datachunk)
                {
                    chunkid = reader.ReadInt32();
                    switch ((WavChunks)chunkid)
                    {
                        case WavChunks.Format:
                            formatsize = reader.ReadInt32();
                            format = reader.ReadInt16();
                            channels = reader.ReadInt16();
                            samplerate = reader.ReadInt32();
                            bitspersecond = reader.ReadInt32();
                            formatblockalign = reader.ReadInt16();
                            bitdepth = reader.ReadInt16();

                            if (formatsize == 18)
                            {
                                int extradata;
                                extradata = reader.ReadInt16();
                                reader.BaseStream.Seek(extradata, SeekOrigin.Current);
                            }
                            break;
                        case WavChunks.RiffHeader:
                            headerid = chunkid;
                            memsize = reader.ReadInt32();
                            riffstyle = reader.ReadInt32();
                            break;
                        case WavChunks.Data:
                            datachunk = true;
                            datasize = reader.ReadInt32();
                            break;
                        default:
                            int skipsize = reader.ReadInt32();
                            reader.BaseStream.Seek(skipsize, SeekOrigin.Current);
                            break;
                    }
                }

                is_valid = true;
                duration = datasize / bitspersecond;
                samplecount = datasize / formatblockalign;
                sampledata = new float[samplecount];
                in_wnd_size = samplerate / wnd_per_sec;

                for (int i = 0; i < samplecount; i++)
                {

                    if (bitdepth == 8)
                    {
                        byte ch = reader.ReadByte();
                        sampledata[i] = (float)(1.0 * ch / 256 - 0.5);
                    }
                    else if (bitdepth == 16)
                    {
                        short sh = reader.ReadInt16();
                        sampledata[i] = (float)(1.0 * sh / 32768);
                    }
                    else if (bitdepth == 24)
                    {
                        byte ch;
                        short sh;

                        ch = reader.ReadByte();
                        sh = reader.ReadInt16();
                        sampledata[i] = (float)(1.0 * (sh * 256 + ch) / 32768 / 256);
                    }
                    else if (bitdepth == 32)
                    {
                        int sh = reader.ReadInt32();
                        sampledata[i] = (float)(1.0 * sh / 32768 / 256 / 256);
                    }
                    int skipsize = (channels - 1) * bitdepth / 8;
                    reader.BaseStream.Seek(skipsize, SeekOrigin.Current);
                }
                reader.Close();
            }

        }
        public float[,] GetSpectre(ref int out_wnd_size, ref int tick_count)
        {
            out_wnd_size = Math.Min(in_wnd_size, out_wnd_size);

            tick_count = Math.Min(tick_count, duration * wnd_per_sec);

            dct_processor.SetWindowSize(in_wnd_size, out_wnd_size);

            tick_count++;
            float[,] spectre = new float[tick_count, out_wnd_size];

            int sample_offset = 0;
            for (int i = 0; i < tick_count; i++)
            {
                int in_count = Math.Min(samplecount - sample_offset, in_wnd_size);

                dct_processor.Compute(sampledata,sample_offset, in_count, spectre, i);

                sample_offset += in_wnd_size;
            }

            return spectre;
        }
    }

}
