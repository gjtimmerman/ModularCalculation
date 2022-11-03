// ModularCalculation.cpp : Defines the functions for the static library.
//

#include "pchmc.h"
#include "framework.h"
#include "ModularCalculation.h"





ModNumber operator-(const ModNumber& l, const ModNumber& r)
{
	ModNumber res;
	if (l == r)					// Optimization
		return res;

	const lint* ll = (const lint*)l.num;
	const lint* rl = (const lint*)r.num;
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
		{
			ltmp -= carry;
		}
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
	if (l == r)					// Optimization and prevent selfassignment
		return l = ModNumber();

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
		{
			ltmp -= carry;
		}
		if (ltmp < rtmp)
		{
			carry = 1;
		}
		ll[i] = ltmp - rtmp;
	}
	return l;
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

ModNumber operator/ (const ModNumber& l, const ModNumber& r)
{
	ModNumber divRes;
	ModNumber mzero;
	if (r == mzero)
		throw std::domain_error("Division by Zero");
	ModNumber mone(1ull);
	if (r == mone)		// Just optimization
		return l;
	ModNumber mtwo(2ull);
	if (r == mtwo)		// Just optimization
		return l >> 1;
	if (l < r)
		return mzero;
	if (l == r)
		return mone;
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
	divRes = ModNumber();
	for (int i = 0; i <= diff; i++)
	{
		llint divisor[COUNTLL] = {};
		llint tmp[COUNTLL] = {};
		for (int j = 0; j <= ri; j++)
		{
			tmp[j + diff - i] = r.num[j];
		}
		divisor[diff - i] = 1ull;
		ModNumber mtmp(tmp);
		ModNumber mdivisor(divisor);
		unsigned int bl = (LLSIZE * 8 - mres.FindFirstNonZeroBitInWord(li)) + (li)*LLSIZE * 8;
		unsigned int br = (LLSIZE * 8 - mtmp.FindFirstNonZeroBitInWord(diff + ri - i)) + (diff + ri - i) * LLSIZE * 8;
		int bdiff = bl - br;
		for (int j = 0; j <= bdiff; j++)
		{
			ModNumber mtmp2(mtmp);
			ModNumber mdivisor2(mdivisor);
			mtmp2 <<= bdiff - j;
			mdivisor2 <<= bdiff - j;
			while (mres >= mtmp2)
			{
				mres -= mtmp2;
				divRes += mdivisor2;
			}
		}
	}
	return divRes;
}


std::tuple<ModNumber,ModNumber> DivideAndModulo(const ModNumber& l, const ModNumber& r)
{
	ModNumber divRes;
	ModNumber mzero;
	if (r == mzero)
		throw std::domain_error("Division by Zero");
	ModNumber mone(1ull);
	if (r == mone)		// Just optimization
		return std::make_tuple(l,mzero);
	ModNumber mtwo(2ull);
	if (r == mtwo)		// Just optimization
	{
		divRes = l >> 1;
		if (l.num[0] & 0x1ull)
			return std::make_tuple(divRes,mone);
		else
			return std::make_tuple(divRes,mzero);
	}
	if (l < r)
		return std::make_tuple(ModNumber(),l);
	if (l == r)
		return std::make_tuple(mone,mzero);
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
	divRes = ModNumber();
	for (int i = 0; i <= diff; i++)
	{
		llint divisor[COUNTLL] = {};
		llint tmp[COUNTLL] = {};
		for (int j = 0; j <= ri; j++)
		{
			tmp[j + diff - i] = r.num[j];
		}
		divisor[diff - i] = 1ull;
		ModNumber mtmp(tmp);
		ModNumber mdivisor(divisor);
		unsigned int bl = (LLSIZE * 8 - mres.FindFirstNonZeroBitInWord(li)) + (li)*LLSIZE * 8;
		unsigned int br = (LLSIZE * 8 - mtmp.FindFirstNonZeroBitInWord(diff + ri - i)) + (diff + ri - i) * LLSIZE * 8;
		int bdiff = bl - br;
		for (int j = 0; j <= bdiff; j++)
		{
			ModNumber mtmp2(mtmp);
			ModNumber mdivisor2(mdivisor);
			mtmp2 <<= bdiff - j;
			mdivisor2 <<= bdiff - j;
			while (mres >= mtmp2)
			{
				mres -= mtmp2;
				divRes += mdivisor2;
			}
		}
	}
	return std::make_tuple(divRes,mres);
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
		return l;
	if (l == r)
		return mzero;
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
		unsigned int bl = (LLSIZE*8 - mres.FindFirstNonZeroBitInWord(li)) + (li)*LLSIZE*8;
		unsigned int br = (LLSIZE*8 - mtmp.FindFirstNonZeroBitInWord(diff+ri-i)) + (diff + ri - i) * LLSIZE * 8;
		int bdiff = bl - br;
		for (int j = 0; j <= bdiff; j++)
		{
			ModNumber mtmp2(mtmp);
			mtmp2 <<= bdiff - j;
			while (mres >= mtmp2)
			{
				mres -= mtmp2;
			}
		}
	}
	return mres;
}

ModNumber& operator%=(ModNumber& l, const ModNumber& r)
{
	if (&l == &r)				// Just optimization
		return l = ModNumber();
	ModNumber mzero;
	if (r == mzero)
		throw std::domain_error("Division by Zero");
	ModNumber mone(1ull);
	if (r == mone)		// Just optimization
		return l = ModNumber();
	ModNumber mtwo(2ull);
	if (r == mtwo)		// Just optimization
	{
		if (l.num[0] & 0x1ull)
			return l = mone;
		else
			return l = ModNumber();
	}
	if (l < r)
	{
		return l;
	}
	if (l == r)
		return l = ModNumber();
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

	for (int i = 0; i <= diff; i++)
	{
		llint tmp[COUNTLL] = {};
		for (int j = 0; j <= ri; j++)
		{
			tmp[j + diff - i] = r.num[j];
		}
		ModNumber mtmp(tmp);
		unsigned int bl = (LLSIZE * 8 - l.FindFirstNonZeroBitInWord(li)) + (li)*LLSIZE * 8;
		unsigned int br = (LLSIZE * 8 - mtmp.FindFirstNonZeroBitInWord(diff + ri - i)) + (diff + ri - i) * LLSIZE * 8;
		int bdiff = bl - br;
		for (int j = 0; j <= bdiff; j++)
		{
			ModNumber mtmp2(mtmp);
			mtmp2 <<= bdiff - j;
			while (l >= mtmp2)
			{
				l -= mtmp2;
			}
		}
	}
	return l;

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
	const lint* pn = (const lint*)n.num;
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

ModNumber operator >> (const ModNumber& n, unsigned int i)
{
	int words = 0;
	if (i >= LSIZE * 8)
	{
		if (i >= NSIZE)
			return ModNumber();
		words = i / (LSIZE * 8);
		i %= (LSIZE * 8);
	}
	const lint* pn = (const lint*)n.num;
	lint pres[COUNTL] = {};
	pres[0] = pn[words] >> i;
	for (int j = 0; j < COUNTL - words - 1; j++)
	{
		llint tmp = ((llint)pn[j + words + 1]) << ((LSIZE* 8)-i);
		pres[j] |= ((lint*)(&tmp))[0];
		pres[j + 1] = ((lint*)(&tmp))[1];
	}
	return ModNumber((llint*)pres);
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
		pn[j] = 0ul;
	return n;
}
ModNumber& operator >>= (ModNumber& n, unsigned int i)
{
	int words = 0;
	if (i >= LSIZE * 8)
	{
		if (i >= NSIZE)
			return n = ModNumber();
		words = i / (LSIZE * 8);
		i %= (LSIZE * 8);
	}
	lint* pn = (lint*)n.num;
	pn[0] = pn[words] >> i;
	for (int j = 0; j < COUNTL - words - 1; j++)
	{
		llint tmp = ((llint)pn[j + words + 1]) << ((LSIZE *8) - i);
		pn[j] |= ((lint*)(&tmp))[0];
		pn[j + 1] = ((lint*)(&tmp))[1];
	}
	for (int j = COUNTL - words; j < COUNTL; j++)
		pn[j] = 0ul;
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
		{
			if (l.num[i] > r.num[i])
				return true;
			else if (l.num[i] < r.num[i])
				return false;
		}
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
		int initial = 3-(NSIZE % 3);
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

ModNumber operator* (const ModNumber l, const ModNumber r)
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
		ModNumber tmp = l * rlint[i];
		for (int j = 0; j < i; j++)
		{
			tmp <<= LSIZE * 8;
		}
		res += tmp;
	}
	return res;
}

std::tuple<ModNumber, lint> ModNumber::DivideAndModulo(lint scalar) const
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
	int firstbits = 3 - (NSIZE %3);
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

ModNumber ModNumber::gcd(const ModNumber l,const ModNumber r)
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

ModNumber ModNumber::lcm(const ModNumber l,const ModNumber r)
{
	ModNumber gcdRes = gcd(l, r);
	ModNumber lDivGcD = l / gcdRes;
	return lDivGcD * r;
}



ModNumber MultGroupMod::Mult(const ModNumber l, const ModNumber r) const
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

ModNumber MultGroupMod::Kwad(const ModNumber x) const
{
	ModNumber l = x;
	ModNumber r = x;
	return Mult(l, r);
}

ModNumber MultGroupMod::Exp(const ModNumber x, const ModNumber e) const
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

ModNumber MultGroupMod::Diff(const ModNumber l, const ModNumber r) const
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

ModNumber MultGroupMod::Inverse(const ModNumber x) const
{
	ModNumber mzero;
	ModNumber mone(1ull);
	if (x == mzero)
		throw std::domain_error("Zero does not have an inverse");
	if (x == mone)
		return mone;
	if (x == n)
		throw std::domain_error("Zero does not have an inverse");
	ModNumber r = x % n;
	ModNumber l = n;
	std::list<ModNumber> divisors;
	std::tuple<ModNumber,ModNumber> res = DivideAndModulo(l,r);
	while (!(std::get<1>(res) == mone))
	{
		if (std::get<1>(res) == mzero)
			throw std::domain_error("Numbers are not relative prime, so there is no inverse.");
		divisors.push_back(std::get<0>(res));
		l = r;
		r = std::get<1>(res);
		res = DivideAndModulo(l, r);
	}
	divisors.push_back(std::get<0>(res));
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

#ifdef _WIN32

unsigned char *ConvertToLittleEndian(unsigned char* p, DWORD cb)
{
	unsigned char* res = new unsigned char[cb];
	for (unsigned int i = 0; i < cb; i++)
		res[i] = p[cb - i - 1];
	return res;
}
RSAParameters GetRSAKey()
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status = NCryptOpenStorageProvider(&provHandle,NULL,0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenStorageProvider returned error code");
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenKey(provHandle, &keyHandle, L"MyCoolKey", AT_KEYEXCHANGE, 0);
	if (status == NTE_BAD_KEYSET)
	{
		status = NCryptCreatePersistedKey(provHandle, &keyHandle, BCRYPT_RSA_ALGORITHM, L"MyCoolKey", AT_KEYEXCHANGE, 0);
		if (status != ERROR_SUCCESS)
			throw std::runtime_error("CryptCreateKey returned error code");
		DWORD policy = NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG | NCRYPT_ALLOW_EXPORT_FLAG;
		status = NCryptSetProperty(keyHandle, NCRYPT_EXPORT_POLICY_PROPERTY, (PBYTE)&policy, sizeof(DWORD), NCRYPT_PERSIST_FLAG);
		if (status != ERROR_SUCCESS)
			throw std::runtime_error("CryptSetProperty returned error code");
		policy = 4096;
		status = NCryptSetProperty(keyHandle, NCRYPT_LENGTH_PROPERTY, (PBYTE)&policy, sizeof(DWORD), NCRYPT_PERSIST_FLAG);
		if (status != ERROR_SUCCESS)
			throw std::runtime_error("CryptSetProperty returned error code");
		status = NCryptFinalizeKey(keyHandle, 0);
		if (status != ERROR_SUCCESS)
			throw std::runtime_error("CryptFinalizeKey returned error code");
	}
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenKey returned error code");

	BCRYPT_RSAKEY_BLOB *pkeyData;
	unsigned char rawKeyData[2331];
	pkeyData = (BCRYPT_RSAKEY_BLOB*)rawKeyData;
	DWORD size;
	status = NCryptExportKey(keyHandle, 0, BCRYPT_RSAFULLPRIVATE_BLOB, NULL, (PBYTE)pkeyData, 2331, &size, 0);
	if (status != ERROR_SUCCESS)
	{
		switch (status)
		{
			case NTE_BAD_FLAGS:
				throw std::runtime_error("CryptOpenStorageProvider returned error code: NTE_BAD_FLAGS");
			case NTE_BAD_KEY_STATE:
				throw std::runtime_error("CryptOpenStorageProvider returned error code: NTE_BAD_KEY_STATE");
			case NTE_BAD_TYPE:
				throw std::runtime_error("CryptOpenStorageProvider returned error code: NTE_BAD_TYPE");
			case NTE_INVALID_HANDLE:
				throw std::runtime_error("CryptOpenStorageProvider returned error code: NTE_INVALID_HANDLE");
			case NTE_INVALID_PARAMETER:
				throw std::runtime_error("CryptOpenStorageProvider returned error code: NTE_INVALID_PARAMETER");
			case NTE_NOT_SUPPORTED:
				throw std::runtime_error("CryptOpenStorageProvider returned error code: NTE_NOT_SUPPORTED");

			default:
				throw std::runtime_error("CryptOpenStorageProvider returned unknown error code");

		}
	}
	status = NCryptFreeObject(keyHandle);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFreeObject returned error code");

	if (pkeyData->Magic != BCRYPT_RSAFULLPRIVATE_MAGIC)
		throw std::runtime_error("Key structure not of type RSA Full Private");
	if (pkeyData->BitLength / 8 != MAXMOD)
		throw std::runtime_error("Key Bitsize not correct!");
	RSAParameters rsaParameters;
	unsigned char* p = rawKeyData + sizeof(BCRYPT_RSAKEY_BLOB);
	unsigned char *pExp = ConvertToLittleEndian(p,pkeyData->cbPublicExp);
	rsaParameters.pubExp = ModNumber(pExp, pkeyData->cbPublicExp);
	p += pkeyData->cbPublicExp;
	unsigned char* pModulus = ConvertToLittleEndian(p ,pkeyData->cbModulus);
	rsaParameters.Modulus = ModNumber (pModulus,pkeyData->cbModulus);
	p += pkeyData->cbModulus;
	unsigned char* pPrime1 = ConvertToLittleEndian(p,pkeyData->cbPrime1);
	rsaParameters.Prime1 = ModNumber(pPrime1, pkeyData->cbPrime1);
	p += pkeyData->cbPrime1;
	unsigned char* pPrime2 = ConvertToLittleEndian(p, pkeyData->cbPrime2);
	rsaParameters.Prime2 = ModNumber(pPrime2, pkeyData->cbPrime2);
	p += pkeyData->cbPrime2;
	unsigned char* pExp1 = ConvertToLittleEndian(p,pkeyData->cbPrime1);
	rsaParameters.Exp1 = ModNumber(pExp1, pkeyData->cbPrime1);
	p += pkeyData->cbPrime1;
	unsigned char* pExp2 = ConvertToLittleEndian(p, pkeyData->cbPrime2);
	rsaParameters.Exp2 = ModNumber(pExp2, pkeyData->cbPrime2);
	p += pkeyData->cbPrime2;
	unsigned char* pCoefficient = ConvertToLittleEndian(p, pkeyData->cbPrime1);
	rsaParameters.Coefficient = ModNumber(pCoefficient, pkeyData->cbPrime1);
	p += pkeyData->cbPrime1;
	unsigned char* pPrivExp = ConvertToLittleEndian(p,pkeyData->cbModulus);
	rsaParameters.PrivExp = ModNumber(pPrivExp, pkeyData->cbModulus);
	delete[] pExp;
	delete[] pModulus;
	delete[] pPrime1;
	delete[] pPrime2;
	delete[] pExp1;
	delete[] pExp2;
	delete[] pCoefficient;
	delete[] pPrivExp;
	return rsaParameters;
}
#endif
