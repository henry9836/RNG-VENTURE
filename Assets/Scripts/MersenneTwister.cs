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
			mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * (UInt32)1664525)) + (UInt32)seed[j] + (UInt32)j; /* non linear */
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
			mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * (UInt32)1566083941)) - (UInt32)i; /* non linear */
			i++;
			if (i >= N)
			{
				mt[0] = mt[N - 1];
				i = 1;
			}
		}
		mt[0] = 2147483648;
	}

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

	public double genrand_real1()       /* divided by 2^32-1 */
	{
		return genrand_Int32() * ((double)1.0 / 4294967295.0);
	}
	public double genrand_real2()       /* divided by 2^32 */
	{
		return genrand_Int32() * ((double)1.0 / 4294967296.0);
	}
	public double genrand_res53()       //with 53 - bit resolution
	{
		UInt32 a = genrand_Int32() >> 5, b = genrand_Int32() >> 6;
		return ((double)a * 67108864.0 + b) * ((double)1.0 / 9007199254740992.0);
	}
}