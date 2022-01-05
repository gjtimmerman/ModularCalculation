// ModularCalculation.cpp : Defines the functions for the static library.
//

#include "pch.h"
#include "framework.h"
#include "ModularCalculation.h"




llint* CalcModulo(llint* n, llint *mod)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
	{

	}
	return nullptr;
}

llint *CalcSubtract(llint *l, llint *r)
{
	llint* res = new llint[COUNTLL];
	lint* ll = (lint*)l;
	lint* rl = (lint*)r;
	lint* resl = (lint*)res;
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

bool CalcEqual(llint* l, llint* r)
{
	for (int i = 0; i < COUNTLL; i++)
		if (l[i] != r[i])
			return false;
	return true;
}

void PrintLL(std::ostream out, llint* n)
{
	for (int i = COUNTLL - 1; i >= 0; i--)
		out << std::hex << n[i];
}

void AddAssignScalar(lint* n, int lpos, lint scalar)
{
	llint res = 0;
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
}

llint *MultiplyByScalar(llint* n, lint scalar)
{
	lint* ln = (lint*)n;
	llint* res = new llint[COUNTLL]{};
	lint* lres = (lint*)res;

	for (int i = 0; i < COUNTL; i++)
	{
		llint res = ((llint)ln[i]) * scalar;
		AddAssignScalar(lres,i, ((lint*)&res)[0]);
		AddAssignScalar(lres, i + 1, ((lint*)&res)[1]);
	}
	return res;
}