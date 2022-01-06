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