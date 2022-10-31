#pragma once


typedef unsigned long long int llint;
#ifdef _WIN32
typedef unsigned long int lint;
#else
typedef unsigned int lint;
#endif

#define LLSIZE sizeof(llint)
#define LSIZE sizeof(lint)

#define MAXMOD (4096/8)

#define NSIZE (NCOUNT*8)

#define NCOUNT (MAXMOD + LLSIZE)

static_assert(LLSIZE == LSIZE * 2, "Sizes are not suitable");

//static_assert(LSIZE == 4, "Long size is not 4");
//static_assert(LLSIZE == 8, "Long Long size is not 8");

#define COUNTL NCOUNT/LSIZE
#define COUNTLL NCOUNT/LLSIZE
#define COUNTMOD MAXMOD/LLSIZE

//static_assert(COUNTL == 128, "COUNTL is not 128");
//static_assert(COUNTLL == 64, "COUNTLL is not 64");


const llint	lintmask = ~0ul;

const int OctalStringLength = NSIZE / 3 + 1;
const int DecimalStringLength = static_cast<int>(std::ceil(NSIZE * 0.30102999566398119521373889472449)); // log(2)
const int HexStringLength = NCOUNT * 2;

class ModNumber
{
public:
	ModNumber() = default;
	
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
	static ModNumber gcd(ModNumber l, ModNumber r);


private:
	llint num[COUNTLL] = {};
	std::string to_string_hex_base() const;
	std::string to_string_octal_base() const;
	std::string to_string_decimal_base() const;
	static ModNumber stomn_hex_base(std::string s);
	static ModNumber stomn_decimal_base(std::string s);
	static ModNumber stomn_octal_base(std::string s);
	static std::string AdjustStringLength(std::string s, size_t desiredLength);
	ModNumber& AddAssignScalar(int lpos, lint scalar);
	std::tuple<ModNumber, lint> DivideAndModulo(lint scalar) const;

	unsigned int FindFirstNonZeroBitInWord(unsigned int word) const;
	void checkMax(int size)
	{
		for (int i = size; i < COUNTLL; i++)
			if (num[i] != 0ull)
				throw std::domain_error("Number is above maximum.");

	}

	friend ModNumber operator-(const ModNumber& l, const ModNumber& r);
	friend ModNumber& operator-=(ModNumber& l, const ModNumber& r);
	friend bool operator==(const ModNumber& l, const ModNumber& r);
	friend ModNumber& operator *=(ModNumber& n, lint scalar);
	friend ModNumber& operator+=(ModNumber& n, lint scalar);
	friend ModNumber operator/ (const ModNumber& n, lint scalar);
	friend ModNumber& operator/= (ModNumber& n, lint scalar);
	friend ModNumber operator* (const ModNumber& n, lint scalar);
	friend ModNumber operator+ (const ModNumber& n, lint scalar);
	friend ModNumber operator+ (const ModNumber& l, const ModNumber &r);
	friend ModNumber& operator+= (ModNumber& l, const ModNumber& r);
	friend lint operator% (const ModNumber& n, lint scalar);
	friend bool operator < (const ModNumber& l, const ModNumber& r);
	friend bool operator > (const ModNumber& l, const ModNumber& r);
	friend bool operator <= (const ModNumber& l, const ModNumber& r);
	friend bool operator >= (const ModNumber& l, const ModNumber& r);
	friend ModNumber operator%(const ModNumber& l, const ModNumber& r);
	friend ModNumber& operator%=(ModNumber& l, const ModNumber& r);
	friend ModNumber operator<<(const ModNumber& n, const unsigned int i);
	friend ModNumber& operator <<= (ModNumber& n, unsigned int i);
	friend ModNumber operator>>(const ModNumber& n, const unsigned int i);
	friend ModNumber& operator >>= (ModNumber& n, unsigned int i);
	friend std::ostream& operator << (std::ostream& out,const ModNumber& n);
	friend std::istream& operator>>(std::istream& in, ModNumber& n);
	friend std::tuple<ModNumber, ModNumber> DivideAndModulo(const ModNumber& l, const ModNumber& r);

	friend class MultGroupMod;

};

class MultGroupMod
{
public:
	MultGroupMod(ModNumber n) : n(n)
	{
		ModNumber mzero;
		if (n == mzero)
			throw std::domain_error("Group modulo zero is not allowed");
		ModNumber mone(1ull);
		if (n == mone)
			throw std::domain_error("Group modulo one is not allowed");
		n.checkMax(COUNTMOD);
	}
	ModNumber Mult(const ModNumber l, const ModNumber r) const;
	ModNumber Kwad(const ModNumber x) const;
	ModNumber Exp(const ModNumber x, const ModNumber e) const;
	ModNumber Diff(const ModNumber l, const ModNumber r) const;
	ModNumber Inverse(const ModNumber x) const;

private:
	ModNumber n;
};

ModNumber operator-(const ModNumber& l, const ModNumber& r);
ModNumber& operator-=(ModNumber& l, const ModNumber& r);
bool operator==(const ModNumber& l, const ModNumber& r);
ModNumber& operator *=(ModNumber& n, lint scalar);
ModNumber& operator+=(ModNumber& n, lint scalar);
ModNumber operator/ (const ModNumber& n, lint scalar);
ModNumber& operator/= (ModNumber& n, lint scalar);
ModNumber operator* (const ModNumber& n, lint scalar);
ModNumber operator+ (const ModNumber& n, lint scalar);
lint operator% (const ModNumber& n, lint scalar);
bool operator < (const ModNumber& l, const ModNumber& r);
bool operator > (const ModNumber& l, const ModNumber& r);
bool operator <= (const ModNumber& l, const ModNumber& r);
bool operator >= (const ModNumber& l, const ModNumber& r);
ModNumber operator%(const ModNumber& l, const ModNumber& r);
ModNumber operator<<(const ModNumber& n, const unsigned int i);
ModNumber& operator <<= (ModNumber& n, unsigned int i);
ModNumber operator>>(const ModNumber& n, const unsigned int i);
ModNumber& operator >>= (ModNumber& n, unsigned int i);
std::ostream& operator<<(std::ostream& out,const ModNumber& n);
std::istream& operator>>(std::istream& in, ModNumber& n);
std::tuple<ModNumber, ModNumber> DivideAndModulo(const ModNumber& l, const ModNumber& r);


