#pragma once

typedef unsigned long long int llint;
typedef unsigned long int lint;

#define LLSIZE sizeof(llint)
#define LSIZE sizeof(lint)

#define NSIZE 4096

#define NCOUNT NSIZE/8

static_assert(LLSIZE == LSIZE * 2, "Sizes are not suitable");

//static_assert(LSIZE == 4, "Long size is not 4");
//static_assert(LLSIZE == 8, "Long Long size is not 8");

#define COUNTL NCOUNT/LSIZE
#define COUNTLL NCOUNT/LLSIZE

//static_assert(COUNTL == 128, "COUNTL is not 128");
//static_assert(COUNTLL == 64, "COUNTLL is not 64");

const llint	lintmask = ~0ul;

class ModNumber
{
public:
	ModNumber()
	{
		memset(num, 0, NCOUNT);
	}
	ModNumber(llint* n)
	{
		for (int i = 0; i < COUNTLL; i++)
			num[i] = n[i];
	}
	ModNumber(llint n) : ModNumber()
	{
		num[0] = n;
	}
	ModNumber& AddAssignScalar(int lpos, lint scalar);
	std::string to_string_hex_base();
	std::string to_string_octal_base();
	std::string to_string(const int base);


private:
	llint num[COUNTLL];

	friend ModNumber operator-(const ModNumber& l, const ModNumber& r);
	friend bool operator==(const ModNumber& l, const ModNumber& r);
	friend ModNumber& operator *=(ModNumber& n, lint scalar);
	friend std::ostream& operator << (std::ostream& out, ModNumber& n);
	friend std::string operator<<(std::string s, const ModNumber& n);

};

ModNumber operator-(const ModNumber& l, const ModNumber& r);
bool operator==(const ModNumber& l, const ModNumber& r);
ModNumber& operator *=(ModNumber& n, lint scalar);
std::ostream& operator<<(std::ostream& out, ModNumber& n);
std::string operator<<(std::string s, const ModNumber& n);


