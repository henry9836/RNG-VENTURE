using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MersenneTwister : MonoBehaviour
{
	private const int N = 624;
	private const int M = 397;
	private const UInt32 UPPER_MASK = (UInt32)2147483648; /* most significant w-r bits */
	private const UInt32 LOWER_MASK = (UInt32)2147483647; /* least significant r bits */

	private UInt32[] mt; /* the array for the state vector  */
	private UInt16 mti; /* mti==N+1 means mt[N] is not initialized */
	private UInt32[] mag01; /* matrix a constant vector a */

	public MersenneTwister()
	{
		reseed(-9999.0f);
	}

	public void reseed(float newseed)
	{
		mt = new UInt32[N];
		mti = N + 1;
		mag01 = new UInt32[] { 0, (UInt32)0x9908b0df };

		List<float> seed = new List<float>() { };

		if (newseed == -9999.0f)
		{
			seed.Add(System.DateTime.Now.Millisecond);
			seed.Add(System.DateTime.Now.Second);
			seed.Add(System.DateTime.Now.DayOfYear);
			seed.Add(System.DateTime.Now.Year);
		}
		else
		{
			seed.Add(newseed);
		}



		mt[0] = 19650218;

		for (mti = 1; mti < N; mti++)
		{
			mt[mti] = ((UInt32)1812433253 * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
		}

		int i = 1;
		int j = 0;

		for (int k = N; k > 0; k--)
		{
			mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * (UInt32)1664525)) + (UInt32)seed[j] + (UInt32)j; 
			i++;
			j++;
			if (i >= N)
			{
				mt[0] = mt[N - 1];
				i = 1;
			}
			if (j >= seed.Count)
			{
				j = 0;
			}
		}

		for (int k = N - 1; k > 0; k--)
		{
			mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * (UInt32)1566083941)) - (UInt32)i; 
			i++;
			if (i >= N)
			{
				mt[0] = mt[N - 1];
				i = 1;
			}
		}
		mt[0] = 2147483648;
	}

	public double genrand_res53()
	{
		UInt32 a = genrand_Int32() >> 5;
		UInt32 b = genrand_Int32() >> 6;
		return ((double)a * 67108864.0 + b) * ((double)1.0 / 9007199254740992.0);
	}

/* 
A C-program for MT19937, with initialization improved 2002/1/26.
Coded by Takuji Nishimura and Makoto Matsumoto.

Before using, initialize the state by using init_genrand(seed)  
or init_by_array(init_key, key_length).

Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
All rights reserved.                          
Copyright (C) 2005, Mutsuo Saito,
All rights reserved.                          

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

  1. Redistributions of source code must retain the above copyright
	 notice, this list of conditions and the following disclaimer.

  2. Redistributions in binary form must reproduce the above copyright
	 notice, this list of conditions and the following disclaimer in the
	 documentation and/or other materials provided with the distribution.

  3. The names of its contributors may not be used to endorse or promote 
	 products derived from this software without specific prior written 
	 permission.

	 THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
	 "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
	 LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
	 A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
	 CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
	 EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
	 PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
	 PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
	 LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
	 NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


	 Any feedback is very welcome.
	 http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html
	 email: m-mat @ math.sci.hiroshima-u.ac.jp (remove space)
	*/


	public UInt32 genrand_Int32()
	{
		UInt32 y;

		if (mti >= N)
		{ /* generate N words at one time */
			int kk;

			for (kk = 0; kk < N - M; kk++)
			{
				y = ((mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK)) >> 1;
				mt[kk] = mt[kk + M] ^ mag01[mt[kk + 1] & 1] ^ y;
			}
			for (; kk < N - 1; kk++)
			{
				y = ((mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK)) >> 1;
				mt[kk] = mt[kk + (M - N)] ^ mag01[mt[kk + 1] & 1] ^ y;
			}
			y = ((mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK)) >> 1;
			mt[N - 1] = mt[M - 1] ^ mag01[mt[0] & 1] ^ y;

			mti = 0;
		}

		y = mt[mti++];

		/* Tempering */
		y ^= (y >> 11);
		y ^= (y << 7) & 0x9d2c5680;
		y ^= (y << 15) & 0xefc60000;
		y ^= (y >> 18);

		return (y);
	}
}