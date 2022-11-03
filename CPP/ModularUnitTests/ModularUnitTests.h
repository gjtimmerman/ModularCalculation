#pragma once

#ifdef _WIN32
using namespace Microsoft::VisualStudio::CppUnitTestFramework;
#define BEGIN_TEST_CLASS
#define END_TEST_CLASS
#else
#define TEST_CLASS(name) class name
#define BEGIN_TEST_CLASS std::vector<std::function<void()>>funcarray={[]()->void{Assert::IsTrue(true);}
#define END_TEST_CLASS };
#define TEST_METHOD(name) ,[]()->void
#include <functional>
#include <vector>

class Assert
{
public:
	static bool Condition;
	static void IsTrue(bool condition)
	{
		Condition = condition;
	}
	static void IsFalse(bool condition)
	{
		Condition = !condition;
	}
	template<typename T>
	static void  ExpectException(std::function<void()> f)
	{
		try
		{
			f();
			Condition = false;
		}
		catch (T t)
		{
			Condition = true;
			return;
		}
		catch (...)
		{
			Condition = false;
			return;
		}
		return;
	}

};

bool Assert::Condition = false;
#endif

#include "../ModularCalculation/ModularCalculation.h"


namespace ModularUnitTests
{
	TEST_CLASS(ModularUnitTests)
	{
	public:
		BEGIN_TEST_CLASS
		TEST_METHOD(TestSubtractSimple)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = 2ull;
				r[i] = 1ull;
			}
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber res = ml - mr;

			Assert::IsTrue(mr == res);
		}
		TEST_METHOD(TestSubtractAssignSimple)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = 2ull;
				r[i] = 1ull;
			}
			ModNumber ml(l);
			ModNumber mr(r);
			ml -= mr;

			Assert::IsTrue(mr == ml);
		}
		TEST_METHOD(TestSubtractWithCarry)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = 0ul;
			l[1] = 1ul;
			r[0] = 1ul;
			r[1] = 0ul;
			llint exp[COUNTLL] = {};
			exp[0] = ~0ull;

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber res = ml - mr;

			Assert::IsTrue(mexp == res);
		}
		TEST_METHOD(TestSubtractAssignWithCarry)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = 0ul;
			l[1] = 1ul;
			r[0] = 1ul;
			r[1] = 0ul;
			llint exp[COUNTLL] = {};
			exp[0] = ~0ull;

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ml -= mr;

			Assert::IsTrue(mexp == ml);
		}
		TEST_METHOD(TestSubtractWithCarryAcrossMultipleSections)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = 0ul;
			l[COUNTLL - 1] = 1ul;
			r[0] = 1ul;
			r[COUNTLL - 1] = 0ul;
			llint exp[COUNTLL];
			exp[COUNTLL - 1] = 0ull;
			for (int i = 0; i < COUNTLL - 1; i++)
			{
				exp[i] = ~0ull;
			}

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber res = ml - mr;

			Assert::IsTrue(mexp == res);
		}
		TEST_METHOD(TestSubtractAssignWithCarryAcrossMultipleSections)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = 0ul;
			l[COUNTLL - 1] = 1ul;
			r[0] = 1ul;
			r[COUNTLL - 1] = 0ul;
			llint exp[COUNTLL];
			exp[COUNTLL - 1] = 0ull;
			for (int i = 0; i < COUNTLL - 1; i++)
			{
				exp[i] = ~0ull;
			}

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ml -= mr;
			Assert::IsTrue(mexp == ml);
		}

		TEST_METHOD(TestSubtractOneFromZero)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			r[0] = 1ull;
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				exp[i] = ~0ull;
			}
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber res = ml - mr;

			Assert::IsTrue(mexp == res);
		}
		TEST_METHOD(TestSubtractAssignOneFromZero)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			r[0] = 1ull;
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				exp[i] = ~0ull;
			}
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ml -= mr;

			Assert::IsTrue(mexp == ml);
		}
		TEST_METHOD(TestSubtractLeftFsFromZero)
		{
			llint r[COUNTLL] = {};
			r[COUNTLL-1] = ~0ull;
			llint exp[COUNTLL] = {};
			exp[COUNTLL - 1] = 1ull;
			ModNumber ml;
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber res = ml - mr;

			Assert::IsTrue(mexp == res);
		}
		TEST_METHOD(TestSubtractAssignLeftFsFromZero)
		{
			llint r[COUNTLL] = {};
			r[COUNTLL - 1] = ~0ull;
			llint exp[COUNTLL] = {};
			exp[COUNTLL - 1] = 1ull;
			ModNumber ml;
			ModNumber mr(r);
			ModNumber mexp(exp);
			ml -= mr;

			Assert::IsTrue(mexp == ml);
		}
		TEST_METHOD(TestSubtractAllFsFromZero)
		{
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				r[i] = ~0ull;
			ModNumber ml;
			ModNumber mr(r);
			ModNumber mexp(1ull);
			ModNumber res = ml - mr;

			Assert::IsTrue(mexp == res);
		}
		TEST_METHOD(TestSubtractAssignAllFsFromZero)
		{
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				r[i] = ~0ull;
			ModNumber ml;
			ModNumber mr(r);
			ModNumber mexp(1ull);
			ml -= mr;

			Assert::IsTrue(mexp == ml);
		}

		TEST_METHOD(TestShiftLeftZero)
		{
			ModNumber ml(12345678ull);
			Assert::IsTrue(ml == ml << 0);
		}
		TEST_METHOD(TestShiftLeftAssignZero)
		{
			ModNumber ml(12345678ull);
			ModNumber exp(ml);
			Assert::IsTrue(exp == (ml <<= 0));
		}
		TEST_METHOD(TestShiftLeftOne)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp(0x2468aull);
			Assert::IsTrue(mexp == ml << 1);
		}
		TEST_METHOD(TestShiftLeftAssignOne)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp(0x2468aull);
			Assert::IsTrue(mexp == (ml <<= 1));
		}
		TEST_METHOD(TestShiftLeft32)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp(0x1234500000000ull);
			Assert::IsTrue(mexp == ml << 32);
		}
		TEST_METHOD(TestShiftLeftAssign32)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp(0x1234500000000ull);
			Assert::IsTrue(mexp == (ml <<= 32));
		}

		TEST_METHOD(TestShiftLeft60)
		{
			ModNumber ml(0x0102030405060708ull);
			llint exp[COUNTLL] = {};
			exp[0] = 0x8000000000000000ull;
			exp[1] = 0x0010203040506070ull;
			ModNumber mexp(exp);
			ModNumber mres = ml << 60;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestShiftLeftAssign60)
		{
			ModNumber ml(0x0102030405060708ull);
			llint exp[COUNTLL] = {};
			exp[0] = 0x8000000000000000ull;
			exp[1] = 0x0010203040506070ull;
			ModNumber mexp(exp);
			ml <<= 60;
			Assert::IsTrue(mexp == ml);
		}

		TEST_METHOD(TestShiftLeft65)
		{
			ModNumber ml(0x12345ull);
			llint exp[COUNTLL] = {};
			exp[1] = 0x2468aull;
			ModNumber mexp(exp);
			Assert::IsTrue(mexp == ml << 65);
		}
		TEST_METHOD(TestShiftLeftAssign65)
		{
			ModNumber ml(0x12345ull);
			llint exp[COUNTLL] = {};
			exp[1] = 0x2468aull;
			ModNumber mexp(exp);
			Assert::IsTrue(mexp == (ml <<= 65));
		}
		TEST_METHOD(TestShiftLeftNSIZEMinusLSIZETimes8)
		{
			ModNumber ml(0x12345ull);
			lint exp[COUNTL] = {};
			exp[COUNTL-1] = 0x12345ul;
			ModNumber mexp((llint *)exp);
			Assert::IsTrue(mexp == ml << (NSIZE-(LSIZE*8)));
		}
		TEST_METHOD(TestShiftLeftAssignNSIZEMinusLSIZETimes8)
		{
			ModNumber ml(0x12345ull);
			lint exp[COUNTL] = {};
			exp[COUNTL-1] = 0x12345ul;
			ModNumber mexp((llint*)exp);
			Assert::IsTrue(mexp == (ml <<= (NSIZE-(LSIZE*8))));
		}
		TEST_METHOD(TestShiftLeftNSIZE)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp;
			Assert::IsTrue(mexp == ml << NSIZE);
		}
		TEST_METHOD(TestShiftLeftAssignNSIZE)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp;
			ml <<= NSIZE;
			Assert::IsTrue(mexp == ml);
		}

		TEST_METHOD(TestShifRightZero)
		{
			ModNumber ml(12345678ull);
			Assert::IsTrue(ml == ml >> 0);
		}
		TEST_METHOD(TestShiftRightAssignZero)
		{
			ModNumber ml(12345678ull);
			ModNumber exp(ml);
			Assert::IsTrue(exp == (ml >>= 0));
		}
		TEST_METHOD(TestShiftRightOne)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp(0x091a2ull);
			Assert::IsTrue(mexp == ml >> 1);
		}
		TEST_METHOD(TestShiftRightAssignOne)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp(0x091a2ull);
			Assert::IsTrue(mexp == (ml >>= 1));
		}
		TEST_METHOD(TestShiftRight32)
		{
			ModNumber ml(0x1234500000000ull);
			ModNumber mexp(0x12345ull);
			Assert::IsTrue(mexp == ml >> 32);
		}
		TEST_METHOD(TestShiftRightAssign32)
		{
			ModNumber ml(0x1234500000000ull);
			ModNumber mexp(0x12345ull);
			Assert::IsTrue(mexp == (ml >>= 32));
		}

		TEST_METHOD(TestShiftRight60)
		{
			ModNumber mexp(0x0102030405060708ull);
			llint l[COUNTLL] = {};
			l[0] = 0x8000000000000000ull;
			l[1] = 0x0010203040506070ull;
			ModNumber ml(l);
			ModNumber mres = ml >> 60;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestShiftRightAssign60)
		{
			ModNumber mexp(0x0102030405060708ull);
			llint l[COUNTLL] = {};
			l[0] = 0x8000000000000000ull;
			l[1] = 0x0010203040506070ull;
			ModNumber ml(l);
			ml >>= 60;
			Assert::IsTrue(mexp == ml);
		}

		TEST_METHOD(TestShiftRight65)
		{
			ModNumber mexp(0x12345ull);
			llint l[COUNTLL] = {};
			l[1] = 0x2468aull;
			ModNumber ml(l);
			Assert::IsTrue(mexp == ml >> 65);
		}
		TEST_METHOD(TestShiftRightAssign65)
		{
			ModNumber mexp(0x12345ull);
			llint l[COUNTLL] = {};
			l[1] = 0x2468aull;
			ModNumber ml(l);
			Assert::IsTrue(mexp == (ml >>= 65));
		}
		TEST_METHOD(TestShiftRightNSIZEMinusLSIZETimes8)
		{
			ModNumber mexp(0x12345ull);
			lint l[COUNTL] = {};
			l[COUNTL - 1] = 0x12345ul;
			ModNumber ml((llint*)l);
			Assert::IsTrue(mexp == ml >> (NSIZE - (LSIZE * 8)));
		}
		TEST_METHOD(TestShiftRightAssignNSIZEMinusLSIZETimes8)
		{
			ModNumber mexp(0x12345ull);
			lint l[COUNTL] = {};
			l[COUNTL - 1] = 0x12345ul;
			ModNumber ml((llint*)l);
			Assert::IsTrue(mexp == (ml >>= (NSIZE - (LSIZE * 8))));
		}
		TEST_METHOD(TestShiftRightLSIZEtimes8)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp;
			Assert::IsTrue(mexp == ml >> (LSIZE * 8));
		}
		TEST_METHOD(TestShiftRightAssignLSIZEtimes8)
		{
			ModNumber ml(0x12345ull);
			ModNumber mexp;
			ml >>= LSIZE * 8;
			Assert::IsTrue(mexp == ml);
		}

		TEST_METHOD(TestModuloDivideByZero)
		{
			ModNumber l(1ull);
			ModNumber r;
			Assert::ExpectException<std::domain_error>([l, r]() {ModNumber res = l % r; });
		}
		TEST_METHOD(TestModuloDivideByOne)
		{
			ModNumber l(1000ull);
			ModNumber r(1ull);
			ModNumber exp;
			Assert::IsTrue(exp == l % r);
		}
		TEST_METHOD(TestModuloDivideZeroByOne)
		{
			ModNumber l(0ull);
			ModNumber r(1ull);
			ModNumber exp;
			Assert::IsTrue(exp == l % r);
		}
		TEST_METHOD(TestModuloDivideEvenByTwo)
		{
			ModNumber l(1000ull);
			ModNumber r(2ull);
			ModNumber exp;
			Assert::IsTrue(exp == l % r);
		}
		TEST_METHOD(TestModuloDivideOddByTwo)
		{
			ModNumber l(1001ull);
			ModNumber r(2ull);
			ModNumber exp(1);
			Assert::IsTrue(exp == l % r);
		}
		TEST_METHOD(TestModuloDivideSmallByLarge)
		{
			ModNumber l(1001ull);
			ModNumber r(2001ull);
			Assert::IsTrue(l == l % r);
		}
		TEST_METHOD(TestModuloDivideEquals)
		{
			ModNumber l(1001ull);
			ModNumber r(1001ull);
			ModNumber mexp;
			Assert::IsTrue(mexp == l % r);
		}
		TEST_METHOD(TestModuloDividePrimeByFive)
		{
			ModNumber l(101ull);
			ModNumber r(5ull);
			ModNumber exp(1);
			Assert::IsTrue(exp == l % r);
		}
		TEST_METHOD(TestModuloDivide2Pow64ByEight)
		{
			llint l[COUNTLL] = {};
			l[1] = 1ull;
			ModNumber ml(l);
			ModNumber mr(8ull);
			ModNumber exp;
			ModNumber mres = ml % mr;
			Assert::IsTrue(exp == mres);
		}
		TEST_METHOD(TestModuloDivideAllFsBy2Pow16)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			ModNumber ml(l);
			ModNumber mr(65536ull);
			ModNumber exp(65535ull);
			ModNumber mres = ml % mr;
			Assert::IsTrue(exp == mres);
		}
		TEST_METHOD(TestModuloDivideAllFsByAlllFs)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			ModNumber ml(l);
			ModNumber exp;
			ModNumber mres = ml % ml;
			Assert::IsTrue(exp == mres);
		}
		TEST_METHOD(TestModuloDivideAllFsByAlllFsAndZeroLowWord)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				r[i] = ~0ull;
			}
			r[0] = 0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			llint exp[COUNTLL] = {};
			exp[0] = ~0ull;
			ModNumber mexp(exp);
			ModNumber mres = ml % mr;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestModuloDivideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne)
		{
			ModNumber mnprime1(355687428095999);
			lint prime2(39916799ul);
			ModNumber mnprime2(prime2);
			ModNumber product = mnprime1 * prime2;
			ModNumber res1 = product % mnprime1;
			ModNumber res2 = product % mnprime2;
			ModNumber mexp1;
			Assert::IsTrue(res1 == mexp1);
			Assert::IsTrue(res2 == mexp1);
			ModNumber mone(1ull);
			ModNumber mnprime1MinusOne = mnprime1 - mone;
			ModNumber mnprime2MinusOne = mnprime2 - mone;
			ModNumber productMinusPrime1 = product - mnprime1;
			ModNumber productMinusPrime2 = product - mnprime2;
			ModNumber res3 = productMinusPrime1 % mnprime2MinusOne;
			ModNumber res4 = productMinusPrime2 % mnprime1MinusOne;
			ModNumber res5 = productMinusPrime1 % mnprime1;
			ModNumber res6 = productMinusPrime2 % mnprime2;
			Assert::IsTrue(res3 == mexp1);
			Assert::IsTrue(res4 == mexp1);
			Assert::IsTrue(res5 == mexp1);
			Assert::IsTrue(res6 == mexp1);
		}

		TEST_METHOD(TestModuloAssignDivideByZero)
		{
			ModNumber l(1ull);
			ModNumber r;
			Assert::ExpectException<std::domain_error>([&l, r]() {l %= r; });
		}
		TEST_METHOD(TestModuloAssignDivideByOne)
		{
			ModNumber l(1000ull);
			ModNumber r(1ull);
			ModNumber exp;
			l %= r;
			Assert::IsTrue(exp == l);
		}
		TEST_METHOD(TestModuloAssignDivideZeroByOne)
		{
			ModNumber l(0ull);
			ModNumber r(1ull);
			ModNumber exp;
			l %= r;
			Assert::IsTrue(exp == l);
		}
		TEST_METHOD(TestModuloAssignDivideEvenByTwo)
		{
			ModNumber l(1000ull);
			ModNumber r(2ull);
			ModNumber exp;
			l %= r;
			Assert::IsTrue(exp == l);
		}
		TEST_METHOD(TestModuloAssignDivideOddByTwo)
		{
			ModNumber l(1001ull);
			ModNumber r(2ull);
			ModNumber exp(1);
			l %= r;
			Assert::IsTrue(exp == l);
		}
		TEST_METHOD(TestModuloAssignDivideSmallByLarge)
		{
			ModNumber l(1001ull);
			ModNumber r(2001ull);
			ModNumber exp(l);
			l %= r;
			Assert::IsTrue(exp == l);
		}
		TEST_METHOD(TestModuloAssignDivideEquals)
		{
			ModNumber l(1001ull);
			ModNumber r(1001ull);
			ModNumber mexp;
			l %= r;
			Assert::IsTrue(mexp == l);
		}
		TEST_METHOD(TestModuloAssignDividePrimeByFive)
		{
			ModNumber l(101ull);
			ModNumber r(5ull);
			ModNumber exp(1);
			l %= r;
			Assert::IsTrue(exp == l);
		}
		TEST_METHOD(TestModuloAssignDivide2Pow64ByEight)
		{
			llint l[COUNTLL] = {};
			l[1] = 1ull;
			ModNumber ml(l);
			ModNumber mr(8ull);
			ModNumber exp;
			ml %= mr;
			Assert::IsTrue(exp == ml);
		}
		TEST_METHOD(TestModuloAssignDivideAllFsBy2Pow16)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			ModNumber ml(l);
			ModNumber mr(65536ull);
			ModNumber exp(65535ull);
			ml %= mr;
			Assert::IsTrue(exp == ml);
		}
		TEST_METHOD(TestModuloAssignDivideAllFsByAlllFs)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			ModNumber ml(l);
			ModNumber exp;
			ml %= ml;
			Assert::IsTrue(exp == ml);
		}
		TEST_METHOD(TestModuloAssignDivideAllFsByAlllFsAndZeroLowWord)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				r[i] = ~0ull;
			}
			r[0] = 0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			llint exp[COUNTLL] = {};
			exp[0] = ~0ull;
			ModNumber mexp(exp);
			ml %= mr;
			Assert::IsTrue(mexp == ml);
		}

		TEST_METHOD(TestDivideAndModuloByZero)
		{
			ModNumber l(1ull);
			ModNumber r;
			Assert::ExpectException<std::domain_error>([l, r]() {std::tuple<ModNumber,ModNumber> res = DivideAndModulo(l, r); });
		}
		TEST_METHOD(TestDivideAndModuloByOne)
		{
			ModNumber l(1000ull);
			ModNumber r(1ull);
			std::tuple<ModNumber,ModNumber> res	= DivideAndModulo(l,r);
			ModNumber expDiv(l);
			ModNumber expMod;

			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloZeroByOne)
		{
			ModNumber l(0ull);
			ModNumber r(1ull);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(l, r);
			ModNumber expDiv;
			ModNumber expMod;
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloEvenByTwo)
		{
			ModNumber l(1000ull);
			ModNumber r(2ull);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(l, r);
			ModNumber expDiv(500ull);
			ModNumber expMod;
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloOddByTwo)
		{
			ModNumber l(1001ull);
			ModNumber r(2ull);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(l, r);
			ModNumber expDiv(500ull);
			ModNumber expMod(1ull);
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloSmallByLarge)
		{
			ModNumber l(1001ull);
			ModNumber r(2001ull);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(l, r);
			ModNumber expDiv;
			ModNumber expMod(l);
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloEquals)
		{
			ModNumber l(1001ull);
			ModNumber r(1001ull);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(l, r);
			ModNumber expDiv(1ull);
			ModNumber expMod;
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloPrimeByFive)
		{
			ModNumber l(101ull);
			ModNumber r(5ull);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(l, r);
			ModNumber expDiv(20ull);
			ModNumber expMod(1ull);
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModulo2Pow64ByEight)
		{
			llint l[COUNTLL] = {};
			l[1] = 1ull;
			ModNumber ml(l);
			ModNumber mr(8ull);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(ml, mr);
			ModNumber expDiv(ml >> 3);
			ModNumber expMod;
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloAllFsBy2Pow16)
		{
			llint l[COUNTLL];
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				exp[i] = ~0ull;
			}
			exp[COUNTLL-1] ^= (65535ull << (LLSIZE *8 - 16));
			ModNumber ml(l);
			ModNumber mr(65536ull);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(ml, mr);
			ModNumber expDiv(exp);
			ModNumber expMod(65535ull);
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloAllFsLeftAndRightWordByAllFsRightWord)
		{
			llint l[COUNTLL]={};
			llint r[COUNTLL]={};
			llint exp[COUNTLL]={};
			l[0] = ~0ull;
			l[COUNTLL - 1] = ~0ull;
			r[0] = ~0ull;
			exp[0] = 1ull;
			exp[COUNTLL - 1] = 1ull;
			ModNumber ml(l);
			ModNumber mr(r);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(ml, mr);
			ModNumber expDiv(exp);
			ModNumber expMod;
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}

		TEST_METHOD(TestDivideAndModuloAllFsByAlllFs)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			ModNumber ml(l);
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(ml, ml);
			ModNumber expDiv(1ull);
			ModNumber expMod;
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloAllFsByAlllFsAndZeroLowWord)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				r[i] = ~0ull;
			}
			r[0] = 0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			llint exp[COUNTLL] = {};
			exp[0] = ~0ull;
			std::tuple<ModNumber, ModNumber> res = DivideAndModulo(ml, mr);
			ModNumber expDiv(1ull);
			ModNumber expMod(exp);
			Assert::IsTrue(std::get<0>(res) == expDiv);
			Assert::IsTrue(std::get<1>(res) == expMod);
		}
		TEST_METHOD(TestDivideAndModuloProductOfPrimesByBothPrimesAndByBothPrimesMinusOne)
		{
			ModNumber mnprime1(355687428095999);
			lint prime2(39916799ul);
			ModNumber mnprime2(prime2);
			ModNumber product = mnprime1 * prime2;
			std::tuple<ModNumber, ModNumber> res1 = DivideAndModulo(product, mnprime1);
			std::tuple<ModNumber, ModNumber> res2 = DivideAndModulo(product, mnprime2);
			ModNumber mexpDiv1(mnprime2);
			ModNumber mexpMod1;
			ModNumber mexpDiv2(mnprime1);
			Assert::IsTrue(std::get<0>(res1) == mexpDiv1);
			Assert::IsTrue(std::get<1>(res1) == mexpMod1);
			Assert::IsTrue(std::get<0>(res2) == mexpDiv2);
			Assert::IsTrue(std::get<1>(res2) == mexpMod1);
			ModNumber mone(1ull);
			ModNumber mnprime1MinusOne = mnprime1 - mone;
			ModNumber mnprime2MinusOne = mnprime2 - mone;
			ModNumber productMinusPrime1 = product - mnprime1;
			ModNumber productMinusPrime2 = product - mnprime2;
			ModNumber mexpDiv3(mnprime1);
			ModNumber mexpDiv4(mnprime2);
			ModNumber mexpDiv5(mnprime2MinusOne);
			ModNumber mexpDiv6(mnprime1MinusOne);
			std::tuple<ModNumber, ModNumber> res3 = DivideAndModulo(productMinusPrime1, mnprime2MinusOne);
			std::tuple<ModNumber, ModNumber> res4 = DivideAndModulo(productMinusPrime2, mnprime1MinusOne);
			std::tuple<ModNumber, ModNumber> res5 = DivideAndModulo(productMinusPrime1, mnprime1);
			std::tuple<ModNumber, ModNumber> res6 = DivideAndModulo(productMinusPrime2, mnprime2);
			Assert::IsTrue(std::get<0>(res3) == mexpDiv3);
			Assert::IsTrue(std::get<1>(res3) == mexpMod1);
			Assert::IsTrue(std::get<0>(res4) == mexpDiv4);
			Assert::IsTrue(std::get<1>(res4) == mexpMod1);
			Assert::IsTrue(std::get<0>(res5) == mexpDiv5);
			Assert::IsTrue(std::get<1>(res5) == mexpMod1);
			Assert::IsTrue(std::get<0>(res6) == mexpDiv6);
			Assert::IsTrue(std::get<1>(res6) == mexpMod1);
		}

		TEST_METHOD(TestDivideByZero)
		{
			ModNumber l(1ull);
			ModNumber r;
			Assert::ExpectException<std::domain_error>([l, r]() {ModNumber res = l / r; });
		}
		TEST_METHOD(TestDivideByOne)
		{
			ModNumber l(1000ull);
			ModNumber r(1ull);
			ModNumber res = l / r;
			ModNumber expDiv(l);

			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivideZeroByOne)
		{
			ModNumber l(0ull);
			ModNumber r(1ull);
			ModNumber res = l / r;
			ModNumber expDiv;
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivideEvenByTwo)
		{
			ModNumber l(1000ull);
			ModNumber r(2ull);
			ModNumber res = l / r;
			ModNumber expDiv(500ull);
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivideOddByTwo)
		{
			ModNumber l(1001ull);
			ModNumber r(2ull);
			ModNumber res = l / r;
			ModNumber expDiv(500ull);
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivideSmallByLarge)
		{
			ModNumber l(1001ull);
			ModNumber r(2001ull);
			ModNumber res = l / r;
			ModNumber expDiv;
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivideEquals)
		{
			ModNumber l(1001ull);
			ModNumber r(1001ull);
			ModNumber res = l / r;
			ModNumber expDiv(1ull);
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDividePrimeByFive)
		{
			ModNumber l(101ull);
			ModNumber r(5ull);
			ModNumber res = l / r;
			ModNumber expDiv(20ull);
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivide2Pow64ByEight)
		{
			llint l[COUNTLL] = {};
			l[1] = 1ull;
			ModNumber ml(l);
			ModNumber mr(8ull);
			ModNumber res = ml / mr;
			ModNumber expDiv(ml >> 3);
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivideAllFsBy2Pow16)
		{
			llint l[COUNTLL];
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				exp[i] = ~0ull;
			}
			exp[COUNTLL - 1] ^= (65535ull << (LLSIZE * 8 - 16));
			ModNumber ml(l);
			ModNumber mr(65536ull);
			ModNumber res = ml / mr;
			ModNumber expDiv(exp);
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivideAllFsLeftAndRightWordByAllFsRightWord)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[0] = ~0ull;
			l[COUNTLL - 1] = ~0ull;
			r[0] = ~0ull;
			exp[0] = 1ull;
			exp[COUNTLL - 1] = 1ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber res = ml / mr;
			ModNumber expDiv(exp);
			Assert::IsTrue(res == expDiv);
		}

		TEST_METHOD(TestDivideAllFsByAlllFs)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			ModNumber ml(l);
			ModNumber res = ml / ml;
			ModNumber expDiv(1ull);
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivideAllFsByAlllFsAndZeroLowWord)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				r[i] = ~0ull;
			}
			r[0] = 0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber res = ml / mr;
			ModNumber expDiv(1ull);
			Assert::IsTrue(res == expDiv);
		}
		TEST_METHOD(TestDivideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne)
		{
			ModNumber mnprime1(355687428095999);
			lint prime2(39916799ul);
			ModNumber mnprime2(prime2);
			ModNumber product = mnprime1 * prime2;
			ModNumber res1 = product / mnprime1;
			ModNumber res2 = product / mnprime2;
			ModNumber mexpDiv1(mnprime2);
			ModNumber mexpDiv2(mnprime1);
			Assert::IsTrue(res1 == mexpDiv1);
			Assert::IsTrue(res2 == mexpDiv2);
			ModNumber mone(1ull);
			ModNumber mnprime1MinusOne = mnprime1 - mone;
			ModNumber mnprime2MinusOne = mnprime2 - mone;
			ModNumber productMinusPrime1 = product - mnprime1;
			ModNumber productMinusPrime2 = product - mnprime2;
			ModNumber mexpDiv3(mnprime1);
			ModNumber mexpDiv4(mnprime2);
			ModNumber mexpDiv5(mnprime2MinusOne);
			ModNumber mexpDiv6(mnprime1MinusOne);
			ModNumber res3 = productMinusPrime1 / mnprime2MinusOne;
			ModNumber res4 = productMinusPrime2 / mnprime1MinusOne;
			ModNumber res5 = productMinusPrime1 / mnprime1;
			ModNumber res6 = productMinusPrime2 / mnprime2;
			Assert::IsTrue(res3 == mexpDiv3);
			Assert::IsTrue(res4 == mexpDiv4);
			Assert::IsTrue(res5 == mexpDiv5);
			Assert::IsTrue(res6 == mexpDiv6);
		}


		TEST_METHOD(TestEqualTrue)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			ModNumber ml(l);
			Assert::IsTrue(ml == ml);
		}
		TEST_METHOD(TestSubtractEqualNumbers)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			ModNumber ml(l);
			ModNumber res;
			Assert::IsTrue(ml - ml == res);
		}
		TEST_METHOD(TestEqualNotTrueFirstSection)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = i;
				r[i] = i;
			}
			r[0] -= 1;
			ModNumber ml(l);
			ModNumber mr(r);
			Assert::IsFalse(ml == mr);
		}
		TEST_METHOD(TestEqualNotTrueLastSection)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = i;
				r[i] = i;
			}
			r[COUNTLL-1] -= 1;
			ModNumber ml(l);
			ModNumber mr(r);
			Assert::IsFalse(ml == mr);
		}
		TEST_METHOD(TestLessThanTrue)
		{
			ModNumber ml(1);
			ModNumber mr(2);
			Assert::IsTrue(ml < mr);
		}
		TEST_METHOD(TestLessThanFalse)
		{
			ModNumber ml(1);
			ModNumber mr(2);
			Assert::IsFalse(mr < ml);
		}
		TEST_METHOD(TestGreaterThanTrueForLargeDifference)
		{
			llint l[COUNTLL] = {};
			l[COUNTLL - 1] = 1ull;
			llint r[COUNTLL] = {};
			r[0] = ~0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			Assert::IsTrue(ml > mr);
		}
		TEST_METHOD(TestLessThanFalseForLargeDifference)
		{
			llint l[COUNTLL] = {};
			l[COUNTLL - 1] = 1ull;
			llint r[COUNTLL] = {};
			r[0] = ~0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			Assert::IsFalse(ml < mr);
		}
		TEST_METHOD(TestLessThanFalseForEquality)
		{
			ModNumber ml(1234);
			Assert::IsFalse(ml < ml);
		}
		TEST_METHOD(TestLessThanFalseForEqualityOfZero)
		{
			ModNumber ml;
			Assert::IsFalse(ml < ml);
		}
		TEST_METHOD(TestLessOrEqualTrueForLessThan)
		{
			ModNumber ml(1);
			ModNumber mr(2);
			Assert::IsTrue(ml <= mr);
		}
		TEST_METHOD(TestLessOrEqualFalseForGreaterThan)
		{
			ModNumber ml(1);
			ModNumber mr(2);
			Assert::IsFalse(mr <= ml);
		}
		TEST_METHOD(TestLessOrEqualTrueForEquality)
		{
			ModNumber ml(1234);
			Assert::IsTrue(ml <= ml);
		}
		TEST_METHOD(TestLessOrEqualTrueForEqualityOfZero)
		{
			ModNumber ml;
			Assert::IsTrue(ml <= ml);
		}

		TEST_METHOD(TestGreaterThanTrue)
		{
			ModNumber ml(2);
			ModNumber mr(1);
			Assert::IsTrue(ml > mr);
		}
		TEST_METHOD(TestGreaterThanFalse)
		{
			ModNumber ml(1);
			ModNumber mr(2);
			Assert::IsFalse(ml > mr);
		}
		TEST_METHOD(TestGreaterThanFalseForEquality)
		{
			ModNumber ml(1234);
			Assert::IsFalse(ml > ml);
		}
		TEST_METHOD(TestGreaterThanFalseForEqualityOfZero)
		{
			ModNumber ml;
			Assert::IsFalse(ml > ml);
		}
		TEST_METHOD(TestGreaterOrEqualTrueForGreaterThan)
		{
			ModNumber ml(2);
			ModNumber mr(1);
			Assert::IsTrue(ml >= mr);
		}
		TEST_METHOD(TestGreaterOrEqualFalseForLessThan)
		{
			ModNumber ml(2);
			ModNumber mr(1);
			Assert::IsFalse(mr >= ml);
		}
		TEST_METHOD(TestGreaterOrEqualTrueForEquality)
		{
			ModNumber ml(1234);
			Assert::IsTrue(ml >= ml);
		}
		TEST_METHOD(TestGreaterOrEqualTrueForEqualityOfZero)
		{
			ModNumber ml;
			Assert::IsTrue(ml >= ml);
		}


		TEST_METHOD(TestAddAssignScalarOneToZero)
		{
			llint l[COUNTLL] = {};
			llint res[COUNTLL] = { 1ull };
			ModNumber ml(l);
			ModNumber mres(res);
			ml += 1ul;
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestAddScalarOneToZero)
		{
			llint l[COUNTLL] = {};
			llint exp[COUNTLL] = { 1ull };
			ModNumber ml(l);
			ModNumber morig(ml);
			ModNumber mexp(exp);
			ModNumber mres = ml + 1ul;
			Assert::IsTrue(mexp == mres);
			Assert::IsTrue(ml == morig);

		}
		TEST_METHOD(TestAddAssignScalarOneToFirstSectionMax)
		{
			llint l[COUNTLL];
			l[0] = ~0ull;
			for (int i = 1; i < COUNTLL; i++)
				l[i] = 0ull;
			llint res[COUNTLL] = {};
			res[1] = 1ull;
			ModNumber ml(l);
			ModNumber mres(res);
			ml += 1ul;
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestAddScalarOneToFirstSectionMax)
		{
			llint l[COUNTLL];
			l[0] = ~0ull;
			for (int i = 1; i < COUNTLL; i++)
				l[i] = 0ull;
			llint exp[COUNTLL] = {};
			exp[1] = 1ull;
			ModNumber ml(l);
			ModNumber mexp(exp);
			ModNumber mres = ml + 1ul;
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestAddAssignScalarOneToMax)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			llint res[COUNTLL] = {};
			ModNumber ml(l);
			ModNumber mres(res);
			ml += 1ul;
			Assert::IsTrue(ml == mres);
		}

		TEST_METHOD(TestAddScalarOneToMax)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			llint exp[COUNTLL] = {};
			ModNumber ml(l);
			ModNumber mexp(exp);
			ModNumber mres = ml + 1ul;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestAddAssignScalarMaxToMax)
		{
			llint l[COUNTLL];
			lint r = 0xFFul;
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
			}
			exp[0] = 0xFEull;
			ModNumber ml(l);
			ModNumber mexp(exp);
			ml += r;
			Assert::IsTrue(mexp == ml);
		}
		TEST_METHOD(TestAddScalarMaxToMax)
		{
			llint l[COUNTLL];
			lint r = 0xFFul;
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
			}
			exp[0] = 0xFEull;
			ModNumber ml(l);
			ModNumber mexp(exp);
			ModNumber mres;
			mres = ml + r;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestAddAssignOneToZero)
		{
			llint l[COUNTLL] = {};
			llint res[COUNTLL] = { 1ull };
			ModNumber ml(l);
			ModNumber mr(1ull);
			ModNumber mres(res);
			ml += mr;
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestAddOneToZero)
		{
			llint l[COUNTLL] = {};
			llint exp[COUNTLL] = { 1ull };
			ModNumber ml(l);
			ModNumber morig(ml);
			ModNumber mexp(exp);
			ModNumber mr(1ull);
			ModNumber mres = ml + mr;
			Assert::IsTrue(mexp == mres);
			Assert::IsTrue(ml == morig);

		}
		TEST_METHOD(TestAddAssignOneToFirstSectionMax)
		{
			llint l[COUNTLL];
			l[0] = ~0ull;
			for (int i = 1; i < COUNTLL; i++)
				l[i] = 0ull;
			llint res[COUNTLL] = {};
			res[1] = 1ull;
			ModNumber ml(l);
			ModNumber mres(res);
			ModNumber mr(1ull);
			ml += mr;
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestAddOneToFirstSectionMax)
		{
			llint l[COUNTLL];
			l[0] = ~0ull;
			for (int i = 1; i < COUNTLL; i++)
				l[i] = 0ull;
			llint exp[COUNTLL] = {};
			exp[1] = 1ull;
			ModNumber ml(l);
			ModNumber mexp(exp);
			ModNumber mr(1ull);
			ModNumber mres = ml + mr;
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestAddAssignOneToMax)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			llint res[COUNTLL] = {};
			ModNumber ml(l);
			ModNumber mr(1ull);
			ModNumber mres(res);
			ml += mr;
			Assert::IsTrue(ml == mres);
		}

		TEST_METHOD(TestAddSelfAssignMax)
		{
			llint l[COUNTLL];
			llint res[COUNTLL] = {};
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				res[i] = ~0ull;
			}
			res[0] -= 1ull;
			ModNumber ml(l);
			ModNumber mres(res);
			ml += ml;
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestAddOneToMax)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			llint exp[COUNTLL] = {};
			ModNumber ml(l);
			ModNumber mr(1ull);
			ModNumber mexp(exp);
			ModNumber mres = ml + mr;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestAddAssignFirstWordMaxToMax)
		{
			llint l[COUNTLL];
			llint r[COUNTLL] = {};
			r[0] = ~0ull;
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
			}
			exp[0] = ~0ull - 1ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ml += mr;
			Assert::IsTrue(mexp == ml);
		}
		TEST_METHOD(TestAddFirstWordMaxToMax)
		{
			llint l[COUNTLL];
			llint r[COUNTLL] = {};
			r[0] = ~0ull;
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
			}
			exp[0] = ~0ull - 1ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres;
			mres = ml + mr;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestAddAssignMaxToMax)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				r[i] = ~0ull;
				exp[i] = ~0ull;
			}
			exp[0] -= 1ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ml += mr;
			Assert::IsTrue(mexp == ml);
		}
		TEST_METHOD(TestAddMaxToMax)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				r[i] = ~0ull;
				exp[i] = ~0ull;
			}
			exp[0] -= 1ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres;
			mres = ml + mr;
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestMultiplyAssignByZero)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			ModNumber ml(l);
			ModNumber mexp;
			ml *= 0ul;
			Assert::IsTrue(ml == mexp);
		}
		TEST_METHOD(TestMultiplyByZeroScalar)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			ModNumber ml(l);
			ModNumber morig(ml);
			ModNumber mexp;
			ModNumber mres = ml * 0ul;
			Assert::IsTrue(mres == mexp);
			Assert::IsTrue(morig == ml);
		}

		TEST_METHOD(TestMultiplyAssignByOneScalar)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			ModNumber ml(l);
			ModNumber mexp(l);
			ml *= 1ul;
			Assert::IsTrue(ml == mexp);
		}
		TEST_METHOD(TestMultiplyByOneScalar)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			ModNumber ml(l);
			ModNumber mexp(l);
			ModNumber mres = ml * 1ul;
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyAssignByTwoScalar)
		{
			llint l[COUNTLL];
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = i;
				exp[i] = i * 2;
			}
			ModNumber ml(l);
			ModNumber mexp(exp);
			ml *= 2ul;
			Assert::IsTrue(ml == mexp);
		}
		TEST_METHOD(TestMultiplyByTwoScalar)
		{
			llint l[COUNTLL];
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = i;
				exp[i] = i * 2;
			}
			ModNumber ml(l);
			ModNumber mexp(exp);
			ModNumber mres = ml * 2ul;
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestMultiplyAssignAllFFFFByTwoScalar)
		{
			llint l[COUNTLL];
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				exp[i] = ~0ull;
			}
			ModNumber ml(l);
			exp[0] ^= 1ull;
			ModNumber mexp(exp);
			ml *= 2ul;
			Assert::IsTrue(ml == mexp);
		}
		TEST_METHOD(TestMultiplyAllFFFFByTwoScalar)
		{
			llint l[COUNTLL];
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				exp[i] = ~0ull;
			}
			ModNumber ml(l);
			exp[0] ^= 1ull;
			ModNumber mexp(exp);
			ModNumber mres = ml * 2ul;
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestMultiplyAssignFsBy2Pow16Scalar)
		{
			llint l[COUNTLL] = {};
			l[0] = ~0ull;
			llint exp[COUNTLL] = {};
			exp[1] = ~0ull >> (LLSIZE * 8 - 16);
			exp[0] = ~0ull << 16;
			ModNumber ml(l);
			ModNumber mexp(exp);
			ml *= 65536ul;
			Assert::IsTrue(mexp == ml);
		}

		TEST_METHOD(TestMultiplyFsBy2Pow16Scalar)
		{
			llint l[COUNTLL] = {};
			l[0] = ~0ull;
			llint exp[COUNTLL] = {};
			exp[1] = ~0ull >> (LLSIZE*8 - 16);
			exp[0] = ~0ull << 16;
			ModNumber ml(l);
			ModNumber mexp(exp);
			ModNumber mres = ml * 65536ul;
			Assert::IsTrue(mexp == mres);
		}


		TEST_METHOD(TestMultiplyAssignAllAsBy2Scalar)
		{
			llint tmp1 = 0xaaaaaaaaaaaaaaaaull;
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = tmp1;
			ModNumber ml(l);
			tmp1 <<= 1;
			llint tmp2 = tmp1 + 1;
			llint exp[COUNTLL];
			for (int i = 1; i < COUNTLL; i++)
				exp[i] = tmp2;
			exp[0] = tmp1;
			ModNumber mexp(exp);
			ml *= 2ull;
			Assert::IsTrue(mexp == ml);
		}
		TEST_METHOD(TestMultiplyAllAsBy2Scalar)
		{
			llint tmp1 = 0xaaaaaaaaaaaaaaaaull;
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = tmp1;
			ModNumber ml(l);
			tmp1 <<= 1;
			llint tmp2 = tmp1 + 1;
			llint exp[COUNTLL];
			for (int i = 1; i < COUNTLL; i++)
				exp[i] = tmp2;
			exp[0] = tmp1;
			ModNumber mexp(exp);
			ModNumber mres = ml * 2ull;
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestMultiplyAssignMod9sDecBy9sDecScalar)
		{
			llint l[COUNTLL] = {};
			l[0] = 99999999ull;
			lint r = 99999999;

			ModNumber ml(l);
			std::string exp = "9999999800000001";
			ModNumber mexp = ModNumber::stomn(exp);
			ml *= r;
			Assert::IsTrue(ml == mexp);
		}

		TEST_METHOD(TestMultiplyMod9sDecBy9sDecScalar)
		{
			llint l[COUNTLL] = {};
			l[0] = 99999999ull;
			lint r = 99999999;

			ModNumber ml(l);
			std::string exp = "9999999800000001";
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = ml * r;
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestDivisionByZero)
		{
			ModNumber ml;
			Assert::ExpectException<std::domain_error>([ml]() {ModNumber res = ml / 0ul; });
		}
		TEST_METHOD(TestDivisionZeroByOne)
		{
			ModNumber ml;
			ModNumber mres = ml / 1ul;
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestDivisionNonZeroByOne)
		{
			ModNumber ml(123456ul);
			ModNumber mres = ml / 1ul;
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestDivisionNonZeroByTwo)
		{
			ModNumber ml(24690ul);
			ModNumber exp(12345ul);
			ModNumber mres = ml / 2ul;
			Assert::IsTrue(exp == mres);
		}
		TEST_METHOD(TestDivisionAllNinesByThree)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = 9999999999999999999ull;
			}
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				exp[i] = 3333333333333333333ull;
			}
			ModNumber ml(l);
			ModNumber mexp(exp);
			ModNumber mres = ml / 3ul;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestDivisionAllNinesByTwoAndMultipliedByTwo)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = 9999999999999999999ull;
			}
			llint exp[COUNTLL];
			exp[0] = 9999999999999999998ull;
			for (int i = 1; i < COUNTLL; i++)
			{
				exp[i] = 9999999999999999999ull;
			}
			ModNumber ml(l);
			ModNumber mexp(exp);
			ModNumber mres1 = ml / 2ul;
			ModNumber mres2 = mres1 * 2ul;
			Assert::IsTrue(mexp == mres2);
		}
		TEST_METHOD(TestDivisionMaxLintTimesTwoByTwo)
		{
			llint l[COUNTLL];
			l[0] = ~0ull;
			l[1] = 1ull;
			for (int i = 2; i < COUNTLL; i++)
			{
				l[i] = 0ull;
			}
			llint exp[COUNTLL];
			exp[0] = ~0ull;
			for (int i = 1; i < COUNTLL; i++)
			{
				exp[i] = 0ull;
			}
			ModNumber ml(l);
			ModNumber mexp(exp);
			ModNumber mres = ml / 2ul;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestDivisionAllAsByTwo)
		{
			llint tmp = 0xaaaaaaaaaaaaaaaaull;
			llint ll[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				ll[i] = tmp;
			tmp >>= 1;
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				exp[i] = tmp;
			ModNumber ml(ll);
			ModNumber mexp(exp);
			ModNumber mres = ml / 2ull;
			Assert::IsTrue(mexp == mres);

		}
		TEST_METHOD(TestToStringIllegalBase)
		{
			ModNumber ml;
			Assert::ExpectException<std::invalid_argument>([ml]() {ml.to_string(11); });
		}
		TEST_METHOD(TestToStringOctalForZero)
		{
			ModNumber ml;
			std::string res = ml.to_string(8);
			std::string exp;
			exp.reserve(OctalStringLength);
			for (int i = 0; i < OctalStringLength; i++)
				exp.append("0");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringOctalForOne)
		{
			ModNumber ml(1);
			std::string res = ml.to_string(8);
			std::string exp;
			exp.reserve(OctalStringLength);
			for (int i = 0; i < OctalStringLength - 1; i++)
				exp.append("0");
			exp.append("1");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringOctalForEight)
		{
			ModNumber ml(8);
			std::string res = ml.to_string(8);
			std::string exp;
			exp.reserve(OctalStringLength);
			exp.assign(OctalStringLength - 2, '0');
			exp.append("10");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringOctalForMax)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0;
			ModNumber ml(l);
			std::string res = ml.to_string(8);
			std::string exp;
			exp.reserve(OctalStringLength);
			exp.assign(OctalStringLength, '7');
			exp[0] = ((3 - LLSIZE * 8 % 3) << 1) - 1 + '0';
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringOctalForMaxesAndZeros)
		{
			llint l[COUNTLL];
			for (int i = 0; i + 6 < COUNTLL; i += 6)
			{
				for (int j = 0; j < 3; j++)
					l[i + j] = ~0;
				for (int j = 3; j < 6; j++)
					l[i + j] = 0;
			}
			for (int i = COUNTLL - (COUNTLL % 6); i < COUNTLL; i++)
				l[i] = 0;
			ModNumber ml(l);
			std::string res = ml.to_string(8);
			std::string exp;
			exp.reserve(OctalStringLength);
			exp.assign(OctalStringLength % 128, '0');
			int startvalue = OctalStringLength % 128;
			for (int i = startvalue; i < OctalStringLength; i += 128)
			{
				exp.append(64, (((OctalStringLength % 2)+ 7) % 8) * 7 + '0');
				exp.append(64, (OctalStringLength % 2) * 7 + '0');
			}
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringHexForZero)
		{
			ModNumber ml;
			std::string res = ml.to_string(16);
			std::string exp;
			exp.reserve(HexStringLength);
			exp.assign(HexStringLength, '0');
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringHexForOne)
		{
			ModNumber ml(1);
			std::string res = ml.to_string(16);
			std::string exp;
			exp.reserve(HexStringLength);
			exp.assign(HexStringLength - 1, '0');
			exp.append("1");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringHexForSixteen)
		{
			ModNumber ml(16);
			std::string res = ml.to_string(16);
			std::string exp;
			exp.reserve(HexStringLength);
			exp.assign(HexStringLength - 2, '0');
			exp.append("10");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringHexForMax)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0;
			ModNumber ml(l);
			std::string res = ml.to_string(16);
			std::string exp;
			exp.reserve(HexStringLength);
			exp.assign(HexStringLength, 'F');
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringDecimalForZero)
		{
			ModNumber ml;
			std::string res = ml.to_string(10);
			std::string exp;
			exp.reserve(DecimalStringLength);
			exp.assign(DecimalStringLength, '0');
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringDecimalForOne)
		{
			ModNumber ml(1);
			std::string res = ml.to_string(10);
			std::string exp;
			exp.reserve(DecimalStringLength);
			exp.assign(DecimalStringLength - 1, '0');
			exp.append("1");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringDecimalForTen)
		{
			ModNumber ml(10);
			std::string res = ml.to_string(10);
			std::string exp;
			exp.reserve(DecimalStringLength);
			exp.assign(DecimalStringLength - 2, '0');
			exp.append("10");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringDecimalForMaxLongPlusOne)
		{
			llint l[COUNTLL];
			l[0] = 1ull << LSIZE * 8;
			for (int i = 1; i < COUNTLL; i++)
				l[i] = 0;
			ModNumber ml(l);
			std::string res = ml.to_string(10);
			std::string exp;
			exp.reserve(DecimalStringLength);
			exp.assign(DecimalStringLength - 10, '0');
			exp.append("4294967296");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringDecimalForMaxLong)
		{
			llint l[COUNTLL];
			l[0] = (lint)~0ul;
			for (int i = 1; i < COUNTLL; i++)
				l[i] = 0;
			ModNumber ml(l);
			std::string res = ml.to_string(10);
			std::string exp;
			exp.reserve(DecimalStringLength);
			exp.assign(DecimalStringLength - 10, '0');
			exp.append("4294967295");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringDecimalForMax)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = (llint)~0ull;
			ModNumber ml(l);
			std::string res = ml.to_string(10);
			ModNumber exp = ModNumber::stomn(res, 10);
			Assert::IsTrue(ml == exp);
		}
		TEST_METHOD(TestToModularNumberIllegalBase)
		{
			std::string s;
			Assert::ExpectException<std::invalid_argument>([s]() {ModNumber::stomn(s, 11); });
		}
		TEST_METHOD(TestToModularNumberHexIllegalChar)
		{
			std::string s("123456789ABCDEFG");
			Assert::ExpectException<std::invalid_argument>([s]() {ModNumber::stomn(s, 16); });
		}

		TEST_METHOD(TestToModularNumberHexForEmptyString)
		{
			ModNumber mexp;
			std::string s;
			Assert::IsTrue(mexp == ModNumber::stomn(s, 16));
		}
		TEST_METHOD(TestToModularNumberHexForOne)
		{
			ModNumber mexp(1);
			std::string s = "1";
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberHexForSixteen)
		{
			ModNumber mexp(16);
			std::string s = "10";
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberHexForSixteenWithLeadingZeros)
		{
			ModNumber mexp(16);
			std::string s = "0000000000000000000010";
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberHexForSixteenWithLeadingMinusSign)
		{
			std::string s = "-10";
			Assert::ExpectException<std::domain_error>([s]() {ModNumber::stomn(s, 16); });
		}
		TEST_METHOD(TestToModularNumberHexForSixteenWithLeadingPlusSign)
		{
			ModNumber mexp(16);
			std::string s = "+10";
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberHexForMAX)
		{
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				exp[i] = ~0ull;
			ModNumber mexp(exp);
			std::string s;
			s.reserve(HexStringLength);
			s.assign(HexStringLength, 'F');
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberHexIncreasingSequenceByteInput)
		{
			unsigned char cexp[NCOUNT];
			for (int i = 0; i < NCOUNT; i++)
			{
				cexp[i] = (unsigned char)i % 16;
			}
			llint* exp = (llint*)cexp;
			ModNumber mexp(exp);
			std::string s;
			s.reserve(HexStringLength);
			std::stringstream tmpstr;

			for (int i = 0; i * 2 < HexStringLength % 32; i++)
			{
				tmpstr.setf(std::ios_base::right | std::ios_base::uppercase);
				tmpstr.fill('0');
				tmpstr.width(2);
				tmpstr << std::hex << ((HexStringLength % 32)/2 - i - 1);
			}
			s.append(tmpstr.str());
			for (int i = HexStringLength % 32; i < HexStringLength; i += 32)
				s.append("0F0E0D0C0B0A09080706050403020100");
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberHexIncreasingSequence)
		{
			llint exp[COUNTLL] = {};
			for (int i = 0; i + 1 < COUNTLL; i += 2)
			{
				exp[i] = 0x0706050403020100ull;
				exp[i + 1] = 0x0F0E0D0C0B0A0908ull;
			}
			ModNumber mexp(exp);
			std::string s;
			s.reserve(HexStringLength);
			for (int i = 0; i < HexStringLength % 32; i++)
				s.append("0");
			for (int i = HexStringLength % 32; i < HexStringLength; i += 32)
				s.append("0F0E0D0C0B0A09080706050403020100");
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberHexIncreasingSequenceSwitched)
		{
			llint exp[COUNTLL] = {};
			for (int i = 0; i + 1 < COUNTLL; i += 2)
			{
				exp[i] = 0x08090A0B0C0D0E0Full;
				exp[i + 1] = 0x0001020304050607ull;
			}
			ModNumber mexp(exp);
			std::string s;
			s.reserve(HexStringLength);
			for (int i = 0; i < HexStringLength % 32; i++)
				s.append("0");
			for (int i = HexStringLength % 32; i < HexStringLength; i += 32)
				s.append("000102030405060708090A0B0C0D0E0F");
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberDecimalForEmptyString)
		{
			ModNumber mexp;
			std::string s;
			Assert::IsTrue(mexp == ModNumber::stomn(s, 10));
		}
		TEST_METHOD(TestToModularNumberDecimalIllegalChar)
		{
			std::string s("123456789A");
			Assert::ExpectException<std::invalid_argument>([s]() {ModNumber::stomn(s, 10); });
		}
		TEST_METHOD(TestToModularNumberDecimalWithLeadingSpaces)
		{
			ModNumber mexp(9);
			std::string s("     9");
			ModNumber mres = ModNumber::stomn(s, 10);
			Assert::IsTrue(mexp == mres);

		}

		TEST_METHOD(TestToModularNumberDecimalForOne)
		{
			ModNumber mexp(1);
			std::string s = "1";
			ModNumber mres = ModNumber::stomn(s, 10);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberDecimalForTen)
		{
			ModNumber mexp(10);
			std::string s = "10";
			ModNumber mres = ModNumber::stomn(s, 10);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberDecimalForTenWithLeadingZeros)
		{
			ModNumber mexp(10);
			std::string s = "0000000000000000000010";
			ModNumber mres = ModNumber::stomn(s, 10);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberDecimalForTenWithLeadingMinusSign)
		{
			std::string s = "-10";
			Assert::ExpectException<std::domain_error>([s]() {ModNumber::stomn(s, 10); });
		}
		TEST_METHOD(TestToModularNumberHexForTenWithLeadingPlusSign)
		{
			ModNumber mexp(10);
			std::string s = "+10";
			ModNumber mres = ModNumber::stomn(s, 10);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberDecimalForTenNines)
		{
			ModNumber mexp(0x2540BE3FF);
			std::string s;
			s.reserve(DecimalStringLength);
			s.assign(DecimalStringLength - 10, '0');
			s.append(10, '9');
			ModNumber mres = ModNumber::stomn(s, 10);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberDecimalForEighteenNines)
		{
			ModNumber mexp(0xDE0B6B3A763FFFF);
			std::string s;
			s.reserve(DecimalStringLength);
			s.assign(DecimalStringLength - 18, '0');
			s.append(18, '9');
			ModNumber mres = ModNumber::stomn(s, 10);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberOctalForEmptyString)
		{
			ModNumber mexp;
			std::string s;
			Assert::IsTrue(mexp == ModNumber::stomn(s, 8));
		}
		TEST_METHOD(TestToModularNumberOctalIllegalChar)
		{
			std::string s("123456789A");
			Assert::ExpectException<std::invalid_argument>([s]() {ModNumber::stomn(s, 8); });
		}
		TEST_METHOD(TestToModularNumberOctalForOne)
		{
			ModNumber mexp(1);
			std::string s = "1";
			ModNumber mres = ModNumber::stomn(s, 8);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberOctalForSixteen)
		{
			ModNumber mexp(16);
			std::string s = "20";
			ModNumber mres = ModNumber::stomn(s, 8);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberOctalForEightWithLeadingZeros)
		{
			ModNumber mexp(8);
			std::string s = "0000000000000000000010";
			ModNumber mres = ModNumber::stomn(s, 8);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberOctalForMAX)
		{
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				exp[i] = ~0ull;
			ModNumber mexp(exp);
			std::string s;
			s.reserve(OctalStringLength);
			s.assign(OctalStringLength, '7');
			s[0] = '3';
			ModNumber mres = ModNumber::stomn(s, 8);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberOctalStringTooLarge)
		{
			std::string s;
			s.reserve(OctalStringLength);
			for (int i = 0; i < OctalStringLength; i += 16)
				s.append("0706050403020100");
			Assert::ExpectException<std::domain_error>([s]() {ModNumber::stomn(s, 8); });

		}
		TEST_METHOD(TestToModularNumberOctalIncreasingSequenceByteInput)
		{
			std::string s;
			s.reserve(OctalStringLength);
			s.assign(OctalStringLength % 16,'0');
			for (int i = (OctalStringLength % 16); i < OctalStringLength; i += 16)
				s.append("0001020304050607");
			ModNumber mres = ModNumber::stomn(s, 8);
			std::string exp = mres.to_string(8);
			Assert::IsTrue(exp == s);
		}
		TEST_METHOD(TestSerializationHexForZero)
		{
			ModNumber mempty;
			ModNumber mexp;
			std::string fname("TestSerializationHexForZero.txt");
			std::ofstream outf;
			outf.open(fname,std::ios::out);
			outf << std::hex << mempty;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::hex >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mempty == mexp);
		}
		TEST_METHOD(TestSerializationHexForOne)
		{
			ModNumber mone(1);
			ModNumber mexp;
			std::string fname("TestSerializationHexForOne.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::hex << mone;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::hex >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mone);
		}
		TEST_METHOD(TestSerializationHexForSixteen)
		{
			ModNumber mone(16);
			ModNumber mexp;
			std::string fname("TestSerializationHexForSixteen.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::hex << mone;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::hex >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mone);
		}
		TEST_METHOD(TestSerializationHexForAllFFFF)
		{
			llint allff[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				allff[i] = ~0ull;
			ModNumber mAllff(allff);
			ModNumber mexp;
			std::string fname("TestSerializationHexForAllFF.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::hex << mAllff;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::hex >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mAllff);
		}
		TEST_METHOD(TestSerializationHexForIncreasingSequence)
		{
			char n[NCOUNT];
			for (int i = 0; i < NCOUNT; i++)
				n[i] = i;
			ModNumber mn((llint*)n);
			ModNumber mexp;
			std::string fname("TestSerializationHexForIncreasingSequence.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::hex << mn;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::hex >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mn);
		}
		TEST_METHOD(TestSerializationOctForZero)
		{
			ModNumber mempty;
			ModNumber mexp;
			std::string fname("TestSerializationOctForZero.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::oct << mempty;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::oct >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mempty == mexp);
		}
		TEST_METHOD(TestSerializationOctForOne)
		{
			ModNumber mone(1);
			ModNumber mres;
			std::string fname("TestSerializationOctForOne.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::oct << mone;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::oct >> mres;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mres == mone);
		}
		TEST_METHOD(TestSerializationOctForSixteen)
		{
			ModNumber mone(16);
			ModNumber mexp;
			std::string fname("TestSerializationOctForSixteen.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::oct << mone;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::oct >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mone);
		}
		TEST_METHOD(TestSerializationOctForAllFFFF)
		{
			llint allff[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				allff[i] = ~0ull;
			ModNumber mAllff(allff);
			ModNumber mexp;
			std::string fname("TestSerializationOctForAllFF.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::oct << mAllff;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::oct >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mAllff);
		}
		TEST_METHOD(TestSerializationOctForIncreasingSequence)
		{
			char n[NCOUNT];
			for (int i = 0; i < NCOUNT; i++)
				n[i] = i;
			ModNumber mn((llint*)n);
			ModNumber mexp;
			std::string fname("TestSerializationOctForIncreasingSequence.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::oct << mn;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::oct >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mn);
		}
		TEST_METHOD(TestSerializationDecForZero)
		{
			ModNumber mempty;
			ModNumber mexp;
			std::string fname("TestSerializationDecForZero.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::dec << mempty;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::dec >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mempty == mexp);
		}
		TEST_METHOD(TestSerializationDecForOne)
		{
			ModNumber mone(1);
			ModNumber mexp;
			std::string fname("TestSerializationDecForOne.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::dec << mone;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::dec >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mone);
		}
		TEST_METHOD(TestSerializationDecForTen)
		{
			ModNumber mone(10);
			ModNumber mexp;
			std::string fname("TestSerializationDecForTen.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::dec << mone;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::dec >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mone);
		}
		TEST_METHOD(TestSerializationDecForAllFFFF)
		{
			llint allff[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				allff[i] = ~0ull;
			ModNumber mAllff(allff);
			ModNumber mexp;
			std::string fname("TestSerializationDecForAllFF.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::dec << mAllff;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::dec >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mAllff);
		}
		TEST_METHOD(TestSerializationDecForIncreasingSequence)
		{
			char n[NCOUNT];
			for (int i = 0; i < NCOUNT; i++)
				n[i] = i;
			ModNumber mn((llint*)n);
			ModNumber mexp;
			std::string fname("TestSerializationDecForIncreasingSequence.txt");
			std::ofstream outf;
			outf.open(fname, std::ios::out);
			outf << std::dec << mn;
			outf.close();
			std::ifstream inf;
			inf.open(fname, std::ios::in);
			inf >> std::dec >> mexp;
			char c;
			inf >> c;
			Assert::IsTrue(inf.eof());
			inf.close();
			Assert::IsTrue(mexp == mn);
		}

		TEST_METHOD(TestMultiplyByZero)
		{
			llint l[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
			}

			ModNumber ml(l);
			ModNumber mr;
			ModNumber mexp;
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyByOne)
		{
			llint l[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
			}

			ModNumber ml(l);
			ModNumber mr(1ull);
			ModNumber mexp(ml);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyByTwo)
		{
			llint l[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
				exp[i] = i * 2;
			}

			ModNumber ml(l);
			ModNumber mr(2ull);
			ModNumber mexp(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestMultiplyAllFFFFByTwo)
		{
			llint l[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD - 1; i++)
			{
				l[i] = ~0ull;
				exp[i] = ~0ull;
			}
			exp[COUNTMOD - 1] = 1ull;
			exp[0] -= 1ull;

			ModNumber ml(l);
			ModNumber mr(2ull);
			ModNumber mexp(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestMultiplyFsByPow16)
		{
			llint l[COUNTLL] = {};
			l[0] = ~0ull;
			llint exp[COUNTLL] = {};
			exp[1] = ~0ull >> (LLSIZE * 8 - 16);
			exp[0] = ~0ull << 16;

			ModNumber ml(l);
			ModNumber mr(65536ull);
			ModNumber mexp(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyAllFsByAllFs)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				r[i] = ~0ull;
			}
			llint exp[COUNTLL] = {};
			exp[0] = 1ull;

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}



		TEST_METHOD(TestMultiply8sBy2)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = 0x8888888888888888ull;
			r[0] = 2;
			llint exp[COUNTLL] = {};
			exp[0] = 0x1111111111111110ull;
			exp[1] = 0x1ull;

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiply9sDecBy9sDec)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = 9999999999999999ull;
			r[0] = l[0];

			ModNumber ml(l);
			ModNumber mr(r);
			std::string exp = "99999999999999980000000000000001";
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyTwoBlock9sDecByTwoBlock9sDec)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);

			std::string exp = "9999999999999999";
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestMultiplyFourBlock9sDecByFourBlock9sDec)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);

			std::string exp = "9999999999999999";
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestMultiplyEightBlock9sDecByEightBlock9sDec)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);

			std::string exp = "9999999999999999";
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}



		TEST_METHOD(TestMultiplySixteenBlock9sDecBySixteenBlock9sDec)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);

			std::string exp = "";
			for (int i = 0; i < 15; i++)
				exp.append("9999999999999999");

			exp.append("9999999999999998");
			for (int i = 0; i < 15; i++)
				exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}



		TEST_METHOD(TestMultiplyThirtyOneBlock9sDecByThirtyOneBlock9sDec)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr = lstr.substr(0, 496);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);

			std::string exp = "";
			for (int i = 0; i < 30; i++)
				exp.append("9999999999999999");

			exp.append("9999999999999998");
			for (int i = 0; i < 30; i++)
				exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestMultiplyThirtyTwoBlock9sDecByThirtyTwoBlock9sDec)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);

			std::string exp = "";
			for (int i = 0; i < 31; i++)
				exp.append("9999999999999999");

			exp.append("9999999999999998");
			for (int i = 0; i < 31; i++)
				exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = ml * mr;
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultGroupModAboveMAX)
		{
			llint n[COUNTLL] = {};
			n[COUNTMOD] = 1ull;
			ModNumber mn(n);
			Assert::ExpectException<std::domain_error>([mn] {MultGroupMod g(mn); });
		}

		TEST_METHOD(TestMultiplyMultGroupModByZero)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
				n[i] = i;
			}
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr;
			ModNumber mexp;
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModByOne)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
				n[i] = i;
			}
			n[0] += 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(1ull);
			ModNumber mexp(ml);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModByTwoResultEqMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
				n[i] = i * 2;
			}
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(2ull);
			ModNumber mexp;
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModByTwoResultLessMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
				n[i] = i * 2;
				exp[i] = i * 2;
			}
			n[0] += 1ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(2ull);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModAllFFFFByTwoResultEqMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = ~0ull;
				n[i] = ~0ull;
			}
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(2ull);
			ModNumber mexp;
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}
		TEST_METHOD(TestMultiplyMultGroupModAllFFFFByTwoResultLessMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD - 1; i++)
			{
				l[i] = ~0ull;
				n[i] = ~0ull;
				exp[i] = ~0ull;
			}
			n[COUNTMOD - 1] = 1ull;
			exp[COUNTMOD - 1] = 1ull;
			exp[0] -= 1ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(2ull);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestMultiplyMultGroupModFsByPow16ResultLessMod)
		{
			llint l[COUNTLL] = {};
			l[0] = ~0ull;
			llint n[COUNTLL] = {};
			n[2] = 1ull;
			llint exp[COUNTLL] = {};
			exp[1] = ~0ull >> (LLSIZE * 8 - 16);
			exp[0] = ~0ull << 16;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(65536ull);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModFsByFsResultModOne3rdBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = ~0ull;
			r[0] = ~0ull;
			llint n[COUNTLL] = {};
			n[2] = 1ull;
			llint exp[COUNTLL] = {};
			exp[1] = ~0ull - 1ull;
			exp[0] = 1ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModFsByFsResultModOne2ndBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = ~0ull;
			r[0] = ~0ull;
			llint n[COUNTLL] = {};
			n[1] = 1ull;
			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModFsByFsResultModEs1ThBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = ~0ull;
			r[0] = ~0ull;
			llint n[COUNTLL] = {};
			n[0] = 0xEEEEEEEEEEEEEEEEull;
			llint exp[COUNTLL] = {};
			exp[0] = 0x1111111111111111ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModAllFsByAllFsResultLessMod)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			for (int i = 0; i < COUNTLL - 3; i++)
			{
				l[i] = ~0ull;
				r[i] = ~0ull;
			}
			llint n[COUNTLL] = {};
			n[COUNTLL - 3] = 0x1ull;
			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModAllFsByAllFsResultGreaterMod)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			for (int i = 0; i < COUNTLL - 3; i++)
			{
				l[i] = ~0ull;
				r[i] = ~0ull;
			}
			llint n[COUNTLL] = {};
			n[1] = 0x1ull;
			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestMultiplyMultGroupMod8sBy2ResultLessMod)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = 0x8888888888888888ull;
			r[0] = 2;
			llint n[COUNTLL] = {};
			n[2] = 1ull;
			llint exp[COUNTLL] = {};
			exp[0] = 0x1111111111111110ull;
			exp[1] = 0x1ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupMod8HexsBy2ResultGreaterMod)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = 0x8888888888888888ull;
			r[0] = 2;
			llint n[COUNTLL] = {};
			n[1] = 1ull;
			llint exp[COUNTLL] = {};
			exp[0] = 0x1111111111111110ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupMod9sDecBy9sDecResultLessMod)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			l[0] = 9999999999999999ull;
			r[0] = l[0];
			llint n[COUNTLL] = {};
			n[4] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mr(r);
			std::string exp = "99999999999999980000000000000001";
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModTwoBlock9sDecByTwoBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			llint n[COUNTLL] = {};
			n[4] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "9999999999999999";
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModTwoBlock9sDecByTwoBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModFourBlock9sDecByFourBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			llint n[COUNTLL] = {};
			n[8] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "9999999999999999";
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModFourBlock9sDecByFourBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModEightBlock9sDecByEightBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			llint n[COUNTLL] = {};
			n[16] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "9999999999999999";
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModEightBlock9sDecByEightBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModSixteenBlock9sDecBySixteenBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			llint n[COUNTLL] = {};
			n[32] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "";
			for (int i = 0; i < 15; i++)
				exp.append("9999999999999999");

			exp.append("9999999999999998");
			for (int i = 0; i < 15; i++)
				exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModSixteenBlock9sDecBySixteenBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModThirtyOneBlock9sDecByThirtyOneBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr = lstr.substr(0, 496);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			llint n[COUNTLL] = {};
			n[62] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "";
			for (int i = 0; i < 30; i++)
				exp.append("9999999999999999");

			exp.append("9999999999999998");
			for (int i = 0; i < 30; i++)
				exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModThirtyOneBlock9sDecByThirtyOneBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr = lstr.substr(0, 496);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestMultiplyMultGroupModThirtyTwoBlock9sDecByThirtyTwoBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			std::string rstr = lstr;
			ModNumber ml = ModNumber::stomn(lstr);
			ModNumber mr = ModNumber::stomn(rstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Mult(ml, mr);
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestGCDOfOneAndZero)
		{
			ModNumber mzero;
			ModNumber mone(1ull);
			ModNumber mres;
			Assert::ExpectException<std::domain_error>([&mres, mzero, mone] {mres = ModNumber::gcd(mone, mzero); });
		}

		TEST_METHOD(TestGCDOfZeroAndOne)
		{
			ModNumber mzero;
			ModNumber mone(1ull);
			ModNumber mres;
			Assert::ExpectException<std::domain_error>([&mres, mzero, mone] {mres = ModNumber::gcd(mzero, mone); });
		}

		TEST_METHOD(TestGCDOf100and1)
		{
			ModNumber mone(1ull);
			ModNumber monehundred(100ull);
			ModNumber exp = mone;
			ModNumber mres = ModNumber::gcd(monehundred, mone);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestGCDOf1and100)
		{
			ModNumber mone(1ull);
			ModNumber monehundred(100ull);
			ModNumber exp = mone;
			ModNumber mres = ModNumber::gcd(mone,monehundred);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestGCDOf101and100)
		{
			ModNumber monehundredone(101ull);
			ModNumber monehundred(100ull);
			ModNumber exp(1ull);
			ModNumber mres = ModNumber::gcd(monehundredone, monehundred);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestGCDOf102and100)
		{
			ModNumber monehundredtwo(102ull);
			ModNumber monehundred(100ull);
			ModNumber exp(2ull);
			ModNumber mres = ModNumber::gcd(monehundredtwo, monehundred);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestGCDOf100and102)
		{
			ModNumber monehundredtwo(102ull);
			ModNumber monehundred(100ull);
			ModNumber exp(2ull);
			ModNumber mres = ModNumber::gcd(monehundred, monehundredtwo);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestGCDOf400and600)
		{
			ModNumber mfourhundred(400ull);
			ModNumber msixhundred(600ull);
			ModNumber exp(200ull);
			ModNumber mres = ModNumber::gcd(mfourhundred, msixhundred);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestGCDOfAllFin2ndBlockAndAllFin1thBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[1] = ~0ull;
			r[0] = ~0ull;
			exp[0] = ~0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = ModNumber::gcd(ml, mr);
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestGCDOfAllFinlastBlockAndAllFin2ndBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[COUNTLL-1] = ~0ull;
			r[1] = ~0ull;
			exp[1] = ~0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = ModNumber::gcd(ml, mr);
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestGCDOfAllFinlastBlockAndAllAin3rdBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[COUNTLL - 1] = ~0ull;
			r[2] = 0xaaaaaaaaaaaaaaaaull;
			exp[2] = r[2];
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = ModNumber::gcd(ml, mr);
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestGCDOfAllFinlastBlockAndAllBin3rdBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[COUNTLL - 1] = ~0ull;
			r[2] = 0xbbbbbbbbbbbbbbbbull;
			exp[2] = 0x1111111111111111ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = ModNumber::gcd(ml, mr);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestLCMOf101and100)
		{
			ModNumber monehundredone(101ull);
			ModNumber monehundred(100ull);
			ModNumber exp(10100ull);
			ModNumber mres = ModNumber::lcm(monehundredone, monehundred);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestLCMOf102and100)
		{
			ModNumber monehundredtwo(102ull);
			ModNumber monehundred(100ull);
			ModNumber exp(5100ull);
			ModNumber mres = ModNumber::lcm(monehundredtwo, monehundred);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestLCMOf100and102)
		{
			ModNumber monehundredtwo(102ull);
			ModNumber monehundred(100ull);
			ModNumber exp(5100ull);
			ModNumber mres = ModNumber::lcm(monehundred, monehundredtwo);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestLCMOf400and600)
		{
			ModNumber mfourhundred(400ull);
			ModNumber msixhundred(600ull);
			ModNumber exp(1200ull);
			ModNumber mres = ModNumber::lcm(mfourhundred, msixhundred);
			Assert::IsTrue(exp == mres);
		}

		TEST_METHOD(TestLCMOfAllFin2ndBlockAndAllFin1thBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[1] = ~0ull;
			r[0] = ~0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(l);
			ModNumber mres = ModNumber::lcm(ml, mr);
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestLCMOfAllFinlastBlockAndAllFin2ndBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[COUNTLL - 1] = ~0ull;
			r[1] = ~0ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(l);
			ModNumber mres = ModNumber::lcm(ml, mr);
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestLCMOfAllFinlastBlockAndAllAin3rdBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[COUNTLL - 1] = ~0ull;
			r[2] = 0xaaaaaaaaaaaaaaaaull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(l);
			ModNumber mres = ModNumber::lcm(ml, mr);
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestLCMOfAllFinlastBlockAndAllBin3rdBlock)
		{
			llint l[COUNTLL] = {};
			llint r[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[COUNTLL - 1] = ~0ull;
			r[2] = 0xbbbbbbbbbbbbbbbbull;
			exp[COUNTLL - 1] = 0xfffffffffffffff5ull;
			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber mres = ModNumber::lcm(ml, mr);
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestMultGroupModOfZero)
		{
			ModNumber mzero;
			
			ModNumber mres;
			Assert::ExpectException<std::domain_error>([mzero] {MultGroupMod mgm(mzero); });
		}
		TEST_METHOD(TestMultGroupModOfOne)
		{
			ModNumber mone(1ull);

			ModNumber mres;
			Assert::ExpectException<std::domain_error>([mone] {MultGroupMod mgm(mone); });
		}

		TEST_METHOD(TestKwadMultGroupEqualMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
				n[i] = i;
			}
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mexp;
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadOneMultGroupLessMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			l[0] = 1ull;
			n[0] = 2ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber mexp(ml);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}
		TEST_METHOD(TestKwadMultGroupMod9sDecResultLessMod)
		{
			llint l[COUNTLL] = {};
			l[0] = 9999999999999999ull;
			llint n[COUNTLL] = {};
			n[4] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			std::string exp = "99999999999999980000000000000001";
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModTwoBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[4] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "9999999999999999";
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModTwoBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModFourBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[8] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "9999999999999999";
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModFourBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModEightBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[16] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "9999999999999999";
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModEightBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModSixteenBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[32] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "";
			for (int i = 0; i < 15; i++)
				exp.append("9999999999999999");

			exp.append("9999999999999998");
			for (int i = 0; i < 15; i++)
				exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModSixteenBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModThirtyOneBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr = lstr.substr(0, 496);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[62] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			std::string exp = "";
			for (int i = 0; i < 30; i++)
				exp.append("9999999999999999");

			exp.append("9999999999999998");
			for (int i = 0; i < 30; i++)
				exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModThirtyOneBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr = lstr.substr(0, 496);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestKwadMultGroupModThirtyTwoBlock9sResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Kwad(ml);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpZeroMultGroupAllOnesLessMod)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
			}
			ModNumber ml(l);
			ModNumber mn(1000);
			MultGroupMod mgm(mn);
			ModNumber me;
			ModNumber mexp(1ull);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpOneMultGroupEqualMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
				n[i] = i;
			}
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(1ull);
			ModNumber ml(l);
			ModNumber mexp;
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpOneMultGroupLessMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
				n[i] = i + 1;
			}
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(1ull);
			ModNumber ml(l);
			ModNumber mexp (ml);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}


		TEST_METHOD(TestExpTwoMultGroupEqualMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			for (int i = 0; i < COUNTMOD; i++)
			{
				l[i] = i;
				n[i] = i;
			}
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);
			ModNumber ml(l);
			ModNumber mexp;
			ModNumber mres = mgm.Exp(ml,me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoOneMultGroupLessMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			l[0] = 1ull;
			n[0] = 2ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber me(2ull);
			ModNumber mexp(ml);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTenOneMultGroupLessMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			l[0] = 1ull;
			n[0] = 2ull;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber me(10ull);
			ModNumber mexp(ml);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupMod9sDecResultLessMod)
		{
			llint l[COUNTLL] = {};
			l[0] = 9999999999999999ull;
			llint n[COUNTLL] = {};
			n[4] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);

			ModNumber ml(l);
			ModNumber me(2ull);
			std::string exp = "99999999999999980000000000000001";
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModTwoBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[4] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);
			std::string exp = "9999999999999999";
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Exp(ml,me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModTwoBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);
			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Exp(ml,me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModFourBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[8] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);
			std::string exp = "9999999999999999";
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModFourBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);
			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModEightBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[16] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);

			std::string exp = "9999999999999999";
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999999");
			exp.append("9999999999999998");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Exp(ml,me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModEightBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModSixteenBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[32] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);

			std::string exp = "";
			for (int i = 0; i < 15; i++)
				exp.append("9999999999999999");

			exp.append("9999999999999998");
			for (int i = 0; i < 15; i++)
				exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwooMultGroupModSixteenBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModThirtyOneBlock9sDecResultLessMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr = lstr.substr(0, 496);
			ModNumber ml = ModNumber::stomn(lstr);
			llint n[COUNTLL] = {};
			n[62] = 1;
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);

			std::string exp = "";
			for (int i = 0; i < 30; i++)
				exp.append("9999999999999999");

			exp.append("9999999999999998");
			for (int i = 0; i < 30; i++)
				exp.append("0000000000000000");
			exp.append("0000000000000001");
			ModNumber mexp = ModNumber::stomn(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModThirtyOneBlock9sDecResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr = lstr.substr(0, 496);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoMultGroupModThirtyTwoBlock9sResultGreaterMod)
		{
			std::string lstr = "9999999999999999";
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			lstr.append(lstr);
			ModNumber ml = ModNumber::stomn(lstr);
			std::string nstr = "10000000000000000";
			ModNumber mn = ModNumber::stomn(nstr);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);

			llint exp[COUNTLL] = {};
			exp[0] = 1ull;
			ModNumber mexp(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpMultGroupModTwoPowerThirteenModTenThousand)
		{
			ModNumber ml(2ull);
			ModNumber mn(10000ull);
			MultGroupMod mgm(mn);
			ModNumber me(13ull);

			ModNumber mexp(8192ull);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpMultGroupModFirstBlockAllOnesPowerTwoLessMod)
		{
			llint l[COUNTLL]={};
			llint n[COUNTLL]={};
			llint exp[COUNTLL]={};
			l[0] = ~0ull;
			n[2] = 1ull;
			exp[0] = 1ull;
			exp[1] = ~0ull - 1ull;
			ModNumber ml(l);
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);

			ModNumber mexp(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpMultGroupModFirstTwoBlocksAllOnesPowerTwoLessMod)
		{
			llint l[COUNTLL] = {};
			llint n[COUNTLL] = {};
			llint exp[COUNTLL] = {};
			l[0] = ~0ull;
			l[1] = ~0ull;
			n[4] = 1ull;
			exp[0] = 1ull;
			exp[1] = 0ull;
			exp[2] = ~0ull - 1ull;
			exp[3] = ~0ull;
			ModNumber ml(l);
			ModNumber mn(n);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);

			ModNumber mexp(exp);
			ModNumber mres = mgm.Exp(ml, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestGroupPropertiesGroupModEleven)
		{
			ModNumber mx(2ull);
			ModNumber mn(11ull);
			MultGroupMod mgm(mn);
			ModNumber me(2ull);
			ModNumber mexp(4ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mres == mexp);
			me += 1ull;
			mexp = ModNumber(8ull);
			mres = mgm.Exp(mx, me);
			Assert::IsTrue(mres == mexp);
			me += 1ull;
			mexp = ModNumber(5ull);
			mres = mgm.Exp(mx, me);
			Assert::IsTrue(mres == mexp);
			me += 1ull;
			mexp = ModNumber(10ull);
			mres = mgm.Exp(mx, me);
			Assert::IsTrue(mres == mexp);
			me += 1ull;
			mexp = ModNumber(9ull);
			mres = mgm.Exp(mx, me);
			Assert::IsTrue(mres == mexp);
			me += 1ull;
			mexp = ModNumber(7ull);
			mres = mgm.Exp(mx, me);
			Assert::IsTrue(mres == mexp);
			me += 1ull;
			mexp = ModNumber(3ull);
			mres = mgm.Exp(mx, me);
			Assert::IsTrue(mres == mexp);
			me += 1ull;
			mexp = ModNumber(6ull);
			mres = mgm.Exp(mx, me);
			Assert::IsTrue(mres == mexp);
			me += 1ull;
			mexp = ModNumber(1ull);
			mres = mgm.Exp(mx, me);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestGCDLargeNumbers)
		{
			ModNumber mx = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			ModNumber my = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mres = ModNumber::gcd(mx, my);
			ModNumber mexp(1ull);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestExpTwoLargeNumberxModLargeNumbery)
		{
			ModNumber mx = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			ModNumber my = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp = ModNumber::stomn("113638967017082606674052656070205979468957500859029166099119331283109255964689");
			MultGroupMod mgm(my);
			ModNumber me(2ull);
			ModNumber mres = mgm.Exp(mx,me);
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestExpThreeLargeNumberxModLargeNumbery)
		{
			ModNumber mx = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			ModNumber my = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp = ModNumber::stomn("959159918151361352804382650352706011784068412944254732668580895529266851090113");
			MultGroupMod mgm(my);
			ModNumber me(3ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpFourLargeNumberxModLargeNumbery)
		{
			ModNumber mx = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			ModNumber my = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp = ModNumber::stomn("43252875649600596472804069974401733886601470807478913480533005345660321341646");
			MultGroupMod mgm(my);
			ModNumber me(4ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpFiveLargeNumberxModLargeNumbery)
		{
			ModNumber mx = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			ModNumber my = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp = ModNumber::stomn("161704454599373732186620154854028099753067612674910409272189542689295694371582");
			MultGroupMod mgm(my);
			ModNumber me(5ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpNineLargeNumberxModLargeNumbery)
		{
			ModNumber mx = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			ModNumber my = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp = ModNumber::stomn("229491963118798811250703753130022542480533480148820031152039444666272832238347");
			MultGroupMod mgm(my);
			ModNumber me(9ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpFourteenLargeNumberxModLargeNumbery)
		{
			ModNumber mx = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			ModNumber my = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp = ModNumber::stomn("512694095576449334310364050646143389495558145452890366434851422723699646134904");
			MultGroupMod mgm(my);
			ModNumber me(14ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpTwentyThreeLargeNumberxModLargeNumbery)
		{
			ModNumber mx = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			ModNumber my = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp = ModNumber::stomn("762907244846308926487595589728323262016384923154412381938714858362885520672888");
			MultGroupMod mgm(my);
			ModNumber me(23ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}

		TEST_METHOD(TestHugeNumberxModHugeNumbery1)
		{
			ModNumber mx = ModNumber::stomn("67885277291854912938587378017853691459496193677756552480553436557302300320101312718354512366504982574838958861888542718272904603102504548837304584105562307267284676071603679712110072004987020967899883804718561430292652099594457858875289933946756413441096538132475400931050009417996185648350176662387902186108621517311830327723099115016623424851268094682681323829709292907275341890111203138351101069715278419043245098727421641629718536436838918204954343308555448232382368388580601057201273701188613232106352300442738562367465251389869660183071271610913707341230465789657707599097586310256539211176484567156528847506639917990006758261522292135816044123533114985744902968420423817686515256802404411459420510055567668821773480759839945638865270345325327032377340654619518821029267132265318694685281072806300858177007288594225758039156380947826545771278835135058430286184244368381139666029873832564755473361825936566690845345140947317051183032581586950151989519809420014114182249129767248172760729288655451472427647241152820746675436044819097064699101275497442767142366134874787841654717249301013092609819939109466282790957714894233169284428494458512543531352888077862617075549676041201530943138909340541678084859095964562913434445958610945");
			ModNumber my = ModNumber::stomn("134726165702296673370426386890283366245915523314703970967013679118347220188155182690330977225943173840726855105111482919931305956440257849275520470254003646345501461780481515668802531224809023461784856160970148295396834447238617115943625107572025345987403272485443712847152864396338847358873706819626414636709346223482144399011474564965089003187992678881573427552175300955270709084383966983460129498021945865772279351116095761583530346030439079292145399495672419313497076142325458060700017164710304404461962683345576478284228376073670571646356401162299439184862813044493573253596123455176468974714188846054112527108968938884292759762043717438585685314138246495541580159383671282098306977461412321590398143395408933484974952209631431062016316646415085049130090980939899431089399294190073420318461005695618030273786677969332954501892209068829746374066239722536639068395204248225513550233334961960928605977988464525551842393134197366398782490722703917222543927938228662256671720674599120818771544202337991100431921007170249357400706732178943189857754690035153719671012851905711621053028628593731588178463669914156357640449019966758018645637841011686702148326897940500377576684094649809133657149348148237929343755825113110373434920873754635");
			ModNumber mexp = ModNumber::stomn("66840888410441760431839008872429674786419329636947418486460242561044919868053869971976464859438191265887896243222940201658401353337753300438215886148441339078216785708877835956692459219822002493884972356251586865104182347644159257068335173625268932546306734352968311916102854978342661710523530157238512450600724706170314071288375449948465578336724584198892103722466008047995367194272763845109028428306667446729034252388674119953811809593600161087191056187116971081114707753744857003498743463521691172355610382902837915916763124683800911463285129551385731843632347254835865654498537144919929763537704278897583679602329020894286001500521425302769641190605131509796677190963247464411791720659007910130977633339841264663201471449791485423151046301089758016752750326320380610060132161924754725633179932889317172096779389375107196462735828121003200602787404587478208782210959879844373884203461129396173132616162527958860997047993250049347599458141116967070554408128808648142489471544831872646010814913682539628004273766017428610725270687359846125158653414537710952528646717030923779398311379292718495568643730804690074849491305072524849361209346553174158616974009862637760501134418608607602714010438807696251258896729148547460000474915143690");
			ModNumber mres = my % mx;
			Assert::IsTrue(mexp == mres);

		}
		TEST_METHOD(TestHugeNumberxModHugeNumbery2)
		{
			ModNumber mx = ModNumber::stomn("66840888410441760431839008872429674786419329636947418486460242561044919868053869971976464859438191265887896243222940201658401353337753300438215886148441339078216785708877835956692459219822002493884972356251586865104182347644159257068335173625268932546306734352968311916102854978342661710523530157238512450600724706170314071288375449948465578336724584198892103722466008047995367194272763845109028428306667446729034252388674119953811809593600161087191056187116971081114707753744857003498743463521691172355610382902837915916763124683800911463285129551385731843632347254835865654498537144919929763537704278897583679602329020894286001500521425302769641190605131509796677190963247464411791720659007910130977633339841264663201471449791485423151046301089758016752750326320380610060132161924754725633179932889317172096779389375107196462735828121003200602787404587478208782210959879844373884203461129396173132616162527958860997047993250049347599458141116967070554408128808648142489471544831872646010814913682539628004273766017428610725270687359846125158653414537710952528646717030923779398311379292718495568643730804690074849491305072524849361209346553174158616974009862637760501134418608607602714010438807696251258896729148547460000474915143690");
			ModNumber my = ModNumber::stomn("67885277291854912938587378017853691459496193677756552480553436557302300320101312718354512366504982574838958861888542718272904603102504548837304584105562307267284676071603679712110072004987020967899883804718561430292652099594457858875289933946756413441096538132475400931050009417996185648350176662387902186108621517311830327723099115016623424851268094682681323829709292907275341890111203138351101069715278419043245098727421641629718536436838918204954343308555448232382368388580601057201273701188613232106352300442738562367465251389869660183071271610913707341230465789657707599097586310256539211176484567156528847506639917990006758261522292135816044123533114985744902968420423817686515256802404411459420510055567668821773480759839945638865270345325327032377340654619518821029267132265318694685281072806300858177007288594225758039156380947826545771278835135058430286184244368381139666029873832564755473361825936566690845345140947317051183032581586950151989519809420014114182249129767248172760729288655451472427647241152820746675436044819097064699101275497442767142366134874787841654717249301013092609819939109466282790957714894233169284428494458512543531352888077862617075549676041201530943138909340541678084859095964562913434445958610945");
			ModNumber mexp = ModNumber::stomn("1044388881413152506748369145424016673076864040809133994093193996257380452047442746378047507066791308951062618665602516614503249764751248399088697957120968189067890362725843755417612785165018474014911448466974565188469751950298601806954760321487480894789803779507089014947154439653523937826646505149389735507896811141516256434723665068157846514543510483789220107243284859279974695838439293242072641408610972314210846338747521675906726843238757117763287121438477151267660634835744053702530237666922059750741917539900646450702126706068748719786142059527975497598118534821841944599049165336609447638780288258945167904310897095720756761000866833046402932927983475948225777457176353274723536143396501328442876715726404158572009310048460215714224044235569015624590328299138210969134970340563969052101139916983686080227899219118561576420552826823345168491430547580221503973284488536765781826412703168582340745663408607829848297147697267703583574440469983081435111680611365971692777584935375526749914374972911844423373475135392135950165357459250939540447860959731814613719417843864062256405870008294597041176208304776207941466409821708319923219147905338384914378878215224856574415257432593928229128470532845426825962366816015453433971043467255");
			ModNumber mres = my % mx;
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestGCDHugeNumbersTooLargeForModGroup)
		{
			ModNumber mx = ModNumber::stomn("67885277291854912938587378017853691459496193677756552480553436557302300320101312718354512366504982574838958861888542718272904603102504548837304584105562307267284676071603679712110072004987020967899883804718561430292652099594457858875289933946756413441096538132475400931050009417996185648350176662387902186108621517311830327723099115016623424851268094682681323829709292907275341890111203138351101069715278419043245098727421641629718536436838918204954343308555448232382368388580601057201273701188613232106352300442738562367465251389869660183071271610913707341230465789657707599097586310256539211176484567156528847506639917990006758261522292135816044123533114985744902968420423817686515256802404411459420510055567668821773480759839945638865270345325327032377340654619518821029267132265318694685281072806300858177007288594225758039156380947826545771278835135058430286184244368381139666029873832564755473361825936566690845345140947317051183032581586950151989519809420014114182249129767248172760729288655451472427647241152820746675436044819097064699101275497442767142366134874787841654717249301013092609819939109466282790957714894233169284428494458512543531352888077862617075549676041201530943138909340541678084859095964562913434445958610945");
			ModNumber my = ModNumber::stomn("134726165702296673370426386890283366245915523314703970967013679118347220188155182690330977225943173840726855105111482919931305956440257849275520470254003646345501461780481515668802531224809023461784856160970148295396834447238617115943625107572025345987403272485443712847152864396338847358873706819626414636709346223482144399011474564965089003187992678881573427552175300955270709084383966983460129498021945865772279351116095761583530346030439079292145399495672419313497076142325458060700017164710304404461962683345576478284228376073670571646356401162299439184862813044493573253596123455176468974714188846054112527108968938884292759762043717438585685314138246495541580159383671282098306977461412321590398143395408933484974952209631431062016316646415085049130090980939899431089399294190073420318461005695618030273786677969332954501892209068829746374066239722536639068395204248225513550233334961960928605977988464525551842393134197366398782490722703917222543927938228662256671720674599120818771544202337991100431921007170249357400706732178943189857754690035153719671012851905711621053028628593731588178463669914156357640449019966758018645637841011686702148326897940500377576684094649809133657149348148237929343755825113110373434920873754626");
			ModNumber mres = ModNumber::gcd(mx, my);
			ModNumber mexp(1ull);
			Assert::IsTrue(mres == mexp);
		}

		TEST_METHOD(TestGCDHugeNumbersFitForModGroup)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber my = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mres = ModNumber::gcd(mx, my);
			ModNumber mexp(1ull);
			Assert::IsTrue(mres == mexp);
		}
		TEST_METHOD(TestExpTwoHugeNumberxModHugeNumbery)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber my = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp = ModNumber::stomn("5603130552131808582980129320151431593287387887422304019872729976542631800068788204319579898052711893227272328490995919630013482288843181272802592367798898524753249376917995287069518581823351056941728232218068059475906712986411367854982914146900624999167769127442072642482993253159299132482762489032243580550536702583738240633476465093340828218312246935203167115248772557253480443158870340161116689390922962852167364803089439715091390938186175699671994828460433891219975915334762748122803062377109012273998249380330075478906670900287087012812096397970441489185240044951680317263972659517703324836533035198756630319129921923499685884740053891793070603801214192199848233151436265254188036888028127957406260369087189527978590594475592951159744836132820316608295381151982751459808466074838264192706066312469551538268619096100114127505148129379927320373232121684138190307396104482210039324618282627066824207318147380670441172882452441196009256198174338127429977948883866986080614196689277860961306323052769429682104967016727143083349244333982457816038315667725514706061625905397835791561560354555405798114663550095985095263976338037301461279067122567382168347234118084620013800495920079606189028067751569874497346329975073");
			MultGroupMod mgm(my);
			ModNumber me(2ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpThreeHugeNumberxModHugeNumbery)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber my = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp = ModNumber::stomn("3250884643230911848040993900898243652249531513932618832535257978042483452600091491071448010142398100478042105296260000778740749600718579122776183390547427824993235206147742097168345798614754365320984010059029219682439536967146512822002373439785534655521802047121803418848012395656429274333098159369154311644233907570422616787627723475926083938361636819678257422496690100421348413270866873065352260789155886255064675768143173146339319044846435864434413724869138551196165618880665585280600177108055462531571760767361621253748226656844565125994418973371936230561763833035606088131573663901610503899191493774161996270592967697542825122532247346414087618074103430674872167210384103188364874320914572308032916218625415657467011179515861941549549325736647746798691179745994168171424216744438039562800153093515133179758626811333952347511710966151695995849060660173034462576806791324819774083226059537296440455387776103603089949810718851894140440522457547023362034434091138265699012346903498488388949729603118123456025773082476853228076714602761291459028567417244212052579099284228737307579246003530093230740016729330631060411372234970037396164109907373323398534755180276564538643086778729169896704343882921981536979169501836");
			MultGroupMod mgm(my);
			ModNumber me(3ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpFourHugeNumberxModHugeNumbery)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber my = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp = ModNumber::stomn("3080268413759030384341460902419062434997721043480105848866779760898068393567503640358696717983838966080526546827579170602983272318149414856875115837808486303722109044485795416198041955727945032540579727215123999690418312813355477277356083910442602752355111136928004841925810195818970742970436174828277478215405315973827710555567088163666229286786112475168136234677576560906615486193027124434823092257664145072814066899657089093522999907680713320331963071391531429767880888740895725104586646572927945523938405860480632775436157861081467981082184225014600032040927373958816953213700926896334768412238419934449779068554365771989177623416767861333527128330993153583146562417568714073880278726400518735126270015465444477429492644113567415231915774290228343428776481054611704559704237363844385486009720675254300380072203334120524517665759003680100816318161530169334907964499853628607313832400971196369426432557562516135301618512093776568587542511257533363363369929170820208909917939777029863549314112729980339793894778468238622396334080091491340293772514425601232034375252947892247868057474710141942798184238957688757577035052595292194615849826082321587207703091038792505545536888544063897477993799779043563238023394522327");
			MultGroupMod mgm(my);
			ModNumber me(4ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpFiveHugeNumberxModHugeNumbery)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber my = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp = ModNumber::stomn("5638711490193321968095582821025837950605444226618375138154713166364105598945660174618956327722878472004540065631638033121477000705571056940596828925718926195830996924121418082037133433987071497831290651435693069630617850623228685502937852675280449423766186849426063726462699651472782151270116860694640035896326187491580654641826172411776043924735160566067399148505252115983531709173811670864367402770737994364134054792446295788679175027471719898343002506661556535699568207586460454047069502939531995819982412227908066828322909893757005530027454140572188979488150548883493656102634585283004388269040778035648844374061133441256485694130373153695441673018808503905692775382098592090808337455691437278762153452127026625199516921413226458123714150014291085888648479717581069331505291998640223669211538320227057005969174493070497899969544813875034244416118133376225061639085905224935975593017160559344785335531808006629894396222402748085358182197175776506465368280926618778577016465379764389402423788365712927902557021317967548482940328717296549354184303249524202412120909360058136253604395042854633260658145671789247912207096954542398140903123427771094175794366551714347951366259653252568398361177136455047293213701876695");
			MultGroupMod mgm(my);
			ModNumber me(5ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpNineHugeNumberxModHugeNumbery)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber my = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp = ModNumber::stomn("6526644289055956736858877068519403631010685992730245336265829496694821013414538499486146315974113032228573239209724596217544324651093152616548529250049120999940655678333315395885001604404430674368413549625865049250132470591380208475377609647874790029400479150205957827909009703086882670985349192731759896918001420494741246424227032373058800952891431686568886131564077128377252007511518433882952722716274723386681179475425448262171359024756092838125772314791606694286983792124323139563528579059823470875200334900733517999777734332795930037234961019202348402151535637919629289729883257595638888662496455827375541928600802304845756950166077359813701732089741989302650908674314893619941837738332749876333210736274793708466037828681236498813341528164998456696630564895982403091586560317432213911618061336210329685952960886797529954329889775089979546107202598911341674985812880959595722541767420107480885582726866206084550433759213495839929831622331550209479075977807000372939391980195376179776796433632330315367306866629459555355570369718663720700763486289795413786830649415088063124797019327343416815040790000844014394638687874096152492020345144138425194906563561203752366115273693820073660712451776789995538713644424100");
			MultGroupMod mgm(my);
			ModNumber me(9ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpFourteenHugeNumberxModHugeNumbery)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber my = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp = ModNumber::stomn("6039455007496128416719267954008821642644933296482285601007280340638849216598327471873836884804919649701022781562170836586122709784265816820825575096191834212185018926216348978275581175822063777722482497113140804514895036442142544430881667557825900078592252798965327258914990338583787210599073287488662673805301914377003451438375705278297648482094110689330320376315698672834604231446800836077962070330168937134865160500778390669728454718569045019256569641694928118011409355278801328133547165356294609337610812695213078600632123496284216488466986412259260415850761842086790286642444976955387290025589459443340299552333990449300537311605581899291492541042318438402365969580472495963876590196668452923761989123797416243373802832579730850755435214147890009913750329406321245312028874106538784143568655391804665025694862487948422738665059792179032606285837274072496216497163641530842989068220287114503203934200124660090185998949932049482606796394057023799906848265652644062953681718535513811220343255677529230782090689257816403377077217366329694164624949416603154344694634920762252017370512301667484263576212344489948933525466419603573310889134534166584747655988525572669221990755765536427726512600894509425046404685796958");
			MultGroupMod mgm(my);
			ModNumber me(14ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpTwentyThreeHugeNumberxModHugeNumbery)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber my = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp = ModNumber::stomn("1910558067664321416396680278112421351311128155508056072368901508695988642856892709484053518717865300498833651493446191878311109463208734254327768677827631662480165138464607173165845236905892018996909578572567001105120710562754908421918976912431771111407349953372447903465407154463492560142565541044022453166310527846151093678811771978142693883611628484850315390633830071712500390333553961144075202278876666115263024160232594658499786859669890239340154655186150173362284474209855367200301366985476358028405918824749647709592321300691313415640628857337008354181546978239419012926142357108578893452157019470600385662609894172793751782975445692878550393789212014876536630398912024388301225226840243278763130519406654670836585560317047461520560082897582974145444240845524666069689002899325216988866056408910609616722220391670072906927908320209990818476307264086217589874274441162728455864826783695133239286440580764813761080633506062001385591618688515524216363685876312727637894283515606949804806273854084400015421161488551836257404747281713334156197917858069802946835981863333047175489480127714118264832628367050972187495226968652928612714283155344928655133603656002771958262019707727171589382556221257208131632394242127");
			MultGroupMod mgm(my);
			ModNumber me(23ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestExpThirtySevenHugeNumberxModHugeNumbery)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber my = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp = ModNumber::stomn("242760452789498187560222116076891445988028458456992017072504963165600161058896002310018225049894657031384427443541488709065472392086152010601151272712242277440209738146286759300402140345293651269163161826881724438362631376103663291045572406831762311376767187296205676842143574047066926958813256927930059355363901562101377907285379657399980663671306578018989929389619907873845508787293871920494666862217995428202920280527033368535241047172354652925147293331298874308471570694445084800591737290938767976686091604826314011132217253235234948408366413966084924212867402650142726851051817862713774158899264782638102048541460003811706714953941909081746204720033432076260814059663672110883330201609933877960327204186047392396598121000669809974553616720356084495585767909271572461836652890116273387079425214687517318304739386849804331869959580502653626045661944656270423012246002104141776443539286778518133774966685946958812610957974544771791195158152619653737875223424316716008726667030501736269703567752542972625389592056282107982296394621743371100502549330863244492025351194876359303058058710155058508915698932387363209783346367328313138661688531785868590170495058360789323129007712138201241342346426980353430114603198996");
			MultGroupMod mgm(my);
			ModNumber me(37ull);
			ModNumber mres = mgm.Exp(mx, me);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestDiffLgreaterRLessMod)
		{
			ModNumber ml(100ull);
			ModNumber mr(90ull);
			ModNumber mn(110ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(10ull);
			ModNumber mres = mgm.Diff(ml, mr);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestDiffLgreaterRGreaterMod)
		{
			ModNumber ml(100ull);
			ModNumber mr(90ull);
			ModNumber mn(60ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(10ull);
			ModNumber mres = mgm.Diff(ml, mr);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestDiffLlessRLessMod)
		{
			ModNumber ml(90ull);
			ModNumber mr(100ull);
			ModNumber mn(110ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(100ull);
			ModNumber mres = mgm.Diff(ml, mr);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestDiffLlessRGreaterMod)
		{
			ModNumber ml(90ull);
			ModNumber mr(100ull);
			ModNumber mn(60ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(50ull);
			ModNumber mres = mgm.Diff(ml, mr);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestInverseOfZero)
		{
			ModNumber mzero;
			ModNumber mtwo(2ull);
			MultGroupMod mgm(mtwo);
			ModNumber mres;
			Assert::ExpectException<std::domain_error>([&mres, mzero, mgm] {mres = mgm.Inverse(mzero); });
		}

		TEST_METHOD(TestInverseThreeAndSevenModTwenty)
		{
			ModNumber mx(3ull);
			ModNumber mn(20ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(7ull);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestInverseSevenAndThreeModTwenty)
		{
			ModNumber mx(7ull);
			ModNumber mn(20ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(3ull);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestInverseTwoAndSixModEleven)
		{
			ModNumber mx(2ull);
			ModNumber mn(11ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(6ull);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestInverseSixAndTwoModEleven)
		{
			ModNumber mx(6ull);
			ModNumber mn(11ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(2ull);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestInverseFiveAndFiveModTwelve)
		{
			ModNumber mx(5ull);
			ModNumber mn(12ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(5ull);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestInverseSevenAndSevenModTwelve)
		{
			ModNumber mx(7ull);
			ModNumber mn(12ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(7ull);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestInverseElevenAndElevenModTwelve)
		{
			ModNumber mx(11ull);
			ModNumber mn(12ull);
			MultGroupMod mgm(mn);
			ModNumber mexp(11ull);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestInverseOfTwentyAndFifteen)
		{
			ModNumber mx(15ull);
			ModNumber mn(20ull);
			MultGroupMod mgm(mn);
			ModNumber mres;
			Assert::ExpectException<std::domain_error>([mx,mgm,&mres] {mres = mgm.Inverse(mx); });
		}
		TEST_METHOD(TestInverseOfLargeNumber1)
		{
			ModNumber mx = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			ModNumber mn = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp1 = ModNumber::stomn("990338159586064333264351853726399202032439273344500090039361795769103042626153");
			MultGroupMod mgm(mn);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp1 == mres);
			ModNumber product = mgm.Mult(mx, mres);
			ModNumber mexp2(1ull);
			Assert::IsTrue(mexp2 == product);
		}
		TEST_METHOD(TestInverseOfLargeNumber1Inverse)
		{
			ModNumber mx = ModNumber::stomn("990338159586064333264351853726399202032439273344500090039361795769103042626153");
			ModNumber mn = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp1 = ModNumber::stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
			MultGroupMod mgm(mn);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp1 == mres);
			ModNumber product = mgm.Mult(mx, mres);
			ModNumber mexp2(1ull);
			Assert::IsTrue(mexp2 == product);
		}

		TEST_METHOD(TestInverseOfLargeNumber2)
		{
			ModNumber mx = ModNumber::stomn("113638967017082606674052656070205979468957500859029166099119331283109255964689");
			ModNumber mn = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp1 = ModNumber::stomn("916563989161488767790915831324982323599332376517448274768723301425613841973609");
			MultGroupMod mgm(mn);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp1 == mres);
			ModNumber product = mgm.Mult(mx, mres);
			ModNumber mexp2(1ull);
			Assert::IsTrue(mexp2 == product);
		}
		TEST_METHOD(TestInverseOfLargeNumber2Inverse)
		{
			ModNumber mx = ModNumber::stomn("916563989161488767790915831324982323599332376517448274768723301425613841973609");
			ModNumber mn = ModNumber::stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
			ModNumber mexp1 = ModNumber::stomn("113638967017082606674052656070205979468957500859029166099119331283109255964689");
			MultGroupMod mgm(mn);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp1 == mres);
			ModNumber product = mgm.Mult(mx, mres);
			ModNumber mexp2(1ull);
			Assert::IsTrue(mexp2 == product);
		}
		TEST_METHOD(TestInverseOfHugeNumber1)
		{
			ModNumber mx = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			ModNumber mn = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp1 = ModNumber::stomn("7077938970216339316645445575285346683424941653237210340139516526256813509979606177021530849277742732364801030056254685283272967813034126654684370996868817665975637877200499989860765830543219806691253546378869794068270590583892833700153879570795112599739140513412553864621285534205670943364750922097762621895818680253818070884230571117040150011896031221708030875177847284891792635438074811694778675171964272643019638156031482913499567423251969964170693617285474555961748092945378333120890780361163560901493876218729987658462900795199003929229716100635333728591704523260412802193993472731422243051758869939898387278562993320264471554982496798519107136733940096336315302043051078369874122713882086967410973438379654109746059534714870547090057664281203709444590153178636867403107145141196420110304550068844058322769189735671101637680650707903615234635200908408342995823505703193770467699263824297761978530688995807637960633835113674050660862505648679547630512260845352364885624977257360366150205959045524481094456933930287176995345532173558521752894170223063256872758780959875487778634305779963228215875539131006201580009220506095752025694701229168899054377957651680451709904249939778196108148618292542907928847184822275");
			MultGroupMod mgm(mn);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp1 == mres);
			ModNumber product = mgm.Mult(mx, mres);
			ModNumber mexp2(1ull);
			Assert::IsTrue(mexp2 == product);
		}
		TEST_METHOD(TestInverseOfHugeNumber1Inverse)
		{
			ModNumber mx = ModNumber::stomn("7077938970216339316645445575285346683424941653237210340139516526256813509979606177021530849277742732364801030056254685283272967813034126654684370996868817665975637877200499989860765830543219806691253546378869794068270590583892833700153879570795112599739140513412553864621285534205670943364750922097762621895818680253818070884230571117040150011896031221708030875177847284891792635438074811694778675171964272643019638156031482913499567423251969964170693617285474555961748092945378333120890780361163560901493876218729987658462900795199003929229716100635333728591704523260412802193993472731422243051758869939898387278562993320264471554982496798519107136733940096336315302043051078369874122713882086967410973438379654109746059534714870547090057664281203709444590153178636867403107145141196420110304550068844058322769189735671101637680650707903615234635200908408342995823505703193770467699263824297761978530688995807637960633835113674050660862505648679547630512260845352364885624977257360366150205959045524481094456933930287176995345532173558521752894170223063256872758780959875487778634305779963228215875539131006201580009220506095752025694701229168899054377957651680451709904249939778196108148618292542907928847184822275");
			ModNumber mn = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp1 = ModNumber::stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
			MultGroupMod mgm(mn);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp1 == mres);
			ModNumber product = mgm.Mult(mx, mres);
			ModNumber mexp2(1ull);
			Assert::IsTrue(mexp2 == product);
		}
		TEST_METHOD(TestInverseOfHugeNumber2)
		{
			ModNumber mx = ModNumber::stomn("5603130552131808582980129320151431593287387887422304019872729976542631800068788204319579898052711893227272328490995919630013482288843181272802592367798898524753249376917995287069518581823351056941728232218068059475906712986411367854982914146900624999167769127442072642482993253159299132482762489032243580550536702583738240633476465093340828218312246935203167115248772557253480443158870340161116689390922962852167364803089439715091390938186175699671994828460433891219975915334762748122803062377109012273998249380330075478906670900287087012812096397970441489185240044951680317263972659517703324836533035198756630319129921923499685884740053891793070603801214192199848233151436265254188036888028127957406260369087189527978590594475592951159744836132820316608295381151982751459808466074838264192706066312469551538268619096100114127505148129379927320373232121684138190307396104482210039324618282627066824207318147380670441172882452441196009256198174338127429977948883866986080614196689277860961306323052769429682104967016727143083349244333982457816038315667725514706061625905397835791561560354555405798114663550095985095263976338037301461279067122567382168347234118084620013800495920079606189028067751569874497346329975073");
			ModNumber mn = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp1 = ModNumber::stomn("6739138816136043312649272213174713769262983880649218872965179549677556315677506972615490310113406336693569234718740510119663919502539189540509251055798460074325514337939662564750863101055411914158661300731270123866093947731042804706074093172689096545237738202130653344811784515331242745926904461789015551948920613465874894376131240703947702355076603139914962929019780686185414664545329663218317295135211022489224303293900015601419851026256029933643673808719974409805841004170959181758705284178201118626199154465204469896041437537889130973291884948000755501999889301784901971136592495490062001754886483556366544831945270673733954935411162006328088367540078380415454971364899079826871715550246362070409406505441342293480933173058131951954050439775679821566910118564679299097353256497284984326254753897393933507219970088105495830123020354077871747461442531592169831588898411349425799974571880327159780649631710046012433252304811853839406510704163016067299984268200721162895812075320190090687162866263831468978576430892453490199685391372592054813829543523397778555188730184762400193992336282063202658915226866054763855569295020560910720893342762594098082777028627182850722798218135965067629433110731488659794290674761733");
			MultGroupMod mgm(mn);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp1 == mres);
			ModNumber product = mgm.Mult(mx, mres);
			ModNumber mexp2(1ull);
			Assert::IsTrue(mexp2 == product);
		}
		TEST_METHOD(TestInverseOfHugeNumber2Inverse)
		{
			ModNumber mx = ModNumber::stomn("6739138816136043312649272213174713769262983880649218872965179549677556315677506972615490310113406336693569234718740510119663919502539189540509251055798460074325514337939662564750863101055411914158661300731270123866093947731042804706074093172689096545237738202130653344811784515331242745926904461789015551948920613465874894376131240703947702355076603139914962929019780686185414664545329663218317295135211022489224303293900015601419851026256029933643673808719974409805841004170959181758705284178201118626199154465204469896041437537889130973291884948000755501999889301784901971136592495490062001754886483556366544831945270673733954935411162006328088367540078380415454971364899079826871715550246362070409406505441342293480933173058131951954050439775679821566910118564679299097353256497284984326254753897393933507219970088105495830123020354077871747461442531592169831588898411349425799974571880327159780649631710046012433252304811853839406510704163016067299984268200721162895812075320190090687162866263831468978576430892453490199685391372592054813829543523397778555188730184762400193992336282063202658915226866054763855569295020560910720893342762594098082777028627182850722798218135965067629433110731488659794290674761733");
			ModNumber mn = ModNumber::stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
			ModNumber mexp1 = ModNumber::stomn("5603130552131808582980129320151431593287387887422304019872729976542631800068788204319579898052711893227272328490995919630013482288843181272802592367798898524753249376917995287069518581823351056941728232218068059475906712986411367854982914146900624999167769127442072642482993253159299132482762489032243580550536702583738240633476465093340828218312246935203167115248772557253480443158870340161116689390922962852167364803089439715091390938186175699671994828460433891219975915334762748122803062377109012273998249380330075478906670900287087012812096397970441489185240044951680317263972659517703324836533035198756630319129921923499685884740053891793070603801214192199848233151436265254188036888028127957406260369087189527978590594475592951159744836132820316608295381151982751459808466074838264192706066312469551538268619096100114127505148129379927320373232121684138190307396104482210039324618282627066824207318147380670441172882452441196009256198174338127429977948883866986080614196689277860961306323052769429682104967016727143083349244333982457816038315667725514706061625905397835791561560354555405798114663550095985095263976338037301461279067122567382168347234118084620013800495920079606189028067751569874497346329975073");
			MultGroupMod mgm(mn);
			ModNumber mres = mgm.Inverse(mx);
			Assert::IsTrue(mexp1 == mres);
			ModNumber product = mgm.Mult(mx, mres);
			ModNumber mexp2(1ull);
			Assert::IsTrue(mexp2 == product);
		}
		TEST_METHOD(TestGetRSAKey)
		{
			RSAParameters rsaParameters = GetRSAKey();
			ModNumber mExponentExp = ModNumber::stomn("010001", 16);
			ModNumber mModulusExp = ModNumber::stomn("A26960E1102074E8E1E7D0754F6BED06BC5AAFEEA086704ACB33B99289FBC180874418D11E02B1B250313EE873E7307D932DA7C30A5E703CBE86EC588B318B46C20BD8BB01F51549635641E8958B0A109E59B06752ABED027539FBEA8BB700E58AD1985CE405F57BD01F4FB91E88BC8B50C4F0208EA02BB72612F41DE302DB3C7BFF5EB12C277417068CE2CA387BEBDE35320D045438951AD51D34CE1F52B10E1F7A9023E9C647582D266CEF6D2E067B60C1BED9C781ADC520223091210854A356C6CAE5B1874FDC59A643441C228F30E4329345AD96A2605972524BFEE8764D207435E42001C678028C48167610A6FCDDF43BC789DD04A6AC3B337E4F6D8085A404E759936355BCA8E7ED93AB6BB031830CEB586D1CC5A90E87F497B8096AC6ABE37BC701E33C9D9B61FBC8AF73446AAC90301ACDCE9E0DEBD322A032F1C90CA8D766285723B5DE918E65F1A987B32E2D51DB7E2C8EB8F2A28C7292E14BD56FB0EC779F44C138C9DF08BEDBD0568DC8FB9F65B7DD20CE80652C9194E160DCAA6C5F96BB5D357BE742B971B3F73923447C36C906E12147C0153CFB2551AB581513B38F2D3B90F2F3FDD17CC799224AA7DDBCAF41309008637CBAD18C27E226E849532B3CEE58CF3C9B3E147438935BCECB35C47B50E29623F76687A51792B75E077BC3D1240C73F6AB79B33577014C7BE2738BBE1F8B179517C5B5BEC23D7C9D", 16);
			ModNumber mPrime1Exp = ModNumber::stomn("CDA901CDEFC7392972350E5160DCE36D9E3415ABA270EFA599B72EA3A36E4BECF727A46DDE4B39181D2A1B76CA2A79DBFCAEE2DF3D46648BB6F5766DE8FBDB9B97915A5D6BC9E3C2AB30DF0251B7DF103CB8BB9C8873188E395C01CD8C374D72B6B65FBFF47331232D6F86E3EC22B3F866D35C74DF6D76D35836363D020571C86104307324EE0A704FF5BB77DBBCDC7A2D98BBDB0C65B60F22F4EFD5EE8555715F2C79CCC438484C4E1B2136E881C2B04AB27E015AD354AECDF0D717A78205CA7DF0353F2312FD54EF8FB51DA12E35115AFDFA0659C4386DF841C0FF51BB5EC37D555CEDF88FC783FB568FA6F5736060837B4020CACD9D9BAC5F75348673EDCB", 16);
			ModNumber mPrime2Exp = ModNumber::stomn("CA2A5948D61356DEDDCF8F5D180855C87E6981FDE13892F7CF593547B36D1CD5682699FD73D1036A24893EB2B8BCDBA8FC256F65E6644E2FC1E80C1BCADA0E21F6DED17EE6EA989BEB5E912E7FDB448573EC376817C4741C33EF78DCCE9DDE4F13FF6F29FB1117FF0873E5EB0B91AD52E41E2148DED2820C840574608F78E6D0E17DAF74F2B0A6171503EDA87D4B713819F86F07DE94CFFFBC2E8799DF78F138E28205AB7AF0286059980C241CD74C48FE897368E4CE6A096C4677BB2A37CF494395A1C3F695201863AFA632D2BBD61755779C20DD87AD78EE5E8E3EDA91D52FE2D0B91971197AA215EEE3E1F00DE0ED93B42E386C7685B71C1A25BDA0337237", 16);
			ModNumber mExp1Exp = ModNumber::stomn("92F32C22E62DC81E61489FCE6E13E73DB0492F22D09339E96E0881FED30C409C358DD1E3DF6E88813E7593E2315A60705C00EA82F0D997866D58BF2291D3447BCCB54388753870D1EC58A34098FB751BB90796A6A6A8F6E6C8FD811C4A1049FAA9AB6E874E37F0E85DD2CEE3951AC6D5BB435350FF44CC1EFF6CDD1C82913958205A49F43C730A6A6DF7FBD80CDEC9FB53114B499D761FBC7911E33D4D8D601BFE115D867EBE688A7D54CD4ADA9FBD691236AFEA6FE7891ECE704480D6640597469D98AE6075752677768BC77C1FF4334E7D2365DE4662BBD123CAAFA4CAE94F6E02CCC00D574F0C734ECA9E55CBF7157F5D55D918900B09ED04D84E8BB3D239", 16);
			ModNumber mExp2Exp = ModNumber::stomn("76DEF5C6CD116C07F9769CFE892E057AB151CA7C845C66964D964FACCC44C12A7E660C2FAA54E0E41D7D919F36DEDD6DFC6CEA968924BB907B3CBD39EE49CEFB8B9432ACA28C98953D8D4A62840D62B9F19298759D5C0745BA07FA12236F52133F57F77E40C12951ED1F70B68703EE5D0A02E961569B829A675A69B68945F6BA258C6889A455D82344591DF9EF4A57D53BF8071922786FE1183E67BF435672C816EF8AF3EC829FCB1F8336C859290312FC8FC78B8AD610256044030CCB632B8011BC3EF4E1DD8794AFB0CD519EF5918A00D826B6706BE1C574ABA68A30AC8524F601F284922B001FA433E64CA881109BC138B5E0B96DEB6BE29B414BC191DE7B", 16);
			ModNumber mCoefficientExp = ModNumber::stomn("80B8FE74BA8F322BB18AD25B5EA75E1B50F54521E648960084FA90895634016A7C3D0997C52A1BC766C58BFFE8CD8953146A188D29480311E624048F83ADAEE157EFC4AE9EFFB144BA8F41E2CACDACD50A51367BB9D013E29B6C859AFC030CAE31B38740F287D61F4CDEB78659E91D404FD6976FD7F5E34C1B7FF530B655AA36161C928BD128D56A6FC1469CF8DB7D308CAFAE0AFDA03A71A399C9EC05889427AAE189EA9DD2BB4A4132FB16622FC9394759F337C8AAD5AF6E3BF659D0624EB10238BA58BDAF858ACF954FBFF03AD12FC0F55C79EDC318FC96D6BC53DEB0688E2C1B80572EC942E37848C75BDAEE6366FA5C11396F8E18DD6AB0F61AD71265D4", 16);
			ModNumber mPrivExpExp = ModNumber::stomn("6DD1E9C365DF3C8E78AB02A740640F2B7DD3A8A12804A00BC3299FB2FA00B5508EA6BA4D3508C75576AA460A72CD0259F75A68D516BED310101935D98D8D9BCAE5CD92E8A9232B18ABC618DD07B9B3BD9705B8A5B51E322E764C160EFC186BCFCB357CDC5B07B87DD73644F73D3162A4A5A1F5695E67EA0777045BF3959C49D8B553F198C5ED8BAF422BA3F2EF8CC3EA4678289B23C3FAF6E0E255E18706A8588F6A95EAE2D9C34659896121E8E6B5555A10C857847328A861D91C94160D070647D8363C68F5C147938B3C72E2BF6FA9B4B48A271481061D367E7A2D8F196B5D2DB3C6F911B7819786C98AB93D6E772C077D6DAAB88B2DE8F20F39FB9003D061F9C8CB3A24B6F650C3C6393117201B765546B40453313045C0C67F9A404016F7073DE6F6877E7D9100EC77B7252D1E064EA99B102E73095D80049D127932C4A63D894DBF37446A5222CBA22B9B1B70B9C09C3EBA69EE4C302B6A59F7EE6C70C1F9283E9E171C14457FB78FAAC615AB15F691318A4CB4E55F1F1A1E635EAE217565A4D94A3505095232A4CB211E579122C2DCB67944A9E2514CEEA5C3E85CEFA916887B9D7B0BC4042EB4622A5E283566105E997FACFCC3EDBD152620E2A472E8CE47464BB0892642AE410CB9A5F283693E2F736444C54338978E21775E616BC2839208E89E5312CD26686C21E964F6AA29874E84FC1776751C1FAF0369B57075", 16);
			Assert::IsTrue(mExponentExp == rsaParameters.pubExp);
			Assert::IsTrue(mModulusExp == rsaParameters.Modulus);
			Assert::IsTrue(mPrime1Exp == rsaParameters.Prime1);
			Assert::IsTrue(mPrime2Exp == rsaParameters.Prime2);
			Assert::IsTrue(mExp1Exp == rsaParameters.Exp1);
			Assert::IsTrue(mExp2Exp == rsaParameters.Exp2);
			Assert::IsTrue(mCoefficientExp == rsaParameters.Coefficient);
			Assert::IsTrue(mPrivExpExp == rsaParameters.PrivExp);
			ModNumber mModulusProduct = rsaParameters.Prime1 * rsaParameters.Prime2;
			ModNumber Pminus1 = rsaParameters.Prime1 - ModNumber(1ull);
			ModNumber Qminus1 = rsaParameters.Prime2 - ModNumber(1ull);
			ModNumber PhiPQ = Pminus1 * Qminus1;
			MultGroupMod mgmPhiPQ(PhiPQ);
			ModNumber PrivExpCalc = mgmPhiPQ.Inverse(rsaParameters.pubExp);
			ModNumber DpCalc = PrivExpCalc % Pminus1;
			ModNumber DqCalc = PrivExpCalc % Qminus1;
			MultGroupMod mgmP(rsaParameters.Prime1);
			ModNumber InverseQCalc = mgmP.Inverse(mPrime2Exp);
			Assert::IsTrue(mModulusExp == mModulusProduct);
			Assert::IsTrue(mPrivExpExp == PrivExpCalc);
			Assert::IsTrue(mExp1Exp == DpCalc);
			Assert::IsTrue(mExp2Exp == DqCalc);
			Assert::IsTrue(mCoefficientExp == InverseQCalc);
		}

	};
	END_TEST_CLASS
}
