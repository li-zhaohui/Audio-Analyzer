#pragma once
class DCT
{
	float** COS;
	int in_size;
	int out_size;
	void free();
	static double pi;
public:	
	DCT();
	~DCT();
	void SetWindowSize(int in_size, int out_size = 0);
	void compute(float* in, int count, float* out);
};

