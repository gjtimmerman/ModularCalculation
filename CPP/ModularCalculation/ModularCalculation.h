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

#define MAXMOD (1024/8)
//#define MAXMOD (2048/8)
//#define MAXMOD (4096/8)

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

#define SIGNATURESIZE 128

const llint	lintmask = ~0ul;

const int OctalStringLength = (NSIZE % 3 == 0) ? (NSIZE / 3) : (NSIZE / 3 + 1);
const int DecimalStringLength = static_cast<int>(std::ceil(NSIZE * 0.30102999566398119521373889472449)); // log(2)
const int HexStringLength = NCOUNT * 2;

enum class ASNElementType : unsigned char
{
	INTEGER_VALUE = 0x02,
	OCTET_STRING = 0x04,
	NULL_VALUE = 0x05,
	OBJECT_IDENTIFIER = 0x06,
	SEQUENCE = 0x10
};

struct RSAParameters;

class ScaledNumber;

class ModNumber
{
public:
	ModNumber() = default;

	explicit ModNumber(const unsigned char* n, int count = MAXMOD) : ModNumber()
	{
		unsigned char* numc = (unsigned char*)num;
		for (int i = 0; i < count; i++)
			numc[i] = n[i];
	}
	
	explicit ModNumber(const lint* n, int count = COUNTL) : ModNumber()
	{
		lint* numl = (lint*)num;
		for (int i = 0; i < count; i++)
			numl[i] = n[i];

	}
	explicit ModNumber(const llint* n, int count = COUNTLL) : ModNumber()
	{
		for (int i = 0; i < count; i++)
			num[i] = n[i];
	}
	explicit ModNumber(llint n) : ModNumber()
	{
		num[0] = n;
	}
	std::string to_string(int base = 10) const;
	static ModNumber stomn(std::string s, int base = 10);
	static ModNumber gcd(const ModNumber &l,const ModNumber &r);
	static ModNumber lcm(const ModNumber &l, const ModNumber &r);
	template <typename T>
	std::basic_string<T> getText() const;
	template <typename T>
	static ModNumber fromText(std::basic_string<T> text);
	ModNumber sqrt() const;

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

	friend ModNumber& operator-=(ModNumber& l, const lint &r);
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
	friend ModNumber operator* (const ModNumber& l, const ModNumber& r);
	friend ModNumber operator/ (const ModNumber& l, const ModNumber& r);
	friend std::tuple<ModNumber, ModNumber> DivideAndModulo(const ModNumber& l, const ModNumber& r);
	friend unsigned int GetByteCount(const ModNumber& mn);
	friend void SetRSAKey(const wchar_t* KeyName, RSAParameters rsaParameters);
	friend unsigned char* CopyKeyPart(const ModNumber& mn, unsigned int cbsize, unsigned char* pDest);
	friend std::tuple<ModNumber, unsigned long> decrypt(const wchar_t *KeyName,const ModNumber& data);
	friend ModNumber encrypt(const wchar_t* KeyName,const ModNumber& data);
	friend bool verify(const wchar_t* keyName, unsigned char* hash, int hashLength, ModNumber signature, const wchar_t *hashAlgorithm);
	friend class ScaledNumber;
	friend class MultGroupMod;
	friend class RSA;
	friend unsigned char* ConvertEndianess(ModNumber m);
	friend ModNumber GetLeftMostBytes(ModNumber m, unsigned int n);
	friend ModNumber GetPKCS1Mask(const ModNumber& m, bool stable, int modulusSize);
	friend ModNumber RemovePKCS1Mask(const ModNumber& m);
	friend ModNumber CreateBERASNString(std::list<std::string> content);
	friend std::list<std::string> ParseBERASNString(const ModNumber& m);
	friend std::tuple<ASNElementType, unsigned int, unsigned int> ReadASNElement(unsigned char* p, unsigned int i);

};

class ScaledNumber
{
public:
	ScaledNumber(ModNumber mn, int scale, bool alreadyScaled = false) : mn(alreadyScaled ? mn : mn << scale * 8), scale(scale)
	{
	}
	ScaledNumber sqrt();
	std::string to_string(int base = 10) const;
	std::tuple<unsigned int, unsigned int> calculateOctalStringLength() const;
	unsigned int calculateDecimalStringLengthLeft() const;
	int scale;
private:
	std::string to_string_hex_base() const;
	std::string to_string_octal_base() const;
	std::string to_string_decimal_base() const;

	ModNumber mn;
	friend bool operator ==(ScaledNumber l, ScaledNumber r);
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
	ModNumber Mult(const ModNumber& l, const ModNumber& r) const;
	ModNumber Kwad(const ModNumber& x) const;
	ModNumber Exp(const ModNumber& x, const ModNumber& e) const;
	ModNumber Add(const ModNumber& l, const ModNumber& r) const;
	ModNumber Diff(const ModNumber& l, const ModNumber& r) const;
	ModNumber Inverse(const ModNumber& x) const;

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
	ModNumber Encrypt(const ModNumber& m) const;
	ModNumber Decrypt(const ModNumber& c) const;
	ModNumber DecryptSignature(const ModNumber signature) const;
	ModNumber EncryptSignature(std::string hashBigEndian, std::string hashOid) const;
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

struct DSAParameters
{
public:
	ModNumber Q;
	ModNumber P;
	ModNumber g;
	ModNumber x;
	ModNumber y;

};

class DSA
{
public:
	DSA(DSAParameters p) : Q(p.Q), P(p.P), g(p.g),x(p.x), y(p.y)
	{

	}
	bool Verify(std::string hash, std::string signature);
private:
	ModNumber Q;
	ModNumber P;
	ModNumber g;
	ModNumber x;
	ModNumber y;
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
ModNumber operator* (const ModNumber& l, const ModNumber& r);
ModNumber& operator*=(ModNumber& l, const ModNumber& r);
ModNumber operator/ (const ModNumber& l, const ModNumber& r);
std::tuple<ModNumber, ModNumber> DivideAndModulo(const ModNumber& l, const ModNumber& r);
unsigned char* CopyKeyPart(const ModNumber& mn, unsigned int cbsize, unsigned char* pDest);
bool operator ==(ScaledNumber l, ScaledNumber r);

unsigned char* ConvertEndianess(const unsigned char* p, unsigned int cb);
unsigned char* ConvertEndianess(ModNumber m);
ModNumber GetLeftMostBytes(ModNumber m, unsigned int n);

ModNumber GetPKCS1Mask(const ModNumber& m, bool stable = false, int modulusSize = MAXMOD);
ModNumber RemovePKCS1Mask(const ModNumber& m);
ModNumber CreateBERASNString(std::list<std::string> content);
std::list<std::string> ParseBERASNString(const ModNumber& m);
std::tuple<ASNElementType, unsigned int, unsigned int> ReadASNElement(unsigned char* p, unsigned int i);


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

template <typename T>
std::basic_string<T> ModNumber::getText() const
{
	std::basic_string<T> res;
	unsigned int byteCount = GetByteCount(*this);
	unsigned int charlen = (unsigned int)sizeof(typename std::basic_string<T>::traits_type::char_type);
	if (byteCount % charlen != 0)
		byteCount += charlen - (byteCount % charlen);
	res.reserve(byteCount / charlen);
	res.clear();
	typename std::basic_string<T>::traits_type::char_type* pText = (typename std::basic_string<T>::traits_type::char_type*)this;
	for (unsigned int i = 0; i < byteCount/charlen; i++)
		res.push_back(pText[i]);
	return res;
}


#ifdef _WIN32
NCRYPT_KEY_HANDLE GenerateKey(const wchar_t* KeyName, NCRYPT_PROV_HANDLE provHandle, const wchar_t* algorithm = L"RSA", int usage = AT_KEYEXCHANGE);
RSAParameters GetRSAKey(const wchar_t *KeyName, bool createIfNotExists);
void SetRSAKey(const wchar_t* KeyName, RSAParameters rsaParameters);
std::tuple<ModNumber, DWORD> decrypt(const wchar_t *KeyName,const ModNumber& data);
ModNumber encrypt(const wchar_t* KeyName,const ModNumber& data);
ModNumber sign(const wchar_t* keyName, unsigned char* hash, int count, const wchar_t* hashAlgorithm = L"SHA256");
bool verify(const wchar_t* keyName, unsigned char* hash, int hashLength, ModNumber signature, const wchar_t* hashAlgorithm = L"SHA256");
std::tuple<unsigned char*, ULONG> hash(unsigned char *data, size_t count, const wchar_t *hashAlgorithm = L"SHA256");
#endif


