#pragma once


typedef unsigned long long int llint;
#ifdef _WIN32
typedef unsigned long int lint;
#else
typedef unsigned int lint;
#endif

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

const int OctalStringLength = NSIZE / 3 + 1;
const int DecimalStringLength = OctalStringLength;
const int HexStringLenght = NCOUNT * 2;

class ModNumber
{
public:
	ModNumber()
	{
		memset(num, 0, NCOUNT);
	}
	explicit ModNumber(llint* n)
	{
		for (int i = 0; i < COUNTLL; i++)
			num[i] = n[i];
	}
	explicit ModNumber(llint n) : ModNumber()
	{
		num[0] = n;
	}
	std::string to_string(const int base = 10) const;
	static ModNumber stomn(std::string s, int base = 10);


private:
	llint num[COUNTLL];
	std::string to_string_hex_base() const;
	std::string to_string_octal_base() const;
	std::string to_string_decimal_base() const;
	static ModNumber stomn_hex_base(std::string s);
	ModNumber& AddAssignScalar(int lpos, lint scalar);
	std::tuple<ModNumber, lint> DivideAndModulo(lint scalar) const;


	friend ModNumber operator-(const ModNumber& l, const ModNumber& r);
	friend bool operator==(const ModNumber& l, const ModNumber& r);
	friend ModNumber& operator *=(ModNumber& n, lint scalar);
	friend ModNumber& operator+=(ModNumber& n, lint scalar);
	friend ModNumber operator/ (const ModNumber& n, lint scalar);
	friend ModNumber& operator/= (ModNumber& n, lint scalar);
	friend ModNumber operator* (ModNumber& n, lint scalar);
	friend lint operator% (const ModNumber& n, lint scalar);
	friend std::ostream& operator << (std::ostream& out, ModNumber& n);

};

ModNumber operator-(const ModNumber& l, const ModNumber& r);
bool operator==(const ModNumber& l, const ModNumber& r);
ModNumber& operator *=(ModNumber& n, lint scalar);
ModNumber& operator+=(ModNumber& n, lint scalar);
ModNumber operator/ (const ModNumber& n, lint scalar);
ModNumber& operator/= (ModNumber& n, lint scalar);
ModNumber operator* (ModNumber& n, lint scalar);
lint operator% (const ModNumber& n, lint scalar);
std::ostream& operator<<(std::ostream& out, ModNumber& n);


