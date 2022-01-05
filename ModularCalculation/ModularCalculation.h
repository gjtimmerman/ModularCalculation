#pragma once

typedef unsigned long long int llint;
typedef unsigned long int lint;

#define LLSIZE sizeof(llint)
#define LSIZE sizeof(lint)

#define NSIZE 4096

static_assert(LLSIZE == LSIZE * 2, "Sizes are not suitable");

static_assert(LSIZE == 4, "Long size is not 4");
static_assert(LLSIZE == 8, "Long Long size is not 8");

#define COUNTL (NSIZE/8)/LSIZE
#define COUNTLL (NSIZE/8)/LLSIZE

static_assert(COUNTL == 128, "COUNTL is not 128");
static_assert(COUNTLL == 64, "COUNTLL is not 64");

const llint	lintmask = ~0ul;

llint* CalcSubtract(llint* l, llint* r);
bool CalcEqual(llint* l, llint* r);
void AddAssignScalar(lint* n, int lpos, lint scalar);