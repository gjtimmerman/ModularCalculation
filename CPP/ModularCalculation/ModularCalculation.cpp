// ModularCalculation.cpp : Defines the functions for the static library.
//

#include "pchmc.h"
#include "framework.h"
#include "ModularCalculation.h"




ModNumber& operator-=(ModNumber& l, const lint &r)
{
	lint* ll = (lint*)l.num;
	lint carry = 0;
	lint ltmp = ll[0];
	if (ltmp < r)
	{
		carry = 1;
	}
	ll[0] = ltmp - r;
	int i = 1;
	while (carry > 0 && i < COUNTL)
	{
		if (carry <= ll[i])
		{
			ll[i] -= carry;
			carry = 0;
		}
		else
			ll[i] -= carry;
		i++;
	}
	return l;
}

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
		{
			if (l.num[i] == r.num[i])
				continue;
			else
				return l.num[i] < r.num[i];
		}
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
			if (l.num[i] == r.num[i])
				continue;
			else
				return l.num[i] > r.num[i];
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
		ModNumber tmp = l * rlint[i];
		for (int j = 0; j < i; j++)
		{
			tmp <<= LSIZE * 8;
		}
		res += tmp;
	}
	return res;
}

ModNumber& operator*=(ModNumber& l, const ModNumber& r)
{
	ModNumber mres = l * r;
	l = mres;
	return l;
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
		ModNumber factor(1ull);
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



unsigned char *ConvertEndianess(const unsigned char* p, unsigned int cb)
{
	unsigned char* res = new unsigned char[cb];
	for (unsigned int i = 0; i < cb; i++)
		res[i] = p[cb - i - 1];
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

ModNumber RSA::GetPKCS1Mask(const ModNumber& m) const
{
	unsigned long keyByteSize = GetByteCount(Modulus);
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
	llint tmp = 0x0002;
	unsigned long totalBytesShift = totalBytesLeft;
	if (totalBytesLeft < 2)
		totalBytesShift += 8;
	for (unsigned int i = 0; i < totalBytesShift - 2; i++)
	{
		tmp <<= 8;
		unsigned char mask = (unsigned char)rand() % 0x100;
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
			unsigned char mask = ((unsigned char)rand() % 0xFF) + 1u;
			tmp |= mask;
		}
		res.num[totalNumWords - i - 2] = tmp;
	}
	tmp = 0;
	for (unsigned int j = 0; j < padLeftOver; j++)
	{
		unsigned char mask = (unsigned char)rand() % 0x100;
		tmp |= mask;
		tmp <<= 8;
	}
	tmp <<= (LLSIZE - padLeftOver - 1) * 8;
	tmp |= m.num[mCount];
	res.num[totalNumWords - padLeftCount - 2] = tmp;
	for (unsigned int i = 0; i < mCount; i++)
	{
		res.num[i] = m.num[i];
	}
	return res;
}

ModNumber RSA::RemovePKCS1Mask(const ModNumber& m) const
{
	unsigned char* pMaskedNumber = (unsigned char*)m.num;
	int i;
	for (i = MAXMOD - 1; i >= 0; i--)
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
	ModNumber res(pMaskedNumber, i);
	return res;

}

std::tuple<ASNElementType,unsigned int, unsigned int> RSA::ReadASNElement(unsigned char* p, unsigned int i) const
{
	if (i <= 0)
		throw std::domain_error("Not a valid BER encoding!");
	switch (p[i] >> 6)
	{
	case 0:
	{
		switch (p[i] >> 5)
		{
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
						default:
							throw std::domain_error("Not a short length specifier!");

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

std::tuple<ModNumber,int> RSA::ParseBERASNString(const ModNumber& m) const
{
	unsigned char* pMaskedNumber = (unsigned char*)m.num;
	unsigned int i;
	for (i = MAXMOD - 1; i >= 0; i--)
	{
		if (pMaskedNumber[i])
			break;
	}
	std::tuple<ASNElementType,unsigned int, unsigned int> ASNElement  = ReadASNElement(pMaskedNumber,i);
	if (std::get<0>(ASNElement) == ASNElementType::SEQUENCE)
	{
		ASNElement = ReadASNElement(pMaskedNumber,std::get<2>(ASNElement));
		if (std::get<0>(ASNElement) == ASNElementType::SEQUENCE)
		{
			ASNElement = ReadASNElement(pMaskedNumber, std::get<2>(ASNElement));

		}

	}

	//if (i > 0 && pMaskedNumber[i--] != 0x30)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x31)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x30)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x0D)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x06)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x09)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x60)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x86)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x48)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x01)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x65)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x03)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x04)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x02)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x01)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x05)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x00)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x04)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	//if (i > 0 && pMaskedNumber[i--] != 0x20)
	//	throw std::domain_error("Not a valid RSA SHA256 OID");
	ModNumber res(pMaskedNumber, i + 1);
	return std::make_tuple(res, i + 1);

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
	return RemovePKCS1Mask(res);
	return res;
}

ModNumber RSA::DecryptSignature(const ModNumber signature) const
{
	MultGroupMod mgm(Modulus);
	ModNumber decryptedSignature = mgm.Exp(signature, pubExp);
	ModNumber removedMask = RemovePKCS1Mask(decryptedSignature);
	std::tuple<ModNumber, int> result = ParseBERASNString(removedMask);
	ModNumber hashBigEndian = std::get<0>(result);
	int hashLen = std::get<1>(result);
	unsigned char* pHashBigEndian = (unsigned char*)hashBigEndian.num;
	unsigned char* pHashLittleEndian = ConvertEndianess(pHashBigEndian, hashLen);
	return ModNumber(pHashLittleEndian, hashLen);
}



#ifdef _WIN32 


int evaluateStatus(SECURITY_STATUS status)
{
	if (status == ERROR_SUCCESS)
		return 0;

	switch (status)
	{
	case NTE_BAD_FLAGS:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_BAD_FLAGS");
	case NTE_BAD_KEY_STATE:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_BAD_KEY_STATE");
	case NTE_BAD_TYPE:
		throw std::runtime_error("CryptEncrypt returned error code: NTE_BAD_TYPE");
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

NCRYPT_KEY_HANDLE GenerateKey(const wchar_t* KeyName, NCRYPT_PROV_HANDLE provHandle)
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
	status = NCryptCreatePersistedKey(provHandle, &keyHandle, BCRYPT_RSA_ALGORITHM, KeyName, AT_KEYEXCHANGE, 0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptCreateKey returned error code");
	DWORD policy = NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG | NCRYPT_ALLOW_EXPORT_FLAG;
	status = NCryptSetProperty(keyHandle, NCRYPT_EXPORT_POLICY_PROPERTY, (PBYTE)&policy, sizeof(DWORD), NCRYPT_PERSIST_FLAG);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptSetProperty returned error code");
	policy = MAXMOD*8;
	status = NCryptSetProperty(keyHandle, NCRYPT_LENGTH_PROPERTY, (PBYTE)&policy, sizeof(DWORD), NCRYPT_PERSIST_FLAG);
	evaluateStatus(status);
	status = NCryptFinalizeKey(keyHandle, 0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptFinalizeKey returned error code");
	if (tmpProvHandle != NULL)
	{
		status = NCryptFreeObject(tmpProvHandle);
		if (status != ERROR_SUCCESS)
			throw std::runtime_error("CryptFreeObject returned error code");
	}
	return keyHandle;

}

RSAParameters GetRSAKey(const wchar_t *KeyName, bool createIfNotExists)
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status = NCryptOpenStorageProvider(&provHandle,NULL,0);
	if (status != ERROR_SUCCESS)
		throw std::runtime_error("CryptOpenStorageProvider returned error code");
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenKey(provHandle, &keyHandle, KeyName, AT_KEYEXCHANGE, 0);
	if (status == NTE_BAD_KEYSET && createIfNotExists)
	{
		keyHandle = GenerateKey(KeyName, provHandle);
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
	default:
		throw std::runtime_error("Cryptographic function returned unknown error code");
	}

}

std::tuple<unsigned char*, ULONG> hash(unsigned char *data, size_t count)
{
	BCRYPT_ALG_HANDLE algHandle;
	NTSTATUS bStatus;
	bStatus = BCryptOpenAlgorithmProvider(&algHandle, BCRYPT_SHA256_ALGORITHM, NULL, 0);
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

ModNumber sign(const wchar_t* keyName, unsigned char* hash, int hashLength)
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status;
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenStorageProvider(&provHandle, NULL, 0);
	evaluateStatus(status);
	status = NCryptOpenKey(provHandle, &keyHandle, keyName, AT_KEYEXCHANGE, 0);
	unsigned char signature[128];
	ULONG cbResult;
	BCRYPT_PKCS1_PADDING_INFO paddingInfo;
	paddingInfo.pszAlgId = BCRYPT_SHA256_ALGORITHM;
	status = NCryptSignHash(keyHandle, &paddingInfo, hash, hashLength, signature, 128, &cbResult, BCRYPT_PAD_PKCS1);
	evaluateStatus(status);
	status = NCryptFreeObject(keyHandle);
	evaluateStatus(status);
	unsigned char *pSignatureLittleEndian = ConvertEndianess(signature, cbResult);
	ModNumber retvalue(pSignatureLittleEndian, cbResult);
	return retvalue; 
}

bool verify(const wchar_t* keyName, unsigned char* hash, int hashLength, ModNumber signature)
{
	NCRYPT_PROV_HANDLE provHandle;
	SECURITY_STATUS status;
	NCRYPT_KEY_HANDLE keyHandle;
	status = NCryptOpenStorageProvider(&provHandle, NULL, 0);
	evaluateStatus(status);
	status = NCryptOpenKey(provHandle, &keyHandle, keyName, AT_KEYEXCHANGE, 0);
	unsigned char *littleEndianSignature = (unsigned char *)signature.num;
	unsigned char *bigEndiansignature = ConvertEndianess(littleEndianSignature,SIGNATURESIZE);
	BCRYPT_PKCS1_PADDING_INFO paddingInfo;
	paddingInfo.pszAlgId = BCRYPT_SHA256_ALGORITHM;
	status = NCryptVerifySignature(keyHandle, &paddingInfo, hash, hashLength, bigEndiansignature, SIGNATURESIZE, BCRYPT_PAD_PKCS1);
	evaluateStatus(status);
	status = NCryptFreeObject(keyHandle);
	evaluateStatus(status);
	return status == ERROR_SUCCESS;
}
#endif
