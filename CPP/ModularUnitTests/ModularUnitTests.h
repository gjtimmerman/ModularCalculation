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
		TEST_METHOD(TestSubtractWithCarry)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			l[0] = 0ul;
			l[1] = 1ul;
			r[0] = 1ul;
			r[1] = 0ul;
			for (int i = 2; i < COUNTLL; i++)
			{
				l[i] = 0ull;
				r[i] = 0ull;
			}
			llint exp[COUNTLL];
			exp[0] = ~0ull;
			for (int i = 1; i < COUNTLL; i++)
			{
				exp[i] = 0ul;
			}

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber res = ml - mr;

			Assert::IsTrue(mexp == res);
		}
		TEST_METHOD(TestSubtractWithCarryAcrossMultipleSections)
		{
			llint l[COUNTLL];
			llint r[COUNTLL];
			l[0] = 0ul;
			l[COUNTLL-1] = 1ul;
			r[0] = 1ul;
			r[COUNTLL-1] = 0ul;
			for (int i = 1; i < COUNTLL-1; i++)
			{
				l[i] = 0ull;
				r[i] = 0ull;
			}
			llint exp[COUNTLL];
			exp[COUNTLL-1] = 0ull;
			for (int i = 0; i < COUNTLL-1; i++)
			{
				exp[i] = ~0ull;
			}

			ModNumber ml(l);
			ModNumber mr(r);
			ModNumber mexp(exp);
			ModNumber res = ml - mr;

			Assert::IsTrue(mexp == res);
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


		TEST_METHOD(TestAddAssignOneToZero)
		{
			llint l[COUNTLL] = {};
			llint res[COUNTLL] = { 1ull };
			ModNumber ml(l);
			ModNumber mres(res);
			ml += 1ul;
			Assert::IsTrue(ml == mres);
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
			ml += 1ul;
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
			ml += 1ul;
			Assert::IsTrue(ml == mres);
		}
		TEST_METHOD(TestAddAssignMaxToMax)
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
		TEST_METHOD(TestMultiplyAllFFFFByTwo)
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
		TEST_METHOD(TestDivisionMaxLintTimesTenByTwo)
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
			exp[0] = '1';
			Assert::IsTrue(res == exp);
		}
		TEST_METHOD(TestToStringOctalForMaxesAndZeros)
		{
			llint l[COUNTLL];
			for (int i = 0; i < COUNTLL - 4; i += 6)
			{
				for (int j = 0; j < 3; j++)
					l[i + j] = ~0;
				for (int j = 3; j < 6; j++)
					l[i + j] = 0;
			}
			for (int i = COUNTLL - 4; i < COUNTLL; i++)
				l[i] = 0;
			ModNumber ml(l);
			std::string res = ml.to_string(8);
			std::string exp;
			exp.reserve(OctalStringLength);
			exp.assign(86, '0');
			for (int i = 86; i < OctalStringLength; i += 128)
			{
				exp.append(64, '0');
				exp.append(64, '7');
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
			for (int i = 0; i < HexStringLength; i += 32)
				s.append("0F0E0D0C0B0A09080706050403020100");
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberHexIncreasingSequence)
		{
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i += 2)
			{
				exp[i] = 0x0706050403020100ull;
				exp[i + 1] = 0x0F0E0D0C0B0A0908ull;
			}
			ModNumber mexp(exp);
			std::string s;
			s.reserve(HexStringLength);
			for (int i = 0; i < HexStringLength; i += 32)
				s.append("0F0E0D0C0B0A09080706050403020100");
			ModNumber mres = ModNumber::stomn(s, 16);
			Assert::IsTrue(mexp == mres);
		}
		TEST_METHOD(TestToModularNumberHexIncreasingSequenceSwitched)
		{
			llint exp[COUNTLL];
			for (int i = 0; i < COUNTLL; i += 2)
			{
				exp[i] = 0x08090A0B0C0D0E0Full;
				exp[i + 1] = 0x0001020304050607ull;
			}
			ModNumber mexp(exp);
			std::string s;
			s.reserve(HexStringLength);
			for (int i = 0; i < HexStringLength; i += 32)
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
			s.assign(6,'0');
			for (int i = 0; i < OctalStringLength-6; i += 16)
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


	};
	END_TEST_CLASS
}
