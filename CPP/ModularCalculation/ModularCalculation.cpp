// ModularCalculation.cpp : Defines the functions for the static library.
//

#include "pchmc.h"
#include "framework.h"
#include "ModularCalculation.h"





ModNumber operator-(const ModNumber& l, const ModNumber& r)
{
	ModNumber res;
	lint* ll = (lint*)l.num;
	lint* rl = (lint*)r.num;
	lint* resl = (lint*)res.num;
	lint carry = 0;
	for (int i = 0; i < COUNTL; i++)
	{
		lint ltmp = ll[i];
		lint rtmp = rl[i];
		if (ltmp >= carry)
		{
			ltmp -= carry;
			carry = 0;
		}
		else
			ltmp -= carry;
		if (ltmp < rtmp)
		{
			carry = 1; 
		}
		resl[i] = ltmp - rtmp;
	}
	return res;
}

ModNumber &operator-=(ModNumber& l, const ModNumber& r)
{
	lint* ll = (lint*)l.num;
	lint* rl = (lint*)r.num;
	lint carry = 0;
	for (int i = 0; i < COUNTL; i++)
	{
		lint ltmp = ll[i];
		lint rtmp = rl[i];
		if (ltmp >= carry)
		{
			ltmp -= carry;
			carry = 0;
		}
		else
			ltmp -= carry;
		if (ltmp < rtmp)
		{
			carry = 1;
		}
		ll[i] = ltmp - rtmp;
	}
	return l;
}

ModNumber operator%(const ModNumber& l, const ModNumber& r)
{
	ModNumber mzero;
	if (r == mzero)
		throw std::domain_error("Division by Zero");
	ModNumber mone(1ull);
	if (r == mone)		// Just optimization
		return mzero;
	ModNumber mtwo(2ull);
	if (r == mtwo)		// Just optimization
	{
		if (l.num[0] & 0x1ull)
			return mone;
		else
			return mzero;
	}
	if (l < r)
	{
		return l;
	}
	if (l == r)
		return ModNumber(0ull);
	int li = 0;
	int ri = 0;
	for (int i = COUNTLL - 1; i >= 0; i--)
		if (l.num[i])
		{
			li = i;
			break;
		}
	for (int i = li; i >= 0; i--)
		if (r.num[i])
		{
			ri = i;
			break;
		}
	int diff = li - ri;
	ModNumber mres(l);
	for (int i = 0; i <= diff; i++)
	{
		llint tmp[COUNTLL] = {};
		for (int j = 0; j <= ri; j++)
		{
			tmp[j + diff - i] = r.num[j];
		}
		ModNumber mtmp(tmp);
		while(mres > mtmp)
		{
			mres -= mtmp;
		}
	}
	return mres;
}

ModNumber operator << (const ModNumber& n, unsigned int i)
{
	int words = 0;
	if (i >= LSIZE*8)
	{
		if (i >= NSIZE)
			return ModNumber();
		words = i / (LSIZE*8);
		i %= (LSIZE*8);
	}
	lint* pn = (lint*)n.num;
	lint pres[COUNTL] = {};
	pres[COUNTL - 1] = pn[COUNTL - words - 1] << i;
	for (int j = COUNTL - 2; j >= words; j--)
	{
		llint tmp = ((llint)pn[j-words]) << i;
		pres[j + 1] |= ((lint*)(&tmp))[1];
		pres[j] = ((lint*)(&tmp))[0];
	}
	return ModNumber((llint *)pres);
}

ModNumber& operator <<= (ModNumber& n, unsigned int i)
{
	int words = 0;
	if (i >= LSIZE*8)
	{
		if (i >= NSIZE)
			return n = ModNumber();
		words = i / (LSIZE*8);
		i %= (LSIZE*8);
	}
	lint* pn = (lint*)n.num;
	pn[COUNTL - 1] = pn[COUNTL - words - 1] << i;
	for (int j = COUNTL - 2; j >= words; j--)
	{
		llint tmp = ((llint)pn[j-words]) << i;
		pn[j + 1] |= ((lint*)(&tmp))[1]; 
		pn[j] = ((lint*)(&tmp))[0];
	}
	for (int j = 0; j < words; j++)
		pn[j] = 0ull;
	return n;
}

bool operator==(const ModNumber& l, const ModNumber& r)
{
	for (int i = 0; i < COUNTLL; i++)
		if (l.num[i] != r.num[i])
			return false;
		else
			;
	return true;
}

bool operator < (const ModNumber& l, const ModNumber& r)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
		if (l.num[i] || r.num[i])
			return l.num[i] < r.num[i];
	return false;
}

bool operator <= (const ModNumber& l, const ModNumber& r)
{
	return l < r || l == r;
}

bool operator >= (const ModNumber& l, const ModNumber& r)
{
	return l > r || l == r;
}

bool operator > (const ModNumber& l, const ModNumber& r)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
		if (l.num[i] || r.num[i])
			return l.num[i] > r.num[i];
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
		int initial = 2;
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

ModNumber& ModNumber::AddAssignScalar(int lpos, lint scalar)
{
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

	for (int i = 0; i < COUNTL; i++)
	{
		llint res = ((llint)ln[i]) * scalar;
		mres.AddAssignScalar(i, ((lint*)&res)[0]);
		if (i < COUNTL - 1)
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

std::tuple<ModNumber, lint> ModNumber::DivideAndModulo(lint scalar) const
{
	if (scalar == 0)
		throw std::domain_error("Division by zero not allowed!");
	ModNumber res;
	lint* nl = (lint*)num;
	lint* resl = (lint*)res.num;
	llint tmp = 0ull;
	for (int i = COUNTL - 1; i >= 0; i--)
	{
		*((lint*)&tmp) = nl[i];
		if (scalar <= tmp)
		{
			resl[i] = (lint)(tmp / scalar);
			tmp %= scalar;
		}
		tmp <<= LSIZE * 8;

	}
	return std::make_tuple(res, ((lint *) & tmp)[1]);
}

ModNumber operator/ (const ModNumber& n, lint scalar)
{
	return std::get<0>(n.DivideAndModulo(scalar));
}

ModNumber &operator/= (ModNumber& n, lint scalar)
{
	n = std::get<0>(n.DivideAndModulo(scalar));
	return n;
}

lint operator% (const ModNumber& n, lint scalar)
{
	return std::get<1>(n.DivideAndModulo(scalar));
}

std::string ModNumber::to_string_hex_base() const
{
	std::string res;
	const int buflen = LLSIZE * 2;
	int width;
	width = buflen;
	res.reserve(buflen * COUNTLL);
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
	res.assign(OctalStringLength, ' ');
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
	res.assign(DecimalStringLength, ' ');
	ModNumber tmp(*this);
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
	int firstbits = 2;
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