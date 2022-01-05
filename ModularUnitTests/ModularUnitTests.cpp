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
			llint l [COUNTLL];
			llint r [COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
			{
				l[i] = 2ull;
				r[i] = 1ull;
			}
			llint* res = CalcSubtract(l, r);
			
			Assert::IsTrue(CalcEqual(r, res));
		}
		TEST_METHOD(TestEqualTrue)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			Assert::IsTrue(CalcEqual(l, l));
		}
		TEST_METHOD(TestSubtractEqualNumbers)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			llint res[COUNTLL] = {};
			Assert::IsTrue(CalcEqual(CalcSubtract(l, l), res));
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
			Assert::IsFalse(CalcEqual(l, r));
		}
		TEST_METHOD(TestAddAssignOneToZero)
		{
			llint l[COUNTLL] = {};
			llint res[COUNTLL] = { 1ull };
			AddAssignScalar((lint*)l, 0, 1ul);
			Assert::IsTrue(CalcEqual(l, res));
		}
		TEST_METHOD(TestAddAssignOneToMax)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = ~0ull;
			llint res[COUNTLL] = {};
			AddAssignScalar((lint*)l, 0, 1ul);
			Assert::IsTrue(CalcEqual(l, res));
		}
		TEST_METHOD(TestMultiplyByZero)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL; i++)
				l[i] = i;
			llint r[COUNTLL] = {};
			Assert::IsTrue(CalcEqual(MultiplyByScalar(l, 0ul), r));
		}
	};
}
