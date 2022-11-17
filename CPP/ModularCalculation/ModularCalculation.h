#pragma once

#ifdef _WIN32
#include <windows.h>
#include <ncrypt.h>
#include <wincrypt.h>
#endif

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

struct RSAParameters;

class ModNumber
{
public:
	ModNumber() = default;

	explicit ModNumber(unsigned char* n, int count = MAXMOD) : ModNumber()
	{
		unsigned char* numc = (unsigned char*)num;
		for (int i = 0; i < count; i++)
			numc[i] = n[i];
	}
	
	explicit ModNumber(lint* n, int count = COUNTL) : ModNumber()
	{
		lint* numl = (lint*)num;
		for (int i = 0; i < count; i++)
			numl[i] = n[i];

	}
	explicit ModNumber(llint* n, int count = COUNTLL) : ModNumber()
	{
		for (int i = 0; i < count; i++)
			num[i] = n[i];
	}
	explicit ModNumber(llint n) : ModNumber()
	{
		num[0] = n;
	}
	std::string to_string(const int base = 10) const;
	static ModNumber stomn(std::string s, int base = 10);
	static ModNumber gcd(const ModNumber l,const ModNumber r);
	static ModNumber lcm(const ModNumber l, const ModNumber r);
	template <typename T>
	static ModNumber fromText(std::basic_string<T> text);


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
	friend ModNumber operator* (const ModNumber l, const ModNumber r);
	friend ModNumber operator/ (const ModNumber& l, const ModNumber& r);
	friend std::tuple<ModNumber, ModNumber> DivideAndModulo(const ModNumber& l, const ModNumber& r);
	friend unsigned int GetByteCount(ModNumber mn);
	friend void SetRSAKey(const wchar_t* KeyName, RSAParameters rsaParameters);
	friend unsigned char* CopyKeyPart(ModNumber mn, unsigned int cbsize, unsigned char* pDest);
	friend class MultGroupMod;
	friend class RSA;

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

struct RSAParameters
{
	ModNumber pubExp;
	ModNumber Modulus;
	ModNumber Prime1;
	ModNumber Prime2;
	ModNumber Exp1;		// DP
	ModNumber Exp2;		// DQ
	ModNumber Coefficient;	// InverseQ
	ModNumber PrivExp;
};

class RSA
{
public:
	RSA(RSAParameters rsaParameters)
	{
		pubExp = rsaParameters.pubExp;
		Modulus = rsaParameters.Modulus;
		Prime1 = rsaParameters.Prime1;
		Prime2 = rsaParameters.Prime2;
		Exp1 = rsaParameters.Exp1;
		Exp2 = rsaParameters.Exp2;
		Coefficient = rsaParameters.Coefficient;
		PrivExp = rsaParameters.PrivExp;
	}
	ModNumber GetPKCS1Mask(ModNumber m) const;
	ModNumber Encrypt(ModNumber m) const;
private:
	ModNumber pubExp;
	ModNumber Modulus;
	ModNumber Prime1;
	ModNumber Prime2;
	ModNumber Exp1;		// DP
	ModNumber Exp2;		// DQ
	ModNumber Coefficient;	// InverseQ
	ModNumber PrivExp;

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
ModNumber operator* (const ModNumber l, const ModNumber r);
ModNumber operator/ (const ModNumber& l, const ModNumber& r);
std::tuple<ModNumber, ModNumber> DivideAndModulo(const ModNumber& l, const ModNumber& r);
unsigned char* CopyKeyPart(ModNumber mn, unsigned int cbsize, unsigned char* pDest);


template <typename T>
ModNumber ModNumber::fromText(std::basic_string<T> text)
{
	llint* pText = (llint*)text.c_str();
	std::size_t charlen = sizeof(typename std::basic_string<T>::traits_type::char_type);
	std::size_t textSize = text.length() * charlen / LLSIZE;
	std::size_t textLeft = text.length() * charlen % LLSIZE;
	llint res[COUNTLL] = {};
	if (textSize > COUNTLL)
		throw std::domain_error("Text message too long");
	if (textSize == COUNTLL && textLeft > 0)
		throw std::domain_error("Text message too long");
	for (unsigned int i = 0; i < textSize; i++)
		res[i] = pText[i];
	llint tmp = 0;
	for (unsigned int i = 0; i < textLeft/charlen; i++)
	{
		llint c = text[(textSize * LLSIZE)/charlen + i];
		c <<= 8 * charlen * i;
		tmp |= (llint)c;
	}
	if (textLeft > 0)
		res[textSize] = tmp;
	return ModNumber(res);
}


#ifdef _WIN32
RSAParameters GetRSAKey(wchar_t *KeyName);
void SetRSAKey(const wchar_t* KeyName, RSAParameters rsaParameters);
#endif


