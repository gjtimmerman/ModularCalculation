// ModularCalculation.cpp : Defines the functions for the static library.
//

#include "pchmc.h"
#include "framework.h"
#include "ModularCalculation.h"




ModNumber& operator-=(ModNumber& ml, const lint &r)
{
	lint* pl = (lint*)ml.num;
	lint carry = 0;
	lint ltmp = pl[0];
	if (ltmp < r)
	{
		carry = 1;
	}
	pl[0] = ltmp - r;
	int i = 1;
	for (int i = 1;carry > 0 && i < COUNTL; i++)
	{
		if (carry <= pl[i])
		{
			pl[i] -= carry;
			carry = 0;
		}
		else
			pl[i] -= carry;
	}
	return ml;
}

void PerformSubtraction(ModNumber& mres, const ModNumber& ml, const ModNumber& mr)
{
	// res might be equal to l
	if (ml == mr)					// Optimization
	{
		mres = ModNumber();
		return;
	}

	const lint* pl = (const lint*)ml.num;
	const lint* pr = (const lint*)mr.num;
	lint* pres = (lint*)mres.num;
	lint carry = 0;
	for (int i = 0; i < COUNTL; i++)
	{
		lint ltmp = pl[i];
		lint rtmp = pr[i];
		if (ltmp >= carry)
		{
			ltmp -= carry;
			carry = 0;
		}
		else
		{
			ltmp -= carry;
		}
		if (ltmp < rtmp)
		{
			carry = 1;
		}
		pres[i] = ltmp - rtmp;
	}


}

ModNumber operator-(const ModNumber& ml, const ModNumber& mr)
{
	ModNumber mres;
	PerformSubtraction(mres, ml, mr);
	return mres;
}

ModNumber &operator-=(ModNumber& ml, const ModNumber& mr)
{
	PerformSubtraction(ml, ml, mr);
	return ml;
}

unsigned int ModNumber::FindFirstNonZeroBitInWord(unsigned int word) const
{
	llint mask = 01ull << (LLSIZE*8 - 1);
	for (int i = 0; i < LLSIZE*8; i++)
	{
		if (num[word] & mask)
			return i;
		mask >>= 1;
	}
	return LLSIZE*8;
}

ModNumber operator/ (const ModNumber& ml, const ModNumber& mr)
{
	ModNumber modRes = ml;
	ModNumber mres = DivideAndModulo(modRes, ml, mr);
	return mres;
}


ModNumber DivideAndModulo(ModNumber& modRes, const ModNumber& ml, const ModNumber& mr, bool onlyModulo)
{
	ModNumber divRes;
	ModNumber mzero;
	if (mr == mzero)
		throw std::domain_error("Division by Zero");
	ModNumber mone(1ull);
	if (mr == mone)		// Just optimization
	{
		modRes = mzero;
		return ml;
	}
	ModNumber mtwo(2ull);
	if (mr == mtwo)		// Just optimization
	{
		divRes = ml >> 1;
		if (ml.num[0] & 0x1ull)
			modRes = mone;
		else
			modRes = mzero;
		return divRes;
	}
	if (ml < mr)
	{
		return mzero;
	}
	if (ml == mr)
	{
		modRes = mzero;
		return mone;
	}
	int indexOfFirstNonzeroWordLeft = 0;
	int indexOfFirstNonzeroWordRight = 0;
	for (int i = COUNTLL - 1; i >= 0; i--)
		if (ml.num[i])
		{
			indexOfFirstNonzeroWordLeft = i;
			break;
		}
	for (int i = indexOfFirstNonzeroWordLeft; i >= 0; i--)
		if (mr.num[i])
		{
			indexOfFirstNonzeroWordRight = i;
			break;
		}
	int differenceLeftAndRight = indexOfFirstNonzeroWordLeft - indexOfFirstNonzeroWordRight;
	divRes = ModNumber();
	for (int i = 0; i <= differenceLeftAndRight; i++)
	{
		llint divisor[COUNTLL] = {};
		llint wordShiftedRightArgument[COUNTLL] = {};
		for (int j = 0; j <= indexOfFirstNonzeroWordRight; j++)
		{
			wordShiftedRightArgument[j + differenceLeftAndRight - i] = mr.num[j];
		}
		divisor[differenceLeftAndRight - i] = 1ull;
		ModNumber mWordShiftedRightArgument(wordShiftedRightArgument);
		ModNumber mdivisor(divisor);
		unsigned int firstNonzeroBitLeft = (LLSIZE * 8 - modRes.FindFirstNonZeroBitInWord(indexOfFirstNonzeroWordLeft)) + (indexOfFirstNonzeroWordLeft)*LLSIZE * 8;
		unsigned int firstNonzeroBitRight = (LLSIZE * 8 - mWordShiftedRightArgument.FindFirstNonZeroBitInWord(differenceLeftAndRight + indexOfFirstNonzeroWordRight - i)) + (differenceLeftAndRight + indexOfFirstNonzeroWordRight - i) * LLSIZE * 8;
		int bitDifference = firstNonzeroBitLeft - firstNonzeroBitRight;
		for (int j = 0; j <= bitDifference; j++)
		{
			ModNumber mBitShiftedRightArgument(mWordShiftedRightArgument);
			ModNumber mDivisorShiftedRight(mdivisor);
			mBitShiftedRightArgument <<= bitDifference - j;
			if (!onlyModulo)
				mDivisorShiftedRight <<= bitDifference - j;
			while (modRes >= mBitShiftedRightArgument)
			{
				modRes -= mBitShiftedRightArgument;
				if (!onlyModulo)
					divRes += mDivisorShiftedRight;
			}
		}
	}
	return divRes;
}


ModNumber operator%(const ModNumber& ml, const ModNumber& mr)
{
	ModNumber modRes = ml;
	DivideAndModulo(modRes,ml, mr, true);
	return modRes;
}

ModNumber& operator%=(ModNumber& ml, const ModNumber& mr)
{

	DivideAndModulo(ml, ml, mr, true);
	return ml;
}

void ShiftLeft(ModNumber& mres, const ModNumber& mn, unsigned int i)
{
	int words = 0;
	if (i >= LSIZE * 8)
	{
		if (i >= NSIZE)
		{
			mres = ModNumber();
			return;
		}
		words = i / (LSIZE * 8);
		i %= (LSIZE * 8);
	}
	const lint* pn = (const lint*)mn.num;
	lint *pres = (lint *)mres.num;
	pres[COUNTL - 1] = pn[COUNTL - words - 1] << i;
	for (int j = COUNTL - 2; j >= words; j--)
	{
		llint tmp = ((llint)pn[j - words]) << i;
		pres[j + 1] |= ((lint*)(&tmp))[1];
		pres[j] = ((lint*)(&tmp))[0];
	}
	if (&mres == &mn)
		for (int j = 0; j < words; j++)
			pres[j] = 0ul;

}

void ShiftRight(ModNumber& mres, const ModNumber& mn, unsigned int i)
{
	int words = 0;
	if (i >= LSIZE * 8)
	{
		if (i >= NSIZE)
		{
			mres = ModNumber();
			return;
		}
		words = i / (LSIZE * 8);
		i %= (LSIZE * 8);
	}
	const lint* pn = (const lint*)mn.num;
	lint *pres = (lint *)mres.num;
	pres[0] = pn[words] >> i;
	for (int j = 0; j < COUNTL - words - 1; j++)
	{
		llint tmp = ((llint)pn[j + words + 1]) << ((LSIZE * 8) - i);
		pres[j] |= ((lint*)(&tmp))[0];
		pres[j + 1] = ((lint*)(&tmp))[1];
	}
	if (&mres == &mn)
		for (int j = COUNTL - words; j < COUNTL; j++)
			pres[j] = 0ul;

}

ModNumber operator << (const ModNumber& mn, unsigned int i)
{
	ModNumber mres;
	ShiftLeft(mres, mn, i);
	return mres;
}

ModNumber operator >> (const ModNumber& mn, unsigned int i)
{
	ModNumber mres;
	ShiftRight(mres, mn, i);
	return mres;
}

ModNumber& operator <<= (ModNumber& mn, unsigned int i)
{

	ShiftLeft(mn, mn, i);
	return mn;
}
ModNumber& operator >>= (ModNumber& mn, unsigned int i)
{

	ShiftRight(mn, mn, i);
	return mn;
}

bool operator==(const ModNumber& ml, const ModNumber& mr)
{
	for (int i = 0; i < COUNTLL; i++)
		if (ml.num[i] != mr.num[i])
			return false;
		else
			;
	return true;
}

bool operator < (const ModNumber& ml, const ModNumber& mr)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
		if (ml.num[i] == mr.num[i])
			continue;
		else
			return ml.num[i] < mr.num[i];
	return false;
}

bool operator <= (const ModNumber& ml, const ModNumber& mr)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
		if (ml.num[i] == mr.num[i])
			continue;
		else
			return ml.num[i] < mr.num[i];
	return true;
}

bool operator >= (const ModNumber& ml, const ModNumber& mr)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
		if (ml.num[i] == mr.num[i])
			continue;
		else
			return ml.num[i] > mr.num[i];
	return true;
}

bool operator > (const ModNumber& ml, const ModNumber& mr)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
		if (ml.num[i] == mr.num[i])
			continue;
		else
			return ml.num[i] > mr.num[i];
	return false;
}

std::ostream& operator<<(std::ostream& out,const ModNumber& n)
{
	std::ios_base::fmtflags flags = out.flags();
	flags |= std::ios_base::right;
	flags |= std::ios_base::uppercase;
	out.setf(flags);
	if (flags & std::ios_base::hex)
		for (int i = COUNTLL - 1; i >= 0; i--)
		{
			out.fill('0');
			out.width(LLSIZE*2);
			out << n.num[i];
		}
	else if (flags & std::ios_base::oct)
	{
		llint mask = ~(~0ull >> 1);
		int count = COUNTLL - 1;
		int bitcounter = 0;
		int initial = (NSIZE % 3 == 0) ? 0 : 3-(NSIZE % 3);
		llint tmp = n.num[count];
		for (int i = 0; i < OctalStringLength; i++)
		{
			int digit = 0;
			for (int j = initial; j < 3; j++)
			{
				llint bits = tmp & mask;
				bits >>= (LLSIZE*8 - 3) + j;
				tmp <<= 1;
				digit |= bits;
				if (++bitcounter % (LLSIZE*8) == 0)
					tmp = n.num[--count];
			}
			initial = 0;
			out << digit;
		}
	}
	else if (flags & std::ios_base::dec)
	{
		std::string s = n.to_string_decimal_base();
		out << s;
	}
	return out;
}

std::istream& operator>>(std::istream& in, ModNumber& n)
{
	int base = 0;
	std::ios_base::fmtflags flags = in.flags();
	if (flags & std::ios_base::hex)
		base = 16;
	else if (flags & std::ios_base::oct)
		base = 8;
	else if (flags & std::ios_base::dec)
		base = 10;
	std::string s;
	in >> s;
	n = ModNumber::stomn(s, base);
	return in;
}

ModNumber operator+(const ModNumber &n, lint scalar)
{
	ModNumber mres(n);
	mres.AddAssignScalar(0, scalar);
	return mres;

}

ModNumber operator+ (const ModNumber& l, const ModNumber& r)
{
	ModNumber mres(l);
	lint* n = (lint*)r.num;
	for (int i = 0; i < COUNTL; i++)
		mres.AddAssignScalar(i, n[i]);
	return mres;
}

ModNumber &operator+= (ModNumber& l, const ModNumber& r)
{
	lint* n;
	ModNumber tmp;
	if (&l == &r) // Watch out for self-assignment
	{
		tmp = r;
		n = (lint*)tmp.num;
	}
	else
		n = (lint*)r.num;
	for (int i = 0; i < COUNTL; i++)
		l.AddAssignScalar(i, n[i]);
	return l;
}


ModNumber& ModNumber::AddAssignScalar(int lpos, lint scalar)
{
	if (lpos >= COUNTL)
		throw std::out_of_range("lpos out of range");
	llint res = 0;
	lint* n = (lint *)num;
	res = ((llint)n[lpos]) + scalar;
	n[lpos++] = ((lint*)&res)[0];
	while (((lint*)&res)[1] > 0ul && lpos < COUNTL)
	{
		res = ((llint)n[lpos]) + ((lint*)&res)[1];
		n[lpos++] = ((lint*)&res)[0];
	}

	return *this;
}

ModNumber& operator+=(ModNumber& n, lint scalar)
{
	return n.AddAssignScalar(0, scalar);
}

ModNumber operator* (const ModNumber& n, lint scalar)
{
	lint* ln = (lint*)n.num;
	ModNumber mres;
	lint* lres = (lint*)mres.num;
	int lim;

	for (lim = COUNTL - 1; lim >= 0; lim--)
	{
		if (ln[lim])
		{
			break;
		}
	}


	for (int i = 0; i <= lim; i++)
	{
		llint res = ((llint)ln[i]) * scalar;
		mres.AddAssignScalar(i, ((lint*)&res)[0]);
		if (i < COUNTL-1)
			mres.AddAssignScalar(i + 1, ((lint*)&res)[1]);
	}
	return mres;
}

ModNumber &operator*=(ModNumber& n, lint scalar)
{
	ModNumber mres = n * scalar;
	n = mres;
	return n;
}

ModNumber operator* (const ModNumber& l, const ModNumber& r)
{
	ModNumber res;
	lint* rlint = (lint*)r.num;
	int lim;
	for (lim = COUNTL - 1; lim >= 0; lim--)
	{
		if (rlint[lim])
		{
			break;
		}
	}
	for (int i = 0; i <= lim; i++)
	{
		res += (l * rlint[i]) << (LSIZE * 8 * i);
	}
	return res;
}

ModNumber& operator*=(ModNumber& l, const ModNumber& r)
{
	ModNumber mres = l * r;
	l = mres;
	return l;
}

std::tuple<ModNumber, lint> ModNumber::DivideAndModulo(lint scalar, bool onlyModulo) const
{
	if (scalar == 0)
		throw std::domain_error("Division by zero not allowed!");
	ModNumber res;
	const lint* nl = (const lint*)num;
	lint* resl = (lint*)res.num;
	llint tmp = 0ull;
	for (int i = COUNTL - 1; i >= 0; i--)
	{
		*((lint*)&tmp) = nl[i];
		if (scalar <= tmp)
		{
			if (!onlyModulo)
				resl[i] = (lint)(tmp / scalar);
			tmp %= scalar;
		}
		tmp <<= LSIZE * 8;

	}
	return std::make_tuple(res, ((lint *) & tmp)[1]);
}

ModNumber operator/ (const ModNumber& n, lint scalar)
{
	return std::get<0>(n.DivideAndModulo(scalar, false));
}

ModNumber &operator/= (ModNumber& n, lint scalar)
{
	n = std::get<0>(n.DivideAndModulo(scalar, false));
	return n;
}

lint operator% (const ModNumber& n, lint scalar)
{
	return std::get<1>(n.DivideAndModulo(scalar, true));
}

std::string ModNumber::to_string_hex_base() const
{
	std::string res;
	const int buflen = LLSIZE * 2;
	int width;
	width = buflen;
	res.reserve(buflen * COUNTLL + 1);
	for (int i = COUNTLL-1; i >= 0; i--)
	{
		std::stringstream mystrstr;
		mystrstr.setf(std::ios_base::right | std::ios_base::uppercase);
		mystrstr.fill('0');
		mystrstr.width(width);
		mystrstr <<std::hex << num[i];
		res.append(mystrstr.str());
	}
	return res;
}

std::string ModNumber::to_string_octal_base() const
{
	std::string res;
 	res.reserve(OctalStringLength);
	res.assign(OctalStringLength, '0');
	lint mask = 7;
	lint* pLint = (lint*)num;
	lint buf[2];
	llint* shiftBuf = (llint*) & buf;
	buf[0] = pLint[0];
	int tripleCount = 0;
	int wordCount = 0;
	for (int i = 0; i < NSIZE; i++)
	{
		if ((wordCount++ % (8 * LSIZE) ) == 0)
		{
			if (wordCount/(8 * LSIZE) + 1 < COUNTL)
				buf[1] = pLint[wordCount / (8 * LSIZE) + 1];
		}
		if ((tripleCount++ % 3) == 0)
		{
			lint numToPrint = buf[0] & mask;
			std::stringstream mystrstr;
			mystrstr << std::oct << numToPrint;
			res[(OctalStringLength) - (tripleCount / 3) - 1] = mystrstr.str()[0];
		}
		(*shiftBuf) >>= 1;
	}
	return res;
}

std::string ModNumber::to_string_decimal_base() const
{
	std::string res;
	res.reserve(DecimalStringLength);
	res.assign(DecimalStringLength, '0');
	ModNumber tmp = *this;
	for (int i = 0; i < DecimalStringLength; i++)
	{
		lint digit = tmp % 10ul;
		tmp /= 10ul;
		res[DecimalStringLength - i - 1] = '0' + (char)digit;
	}
	return res;
}

std::string ModNumber::to_string(const int base) const
{
	if (!(base == 8 || base == 10 || base == 16))
		throw std::invalid_argument("Only base 8, 10 and 16 are valid");
	switch (base)
	{
	case 16:
		return to_string_hex_base();
	case 10:
		return to_string_decimal_base();
	case 8:
		return to_string_octal_base();
	}
	std::string res;
	return res;
}

std::string ModNumber::AdjustStringLength(std::string s,size_t desiredLength)
{
	size_t len = s.length();
	if (len > desiredLength)
		throw std::domain_error("Value to large");
	if (len < desiredLength)
	{
		std::string tmp(desiredLength - len, '0');
		s = tmp + s;
	}
	return s;
}

unsigned char* strtoCharArray(std::string str)
{
	if (str.size() % 2 != 0)
		throw std::invalid_argument("Invalid input string, odd length");
	unsigned char* result = new unsigned char[str.size() / 2];
	for (int i = 0; i < str.size(); i += 2)
	{
		std::string tmp = str.substr(i, 2);
		int res = std::stoi(tmp, 0, 16);
		result[i / 2] = (char)res;

	}
	return result;
}

ModNumber ModNumber::stomn_hex_base(std::string s)
{
	llint n[COUNTLL] = {};
	s = AdjustStringLength(s, HexStringLength);
	for (char c : s)
	{
		if (!std::isxdigit(c))
			throw std::invalid_argument("Only hex digits allowed");

	}
	for (int i = 0; i < HexStringLength; i += LLSIZE*2)
	{
		std::string tmp = s.substr(i, LLSIZE*2);
		llint tmpll = std::stoull(tmp,nullptr,16);
		n[COUNTLL - (i / (LLSIZE * 2)) - 1] = tmpll;
	}
	return ModNumber(n);
}

ModNumber ModNumber::stomn_decimal_base(std::string s)
{
	ModNumber n;
	s = AdjustStringLength(s, DecimalStringLength);
	for (int i = 0; i < DecimalStringLength; i++)
	{
		if (!std::isdigit(s[i]))
			throw std::invalid_argument("Only digits allowed");
		lint digit = s[i] - '0';
		n *= (lint)10u;
		n += digit;
	}
	return n;
}

ModNumber ModNumber::stomn_octal_base(std::string s)
{
	llint n[COUNTLL] = {};
	llint buf = 0u;
	int bitCount = 0;
	int firstbits = (NSIZE % 3 == 0) ? 0 : 3 - (NSIZE %3);
	const llint mask = 4u;
	s = AdjustStringLength(s, OctalStringLength);
	for (int i = 0; i < OctalStringLength; i++)
	{
		if (!std::isdigit(s[i]) || s[i] == '8' || s[i] == '9')
			throw std::invalid_argument("Only octal digits allowed");
		llint digit = s[i] - '0';
		for (int j = 0; j < 3; j++)
		{
			buf <<= 1;
			llint res = digit & mask;
			res >>= 2;
			buf |= res;
			digit <<= 1;
			if (firstbits != 0)
			{
				firstbits--;
				continue;
			}
			bitCount++;
			if ((bitCount % (8 * LLSIZE) == 0))
			{
				n[COUNTLL - (bitCount / (8 * LLSIZE))] = buf;
				buf = 0u;
			}
		}
	}
	return ModNumber(n);
}


ModNumber ModNumber::stomn(std::string s, int base)
{
	if (!(base == 8 || base == 10 || base == 16))
		throw std::invalid_argument("Only base 8, 10 and 16 are valid");
	size_t len = s.length();
	if (len == 0)
	{
		return ModNumber(0ull);
	}
	int i;
	for (i = 0; i < len; i++)
		if (!std::isspace(s[i]))
			break;
	if (i > 0)
		s = s.substr(i);
	if (s[0] == '-')
		throw std::domain_error("Only positive numbers allowed");
	if (s[0] == '+')
		s = s.substr(1);
	switch (base)
	{
	case 16:
		return stomn_hex_base(s);
	case 10:
		return stomn_decimal_base(s);
	case 8:
		return stomn_octal_base(s);
	}
	throw std::invalid_argument("Invalid argument passed");
}

ModNumber ModNumber::gcd(const ModNumber& l,const ModNumber& r)
{
	ModNumber mzero;
	ModNumber mone(1ull);
	if (l == mzero || r == mzero)
		throw std::domain_error("Division by zero not allowed");
	if (l == mone)
		return mone;
	if (r == mone)
		return mone;
	if (l == r)
		return ModNumber(l);
	ModNumber lcopy = l;
	ModNumber rcopy = r;
	if (lcopy < rcopy)
		std::swap(lcopy, rcopy);
	ModNumber tmp = lcopy % rcopy;
	while (!(tmp == mone))
	{
		if (tmp == mzero)
			return ModNumber(rcopy);
		lcopy = rcopy;
		rcopy = tmp;
		tmp = lcopy % rcopy;
	}
	return ModNumber(tmp);

}

ModNumber ModNumber::lcm(const ModNumber& l,const ModNumber& r)
{
	ModNumber gcdRes = gcd(l, r);
	ModNumber lDivGcD = l / gcdRes;
	return lDivGcD * r;
}

unsigned short GetDoubleByteValue(const llint *pNum, int db)
{
	unsigned short* p = (unsigned short*)pNum;
	return p[db];
}

ModNumber ModNumber::sqrt() const
{
	ModNumber mzero;
	if (*this == mzero)
		return mzero;
	ModNumber mone(1ull);
	if (*this == mone)
		return mone;
	ModNumber mres;
	ModNumber mdivisor;
	ModNumber mremainder;
	unsigned int doubleByteCount = (GetByteCount(*this) - 1u) / 2u;
	for (int ui = doubleByteCount; ui >= 0; ui--)
	{
		unsigned short tmp = GetDoubleByteValue(this->num, ui);
		mremainder <<= 16;
		mdivisor <<= 8;
		mres <<= 8;
		unsigned short* pRemainder = (unsigned short*)mremainder.num;
		*pRemainder |= tmp;
		if (mremainder == mzero)
		{
			continue;
		}
		unsigned short counter = 1;
		mdivisor += 1;
		ModNumber divisorTimesCounter = mdivisor * counter;
		while (divisorTimesCounter < mremainder)
		{
			mdivisor += 1;
			divisorTimesCounter = mdivisor * ++counter;
		}
		if (divisorTimesCounter == mremainder)
		{
			mremainder = ModNumber(0ull);
		}
		else
		{
			mdivisor -= 1;
			divisorTimesCounter = mdivisor * --counter;
			mremainder -= divisorTimesCounter;
		}
		mdivisor += counter;
		mres += counter;
	}
	return mres;
}

bool operator == (ScaledNumber l, ScaledNumber r)
{
	return l.scale == r.scale && l.mn == r.mn;
}

ScaledNumber ScaledNumber::sqrt()
{
	ModNumber rt = this->mn.sqrt();
	return ScaledNumber(rt, this->scale / 2, true);
}

std::string ScaledNumber::to_string_hex_base() const
{
	std::string res;
	const int buflen = LLSIZE * 2;
	int width;
	width = buflen;
	res.reserve(buflen * COUNTLL + 1);
	for (int i = COUNTLL - 1; i >= 0; i--)
	{
		std::stringstream mystrstr;
		mystrstr.setf(std::ios_base::right | std::ios_base::uppercase);
		mystrstr.fill('0');
		mystrstr.width(width);
		mystrstr << std::hex << this->mn.num[i];
		res.append(mystrstr.str());
	}
	if (scale > 0)
	{
		size_t pos = buflen * COUNTLL - (scale * 2);
		res.insert(pos, ".");
	}
	return res;

}

std::tuple<unsigned int, unsigned int> ScaledNumber::calculateOctalStringLength() const
{
	return std::make_tuple<unsigned int, unsigned int>(((NCOUNT - scale) * 8) % 3 == 0 ? (NCOUNT - scale) * 8 / 3 : (NCOUNT - scale) * 8 / 3 + 1, (scale * 8 % 3 == 0) ? (scale * 8 / 3) : ((scale * 8) / 3) + 1);
}

std::string ScaledNumber::to_string_octal_base() const
{
	std::string res;
	res.reserve(OctalStringLength+3);
	lint mask = 7;
	int wordsToSkip = scale / LSIZE;
	int bitsToSkip = (scale % LSIZE) * 8;
	lint* pLint = (lint*)this->mn.num+wordsToSkip;
	lint buf[2]; 
	llint* shiftBuf = (llint*)&buf;
	buf[0] = pLint[0];
	buf[1] = pLint[1];
	int tripleCount = 0;
	std::tuple<unsigned int, unsigned int> octalLengths = calculateOctalStringLength();
	unsigned int digitsLeft = std::get<0>(octalLengths);
	unsigned int digitsToSkip = std::get<1>(octalLengths);
	res.assign(digitsToSkip + digitsLeft + 1, '0');
	res[digitsLeft] = '.';
	int wordCount = bitsToSkip;
	(*shiftBuf) >>= bitsToSkip;
	for (int i = scale * 8; i < NSIZE; i++)
	{
		if ((wordCount++ % (8 * LSIZE)) == 0)
		{
			if (wordCount / (8 * LSIZE) + 1 < (COUNTL - wordsToSkip))
				buf[1] = pLint[wordCount / (8 * LSIZE) + 1];
			else
				buf[1] = 0;
		}
		if ((tripleCount++ % 3) == 0)
		{
			lint numToPrint = buf[0] & mask;
			std::stringstream mystrstr;
			mystrstr << std::oct << numToPrint;
			res[(digitsLeft)-(tripleCount / 3) -1] = mystrstr.str()[0];
		}
		(*shiftBuf) >>= 1;
	}


	buf[1] = pLint[0];
	if (wordsToSkip > 0)
	{
		buf[0] = pLint[-1];
	}
	else
		buf[0] = 0;
	(*shiftBuf) <<= (LSIZE * 8) - bitsToSkip;

	wordCount = ((8 * LSIZE) - bitsToSkip);
	tripleCount = 0;
	for (int i = 0; i < scale * 8; i++)
	{
		if ((wordCount++ % (8 * LSIZE)) == 0)
		{
			if (wordCount < (wordsToSkip * 8 * LSIZE))
				buf[0] = pLint[-1 -(wordCount/(8 * LSIZE))];
			else
				buf[0] = 0;
		}
		if ((tripleCount++ % 3) == 0)
		{
			lint numToPrint = (buf[1] >> (32-3)) & mask;
			std::stringstream mystrstr;
			mystrstr << std::oct << numToPrint;
			res[digitsLeft+(tripleCount / 3) + 1] = mystrstr.str()[0];
		}
		(*shiftBuf) <<= 1;

	}
	return res;
}

unsigned int ScaledNumber::calculateDecimalStringLengthLeft() const
{
	return static_cast<int>(std::ceil((NSIZE - scale * 8) * 0.30102999566398119521373889472449)); // log(2)
}

std::string ScaledNumber::to_string_decimal_base() const
{
	std::string res;
	const int IntegerStringLength = calculateDecimalStringLengthLeft();
	res.reserve(DecimalStringLength + 3);
	res.assign(IntegerStringLength, '0');
	ModNumber tmp = this->mn >> scale * 8;
	for (int i = 0; i < IntegerStringLength; i++)
	{
		lint digit = tmp % 10ul;
		tmp /= 10ul;
		res[IntegerStringLength - i - 1] = '0' + (char)digit;
	}
	if (scale > 0)
	{
		res.append(".");
		ModNumber tmp(this->mn);
		ModNumber divisor(1ull);
		for (int i = 0; i < scale * 2; i++)
			divisor *= 10ul;
		tmp *= divisor;
		tmp >>= scale * 8;
		tmp %= divisor;
		for (int i = 0; i < scale * 2; i++)
		{
			divisor /= 10ul;
			ModNumber resTmp = tmp / divisor;
			lint digit = resTmp % 10ul;
			res.append(1, '0' + (char)digit);
		}
	}
	return res;

}


std::string ScaledNumber::to_string(int base) const
{
	if (!(base == 8 || base == 10 || base == 16))
		throw std::invalid_argument("Only base 8, 10 and 16 are valid");
	switch (base)
	{
	case 16:
		return to_string_hex_base();
	case 10:
		return to_string_decimal_base();
	case 8:
		return to_string_octal_base();
	}
	std::string res;
	return res;

}




ModNumber MultGroupMod::Mult(const ModNumber& l, const ModNumber& r) const
{
	ModNumber res;
	ModNumber lMod = l % n;
	ModNumber rMod = r % n;
	lint* rlint = (lint*)rMod.num;
	int lim;
	for (lim = COUNTL - 1; lim >= 0; lim--)
	{
		if (rlint[lim])
		{
			break;
		}
	}
	for (int i = 0; i <= lim; i++)
	{
		ModNumber tmp = lMod * rlint[i];
		for (int j = 0; j < i; j++)
		{
			tmp %= n;
			tmp <<= LSIZE * 8;
		}
		tmp %= n;
		res += tmp;
	}
	res %= n;
	return res;
}

ModNumber MultGroupMod::Kwad(const ModNumber& x) const
{
	const ModNumber &l = x;
	const ModNumber &r = x;
	return Mult(l, r);
}

ModNumber MultGroupMod::Exp(const ModNumber& x, const ModNumber& e) const
{
	ModNumber res(1ull);
	ModNumber xMod = x % n;
	int top;
	for (top = COUNTLL - 1; top >= 0; top--)
		if (e.num[top])
			break;
	for (int i = 0; i <= top; i++)
	{
		llint mask = 1ull;
		llint tmp = e.num[i];
		for (int j = 0; j < LLSIZE * 8 && (tmp || i < top); j++)
		{
			if (tmp & mask)
			{
				res = Mult(res, xMod);
			}
			xMod = Kwad(xMod);
			tmp >>= 1;
		}
	}
	return res;
}

ModNumber MultGroupMod::Add(const ModNumber& l, const ModNumber& r) const
{
	ModNumber lMod = l % n;
	ModNumber rMod = r % n;
	return (lMod + rMod) % n;
}

ModNumber MultGroupMod::Diff(const ModNumber& l, const ModNumber& r) const
{
	ModNumber lMod = l % n;
	ModNumber rMod = r % n;
	if (lMod == rMod)
		return ModNumber(0ull);
	if (lMod > rMod)
		return lMod - rMod;
	else
		return n - rMod + lMod;
}

ModNumber MultGroupMod::Inverse(const ModNumber& x) const
{
	ModNumber mzero;
	ModNumber mone(1ull);
	if (x == mzero)
		throw std::domain_error("Zero does not have an inverse");
	if (x == mone)
		return mone;
	if (x == n)
		throw std::domain_error("Zero does not have an inverse");
	if (x == mone)
		return mone;
	ModNumber r = x % n;
	ModNumber l = n;
	std::list<ModNumber> divisors;
	ModNumber modRes = l;
	ModNumber divRes = DivideAndModulo(modRes,l,r);
	while (!(modRes == mone))
	{
		if (modRes == mzero)
			throw std::domain_error("Numbers are not relative prime, so there is no inverse.");
		divisors.push_back(divRes);
		l = r;
		r = modRes;
		modRes = l;
		divRes = DivideAndModulo(modRes,l, r);
	}
	divisors.push_back(divRes);
	ModNumber tmp1 = mzero;
	ModNumber tmp2 = mone;
	for (auto it = divisors.rbegin(); it != divisors.rend(); it++)
	{
		ModNumber tmp = tmp2;
		ModNumber product = Mult(tmp2, *it);
		tmp2 = Diff(tmp1, product);
		tmp1 = tmp;

	}
	return tmp2;
}



unsigned char *ConvertEndianess(const unsigned char* p, unsigned int cb)
{
	unsigned char* res = new unsigned char[cb];
	for (unsigned int i = 0; i < cb; i++)
		res[i] = p[cb - i - 1];
	return res;
}

unsigned char* ConvertEndianess(const ModNumber& m, unsigned int cb)
{
	unsigned char* p = (unsigned char *)m.num;
	unsigned int n = cb > 0 ? cb : GetByteCount(m);
	return ConvertEndianess(p, n);
}

ModNumber GetLeftMostBytes(const ModNumber& m, unsigned int leftBytes)
{
	unsigned int n = GetByteCount(m);
	unsigned char* p = (unsigned char*)m.num;
	ModNumber res(p + (n - leftBytes), leftBytes);
	return res;
}
unsigned int GetByteCount(const ModNumber& mn)
{
	unsigned char* p = (unsigned char *)mn.num;
	for (int i = NCOUNT - 1; i >= 0; i--)
		if (p[i])
			return i + 1;
	return 0;
}

unsigned char* CopyKeyPart(const ModNumber& mn, unsigned int cbsize, unsigned char* pDest)
{
	unsigned char* pKey = ConvertEndianess((unsigned char*)(mn.num), cbsize);
	for (unsigned int i = 0; i < cbsize; i++)
		pDest[i] = pKey[i];
	delete[] pKey;
	return pDest + cbsize;
}

ModNumber GetPKCS1Mask(const ModNumber& m, bool stable, int modulusSize)
{
	unsigned long keyByteSize = modulusSize;
	unsigned long mSize = GetByteCount(m);
	unsigned long mCount = mSize / LLSIZE;
	if (keyByteSize - 11u < mSize)
		throw std::domain_error("Message size greater than Key Byte size minus 11");
	ModNumber res;
	srand((unsigned int)time(0));
	unsigned long padSize = keyByteSize - mSize - 3;
	unsigned long totalBytesLeft = keyByteSize % LLSIZE;
	unsigned long totalNumWords = keyByteSize / LLSIZE;
	if (totalBytesLeft > 1)
		totalNumWords++;
	llint tmp = stable ? 0x0001 : 0x0002;
	unsigned long totalBytesShift = totalBytesLeft;
	if (totalBytesLeft < 2)
		totalBytesShift += 8;
	for (unsigned int i = 0; i < totalBytesShift - 2; i++)
	{
		tmp <<= 8;
		unsigned char mask = stable ? 0xFF : ((unsigned char)rand() % 0xFF) + 1u;
		tmp |= mask;
	}
	res.num[totalNumWords - 1] = tmp;
	unsigned long padLeft = padSize - (totalBytesShift - 2);
	unsigned long padLeftCount = padLeft / LLSIZE;
	unsigned long padLeftOver = padLeft % LLSIZE;
	for (unsigned int i = 0; i < padLeftCount; i++)
	{
		tmp = 0;
		for (int j = 0; j < LLSIZE; j++)
		{
			tmp <<= 8;
			unsigned char mask = stable ? 0xFF : ((unsigned char)rand() % 0xFF) + 1u;
			tmp |= mask;
		}
		res.num[totalNumWords - i - 2] = tmp;
	}
	tmp = 0;
	for (unsigned int j = 0; j < padLeftOver; j++)
	{
		unsigned char mask = stable ? 0xFF : ((unsigned char)rand() % 0xFF) + 1u;
		tmp |= mask;
		tmp <<= 8;
	}
	tmp <<= (LLSIZE - padLeftOver - 1) * 8; // have to add size parameter !
	tmp |= m.num[mCount];
	res.num[totalNumWords - padLeftCount - 2] = tmp;
	for (unsigned int i = 0; i < mCount; i++)
	{
		res.num[i] = m.num[i];
	}
	return res;
}

std::tuple<unsigned char *, int> RemovePKCS1Mask(const std::string asnString)
{
	unsigned char* pMaskedNumberBigEndian = strtoCharArray(asnString);
	unsigned char* pMaskedNumber = ConvertEndianess(pMaskedNumberBigEndian, (unsigned int)asnString.length()/2);
	delete[] pMaskedNumberBigEndian;
	int i;
	for (i = (int)asnString.length()/2 - 1; i >= 0; i--)
	{
		if (pMaskedNumber[i])
			break;
	}
	if (pMaskedNumber[i+1] != 0x00u)
		throw std::domain_error("Not a valid PKCS1 Mask");
	if (pMaskedNumber[i] == 0x01u)
	{
		while (pMaskedNumber[--i] == 0xFF && i >= 0)
			;
	}
	else if (pMaskedNumber[i] == 0x02u)
	{
		while (pMaskedNumber[i] != 0x00u && i >= 0)
			i--;
	}
	else
		throw std::domain_error("Not a valid PKCS1 Mask");
	if (pMaskedNumber[i] != 0x00u)
		throw std::domain_error("Not a valid PKCS1 Mask");
	return std::make_tuple(pMaskedNumber, i);

}

std::tuple<ASNElementType,unsigned int, unsigned int> ReadASNElement(unsigned char* p, unsigned int i)
{
	if (i <= 0)
		throw std::domain_error("Not a valid BER encoding!");
	switch (p[i] >> 6)
	{
	case 0:
	{
		switch (p[i] >> 5)
		{
		case 0:
		case 1:
			{
				unsigned char mask = 0x1Fu;
				unsigned char masked = p[i] & mask;
				switch (masked)
				{
					case (int)ASNElementType::SEQUENCE:
					{
						switch (p[i-1] >> 7)
						{
							case 0:
							{
								unsigned char mask = 0x7F;
								unsigned char masked = p[i - 1] & mask;
								return std::make_tuple(ASNElementType::SEQUENCE, masked, i - 2);
							}
							case 1:
							{
								unsigned char lLen = p[i - 1] & 0x7F;
								unsigned int byteLen = 0;
								for (int j = 0; j < lLen; j++)
								{
									byteLen |= (p[i - lLen - 1 + j] << (j * 8));
								}
								return std::make_tuple(ASNElementType::SEQUENCE, byteLen, i - lLen - 2);
							}
						}
					}
					case (int)ASNElementType::OBJECT_IDENTIFIER:
					{
						switch (p[i - 1] >> 7)
						{
						case 0:
						{
							unsigned char mask = 0x7F;
							unsigned char masked = p[i - 1] & mask;
							return std::make_tuple(ASNElementType::OBJECT_IDENTIFIER, masked, i - 2);
						}
						default:
							throw std::domain_error("Not a short length specifier!");
						}
					}
					case (int)ASNElementType::NULL_VALUE:
						if (p[i - 1] != 0)
							throw std::domain_error("Not a valid NULL Object");
						return std::make_tuple(ASNElementType::NULL_VALUE, p[i - 1], i - 2);
					case (int)ASNElementType::OCTET_STRING:
						switch (p[i - 1] >> 7)
						{
						case 0:
						{
							unsigned char mask = 0x7F;
							unsigned char masked = p[i - 1] & mask;
							return std::make_tuple(ASNElementType::OCTET_STRING, masked, i - 2);
						}
						default:
							throw std::domain_error("Not a short length specifier!");
						}
					case (int)ASNElementType::INTEGER_VALUE:
					{
						switch (p[i - 1] >> 7)
						{
						case 0:
						{
							unsigned char mask = 0x7F;
							unsigned char masked = p[i - 1] & mask;
							return std::make_tuple(ASNElementType::INTEGER_VALUE, masked, i - 2);
						}
						default:
							throw std::domain_error("Not a short length specifier!");
						}
					}
					default:
						throw std::domain_error("Not an expected ASN Type");

				}
			}
			default:
				throw std::domain_error("Not a constructed ASN.1 type!");
		}
	}
	default:
		throw std::domain_error("Not a native ASN.1 type!");

	}

}

std::string CreateBERASNStringForDSASignature(std::list<std::string> content)
{
	std::string ASNString;
	std::list<std::string>::iterator myIterator = content.begin();
	std::string r = *myIterator++;
	std::string s = *myIterator++;
	std::stringstream strStream;
	strStream.fill('0');
	strStream << std::setw(2) <<std::hex << ((unsigned int)ASNElementType::SEQUENCE | 0x20);
	
	unsigned int len = (unsigned char)(r.length() + s.length());
	unsigned int extra = 2 + 2;
	if (len + extra > 0x7F)
	{
		unsigned int tmpLen = len + extra;
		unsigned int numLenBytes = 1;
		while ((tmpLen >>= 8) != 0)
			numLenBytes++;
		strStream << std::setw(2) << std::hex << (0x80 | (char)numLenBytes);
		for (unsigned int i = 0; i < numLenBytes; i++)
			strStream << std::setw(2) << std::hex << ((len + extra) >> (numLenBytes - i - 1) * 8);
	}
	else
		strStream << std::setw(2) << std::hex << len + extra;
	

	strStream << std::setw(2) << std::hex << (unsigned int)(ASNElementType::INTEGER_VALUE);

	strStream << std::setw(2) << std::hex << r.length();
	for (int i = 0; i < r.length(); i++)
		strStream << std::setw(2) << std::hex << (unsigned int)(unsigned char)r[i];
	
	strStream << std::setw(2) << std::hex << (unsigned int)ASNElementType::INTEGER_VALUE;
	strStream << std::setw(2) << std::hex << s.length();
	for (int i = 0; i < s.length(); i++)
		strStream << std::setw(2) << std::hex << (unsigned int)(unsigned char)s[i];
	return strStream.str();
}

ModNumber CreateBERASNString(std::list<std::string> content)
{
	std::string result;
	std::string outerASN;
	std::string innerASN;
	std::string encodedOid;
	std::list<std::string>::iterator myIterator = content.begin();
	std::string plainOid = *myIterator++;
	std::string hash = *myIterator++;
	std::stringstream oidStream(plainOid, std::ios::in);
	int countSeparators = 0;
	for (std::string::iterator it = plainOid.begin(); it != plainOid.end(); it++)
		if (*it == '.')
			countSeparators++;
	std::list<unsigned int> oidNumbers;
	unsigned int number;
	char dot;
	oidStream >> number;
	oidNumbers.push_back(number);
	for (int i = 0; i < countSeparators; i++)
	{
		oidStream >> dot;
		oidStream >> number;
		oidNumbers.push_back(number);
	}
	std::list<unsigned int>::iterator numberIterator = oidNumbers.begin();
	unsigned int first = *numberIterator++ * 40;
	first += *numberIterator++;
	encodedOid.append(1, first);
	for (; numberIterator != oidNumbers.end(); numberIterator++)
	{
		unsigned int oidPart = *numberIterator;
		unsigned char divisor = 0x80;
		unsigned char mask = 0x7F;
		std::list<char> oidCharList;
		unsigned char oidChar = (oidPart % divisor);
		oidCharList.push_back(oidChar);
		oidPart >>= 7;
		while (oidPart > 0)
		{
			oidChar = (oidPart & mask) | divisor;
			oidCharList.push_back(oidChar);
			oidPart >>= 7;
		}
		for (std::list<char>::reverse_iterator it = oidCharList.rbegin(); it != oidCharList.rend(); it++)
			encodedOid.append(1, *it);
	}

	innerASN.append(1, (unsigned char)ASNElementType::OBJECT_IDENTIFIER);
	innerASN.append(1, (unsigned char)encodedOid.length());
	innerASN.append(encodedOid);
	innerASN.append(1, (unsigned char)ASNElementType::NULL_VALUE);
	innerASN.append(1, (unsigned char)'\0');
	outerASN.append(1, (unsigned char)ASNElementType::SEQUENCE | 0x20);
	outerASN.append(1, (unsigned char)innerASN.length());
	outerASN.append(innerASN);
	outerASN.append(1, (unsigned char)ASNElementType::OCTET_STRING);
	outerASN.append(1, (unsigned char)hash.length());
	outerASN.append(hash);
	result.append(1, (unsigned char)ASNElementType::SEQUENCE | 0x20);
	result.append(1, (unsigned char)(outerASN.length()));
	result.append(outerASN);
	unsigned char* resLittleEndian = ConvertEndianess((const unsigned char*)result.c_str(), (unsigned int)result.length());
	ModNumber res(resLittleEndian, (unsigned int)result.length());
	delete[] resLittleEndian;
	return res;

}

std::list<std::string> ParseBERASNString(const std::string asnString)
{
	unsigned char* pMaskedNumber;
	unsigned char* pMaskedNumberBigEndian = strtoCharArray(asnString);
	pMaskedNumber = ConvertEndianess(pMaskedNumberBigEndian, (unsigned int)asnString.length());
	delete[] pMaskedNumberBigEndian;
	return ParseBERASNString(pMaskedNumber, asnString.length());
}
std::list<std::string> ParseBERASNString(unsigned char * pMaskedNumber, size_t size)
{
	std::list<std::string> result;
	int i;
	for (i = (int)size-1; i > 0; i--)
	{
		if (pMaskedNumber[i])
			break;
	}
	std::tuple<ASNElementType,unsigned int, unsigned int> ASNElement1  = ReadASNElement(pMaskedNumber,i);
	if (std::get<0>(ASNElement1) == ASNElementType::SEQUENCE)
	{
		std::tuple<ASNElementType, unsigned int, unsigned int> ASNElement2 = ReadASNElement(pMaskedNumber,std::get<2>(ASNElement1));
		if (std::get<0>(ASNElement2) == ASNElementType::SEQUENCE)
		{
			std::tuple<ASNElementType, unsigned int, unsigned int> ASNElement3 = ReadASNElement(pMaskedNumber, std::get<2>(ASNElement2));
			if (std::get<0>(ASNElement3) == ASNElementType::OBJECT_IDENTIFIER)
			{
				unsigned int len = std::get<1>(ASNElement3);
				unsigned int index = std::get<2>(ASNElement3);
				char c = pMaskedNumber[index];
				std::stringstream stringStr;
				stringStr << (int)c / 40;
				stringStr << '.';
				stringStr << (int)c % 40;
				stringStr << '.';
				llint number = 0;
				for (unsigned int i = 1; i < len; i++)
				{
					unsigned char mask = 0x80;
					c = pMaskedNumber[index - i];
					number <<= 7;
					number |= c & ((unsigned char)~mask);
					if (!(c & mask))
					{
						stringStr << number;
						stringStr << '.';
						number = 0;
					}
				}
				std::string resultStr = stringStr.str();
				result.push_back(resultStr);
			}
			std::tuple<ASNElementType, unsigned int, unsigned int> ASNElement4 = ReadASNElement(pMaskedNumber, std::get<2>(ASNElement3) - std::get<1>(ASNElement3));
			std::tuple<ASNElementType, unsigned int, unsigned int> ASNElement5 = ReadASNElement(pMaskedNumber, std::get<2>(ASNElement4));
			if (std::get<0>(ASNElement5) == ASNElementType::OCTET_STRING)
			{
				unsigned int len = std::get<1>(ASNElement5);
				unsigned int index = std::get<2>(ASNElement5);
				std::string resultStr;
				for (unsigned int i = 0; i < len; i++)
				{
					resultStr.append(1, pMaskedNumber[index - i]);
				}
				result.push_back(resultStr);
			}
		}
		else if (std::get<0>(ASNElement2) == ASNElementType::INTEGER_VALUE)
		{
			unsigned int len = std::get<1>(ASNElement2);
			unsigned int index = std::get<2>(ASNElement2);
			std::string resultStr;
			for (unsigned int i = 0; i < len; i++)
			{
				resultStr.append(1, pMaskedNumber[index - i]);
			}
			result.push_back(resultStr);
			std::tuple<ASNElementType, unsigned int, unsigned int> ASNElement3 = ReadASNElement(pMaskedNumber, std::get<2>(ASNElement2)-std::get<1>(ASNElement2));
			if (std::get<0>(ASNElement3) == ASNElementType::INTEGER_VALUE)
			{
				unsigned int len = std::get<1>(ASNElement3);
				unsigned int index = std::get<2>(ASNElement3);
				std::string resultStr;
				for (unsigned int i = 0; i < len; i++)
				{
					resultStr.append(1, pMaskedNumber[index - i]);
				}
				result.push_back(resultStr);

			}
		}
	}
	delete[] pMaskedNumber;
	return result;
}

ModNumber RSA::Encrypt(const ModNumber& m) const
{
	ModNumber masked = GetPKCS1Mask(m);
	MultGroupMod mgm(Modulus);
	return mgm.Exp(masked, pubExp);
}

ModNumber RSA::Decrypt(const ModNumber& c) const
{
	MultGroupMod mgmp(Prime1);
	MultGroupMod mgmq(Prime2);
	MultGroupMod mgmn(Modulus);
	ModNumber m1;
	ModNumber m2;
	std::thread th1([&m1, &mgmp, &c, this]() {m1 = mgmp.Exp(c, Exp1); });
	std::thread th2([&m2, &mgmq, &c, this]() {m2 = mgmq.Exp(c, Exp2); });
	th1.join();
	th2.join();
	ModNumber diff = mgmp.Diff(m1,m2);
	ModNumber h = mgmp.Mult(Coefficient, diff);
	ModNumber hq = mgmn.Mult(h,Prime2);
	ModNumber res =  mgmn.Add(m2, hq);
	std::string maskedResult = res.to_string(16);
	std::tuple<unsigned char *, int> result = RemovePKCS1Mask(maskedResult);
	ModNumber resultMn = ModNumber(std::get<0>(result), std::get<1>(result));
	delete[] std::get<0>(result);
	return resultMn;
}

ModNumber RSA::DecryptSignature(const ModNumber signature) const
{
	MultGroupMod mgm(Modulus);
	ModNumber decryptedSignature = mgm.Exp(signature, pubExp);
	std::string decryptedSignatureStr = decryptedSignature.to_string(16);
	std::tuple<unsigned char*,int> pRemovedMask = RemovePKCS1Mask(decryptedSignatureStr);
	std::list<std::string> result = ParseBERASNString(std::get<0>(pRemovedMask), (size_t)std::get<1>(pRemovedMask));
	std::list<std::string>::iterator resultIterator = result.begin();
	resultIterator++;
	ModNumber hashBigEndian((const unsigned char *)resultIterator->c_str());
	unsigned int hashLen = (unsigned int)resultIterator->size();
	unsigned char* pHashBigEndian = (unsigned char*)hashBigEndian.num;
	unsigned char* pHashLittleEndian = ConvertEndianess(pHashBigEndian, hashLen);
	ModNumber retValue(pHashLittleEndian, hashLen);
	delete[] pHashLittleEndian;
	return retValue;
}

ModNumber RSA::EncryptSignature(std::string hashBigEndian, std::string hashOid) const
{
	std::list<std::string> myList;
	myList.push_back(hashOid);
	myList.push_back(hashBigEndian);
	ModNumber unmaskedResult = CreateBERASNString(myList);
	ModNumber maskedResult = GetPKCS1Mask(unmaskedResult, true);
	MultGroupMod mgmp(Prime1);
	MultGroupMod mgmq(Prime2);
	MultGroupMod mgmn(Modulus);
	ModNumber m1;
	ModNumber m2;
	std::thread th1([&m1, &mgmp, &maskedResult, this]() {m1 = mgmp.Exp(maskedResult, Exp1); });
	std::thread th2([&m2, &mgmq, &maskedResult, this]() {m2 = mgmq.Exp(maskedResult, Exp2); });
	th1.join();
	th2.join();
	ModNumber diff = mgmp.Diff(m1, m2);
	ModNumber h = mgmp.Mult(Coefficient, diff);
	ModNumber hq = mgmn.Mult(h, Prime2);
	ModNumber res = mgmn.Add(m2, hq);
	return res;
}

ModNumber DSA::CalcR(const ModNumber& mk) const
{
	MultGroupMod mgmp(P);
	return mgmp.Exp(g, mk) % Q;
}

std::string DSA::Sign(unsigned char* hash, unsigned int hashLen, bool DerEncoded) const
{
	return CalculateDSASignature(Q, x, hash, hashLen, DerEncoded);
}

std::tuple<ModNumber, ModNumber, ModNumber> DSACalculateU1U2Mr(const ModNumber& q, unsigned int qLen, unsigned char* hash, unsigned int hashLen, std::string signature, bool DerEncoded)
{
	unsigned char* pHashLittleEndian = ConvertEndianess(hash, hashLen);
	ModNumber mHash(pHashLittleEndian, hashLen);
	delete[] pHashLittleEndian;
	if (hashLen > qLen)
		mHash = GetLeftMostBytes(mHash, qLen);
	std::list<std::string> results;
	if (DerEncoded)
	{
		results = ParseBERASNString(signature);
	}
	else
	{
		results.push_back(signature.substr(0, signature.length() / 2));
		results.push_back(signature.substr(signature.length() / 2, signature.length() / 2));
	}

	std::list<std::string>::iterator myListIterator = results.begin();
	std::string r = *myListIterator++;
	std::string s = *myListIterator++;
	unsigned char* rLittleEndian = ConvertEndianess((const unsigned char*)r.c_str(), (unsigned int)r.length());
	unsigned char* sLittleEndian = ConvertEndianess((const unsigned char*)s.c_str(), (unsigned int)s.length());
	ModNumber mr(rLittleEndian, (unsigned int)r.length());
	ModNumber ms(sLittleEndian, (unsigned int)s.length());
	delete[] rLittleEndian;
	delete[] sLittleEndian;
	if (!(mr < q && ms < q))
		throw std::domain_error("Invalid signature");
	MultGroupMod mgmn(q);
	ModNumber sInverse = mgmn.Inverse(ms);
	ModNumber u1;
	std::thread th1([&u1, &mgmn, &mHash, &sInverse] {u1 = mgmn.Mult(mHash, sInverse); });

	ModNumber u2;
	std::thread th2([&u2, &mgmn, mr, sInverse] {u2 = mgmn.Mult(mr, sInverse); });
	th1.join();
	th2.join();
	return std::make_tuple(u1, u2, mr);
}


bool DSA::Verify(unsigned char *hash, unsigned int hashLen, std::string signature, bool DerEncoded) const
{
	MultGroupMod mgm(P);
	ModNumber mv1;
	unsigned int bcQ = GetByteCount(Q);
	std::tuple<ModNumber, ModNumber, ModNumber> u1u2mr = DSACalculateU1U2Mr(Q, bcQ, hash, hashLen, signature, DerEncoded);
	ModNumber u1 = std::get<0>(u1u2mr);
	ModNumber u2 = std::get<1>(u1u2mr);
	ModNumber mr = std::get<2>(u1u2mr);
	std::thread th3([&mv1, &mgm, this, &u1] {mv1 = mgm.Exp(g, u1); });
	ModNumber mv2;
	std::thread th4([&mv2, &mgm, this, &u2] {mv2 = mgm.Exp(y, u2); });
	th3.join();
	th4.join();
	ModNumber mv = mgm.Mult(mv1, mv2) % Q;
	return mv == mr;
}

bool operator == (const ECPoint& l, const ECPoint& r)
{
	if (l.IsAtInfinity && r.IsAtInfinity)
		return true;
	if (l.IsAtInfinity || r.IsAtInfinity)
		return false;
	return l.x == r.x && l.y == r.y;
}
ModNumber EC::CalculateRhs(const ModNumber& x) const
{
	ModNumber xPower3 = mgm.Exp(x, ModNumber(3ull));
	ModNumber aTimesX = mgm.Mult(x, a);
	ModNumber rhs = mgm.Add(mgm.Add(xPower3, aTimesX), ModNumber(b));
	return rhs;
}

bool EC::IsOnCurve(const ECPoint& p) const
{
	if (p.IsAtInfinity)
		return true;
	ModNumber yKwad = mgm.Kwad(p.y);
	return yKwad == CalculateRhs(p.x);
}

ModNumber EC::CalculateY(const ModNumber& x) const
{
	return CalculateRhs(x).sqrt();
}


ECPoint EC::Add(ECPoint p, ECPoint q) const
{
	if (!IsOnCurve(p) || !IsOnCurve(q))
		throw std::domain_error("One of the points is not on the curve");
	ECPoint result;
	if (p == q)
		return Times2(p);
	if (p.IsAtInfinity)
		return q;
	if (q.IsAtInfinity)
		return p;
	if (p.x == q.x)
	{
		result.IsAtInfinity = true;
		return result;
	}
	ModNumber mzero;
	ModNumber xDelta = mgm.Diff(q.x, p.x);
	ModNumber yDelta = mgm.Diff(q.y, p.y);
	ModNumber xDeltaInverse = mgm.Inverse(xDelta);
	ModNumber lambda = mgm.Mult(yDelta, xDeltaInverse);
	result.x = mgm.Diff(mgm.Diff(mgm.Kwad(lambda), p.x),q.x);
	result.y = mgm.Diff(mzero,mgm.Add(p.y, mgm.Mult(lambda, mgm.Diff(result.x, p.x))));
	return result;
}

ECPoint EC::Times2(ECPoint p) const
{
	if (!IsOnCurve(p))
		throw std::domain_error("Fhe point is not on the curve");
	if (p.IsAtInfinity)
		return p;
	ECPoint result;
	ModNumber mzero;

	if (p.y == mzero)
	{
		result.IsAtInfinity = true;
		return result;
	}
	ModNumber xpSquared = mgm.Kwad(p.x);
	ModNumber xpSquaredTimes3 = mgm.Mult(xpSquared, ModNumber(3ull));
	ModNumber xpSquaredTime3PlusA = mgm.Add(xpSquaredTimes3, a);
	ModNumber ypTimes2 = mgm.Mult(p.y, ModNumber(2ull));
	ModNumber ypTimes2Inverse = mgm.Inverse(ypTimes2);
	ModNumber lambda = mgm.Mult(xpSquaredTime3PlusA, ypTimes2Inverse);
	ModNumber xpTimes2 = mgm.Mult(p.x, ModNumber(2ull));
	ModNumber lambdaSquaredMinusXpTimes2 = mgm.Diff(mgm.Kwad(lambda), xpTimes2);
	ModNumber xrMinusXp = mgm.Diff(lambdaSquaredMinusXpTimes2, p.x);
	ModNumber lambdaTimesXrMinusXp = mgm.Mult(lambda, xrMinusXp);
	ModNumber yPPluslambdaTimesXrMinusXp = mgm.Add(p.y, lambdaTimesXrMinusXp);
	result.x = lambdaSquaredMinusXpTimes2;
	result.y = mgm.Diff(ModNumber(), yPPluslambdaTimesXrMinusXp);
	return result;
}


ECPoint EC::Mult(ECPoint p, ModNumber n) const
{
	if (!IsOnCurve(p))
		throw std::domain_error("The point is not on the curve");

	if (p.IsAtInfinity)
		return p;
	ECPoint pCopy = p;
	ModNumber mzero;
	ECPoint result;
	result.IsAtInfinity = true;
	llint mask = 1ull;
	while (n > mzero)
	{
		llint first = n.num[0];
		if (first & mask)
		{
			result = Add(result, pCopy);
		}
		pCopy = Times2(pCopy);
		n >>= 1;
	}
	return result;
}

std::string DSABase::CalculateDSASignature(ModNumber q, ModNumber x, unsigned char* hash, unsigned int hashLen, bool DerEncoded) const
{
	unsigned char* pHashLittleEndian = ConvertEndianess(hash, hashLen);
	ModNumber mHash(pHashLittleEndian, hashLen);
	delete[] pHashLittleEndian;
	unsigned int nLen = GetByteCount(q);
	if (hashLen > nLen)
		mHash = GetLeftMostBytes(mHash, nLen);
	llint k[COUNTLL] = {};
	unsigned char* p = (unsigned char*)k;
	srand((unsigned int)time(0));

	ModNumber r;
	ModNumber s;
	ModNumber mk;
	ModNumber mzero;

	do
	{
		do
		{
			for (unsigned int i = 0; i < nLen; i++)
			{
				p[i] = (unsigned char)(rand() % 0x100);
			}
			unsigned char* pq = (unsigned char*)&q.num;
			p[nLen - 1] &= pq[nLen - 1];
			mk = ModNumber(k);
			if (mk == mzero)
			{
				mk += rand() + 1;
			}
			while (mk >= q)
			{
				p[nLen - 1] -= 1;
				mk = ModNumber(k);
			}
			r = CalcR(mk);
		} while (r == mzero);
		ModNumber kInverse;
		MultGroupMod mgmn(q);
		std::thread th1([&mgmn, &kInverse, &mk] {kInverse = mgmn.Inverse(mk); });
		ModNumber hashPlusXR;
		std::thread th2([&mgmn, &x, &r, &mHash, &hashPlusXR] {
			ModNumber xR = mgmn.Mult(x, r);
			hashPlusXR = mgmn.Add(mHash, xR);
			});
		th1.join();
		th2.join();
		s = mgmn.Mult(kInverse, hashPlusXR);
	} while (s == mzero);
	if (!(s < q && r < q))
		throw std::domain_error("Wrong signature");
	unsigned char* rBigEndian = ConvertEndianess(r, nLen);
	unsigned char* sBigEndian = ConvertEndianess(s, nLen);
	if (DerEncoded)
	{
		std::list<std::string> myList;
		std::string rStr((const char*)rBigEndian, nLen);
		myList.push_back(rStr);
		std::string sStr((const char*)sBigEndian, nLen);
		myList.push_back(sStr);
		delete[] rBigEndian;
		delete[] sBigEndian;
		return CreateBERASNStringForDSASignature(myList);
	}
	else
	{
		std::string rs((const char*)rBigEndian, nLen);
		rs.append((const char*)sBigEndian, nLen);
		delete[] rBigEndian;
		delete[] sBigEndian;
		return rs;
	}

}

ModNumber ECDSA::CalcR(const ModNumber& mk) const
{
	return ecKeyPair.ec.Mult(ecKeyPair.ec.g, mk).x % ecKeyPair.ec.n;
}

std::string ECDSA::Sign(unsigned char* hash, unsigned int hashLen, bool DerEncoded) const
{
	return CalculateDSASignature(ecKeyPair.ec.n, ecKeyPair.x, hash, hashLen, DerEncoded);
}


bool ECDSA::Verify(unsigned char* hash, unsigned int hashLen, std::string signature, bool DerEncoded) const
{
	unsigned int nLen = GetByteCount(ecKeyPair.ec.n);
	std::tuple<ModNumber, ModNumber, ModNumber> u1u2mr = DSACalculateU1U2Mr(ecKeyPair.ec.n, nLen, hash, hashLen, signature, DerEncoded);
	ModNumber u1 = std::get<0>(u1u2mr);
	ModNumber u2 = std::get<1>(u1u2mr);
	ModNumber mr = std::get<2>(u1u2mr);

	ECPoint pt1;
	std::thread th3([&pt1, this, &u1] {pt1 = ecKeyPair.ec.Mult(ecKeyPair.ec.g, u1); });
	ECPoint pt2;
	std::thread th4([&pt2, this, &u2] {pt2 = ecKeyPair.ec.Mult(ecKeyPair.y, u2); });
	th3.join();
	th4.join();
	ECPoint ptv = ecKeyPair.ec.Add(pt1, pt2);
	if (ptv.IsAtInfinity)
		return false;
	return ptv.x == mr;
}

ModNumber CalculateECDHSharedSecret(const ECKeyPair& pair1, const ECKeyPair& pair2)
{
	ECPoint resultPt = pair1.ec.Mult(pair1.y, pair2.x);
	return resultPt.x;
}

ModNumber CalculateDHSharedSecret(const DHParameters& publicKey, const DHParameters& privateKey)
{
	if (!(publicKey.Modulus == privateKey.Modulus))
		throw std::domain_error("Modulus of public key and private key should be the same");
	if (!(publicKey.Generator == privateKey.Generator))
		throw std::domain_error("Generator of public key and private key should be the same");
	MultGroupMod mgm(publicKey.Modulus);
	return mgm.Exp(publicKey.Public, privateKey.PrivateExp);
}

ModNumber GenerateDHPrivateKey(const ModNumber& modulus)
{
	srand((unsigned int)time(0));
	llint privKey[COUNTLL] = {};
	unsigned char* p = (unsigned char*)privKey;
	for (int i = 0; i < MAXMOD/4; i++)
	{
		p[i] = rand() % 0x100;
	}
	ModNumber mPrivKey(privKey);
	return mPrivKey % modulus;
}


#ifdef _WIN32 


int evaluateStatus(SECURITY_STATUS status)
{
	if (status == ERROR_SUCCESS || status == NTE_BAD_SIGNATURE)
		return 0;

	switch (status)
	{
	case NTE_BAD_FLAGS:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_BAD_FLAGS");
	case NTE_BAD_KEY_STATE:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_BAD_KEY_STATE");
	case NTE_BAD_KEYSET:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_BAD_KEYSET");
	case NTE_BAD_TYPE:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_BAD_TYPE");
	case NTE_EXISTS:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_EXISTS");
	case NTE_INVALID_HANDLE:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_INVALID_HANDLE");
	case NTE_INVALID_PARAMETER:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_INVALID_PARAMETER");
	case NTE_NOT_SUPPORTED:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_NOT_SUPPORTED");

	default:
		throw std::runtime_error("CryptEncrypt returned unknown error code");

	}

}

NCRYPT_KEY_HANDLE GenerateKey(const wchar_t* KeyName, NCRYPT_PROV_HANDLE provHandle, const wchar_t *algorithm, int usage)
{
	NCRYPT_KEY_HANDLE keyHandle;
	NCRYPT_PROV_HANDLE tmpProvHandle = NULL;
	SECURITY_STATUS status;
	if (provHandle == NULL)
	{
		status = NCryptOpenStorageProvider(&provHandle, NULL, 0);
		tmpProvHandle = provHandle;
		if (status != ERROR_SUCCESS)
			throw std::runtime_error("CryptOpenStorageProvider returned error code");
	}
	status = NCryptCreatePersistedKey(provHandle, &keyHandle, algorithm, KeyName, usage, 0);
	if (status != NTE_EXISTS)
		evaluateStatus(status);
	else
		return 0;
	DWORD policy = NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG | NCRYPT_ALLOW_EXPORT_FLAG;
	status = NCryptSetProperty(keyHandle, NCRYPT_EXPORT_POLICY_PROPERTY, (PBYTE)&policy, sizeof(DWORD), NCRYPT_PERSIST_FLAG);
	evaluateStatus(status);
	policy = MAXMOD*8;
	status = NCryptSetProperty(keyHandle, NCRYPT_LENGTH_PROPERTY, (PBYTE)&policy, sizeof(DWORD), NCRYPT_PERSIST_FLAG); 
	evaluateStatus(status);
	status = NCryptFinalizeKey(keyHandle, 0);
	evaluateStatus(status);
	if (tmpProvHandle != NULL)
	{
		status = NCryptFreeObject(tmpProvHandle);
		evaluateStatus(status);
	}
	return keyHandle;

}

void DeleteKey(const wchar_t *keyName, NCRYPT_PROV_HANDLE provHandle, int usage)
{
	SECURITY_STATUS status;
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenKey(provHandle, &keyHandle, keyName, usage, 0);
	if (status != NTE_BAD_KEYSET)
		evaluateStatus(status);
	else
		return;
	status = NCryptDeleteKey(keyHandle, 0);
	evaluateStatus(status);
}

RSAParameters GetRSAKey(const wchar_t *KeyName, bool createIfNotExists, int usage)
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status = NCryptOpenStorageProvider(&provHandle,NULL,0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenStorageProvider returned error code");
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenKey(provHandle, &keyHandle, KeyName, usage, 0);
	if (status == NTE_BAD_KEYSET && createIfNotExists)
	{
		keyHandle = GenerateKey(KeyName, provHandle, L"RSA", usage);
	}
	else if (status == NTE_BAD_KEYSET && !createIfNotExists)
	{
		throw std::runtime_error("Key does not exist!");
	}
	else if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenKey returned error code");
	DWORD keyLength;
	DWORD keyLengthSize;
	status = NCryptGetProperty(keyHandle, NCRYPT_LENGTH_PROPERTY, (unsigned char*)&keyLength, sizeof(DWORD), &keyLengthSize, 0);
	if (keyLength > MAXMOD * 8)
		throw std::domain_error("KeyLength not less or equal to MAXMOD");
	BCRYPT_RSAKEY_BLOB *pkeyData;
	DWORD size;
	status = NCryptExportKey(keyHandle, 0, BCRYPT_RSAFULLPRIVATE_BLOB, NULL, NULL, 0, &size, 0);
	unsigned char *rawKeyData = new unsigned char[size];
	pkeyData = (BCRYPT_RSAKEY_BLOB*)rawKeyData;
	status = NCryptExportKey(keyHandle, 0, BCRYPT_RSAFULLPRIVATE_BLOB, NULL, (PBYTE)pkeyData, size, &size, 0);
	evaluateStatus(status);
	status = NCryptFreeObject(keyHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");
	status = NCryptFreeObject(provHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");

	if (pkeyData->Magic != BCRYPT_RSAFULLPRIVATE_MAGIC)
		throw std::runtime_error("Key structure not of type RSA Full Private");
	if (pkeyData->BitLength / 8 > MAXMOD)
		throw std::runtime_error("Key Bitsize not correct!");
	RSAParameters rsaParameters;
	unsigned char* p = rawKeyData + sizeof(BCRYPT_RSAKEY_BLOB);
	unsigned char *pExp = ConvertEndianess(p,pkeyData->cbPublicExp);
	rsaParameters.pubExp = ModNumber(pExp, pkeyData->cbPublicExp);
	p += pkeyData->cbPublicExp;
	unsigned char* pModulus = ConvertEndianess(p ,pkeyData->cbModulus);
	rsaParameters.Modulus = ModNumber (pModulus,pkeyData->cbModulus);
	p += pkeyData->cbModulus;
	unsigned char* pPrime1 = ConvertEndianess(p,pkeyData->cbPrime1);
	rsaParameters.Prime1 = ModNumber(pPrime1, pkeyData->cbPrime1);
	p += pkeyData->cbPrime1;
	unsigned char* pPrime2 = ConvertEndianess(p, pkeyData->cbPrime2);
	rsaParameters.Prime2 = ModNumber(pPrime2, pkeyData->cbPrime2);
	p += pkeyData->cbPrime2;
	unsigned char* pExp1 = ConvertEndianess(p,pkeyData->cbPrime1);
	rsaParameters.Exp1 = ModNumber(pExp1, pkeyData->cbPrime1);
	p += pkeyData->cbPrime1;
	unsigned char* pExp2 = ConvertEndianess(p, pkeyData->cbPrime2);
	rsaParameters.Exp2 = ModNumber(pExp2, pkeyData->cbPrime2);
	p += pkeyData->cbPrime2;
	unsigned char* pCoefficient = ConvertEndianess(p, pkeyData->cbPrime1);
	rsaParameters.Coefficient = ModNumber(pCoefficient, pkeyData->cbPrime1);
	p += pkeyData->cbPrime1;
	unsigned char* pPrivExp = ConvertEndianess(p,pkeyData->cbModulus);
	rsaParameters.PrivExp = ModNumber(pPrivExp, pkeyData->cbModulus);
	delete[] pExp;
	delete[] pModulus;
	delete[] pPrime1;
	delete[] pPrime2;
	delete[] pExp1;
	delete[] pExp2;
	delete[] pCoefficient;
	delete[] pPrivExp;
	delete[] rawKeyData;
	return rsaParameters;
}

void SetRSAKey(const wchar_t* KeyName, RSAParameters rsaParameters)
{

	BCRYPT_RSAKEY_BLOB *pkeyData;
	DWORD cbPublicExp;
	DWORD cbModulus;
	DWORD cbPrime1;
	DWORD cbPrime2;
	cbPublicExp = GetByteCount(rsaParameters.pubExp);
	cbModulus = GetByteCount(rsaParameters.Modulus);
	cbPrime1 = GetByteCount(rsaParameters.Prime1);
	cbPrime2 = GetByteCount(rsaParameters.Prime2);
	DWORD size = sizeof(BCRYPT_RSAKEY_BLOB) + cbPublicExp + (cbModulus * 2) + (cbPrime1 * 3) + (cbPrime2 * 2);
	unsigned char *rawKeyData = new unsigned char[size];
	pkeyData = (BCRYPT_RSAKEY_BLOB*)rawKeyData;
	pkeyData->Magic = BCRYPT_RSAFULLPRIVATE_MAGIC;
	pkeyData->BitLength = MAXMOD * 8;
	pkeyData->cbModulus = cbModulus;
	pkeyData->cbPrime1 = cbPrime1;
	pkeyData->cbPrime2 = cbPrime2;
	pkeyData->cbPublicExp = cbPublicExp;
	unsigned char* p = rawKeyData + sizeof(BCRYPT_RSAKEY_BLOB);
	p = CopyKeyPart(rsaParameters.pubExp,pkeyData->cbPublicExp, p);
	p = CopyKeyPart(rsaParameters.Modulus, pkeyData->cbModulus, p);
	p = CopyKeyPart(rsaParameters.Prime1, pkeyData->cbPrime1, p);
	p = CopyKeyPart(rsaParameters.Prime2, pkeyData->cbPrime2,p);
	p = CopyKeyPart(rsaParameters.Exp1, pkeyData->cbPrime1,p);
	p = CopyKeyPart(rsaParameters.Exp2, pkeyData->cbPrime2,p);
	p = CopyKeyPart(rsaParameters.Coefficient, pkeyData->cbPrime1,p);
	p = CopyKeyPart(rsaParameters.PrivExp, pkeyData->cbModulus,p);
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status = NCryptOpenStorageProvider(&provHandle, NULL, 0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenStorageProvider returned error code");
	NCryptBufferDesc bufferDesc;
	BCryptBuffer cryptBuffer;
	cryptBuffer.BufferType = NCRYPTBUFFER_PKCS_KEY_NAME;
	cryptBuffer.cbBuffer = (ULONG)(wcslen(KeyName) + 1) * sizeof(wchar_t);
	cryptBuffer.pvBuffer = (PVOID)KeyName;
	bufferDesc.cBuffers = 1;
	bufferDesc.pBuffers = &cryptBuffer;
	bufferDesc.ulVersion = NCRYPTBUFFER_VERSION;
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptImportKey(provHandle, 0, BCRYPT_RSAFULLPRIVATE_BLOB, &bufferDesc,&keyHandle, (PBYTE)pkeyData, size, NCRYPT_DO_NOT_FINALIZE_FLAG);
	evaluateStatus(status);
	DWORD policy = NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG | NCRYPT_ALLOW_EXPORT_FLAG;
	status = NCryptSetProperty(keyHandle, NCRYPT_EXPORT_POLICY_PROPERTY, (PBYTE)&policy, sizeof(DWORD), NCRYPT_PERSIST_FLAG);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptSetProperty returned error code");
	status = NCryptFinalizeKey(keyHandle, 0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFinalizeKey returned error code");
	status = NCryptFreeObject(keyHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");
	delete[] rawKeyData;
}

DSAParameters GetDSAKey(const wchar_t* KeyName, bool createIfNotExists)
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status = NCryptOpenStorageProvider(&provHandle, NULL, 0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenStorageProvider returned error code");
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenKey(provHandle, &keyHandle, KeyName, AT_SIGNATURE, 0);
	if (status == NTE_BAD_KEYSET && createIfNotExists)
	{
		keyHandle = GenerateKey(KeyName, provHandle,L"DSA", AT_SIGNATURE);
	}
	else if (status == NTE_BAD_KEYSET && !createIfNotExists)
	{
		throw std::runtime_error("Key does not exist!");
	}
	else if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenKey returned error code");
	DWORD keyLength;
	DWORD keyLengthSize;
	status = NCryptGetProperty(keyHandle, NCRYPT_LENGTH_PROPERTY, (unsigned char*)&keyLength, sizeof(DWORD), &keyLengthSize, 0);
	if (keyLength > MAXMOD * 8)
		throw std::domain_error("KeyLength not less or equal to MAXMOD");
	DSAParameters dsaParameters;
#if (MAXMOD == 1024/8)
	BCRYPT_DSA_KEY_BLOB* pkeyData;
	DWORD size;
	status = NCryptExportKey(keyHandle, 0, BCRYPT_DSA_PRIVATE_BLOB, NULL, NULL, 0, &size, 0);
	unsigned char* rawKeyData = new unsigned char[size];
	pkeyData = (BCRYPT_DSA_KEY_BLOB*)rawKeyData;
	status = NCryptExportKey(keyHandle, 0, BCRYPT_DSA_PRIVATE_BLOB, NULL, (PBYTE)pkeyData, size, &size, 0);
	evaluateStatus(status);
	status = NCryptFreeObject(keyHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");
	status = NCryptFreeObject(provHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");

	if (pkeyData->dwMagic != BCRYPT_DSA_PRIVATE_MAGIC)
		throw std::runtime_error("Key structure not of type DSA Private");
	if (pkeyData->cbKey != MAXMOD)
		throw std::runtime_error("Key Bitsize not correct!");
	unsigned char* pQ = ConvertEndianess(pkeyData->q, 20 );
	dsaParameters.Q = ModNumber(pQ, 20);
	unsigned char* p = rawKeyData + sizeof(BCRYPT_DSA_KEY_BLOB);
	unsigned char* pP = ConvertEndianess(p, pkeyData->cbKey);
	dsaParameters.P = ModNumber(pP, pkeyData->cbKey);
	p += pkeyData->cbKey;
	unsigned char* pG = ConvertEndianess(p, pkeyData->cbKey);
	dsaParameters.g = ModNumber(pG, pkeyData->cbKey);
	p += pkeyData->cbKey;
	unsigned char* pY = ConvertEndianess(p, pkeyData->cbKey);
	dsaParameters.y = ModNumber(pY, pkeyData->cbKey);
	p += pkeyData->cbKey;
	unsigned char* pX = ConvertEndianess(p, 20);
	dsaParameters.x = ModNumber(pX, 20);
	delete[] pQ;
	delete[] pP;
	delete[] pG;
	delete[] pY;
	delete[] pX;
	delete[] rawKeyData;
#elif (MAXMOD == 2048/8 || MAXMOD == 3072/8)
	BCRYPT_DSA_KEY_BLOB_V2* pkeyData;
	DWORD size;
	status = NCryptExportKey(keyHandle, 0, BCRYPT_DSA_PRIVATE_BLOB, NULL, NULL, 0, &size, 0);
	unsigned char* rawKeyData = new unsigned char[size];
	pkeyData = (BCRYPT_DSA_KEY_BLOB_V2*)rawKeyData;
	status = NCryptExportKey(keyHandle, 0, BCRYPT_DSA_PRIVATE_BLOB, NULL, (PBYTE)pkeyData, size, &size, 0);
	evaluateStatus(status);
	status = NCryptFreeObject(keyHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");
	status = NCryptFreeObject(provHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");

	if (pkeyData->dwMagic != BCRYPT_DSA_PRIVATE_MAGIC_V2)
		throw std::runtime_error("Key structure not of type DSA Private");
	if (pkeyData->cbKey != MAXMOD)
		throw std::runtime_error("Key Bitsize not correct!");
	unsigned char* p = rawKeyData + sizeof(BCRYPT_DSA_KEY_BLOB_V2) + pkeyData->cbSeedLength;
	unsigned char* pQ = ConvertEndianess(p, pkeyData->cbGroupSize);
	dsaParameters.Q = ModNumber(pQ, pkeyData->cbGroupSize);
	p += pkeyData->cbGroupSize;
	unsigned char* pP = ConvertEndianess(p, pkeyData->cbKey);
	dsaParameters.P = ModNumber(pP, pkeyData->cbKey);
	p += pkeyData->cbKey;
	unsigned char* pG = ConvertEndianess(p, pkeyData->cbKey);
	dsaParameters.g = ModNumber(pG, pkeyData->cbKey);
	p += pkeyData->cbKey;
	unsigned char* pY = ConvertEndianess(p, pkeyData->cbKey);
	dsaParameters.y = ModNumber(pY, pkeyData->cbKey);
	p += pkeyData->cbKey;
	unsigned char* pX = ConvertEndianess(p, pkeyData->cbGroupSize);
	dsaParameters.x = ModNumber(pX, pkeyData->cbGroupSize);
	delete[] pQ;
	delete[] pP;
	delete[] pG;
	delete[] pY;
	delete[] pX;
	delete[] rawKeyData;

#endif
	return dsaParameters;
}

std::tuple<ModNumber,DWORD> decrypt(const wchar_t *KeyName,const ModNumber& data)
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status = NCryptOpenStorageProvider(&provHandle, NULL, 0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenStorageProvider returned error code");
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenKey(provHandle, &keyHandle, KeyName, AT_KEYEXCHANGE, 0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenKey returned error code");
	unsigned int cbData = GetByteCount(data);
	unsigned char *pData = (unsigned char *)data.num;
	unsigned char* pDataBigEndian = ConvertEndianess(pData, cbData);
	unsigned char* pDataDecryptedBigEndian = new unsigned char[cbData];
	unsigned char* pDataDecrypted;
	DWORD cbResult;
	status = NCryptDecrypt(keyHandle, pDataBigEndian, cbData, nullptr, pDataDecryptedBigEndian, cbData, &cbResult, NCRYPT_PAD_PKCS1_FLAG);
	evaluateStatus(status);
	pDataDecrypted = ConvertEndianess(pDataDecryptedBigEndian, cbResult);
	ModNumber res(pDataDecrypted, cbResult);
	delete[] pDataDecryptedBigEndian;
	delete[] pDataDecrypted;
	status = NCryptFreeObject(keyHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");
	status = NCryptFreeObject(provHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");

	return std::make_tuple(res, cbResult);
}



ModNumber encrypt(const wchar_t* KeyName,const ModNumber& data)
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status = NCryptOpenStorageProvider(&provHandle, NULL, 0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenStorageProvider returned error code");
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenKey(provHandle, &keyHandle, KeyName, AT_KEYEXCHANGE, 0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenKey returned error code");
	unsigned int cbData = GetByteCount(data);
	unsigned char* pData = (unsigned char*)data.num;
	unsigned char* pDataBigEndian = ConvertEndianess(pData, cbData);
	unsigned char pDataEncryptedBigEndian[MAXMOD];
	unsigned char* pDataEncrypted;
	DWORD cbResult;
	status = NCryptEncrypt(keyHandle, pDataBigEndian, cbData, NULL, pDataEncryptedBigEndian, MAXMOD, &cbResult, NCRYPT_PAD_PKCS1_FLAG);
	evaluateStatus(status);
	pDataEncrypted = ConvertEndianess(pDataEncryptedBigEndian, cbResult);
	ModNumber res(pDataEncrypted, cbResult);
	delete[] pDataBigEndian;
	delete[] pDataEncrypted;
	status = NCryptFreeObject(keyHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");
	status = NCryptFreeObject(provHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");
	return res;
}

int evaluateBStatus(NTSTATUS status)
{
	if (status == ERROR_SUCCESS)
		return 0;
	switch (status)
	{
	case STATUS_INVALID_HANDLE:
		throw std::runtime_error("Cryptographic function returned error code: STATUS_INVALID_HANDLE");
	case STATUS_INVALID_PARAMETER:
		throw std::runtime_error("Cryptographic function returned error code: STATUS_INVALID_PARAMETER");
	case STATUS_NO_MEMORY:
		throw std::runtime_error("Cryptographic function returned error code: STATUS_NO_MEMORY");
	case STATUS_NOT_SUPPORTED:
		throw std::runtime_error("Cryptographic function returned error code: STATUS_NOT_SUPPORTED");

	default:
		throw std::runtime_error("Cryptographic function returned unknown error code");
	}

}

std::tuple<unsigned char*, ULONG> hash(unsigned char *data, size_t count, const wchar_t *hashAlgorithm)
{
	BCRYPT_ALG_HANDLE algHandle;
	NTSTATUS bStatus;
	bStatus = BCryptOpenAlgorithmProvider(&algHandle, hashAlgorithm, NULL, 0);
	evaluateBStatus(bStatus);
	ULONG hashLength;
	ULONG cbHashLength = sizeof(ULONG);
	ULONG cbHashLengthReturned;
	bStatus = BCryptGetProperty(algHandle, BCRYPT_HASH_LENGTH, (PUCHAR)&hashLength, cbHashLength, &cbHashLengthReturned, 0);
	evaluateBStatus(bStatus);
	BCRYPT_HASH_HANDLE hashHandle;
	bStatus = BCryptCreateHash(algHandle, &hashHandle, NULL, 0, NULL, 0, 0);
	evaluateBStatus(bStatus);
	bStatus = BCryptCloseAlgorithmProvider(algHandle, 0);
	evaluateBStatus(bStatus);
	bStatus = BCryptHashData(hashHandle, data, (ULONG)count, 0);
	evaluateBStatus(bStatus);
	unsigned char* hash = new unsigned char[hashLength];
	bStatus = BCryptFinishHash(hashHandle, hash, hashLength, 0);
	evaluateBStatus(bStatus);
	bStatus = BCryptDestroyHash(hashHandle);
	evaluateBStatus(bStatus);
	return std::make_tuple(hash, hashLength);
}

std::string sign(const wchar_t* keyName, unsigned char* hash, int hashLength, const wchar_t *hashAlgorithm)
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status;
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenStorageProvider(&provHandle, NULL, 0);
	evaluateStatus(status);
	status = NCryptOpenKey(provHandle, &keyHandle, keyName, AT_SIGNATURE, 0);
	evaluateStatus(status);
	ULONG keyLength;
	DWORD cbLength;
	status = NCryptGetProperty(keyHandle, NCRYPT_LENGTH_PROPERTY, (PBYTE) & keyLength, sizeof(ULONG), &cbLength, 0);
	evaluateStatus(status);
	if (!hashAlgorithm && keyLength == 0x400)
	{
		hashLength = 20;
	}
	unsigned char signature[MAXMOD];
	ULONG cbResult;
	BCRYPT_PKCS1_PADDING_INFO paddingInfo;
	paddingInfo.pszAlgId = hashAlgorithm;
	status = NCryptSignHash(keyHandle, hashAlgorithm ? &paddingInfo : 0, hash, hashLength, signature, MAXMOD, &cbResult, hashAlgorithm ? BCRYPT_PAD_PKCS1 : 0);
	evaluateStatus(status);
	status = NCryptFreeObject(keyHandle);
	evaluateStatus(status);
	return std::string((const char *)signature,cbResult); 
}

bool verify(const wchar_t* keyName, unsigned char* hash, int hashLength, std::string signature, const wchar_t *hashAlgorithm )
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status;
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenStorageProvider(&provHandle, NULL, 0);
	evaluateStatus(status);
	status = NCryptOpenKey(provHandle, &keyHandle, keyName, AT_SIGNATURE, 0);
	ULONG keyLength;
	DWORD cbLength;
	status = NCryptGetProperty(keyHandle, NCRYPT_LENGTH_PROPERTY, (PBYTE)&keyLength, sizeof(ULONG), &cbLength, 0);
	evaluateStatus(status);
	if (!hashAlgorithm && keyLength == 0x400)
	{
		hashLength = 20;
	}
	BCRYPT_PKCS1_PADDING_INFO paddingInfo;
	paddingInfo.pszAlgId = hashAlgorithm;
	status = NCryptVerifySignature(keyHandle,hashAlgorithm ? &paddingInfo : 0, hash, hashLength, (PBYTE)signature.c_str(),(DWORD) signature.length(), hashAlgorithm ? BCRYPT_PAD_PKCS1 : 0);
	evaluateStatus(status);
	NCryptFreeObject(keyHandle);
	return status == ERROR_SUCCESS;
}

BCRYPT_KEY_HANDLE importECCKey(const ECKeyPair& ecKeyPair, const wchar_t *curveName, const wchar_t *algorithm, ULONG magic)
{
	NTSTATUS status;
	BCRYPT_ALG_HANDLE algHandle;
	status = BCryptOpenAlgorithmProvider(&algHandle, algorithm , nullptr, 0);
	evaluateBStatus(status);
	status = BCryptSetProperty(algHandle, BCRYPT_ECC_CURVE_NAME, (PUCHAR)curveName, (ULONG)(wcslen(curveName) + 1) * sizeof(wchar_t), 0);
	evaluateBStatus(status);
	BCRYPT_KEY_HANDLE keyHandle;
	unsigned int nLen = GetByteCount(ecKeyPair.ec.n);
	ULONG blobSize = sizeof(BCRYPT_ECCKEY_BLOB) + nLen * ((magic == BCRYPT_ECDH_PRIVATE_GENERIC_MAGIC || magic == BCRYPT_ECDSA_PRIVATE_GENERIC_MAGIC) ? 3 : 2);
	BCRYPT_ECCKEY_BLOB* keyBlob = (BCRYPT_ECCKEY_BLOB*)new char[blobSize];
	keyBlob->dwMagic = magic;
	keyBlob->cbKey = nLen;
	char* p = (char*)(keyBlob + 1);
	unsigned char* publicKeyXBigEndian = ConvertEndianess(ecKeyPair.y.x,nLen);
	memcpy(p, publicKeyXBigEndian, nLen);
	p += nLen;
	unsigned char* publicKeyYBigEndian = ConvertEndianess(ecKeyPair.y.y, nLen);
	memcpy(p, publicKeyYBigEndian, nLen);
	if ((magic == BCRYPT_ECDH_PRIVATE_GENERIC_MAGIC || magic == BCRYPT_ECDSA_PRIVATE_GENERIC_MAGIC))
	{
		p += nLen;
		unsigned char* privateKeyBigEndian = ConvertEndianess(ecKeyPair.x, nLen);
		memcpy(p, privateKeyBigEndian, nLen);
		delete[] privateKeyBigEndian;
	}
	delete[] publicKeyXBigEndian;
	delete[] publicKeyYBigEndian;

	status = BCryptImportKeyPair(algHandle, 0, (magic == BCRYPT_ECDH_PRIVATE_GENERIC_MAGIC || magic == BCRYPT_ECDSA_PRIVATE_GENERIC_MAGIC) ? BCRYPT_ECCPRIVATE_BLOB : BCRYPT_ECCPUBLIC_BLOB, &keyHandle, (PUCHAR)keyBlob, blobSize, 0);
	evaluateBStatus(status);
	delete[] keyBlob;
	status = BCryptCloseAlgorithmProvider(algHandle, 0);
	return keyHandle;
}



BCRYPT_KEY_HANDLE importDHKey(const DHParameters& key, ULONG magic)
{
	NTSTATUS status;
	BCRYPT_ALG_HANDLE algHandle;
	status = BCryptOpenAlgorithmProvider(&algHandle, BCRYPT_DH_ALGORITHM, nullptr, 0);
	evaluateBStatus(status);
	BCRYPT_KEY_HANDLE keyHandle;
	ULONG blobSize = sizeof(BCRYPT_DH_KEY_BLOB) + MAXMOD * (magic == BCRYPT_DH_PRIVATE_MAGIC? 4 : 3);
	BCRYPT_DH_KEY_BLOB* keyBlob = (BCRYPT_DH_KEY_BLOB*)new char[blobSize];
	keyBlob->dwMagic = magic;
	keyBlob->cbKey = MAXMOD;
	char* p = (char*)(keyBlob + 1);
	unsigned char* pModulusBigEndian = ConvertEndianess(key.Modulus, MAXMOD);
	memcpy(p, pModulusBigEndian, MAXMOD);
	p += MAXMOD;
	unsigned char* pGeneratorBigEndian = ConvertEndianess(key.Generator, MAXMOD);
	memcpy(p, pGeneratorBigEndian, MAXMOD);
	p += MAXMOD;
	unsigned char* pPublicBigEndian = ConvertEndianess(key.Public, MAXMOD);
	memcpy(p, pPublicBigEndian, MAXMOD);
	if (magic == BCRYPT_DH_PRIVATE_MAGIC)
	{
		p += MAXMOD;
		unsigned char* pPrivateExpBigEndian = ConvertEndianess(key.PrivateExp, MAXMOD);
		memcpy(p, pPrivateExpBigEndian, MAXMOD);
		delete[] pPrivateExpBigEndian;
	}
	delete[] pModulusBigEndian;
	delete[] pGeneratorBigEndian;
	delete[] pPublicBigEndian;

	status = BCryptImportKeyPair(algHandle, 0, magic == BCRYPT_DH_PRIVATE_MAGIC ? BCRYPT_DH_PRIVATE_BLOB : BCRYPT_DH_PUBLIC_BLOB, &keyHandle, (PUCHAR)keyBlob, blobSize, 0);
	evaluateBStatus(status);
	delete[] keyBlob;
	status = BCryptCloseAlgorithmProvider(algHandle, 0);
	return keyHandle;

}

std::string signECDsa(const ECDSA& ecDsa, unsigned char* hash, unsigned int hashLength, const wchar_t *curveName)
{
	NTSTATUS status;
	BCRYPT_KEY_HANDLE keyHandle = importECCKey(ecDsa.ecKeyPair, curveName, BCRYPT_ECDSA_ALGORITHM, BCRYPT_ECDSA_PRIVATE_GENERIC_MAGIC);
	unsigned int nLen = GetByteCount(ecDsa.ecKeyPair.ec.n);
	if (hashLength > nLen)
		hashLength = nLen;
	DWORD resultSize;
	status = BCryptSignHash(keyHandle, 0, (PUCHAR)hash, hashLength, nullptr, 0, &resultSize, 0);
	evaluateBStatus(status);
	unsigned char* signature = new unsigned char[resultSize];
	status = BCryptSignHash(keyHandle, 0, (PUCHAR)hash, hashLength, signature, resultSize, &resultSize, 0);
	BCryptDestroyKey(keyHandle);
	evaluateBStatus(status);
	evaluateBStatus(status);
	return std::string((const char *)signature, resultSize);
}

bool verifyECDsa(const ECDSA& ecDsa, unsigned char* hash, unsigned int hashLength, std::string signature,const wchar_t *curveName)
{
	NTSTATUS status;
	BCRYPT_KEY_HANDLE keyHandle = importECCKey(ecDsa.ecKeyPair, curveName, BCRYPT_ECDSA_ALGORITHM, BCRYPT_ECDSA_PRIVATE_GENERIC_MAGIC);
	unsigned int nLen = GetByteCount(ecDsa.ecKeyPair.ec.n);
	if (hashLength > nLen)
		hashLength = nLen;
	unsigned char* pSignature = (unsigned char *)signature.c_str();
	status = BCryptVerifySignature(keyHandle, nullptr, hash, hashLength, pSignature, (ULONG)signature.length(), 0);
	BCryptDestroyKey(keyHandle);
	if (!(status == ERROR_SUCCESS || status == STATUS_INVALID_SIGNATURE))
		evaluateBStatus(status);
	return status == ERROR_SUCCESS;
}

ModNumber GetSecretECDHAgreement(const ECKeyPair& pair1, const ECKeyPair& pair2,const wchar_t * curveName)
{
	NTSTATUS status;
	BCRYPT_KEY_HANDLE keyHandle1;
	BCRYPT_KEY_HANDLE keyHandle2;
	keyHandle1 = importECCKey(pair1, curveName,BCRYPT_ECDH_ALGORITHM, BCRYPT_ECDH_PRIVATE_GENERIC_MAGIC);
	keyHandle2 = importECCKey(pair2, curveName, BCRYPT_ECDH_ALGORITHM, BCRYPT_ECDH_PUBLIC_GENERIC_MAGIC);
	BCRYPT_SECRET_HANDLE secretHandle;
	status = BCryptSecretAgreement(keyHandle1, keyHandle2, &secretHandle, 0);
	evaluateBStatus(status);
	BCryptDestroyKey(keyHandle1);
	BCryptDestroyKey(keyHandle2);
	ULONG cbResult;
	status = BCryptDeriveKey(secretHandle, BCRYPT_KDF_RAW_SECRET, nullptr, nullptr, 0, &cbResult, 0);
	unsigned char* pDerivedKey = new unsigned char[cbResult];
	status = BCryptDeriveKey(secretHandle, BCRYPT_KDF_RAW_SECRET, nullptr, pDerivedKey, cbResult, &cbResult, 0);
	evaluateBStatus(status);
	ModNumber result(pDerivedKey, cbResult);
	return result;
}

ModNumber GetSecretDHAgreement(const DHParameters& publicKey , const DHParameters privateKey)
{
	NTSTATUS status;
	BCRYPT_KEY_HANDLE keyHandlePubKey;
	BCRYPT_KEY_HANDLE keyHandlePrivKey;
	keyHandlePubKey = importDHKey(publicKey, BCRYPT_DH_PUBLIC_MAGIC);
	keyHandlePrivKey = importDHKey(privateKey, BCRYPT_DH_PRIVATE_MAGIC);
	BCRYPT_SECRET_HANDLE secretHandle;
	status = BCryptSecretAgreement(keyHandlePrivKey, keyHandlePubKey, &secretHandle, 0);
	evaluateBStatus(status);
	BCryptDestroyKey(keyHandlePrivKey);
	BCryptDestroyKey(keyHandlePubKey);
	ULONG cbResult;
	status = BCryptDeriveKey(secretHandle, BCRYPT_KDF_RAW_SECRET, nullptr, nullptr, 0, &cbResult, 0);
	unsigned char* pDerivedKey = new unsigned char[cbResult];
	status = BCryptDeriveKey(secretHandle, BCRYPT_KDF_RAW_SECRET, nullptr, pDerivedKey, cbResult, &cbResult, 0);
	evaluateBStatus(status);
	ModNumber result(pDerivedKey, cbResult);
	return result;
}



std::list<BCRYPT_ALGORITHM_IDENTIFIER> getAlgorithms()
{
	NTSTATUS status;
	ULONG cbCount;
	BCRYPT_ALGORITHM_IDENTIFIER* pAlgorithmList;
	std::list<BCRYPT_ALGORITHM_IDENTIFIER> myList;
	status = BCryptEnumAlgorithms(BCRYPT_ASYMMETRIC_ENCRYPTION_OPERATION | BCRYPT_CIPHER_OPERATION, &cbCount, &pAlgorithmList, 0);
	evaluateBStatus(status);
	for (unsigned int i = 0; i < cbCount; i++)
		myList.push_back(pAlgorithmList[i]);
	return myList;
}
#endif
