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

ModNumber &operator*=(ModNumber& n, lint scalar)
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
	n = mres;
	return n;
}

std::string ModNumber::to_string_fixed_sized_base(const int base)
{
	if (!(base == 8 || base == 16))
		throw std::invalid_argument("Only base 8 and 16 are valid");
	std::string res;
	const int buflen = NCOUNT * 4;
	int width;
	char formatchar;
	switch (base)
	{
	case 8:
		width = buflen;
		formatchar = 'o';
		break;
	case 16:
		width = buflen / 2;
		formatchar = 'x';
		break;
	}
	res.reserve(buflen * COUNTLL * 8 / base);
	char buf[buflen+1];
	const int formatlen = 8;
	char format[formatlen];
	sprintf_s(format, "%%0%dll%c", width,formatchar);
	for (int i = 0; i < COUNTLL; i++)
	{
		sprintf_s(buf, format, num);
		res.append(buf);
	}
	return res;
}

std::string ModNumber::to_string(const int base)
{
	if (!(base == 8 || base == 10 || base == 16))
		throw std::invalid_argument("Only base 8, 10 and 16 are valid");
	if (base == 8 || base == 16)
		return to_string_fixed_sized_base(base);
	std::string res;
	return res;
}