#include "pch.h"
#include "WaveReader.h"

WaveReader::WaveReader() 
{
    sampledata = NULL;
}
WaveReader::~WaveReader()
{
    Free();
}

void WaveReader::Free() 
{
    if (sampledata)
    {
        delete sampledata;
    }
    sampledata = NULL;
}

void WaveReader::SetFile(CString strFileName) 
{
	m_strFileName = strFileName;
    ReadFile();
}

void WaveReader::ReadFile() 
{
    Free();
    is_valid = false;

    CFile file;
    if (file.Open(m_strFileName, CFile::modeRead) == FALSE)
    {
        return;
    }
    int chunkid = 0;
    bool datachunk = false;
    while (!datachunk) {
        
        file.Read(&chunkid, 4);
        switch ((WavChunks)chunkid) {
        case WavChunks::Format:
            file.Read(&formatsize, 4);
            file.Read(&format, 2);
            file.Read(&channels, 2);
            file.Read(&samplerate, 4);
            file.Read(&bitspersecond, 4);
            file.Read(&formatblockalign, 2);
            file.Read(&bitdepth, 2);

            if (formatsize == 18) 
            {
                int extradata;
                file.Read(&extradata, 16);
                file.Seek(extradata, CFile::current);
            }
            break;
        case WavChunks::RiffHeader:
            headerid = chunkid;
            file.Read(&memsize, 4);
            file.Read(&riffstyle, 4);
            break;
        case WavChunks::Data:
            datachunk = true;
            file.Read(&datasize, 4);
            break;
        default:
            int skipsize;
            file.Read(&skipsize, 4);
            file.Seek(skipsize, CFile::current);
            break;
        }
    }

    is_valid = true;
    duration = datasize / bitspersecond;
    samplecount = datasize / formatblockalign;
    sampledata = new float[samplecount];
    for (int i = 0; i < samplecount; i++)
    {

        if (bitdepth == 8)
        {
            unsigned char ch;
            file.Read(&ch, 1);
            sampledata[i] = 1.0 * ch / 256 - 0.5;
        }
        else if (bitdepth == 16)
        {
            short sh;
            file.Read(&sh, 2);
            sampledata[i] = 1.0 * sh / 32768;
        }
        else if (bitdepth == 24) 
        {
            int sh = 0;
            file.Read(&sh, 3);
            sampledata[i] = 1.0 * sh / 32768 / 256;
        }
        else if (bitdepth == 32)
        {
            int sh;
            file.Read(&sh, 4);
            sampledata[i] = 1.0 * sh / 32768 / 256 / 256;
        }
        int skipsize = (channels - 1) * bitdepth / 8;
        file.Seek(skipsize, CFile::current);
    }
    file.Close();
}