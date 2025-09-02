# ModularCalculation

A modular, extensible cryptographic library focused on modular arithmetic and public-key cryptography operations, with support for RSA, DSA, and Elliptic Curve Cryptography (ECC).  
This repository features cross-platform C++ and Java implementations, utilities for ASN.1 and PKCS1 encoding, and comprehensive unit tests.

---

## Features

- **Big Number Arithmetic**: Efficient modular operations for large integers (addition, multiplication, exponentiation, division, etc.).
- **Cryptographic Algorithms**: 
  - **RSA**: Key generation, encryption, decryption, digital signatures.
  - **DSA**: Digital signature generation and verification.
  - **Elliptic Curve Cryptography (ECC)**: Key agreement (ECDH), digital signatures (ECDSA).
- **Key Management**: Key generation, import/export, and secure storage.
- **Hashing**: Support for SHA256 and other algorithms.
- **ASN.1/PKCS1 Utilities**: Encoding and decoding for secure data transfer and signature padding.
- **Cross-Platform**: Core logic is portable. Windows-specific code uses CNG APIs; platform abstractions allow future Linux/macOS support.
- **Unit Testing**: Modular test suite to validate cryptographic operations and arithmetic.

---

## Getting Started

### Prerequisites

- **C++**: C++17 or later; CMake recommended for building.
- **Java**: JDK 8 or later.
- **Windows**: For C++ Windows-specific features, Windows Cryptography API: Next Generation (CNG).

### Build (C++)

```bash
cd CPP
mkdir build && cd build
cmake ..
cmake --build .
```

### Build (Java)

```bash
cd Java/ModularCalculation
./gradlew build
```

---

## Usage

### C++ Example: RSA Encryption

```cpp
#include "ModularCalculation.h"

ModNumber message = ModNumber::fromText("Hello, world!");
ModNumber encrypted = encrypt(L"MyRSAKey", message);
auto [decrypted, length] = decrypt(L"MyRSAKey", encrypted);
std::string plain = decrypted.getText<char>();
```

### Java Example: RSA Encryption/Decryption

```java
RSAParameters params = ...; // Initialize with key material
RSA rsa = new RSA(params);
ModNumber message = ModNumber.fromText("Hello, world!");
ModNumber encrypted = rsa.encrypt(message, 256);
ModNumber decrypted = rsa.decrypt(encrypted);
String plain = decrypted.getText();
```

---

## Project Structure

```
CPP/
  ModularCalculation/         # Core C++ implementation
  ModularCalculationMain/     # C++ entry point
  ModularUnitTests/           # Unit tests

Java/
  ModularCalculation/         # Java implementation
```

---

## Testing

To run the C++ tests:

```bash
cd CPP/ModularCalculationMain
./ModularCalculationMain
```

---

## Security Notice

This library is for research and educational purposes.  
Before using in production or security-critical applications, conduct a full code audit and consider using well-established cryptographic libraries.

---

## Contributing

Contributions are welcome! Please open issues and pull requests for bugs, feature requests, or improvements.

---

## Author

[Gerrit Jan Timmerman](https://github.com/gjtimmerman)
