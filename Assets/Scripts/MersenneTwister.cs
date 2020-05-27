using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MersenneTwister : MonoBehaviour
{
	private const Int16 N = 624;
	private const Int16 M = 397;
	private const UInt32 MATRIX_A = (UInt32)0x9908b0df;   /* constant vector a */
	private const UInt32 UPPER_MASK = (UInt32)0x80000000; /* most significant w-r bits */
	private const UInt32 LOWER_MASK = (UInt32)0x7fffffff; /* least significant r bits */
	private UInt32[] mt = new UInt32[N]; /* the array for the state vector  */
	private UInt16 mti = N + 1; /* mti==N+1 means mt[N] is not initialized */
	private UInt32[] mag01 = new UInt32[] { 0, MATRIX_A };

	public MersenneTwister()
	{
		UInt32[] seed_key = new UInt32[6];
		Byte[] rnseed = new Byte[8];

		seed_key[0] = (UInt32)System.DateTime.Now.Millisecond;
		seed_key[1] = (UInt32)System.DateTime.Now.Second;
		seed_key[2] = (UInt32)System.DateTime.Now.DayOfYear;
		seed_key[3] = (UInt32)System.DateTime.Now.Year;
		seed_key[4] = ((UInt32)rnseed[0] << 24) | ((UInt32)rnseed[1] << 16) | ((UInt32)rnseed[2] << 8) | ((UInt32)rnseed[3]);
		seed_key[5] = ((UInt32)rnseed[4] << 24) | ((UInt32)rnseed[5] << 16) | ((UInt32)rnseed[6] << 8) | ((UInt32)rnseed[7]);


		UInt32 i, j;
		Int32 k;
		Int32 key_length = seed_key.Length;

		mt[0] = 19650218;

		for (mti = 1; mti < N; mti++)
		{
			mt[mti] = ((UInt32)1812433253 * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
		}

		i = 1; j = 0;
		k = (N > key_length ? N : key_length);

		for (; k > 0; k--)
		{
			mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * (UInt32)1664525))
				+ seed_key[j] + (UInt32)j; /* non linear */
			i++; j++;
			if (i >= N) { mt[0] = mt[N - 1]; i = 1; }
			if (j >= key_length) j = 0;
		}
		for (k = N - 1; k > 0; k--)
		{
			mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * (UInt32)1566083941))
				- (UInt32)i; /* non linear */
			i++;
			if (i >= N)
			{
				mt[0] = mt[N - 1];
				i = 1;
			}
		}
		mt[0] = 0x80000000; /* MSB is 1; assuring non-zero initial array */
	}

	public UInt32 genrand_Int32()
	{
		UInt32 y;

		if (mti >= N)
		{ /* generate N words at one time */
			Int16 kk;

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

		return y;
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