using System;
using System.Collections.Generic;
using System.Text;

namespace AudioAnalyzer
{ 

    public class DCT
    {
        int in_size;
        int out_size;
        float[,] COS;
        public DCT() 
        { 
        }
        public void SetWindowSize(int in_size_t, int out_size_t = 0)
        {
            if (in_size_t == 0)
            {
                return;
            }

            if (out_size_t == 0)
            {
                out_size_t = in_size_t;
            }

            in_size = in_size_t;
            out_size = out_size_t;


            float scale0 = (float)(1.0 / Math.Sqrt(in_size));
            float scale1 = (float)(Math.Sqrt(2.0 / in_size));

            COS = new float[out_size, in_size];
            for (int i = 0; i < out_size; i++)
            {
                float scale = (i == 0) ? scale0 : scale1;
                float freq_multi = (float)(Math.PI / in_size * i);

                for (int j = 0; j < in_size; j++)
                {
                    COS[i,j] = (float)(scale * Math.Cos(freq_multi * (j + 0.5)));
                }
            }

        }

        public void Compute(float[] in_data,int start_index, int count, float[,] spectr, int out_index)
        {
            if (COS == null) return;
            if (count > in_size)
            {
                count = in_size;
            }

            for (int i = 0; i < out_size; ++i)
            {
	            spectr[out_index, i] = 0.0f;
                for (int j = 0; j < count; ++j)
                {
		            spectr[out_index, i] += in_data[start_index + j] * COS[i,j];
                }
                spectr[out_index, i] = Math.Min(Math.Max(spectr[out_index, i], -1), 1);
            }
        }
    }
    
}
