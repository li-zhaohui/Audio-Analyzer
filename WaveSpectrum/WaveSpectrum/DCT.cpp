#include "pch.h"
#include "DCT.h"
double DCT::pi = acos(-1);
DCT::DCT() 
{
	COS = NULL;
	in_size = 0;
}

DCT::~DCT()
{
	free();
}

void DCT::free() {
	if (COS)
	{
		for (int i = 0; i < out_size; i++)
		{
			delete COS[i];
		}
		delete COS;
	}
	COS = NULL;
}

void DCT::SetWindowSize(int in_size_t, int out_size_t)
{
	
	free();
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

	
	float scale0 = 1.0 / sqrt(in_size);
	float scale1 = sqrt(2.0 / in_size);

	COS = new float*[out_size];
	for (int i = 0; i < out_size; i++) 
	{
		COS[i] = new float[in_size];
		float scale = (i == 0) ? scale0 : scale1;
		float freq_multi = pi / in_size * i;

		for (int j = 0; j < in_size; j++)
		{
			COS[i][j] = scale * cos(freq_multi * (j + 0.5));
		}
	}

}

void DCT::compute(float* in, int count, float* out)
{
	if (COS == NULL)return;
	if (count > in_size)
	{
		count = in_size;
	}
	int lifter = 100;
	for (int i = 0; i < out_size; ++i) 
	{
		out[i] = 0.0;
		for (int j = 0; j < count; ++j)
		{
			out[i] += in[j] * COS[i][j];
		}
	}

}