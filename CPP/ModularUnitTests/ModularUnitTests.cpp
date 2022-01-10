#include "pch.h"
#include "CppUnitTest.h"

#include "..\ModularCalculation\ModularCalculation.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace ModularUnitTests
{
	TEST_CLASS(ModularUnitTests)
	{
	public:
		
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
			
			Assert::IsTrue(r == res);
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
		TEST_METHOD(TestEqualNotTrue)
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
			Assert::IsFalse(l == r);
		}
		TEST_METHOD(TestAddAssignOneToZero)
		{
			llint l[COUNTLL] = {};
			llint res[COUNTLL] = { 1ull };
			ModNumber ml(l);
			ModNumber mres(res);
			ml.AddAssignScalar(0, 1ul);
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestAddAssignOneToMax)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			llint res[COUNTLL] = {};
			ModNumber ml(l);
			ModNumber mres(res);
			ml.AddAssignScalar(0, 1ul);
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestMultiplyByZero)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			ModNumber ml(l);
			ModNumber mr;
			Assert::IsTrue((ml *= 0ul) == mr);
		}
		TEST_METHOD(TestMultiplyByOne)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			ModNumber ml(l);
			ModNumber mr(l);
			Assert::IsTrue((ml *= 1ul) == mr);
		}
		TEST_METHOD(TestMultiplyByTwo)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = i;
				r[i] = i * 2;
			}
			ModNumber ml(l);
			ModNumber mr(r);
			Assert::IsTrue((ml *= 2ul) == mr);
		}
		TEST_METHOD(TestMultiplyAllNinesByTwo)
		{
			llint l[COUNTLL];
			llint res[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = ~0ull;
				res[i] = ~0ull;
			}
			ModNumber ml(l);
			res[0] ^= 1ull;
			ModNumber mres(res);
			Assert::IsTrue((ml *= 2ul) == mres);
		}
		TEST_METHOD(TestToStringOctalForZero)
		{
			ModNumber ml;
			std::string res = ml.to_string(8);
			std::string exp;
			exp.reserve(NSIZE / 3 + 1);
			for (int i = 0; i < NSIZE / 3 + 1; i++)
				exp.append("0");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringOctalForOne)
		{
			ModNumber ml(1);
			std::string res = ml.to_string(8);
			std::string exp;
			exp.reserve(NSIZE / 3 + 1);
			for (int i = 0; i < NSIZE / 3; i++)
				exp.append("0");
			exp.append("1");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringOctalForEight)
		{
			ModNumber ml(8);
			std::string res = ml.to_string(8);
			std::string exp;
			exp.reserve(NSIZE / 3 + 1);
			for (int i = 0; i < NSIZE / 3 - 1; i++)
				exp.append("0");
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
			exp.reserve(NSIZE / 3 + 1);
			exp.assign(NSIZE / 3 + 1, '7');
			exp[0] = '1';
			Assert::IsTrue(res == exp);
		}
		//TEST_METHOD(TestToStringOctalForMaxesAndZeros)
		//{
		//	llint l[COUNTLL];
		//	for (int i = 0; i < COUNTLL - 4; i += 6)
		//	{
		//		for (int j = 0; j < 3; j++)
		//			l[i + j] = ~0;
		//		for (int j = 3; j < 6; j++)
		//			l[i + j] = 0;
		//	}
		//	for (int i = COUNTLL - 4; i < COUNTLL; i++)
		//		l[i] = 0;
		//	ModNumber ml(l);
		//	std::string res = ml.to_string(8);
		//	std::string exp;
		//	exp.reserve(NSIZE / 3 + 1);
		//	for (int i = 0; i < 6; i++)
		//		exp.append("0");
		//	for (int i = 6; i < NSIZE / 3 + 1; i += 8)
		//	{
		//		for (int j = 0; j < 4; j++)
		//			exp.append("0");
		//		for (int j = 0; j < 4; j++)
		//			exp.append("7");
		//	}
		//	Assert::IsTrue(res == exp);
		//}
		TEST_METHOD(TestToStringHexForZero)
		{
			ModNumber ml;
			std::string res = ml.to_string(16);
			std::string exp;
			exp.reserve(NCOUNT * 2);
			for (int i = 0; i < NCOUNT * 2; i++)
				exp.append("0");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringHexForOne)
		{
			ModNumber ml(1);
			std::string res = ml.to_string(16);
			std::string exp;
			exp.reserve(NCOUNT * 2);
			for (int i = 0; i < NCOUNT * 2 - 1; i++)
				exp.append("0");
			exp.append("1");
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringHexForSixteen)
		{
			ModNumber ml(16);
			std::string res = ml.to_string(16);
			std::string exp;
			exp.reserve(NCOUNT * 2);
			for (int i = 0; i < NCOUNT * 2 - 2; i++)
				exp.append("0");
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
			exp.reserve(NCOUNT * 2);
			for (int i = 0; i < NCOUNT * 2; i++)
				exp.append("F");
			Assert::IsTrue(res == exp);
		}

	};
}
