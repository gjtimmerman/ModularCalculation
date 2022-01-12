// ModularCalculation.cpp : Defines the functions for the static library.
//

#include "pchmc.h"
#include "framework.h"
#include "ModularCalculation.h"




llint* CalcModulo(llint* n, llint *mod)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
	{

	}
	return nullptr;
}

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
			carry = 0;
		ltmp -= carry;
		if (ltmp < rtmp)
		{
			carry = 1; 
		}
		resl[i] = ltmp - rtmp;
	}
	return res;
}

bool operator==(const ModNumber& l, const ModNumber& r)
{
	for (int i = 0; i < COUNTLL; i++)
		if (l.num[i] != r.num[i])
			return false;
	return true;
}

std::ostream& operator<<(std::ostream& out, ModNumber& n)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
		out << std::hex << n.num[i];
	return out;
}

ModNumber& ModNumber::AddAssignScalar(int lpos, lint scalar)
{
	llint res = 0;
	lint* n = (lint *)num;
	do
	{
		int lposc = lpos;
		while (((lint *) &res)[1] > 0ul && lposc < COUNTL)
		{
			res = ((llint)n[lposc]) + ((lint*)&res)[1];
			n[lposc++] = ((lint *) &res)[0];
		}
		if (lposc < COUNTL)
		{
			res = ((llint)n[lpos]) + scalar;
			n[lpos++] = ((lint*)&res)[0];
		}
		else
			break;
	} while (((lint*) & res)[1] > 0 && lpos < COUNTL);
	return *this;
}

ModNumber& operator+=(ModNumber& n, lint scalar)
{
	return n.AddAssignScalar(0, scalar);
}

ModNumber operator* (ModNumber& n, lint scalar)
{
	lint* ln = (lint*)n.num;
	ModNumber mres;
	lint* lres = (lint*)mres.num;

	for (int i = 0; i < COUNTL; i++)
	{
		llint res = ((llint)ln[i]) * scalar;
		mres.AddAssignScalar(i, ((lint*)&res)[0]);
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
	char formatchar;
	width = buflen;
	formatchar = 'X';
	res.reserve(buflen * COUNTLL);
	char buf[buflen+1];
	const int formatlen = 7;
	char format[formatlen+1];
	sprintf_s(format, "%%0%dll%c", width,formatchar);
	for (int i = COUNTLL-1; i >= 0; i--)
	{
		sprintf_s(buf, format, num[i]);
		res.append(buf);
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
		char strbuf[2];
		if ((wordCount++ % (8 * LSIZE) ) == 0)
		{
			if (wordCount/(8 * LSIZE) + 1 < COUNTL)
				buf[1] = pLint[wordCount / (8 * LSIZE) + 1];
		}
		if ((tripleCount++ % 3) == 0)
		{
			lint numToPrint = buf[0] & mask;
			sprintf_s(strbuf, "%lo", numToPrint);
			res[(OctalStringLength) - (tripleCount / 3) - 1] = strbuf[0];
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

ModNumber ModNumber::stomn_hex_base(std::string s)
{
	llint n[COUNTLL] = {};
	size_t len = s.length();
	if (len > NCOUNT * 2)
		throw std::domain_error("Value to large");
	if (len < NCOUNT * 2)
	{
		std::string tmp(NCOUNT * 2 - len,'0');
		s = tmp + s;
	}
	for (int i = 0; i < NCOUNT * 2; i += LLSIZE*2)
	{
		std::string tmp = s.substr(i, LLSIZE*2);
		llint tmpll = std::stoull(tmp,nullptr,16);
		n[COUNTLL - (i / (LLSIZE * 2)) - 1] = tmpll;
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
	}
}