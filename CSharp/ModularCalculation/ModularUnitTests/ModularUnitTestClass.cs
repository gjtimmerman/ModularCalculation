//#define LARGEMOD
//#define LARGEMODSIGNATURE
//#define MEDMOD
#define SMALLMOD

using ModularCalculation;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace ModularUnitTests
{
    [TestClass]
    public class ModularUnitTestClass
    {
        [ClassInitialize]
        public static void InitializeKeys(TestContext context)
        {
            CngKey ?cngKeySigningRsa = null;
            CngKeyCreationParameters parametersSigningRsa = new CngKeyCreationParameters();
            parametersSigningRsa.KeyUsage = CngKeyUsages.Signing;

            CngKey ?cngKeyEncryptionRsa = null;
            CngKeyCreationParameters parametersEncryptionRsa = new CngKeyCreationParameters();
            parametersEncryptionRsa.KeyUsage = CngKeyUsages.Decryption;

            CngKey ?cngKeySigningDsa = null;
            CngKeyCreationParameters parametersSigningDsa = new CngKeyCreationParameters();
            parametersSigningDsa.KeyUsage = CngKeyUsages.Signing;


            byte[] keyLength = new byte[4];
            keyLength[0] = (ModNumber.MaxMod * 8) % 0x100;
            keyLength[1] = (ModNumber.MaxMod * 8) / 0x100;
            CngPropertyOptions options = new CngPropertyOptions();

            CngProperty property = new CngProperty("Length", keyLength, options);
            parametersSigningRsa.Parameters.Add(property);
            parametersSigningDsa.Parameters.Add(property);
            parametersEncryptionRsa.Parameters.Add(property);
            byte[] exportp = new byte[4];
            exportp[0] = 0x03;
            property = new CngProperty("Export Policy", exportp, options);
            parametersSigningRsa.Parameters.Add(property);
            parametersSigningDsa.Parameters.Add(property);
            parametersEncryptionRsa.Parameters.Add(property);
#if LARGEMOD
            cngKeySigningRsa = CngKey.Create(CngAlgorithm.Rsa, "MyCoolRSASignatureKey4096", parametersSigningRsa);
            cngKeyEncryptionRsa = CngKey.Create(CngAlgorithm.Rsa, "MyCoolRSAKey4096", parametersEncryptionRsa);
#elif LARGEMODSIGNATURE
            cngKeySigningDsa = CngKey.Create(new CngAlgorithm("DSA"), "MyCoolDSAKey3072", parametersSigningDsa);

#elif MEDMOD
            cngKeySigningRsa = CngKey.Create(CngAlgorithm.Rsa, "MyCoolRSASignatureKey2048", parametersSigningRsa);
            cngKeySigningDsa = CngKey.Create(new CngAlgorithm("DSA"), "MyCoolDSAKey2048", parametersSigningDsa);
            cngKeyEncryptionRsa = CngKey.Create(CngAlgorithm.Rsa, "MyCoolRSAKey2048", parametersEncryptionRsa);
#elif SMALLMOD
            //cngKeySigningRsa = CngKey.Create(CngAlgorithm.Rsa, "MyCoolRSASignatureKey1024", parametersSigningRsa);
            //cngKeySigningDsa = CngKey.Create(new CngAlgorithm("DSA"), "MyCoolDSAKey1024", parametersSigningDsa);
            //cngKeyEncryptionRsa = CngKey.Create(CngAlgorithm.Rsa, "MyCoolRSAKey1024", parametersEncryptionRsa);
#endif
            cngKeySigningRsa?.Dispose();
            cngKeySigningDsa?.Dispose();
            cngKeyEncryptionRsa?.Dispose();

        }
        [ClassCleanup]
        public static void CleanupKeys()
        {
            CngKey ?cngKeySigningRsa = null;
            CngKey? cngKeySigningDsa = null;
            CngKey? cngKeyEncryptionRsa = null;
#if LARGEMOD
            cngKeySigningRsa = CngKey.Open("MyCoolRSASignatureKey4096");
            cngKeyEncryptionRsa = CngKey.Open("MyCoolRSAKey4096");
#elif LARGEMODSIGNATURE
            cngKeySigningDsa = CngKey.Open("MyCoolDSAKey3072");

#elif MEDMOD
            cngKeySigningRsa = CngKey.Open("MyCoolRSASignatureKey2048");
            cngKeySigningDsa = CngKey.Open("MyCoolDSAKey2048");
            cngKeyEncryptionRsa = CngKey.Open("MyCoolRSAKey2048");
#elif SMALLMOD
            //cngKeySigningRsa = CngKey.Open("MyCoolRSASignatureKey1024");
            //cngKeySigningDsa = CngKey.Open("MyCoolDSAKey1024");
            //cngKeyEncryptionRsa = CngKey.Open("MyCoolRSAKey1024");
#endif
            cngKeySigningRsa?.Delete();
            cngKeySigningRsa?.Dispose();
            cngKeySigningDsa?.Delete();
            cngKeySigningDsa?.Dispose();
            cngKeyEncryptionRsa?.Delete();
            cngKeyEncryptionRsa?.Dispose();
        }
        [TestMethod]
        public void TestSubtractScalarSimple()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = 2;
                exp[i] = 2;
            }
            ModNumber ml = new ModNumber(l);
            exp[0] = 1;
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml - 1u;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSubtractAssignScalarWithCarry()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = 0ul;
            l[1] = 1ul;
            ModNumber ml = new ModNumber(l);
            uint r = 1u;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = ~0ul;
            ModNumber mexp = new ModNumber(exp);
            ml -= r;
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestSubtractAssignScalarWithCarryAcrossMultipleSections()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = 1ul;
            uint r = 1u;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
            {
                exp[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ml -= r;
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestSubtractAssignScalarOneFromZero()
        {
            ModNumber ml = new ModNumber();
            uint r = 1u;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                exp[i] = ~0ul;
            }
            ModNumber mexp = new ModNumber(exp);
            ml -= r;
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestSubtractAssignScalarAllFsFromZero()
        {
            ModNumber ml = new ModNumber();
            uint r = ~0u;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = (~0ul << (ModNumber.ISIZE * 8));
            exp[0] += 1;
            for (int i = 1; i < ModNumber.LCOUNT; i++)
                exp[i] = ~0ul;
            ModNumber mexp = new ModNumber(exp);
            ml -= r;
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestSubtractSimple()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = 2;
                r[i] = 1;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mres = ml - mr;
            Assert.IsTrue(mres == mr);
        }
        [TestMethod]
        public void TestSubtractAssignSimple()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = 2;
                r[i] = 1;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ml -= mr;
            Assert.IsTrue(ml == mr);

        }
        [TestMethod]
        public void TestSubtractWithCarry()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            l[0] = 0ul;
            l[1] = 1ul;
            r[0] = 1ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml - mr;
            Assert.IsTrue(mres == mexp);

        }
        [TestMethod]
        public void TestSubtractAssignWithCarry()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            l[0] = 0ul;
            l[1] = 1ul;
            r[0] = 1ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ml -= mr;
            Assert.IsTrue(ml == mexp);

        }
        [TestMethod]
        public void TestSubtractWithCarryAcrossMultipleSections()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = 1ul;
            r[0] = 1ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
            {
                exp[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml - mr;
            Assert.IsTrue(mres == mexp);

        }
        [TestMethod]
        public void TestSubtractAssignWithCarryAcrossMultipleSections()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = 1ul;
            r[0] = 1ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
            {
                exp[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ml -= mr;
            Assert.IsTrue(ml == mexp);

        }
        [TestMethod]
        public void TestSubtractOneFromZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mr = new ModNumber(1ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                exp[i] = ~0ul;
            }
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml - mr;
            Assert.IsTrue(mres == mexp);

        }
        [TestMethod]
        public void TestSubtractAssignOneFromZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mr = new ModNumber(1ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                exp[i] = ~0ul;
            }
            ModNumber mexp = new ModNumber(exp);
            ml -= mr;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestSubtractLeftFsFromZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[ModNumber.LCOUNT - 1] = ~0ul;
            ModNumber mr = new ModNumber(r);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[ModNumber.LCOUNT - 1] = 1ul;
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml - mr;
            Assert.IsTrue(mres == mexp);

        }
        [TestMethod]
        public void TestSubtractAssignLeftFsFromZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[ModNumber.LCOUNT - 1] = ~0ul;
            ModNumber mr = new ModNumber(r);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[ModNumber.LCOUNT - 1] = 1ul;
            ModNumber mexp = new ModNumber(exp);
            ml -= mr;
            Assert.IsTrue(ml == mexp);

        }
        [TestMethod]
        public void TestSubtractAllFsFromZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                r[i] = ~0ul;
            }
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = ml - mr;
            Assert.IsTrue(mres == mexp);

        }
        [TestMethod]
        public void TestSubtractAssignAllFsFromZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                r[i] = ~0ul;
            }
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(1ul);
            ml -= mr;
            Assert.IsTrue(ml == mexp);

        }
        [TestMethod]
        [DataRow(0x12345678ul, 0x12345678ul, 0)]
        [DataRow(0x12345ul, 0x2468aul, 1)]
        [DataRow(0x12345ul, 0x1234500000000ul, 32)]
        [DataRow(0x12345ul, 0x0ul, ModNumber.NSIZE)]
        public void TestShiftLeft(ulong n, ulong exp, int shift)
        {
            ModNumber ml = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == (ml << shift));

        }
        [TestMethod]
        [DataRow(0x12345678ul, 0x12345678ul, 0)]
        [DataRow(0x12345ul, 0x2468aul, 1)]
        [DataRow(0x12345ul, 0x1234500000000ul, 32)]
        [DataRow(0x12345ul, 0x0ul, ModNumber.NSIZE)]
        public void TestShiftLeftAssign(ulong n, ulong exp, int shift)
        {
            ModNumber ml = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            ml <<= shift;
            Assert.IsTrue(mexp == ml);

        }
        [TestMethod]
        public void TestShiftLeft60()
        {
            ModNumber ml = new ModNumber(0x0102030405060708ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x8000000000000000ul;
            exp[1] = 0x0010203040506070ul;
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == (ml << 60));
        }
        [TestMethod]
        public void TestShiftLeftAssign60()
        {
            ModNumber ml = new ModNumber(0x0102030405060708ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x8000000000000000ul;
            exp[1] = 0x0010203040506070ul;
            ModNumber mexp = new ModNumber(exp);
            ml <<= 60;
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestShiftLeft65()
        {
            ModNumber ml = new ModNumber(0x12345ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = 0x2468aul;
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == (ml << 65));
        }
        [TestMethod]
        public void TestShiftLeftAssign65()
        {
            ModNumber ml = new ModNumber(0x12345ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = 0x2468aul;
            ModNumber mexp = new ModNumber(exp);
            ml <<= 65;
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestShiftLeftNSIZEMinusLSIZETimes8()
        {
            ModNumber ml = new ModNumber(0x12345ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[ModNumber.LCOUNT - 1] = 0x12345ul;
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == (ml << (ModNumber.NSIZE - ModNumber.LSIZE * 8)));
        }
        [TestMethod]
        public void TestShiftLeftAssignNSIZEMinusLSIZETimes8()
        {
            ModNumber ml = new ModNumber(0x12345ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[ModNumber.LCOUNT - 1] = 0x12345ul;
            ModNumber mexp = new ModNumber(exp);
            ml <<= (ModNumber.NSIZE - ModNumber.LSIZE * 8);
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        [DataRow(0x12345678ul, 0x12345678ul, 0)]
        [DataRow(0x12345ul, 0x091a2ul, 1)]
        [DataRow(0x1234500000000ul, 0x12345ul, 32)]
        [DataRow(0x12345ul, 0x0ul, ModNumber.ISIZE * 8)]
        public void TestShiftRight(ulong n, ulong exp, int shift)
        {
            ModNumber ml = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == (ml >> shift));

        }
        [TestMethod]
        [DataRow(0x12345678ul, 0x12345678ul, 0)]
        [DataRow(0x12345ul, 0x091a2ul, 1)]
        [DataRow(0x1234500000000ul, 0x12345ul, 32)]
        [DataRow(0x12345ul, 0x0ul, ModNumber.ISIZE * 8)]
        public void TestShiftRightAssign(ulong n, ulong exp, int shift)
        {
            ModNumber ml = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            ml >>= shift;
            Assert.IsTrue(mexp == ml);

        }
        [TestMethod]
        public void TestShiftRight60()
        {
            ModNumber mexp = new ModNumber(0x0102030405060708ul);
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = 0x8000000000000000ul;
            l[1] = 0x0010203040506070ul;
            ModNumber ml = new ModNumber(l);
            Assert.IsTrue(mexp == (ml >> 60));
        }
        [TestMethod]
        public void TestShiftRightAssign60()
        {
            ModNumber mexp = new ModNumber(0x0102030405060708ul);
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = 0x8000000000000000ul;
            l[1] = 0x0010203040506070ul;
            ModNumber ml = new ModNumber(l);
            ml >>= 60;
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestShiftRight65()
        {
            ModNumber mexp = new ModNumber(0x12345ul);
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[1] = 0x2468aul;
            ModNumber ml = new ModNumber(l);
            Assert.IsTrue(mexp == (ml >> 65));
        }
        [TestMethod]
        public void TestShiftRightAssign65()
        {
            ModNumber mexp = new ModNumber(0x12345ul);
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[1] = 0x2468aul;
            ModNumber ml = new ModNumber(l);
            ml >>= 65;
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestShiftRightNSIZEMinusLSIZETimes8()
        {
            ModNumber mexp = new ModNumber(0x12345ul);
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = 0x12345ul;
            ModNumber ml = new ModNumber(l);
            Assert.IsTrue(mexp == (ml >> (ModNumber.NSIZE - ModNumber.LSIZE * 8)));
        }
        [TestMethod]
        public void TestShiftRightAssignNSIZEMinusLSIZETimes8()
        {
            ModNumber mexp = new ModNumber(0x12345ul);
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = 0x12345ul;
            ModNumber ml = new ModNumber(l);
            ml >>= (ModNumber.NSIZE - ModNumber.LSIZE * 8);
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestModuloDivideByZero()
        {
            ModNumber ml = new ModNumber(1ul);
            ModNumber mr = new ModNumber(0ul);
            Assert.ThrowsException<DivideByZeroException>(() => ml % mr);
        }
        [TestMethod]
        [DataRow(1000ul, 1ul, 0ul)]
        [DataRow(0ul, 1ul, 0ul)]
        [DataRow(1000ul, 2ul, 0ul)]
        [DataRow(1001ul, 2ul, 1ul)]
        [DataRow(1001ul, 2001ul, 1001ul)]
        [DataRow(1001ul, 1001ul, 0ul)]
        [DataRow(101ul, 5ul, 1ul)]
        public void TestModulo(ulong l, ulong r, ulong exp)
        {
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == ml % mr);
        }
        [TestMethod]
        public void TestModuloDivide2Pow64ByEight()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[1] = 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(8ul);
            ModNumber mexp = new ModNumber(0ul);
            Assert.IsTrue(mexp == ml % mr);

        }
        [TestMethod]
        public void TestModuloDivideAllFsBy2Pow16()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
                l[i] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(65536ul);
            ModNumber mexp = new ModNumber(65535ul);
            Assert.IsTrue(mexp == ml % mr);

        }
        [TestMethod]
        public void TestModuloDivideAllFsByAlllFs()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
                l[i] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(0ul);
            Assert.IsTrue(mexp == ml % ml);

        }
        [TestMethod]
        public void TestModuloDivideAllFsByAlllFsAndZeroLowWord()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
            }
            r[0] = 0ul;
            exp[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == ml % mr);

        }
        [TestMethod]
        public void TestModuloDivideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne()
        {
            ModNumber mnprime1 = new ModNumber(355687428095999ul);
            uint prime2 = 39916799u;
            ModNumber mnprime2 = new ModNumber(prime2);
            ModNumber product = mnprime1 * prime2;
            ModNumber res1 = product % mnprime1;
            ModNumber res2 = product % mnprime2;
            ModNumber mexp1 = new ModNumber(0ul);
            Assert.IsTrue(res1 == mexp1);
            Assert.IsTrue(res2 == mexp1);
            ModNumber mone = new ModNumber(1ul);
            ModNumber mnprime1MinusOne = mnprime1 - mone;
            ModNumber mnprime2MinusOne = mnprime2 - mone;
            ModNumber productMinusPrime1 = product - mnprime1;
            ModNumber productMinusPrime2 = product - mnprime2;
            ModNumber res3 = productMinusPrime1 % mnprime2MinusOne;
            ModNumber res4 = productMinusPrime2 % mnprime1MinusOne;
            ModNumber res5 = productMinusPrime1 % mnprime1;
            ModNumber res6 = productMinusPrime2 % mnprime2;
            Assert.IsTrue(res3 == mexp1);
            Assert.IsTrue(res4 == mexp1);
            Assert.IsTrue(res5 == mexp1);
            Assert.IsTrue(res6 == mexp1);

        }
        public void TestModuloAssignDivideByZero()
        {
            ModNumber ml = new ModNumber(1ul);
            ModNumber mr = new ModNumber(0ul);
            Assert.ThrowsException<DivideByZeroException>(() =>
            {
                ml %= mr;
            });
        }
        [TestMethod]
        [DataRow(1000ul, 1ul, 0ul)]
        [DataRow(0ul, 1ul, 0ul)]
        [DataRow(1000ul, 2ul, 0ul)]
        [DataRow(1001ul, 2ul, 1ul)]
        [DataRow(1001ul, 2001ul, 1001ul)]
        [DataRow(1001ul, 1001ul, 0ul)]
        [DataRow(101ul, 5ul, 1ul)]
        public void TestModuloAssign(ulong l, ulong r, ulong exp)
        {
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ml %= mr;
            Assert.IsTrue(mexp == ml);
        }
        [TestMethod]
        public void TestModuloAssignDivide2Pow64ByEight()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[1] = 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(8ul);
            ModNumber mexp = new ModNumber(0ul);
            ml %= mr;
            Assert.IsTrue(mexp == ml);

        }
        [TestMethod]
        public void TestModuloAssignDivideAllFsBy2Pow16()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
                l[i] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(65536ul);
            ModNumber mexp = new ModNumber(65535ul);
            ml %= mr;
            Assert.IsTrue(mexp == ml);

        }
        [TestMethod]
        public void TestModuloAssignDivideAllFsByAlllFs()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
                l[i] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(0ul);
            ml %= ml;
            Assert.IsTrue(mexp == ml);

        }
        [TestMethod]
        public void TestModuloAssignDivideAllFsByAlllFsAndZeroLowWord()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
            }
            r[0] = 0ul;
            exp[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ml %= mr;
            Assert.IsTrue(mexp == ml);

        }
        [TestMethod]
        public void TestModuloAssignDivideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne()
        {
            ModNumber mnprime1 = new ModNumber(355687428095999ul);
            uint prime2 = 39916799u;
            ModNumber mnprime2 = new ModNumber(prime2);
            ModNumber product = mnprime1 * prime2;
            ModNumber res1 = product;
            res1 %= mnprime1;
            ModNumber res2 = product;
            res2 %= mnprime2;
            ModNumber mexp1 = new ModNumber(0ul);
            Assert.IsTrue(res1 == mexp1);
            Assert.IsTrue(res2 == mexp1);
            ModNumber mone = new ModNumber(1ul);
            ModNumber mnprime1MinusOne = mnprime1 - mone;
            ModNumber mnprime2MinusOne = mnprime2 - mone;
            ModNumber productMinusPrime1 = product - mnprime1;
            ModNumber productMinusPrime2 = product - mnprime2;
            ModNumber res3 = productMinusPrime1;
            res3 %= mnprime2MinusOne;
            ModNumber res4 = productMinusPrime2;
            res4 %= mnprime1MinusOne;
            ModNumber res5 = productMinusPrime1;
            res5 %= mnprime1;
            ModNumber res6 = productMinusPrime2;
            res6 %= mnprime2;
            Assert.IsTrue(res3 == mexp1);
            Assert.IsTrue(res4 == mexp1);
            Assert.IsTrue(res5 == mexp1);
            Assert.IsTrue(res6 == mexp1);

        }
        [TestMethod]
        public void TestDivideAndModuloByZero()
        {
            ModNumber ml = new ModNumber(1ul);
            ModNumber mr = new ModNumber(0ul);
            Assert.ThrowsException<DivideByZeroException>(() => { (ModNumber divRes, ModNumber modRes) = ModNumber.DivideAndModulo(ml, mr, false); });

        }
        [TestMethod]
        [DataRow(1000ul, 1ul, 1000ul, 0ul)]
        [DataRow(0ul, 1ul, 0ul, 0ul)]
        [DataRow(1000ul, 2ul, 500ul, 0ul)]
        [DataRow(1001ul, 2ul, 500ul, 1ul)]
        [DataRow(1001ul, 2001ul, 0ul, 1001ul)]
        [DataRow(1001ul, 1001ul, 1ul, 0ul)]
        [DataRow(101ul, 5ul, 20ul, 1ul)]
        public void TestDivideAndModulo(ulong l, ulong r, ulong div, ulong mod)
        {
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            (ModNumber divRes, ModNumber modRes) = ModNumber.DivideAndModulo(ml, mr, false);
            ModNumber expDiv = new ModNumber(div);
            ModNumber expMod = new ModNumber(mod);
            Assert.IsTrue(expDiv == divRes);
            Assert.IsTrue(expMod == modRes);
        }
        [TestMethod]
        public void TestDivideAndModulo2Pow64ByEight()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[1] = 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(8ul);
            ModNumber expDiv = new ModNumber(ml >> 3);
            ModNumber expMod = new ModNumber(0ul);
            (ModNumber divRes, ModNumber modRes) = ModNumber.DivideAndModulo(ml, mr, false);
            Assert.IsTrue(expDiv == divRes);
            Assert.IsTrue(expMod == modRes);
        }
        [TestMethod]
        public void TestDivideAndModuloAllFsBy2Pow16()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] expDiv = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                expDiv[i] = ~0ul;
            }
            expDiv[ModNumber.LCOUNT - 1] ^= (0xfffful << (ModNumber.LSIZE * 8 - 16));
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(0x10000ul);
            ModNumber mexpDiv = new ModNumber(expDiv);
            ModNumber mexpMod = new ModNumber(0xfffful);
            (ModNumber divRes, ModNumber modRes) = ModNumber.DivideAndModulo(ml, mr, false);
            Assert.IsTrue(mexpDiv == divRes);
            Assert.IsTrue(mexpMod == modRes);

        }
        [TestMethod]
        public void TestDivideAndModuloAllFsLeftAndRightWordByAllFsRightWord()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] expDiv = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            r[0] = ~0ul;
            expDiv[0] = 1ul;
            l[ModNumber.LCOUNT - 1] = ~0ul;
            expDiv[ModNumber.LCOUNT - 1] = 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexpDiv = new ModNumber(expDiv);
            ModNumber mexpMod = new ModNumber(0ul);
            (ModNumber divRes, ModNumber modRes) = ModNumber.DivideAndModulo(ml, mr, false);
            Assert.IsTrue(mexpDiv == divRes);
            Assert.IsTrue(mexpMod == modRes);

        }
        [TestMethod]
        public void TestDivideAndModuloAllFsByAlllFs()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexpDiv = new ModNumber(1ul);
            ModNumber mexpMod = new ModNumber(0ul);
            (ModNumber divRes, ModNumber modRes) = ModNumber.DivideAndModulo(ml, mr, false);
            Assert.IsTrue(mexpDiv == divRes);
            Assert.IsTrue(mexpMod == modRes);

        }
        [TestMethod]
        public void TestDivideAndModuloAllFsByAlllFsAndZeroLowWord()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] expMod = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
            }
            r[0] = 0ul;
            expMod[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexpDiv = new ModNumber(1ul);
            ModNumber mexpMod = new ModNumber(expMod);
            (ModNumber divRes, ModNumber modRes) = ModNumber.DivideAndModulo(ml, mr, false);
            Assert.IsTrue(mexpDiv == divRes);
            Assert.IsTrue(mexpMod == modRes);

        }
        [TestMethod]
        public void TestDivideAndModuloProductOfPrimesByBothPrimesAndByBothPrimesMinusOne()
        {
            ModNumber mnprime1 = new ModNumber(355687428095999ul);
            uint prime2 = 39916799u;
            ModNumber mnprime2 = new ModNumber(prime2);
            ModNumber product = mnprime1 * prime2;
            (ModNumber divRes1, ModNumber modRes1) = ModNumber.DivideAndModulo(product, mnprime1, false);
            (ModNumber divRes2, ModNumber modRes2) = ModNumber.DivideAndModulo(product, mnprime2, false);
            ModNumber mexpDiv1 = new ModNumber(mnprime2);
            ModNumber mexpDiv2 = new ModNumber(mnprime1);
            ModNumber mexpMod = new ModNumber(0ul);
            Assert.IsTrue(divRes1 == mexpDiv1);
            Assert.IsTrue(divRes2 == mexpDiv2);
            Assert.IsTrue(modRes1 == mexpMod);
            Assert.IsTrue(modRes2 == mexpMod);
            ModNumber mone = new ModNumber(1ul);
            ModNumber mnprime1MinusOne = mnprime1 - mone;
            ModNumber mnprime2MinusOne = mnprime2 - mone;
            ModNumber productMinusPrime1 = product - mnprime1;
            ModNumber productMinusPrime2 = product - mnprime2;
            ModNumber mexpDiv3 = new ModNumber(mnprime1);
            ModNumber mexpDiv4 = new ModNumber(mnprime2);
            ModNumber mexpDiv5 = new ModNumber(mnprime2MinusOne);
            ModNumber mexpDiv6 = new ModNumber(mnprime1MinusOne);
            (ModNumber divRes3, ModNumber modRes3) = ModNumber.DivideAndModulo(productMinusPrime1, mnprime2MinusOne, false);
            (ModNumber divRes4, ModNumber modRes4) = ModNumber.DivideAndModulo(productMinusPrime2, mnprime1MinusOne, false);
            (ModNumber divRes5, ModNumber modRes5) = ModNumber.DivideAndModulo(productMinusPrime1, mnprime1, false);
            (ModNumber divRes6, ModNumber modRes6) = ModNumber.DivideAndModulo(productMinusPrime2, mnprime2, false);

            Assert.IsTrue(divRes3 == mexpDiv3);
            Assert.IsTrue(divRes4 == mexpDiv4);
            Assert.IsTrue(divRes5 == mexpDiv5);
            Assert.IsTrue(divRes6 == mexpDiv6);
            Assert.IsTrue(modRes3 == mexpMod);
            Assert.IsTrue(modRes4 == mexpMod);
            Assert.IsTrue(modRes5 == mexpMod);
            Assert.IsTrue(modRes6 == mexpMod);

        }
        [TestMethod]
        public void TestDivideAndModuloProductOfLargesPrimesByBothPrimesAndByBothPrimesMinusOne()
        {
            ModNumber mPrime1 = ModNumber.Stomn("C882E9B30B9F87F47AD7653886EF5CD53D9CAA96D2B83C24C643F024B86CDFE61DE6627F0C63B3CBB885DD3212C77CBE47A3469E5EA2FAD245984A75D866AAF3", 16);
            ModNumber mPrime2 = ModNumber.Stomn("E1668918F4DD83B3066B32577933BD27B1FFA8F49907637863750304EFCBDB1DCC86362EEBC81ACD68B7E0C0BCE35B1BC0F353F16B59C5E7636AF1464805CAB3", 16);
            ModNumber product = mPrime1 * mPrime2;
            (ModNumber divRes1, ModNumber modRes1) = ModNumber.DivideAndModulo(product, mPrime1, false);
            (ModNumber divRes2, ModNumber modRes2) = ModNumber.DivideAndModulo(product, mPrime2, false);
            ModNumber mexpDiv1 = new ModNumber(mPrime2);
            ModNumber mexpDiv2 = new ModNumber(mPrime1);
            ModNumber mexpMod = new ModNumber(0ul);
            Assert.IsTrue(divRes1 == mexpDiv1);
            Assert.IsTrue(divRes2 == mexpDiv2);
            Assert.IsTrue(modRes1 == mexpMod);
            Assert.IsTrue(modRes2 == mexpMod);
            ModNumber mone = new ModNumber(1ul);
            ModNumber mnprime1MinusOne = mPrime1 - mone;
            ModNumber mnprime2MinusOne = mPrime2 - mone;
            ModNumber productMinusPrime1 = product - mPrime1;
            ModNumber productMinusPrime2 = product - mPrime2;
            ModNumber mexpDiv3 = new ModNumber(mPrime1);
            ModNumber mexpDiv4 = new ModNumber(mPrime2);
            ModNumber mexpDiv5 = new ModNumber(mnprime2MinusOne);
            ModNumber mexpDiv6 = new ModNumber(mnprime1MinusOne);
            (ModNumber divRes3, ModNumber modRes3) = ModNumber.DivideAndModulo(productMinusPrime1, mnprime2MinusOne, false);
            (ModNumber divRes4, ModNumber modRes4) = ModNumber.DivideAndModulo(productMinusPrime2, mnprime1MinusOne, false);
            (ModNumber divRes5, ModNumber modRes5) = ModNumber.DivideAndModulo(productMinusPrime1, mPrime1, false);
            (ModNumber divRes6, ModNumber modRes6) = ModNumber.DivideAndModulo(productMinusPrime2, mPrime2, false);

            Assert.IsTrue(divRes3 == mexpDiv3);
            Assert.IsTrue(divRes4 == mexpDiv4);
            Assert.IsTrue(divRes5 == mexpDiv5);
            Assert.IsTrue(divRes6 == mexpDiv6);
            Assert.IsTrue(modRes3 == mexpMod);
            Assert.IsTrue(modRes4 == mexpMod);
            Assert.IsTrue(modRes5 == mexpMod);
            Assert.IsTrue(modRes6 == mexpMod);

        }
        [TestMethod]
        public void TestDivideByZero()
        {
            ModNumber ml = new ModNumber(1ul);
            ModNumber mr = new ModNumber(0ul);
            Assert.ThrowsException<DivideByZeroException>(() => { ModNumber res = ml / mr; });
        }
        [TestMethod]
        public void TestDivideByOne()
        {
            ModNumber ml = new ModNumber(1000ul);
            ModNumber mr = new ModNumber(1ul);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(1000ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideZeroByOne()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mr = new ModNumber(1ul);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(0ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideEvenByTwo()
        {
            ModNumber ml = new ModNumber(1000ul);
            ModNumber mr = new ModNumber(2ul);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(500ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideOddByTwo()
        {
            ModNumber ml = new ModNumber(1001ul);
            ModNumber mr = new ModNumber(2ul);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(500ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideSmallByLarge()
        {
            ModNumber ml = new ModNumber(1001ul);
            ModNumber mr = new ModNumber(2001ul);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(0ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideEquals()
        {
            ModNumber ml = new ModNumber(1001ul);
            ModNumber mr = new ModNumber(1001ul);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(1ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDividePrimeByFive()
        {
            ModNumber ml = new ModNumber(101ul);
            ModNumber mr = new ModNumber(5ul);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(20ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivide2Pow64ByEight()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[1] = 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(8ul);
            ModNumber mres = ml / mr;
            ModNumber mexp = ml >> 3;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideAllFsBy2Pow16()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                exp[i] = ~0ul;
            }
            exp[ModNumber.LCOUNT - 1] ^= (65535ul << (ModNumber.LSIZE * 8 - 16));
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(65536ul);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideAllFsLeftAndRightWordByAllFsRightWord()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            l[ModNumber.LCOUNT - 1] = ~0ul;
            r[0] = ~0ul;
            exp[0] = 1ul;
            exp[ModNumber.LCOUNT - 1] = 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideAllFsByAllFs()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(1ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideAllFsByAllFsAndZeroLowWord()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
            }
            r[0] = 0;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mres = ml / mr;
            ModNumber mexp = new ModNumber(1ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne()
        {
            ModNumber mnprime1 = new ModNumber(355687428095999ul);
            uint prime2 = 39916799u;
            ModNumber mnprime2 = new ModNumber(prime2);
            ModNumber product = mnprime1 * prime2;
            ModNumber divRes1 = product / mnprime1;
            ModNumber divRes2 = product / mnprime2;
            ModNumber mexpDiv1 = new ModNumber(mnprime2);
            ModNumber mexpDiv2 = new ModNumber(mnprime1);
            Assert.IsTrue(divRes1 == mexpDiv1);
            Assert.IsTrue(divRes2 == mexpDiv2);
            ModNumber mone = new ModNumber(1ul);
            ModNumber mnprime1MinusOne = mnprime1 - mone;
            ModNumber mnprime2MinusOne = mnprime2 - mone;
            ModNumber productMinusPrime1 = product - mnprime1;
            ModNumber productMinusPrime2 = product - mnprime2;
            ModNumber mexpDiv3 = new ModNumber(mnprime1);
            ModNumber mexpDiv4 = new ModNumber(mnprime2);
            ModNumber mexpDiv5 = new ModNumber(mnprime2MinusOne);
            ModNumber mexpDiv6 = new ModNumber(mnprime1MinusOne);
            ModNumber divRes3 = productMinusPrime1 / mnprime2MinusOne;
            ModNumber divRes4 = productMinusPrime2 / mnprime1MinusOne;
            ModNumber divRes5 = productMinusPrime1 / mnprime1;
            ModNumber divRes6 = productMinusPrime2 / mnprime2;

            Assert.IsTrue(divRes3 == mexpDiv3);
            Assert.IsTrue(divRes4 == mexpDiv4);
            Assert.IsTrue(divRes5 == mexpDiv5);
            Assert.IsTrue(divRes6 == mexpDiv6);
        }
        [TestMethod]
        public void TestEqualTrue()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(ml);
            Assert.IsTrue(ml == mr);
        }
        [TestMethod]
        public void TestSubtractEqualNumbers()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(ml);
            ModNumber mres = ml - mr;
            ModNumber mexp = new ModNumber(0ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestEqualNotTrueFirstSection()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
                r[i] = i;
            }
            r[0] -= 1;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            Assert.IsFalse(ml == mr);

        }
        [TestMethod]
        public void TestEqualNotTrueLastSection()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
                r[i] = i;
            }
            r[ModNumber.LCOUNT - 1] -= 1;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            Assert.IsFalse(ml == mr);

        }
        [TestMethod]
        public void TestLessThanTrue()
        {
            ModNumber ml = new ModNumber(1ul);
            ModNumber mr = new ModNumber(2ul);
            Assert.IsTrue(ml < mr);
        }
        [TestMethod]
        public void TestLessThanFalse()
        {
            ModNumber ml = new ModNumber(1ul);
            ModNumber mr = new ModNumber(2ul);
            Assert.IsFalse(mr < ml);
        }
        [TestMethod]
        public void TestGreaterThanTrueForLargeDifference()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = 1ul;
            r[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            Assert.IsTrue(ml > mr);

        }
        [TestMethod]
        public void TestLessThanFalseForLargeDifference()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = 1ul;
            r[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            Assert.IsFalse(ml < mr);

        }
        [TestMethod]
        public void TestLessThanFalseForEquality()
        {
            ModNumber ml = new ModNumber(1234ul);
            ModNumber mr = new ModNumber(ml);
            Assert.IsFalse(ml < mr);

        }
        [TestMethod]
        public void TestLessThanFalseForEqualityOfZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mr = new ModNumber(ml);
            Assert.IsFalse(ml < mr);

        }
        [TestMethod]
        public void TestLessOrEqualTrueForLessThan()
        {
            ModNumber ml = new ModNumber(1ul);
            ModNumber mr = new ModNumber(2ul);
            Assert.IsTrue(ml <= mr);

        }
        [TestMethod]
        public void TestLessOrEqualFalseForGreaterThan()
        {
            ModNumber ml = new ModNumber(1ul);
            ModNumber mr = new ModNumber(2ul);
            Assert.IsFalse(mr <= ml);

        }
        [TestMethod]
        public void TestLessOrEqualTrueForEquality()
        {
            ModNumber ml = new ModNumber(1234ul);
            ModNumber mr = new ModNumber(ml);
            Assert.IsTrue(ml <= mr);

        }
        [TestMethod]
        public void TestLessOrEqualTrueForEqualityOfZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mr = new ModNumber(ml);
            Assert.IsTrue(ml <= mr);

        }
        [TestMethod]
        public void TestGreaterThanTrue()
        {
            ModNumber ml = new ModNumber(2ul);
            ModNumber mr = new ModNumber(1ul);
            Assert.IsTrue(ml > mr);

        }
        [TestMethod]
        public void TestGreaterThanFalse()
        {
            ModNumber ml = new ModNumber(1ul);
            ModNumber mr = new ModNumber(2ul);
            Assert.IsFalse(ml > mr);

        }
        [TestMethod]
        public void TestGreaterThanFalseForEquality()
        {
            ModNumber ml = new ModNumber(1234ul);
            ModNumber mr = new ModNumber(ml);
            Assert.IsFalse(ml > mr);

        }
        [TestMethod]
        public void TestGreaterThanFalseForEqualityOfZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mr = new ModNumber(ml);
            Assert.IsFalse(ml > mr);

        }
        [TestMethod]
        public void TestGreaterOrEqualTrueForGreaterThan()
        {
            ModNumber ml = new ModNumber(2ul);
            ModNumber mr = new ModNumber(1ul);
            Assert.IsTrue(ml >= mr);

        }
        [TestMethod]
        public void TestGreaterOrEqualFalseForLessThan()
        {
            ModNumber ml = new ModNumber(2ul);
            ModNumber mr = new ModNumber(1ul);
            Assert.IsFalse(mr >= ml);

        }
        [TestMethod]
        public void TestGreaterOrEqualTrueForEquality()
        {
            ModNumber ml = new ModNumber(1234ul);
            ModNumber mr = new ModNumber(ml);
            Assert.IsTrue(ml >= mr);

        }
        [TestMethod]
        public void TestGreaterOrEqualTrueForEqualityOfZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mr = new ModNumber(ml);
            Assert.IsTrue(ml >= mr);

        }
        [TestMethod]
        public void TestAddAssignScalarOneToZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mexp = new ModNumber(1ul);
            ml += 1u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddScalarOneToZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = ml + 1u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestAddAssignScalarOneToFirstSectionMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = 1ul;
            ModNumber mexp = new ModNumber(exp);
            ml += 1u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddScalarOneToFirstSectionMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = 1ul;
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml + 1u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestAddAssignScalarOneToMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(0ul);
            ml += 1u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddScalarOneToMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = ml + 1u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestAddAssignScalarMaxToMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(0xFEul);
            ml += 0xFFu;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddScalarMaxToMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(0xFEul);
            ModNumber mres = ml + 0xFFu;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestAddAssignOneToZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mr = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(1ul);
            ml += mr;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddOneToZero()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mr = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = ml + mr;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestAddAssignOneToFirstSectionMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(1ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = 1ul;
            ModNumber mexp = new ModNumber(exp);
            ml += mr;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddOneToFirstSectionMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(1ul);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = 1ul;
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml + mr;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestAddAssignOneToMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(0ul);
            ml += mr;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddSelfAssignMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                exp[i] = ~0ul;
            }
            exp[0] -= 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ml += ml;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddAssignFirstWordMaxToMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(~0ul);
            ModNumber mexp = new ModNumber(~0ul - 1ul);
            ml += mr;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddFirstWordMaxToMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(~0ul);
            ModNumber mexp = new ModNumber(~0ul - 1ul);
            ModNumber mres = ml + mr;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestAddAssignMaxToMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
                exp[i] = ~0ul;
            }
            exp[0] -= 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ml += mr;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestAddMaxToMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
                exp[i] = ~0ul;
            }
            exp[0] -= 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml + mr;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestMultiplyAssignByZero()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(0ul);
            ml *= 0u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestMultiplyByZeroScalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber morig = new ModNumber(l);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = ml * 0u;
            Assert.IsTrue(mres == mexp);
            Assert.IsTrue(morig == ml);
        }
        [TestMethod]
        public void TestMultiplyAssignByOneScalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(l);
            ml *= 1u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestMultiplyByOneScalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(l);
            ModNumber mres = ml * 1u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestMultiplyAssignByTwoScalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
                exp[i] = i * 2;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ml *= 2u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestMultiplyByTwoScalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = i;
                exp[i] = i * 2;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * 2u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestMultiplyAssignAllFFFFByTwoScalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                exp[i] = ~0ul;
            }
            exp[0] ^= 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ml *= 2u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestMultiplyAllFFFFByTwoScalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                exp[i] = ~0ul;
            }
            exp[0] ^= 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * 2u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestMultiplyAssignFsBy2Pow16Scalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            exp[1] = ~0ul >> (ModNumber.LSIZE * 8 - 16);
            exp[0] = ~0ul << 16;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ml *= 65536u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestMultiplyFsBy2Pow16Scalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            exp[1] = ~0ul >> (ModNumber.LSIZE * 8 - 16);
            exp[0] = ~0ul << 16;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * 65536u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestMultiplyAssignAllAsByTwoScalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = 0xaaaaaaaaaaaaaaaaul;
                exp[i] = (l[i] << 1) + 1;
            }
            exp[0] = l[0] << 1;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ml *= 2u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestMultiplyAllAsByTwoScalar()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (ulong i = 0ul; i < ModNumber.LCOUNT; i++)
            {
                l[i] = 0xaaaaaaaaaaaaaaaaul;
                exp[i] = (l[i] << 1) + 1;
            }
            exp[0] = l[0] << 1;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * 2u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestMultiplyAssign9sDecBy9sDecScalar()
        {
            ModNumber ml = new ModNumber(99999999ul);
            ModNumber mexp = ModNumber.Stomn("9999999800000001");
            ml *= 99999999u;
            Assert.IsTrue(ml == mexp);
        }
        [TestMethod]
        public void TestMultiply9sDecBy9sDecScalar()
        {
            ModNumber ml = new ModNumber(99999999ul);
            ModNumber mexp = ModNumber.Stomn("9999999800000001");
            ModNumber mres = ml * 99999999u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestDivisionByZeroScalar()
        {
            ModNumber ml = new ModNumber(0ul);
            Assert.ThrowsException<DivideByZeroException>(() => { ModNumber mres = ml / 0u; });
        }
        [TestMethod]
        public void TestDivisionZeroByOneScalar()
        {
            ModNumber ml = new ModNumber(0ul);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = ml / 1u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestDivisionNonZeroByOneScalar()
        {
            ModNumber ml = new ModNumber(123456ul);
            ModNumber mexp = new ModNumber(123456ul);
            ModNumber mres = ml / 1u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestDivisionNonZeroByTwoScalar()
        {
            ModNumber ml = new ModNumber(24690ul);
            ModNumber mexp = new ModNumber(12345ul);
            ModNumber mres = ml / 2u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestDivisionAll9sBy3s()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = 9999999999999999999ul;
                exp[i] = 3333333333333333333ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml / 3u;
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestDivisionAllNinesByTwoAndMultipliedByTwo()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = 9999999999999999999ul;
                exp[i] = 9999999999999999999ul;
            }
            exp[0] = 9999999999999999998ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres1 = ml / 2u;
            ModNumber mres2 = mres1 * 2u;
            Assert.IsTrue(mres2 == mexp);
        }
        [TestMethod]
        public void TestDivisionMaxULongTimesTwoByTwo()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            l[1] = 1;
            exp[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml / 2u;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDivisionAllAsByTwo()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = 0xaaaaaaaaaaaaaaaaul;
                exp[i] = 0x5555555555555555ul;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml / 2u;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestToStringIllegalBase()
        {
            ModNumber ml = new ModNumber(0ul);
            Assert.ThrowsException<ArgumentException>(() => { string s = ml.ToString(11); });
        }
        [TestMethod]
        public void TestToStringOctalForZero()
        {
            ModNumber ml = new ModNumber(0ul);
            string exp = new string('0', ModNumber.OctalStringLength);
            string res = ml.ToString(8);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringOctalForOne()
        {
            ModNumber ml = new ModNumber(1ul);
            string exp = new string('0', ModNumber.OctalStringLength - 1);
            exp += "1";
            string res = ml.ToString(8);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringOctalForEight()
        {
            ModNumber ml = new ModNumber(8ul);
            string exp = new string('0', ModNumber.OctalStringLength - 2);
            exp += "10";
            string res = ml.ToString(8);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringOctalForEightScale6()
        {
            ModNumber ml = new ModNumber(8ul);
            ScaledNumber sn = new ScaledNumber(ml, 6);
            (int digitsLeft, int digitsRight) = sn.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft - 2);
            exp += "10.0000000000000000";
            string res = sn.ToString(8);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringOctalFor0x012345678910Scaled6()
        {
            ModNumber ml = new ModNumber(0x012345678910ul);
            ScaledNumber sn = new ScaledNumber(ml, 6, true);
            (int digitsLeft, int digitsRight) = sn.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft);
            exp += ".0022150531704420";
            string res = sn.ToString(8);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringOctalForMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            string exp = new string('7', ModNumber.OctalStringLength - 1);
            uint left = ModNumber.NCOUNT * 8 % 3;
            switch (left)
            {
                case 0:
                    exp = "7" + exp;
                    break;
                case 1:
                    exp = "1" + exp;
                    break;
                case 2:
                    exp = "3" + exp;
                    break;
            }
            string res = ml.ToString(8);
            Assert.IsTrue(exp == res);
        }
        [TestMethod]
        public void TestToStringOctalForMaxScaled6()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            ScaledNumber sn = new ScaledNumber(ml, 6, true);
            (int digitsLeft, int digitsRight) = sn.CalculateOctalStringLength();
            string exp = new string('7', digitsLeft - 1);
            uint left = (ModNumber.NCOUNT - 6) * 8 % 3;
            switch (left)
            {
                case 0:
                    exp = "7" + exp;
                    break;
                case 1:
                    exp = "1" + exp;
                    break;
                case 2:
                    exp = "3" + exp;
                    break;
            }
            exp += ".7777777777777777";
            string res = sn.ToString(8);
            Assert.IsTrue(exp == res);
        }
        [TestMethod]
        public void TestToStringOctalForMaxesAndZeros()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT - 6; i += 6)
            {
                for (int j = 0; j < 3; j++)
                    l[i + j] = ~0ul;
                for (int j = 3; j < 6; j++)
                    l[i + j] = 0ul;
            }
            for (int i = ModNumber.LCOUNT - (ModNumber.LCOUNT % 6); i < ModNumber.LCOUNT; i++)
                l[i] = 0;
            ModNumber ml = new ModNumber(l);
            StringBuilder exp = new StringBuilder(ModNumber.OctalStringLength);
            exp.Append(new string('0', ModNumber.OctalStringLength % 128));
            for (int i = ModNumber.OctalStringLength % 128; i < ModNumber.OctalStringLength; i += 128)
            {
                exp.Append(new string('0', 64));
                exp.Append(new string('7', 64));
            }
            string res = ml.ToString(8);
            Assert.IsTrue(exp.ToString() == res);

        }
        [TestMethod]
        public void TestToStringHexForZero()
        {
            ModNumber ml = new ModNumber(0ul);
            string exp = new string('0', ModNumber.HexStringLength);
            string res = ml.ToString(16);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringHexForOne()
        {
            ModNumber ml = new ModNumber(1ul);
            string exp = new string('0', ModNumber.HexStringLength - 1);
            exp += '1';
            string res = ml.ToString(16);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringHexForSixteen()
        {
            ModNumber ml = new ModNumber(16ul);
            string exp = new string('0', ModNumber.HexStringLength - 2);
            exp += "10";
            string res = ml.ToString(16);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringHexForMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            string exp = new string('F', ModNumber.HexStringLength);
            string res = ml.ToString(16);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringDecimalForZero()
        {
            ModNumber ml = new ModNumber(0ul);
            string exp = new string('0', ModNumber.DecimalStringLength);
            string res = ml.ToString(10);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringDecimalForOne()
        {
            ModNumber ml = new ModNumber(1ul);
            string exp = new string('0', ModNumber.DecimalStringLength - 1);
            exp += "1";
            string res = ml.ToString(10);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringDecimalForTen()
        {
            ModNumber ml = new ModNumber(10ul);
            string exp = new string('0', ModNumber.DecimalStringLength - 2);
            exp += "10";
            string res = ml.ToString(10);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringDecimalForMaxIntPlusOne()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = 1ul << ModNumber.ISIZE * 8;
            ModNumber ml = new ModNumber(l);
            string exp = new string('0', ModNumber.DecimalStringLength - 10);
            exp += "4294967296";
            string res = ml.ToString(10);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringDecimalForMaxInt()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = (uint)~0u;
            ModNumber ml = new ModNumber(l);
            string exp = new string('0', ModNumber.DecimalStringLength - 10);
            exp += "4294967295";
            string res = ml.ToString(10);
            Assert.IsTrue(res == exp);
        }
        [TestMethod]
        public void TestToStringDecimalForMax()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
            }
            ModNumber ml = new ModNumber(l);
            string res = ml.ToString(10);
            ModNumber exp = ModNumber.Stomn(res, 10);
            Assert.IsTrue(ml == exp);
        }
        [TestMethod]
        public void TestToModularNumberIllegalBase()
        {
            string s = "";
            Assert.ThrowsException<ArgumentException>(() => { ModNumber n = ModNumber.Stomn(s, 11); });
        }
        [TestMethod]
        public void TestToModularNumberIllegalChar()
        {
            string s = "123456789ABCDEFG";
            Assert.ThrowsException<ArgumentException>(() => { ModNumber n = ModNumber.Stomn(s, 16); });
        }
        [TestMethod]
        public void TestToModularNumberHexForEmptyString()
        {
            string s = "";
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = ModNumber.Stomn(s, 16);
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestToModularNumberHexForOne()
        {
            string s = "1";
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = ModNumber.Stomn(s, 16);
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestToModularNumberHexForSixteen()
        {
            string s = "10";
            ModNumber mexp = new ModNumber(16ul);
            ModNumber mres = ModNumber.Stomn(s, 16);
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestToModularNumberHexForSixteenWithLeadingZeros()
        {
            string s = "000000000000000000000000010";
            ModNumber mexp = new ModNumber(16ul);
            ModNumber mres = ModNumber.Stomn(s, 16);
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestToModularNumberHexForSixteenWithLeadingMinusSign()
        {
            string s = "-10";
            Assert.ThrowsException<ArgumentException>(() => { ModNumber mres = ModNumber.Stomn(s, 16); });
        }
        [TestMethod]
        public void TestToModularNumberHexForSixteenWithLeadingPlusSign()
        {
            string s = "+10";
            ModNumber mexp = new ModNumber(16ul);
            ModNumber mres = ModNumber.Stomn(s, 16);
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestToModularNumberHexForMax()
        {
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                exp[i] = ~0ul;
            }
            ModNumber mexp = new ModNumber(exp);
            string str = new string('F', ModNumber.HexStringLength);
            ModNumber mres = ModNumber.Stomn(str, 16);
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestToModularNumberHexIncreasingSequenceByteInput()
        {
            byte[] exp = new byte[ModNumber.NCOUNT];
            for (int i = 0; i < ModNumber.NCOUNT; i++)
            {
                exp[i] = (byte)(i % 16);
            }
            ModNumber mexp = new ModNumber(exp);
            StringBuilder stringBuilder = new StringBuilder(ModNumber.HexStringLength);
            for (int i = 0; i * 2 < ModNumber.HexStringLength % 32; i++)
            {
                string s = string.Format("{0,2:X2}", (ModNumber.HexStringLength % 32) / 2 - i - 1);
                stringBuilder.Append(s);
            }
            for (int i = ModNumber.HexStringLength % 32; i < ModNumber.HexStringLength; i += 32)
            {
                stringBuilder.Append("0F0E0D0C0B0A09080706050403020100");
            }
            ModNumber mres = ModNumber.Stomn(stringBuilder.ToString(), 16);
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestToModularNumberHexIncreasingSequence()
        {
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i + 1 < ModNumber.LCOUNT; i += 2)
            {
                exp[i] = 0x0706050403020100ul;
                exp[i + 1] = 0x0F0E0D0C0B0A0908ul;

            }
            ModNumber mexp = new ModNumber(exp);
            StringBuilder stringBuilder = new StringBuilder(ModNumber.HexStringLength);
            for (int i = 0; i < ModNumber.HexStringLength % 32; i++)
            {
                stringBuilder.Append('0');
            }
            for (int i = ModNumber.HexStringLength % 32; i < ModNumber.HexStringLength; i += 32)
            {
                stringBuilder.Append("0F0E0D0C0B0A09080706050403020100");
            }
            ModNumber mres = ModNumber.Stomn(stringBuilder.ToString(), 16);
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestToModularNumberHexIncreasingSequenceSwitched()
        {
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i + 1 < ModNumber.LCOUNT; i += 2)
            {
                exp[i] = 0x08090A0B0C0D0E0Ful;
                exp[i + 1] = 0x0001020304050607ul;

            }
            ModNumber mexp = new ModNumber(exp);
            StringBuilder stringBuilder = new StringBuilder(ModNumber.HexStringLength);
            for (int i = 0; i < ModNumber.HexStringLength % 32; i++)
            {
                stringBuilder.Append('0');
            }
            for (int i = ModNumber.HexStringLength % 32; i < ModNumber.HexStringLength; i += 32)
            {
                stringBuilder.Append("000102030405060708090A0B0C0D0E0F");
            }
            ModNumber mres = ModNumber.Stomn(stringBuilder.ToString(), 16);
            Assert.IsTrue(mres == mexp);
        }
        [TestMethod]
        public void TestToModularNumberDecimalForEmptyString()
        {
            ModNumber mexp = new ModNumber(0ul);
            string s = "";
            Assert.IsTrue(mexp == ModNumber.Stomn(s, 10));
        }
        [TestMethod]
        public void TestToModularNumberDecimalIllegalChar()
        {
            string s = "123456789A";
            Assert.ThrowsException<ArgumentException>(() => ModNumber.Stomn(s, 10));
        }
        [TestMethod]
        public void TestToModularNumberDecimalWithLeadingSpaces()
        {
            ModNumber mexp = new ModNumber(9ul);
            string s = "        9";
            Assert.IsTrue(mexp == ModNumber.Stomn(s, 10));
        }
        [TestMethod]
        public void TestToModularNumberDecimalForOne()
        {
            ModNumber mexp = new ModNumber(1ul);
            string s = "1";
            Assert.IsTrue(mexp == ModNumber.Stomn(s, 10));
        }
        [TestMethod]
        public void TestToModularNumberDecimalForTen()
        {
            ModNumber mexp = new ModNumber(10ul);
            string s = "10";
            Assert.IsTrue(mexp == ModNumber.Stomn(s, 10));
        }
        [TestMethod]
        public void TestToModularNumberDecimalForTenWithLeadingZeros()
        {
            ModNumber mexp = new ModNumber(10ul);
            string s = "             10";
            Assert.IsTrue(mexp == ModNumber.Stomn(s, 10));
        }
        [TestMethod]
        public void TestToModularNumberDecimalForTenWithLeadingMinusSign()
        {
            string s = "-10";
            Assert.ThrowsException<ArgumentException>(() => ModNumber.Stomn(s, 10));
        }
        [TestMethod]
        public void TestToModularNumberDecimalForTenWithLeadingPlusSign()
        {
            ModNumber mexp = new ModNumber(10ul);
            string s = "+10";
            Assert.IsTrue(mexp == ModNumber.Stomn(s, 10));
        }
        [TestMethod]
        public void TestToModularNumberDecimalForTenNines()
        {
            ModNumber mexp = new ModNumber(0x2540BE3FFul);
            StringBuilder s = new StringBuilder(ModNumber.DecimalStringLength);
            s.Append('0', ModNumber.DecimalStringLength - 10);
            s.Append('9', 10);
            Assert.IsTrue(mexp == ModNumber.Stomn(s.ToString(), 10));
        }
        [TestMethod]
        public void TestToModularNumberDecimalForEighteenNines()
        {
            ModNumber mexp = new ModNumber(0xDE0B6B3A763FFFFul);
            StringBuilder s = new StringBuilder(ModNumber.DecimalStringLength);
            s.Append('0', ModNumber.DecimalStringLength - 18);
            s.Append('9', 18);
            Assert.IsTrue(mexp == ModNumber.Stomn(s.ToString(), 10));
        }
        [TestMethod]
        public void TestToModularNumberOctalForEmptyString()
        {
            ModNumber mexp = new ModNumber(0ul);
            string s = "";
            Assert.IsTrue(mexp == ModNumber.Stomn(s.ToString(), 8));
        }
        [TestMethod]
        public void TestToModularNumberOctalIllegalChar()
        {
            string s = "123456789A";
            Assert.ThrowsException<ArgumentException>(() => ModNumber.Stomn(s, 8));
        }
        [TestMethod]
        public void TestToModularNumberOctalForOne()
        {
            ModNumber mexp = new ModNumber(1ul);
            string s = "1";
            Assert.IsTrue(mexp == ModNumber.Stomn(s.ToString(), 8));
        }
        [TestMethod]
        public void TestToModularNumberOctalForSixteen()
        {
            ModNumber mexp = new ModNumber(16ul);
            string s = "20";
            Assert.IsTrue(mexp == ModNumber.Stomn(s.ToString(), 8));
        }
        [TestMethod]
        public void TestToModularNumberOctalForEightWithLeadingZeros()
        {
            ModNumber mexp = new ModNumber(8ul);
            string s = "000000000000000000010";
            Assert.IsTrue(mexp == ModNumber.Stomn(s.ToString(), 8));
        }
        [TestMethod]
        public void TestToModularNumberOctalForMax()
        {
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
                exp[i] = ~0ul;
            ModNumber mexp = new ModNumber(exp);
            StringBuilder s = new StringBuilder(ModNumber.OctalStringLength);
            s.Append('7', ModNumber.OctalStringLength);
            switch (ModNumber.NSIZE % 3)
            {
                case 0:
                    break;
                case 1:
                    s[0] = '1';
                    break;
                case 2:
                    s[0] = '3';
                    break;
            }
            Assert.IsTrue(mexp == ModNumber.Stomn(s.ToString(), 8));
        }
        [TestMethod]
        public void TestToModularNumberOctalForStringTooLarge()
        {
            StringBuilder s = new StringBuilder(ModNumber.OctalStringLength + 16);
            for (int i = 0; i < ModNumber.OctalStringLength + 1; i += 16)
                s.Append("0706050403020100");
            Assert.ThrowsException<ArgumentException>(() => ModNumber.Stomn(s.ToString(), 8));
        }
        [TestMethod]
        public void TestToModularNumberOctalIncreasingSequenceByteInput()
        {
            StringBuilder s = new StringBuilder(ModNumber.OctalStringLength);
            s.Append('0', ModNumber.OctalStringLength % 16);
            for (int i = ModNumber.OctalStringLength % 16; i < ModNumber.OctalStringLength; i += 16)
                s.Append("0001020304050607");
            ModNumber mres = ModNumber.Stomn(s.ToString(), 8);
            string resStr = mres.ToString(8);
            Assert.IsTrue(resStr == s.ToString());
        }
        [TestMethod]
        public void TestSerializationHexForZero()
        {
            ModNumber mzero = new ModNumber();
            ModNumber mexp = new ModNumber();
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mzero.Write(sw, 16);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 16);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationHexForOne()
        {
            ModNumber mone = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(1ul);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mone.Write(sw, 16);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 16);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationHexForSixteen()
        {
            ModNumber mn = new ModNumber(16ul);
            ModNumber mexp = new ModNumber(16ul);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mn.Write(sw, 16);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 16);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationHexForAllFFFF()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                n[i] = ~0ul;
                exp[i] = ~0ul;
            }
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mn.Write(sw, 16);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 16);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationHexForIncreasingSequence()
        {
            byte[] n = new byte[ModNumber.NCOUNT];
            byte[] exp = new byte[ModNumber.NCOUNT];
            for (int i = 0; i < ModNumber.NCOUNT; i++)
            {
                n[i] = (byte)i;
                exp[i] = (byte)i;
            }
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mn.Write(sw, 16);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 16);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationOctForZero()
        {
            ModNumber mzero = new ModNumber();
            ModNumber mexp = new ModNumber();
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mzero.Write(sw, 8);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 8);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationOctForOne()
        {
            ModNumber mone = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(1ul);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mone.Write(sw, 8);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 8);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationOctForSixteen()
        {
            ModNumber mn = new ModNumber(16ul);
            ModNumber mexp = new ModNumber(16ul);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mn.Write(sw, 8);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 8);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationOctForAllFFFF()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                n[i] = ~0ul;
                exp[i] = ~0ul;
            }
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mn.Write(sw, 8);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 8);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationOctForIncreasingSequence()
        {
            byte[] n = new byte[ModNumber.NCOUNT];
            byte[] exp = new byte[ModNumber.NCOUNT];
            for (int i = 0; i < ModNumber.NCOUNT; i++)
            {
                n[i] = (byte)i;
                exp[i] = (byte)i;
            }
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mn.Write(sw, 8);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 8);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationDecForZero()
        {
            ModNumber mzero = new ModNumber();
            ModNumber mexp = new ModNumber();
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mzero.Write(sw, 10);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 10);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationDecForOne()
        {
            ModNumber mone = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(1ul);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mone.Write(sw, 10);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 10);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationDecForSixteen()
        {
            ModNumber mn = new ModNumber(16ul);
            ModNumber mexp = new ModNumber(16ul);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mn.Write(sw, 10);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 10);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationDecForAllFFFF()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                n[i] = ~0ul;
                exp[i] = ~0ul;
            }
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mn.Write(sw, 10);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 10);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestSerializationDecForIncreasingSequence()
        {
            byte[] n = new byte[ModNumber.NCOUNT];
            byte[] exp = new byte[ModNumber.NCOUNT];
            for (int i = 0; i < ModNumber.NCOUNT; i++)
            {
                n[i] = (byte)i;
                exp[i] = (byte)i;
            }
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            using (MemoryStream ms = new MemoryStream())
            {
                using StreamWriter sw = new StreamWriter(ms);
                mn.Write(sw, 10);
                sw.Flush();
                ms.Position = 0;
                using StreamReader sr = new StreamReader(ms);
                ModNumber mres = ModNumber.Read(sr, 10);
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestMultiplyByZero()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
                l[i] = (ulong)i;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(0ul);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyByOne()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = (ulong)i;
                exp[i] = (ulong)i;
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyByTwo()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = (ulong)i;
                exp[i] = (ulong)(i * 2);
            }
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyAllFFFFByTwo()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
            {
                l[i] = ~0ul;
                exp[i] = ~0ul;
            }
            exp[ModNumber.LCOUNT - 1] = 1ul;
            exp[0] -= 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyFsByTwoPower16()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = ~0ul >> (ModNumber.LSIZE * 8 - 16);
            exp[0] = ~0ul << 16;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(65536ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyAllFFFFByAllFFFF()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
            }
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyEightsBy2()
        {
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x1111111111111110ul;
            exp[1] = 0x1ul;
            ModNumber ml = new ModNumber(0x8888888888888888ul);
            ModNumber mr = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiply9sDecBy9sDec()
        {
            ModNumber ml = new ModNumber(9999999999999999ul);
            ModNumber mr = new ModNumber(9999999999999999ul);
            string exp = "99999999999999980000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp, 10);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyTwoBlocks9sDecByTwoBlocks9sDec()
        {
            string tmp = "9999999999999999";
            string l = tmp + tmp;
            string r = l;
            string exp = "9999999999999999999999999999999800000000000000000000000000000001";
            ModNumber ml = ModNumber.Stomn(l, 10);
            ModNumber mr = ModNumber.Stomn(r, 10);
            ModNumber mexp = ModNumber.Stomn(exp, 10);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyFourBlocks9sDecByFourBlocks9sDec()
        {
            string tmp1 = "9999999999999999";
            string tmp2 = tmp1 + tmp1;
            string l = tmp2 + tmp2;
            string r = l;
            string exp = "99999999999999999999999999999999999999999999999999999999999999980000000000000000000000000000000000000000000000000000000000000001";
            ModNumber ml = ModNumber.Stomn(l, 10);
            ModNumber mr = ModNumber.Stomn(r, 10);
            ModNumber mexp = ModNumber.Stomn(exp, 10);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyEightBlocks9sDecByEightBlocks9sDec()
        {
            string tmp1 = "9999999999999999";
            string tmp2 = tmp1 + tmp1;
            string tmp3 = tmp2 + tmp2;
            string l = tmp3 + tmp3;
            string r = l;
            string exp = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001";
            ModNumber ml = ModNumber.Stomn(l, 10);
            ModNumber mr = ModNumber.Stomn(r, 10);
            ModNumber mexp = ModNumber.Stomn(exp, 10);
            ModNumber mres = ml * mr;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplySixteenBlocks9sDecBySixteenBlocks9sDec()
        {
            if (ModNumber.MaxMod > 1024 / 8)
            {
                string tmp1 = "9999999999999999";
                string tmp2 = tmp1 + tmp1;
                string tmp3 = tmp2 + tmp2;
                string tmp4 = tmp3 + tmp3;
                string l = tmp4 + tmp4;
                string r = l;
                string exp = "99999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999980000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001";
                ModNumber ml = ModNumber.Stomn(l, 10);
                ModNumber mr = ModNumber.Stomn(r, 10);
                ModNumber mexp = ModNumber.Stomn(exp, 10);
                ModNumber mres = ml * mr;
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestMultiplyThirtyOneBlocks9sDecByThirtyOneBlocks9sDec()
        {
            if (ModNumber.MaxMod == 4096 / 8)
            {
                string tmp1 = "9999999999999999";
                string tmp2 = tmp1 + tmp1;
                string tmp3 = tmp2 + tmp2;
                string tmp4 = tmp3 + tmp3;
                string tmp5 = tmp4 + tmp4;
                string l = tmp5 + tmp4 + tmp3 + tmp2 + tmp1;
                string r = l;
                string exp = "99999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999980000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001";
                ModNumber ml = ModNumber.Stomn(l, 10);
                ModNumber mr = ModNumber.Stomn(r, 10);
                ModNumber mexp = ModNumber.Stomn(exp, 10);
                ModNumber mres = ml * mr;
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestMultiplyThirtyTwoBlocks9sDecByThirtyTwoBlocks9sDec()
        {
            if (ModNumber.MaxMod == 4096 / 8)
            {
                string tmp1 = "9999999999999999";
                string tmp2 = tmp1 + tmp1;
                string tmp3 = tmp2 + tmp2;
                string tmp4 = tmp3 + tmp3;
                string tmp5 = tmp4 + tmp4;
                string l = tmp5 + tmp5;
                string r = l;
                string exp = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001";
                ModNumber ml = ModNumber.Stomn(l, 10);
                ModNumber mr = ModNumber.Stomn(r, 10);
                ModNumber mexp = ModNumber.Stomn(exp, 10);
                ModNumber mres = ml * mr;
                Assert.IsTrue(mexp == mres);
            }
        }
        [TestMethod]
        public void TestMultGroupModAboveMax()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[ModNumber.COUNTMOD] = 1ul;
            ModNumber mn = new ModNumber(n);
            Assert.ThrowsException<ArgumentException>(() => new MultGroupMod(mn));
        }
        [TestMethod]
        public void TestMultiplyMultGroupModByZero()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD; i++)
            {
                l[i] = (ulong)i;
                n[i] = (ulong)i;
            }
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(0ul);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModByOne()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD; i++)
            {
                l[i] = (ulong)i;
                n[i] = (ulong)i;
            }
            n[0] += 1;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(ml);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModByTwoResultEqMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD; i++)
            {
                l[i] = (ulong)i;
                n[i] = (ulong)i * 2;
            }
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModByTwoResultLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD; i++)
            {
                l[i] = (ulong)i;
                n[i] = (ulong)i * 2;
                exp[i] = (ulong)i * 2;
            }
            n[0] += 1;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModAllFFFFByTwoResultEqMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD; i++)
            {
                l[i] = ~0ul;
                n[i] = ~0ul;
            }
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModAllFFFFByTwoResultLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD - 1; i++)
            {
                l[i] = ~0ul;
                n[i] = ~0ul;
                exp[i] = ~0ul;
            }
            n[ModNumber.COUNTMOD - 1] = 1ul;
            exp[ModNumber.COUNTMOD - 1] = 1ul;
            exp[0] -= 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModFsByPow16ResultLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[2] = 1ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = ~0ul >> (ModNumber.LSIZE * 8 - 16);
            exp[0] = ~0ul << 16;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(65536ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModFsByFsResultModOne3rdBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[0] = ~0ul;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[2] = 1ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = ~0ul - 1ul;
            exp[0] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModFsByFsResultModOne2ndBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[0] = ~0ul;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[1] = 1ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModFsByFsResultModEs1thBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[0] = ~0ul;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[0] = 0xEEEEEEEEEEEEEEEEul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x1111111111111111ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModAllFsByAllFsResultLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
            }
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[ModNumber.LCOUNT - 3] = 1ul;
            ModNumber mn = new ModNumber(n);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModAllFsByAllFsResultGreaterMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
            }
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[1] = 1ul;
            ModNumber mn = new ModNumber(n);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupMod8sBy2ResultLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = 0x8888888888888888ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[0] = 2ul;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[2] = 1ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x1111111111111110ul;
            exp[1] = 0x1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupMod8sBy2ResultGreaterMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = 0x8888888888888888ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[0] = 2ul;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[1] = 1ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x1111111111111110ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupMod9sDecBy9sDecResultLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[0] = 9999999999999999ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[0] = 9999999999999999ul;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[4] = 1ul;
            string exp = "99999999999999980000000000000001";
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModTwoBlock9sDecByTwoBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[4] = 1ul;
            string exp = "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModTwoBlock9sDecByTwoBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModFourBlock9sDecByFourBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[8] = 1ul;
            string exp = "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModFourBlock9sDecByFourBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }

#if LARGEMOD || MEDMOD
        [TestMethod]
        public void TestMultiplyMultGroupModEightBlock9sDecByEightBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[16] = 1ul;
            string exp = "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }

#endif
        [TestMethod]
        public void TestMultiplyMultGroupModEightBlock9sDecByEightBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            ModNumber mexp = new ModNumber(exp);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }

#if LARGEMOD
    [TestMethod]
    public void TestMultiplyMultGroupModSixteenBlock9sDecBySixteenBlock9sDecResultLessMod()
    {
        string lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        string rstr = lstr;
        ModNumber ml = ModNumber.Stomn(lstr);
        ModNumber mr = ModNumber.Stomn(rstr);
        ulong[] n = new ulong[ModNumber.LCOUNT];
        n[32] = 1ul;
        string exp = "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999998";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000001";
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = ModNumber.Stomn(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        Assert.IsTrue(mexp == mres);
    }

#endif
        [TestMethod]
        public void TestMultiplyMultGroupModSixteenBlock9sDecBySixteenBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            ModNumber mexp = new ModNumber(exp);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
#if LARGEMOD
        [TestMethod]
        public void TestMultiplyMultGroupModThirtyOneBlock9sDecByThirtyOneBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.Substring(0, ModNumber.NCOUNT - 24);
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[ModNumber.LCOUNT - 2] = 1ul;
            string exp = "";
            for (int i = 0; i < 30; i++)
                exp += "9999999999999999";
            exp += "9999999999999998";
            for (int i = 0; i < 30; i++)
                exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }

#endif
#if LARGEMOD || MEDMOD
        [TestMethod]
        public void TestMultiplyMultGroupModThirtyOneBlock9sDecByThirtyOneBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.Substring(0, ModNumber.NCOUNT - 24);
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[ModNumber.LCOUNT - 2] = 1ul;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestMultiplyMultGroupModThirtyTwoBlock9sDecByThirtyTwoBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string rstr = lstr;
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mr = ModNumber.Stomn(rstr);
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[ModNumber.LCOUNT - 2] = 1ul;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 1ul;
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            Assert.IsTrue(mexp == mres);
        }

#endif
        [TestMethod]
        public void TestGcdOfOneAndZero()
        {
            ModNumber mzero = new ModNumber(0ul);
            ModNumber mone = new ModNumber(1ul);
            ModNumber mres;
            Assert.ThrowsException<ArgumentException>(() => mres = ModNumber.Gcd(mone, mzero));
        }
        [TestMethod]
        public void TestGcdOfZeroAndOne()
        {
            ModNumber mzero = new ModNumber(0ul);
            ModNumber mone = new ModNumber(1ul);
            ModNumber mres;
            Assert.ThrowsException<ArgumentException>(() => mres = ModNumber.Gcd(mzero, mone));
        }
        [TestMethod]
        public void TestGcdOf100AndOne()
        {
            ModNumber monehundred = new ModNumber(100ul);
            ModNumber mone = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = ModNumber.Gcd(monehundred, mone);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGcdOfOneAnd100()
        {
            ModNumber monehundred = new ModNumber(100ul);
            ModNumber mone = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = ModNumber.Gcd(mone, monehundred);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGcdOf101And100()
        {
            ModNumber monehundred = new ModNumber(100ul);
            ModNumber monehundredone = new ModNumber(101ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = ModNumber.Gcd(monehundredone, monehundred);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGcdOf102And100()
        {
            ModNumber monehundred = new ModNumber(100ul);
            ModNumber monehundredtwo = new ModNumber(102ul);
            ModNumber mexp = new ModNumber(2ul);
            ModNumber mres = ModNumber.Gcd(monehundredtwo, monehundred);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGcdOf100And102()
        {
            ModNumber monehundred = new ModNumber(100ul);
            ModNumber monehundredtwo = new ModNumber(102ul);
            ModNumber mexp = new ModNumber(2ul);
            ModNumber mres = ModNumber.Gcd(monehundred, monehundredtwo);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGcdOf400And600()
        {
            ModNumber mfourhundred = new ModNumber(400ul);
            ModNumber msixhundred = new ModNumber(600ul);
            ModNumber mexp = new ModNumber(200ul);
            ModNumber mres = ModNumber.Gcd(mfourhundred, msixhundred);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGcdOfAllFin2ndBlockAndAllFin1thBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[1] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[0] = ~0ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.Gcd(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGcdOfAllFinLastBlockAndAllFin2ndBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT-1] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[1] = ~0ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.Gcd(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGcdOfAllFinLastBlockAndAllAin3rdBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[2] = 0xaaaaaaaaaaaaaaaaul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[2] = 0xaaaaaaaaaaaaaaaaul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.Gcd(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGcdOfAllFinLastBlockAndAllBin3rdBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[2] = 0xbbbbbbbbbbbbbbbbul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[2] = 0x1111111111111111ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.Gcd(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestLcmOf101And100()
        {
            ModNumber monehundred = new ModNumber(100ul);
            ModNumber monehundredone = new ModNumber(101ul);
            ModNumber mexp = new ModNumber(10100ul);
            ModNumber mres = ModNumber.Lcm(monehundredone, monehundred);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestLcmOf102And100()
        {
            ModNumber monehundred = new ModNumber(100ul);
            ModNumber monehundredtwo = new ModNumber(102ul);
            ModNumber mexp = new ModNumber(5100ul);
            ModNumber mres = ModNumber.Lcm(monehundredtwo, monehundred);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestLcmOf100And102()
        {
            ModNumber monehundred = new ModNumber(100ul);
            ModNumber monehundredtwo = new ModNumber(102ul);
            ModNumber mexp = new ModNumber(5100ul);
            ModNumber mres = ModNumber.Lcm(monehundred, monehundredtwo);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestLcmOf400And600()
        {
            ModNumber mfourhundred = new ModNumber(400ul);
            ModNumber msixhundred = new ModNumber(600ul);
            ModNumber mexp = new ModNumber(1200ul);
            ModNumber mres = ModNumber.Lcm(mfourhundred, msixhundred);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestLcmOfAllFin2ndBlockAndAllFin1thBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[1] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[0] = ~0ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[1] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.Lcm(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestLcmOfAllFinLastBlockAndAllFin2ndBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[1] = ~0ul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[ModNumber.LCOUNT - 1] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.Lcm(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestLcmOfAllFinLastBlockAndAllAin3rdBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[2] = 0xaaaaaaaaaaaaaaaaul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[ModNumber.LCOUNT - 1] = ~0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.Lcm(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestLcmOfAllFinLastBlockAndAllBin3rdBlock()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            l[ModNumber.LCOUNT - 1] = ~0ul;
            ulong[] r = new ulong[ModNumber.LCOUNT];
            r[2] = 0xbbbbbbbbbbbbbbbbul;
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[ModNumber.LCOUNT - 1] = 0xfffffffffffffff5ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.Lcm(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOfZero()
        {
            ModNumber mn = new ModNumber(0ul);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOfOne()
        {
            ModNumber mn = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOfFour()
        {
            ModNumber mn = new ModNumber(4ul);
            ModNumber mexp = new ModNumber(2ul);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOfTwentyFive()
        {
            ModNumber mn = new ModNumber(25ul);
            ModNumber mexp = new ModNumber(5ul);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOfOneHundredSixtyNine()
        {
            ModNumber mn = new ModNumber(169ul);
            ModNumber mexp = new ModNumber(13ul);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOf152399025()
        {
            ModNumber mn = new ModNumber(152399025ul);
            ModNumber mexp = new ModNumber(12345ul);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOf1524157875019052100()
        {
            ModNumber mn = new ModNumber(1524157875019052100ul);
            ModNumber mexp = new ModNumber(1234567890ul);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOf152415787532374345526722756()
        {
            ModNumber mn = ModNumber.Stomn("152415787532374345526722756");
            ModNumber mexp = new ModNumber(12345678901234ul);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOfAllFsSquared()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            int numInts = ModNumber.LCOUNT % 2 == 0 ? ModNumber.LCOUNT : ModNumber.LCOUNT - 1;
            for (int i = numInts / 2; i < numInts; i++)
                n[i] = ~0ul;
            n[numInts / 2] <<= 1;
            n[0] = 1;
            for (int i = 0; i < numInts / 2; i++)
                exp[i] = ~0ul;
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtOf1InLastWord()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            int numInts = ModNumber.LCOUNT % 2 == 1 ? ModNumber.LCOUNT : ModNumber.LCOUNT - 1;
            n[numInts-1] = 1ul;
            exp[(numInts - 1)/2] = 1ul;
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mn.Sqrt();
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestSqrtPrecision18Of2()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 18);
            ulong [] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x09e667f3bcc908b2ul;
            exp[1] = 0x016aul;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 18 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecision18Of2StrHex()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 18);
            string exp = new string('0',ModNumber.HexStringLength-19);
            exp += "1.6A09E667F3BCC908B2";
            ScaledNumber snres = sn.Sqrt();
            string resStr = snres.ToString(16);
            Assert.IsTrue(exp == resStr);

        }
        [TestMethod]
        public void TestSqrtPrecision18Of2StrDec()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 18);
            ScaledNumber snres = sn.Sqrt();
            int IntegerStringLength = snres.calculateDecimalStringLengthLeft();
            string exp = new string('0', IntegerStringLength - 1);
            exp += "1.414213562373095048";
            Assert.IsTrue(18 / 2 == snres.scale);
            string resStr = snres.ToString();
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision18Of2StrOctal()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 18);
            ScaledNumber snres = sn.Sqrt();
            (int digitsLeft, int digitsRight) = snres.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft - 1);
            exp += "1.324047463177167462204262";
            Assert.IsTrue(18 / 2 == snres.scale);
            string resStr = snres.ToString(8);
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision16Of2()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 16);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x6a09e667f3bcc908ul;
            exp[1] = 0x01ul;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 16 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecision16Of2StrHex()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 16);
            string exp = new string('0', ModNumber.HexStringLength - 17);
            exp += "1.6A09E667F3BCC908";
            ScaledNumber snres = sn.Sqrt();
            string resStr = snres.ToString(16);
            Assert.IsTrue(exp == resStr);

        }
        [TestMethod]
        public void TestSqrtPrecision16Of2StrDec()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 16);
            ScaledNumber snres = sn.Sqrt();
            int IntegerStringLength = snres.calculateDecimalStringLengthLeft();
            string exp = new string('0', IntegerStringLength - 1);
            exp += "1.4142135623730950";
            Assert.IsTrue(16 / 2 == snres.scale);
            string resStr = snres.ToString();
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision16Of2StrOctal()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 16);
            ScaledNumber snres = sn.Sqrt();
            (int digitsLeft, int digitsRight) = snres.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft - 1);
            exp += "1.3240474631771674622040";
            Assert.IsTrue(16 / 2 == snres.scale);
            string resStr = snres.ToString(8);
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision14Of2()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 14);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x016a09e667f3bcc9ul;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 14 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecision14Of2StrHex()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 14);
            string exp = new string('0', ModNumber.HexStringLength - 15);
            exp += "1.6A09E667F3BCC9";
            ScaledNumber snres = sn.Sqrt();
            string resStr = snres.ToString(16);
            Assert.IsTrue(exp == resStr);

        }
        [TestMethod]
        public void TestSqrtPrecision14Of2StrDec()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 14);
            ScaledNumber snres = sn.Sqrt();
            int IntegerStringLength = snres.calculateDecimalStringLengthLeft();
            string exp = new string('0', IntegerStringLength - 1);
            exp += "1.41421356237309";
            Assert.IsTrue(14 / 2 == snres.scale);
            string resStr = snres.ToString();
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision14Of2StrOctal()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 14);
            ScaledNumber snres = sn.Sqrt();
            (int digitsLeft, int digitsRight) = snres.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft - 1);
            exp += "1.3240474631771674622";
            Assert.IsTrue(14 / 2 == snres.scale);
            string resStr = snres.ToString(8);
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision12Of2()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 12);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x016a09e667f3bcul;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 12 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecision12Of2StrHex()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 12);
            string exp = new string('0', ModNumber.HexStringLength - 13);
            exp += "1.6A09E667F3BC";
            ScaledNumber snres = sn.Sqrt();
            string resStr = snres.ToString(16);
            Assert.IsTrue(exp == resStr);

        }
        [TestMethod]
        public void TestSqrtPrecision12Of2StrDec()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 12);
            ScaledNumber snres = sn.Sqrt();
            int IntegerStringLength = snres.calculateDecimalStringLengthLeft();
            string exp = new string('0', IntegerStringLength - 1);
            exp += "1.414213562373";
            Assert.IsTrue(12 / 2 == snres.scale);
            string resStr = snres.ToString();
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision12Of2StrOctal()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 12);
            ScaledNumber snres = sn.Sqrt();
            (int digitsLeft, int digitsRight) = snres.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft - 1);
            exp += "1.3240474631771674";
            Assert.IsTrue(12 / 2 == snres.scale);
            string resStr = snres.ToString(8);
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision10Of2()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 10);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x016a09e667f3ul;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 10 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecision10Of2StrHex()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 10);
            string exp = new string('0', ModNumber.HexStringLength - 11);
            exp += "1.6A09E667F3";
            ScaledNumber snres = sn.Sqrt();
            string resStr = snres.ToString(16);
            Assert.IsTrue(exp == resStr);

        }
        [TestMethod]
        public void TestSqrtPrecision10Of2StrDec()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 10);
            ScaledNumber snres = sn.Sqrt();
            int IntegerStringLength = snres.calculateDecimalStringLengthLeft();
            string exp = new string('0', IntegerStringLength - 1);
            exp += "1.4142135623";
            Assert.IsTrue(10 / 2 == snres.scale);
            string resStr = snres.ToString();
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision10Of2StrOctal()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 10);
            ScaledNumber snres = sn.Sqrt();
            (int digitsLeft, int digitsRight) = snres.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft - 1);
            exp += "1.32404746317714";
            Assert.IsTrue(10 / 2 == snres.scale);
            string resStr = snres.ToString(8);
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision8Of2()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x016a09e667ul;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 8 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecision8Of2StrHex()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            string exp = new string('0', ModNumber.HexStringLength - 9);
            exp += "1.6A09E667";
            ScaledNumber snres = sn.Sqrt();
            string resStr = snres.ToString(16);
            Assert.IsTrue(exp == resStr);

        }
        [TestMethod]
        public void TestSqrtPrecision8Of2StrDec()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ScaledNumber snres = sn.Sqrt();
            int IntegerStringLength = snres.calculateDecimalStringLengthLeft();
            string exp = new string('0', IntegerStringLength - 1);
            exp += "1.41421356";
            Assert.IsTrue(8 / 2 == snres.scale);
            string resStr = snres.ToString();
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision8Of2StrOctal()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ScaledNumber snres = sn.Sqrt();
            (int digitsLeft, int digitsRight) = snres.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft - 1);
            exp += "1.32404746316";
            Assert.IsTrue(8 / 2 == snres.scale);
            string resStr = snres.ToString(8);
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision4Of2()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 4);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x016a09ul;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 4 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecision4Of2StrHex()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 4);
            string exp = new string('0', ModNumber.HexStringLength - 5);
            exp += "1.6A09";
            ScaledNumber snres = sn.Sqrt();
            string resStr = snres.ToString(16);
            Assert.IsTrue(exp == resStr);

        }
        [TestMethod]
        public void TestSqrtPrecision4Of2StrDec()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 4);
            ScaledNumber snres = sn.Sqrt();
            int IntegerStringLength = snres.calculateDecimalStringLengthLeft();
            string exp = new string('0', IntegerStringLength - 1);
            exp += "1.4141";
            Assert.IsTrue(4 / 2 == snres.scale);
            string resStr = snres.ToString();
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision4Of2StrOctal()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 4);
            ScaledNumber snres = sn.Sqrt();
            (int digitsLeft, int digitsRight) = snres.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft - 1);
            exp += "1.324044";
            Assert.IsTrue(4 / 2 == snres.scale);
            string resStr = snres.ToString(8);
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision2Of2()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 2);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x016aul;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 2 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecision2Of2StrHex()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 2);
            string exp = new string('0', ModNumber.HexStringLength - 3);
            exp += "1.6A";
            ScaledNumber snres = sn.Sqrt();
            string resStr = snres.ToString(16);
            Assert.IsTrue(exp == resStr);

        }
        [TestMethod]
        public void TestSqrtPrecision2Of2StrDec()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 2);
            ScaledNumber snres = sn.Sqrt();
            int IntegerStringLength = snres.calculateDecimalStringLengthLeft();
            string exp = new string('0', IntegerStringLength - 1);
            exp += "1.41";
            Assert.IsTrue(2 / 2 == snres.scale);
            string resStr = snres.ToString();
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecision2Of2StrOctal()
        {
            ModNumber mn = new ModNumber(2ul);
            ScaledNumber sn = new ScaledNumber(mn, 2);
            ScaledNumber snres = sn.Sqrt();
            (int digitsLeft, int digitsRight) = snres.CalculateOctalStringLength();
            string exp = new string('0', digitsLeft - 1);
            exp += "1.324";
            Assert.IsTrue(2 / 2 == snres.scale);
            string resStr = snres.ToString(8);
            Assert.IsTrue(exp == resStr);
        }
        [TestMethod]
        public void TestSqrtPrecisionOf3()
        {
            ModNumber mn = new ModNumber(3ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x01bb67ae85;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 8 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecisionOf3Str()
        {
            ModNumber mn = new ModNumber(3ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            string exp = new string('0', ModNumber.HexStringLength - 9);
            exp += "1.BB67AE85";
            ScaledNumber snres = sn.Sqrt();
            string resStr = snres.ToString(16);
            Assert.IsTrue(exp == resStr);

        }
        [TestMethod]
        public void TestSqrtPrecisionOf5()
        {
            ModNumber mn = new ModNumber(5ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x023c6ef372;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 8 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecisionOf7()
        {
            ModNumber mn = new ModNumber(7ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x02a54ff53a;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 8 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecisionOf11()
        {
            ModNumber mn = new ModNumber(11ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x03510e527f;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 8 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecisionOf13()
        {
            ModNumber mn = new ModNumber(13ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x039b05688c;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 8 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecisionOf17()
        {
            ModNumber mn = new ModNumber(17ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x041f83d9ab;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 8 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestSqrtPrecisionOf19()
        {
            ModNumber mn = new ModNumber(19ul);
            ScaledNumber sn = new ScaledNumber(mn, 8);
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x045be0cd19;
            ModNumber mexp = new ModNumber(exp);
            ScaledNumber snexp = new ScaledNumber(mexp, 8 / 2, true);
            ScaledNumber snres = sn.Sqrt();
            Assert.IsTrue(snexp == snres);

        }
        [TestMethod]
        public void TestMultGroupModOfZero()
        {
            ModNumber mzero = new ModNumber(0ul);
            MultGroupMod mgm;
            Assert.ThrowsException<ArgumentException>(() => { mgm = new MultGroupMod(mzero); });

        }
        [TestMethod]
        public void TestMultGroupModOfOne()
        {
            ModNumber mone = new ModNumber(1ul);
            MultGroupMod mgm;
            Assert.ThrowsException<ArgumentException>(() => { mgm = new MultGroupMod(mone); });

        }
        [TestMethod]
        public void TestKwadMultGroupEqualMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD; i++)
            {
                l[i] = (ulong)i;
                n[i] = (ulong)i;
            }
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestKwadOneMultGroupLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            l[0] = 1ul;
            n[0] = 2ul; 
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber mexp = new ModNumber(ml);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestKwadMultGroupMod9sDecResultLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            l[0] = 9999999999999999ul;
            n[4] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            string exp = "99999999999999980000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestKwadMultGroupModTwoBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[4] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            string exp = "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestKwadMultGroupModTwoBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
             ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestKwadMultGroupModFourBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[8] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            string exp = "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestKwadMultGroupModFourBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
#if LARGEMOD || MEDMOD
        [TestMethod]
        public void TestKwadMultGroupModEightBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[16] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            string exp = "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }

#endif
        [TestMethod]
        public void TestKwadMultGroupModEightBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
#if LARGEMOD
        [TestMethod]
        public void TestKwadMultGroupModSixteenBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[32] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            string exp = "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }

#endif
        [TestMethod]
        public void TestKwadMultGroupModSixteenBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
#if LARGEMOD
        [TestMethod]
        public void TestKwadMultGroupModThirtyOneBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.Substring(0, 496);
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[ModNumber.LCOUNT-2] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            string exp = "";
            for (int i = 0; i < 30; i++)
                exp += "9999999999999999";
            exp += "9999999999999998";
            for (int i = 0; i < 30; i++)
                exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }

#endif
#if LARGEMOD || MEDMOD
        [TestMethod]
        public void TestKwadMultGroupModThirtyOneBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.Substring(0, 496);
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestKwadMultGroupModThirtyTwoBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Kwad(ml);
            Assert.AreEqual(mexp, mres);

        }

#endif
        [TestMethod]
        public void TestExpZeroMultGroupAllOnesLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
                l[i] = ~0ul;
            ModNumber mn = new ModNumber(1000ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber me = new ModNumber(0ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpOneMultGroupEqualMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD; i++)
            {
                l[i] = (ulong)i;
                n[i] = (ulong)i;
            }
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber me = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpOneMultGroupLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD; i++)
            {
                l[i] = (ulong)i;
                n[i] = (ulong)i + 1;
            }
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber me = new ModNumber(1ul);
            ModNumber mexp = new ModNumber(ml);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpTwoMultGroupEqualMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.COUNTMOD; i++)
            {
                l[i] = (ulong)i;
                n[i] = (ulong)i;
            }
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(0ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpTwoOneMultGroupLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            l[0] = 1ul;
            n[0] = 2ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(ml);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpTenOneMultGroupLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            l[0] = 1ul;
            n[0] = 2ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber me = new ModNumber(10ul);
            ModNumber mexp = new ModNumber(ml);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpTwoMultGroupMod9sDecResultLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            l[0] = 9999999999999999ul;
            n[4] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber me = new ModNumber(2ul);
            string exp = "99999999999999980000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpTwoMultGroupModTwoBlock9sDecResultLessMod()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            string lstr = "9999999999999999";
            lstr += lstr;
            n[4] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            string exp = "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpTwoMultGroupModTwoBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpTwoMultGroupModFourBlock9sDecResultLessMod()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            n[8] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            string exp = "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpTwoMultGroupModFourBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
#if LARGEMOD || MEDMOD
        [TestMethod]
        public void TestExpTwoMultGroupModEightBlock9sDecResultLessMod()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            n[16] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            string exp = "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
#endif
        [TestMethod]
        public void TestExpTwoMultGroupModEightBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
#if LARGEMOD
        [TestMethod]
        public void TestExpTwoMultGroupModSixteenBlock9sDecResultLessMod()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            n[32] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            string exp = "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999999";
            exp += "9999999999999998";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
#endif
        [TestMethod]
        public void TestExpTwoMultGroupModSixteenBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
#if LARGEMOD
        [TestMethod]
        public void TestTestExpTwoMultGroupModThirtyOneBlock9sDecResultLessMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.Substring(0, 496);
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[ModNumber.LCOUNT - 2] = 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            string exp = "";
            for (int i = 0; i < 30; i++)
                exp += "9999999999999999";
            exp += "9999999999999998";
            for (int i = 0; i < 30; i++)
                exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.Stomn(exp);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }

#endif
#if LARGEMOD || MEDMOD
        [TestMethod]
        public void TestExpTwoMultGroupModThirtyOneBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.Substring(0, 496);
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpTwoMultGroupModThirtyTwoBlock9sDecResultGreaterMod()
        {
            string lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            string nstr = "10000000000000000";
            ModNumber mn = ModNumber.Stomn(nstr);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.Stomn(lstr);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }

#endif
        [TestMethod]
        public void TestExpMultGroupModTwoPowerThirteenModTenThousand()
        {
            ModNumber ml = new ModNumber(2ul);
            ModNumber mn = new ModNumber(10000ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber me = new ModNumber(13ul);
            ModNumber mexp = new ModNumber(8192ul);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpMultGroupModFirstBlockAllOnesPowerTwoLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            n[2] = 1ul;
            exp[0] = 1ul;
            exp[1] = ~0ul - 1ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestExpMultGroupModFirstTwoBlockAllOnesPowerTwoLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            l[1] = ~0ul;
            n[4] = 1ul;
            exp[0] = 1ul;
            exp[1] = 0ul;
            exp[2] = ~0ul - 1ul;
            exp[3] = ~0ul;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = new ModNumber(l);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Exp(ml, me);
            Assert.AreEqual(mexp, mres);

        }
        [TestMethod]
        public void TestGroupPropertiesGroupModEleven()
        {
            ModNumber mx = new ModNumber(2ul);
            ModNumber mn = new ModNumber(11ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber me = new ModNumber(2ul);
            ModNumber mexp = new ModNumber(4ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
            me += 1u;
            mexp = new ModNumber(8ul);
            mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
            me += 1u;
            mexp = new ModNumber(5ul);
            mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
            me += 1u;
            mexp = new ModNumber(10ul);
            mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
            me += 1u;
            mexp = new ModNumber(9ul);
            mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
            me += 1u;
            mexp = new ModNumber(7ul);
            mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
            me += 1u;
            mexp = new ModNumber(3ul);
            mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
            me += 1u;
            mexp = new ModNumber(6ul);
            mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
            me += 1u;
            mexp = new ModNumber(1ul);
            mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);

        }
        [TestMethod]
        public void TestGCDLargeNumbers()
        {
            ModNumber mx = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            ModNumber my = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mres = ModNumber.Gcd(mx, my);
            ModNumber mexp = new ModNumber(1ul);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpTwoLargeNumberxModLargeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            ModNumber my = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp = ModNumber.Stomn("113638967017082606674052656070205979468957500859029166099119331283109255964689");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(2ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpThreeLargeNumberxModLargeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            ModNumber my = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp = ModNumber.Stomn("959159918151361352804382650352706011784068412944254732668580895529266851090113");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(3ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpFourLargeNumberxModLargeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            ModNumber my = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp = ModNumber.Stomn("43252875649600596472804069974401733886601470807478913480533005345660321341646");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(4ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpFiveLargeNumberxModLargeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            ModNumber my = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp = ModNumber.Stomn("161704454599373732186620154854028099753067612674910409272189542689295694371582");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(5ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpNineLargeNumberxModLargeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            ModNumber my = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp = ModNumber.Stomn("229491963118798811250703753130022542480533480148820031152039444666272832238347");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(9ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpFourteenLargeNumberxModLargeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            ModNumber my = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp = ModNumber.Stomn("512694095576449334310364050646143389495558145452890366434851422723699646134904");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(14ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpTwentyThreeLargeNumberxModLargeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            ModNumber my = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp = ModNumber.Stomn("762907244846308926487595589728323262016384923154412381938714858362885520672888");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(23ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
#if LARGEMOD
        [TestMethod]
        public void TestHugeNumberxModHugeNumbery1()
        {
            ModNumber mx = ModNumber.Stomn("67885277291854912938587378017853691459496193677756552480553436557302300320101312718354512366504982574838958861888542718272904603102504548837304584105562307267284676071603679712110072004987020967899883804718561430292652099594457858875289933946756413441096538132475400931050009417996185648350176662387902186108621517311830327723099115016623424851268094682681323829709292907275341890111203138351101069715278419043245098727421641629718536436838918204954343308555448232382368388580601057201273701188613232106352300442738562367465251389869660183071271610913707341230465789657707599097586310256539211176484567156528847506639917990006758261522292135816044123533114985744902968420423817686515256802404411459420510055567668821773480759839945638865270345325327032377340654619518821029267132265318694685281072806300858177007288594225758039156380947826545771278835135058430286184244368381139666029873832564755473361825936566690845345140947317051183032581586950151989519809420014114182249129767248172760729288655451472427647241152820746675436044819097064699101275497442767142366134874787841654717249301013092609819939109466282790957714894233169284428494458512543531352888077862617075549676041201530943138909340541678084859095964562913434445958610945");
            ModNumber my = ModNumber.Stomn("134726165702296673370426386890283366245915523314703970967013679118347220188155182690330977225943173840726855105111482919931305956440257849275520470254003646345501461780481515668802531224809023461784856160970148295396834447238617115943625107572025345987403272485443712847152864396338847358873706819626414636709346223482144399011474564965089003187992678881573427552175300955270709084383966983460129498021945865772279351116095761583530346030439079292145399495672419313497076142325458060700017164710304404461962683345576478284228376073670571646356401162299439184862813044493573253596123455176468974714188846054112527108968938884292759762043717438585685314138246495541580159383671282098306977461412321590398143395408933484974952209631431062016316646415085049130090980939899431089399294190073420318461005695618030273786677969332954501892209068829746374066239722536639068395204248225513550233334961960928605977988464525551842393134197366398782490722703917222543927938228662256671720674599120818771544202337991100431921007170249357400706732178943189857754690035153719671012851905711621053028628593731588178463669914156357640449019966758018645637841011686702148326897940500377576684094649809133657149348148237929343755825113110373434920873754635");
            ModNumber mexp = ModNumber.Stomn("66840888410441760431839008872429674786419329636947418486460242561044919868053869971976464859438191265887896243222940201658401353337753300438215886148441339078216785708877835956692459219822002493884972356251586865104182347644159257068335173625268932546306734352968311916102854978342661710523530157238512450600724706170314071288375449948465578336724584198892103722466008047995367194272763845109028428306667446729034252388674119953811809593600161087191056187116971081114707753744857003498743463521691172355610382902837915916763124683800911463285129551385731843632347254835865654498537144919929763537704278897583679602329020894286001500521425302769641190605131509796677190963247464411791720659007910130977633339841264663201471449791485423151046301089758016752750326320380610060132161924754725633179932889317172096779389375107196462735828121003200602787404587478208782210959879844373884203461129396173132616162527958860997047993250049347599458141116967070554408128808648142489471544831872646010814913682539628004273766017428610725270687359846125158653414537710952528646717030923779398311379292718495568643730804690074849491305072524849361209346553174158616974009862637760501134418608607602714010438807696251258896729148547460000474915143690");
            ModNumber mres = my % mx;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestHugeNumberxModHugeNumbery2()
        {
            ModNumber mx = ModNumber.Stomn("66840888410441760431839008872429674786419329636947418486460242561044919868053869971976464859438191265887896243222940201658401353337753300438215886148441339078216785708877835956692459219822002493884972356251586865104182347644159257068335173625268932546306734352968311916102854978342661710523530157238512450600724706170314071288375449948465578336724584198892103722466008047995367194272763845109028428306667446729034252388674119953811809593600161087191056187116971081114707753744857003498743463521691172355610382902837915916763124683800911463285129551385731843632347254835865654498537144919929763537704278897583679602329020894286001500521425302769641190605131509796677190963247464411791720659007910130977633339841264663201471449791485423151046301089758016752750326320380610060132161924754725633179932889317172096779389375107196462735828121003200602787404587478208782210959879844373884203461129396173132616162527958860997047993250049347599458141116967070554408128808648142489471544831872646010814913682539628004273766017428610725270687359846125158653414537710952528646717030923779398311379292718495568643730804690074849491305072524849361209346553174158616974009862637760501134418608607602714010438807696251258896729148547460000474915143690");
            ModNumber my = ModNumber.Stomn("67885277291854912938587378017853691459496193677756552480553436557302300320101312718354512366504982574838958861888542718272904603102504548837304584105562307267284676071603679712110072004987020967899883804718561430292652099594457858875289933946756413441096538132475400931050009417996185648350176662387902186108621517311830327723099115016623424851268094682681323829709292907275341890111203138351101069715278419043245098727421641629718536436838918204954343308555448232382368388580601057201273701188613232106352300442738562367465251389869660183071271610913707341230465789657707599097586310256539211176484567156528847506639917990006758261522292135816044123533114985744902968420423817686515256802404411459420510055567668821773480759839945638865270345325327032377340654619518821029267132265318694685281072806300858177007288594225758039156380947826545771278835135058430286184244368381139666029873832564755473361825936566690845345140947317051183032581586950151989519809420014114182249129767248172760729288655451472427647241152820746675436044819097064699101275497442767142366134874787841654717249301013092609819939109466282790957714894233169284428494458512543531352888077862617075549676041201530943138909340541678084859095964562913434445958610945");
            ModNumber mexp = ModNumber.Stomn("1044388881413152506748369145424016673076864040809133994093193996257380452047442746378047507066791308951062618665602516614503249764751248399088697957120968189067890362725843755417612785165018474014911448466974565188469751950298601806954760321487480894789803779507089014947154439653523937826646505149389735507896811141516256434723665068157846514543510483789220107243284859279974695838439293242072641408610972314210846338747521675906726843238757117763287121438477151267660634835744053702530237666922059750741917539900646450702126706068748719786142059527975497598118534821841944599049165336609447638780288258945167904310897095720756761000866833046402932927983475948225777457176353274723536143396501328442876715726404158572009310048460215714224044235569015624590328299138210969134970340563969052101139916983686080227899219118561576420552826823345168491430547580221503973284488536765781826412703168582340745663408607829848297147697267703583574440469983081435111680611365971692777584935375526749914374972911844423373475135392135950165357459250939540447860959731814613719417843864062256405870008294597041176208304776207941466409821708319923219147905338384914378878215224856574415257432593928229128470532845426825962366816015453433971043467255");
            ModNumber mres = my % mx;
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGCDHugeNumbersTooLargeForModGroup()
        {
            ModNumber mx = ModNumber.Stomn("67885277291854912938587378017853691459496193677756552480553436557302300320101312718354512366504982574838958861888542718272904603102504548837304584105562307267284676071603679712110072004987020967899883804718561430292652099594457858875289933946756413441096538132475400931050009417996185648350176662387902186108621517311830327723099115016623424851268094682681323829709292907275341890111203138351101069715278419043245098727421641629718536436838918204954343308555448232382368388580601057201273701188613232106352300442738562367465251389869660183071271610913707341230465789657707599097586310256539211176484567156528847506639917990006758261522292135816044123533114985744902968420423817686515256802404411459420510055567668821773480759839945638865270345325327032377340654619518821029267132265318694685281072806300858177007288594225758039156380947826545771278835135058430286184244368381139666029873832564755473361825936566690845345140947317051183032581586950151989519809420014114182249129767248172760729288655451472427647241152820746675436044819097064699101275497442767142366134874787841654717249301013092609819939109466282790957714894233169284428494458512543531352888077862617075549676041201530943138909340541678084859095964562913434445958610945");
            ModNumber my = ModNumber.Stomn("134726165702296673370426386890283366245915523314703970967013679118347220188155182690330977225943173840726855105111482919931305956440257849275520470254003646345501461780481515668802531224809023461784856160970148295396834447238617115943625107572025345987403272485443712847152864396338847358873706819626414636709346223482144399011474564965089003187992678881573427552175300955270709084383966983460129498021945865772279351116095761583530346030439079292145399495672419313497076142325458060700017164710304404461962683345576478284228376073670571646356401162299439184862813044493573253596123455176468974714188846054112527108968938884292759762043717438585685314138246495541580159383671282098306977461412321590398143395408933484974952209631431062016316646415085049130090980939899431089399294190073420318461005695618030273786677969332954501892209068829746374066239722536639068395204248225513550233334961960928605977988464525551842393134197366398782490722703917222543927938228662256671720674599120818771544202337991100431921007170249357400706732178943189857754690035153719671012851905711621053028628593731588178463669914156357640449019966758018645637841011686702148326897940500377576684094649809133657149348148237929343755825113110373434920873754626");
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = ModNumber.Gcd(mx, my);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestGCDHugeNumbersFitForModGroup()
        {
            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber my = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp = new ModNumber(1ul);
            ModNumber mres = ModNumber.Gcd(mx, my);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpTwoHugeNumberxModHugeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber my = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp = ModNumber.Stomn("5603130552131808582980129320151431593287387887422304019872729976542631800068788204319579898052711893227272328490995919630013482288843181272802592367798898524753249376917995287069518581823351056941728232218068059475906712986411367854982914146900624999167769127442072642482993253159299132482762489032243580550536702583738240633476465093340828218312246935203167115248772557253480443158870340161116689390922962852167364803089439715091390938186175699671994828460433891219975915334762748122803062377109012273998249380330075478906670900287087012812096397970441489185240044951680317263972659517703324836533035198756630319129921923499685884740053891793070603801214192199848233151436265254188036888028127957406260369087189527978590594475592951159744836132820316608295381151982751459808466074838264192706066312469551538268619096100114127505148129379927320373232121684138190307396104482210039324618282627066824207318147380670441172882452441196009256198174338127429977948883866986080614196689277860961306323052769429682104967016727143083349244333982457816038315667725514706061625905397835791561560354555405798114663550095985095263976338037301461279067122567382168347234118084620013800495920079606189028067751569874497346329975073");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(2ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpThreeHugeNumberxModHugeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber my = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp = ModNumber.Stomn("3250884643230911848040993900898243652249531513932618832535257978042483452600091491071448010142398100478042105296260000778740749600718579122776183390547427824993235206147742097168345798614754365320984010059029219682439536967146512822002373439785534655521802047121803418848012395656429274333098159369154311644233907570422616787627723475926083938361636819678257422496690100421348413270866873065352260789155886255064675768143173146339319044846435864434413724869138551196165618880665585280600177108055462531571760767361621253748226656844565125994418973371936230561763833035606088131573663901610503899191493774161996270592967697542825122532247346414087618074103430674872167210384103188364874320914572308032916218625415657467011179515861941549549325736647746798691179745994168171424216744438039562800153093515133179758626811333952347511710966151695995849060660173034462576806791324819774083226059537296440455387776103603089949810718851894140440522457547023362034434091138265699012346903498488388949729603118123456025773082476853228076714602761291459028567417244212052579099284228737307579246003530093230740016729330631060411372234970037396164109907373323398534755180276564538643086778729169896704343882921981536979169501836");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(3ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpFourHugeNumberxModHugeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber my = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp = ModNumber.Stomn("3080268413759030384341460902419062434997721043480105848866779760898068393567503640358696717983838966080526546827579170602983272318149414856875115837808486303722109044485795416198041955727945032540579727215123999690418312813355477277356083910442602752355111136928004841925810195818970742970436174828277478215405315973827710555567088163666229286786112475168136234677576560906615486193027124434823092257664145072814066899657089093522999907680713320331963071391531429767880888740895725104586646572927945523938405860480632775436157861081467981082184225014600032040927373958816953213700926896334768412238419934449779068554365771989177623416767861333527128330993153583146562417568714073880278726400518735126270015465444477429492644113567415231915774290228343428776481054611704559704237363844385486009720675254300380072203334120524517665759003680100816318161530169334907964499853628607313832400971196369426432557562516135301618512093776568587542511257533363363369929170820208909917939777029863549314112729980339793894778468238622396334080091491340293772514425601232034375252947892247868057474710141942798184238957688757577035052595292194615849826082321587207703091038792505545536888544063897477993799779043563238023394522327");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(4ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpFiveHugeNumberxModHugeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber my = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp = ModNumber.Stomn("5638711490193321968095582821025837950605444226618375138154713166364105598945660174618956327722878472004540065631638033121477000705571056940596828925718926195830996924121418082037133433987071497831290651435693069630617850623228685502937852675280449423766186849426063726462699651472782151270116860694640035896326187491580654641826172411776043924735160566067399148505252115983531709173811670864367402770737994364134054792446295788679175027471719898343002506661556535699568207586460454047069502939531995819982412227908066828322909893757005530027454140572188979488150548883493656102634585283004388269040778035648844374061133441256485694130373153695441673018808503905692775382098592090808337455691437278762153452127026625199516921413226458123714150014291085888648479717581069331505291998640223669211538320227057005969174493070497899969544813875034244416118133376225061639085905224935975593017160559344785335531808006629894396222402748085358182197175776506465368280926618778577016465379764389402423788365712927902557021317967548482940328717296549354184303249524202412120909360058136253604395042854633260658145671789247912207096954542398140903123427771094175794366551714347951366259653252568398361177136455047293213701876695");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(5ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpNineHugeNumberxModHugeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber my = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp = ModNumber.Stomn("6526644289055956736858877068519403631010685992730245336265829496694821013414538499486146315974113032228573239209724596217544324651093152616548529250049120999940655678333315395885001604404430674368413549625865049250132470591380208475377609647874790029400479150205957827909009703086882670985349192731759896918001420494741246424227032373058800952891431686568886131564077128377252007511518433882952722716274723386681179475425448262171359024756092838125772314791606694286983792124323139563528579059823470875200334900733517999777734332795930037234961019202348402151535637919629289729883257595638888662496455827375541928600802304845756950166077359813701732089741989302650908674314893619941837738332749876333210736274793708466037828681236498813341528164998456696630564895982403091586560317432213911618061336210329685952960886797529954329889775089979546107202598911341674985812880959595722541767420107480885582726866206084550433759213495839929831622331550209479075977807000372939391980195376179776796433632330315367306866629459555355570369718663720700763486289795413786830649415088063124797019327343416815040790000844014394638687874096152492020345144138425194906563561203752366115273693820073660712451776789995538713644424100");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(9ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpFourteenHugeNumberxModHugeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber my = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp = ModNumber.Stomn("6039455007496128416719267954008821642644933296482285601007280340638849216598327471873836884804919649701022781562170836586122709784265816820825575096191834212185018926216348978275581175822063777722482497113140804514895036442142544430881667557825900078592252798965327258914990338583787210599073287488662673805301914377003451438375705278297648482094110689330320376315698672834604231446800836077962070330168937134865160500778390669728454718569045019256569641694928118011409355278801328133547165356294609337610812695213078600632123496284216488466986412259260415850761842086790286642444976955387290025589459443340299552333990449300537311605581899291492541042318438402365969580472495963876590196668452923761989123797416243373802832579730850755435214147890009913750329406321245312028874106538784143568655391804665025694862487948422738665059792179032606285837274072496216497163641530842989068220287114503203934200124660090185998949932049482606796394057023799906848265652644062953681718535513811220343255677529230782090689257816403377077217366329694164624949416603154344694634920762252017370512301667484263576212344489948933525466419603573310889134534166584747655988525572669221990755765536427726512600894509425046404685796958");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(14ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpTwentyThreeHugeNumberxModHugeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber my = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp = ModNumber.Stomn("1910558067664321416396680278112421351311128155508056072368901508695988642856892709484053518717865300498833651493446191878311109463208734254327768677827631662480165138464607173165845236905892018996909578572567001105120710562754908421918976912431771111407349953372447903465407154463492560142565541044022453166310527846151093678811771978142693883611628484850315390633830071712500390333553961144075202278876666115263024160232594658499786859669890239340154655186150173362284474209855367200301366985476358028405918824749647709592321300691313415640628857337008354181546978239419012926142357108578893452157019470600385662609894172793751782975445692878550393789212014876536630398912024388301225226840243278763130519406654670836585560317047461520560082897582974145444240845524666069689002899325216988866056408910609616722220391670072906927908320209990818476307264086217589874274441162728455864826783695133239286440580764813761080633506062001385591618688515524216363685876312727637894283515606949804806273854084400015421161488551836257404747281713334156197917858069802946835981863333047175489480127714118264832628367050972187495226968652928612714283155344928655133603656002771958262019707727171589382556221257208131632394242127");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(23ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestExpThirtySevenHugeNumberxModHugeNumbery()
        {
            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber my = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp = ModNumber.Stomn("242760452789498187560222116076891445988028458456992017072504963165600161058896002310018225049894657031384427443541488709065472392086152010601151272712242277440209738146286759300402140345293651269163161826881724438362631376103663291045572406831762311376767187296205676842143574047066926958813256927930059355363901562101377907285379657399980663671306578018989929389619907873845508787293871920494666862217995428202920280527033368535241047172354652925147293331298874308471570694445084800591737290938767976686091604826314011132217253235234948408366413966084924212867402650142726851051817862713774158899264782638102048541460003811706714953941909081746204720033432076260814059663672110883330201609933877960327204186047392396598121000669809974553616720356084495585767909271572461836652890116273387079425214687517318304739386849804331869959580502653626045661944656270423012246002104141776443539286778518133774966685946958812610957974544771791195158152619653737875223424316716008726667030501736269703567752542972625389592056282107982296394621743371100502549330863244492025351194876359303058058710155058508915698932387363209783346367328313138661688531785868590170495058360789323129007712138201241342346426980353430114603198996");
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(37ul);
            ModNumber mres = mgm.Exp(mx, me);
            Assert.IsTrue(mexp == mres);
        }

#endif
        [TestMethod]
        public void TestDiffLGreaterRLessMod()
        {
            ModNumber ml = new ModNumber(100ul);
            ModNumber mr = new ModNumber(90ul);
            ModNumber mn = new ModNumber(110ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(10ul);
            ModNumber mres = mgm.Diff(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDiffLGreaterRGreaterMod()
        {
            ModNumber ml = new ModNumber(100ul);
            ModNumber mr = new ModNumber(90ul);
            ModNumber mn = new ModNumber(60ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(10ul);
            ModNumber mres = mgm.Diff(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDiffLlessRLessMod()
        {
            ModNumber ml = new ModNumber(90ul);
            ModNumber mr = new ModNumber(100ul);
            ModNumber mn = new ModNumber(110ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(100ul);
            ModNumber mres = mgm.Diff(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestDiffLlessRGreaterMod()
        {
            ModNumber ml = new ModNumber(90ul);
            ModNumber mr = new ModNumber(100ul);
            ModNumber mn = new ModNumber(60ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(50ul);
            ModNumber mres = mgm.Diff(ml, mr);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestInverseOfZero()
        {

            ModNumber mzero = new ModNumber(0ul);
            ModNumber mtwo = new ModNumber(2ul);
            MultGroupMod mgm = new MultGroupMod(mtwo);
            ModNumber mres;
            Assert.ThrowsException<ArgumentException>(() => mres = mgm.Inverse(mzero));
        }
        [TestMethod]
        public void TestInverseThreeAndSevenModTwenty()
        {

            ModNumber mx = new ModNumber(3ul);
            ModNumber mn = new ModNumber(20ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(7ul);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestInverseSevenAndThreeModTwenty()
        {

            ModNumber mx = new ModNumber(7ul);
            ModNumber mn = new ModNumber(20ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(3ul);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestInverseTwoAndSixModEleven ()
        {

            ModNumber mx = new ModNumber(2ul);
            ModNumber mn = new ModNumber(11ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(6ul);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestInverseFiveAndFiveModTwelve()
        {

            ModNumber mx = new ModNumber(5ul);
            ModNumber mn = new ModNumber(12ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(5ul);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestInverseSevenAndSevenModTwelve()
        {

            ModNumber mx = new ModNumber(7ul);
            ModNumber mn = new ModNumber(12ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(7ul);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestInverseElevenAndElevenModTwelve()
        {

            ModNumber mx = new ModNumber(11ul);
            ModNumber mn = new ModNumber(12ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mexp = new ModNumber(11ul);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestInverseOfTwentyAndFifteen()
        {

            ModNumber mx = new ModNumber(15ul);
            ModNumber mn = new ModNumber(20ul);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres;
            Assert.ThrowsException<ArgumentException>(() => mres = mgm.Inverse(mx));
        }
        [TestMethod]
        public void TestInverseOfLargeNumber1()
        {

            ModNumber mx = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            ModNumber mn = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp1 = ModNumber.Stomn("990338159586064333264351853726399202032439273344500090039361795769103042626153");
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp1 == mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1ul);
            Assert.IsTrue(mexp2 == product);
        }
        [TestMethod]
        public void TestInverseOfLargeNumber1Inverse()
        {

            ModNumber mx = ModNumber.Stomn("990338159586064333264351853726399202032439273344500090039361795769103042626153");
            ModNumber mn = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp1 = ModNumber.Stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867");
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp1 == mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1ul);
            Assert.IsTrue(mexp2 == product);
        }
        [TestMethod]
        public void TestInverseOfLargeNumber2()
        {

            ModNumber mx = ModNumber.Stomn("113638967017082606674052656070205979468957500859029166099119331283109255964689");
            ModNumber mn = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp1 = ModNumber.Stomn("916563989161488767790915831324982323599332376517448274768723301425613841973609");
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp1 == mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1ul);
            Assert.IsTrue(mexp2 == product);
        }
        [TestMethod]
        public void TestInverseOfLargeNumber2Inverse()
        {

            ModNumber mx = ModNumber.Stomn("916563989161488767790915831324982323599332376517448274768723301425613841973609");
            ModNumber mn = ModNumber.Stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825");
            ModNumber mexp1 = ModNumber.Stomn("113638967017082606674052656070205979468957500859029166099119331283109255964689");
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp1 == mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1ul);
            Assert.IsTrue(mexp2 == product);
        }
#if LARGEMOD
        [TestMethod]
        public void TestInverseOfHugeNumber1()
        {

            ModNumber mx = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            ModNumber mn = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp1 = ModNumber.Stomn("7077938970216339316645445575285346683424941653237210340139516526256813509979606177021530849277742732364801030056254685283272967813034126654684370996868817665975637877200499989860765830543219806691253546378869794068270590583892833700153879570795112599739140513412553864621285534205670943364750922097762621895818680253818070884230571117040150011896031221708030875177847284891792635438074811694778675171964272643019638156031482913499567423251969964170693617285474555961748092945378333120890780361163560901493876218729987658462900795199003929229716100635333728591704523260412802193993472731422243051758869939898387278562993320264471554982496798519107136733940096336315302043051078369874122713882086967410973438379654109746059534714870547090057664281203709444590153178636867403107145141196420110304550068844058322769189735671101637680650707903615234635200908408342995823505703193770467699263824297761978530688995807637960633835113674050660862505648679547630512260845352364885624977257360366150205959045524481094456933930287176995345532173558521752894170223063256872758780959875487778634305779963228215875539131006201580009220506095752025694701229168899054377957651680451709904249939778196108148618292542907928847184822275");
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp1 == mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1ul);
            Assert.IsTrue(mexp2 == product);
        }
        [TestMethod]
        public void TestInverseOfHugeNumber1Inverse()
        {

            ModNumber mx = ModNumber.Stomn("7077938970216339316645445575285346683424941653237210340139516526256813509979606177021530849277742732364801030056254685283272967813034126654684370996868817665975637877200499989860765830543219806691253546378869794068270590583892833700153879570795112599739140513412553864621285534205670943364750922097762621895818680253818070884230571117040150011896031221708030875177847284891792635438074811694778675171964272643019638156031482913499567423251969964170693617285474555961748092945378333120890780361163560901493876218729987658462900795199003929229716100635333728591704523260412802193993472731422243051758869939898387278562993320264471554982496798519107136733940096336315302043051078369874122713882086967410973438379654109746059534714870547090057664281203709444590153178636867403107145141196420110304550068844058322769189735671101637680650707903615234635200908408342995823505703193770467699263824297761978530688995807637960633835113674050660862505648679547630512260845352364885624977257360366150205959045524481094456933930287176995345532173558521752894170223063256872758780959875487778634305779963228215875539131006201580009220506095752025694701229168899054377957651680451709904249939778196108148618292542907928847184822275");
            ModNumber mn = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp1 = ModNumber.Stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105");
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp1 == mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1ul);
            Assert.IsTrue(mexp2 == product);
        }
        [TestMethod]
        public void TestInverseOfHugeNumber2()
        {

            ModNumber mx = ModNumber.Stomn("5603130552131808582980129320151431593287387887422304019872729976542631800068788204319579898052711893227272328490995919630013482288843181272802592367798898524753249376917995287069518581823351056941728232218068059475906712986411367854982914146900624999167769127442072642482993253159299132482762489032243580550536702583738240633476465093340828218312246935203167115248772557253480443158870340161116689390922962852167364803089439715091390938186175699671994828460433891219975915334762748122803062377109012273998249380330075478906670900287087012812096397970441489185240044951680317263972659517703324836533035198756630319129921923499685884740053891793070603801214192199848233151436265254188036888028127957406260369087189527978590594475592951159744836132820316608295381151982751459808466074838264192706066312469551538268619096100114127505148129379927320373232121684138190307396104482210039324618282627066824207318147380670441172882452441196009256198174338127429977948883866986080614196689277860961306323052769429682104967016727143083349244333982457816038315667725514706061625905397835791561560354555405798114663550095985095263976338037301461279067122567382168347234118084620013800495920079606189028067751569874497346329975073");
            ModNumber mn = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp1 = ModNumber.Stomn("6739138816136043312649272213174713769262983880649218872965179549677556315677506972615490310113406336693569234718740510119663919502539189540509251055798460074325514337939662564750863101055411914158661300731270123866093947731042804706074093172689096545237738202130653344811784515331242745926904461789015551948920613465874894376131240703947702355076603139914962929019780686185414664545329663218317295135211022489224303293900015601419851026256029933643673808719974409805841004170959181758705284178201118626199154465204469896041437537889130973291884948000755501999889301784901971136592495490062001754886483556366544831945270673733954935411162006328088367540078380415454971364899079826871715550246362070409406505441342293480933173058131951954050439775679821566910118564679299097353256497284984326254753897393933507219970088105495830123020354077871747461442531592169831588898411349425799974571880327159780649631710046012433252304811853839406510704163016067299984268200721162895812075320190090687162866263831468978576430892453490199685391372592054813829543523397778555188730184762400193992336282063202658915226866054763855569295020560910720893342762594098082777028627182850722798218135965067629433110731488659794290674761733");
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp1 == mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1ul);
            Assert.IsTrue(mexp2 == product);
        }
        [TestMethod]
        public void TestInverseOfHugeNumber2Inverse()
        {

            ModNumber mx = ModNumber.Stomn("6739138816136043312649272213174713769262983880649218872965179549677556315677506972615490310113406336693569234718740510119663919502539189540509251055798460074325514337939662564750863101055411914158661300731270123866093947731042804706074093172689096545237738202130653344811784515331242745926904461789015551948920613465874894376131240703947702355076603139914962929019780686185414664545329663218317295135211022489224303293900015601419851026256029933643673808719974409805841004170959181758705284178201118626199154465204469896041437537889130973291884948000755501999889301784901971136592495490062001754886483556366544831945270673733954935411162006328088367540078380415454971364899079826871715550246362070409406505441342293480933173058131951954050439775679821566910118564679299097353256497284984326254753897393933507219970088105495830123020354077871747461442531592169831588898411349425799974571880327159780649631710046012433252304811853839406510704163016067299984268200721162895812075320190090687162866263831468978576430892453490199685391372592054813829543523397778555188730184762400193992336282063202658915226866054763855569295020560910720893342762594098082777028627182850722798218135965067629433110731488659794290674761733");
            ModNumber mn = ModNumber.Stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281");
            ModNumber mexp1 = ModNumber.Stomn("5603130552131808582980129320151431593287387887422304019872729976542631800068788204319579898052711893227272328490995919630013482288843181272802592367798898524753249376917995287069518581823351056941728232218068059475906712986411367854982914146900624999167769127442072642482993253159299132482762489032243580550536702583738240633476465093340828218312246935203167115248772557253480443158870340161116689390922962852167364803089439715091390938186175699671994828460433891219975915334762748122803062377109012273998249380330075478906670900287087012812096397970441489185240044951680317263972659517703324836533035198756630319129921923499685884740053891793070603801214192199848233151436265254188036888028127957406260369087189527978590594475592951159744836132820316608295381151982751459808466074838264192706066312469551538268619096100114127505148129379927320373232121684138190307396104482210039324618282627066824207318147380670441172882452441196009256198174338127429977948883866986080614196689277860961306323052769429682104967016727143083349244333982457816038315667725514706061625905397835791561560354555405798114663550095985095263976338037301461279067122567382168347234118084620013800495920079606189028067751569874497346329975073");
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            Assert.IsTrue(mexp1 == mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1ul);
            Assert.IsTrue(mexp2 == product);
        }

#endif
        [TestMethod]
        public void TestMultGroupModAddAllFsFirstBlockBothLessMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            r[0] = ~0ul;
            n[1] = 0x10ul;
            exp[0] = ~0ul << 1;
            exp[1] = 0x1ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Add(ml, mr);
            Assert.IsTrue(mexp == mres);

        }
        [TestMethod]
        public void TestMultGroupModAddAllFsFirstBlockBothGreaterMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            l[0] = ~0ul;
            r[0] = ~0ul;
            n[1] = 0x01ul;
            exp[0] = ~0ul << 1;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Add(ml, mr);
            Assert.IsTrue(mexp == mres);

        }
        [TestMethod]
        public void TestMultGroupModAddAllFsBothGreaterMod()
        {
            ulong[] l = new ulong[ModNumber.LCOUNT];
            ulong[] r = new ulong[ModNumber.LCOUNT];
            ulong[] n = new ulong[ModNumber.LCOUNT];
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                l[i] = ~0ul;
                r[i] = ~0ul;
                n[i] = ~0ul;
            }
            exp[0] = ~0ul << 1;
            exp[1] = 1ul;
            n[ModNumber.LCOUNT - 1] = 0ul;
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mn = new ModNumber(n);
            ModNumber mexp = new ModNumber(exp);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Add(ml, mr);
            Assert.IsTrue(mexp == mres);

        }

        [TestMethod]
        public void TestCalculateRSAKeyGivenTwoPrimesAndChosenExponent()
        {
#if LARGEMOD
            ModNumber mExponent = ModNumber.Stomn("010001", 16);
            ModNumber mModulusExp = ModNumber.Stomn("A26960E1102074E8E1E7D0754F6BED06BC5AAFEEA086704ACB33B99289FBC180874418D11E02B1B250313EE873E7307D932DA7C30A5E703CBE86EC588B318B46C20BD8BB01F51549635641E8958B0A109E59B06752ABED027539FBEA8BB700E58AD1985CE405F57BD01F4FB91E88BC8B50C4F0208EA02BB72612F41DE302DB3C7BFF5EB12C277417068CE2CA387BEBDE35320D045438951AD51D34CE1F52B10E1F7A9023E9C647582D266CEF6D2E067B60C1BED9C781ADC520223091210854A356C6CAE5B1874FDC59A643441C228F30E4329345AD96A2605972524BFEE8764D207435E42001C678028C48167610A6FCDDF43BC789DD04A6AC3B337E4F6D8085A404E759936355BCA8E7ED93AB6BB031830CEB586D1CC5A90E87F497B8096AC6ABE37BC701E33C9D9B61FBC8AF73446AAC90301ACDCE9E0DEBD322A032F1C90CA8D766285723B5DE918E65F1A987B32E2D51DB7E2C8EB8F2A28C7292E14BD56FB0EC779F44C138C9DF08BEDBD0568DC8FB9F65B7DD20CE80652C9194E160DCAA6C5F96BB5D357BE742B971B3F73923447C36C906E12147C0153CFB2551AB581513B38F2D3B90F2F3FDD17CC799224AA7DDBCAF41309008637CBAD18C27E226E849532B3CEE58CF3C9B3E147438935BCECB35C47B50E29623F76687A51792B75E077BC3D1240C73F6AB79B33577014C7BE2738BBE1F8B179517C5B5BEC23D7C9D", 16);
            ModNumber mPrime1 = ModNumber.Stomn("CDA901CDEFC7392972350E5160DCE36D9E3415ABA270EFA599B72EA3A36E4BECF727A46DDE4B39181D2A1B76CA2A79DBFCAEE2DF3D46648BB6F5766DE8FBDB9B97915A5D6BC9E3C2AB30DF0251B7DF103CB8BB9C8873188E395C01CD8C374D72B6B65FBFF47331232D6F86E3EC22B3F866D35C74DF6D76D35836363D020571C86104307324EE0A704FF5BB77DBBCDC7A2D98BBDB0C65B60F22F4EFD5EE8555715F2C79CCC438484C4E1B2136E881C2B04AB27E015AD354AECDF0D717A78205CA7DF0353F2312FD54EF8FB51DA12E35115AFDFA0659C4386DF841C0FF51BB5EC37D555CEDF88FC783FB568FA6F5736060837B4020CACD9D9BAC5F75348673EDCB", 16);
            ModNumber mPrime2 = ModNumber.Stomn("CA2A5948D61356DEDDCF8F5D180855C87E6981FDE13892F7CF593547B36D1CD5682699FD73D1036A24893EB2B8BCDBA8FC256F65E6644E2FC1E80C1BCADA0E21F6DED17EE6EA989BEB5E912E7FDB448573EC376817C4741C33EF78DCCE9DDE4F13FF6F29FB1117FF0873E5EB0B91AD52E41E2148DED2820C840574608F78E6D0E17DAF74F2B0A6171503EDA87D4B713819F86F07DE94CFFFBC2E8799DF78F138E28205AB7AF0286059980C241CD74C48FE897368E4CE6A096C4677BB2A37CF494395A1C3F695201863AFA632D2BBD61755779C20DD87AD78EE5E8E3EDA91D52FE2D0B91971197AA215EEE3E1F00DE0ED93B42E386C7685B71C1A25BDA0337237", 16);
            ModNumber mExp1Exp = ModNumber.Stomn("92F32C22E62DC81E61489FCE6E13E73DB0492F22D09339E96E0881FED30C409C358DD1E3DF6E88813E7593E2315A60705C00EA82F0D997866D58BF2291D3447BCCB54388753870D1EC58A34098FB751BB90796A6A6A8F6E6C8FD811C4A1049FAA9AB6E874E37F0E85DD2CEE3951AC6D5BB435350FF44CC1EFF6CDD1C82913958205A49F43C730A6A6DF7FBD80CDEC9FB53114B499D761FBC7911E33D4D8D601BFE115D867EBE688A7D54CD4ADA9FBD691236AFEA6FE7891ECE704480D6640597469D98AE6075752677768BC77C1FF4334E7D2365DE4662BBD123CAAFA4CAE94F6E02CCC00D574F0C734ECA9E55CBF7157F5D55D918900B09ED04D84E8BB3D239", 16);
            ModNumber mExp2Exp = ModNumber.Stomn("76DEF5C6CD116C07F9769CFE892E057AB151CA7C845C66964D964FACCC44C12A7E660C2FAA54E0E41D7D919F36DEDD6DFC6CEA968924BB907B3CBD39EE49CEFB8B9432ACA28C98953D8D4A62840D62B9F19298759D5C0745BA07FA12236F52133F57F77E40C12951ED1F70B68703EE5D0A02E961569B829A675A69B68945F6BA258C6889A455D82344591DF9EF4A57D53BF8071922786FE1183E67BF435672C816EF8AF3EC829FCB1F8336C859290312FC8FC78B8AD610256044030CCB632B8011BC3EF4E1DD8794AFB0CD519EF5918A00D826B6706BE1C574ABA68A30AC8524F601F284922B001FA433E64CA881109BC138B5E0B96DEB6BE29B414BC191DE7B", 16);
            ModNumber mCoefficientExp = ModNumber.Stomn("80B8FE74BA8F322BB18AD25B5EA75E1B50F54521E648960084FA90895634016A7C3D0997C52A1BC766C58BFFE8CD8953146A188D29480311E624048F83ADAEE157EFC4AE9EFFB144BA8F41E2CACDACD50A51367BB9D013E29B6C859AFC030CAE31B38740F287D61F4CDEB78659E91D404FD6976FD7F5E34C1B7FF530B655AA36161C928BD128D56A6FC1469CF8DB7D308CAFAE0AFDA03A71A399C9EC05889427AAE189EA9DD2BB4A4132FB16622FC9394759F337C8AAD5AF6E3BF659D0624EB10238BA58BDAF858ACF954FBFF03AD12FC0F55C79EDC318FC96D6BC53DEB0688E2C1B80572EC942E37848C75BDAEE6366FA5C11396F8E18DD6AB0F61AD71265D4", 16);
            ModNumber mPrivExpExp = ModNumber.Stomn("6DD1E9C365DF3C8E78AB02A740640F2B7DD3A8A12804A00BC3299FB2FA00B5508EA6BA4D3508C75576AA460A72CD0259F75A68D516BED310101935D98D8D9BCAE5CD92E8A9232B18ABC618DD07B9B3BD9705B8A5B51E322E764C160EFC186BCFCB357CDC5B07B87DD73644F73D3162A4A5A1F5695E67EA0777045BF3959C49D8B553F198C5ED8BAF422BA3F2EF8CC3EA4678289B23C3FAF6E0E255E18706A8588F6A95EAE2D9C34659896121E8E6B5555A10C857847328A861D91C94160D070647D8363C68F5C147938B3C72E2BF6FA9B4B48A271481061D367E7A2D8F196B5D2DB3C6F911B7819786C98AB93D6E772C077D6DAAB88B2DE8F20F39FB9003D061F9C8CB3A24B6F650C3C6393117201B765546B40453313045C0C67F9A404016F7073DE6F6877E7D9100EC77B7252D1E064EA99B102E73095D80049D127932C4A63D894DBF37446A5222CBA22B9B1B70B9C09C3EBA69EE4C302B6A59F7EE6C70C1F9283E9E171C14457FB78FAAC615AB15F691318A4CB4E55F1F1A1E635EAE217565A4D94A3505095232A4CB211E579122C2DCB67944A9E2514CEEA5C3E85CEFA916887B9D7B0BC4042EB4622A5E283566105E997FACFCC3EDBD152620E2A472E8CE47464BB0892642AE410CB9A5F283693E2F736444C54338978E21775E616BC2839208E89E5312CD26686C21E964F6AA29874E84FC1776751C1FAF0369B57075", 16);
            ModNumber mModulusProduct = mPrime1 * mPrime2;
            ModNumber mPminus1 = mPrime1 - new ModNumber(1ul);
            ModNumber mQminus1 = mPrime2 - new ModNumber(1ul);
            ModNumber mPhiPQ = mPminus1 * mQminus1;
            MultGroupMod mgmPhiPQ = new MultGroupMod(mPhiPQ);
            ModNumber mPrivExpCalc = mgmPhiPQ.Inverse(mExponent);
            ModNumber mDpCalc = mPrivExpCalc % mPminus1;
            ModNumber mDqCalc = mPrivExpCalc % mQminus1;
            MultGroupMod mgmP = new MultGroupMod(mPrime1);
            ModNumber mInverseQCalc = mgmP.Inverse(mPrime2);
            Assert.IsTrue(mModulusExp == mModulusProduct);
            Assert.IsTrue(mPrivExpExp == mPrivExpCalc);
            Assert.IsTrue(mExp1Exp == mDpCalc);
            Assert.IsTrue(mExp2Exp == mDqCalc);
            Assert.IsTrue(mCoefficientExp == mInverseQCalc);
#elif MEDMOD
            ModNumber mExponent = ModNumber.Stomn("010001", 16);
            ModNumber mModulusExp = ModNumber.Stomn("CA75BD1F951E545F8BA1BF6C985398C48BE5CD45E1794AD7D151CF8871D4088C32B1E88D8E4A9106714EACD38C284D70A1E15EBCC289BED8EE90C771B7A45A86DA92C7D5D4936782A701CCC6C7BF75A9CF22E2C079B2E94A4C7ED28B2983AD71B8D24D4E43501CA1FE0C15204A311BD4EEFC9B62A9D1078D27CC5C213A4D328002100FB350D8749DCCC982D736F6ED95B70B146152C966661FF48FFDF8CB8180E0A49D0D62EDED2FBE40D8C639B4F3817EDE2D7D5D9AAE4E0ECE362598DA62613E9B7B9BF7D93F6C232AE9BFC34142A65774DB2241D8B810734FED65C97370FC84599E002680901B55D0E991E51C6C4856FC361FD526193EB972E2C29467F211", 16);
            ModNumber mPrime1 = ModNumber.Stomn("E0162AC55685C9903231BE3A9A65D9C5E28F7D12683FD835B240DB86A0E3077ADA211CA866C00748430835882D586F44B0FF5784B4980C0EC73C52C7A195D2C514DC5A59E8683F6C0C4B614FD63F510505429928679F7807D7689E48966FBC29728CE81ACE99CDD69F48B0BE47E22AD0A65451FD80DE1E7BD93FF7755EA0FEFB", 16);
            ModNumber mPrime2 = ModNumber.Stomn("E74B192C59634F62C4E14DF1CF88B26163567EAB1A8DF1B4002CA14DAAEEF3F99DD2A8493DBA4BA79055300D1CA685A9964D69E3F5CEAB0CF728914B66DCDB41063170DDC0254088C33134EE4ACAF86AA4F50458804FC55F306654E221A33566FE9F2ECD22B72527DD402AC3A5FE715EBA7C4FB23998C3E59C2A35F08DB45563", 16);
            ModNumber mExp1Exp = ModNumber.Stomn("4B3AE5ED0E59280C42EDB4419468312E601B700FE7F3D7305FC1AA718E3DE9DAF2B43F1CC23221BFBED158B77911C1793C3ADD34EE344588089755EB33724B2A63107F3CC3EB8BAF48895E0821936C2DD550407EF21AF57F1C704103798DD58BCCAB45990A575465836032060FB7FC4BC42B76C336AA5B6D94C806D9DE850A55", 16);
            ModNumber mExp2Exp = ModNumber.Stomn("AA0EE371623FCC8CB86957DFE55182A66C5D9A40C00381A571435F4E35D1FF7C96F7DE580B94C02F71C5D0D0B33214B0BE1C29313CF839780CF9BCEA8DB4F1587BEF3A273131B2A35BB10053FC9603D49578B93B410C65F83923E461678E12A3A9AEECC05338098FA7F397FDC63CB0AA8FBEFAF1F5BB46BDB9F6A736395A5035", 16);
            ModNumber mCoefficientExp = ModNumber.Stomn("6F2C5D2D9A587AA5657032960ABBB1246A0D7B51196625693284F6B8B8222C5B2E8BE3139FEA29D20CC70FD6C8740CA5DE0A94C5ED1D56E6CCE8BC67AAE9153C57A02EEBDC5764B11608AED772B661243E265FBA989B74DB2B9DAC0CEC7AE9C322232C56AFD4BDD8746C8095BBCFDF04C553219092C8E243C94E84757FDA30AC", 16);
            ModNumber mPrivExpExp = ModNumber.Stomn("0B5BF1C17CDF57C2889609A471237795C7779C3DC879C2AE3B2B48E691E4609A6519C03F78F54C0FBD79B98EA0F57DEACCDA55336802CC776563301D7C3D376B0B8662B9E79461A15DF766E2DC402CBCDB474CCB99C864F86B344E61CA717DE0BC8AE815750760A3E21A11715F5844047D6824DBB3456C140A6097E6B15BB572899E5706451E48D95BE641E03162BFFCAF5272807661DA94CC9C6081DD179EE2C158F2B713C76EA71969AB83DA5F2EAA59DAC9E29B6F55F637B6C109139B0DEE731F36FF59C799357BB7732DF9C8F5D09085B8187E6FF016E3563833EA34ACD1867DEE6F7844749F32BDAD344E1DC7FE2E89A1696CC5D9E12E3902DBF0FE2265", 16);
            ModNumber mModulusProduct = mPrime1 * mPrime2;
            ModNumber mPminus1 = mPrime1 - new ModNumber(1ul);
            ModNumber mQminus1 = mPrime2 - new ModNumber(1ul);
            ModNumber mPhiPQ = mPminus1 * mQminus1;
            MultGroupMod mgmPhiPQ = new MultGroupMod(mPhiPQ);
            ModNumber mPrivExpCalc = mgmPhiPQ.Inverse(mExponent);
            ModNumber mDpCalc = mPrivExpCalc % mPminus1;
            ModNumber mDqCalc = mPrivExpCalc % mQminus1;
            MultGroupMod mgmP = new MultGroupMod(mPrime1);
            ModNumber mInverseQCalc = mgmP.Inverse(mPrime2);
            Assert.IsTrue(mModulusExp == mModulusProduct);
            Assert.IsTrue(mPrivExpExp == mPrivExpCalc);
            Assert.IsTrue(mExp1Exp == mDpCalc);
            Assert.IsTrue(mExp2Exp == mDqCalc);
            Assert.IsTrue(mCoefficientExp == mInverseQCalc);
#elif SMALLMOD
            ModNumber mExponent = ModNumber.Stomn("010001", 16);
            ModNumber mModulusExp = ModNumber.Stomn("B08B5EF115AED8F91EE2FB82E7E8C8611DF101B4138B37787A18D1E72FC75437342A8961F7C462CBD2208DE6EC0472882CE6410BCE3AE096BB2CA0506B5FBF5865FC80465653F2AEA678C8A8638AF8CF98F105166B6CFA71A61567765DF9521FAE5616DBBF26F99512EEBF857C166941FC2402C5E27258B10206A8AF016C45E9", 16);
            ModNumber mPrime1 = ModNumber.Stomn("C882E9B30B9F87F47AD7653886EF5CD53D9CAA96D2B83C24C643F024B86CDFE61DE6627F0C63B3CBB885DD3212C77CBE47A3469E5EA2FAD245984A75D866AAF3", 16);
            ModNumber mPrime2 = ModNumber.Stomn("E1668918F4DD83B3066B32577933BD27B1FFA8F49907637863750304EFCBDB1DCC86362EEBC81ACD68B7E0C0BCE35B1BC0F353F16B59C5E7636AF1464805CAB3", 16);
            ModNumber mExp1Exp = ModNumber.Stomn("A8130C8077D2BEECD0F7B2B242716C5896EFB712FC0950E8E684D64C818749DB23DF155B37F06F2AEA3ED39E809D55544ECA6F8C9AF974D4436017B256B620BF", 16);
            ModNumber mExp2Exp = ModNumber.Stomn("74EDCA456611DC166D4802EF99F29B381280FC571763DBD842501B66DE3734CE7AA14D4E7E066AD110780362391B00D29F001C15EB88C0BFCEB555C10DEFE33D", 16);
            ModNumber mCoefficientExp = ModNumber.Stomn("89C0F663B05BC7C1CE547866B45F137DC73739DF9C159B5BA648A5C7818BC46A9CD12C41922A82DB150F9D34F08D69B4A94A947848E88DC5735800C882C856A1", 16);
            ModNumber mPrivExpExp = ModNumber.Stomn("9E7C2F39FBFE1FD7DC2B662009328717EFFA184E61311C15F27DEF893BF2141F0E9C9502369BBD193E446D3EFD67ACADA4A8FB81AE9C5A5BD621E4B4ECFF625469B82CE442C50E56F2C7E860FD7414AB46C9BA2C8F043FC2FAF5408E50A758BFDB2AF454020A1E77586C4F7E2D7CCF66E354715606B5223C31538AEDAAA0DB85", 16);
            ModNumber mModulusProduct = mPrime1 * mPrime2;
            ModNumber mPminus1 = mPrime1 - new ModNumber(1ul);
            ModNumber mQminus1 = mPrime2 - new ModNumber(1ul);
            ModNumber mPhiPQ = mPminus1 * mQminus1;
            MultGroupMod mgmPhiPQ = new MultGroupMod(mPhiPQ);
            ModNumber mPrivExpCalc = mgmPhiPQ.Inverse(mExponent);
            ModNumber mDpCalc = mPrivExpCalc % mPminus1;
            ModNumber mDqCalc = mPrivExpCalc % mQminus1;
            MultGroupMod mgmP = new MultGroupMod(mPrime1);
            ModNumber mInverseQCalc = mgmP.Inverse(mPrime2);
            Assert.IsTrue(mModulusExp == mModulusProduct);
            Assert.IsTrue(mPrivExpExp == mPrivExpCalc);
            Assert.IsTrue(mExp1Exp == mDpCalc);
            Assert.IsTrue(mExp2Exp == mDqCalc);
            Assert.IsTrue(mCoefficientExp == mInverseQCalc);

#endif
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageTooLong()
        {
            ModNumber message = ModNumber.Stomn("FFFFFFFFFFFF", 16);
            Assert.ThrowsException<ArgumentException>(() => message.GetPKCS1Mask(false, 16));
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageEmptyModulus26Fs()
        {
            ModNumber message = new ModNumber(0ul);
            ModNumber mres = message.GetPKCS1Mask(false, 13);
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.HexStringLength - 26), new string('0', ModNumber.HexStringLength - 26)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 26, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 2, 2), "00") == 0);
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageFourFsModulus26Fs()
        {
            ModNumber message = ModNumber.Stomn("FFFF", 16);
            ModNumber mres = message.GetPKCS1Mask(false, 13);
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.HexStringLength - 26), new string('0', ModNumber.HexStringLength - 26)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 26, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 6, 6), "00FFFF") == 0);
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageEightFsModulus30Fs()
        {
            ModNumber message = ModNumber.Stomn("FFFFFFFF", 16);
            ModNumber mres = message.GetPKCS1Mask(false, 15);
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.HexStringLength - 30), new string('0', ModNumber.HexStringLength - 30)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 30, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 10, 10), "00FFFFFFFF") == 0);
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageEightFsModulus32Fs()
        {
            ModNumber message = ModNumber.Stomn("FFFFFFFF", 16);
            ModNumber mres = message.GetPKCS1Mask(false, 16);
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.HexStringLength - 32), new string('0', ModNumber.HexStringLength - 32)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 32, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 10, 10), "00FFFFFFFF") == 0);
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageEightFsModulus34Fs()
        {
            ModNumber message = ModNumber.Stomn("FFFFFFFF", 16);
            ModNumber mres = message.GetPKCS1Mask(false, 17);
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.HexStringLength - 34), new string('0', ModNumber.HexStringLength - 34)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 34, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 10, 10), "00FFFFFFFF") == 0);
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageTenFsModulus36Fs()
        {
            ModNumber message = ModNumber.Stomn("FFFFFFFFFF", 16);
            ModNumber mres = message.GetPKCS1Mask(false, 18);
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.HexStringLength - 36), new string('0', ModNumber.HexStringLength - 36)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 36, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 12, 12), "00FFFFFFFFFF") == 0);
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageTwentyFsModulus72Fs()
        {
            ModNumber message = ModNumber.Stomn("FFFFFFFFFFFFFFFFFFFF", 16);
            ModNumber mres = message.GetPKCS1Mask(false, 36);
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.HexStringLength - 72), new string('0', ModNumber.HexStringLength - 72)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 72, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.HexStringLength - 22, 22), "00FFFFFFFFFFFFFFFFFFFF") == 0);
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageMaxFs()
        {
            ModNumber message = ModNumber.Stomn(new string('F',ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 22), 16);
            ModNumber mres = message.GetPKCS1Mask();
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.LSIZE * 2), new string('0', ModNumber.LSIZE * 2)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.LSIZE * 2, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.LSIZE * 2 + 4 + 16, 2), "00") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.LSIZE * 2 + 4 + 16 + 2, ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 22), new string('F', ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 22)) == 0);
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageMaxFsMinus2()
        {
            ModNumber message = ModNumber.Stomn(new string('F', ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 24), 16);
            ModNumber mres = message.GetPKCS1Mask();
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.LSIZE * 2), new string('0', ModNumber.LSIZE * 2)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.LSIZE * 2, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.LSIZE * 2 + 4 + 18, 2), "00") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.LSIZE * 2 + 4 + 18 + 2, ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 24), new string('F', ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 24)) == 0);
        }
        [TestMethod]
        public void TestGetPKCS1MaskMessageMaxFsMinus4()
        {
            ModNumber message = ModNumber.Stomn(new string('F', ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 26), 16);
            ModNumber mres = message.GetPKCS1Mask();
            string resStr = mres.ToString(16);
            Assert.IsTrue(string.Compare(resStr.Substring(0, ModNumber.LSIZE * 2), new string('0', ModNumber.LSIZE * 2)) == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.LSIZE * 2, 4), "0002") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.LSIZE * 2 + 4 + 20, 2), "00") == 0);
            Assert.IsTrue(string.Compare(resStr.Substring(ModNumber.LSIZE * 2 + 4 + 20 + 2, ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 26), new string('F', ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 26)) == 0);
        }
        [TestMethod]
        public void TestfromTextTextOneCharTooLong()
        {
            string s = new string('a', (ModNumber.LCOUNT * ModNumber.LSIZE)/sizeof(char) + 1);
            Assert.ThrowsException<ArgumentException>(() => ModNumber.fromText(s));
        }
        [TestMethod]
        public void TestfromTextTextEightCharsTooLong()
        {
            string s = new string('a', (ModNumber.LCOUNT+1)/sizeof(char) * ModNumber.LSIZE);
            Assert.ThrowsException<ArgumentException>(() => ModNumber.fromText(s));
        }
        [TestMethod]
        public void TestfromTextTextMaxSizeAllas()
        {
            string s = new string('\x6161', (ModNumber.LCOUNT * ModNumber.LSIZE) / sizeof(char));
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
                exp[i] = 0x6161616161616161ul;
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.fromText(s);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestfromTextTextMaxSizeMinusOneAllas()
        {
            string s = new string('\x6161', (ModNumber.LCOUNT * ModNumber.LSIZE - 1) / sizeof(char));
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
                exp[i] = 0x6161616161616161ul;
            exp[ModNumber.LCOUNT - 1] = 0x616161616161ul;
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.fromText(s);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestfromTextWholeAlphabet()
        {
            string s = "abcdefghijklmnopqrstuvwxyz";
            ulong[] exp = new ulong[ModNumber.LCOUNT];
            exp[0] = 0x0064006300620061ul;
            exp[1] = 0x0068006700660065ul;
            exp[2] = 0x006c006b006a0069ul;
            exp[3] = 0x0070006f006e006dul;
            exp[4] = 0x0074007300720071ul;
            exp[5] = 0x0078007700760075ul;
            exp[6] = 0x007a0079ul;
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = ModNumber.fromText(s);
            Assert.IsTrue(mexp == mres);
        }
        [TestMethod]
        public void TestgetTextTextEmpty()
        {
            ModNumber mzero = new ModNumber(0);
            string res = mzero.getText();
            string exp = "";
            Assert.IsTrue(exp == res);

        }
        [TestMethod]
        public void TestgetTextTextSingleChar()
        {
            ModNumber mn = new ModNumber(0x0061);
            string res = mn.getText();
            string exp = "a";
            Assert.IsTrue(exp == res);

        }
        [TestMethod]
        public void TestgetTextTextMaxSizeAllAs()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT; i++)
            {
                n[i] = 0x0061006100610061ul;
            }
            ModNumber mn = new ModNumber(n);

            string res = mn.getText();
            string exp = new string('a',ModNumber.LCOUNT * ModNumber.LSIZE / sizeof(char));
            Assert.IsTrue(exp == res);

        }
        [TestMethod]
        public void TestgetTextTextMaxSizeMinusOneAllAs()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
            {
                n[i] = 0x0061006100610061ul;
            }
            n[ModNumber.LCOUNT - 1] = 0x006100610061ul;
            ModNumber mn = new ModNumber(n);

            string res = mn.getText();
            string exp = new string('a', ModNumber.LCOUNT * ModNumber.LSIZE / sizeof(char) - 1);
            Assert.IsTrue(exp == res);

        }
        [TestMethod]
        public void TestgetTextWholeAlphabet()
        {
            ulong[] n = new ulong[ModNumber.LCOUNT];
            n[0] = 0x0064006300620061ul;
            n[1] = 0x0068006700660065ul;
            n[2] = 0x006c006b006a0069ul;
            n[3] = 0x0070006f006e006dul;
            n[4] = 0x0074007300720071ul;
            n[5] = 0x0078007700760075ul;
            n[6] = 0x007a0079ul;
            ModNumber mn = new ModNumber(n);
            string res = mn.getText();
            string exp = "abcdefghijklmnopqrstuvwxyz";
            Assert.IsTrue(exp == res);

        }
        [TestMethod]
        public void TestReadDSASignature()
        {
            ModNumber signature = ModNumber.Stomn("302C021427FBE13628A0AA7053E3C11CE6B4E7F40624C18F02146D9F22C0AA16841B26969166C692E92B41176232", 16);
            List<object> results = signature.ParseBERASNString();
            byte [] exp1 = { 0x27, 0xFB, 0xE1, 0x36, 0x28, 0xA0, 0xAA, 0x70, 0x53, 0xE3, 0xC1, 0x1C, 0xE6, 0xB4, 0xE7, 0xF4, 0x06, 0x24, 0xC1, 0x8F };
            byte [] exp2 = { 0x6D, 0x9F, 0x22, 0xC0, 0xAA, 0x16, 0x84, 0x1B, 0x26, 0x96, 0x91, 0x66, 0xC6, 0x92, 0xE9, 0x2B, 0x41, 0x17, 0x62, 0x32 };
            byte[] result1 = (byte [])results[0];
            byte[] result2 = (byte [])results[1];
            bool results1Equal = true;
            for (int i = 0; i < result1.Length; i++)
            {
                if (exp1[i] != result1[i])
                {
                    results1Equal = false;
                    break;
                }
            }
            bool results2Equal = true;
            for (int i = 0; i < result2.Length; i++)
            {
                if (exp2[i] != result2[i])
                {
                    results2Equal = false;
                    break;
                }
            }
            Assert.IsTrue(results1Equal);
            Assert.IsTrue(results2Equal);

        }
        [TestMethod]
        public void TestDSAParameters()
        {
#if LARGEMODSIGNATURE
            ModNumber P = ModNumber.Stomn("BCAE0FB273D98B3CDDDFBB1BE44470A592AAC3EA0885D06C272C5333D8C62AA7F8E5A09D56FF462BFF1032870B306F1A997CE635B920630B128C9030EE5506B7C19992EEBEDE39892D0107E0F6BBC417FE1EF3F1FE436A30E7FB1FD9043D4BAC3A240D6E287FE33090351746BC25639FAD56D3E2922D47092D0EF4AC7B0F0F85E747CF55A11E702276A6C40E285C3FBB61E50755EE3F27764B748623EC67D8E2D8B2E5A6142D244A5C4500C3A580A174FDC9D48BDC9CD93AEF8930563A28FBCBA5E52D8DDCCCE9FDB249E910594B02AB4A9283DEE01B42B405E8D71EC0B613ADCBB7095991256F6B38AE318AFEBB2432D708BCB913E6D67729E8B0F97B30CB30F5223980F792B9C8ECBDB19236FDEE155F1D8F4D0540B63EED872599DFF8B48A3A8F3F966F6CDC389781DAAFBC7DBFF8F787F0ACE60680370F6C94C0DC615A437068583E7C31FF381E1BAB1F8311EA3FD588FD74D12ACC15E6217E118E76C2C66686215D07BE8A54EA754ECECA6377E0BDFA8289F1FC3380C517E0A2CD06B8A7", 16);
            ModNumber Q = ModNumber.Stomn("BEF143E5D8624534038989D4AC6A76FC83A44E501C20BA8E6D3FC092F0EE36A7", 16);
            ModNumber g = ModNumber.Stomn("402F21E6DCBC4614C35105555518FA9C7F1E78330AA227BE42F4E63C127A677D0736D814FC76F2E411B06EB860C6F9D7014193ED8E5052D02E8E0045940A3C9E4C5DE8C3219A27C912D439B7F8EDFD7039989A18C7254985608B667E642E8526A069CE050E488D94F8AA35BA272EADA29DD42D9C40C8E82A69C07E8CC29801C5463100631D51810306B60BF75BDCEFA779C6C4469575C0F8B4DC100AD74052DF124987E1C6859E03FD79CD4A1AF7BD58EEB3B86D124BEB77ED2FBF82F5104E3B3CB524842A24C8048BEBBB3D153F0A9FB9B98422F8144EB1D243C1ED82F858277E68ACC83D877CD6403573A88C2CA532899631C8D68E811C16939C663AB9A5A3A3DADBAB2587DA7796D620068B26C96FBCFDC616BC0AE10829D9F6ED207FA0B2B1DFE78E3BD9A1AD45C261DC3DB231AC34B971E0B3017509504949C743EBC1D47520459FACECCCB5F442E40CAF35390DFCDB524FE23508B03EDFEDB435DF9A0E4305CFF095E81CFEDA0C1F75B45DAA63A1DAE14AC31629A2E3C9CF6EF0191418", 16);
            ModNumber x = ModNumber.Stomn("8BD650804F276A2EE4FAF859F63E25389614D8541119EF81CA482DEE4F312499", 16);
            ModNumber y = ModNumber.Stomn("A3FCC9FDEE022201CD3F90D8348E71C4CB433B1015BCD63CE124763F94DCA05C431FCB1061445516436F6167B772EE4865189B019323D200FEBA62CB4EF7EA3F553F46699CDB8E4803290FF50145DAB9D77E98F50CEDF12FB3564D7F7B5C88874F7AF5BE5793331BD60B2E32EEF8E88DBAFA738D15D52A3B186E20FAACFCFA383D4B1CD5863B0719CCC332CCCEE0E9739720121C3AB0DE5C754D700A6ABC5953CFEB17BE1DD0E45E519D306A1AF01D89ACAE636DC73617A70AFD7D73926AADD6D1C6706ACAFE60AD8E411FA9D07BE96CC2C91CF22CC7C3AE32DBFF4F5660B9CDFFDB3E975E2169208F400A9017A669469EEAB21CCD0018D67FABCE18697AB775A7EFBE3A08EEF72E0C2A5E026A892799231BB592B23F02C1E829E445CACB29379D2FF6AC1B44DCE032ADBB67C585B0E182E5C9902EC8919BC4BA01E7E2568B0F4134B71529D8F56E0C2DFA4CAE327E14E6473DB59AB4A32294452284B27366A33CFF27DD341D89EC114F3929DC78B0CE719789617A0696AB0A75459FD7E2B25C", 16);
#elif MEDMOD
            ModNumber P = ModNumber.Stomn("DFC017474A4FAF7A5E094FC31BD901731AD1823C5FAF5A06433AB3928FF9BC2EF5A0A90FC0AADB4FEB3294175089DCCE2509E2F7E5E7B7D7FB0CD54A025C6C6DCB599DA196BB9729B824BC811E67F6C1DB95F40DB8BD8A5C8F0C98A2B887127B975279B7F744D0392DD76350F989B2FF212B58557AADD9A718B8EF5453C71F2F4989EC458DF17F1126BE8FA0A48848F028A9808FBF524BF24E0960912640FD0A610C064A243299F992245DEBBAF084601ED8164875B0ABB7EDE528053A4753AC6C91CEB4DA8DE85DC80CDF4CF95A31A03667C5B774E9F7C9436E706F08C4F9FF58ABFECA29D255D732F57CCBB92CDDBD5A56DDAF50ADA60FF932239CCFF8B039", 16);
            ModNumber Q = ModNumber.Stomn("8B50F6EA9476593FC9295A3D28CA0E80E46164C8E5F16E80E97007C8612CDE93", 16);
            ModNumber g = ModNumber.Stomn("DDF1F59904EE5F22270307B45C75153D296965D17FA6C5A12F71D3FDC18BD7FB8C11337958500124550B3084E651FD62407C1E82374641742745169685DA6A9E2060D1A1BBC9E747A2B1A19A004BF527DE7859E75703111E81AD9A0DB62E78208E91CAF32635A0F669DC3A51ED18D8EBB5613355364CF2FBB4D16D2379E81532A7246D824B85AA74CDA49D7EB0582F44C75FDEB0709CEC1F826B8117B6D4348EF58CCE26443A383DD5717CD84F692C587D915B80481838BF27ABFD7B52D37443A52D912CF744FC313F8DDE5D322D054DF9A75717164EBB99CC345E9C39530C3776E8981B5FACB727F57A3BB072DA4C24DDB011B5E2BE8A2D77247B302ADFAB68", 16);
            ModNumber x = ModNumber.Stomn("258A57C6E1F497C6B2D546E21B9C4F24A9666CE4DEF69E1666433B626B818437", 16);
            ModNumber y = ModNumber.Stomn("9A071F438C87B44175A58DF2C0469B7048221E6D920B3983961BFF559099CC8E9C6D312EC9798E8F55CB4FCD85551DB50B91EA9BE12A8D709562D9253395692E9AF2E6F357F291AC5172818292979CDC4FD0A809833E431C5A29DD91D319EA62C1CE24A5F04558A0A918D8CB3D181B5A22D7BA8B5BCCC58120AE7D0D2C90A706C70BE62C5F0ED4D364D6F805A4D5C4F568F3772E5763411B153130EAB0CFC3D681E6C321B99D3EAA91CA3BCA2BD9F6C22A4169AE17750095A75C74FBC4C0A0FB5945DDE9FB2BA4868EDAE6B41940D8A47AF036A9E0DCEDF456DC48A61A0ABF9745F71CA6AEC24CC5783C76D2732662B351EDD3D98C6F83F84B472189C07DC519", 16);
#elif SMALLMOD
            ModNumber P = ModNumber.Stomn("C18006893BCE75DC77605271C0AA43F5BFF9FD7B24F3DE9817A1B8230650542B1B30B66727EA3D44FBB476451D96B869555780D447729A7DC1ED7216B4F5C6A1A544F361723C3ACACBEFFA393389F42A7DE7F42106A30C04B0275DE550C8FF91392AA44FFBEBA94FE07CD19B50C5D66814BB31D741628503B0E579C76D32FF2D", 16);
            ModNumber Q = ModNumber.Stomn("859D7C3729301E8865F41654C0041A3C562A682F", 16);
            ModNumber g = ModNumber.Stomn("847AE11402C2FF443350DE8EB7665060DEC027A5AC1103C98C7F2CCD9C88D127185B7A0189F1674A0039FBD5DCFF4AE152F6C100D122ABF3B3E177F5693673408A0457C35667A3C80E51350F94F9696E8C5AC376FFC51E8CA9FD38169E6A1D0025EA1747D434C8DC8DE47B61E39AE13FCD8AB9ADEBEC982E5B3D249618CAD307", 16);
            ModNumber x = ModNumber.Stomn("334F55231BC70F896CE4BB081583812440110F1A", 16);
            ModNumber y = ModNumber.Stomn("3C7E5B1749C4D012C70A78D303CF9ED5C765DFDC0DB94DB0780704ABC0659B4D00BC13E02BE8CCB4F63E90EE76391CAFF69FCE225094275C912987AECCC21A52C3D2FE5FD338093CCFDEB35D097390609FC44AA8059CA745E161BB96A80AC0F93D6FDDEB2D0520AE3A6C8F7DBA4140B4057AF54C8AD5004FED3327FAB91136B9", 16);

#endif
#if !LARGEMOD
            MultGroupMod mgm = new MultGroupMod(P);
            ModNumber computedY = mgm.Exp(g, x);
            Assert.IsTrue(y == computedY);
            ModNumber pModQ = P % Q;
            ModNumber mone = new ModNumber(1ul);
            Assert.IsTrue(mone == pModQ);
            ModNumber computedGPowQ = mgm.Exp(g, Q);
            Assert.IsTrue(mone == computedGPowQ);
#endif
        }
        [TestMethod]
        public void TestVerifyDSASignature()
        {
            ModularCalculation.DSAParameters dsaParameters;
#if LARGEMODSIGNATURE
            dsaParameters.P = ModNumber.Stomn("BCAE0FB273D98B3CDDDFBB1BE44470A592AAC3EA0885D06C272C5333D8C62AA7F8E5A09D56FF462BFF1032870B306F1A997CE635B920630B128C9030EE5506B7C19992EEBEDE39892D0107E0F6BBC417FE1EF3F1FE436A30E7FB1FD9043D4BAC3A240D6E287FE33090351746BC25639FAD56D3E2922D47092D0EF4AC7B0F0F85E747CF55A11E702276A6C40E285C3FBB61E50755EE3F27764B748623EC67D8E2D8B2E5A6142D244A5C4500C3A580A174FDC9D48BDC9CD93AEF8930563A28FBCBA5E52D8DDCCCE9FDB249E910594B02AB4A9283DEE01B42B405E8D71EC0B613ADCBB7095991256F6B38AE318AFEBB2432D708BCB913E6D67729E8B0F97B30CB30F5223980F792B9C8ECBDB19236FDEE155F1D8F4D0540B63EED872599DFF8B48A3A8F3F966F6CDC389781DAAFBC7DBFF8F787F0ACE60680370F6C94C0DC615A437068583E7C31FF381E1BAB1F8311EA3FD588FD74D12ACC15E6217E118E76C2C66686215D07BE8A54EA754ECECA6377E0BDFA8289F1FC3380C517E0A2CD06B8A7", 16);
            dsaParameters.Q = ModNumber.Stomn("BEF143E5D8624534038989D4AC6A76FC83A44E501C20BA8E6D3FC092F0EE36A7", 16);
            dsaParameters.g = ModNumber.Stomn("402F21E6DCBC4614C35105555518FA9C7F1E78330AA227BE42F4E63C127A677D0736D814FC76F2E411B06EB860C6F9D7014193ED8E5052D02E8E0045940A3C9E4C5DE8C3219A27C912D439B7F8EDFD7039989A18C7254985608B667E642E8526A069CE050E488D94F8AA35BA272EADA29DD42D9C40C8E82A69C07E8CC29801C5463100631D51810306B60BF75BDCEFA779C6C4469575C0F8B4DC100AD74052DF124987E1C6859E03FD79CD4A1AF7BD58EEB3B86D124BEB77ED2FBF82F5104E3B3CB524842A24C8048BEBBB3D153F0A9FB9B98422F8144EB1D243C1ED82F858277E68ACC83D877CD6403573A88C2CA532899631C8D68E811C16939C663AB9A5A3A3DADBAB2587DA7796D620068B26C96FBCFDC616BC0AE10829D9F6ED207FA0B2B1DFE78E3BD9A1AD45C261DC3DB231AC34B971E0B3017509504949C743EBC1D47520459FACECCCB5F442E40CAF35390DFCDB524FE23508B03EDFEDB435DF9A0E4305CFF095E81CFEDA0C1F75B45DAA63A1DAE14AC31629A2E3C9CF6EF0191418", 16);
            dsaParameters.x = ModNumber.Stomn("8BD650804F276A2EE4FAF859F63E25389614D8541119EF81CA482DEE4F312499", 16);
            dsaParameters.y = ModNumber.Stomn("A3FCC9FDEE022201CD3F90D8348E71C4CB433B1015BCD63CE124763F94DCA05C431FCB1061445516436F6167B772EE4865189B019323D200FEBA62CB4EF7EA3F553F46699CDB8E4803290FF50145DAB9D77E98F50CEDF12FB3564D7F7B5C88874F7AF5BE5793331BD60B2E32EEF8E88DBAFA738D15D52A3B186E20FAACFCFA383D4B1CD5863B0719CCC332CCCEE0E9739720121C3AB0DE5C754D700A6ABC5953CFEB17BE1DD0E45E519D306A1AF01D89ACAE636DC73617A70AFD7D73926AADD6D1C6706ACAFE60AD8E411FA9D07BE96CC2C91CF22CC7C3AE32DBFF4F5660B9CDFFDB3E975E2169208F400A9017A669469EEAB21CCD0018D67FABCE18697AB775A7EFBE3A08EEF72E0C2A5E026A892799231BB592B23F02C1E829E445CACB29379D2FF6AC1B44DCE032ADBB67C585B0E182E5C9902EC8919BC4BA01E7E2568B0F4134B71529D8F56E0C2DFA4CAE327E14E6473DB59AB4A32294452284B27366A33CFF27DD341D89EC114F3929DC78B0CE719789617A0696AB0A75459FD7E2B25C", 16);
            string signature = "3044022055386B167337291F0E404B142ED9A9DBF95AD9797C6C5CE550B7BD68BE5B075F022058D0DF7E3152DE1F7155BC76F3FB753B0605EE9D1D13B28AC0E428FB8C79003D";

#elif MEDMOD
            dsaParameters.P = ModNumber.Stomn("DFC017474A4FAF7A5E094FC31BD901731AD1823C5FAF5A06433AB3928FF9BC2EF5A0A90FC0AADB4FEB3294175089DCCE2509E2F7E5E7B7D7FB0CD54A025C6C6DCB599DA196BB9729B824BC811E67F6C1DB95F40DB8BD8A5C8F0C98A2B887127B975279B7F744D0392DD76350F989B2FF212B58557AADD9A718B8EF5453C71F2F4989EC458DF17F1126BE8FA0A48848F028A9808FBF524BF24E0960912640FD0A610C064A243299F992245DEBBAF084601ED8164875B0ABB7EDE528053A4753AC6C91CEB4DA8DE85DC80CDF4CF95A31A03667C5B774E9F7C9436E706F08C4F9FF58ABFECA29D255D732F57CCBB92CDDBD5A56DDAF50ADA60FF932239CCFF8B039", 16);
            dsaParameters.Q = ModNumber.Stomn("8B50F6EA9476593FC9295A3D28CA0E80E46164C8E5F16E80E97007C8612CDE93", 16);
            dsaParameters.g = ModNumber.Stomn("DDF1F59904EE5F22270307B45C75153D296965D17FA6C5A12F71D3FDC18BD7FB8C11337958500124550B3084E651FD62407C1E82374641742745169685DA6A9E2060D1A1BBC9E747A2B1A19A004BF527DE7859E75703111E81AD9A0DB62E78208E91CAF32635A0F669DC3A51ED18D8EBB5613355364CF2FBB4D16D2379E81532A7246D824B85AA74CDA49D7EB0582F44C75FDEB0709CEC1F826B8117B6D4348EF58CCE26443A383DD5717CD84F692C587D915B80481838BF27ABFD7B52D37443A52D912CF744FC313F8DDE5D322D054DF9A75717164EBB99CC345E9C39530C3776E8981B5FACB727F57A3BB072DA4C24DDB011B5E2BE8A2D77247B302ADFAB68", 16);
            dsaParameters.x = ModNumber.Stomn("334F55231BC70F896CE4BB081583812440110F1A", 16);
            dsaParameters.y = ModNumber.Stomn("9A071F438C87B44175A58DF2C0469B7048221E6D920B3983961BFF559099CC8E9C6D312EC9798E8F55CB4FCD85551DB50B91EA9BE12A8D709562D9253395692E9AF2E6F357F291AC5172818292979CDC4FD0A809833E431C5A29DD91D319EA62C1CE24A5F04558A0A918D8CB3D181B5A22D7BA8B5BCCC58120AE7D0D2C90A706C70BE62C5F0ED4D364D6F805A4D5C4F568F3772E5763411B153130EAB0CFC3D681E6C321B99D3EAA91CA3BCA2BD9F6C22A4169AE17750095A75C74FBC4C0A0FB5945DDE9FB2BA4868EDAE6B41940D8A47AF036A9E0DCEDF456DC48A61A0ABF9745F71CA6AEC24CC5783C76D2732662B351EDD3D98C6F83F84B472189C07DC519", 16);
            string signature = "30440220218D53B69428D68138B2B4C66A2B6DB31CE00F16261299EE492D1A597B50341502202BB46382B6F71A663C0548692B2A7F970DE5B4691E6DE730E8CA61683EB11137";

#elif SMALLMOD
            dsaParameters.P = ModNumber.Stomn("C18006893BCE75DC77605271C0AA43F5BFF9FD7B24F3DE9817A1B8230650542B1B30B66727EA3D44FBB476451D96B869555780D447729A7DC1ED7216B4F5C6A1A544F361723C3ACACBEFFA393389F42A7DE7F42106A30C04B0275DE550C8FF91392AA44FFBEBA94FE07CD19B50C5D66814BB31D741628503B0E579C76D32FF2D", 16);
            dsaParameters.Q = ModNumber.Stomn("859D7C3729301E8865F41654C0041A3C562A682F", 16);
            dsaParameters.g = ModNumber.Stomn("847AE11402C2FF443350DE8EB7665060DEC027A5AC1103C98C7F2CCD9C88D127185B7A0189F1674A0039FBD5DCFF4AE152F6C100D122ABF3B3E177F5693673408A0457C35667A3C80E51350F94F9696E8C5AC376FFC51E8CA9FD38169E6A1D0025EA1747D434C8DC8DE47B61E39AE13FCD8AB9ADEBEC982E5B3D249618CAD307", 16);
            dsaParameters.x = ModNumber.Stomn("258A57C6E1F497C6B2D546E21B9C4F24A9666CE4DEF69E1666433B626B818437", 16);
            dsaParameters.y = ModNumber.Stomn("3C7E5B1749C4D012C70A78D303CF9ED5C765DFDC0DB94DB0780704ABC0659B4D00BC13E02BE8CCB4F63E90EE76391CAFF69FCE225094275C912987AECCC21A52C3D2FE5FD338093CCFDEB35D097390609FC44AA8059CA745E161BB96A80AC0F93D6FDDEB2D0520AE3A6C8F7DBA4140B4057AF54C8AD5004FED3327FAB91136B9", 16);
            string signature = "302D02144BB9FCEFAB5E1C25354ADD5873F2468C603027C902150080F7749B950D724EEB88384C4FFAC64F2E474A6C";

#endif
#if !LARGEMOD
            string hash = "25BDECAE5C8BC7905CBBDA89485AFEC7C607D60AC0B1D4EA66C3CA01D7593D87";
            ModNumber mHash = ModNumber.Stomn(hash, 16);
            byte[] hashBigEndian = mHash.convertEndianess();
            ModularCalculation.DSA dsa = new ModularCalculation.DSA(dsaParameters);
            Assert.IsTrue(dsa.Verify(hashBigEndian, signature));
#endif
        }
        [TestMethod]
        public void TestSignAndVerifyDSASignature()
        {
            ModularCalculation.DSAParameters dsaParameters;
#if LARGEMODSIGNATURE
            dsaParameters.P = ModNumber.Stomn("BCAE0FB273D98B3CDDDFBB1BE44470A592AAC3EA0885D06C272C5333D8C62AA7F8E5A09D56FF462BFF1032870B306F1A997CE635B920630B128C9030EE5506B7C19992EEBEDE39892D0107E0F6BBC417FE1EF3F1FE436A30E7FB1FD9043D4BAC3A240D6E287FE33090351746BC25639FAD56D3E2922D47092D0EF4AC7B0F0F85E747CF55A11E702276A6C40E285C3FBB61E50755EE3F27764B748623EC67D8E2D8B2E5A6142D244A5C4500C3A580A174FDC9D48BDC9CD93AEF8930563A28FBCBA5E52D8DDCCCE9FDB249E910594B02AB4A9283DEE01B42B405E8D71EC0B613ADCBB7095991256F6B38AE318AFEBB2432D708BCB913E6D67729E8B0F97B30CB30F5223980F792B9C8ECBDB19236FDEE155F1D8F4D0540B63EED872599DFF8B48A3A8F3F966F6CDC389781DAAFBC7DBFF8F787F0ACE60680370F6C94C0DC615A437068583E7C31FF381E1BAB1F8311EA3FD588FD74D12ACC15E6217E118E76C2C66686215D07BE8A54EA754ECECA6377E0BDFA8289F1FC3380C517E0A2CD06B8A7", 16);
            dsaParameters.Q = ModNumber.Stomn("BEF143E5D8624534038989D4AC6A76FC83A44E501C20BA8E6D3FC092F0EE36A7", 16);
            dsaParameters.g = ModNumber.Stomn("402F21E6DCBC4614C35105555518FA9C7F1E78330AA227BE42F4E63C127A677D0736D814FC76F2E411B06EB860C6F9D7014193ED8E5052D02E8E0045940A3C9E4C5DE8C3219A27C912D439B7F8EDFD7039989A18C7254985608B667E642E8526A069CE050E488D94F8AA35BA272EADA29DD42D9C40C8E82A69C07E8CC29801C5463100631D51810306B60BF75BDCEFA779C6C4469575C0F8B4DC100AD74052DF124987E1C6859E03FD79CD4A1AF7BD58EEB3B86D124BEB77ED2FBF82F5104E3B3CB524842A24C8048BEBBB3D153F0A9FB9B98422F8144EB1D243C1ED82F858277E68ACC83D877CD6403573A88C2CA532899631C8D68E811C16939C663AB9A5A3A3DADBAB2587DA7796D620068B26C96FBCFDC616BC0AE10829D9F6ED207FA0B2B1DFE78E3BD9A1AD45C261DC3DB231AC34B971E0B3017509504949C743EBC1D47520459FACECCCB5F442E40CAF35390DFCDB524FE23508B03EDFEDB435DF9A0E4305CFF095E81CFEDA0C1F75B45DAA63A1DAE14AC31629A2E3C9CF6EF0191418", 16);
            dsaParameters.x = ModNumber.Stomn("8BD650804F276A2EE4FAF859F63E25389614D8541119EF81CA482DEE4F312499", 16);
            dsaParameters.y = ModNumber.Stomn("A3FCC9FDEE022201CD3F90D8348E71C4CB433B1015BCD63CE124763F94DCA05C431FCB1061445516436F6167B772EE4865189B019323D200FEBA62CB4EF7EA3F553F46699CDB8E4803290FF50145DAB9D77E98F50CEDF12FB3564D7F7B5C88874F7AF5BE5793331BD60B2E32EEF8E88DBAFA738D15D52A3B186E20FAACFCFA383D4B1CD5863B0719CCC332CCCEE0E9739720121C3AB0DE5C754D700A6ABC5953CFEB17BE1DD0E45E519D306A1AF01D89ACAE636DC73617A70AFD7D73926AADD6D1C6706ACAFE60AD8E411FA9D07BE96CC2C91CF22CC7C3AE32DBFF4F5660B9CDFFDB3E975E2169208F400A9017A669469EEAB21CCD0018D67FABCE18697AB775A7EFBE3A08EEF72E0C2A5E026A892799231BB592B23F02C1E829E445CACB29379D2FF6AC1B44DCE032ADBB67C585B0E182E5C9902EC8919BC4BA01E7E2568B0F4134B71529D8F56E0C2DFA4CAE327E14E6473DB59AB4A32294452284B27366A33CFF27DD341D89EC114F3929DC78B0CE719789617A0696AB0A75459FD7E2B25C", 16);

#elif MEDMOD
            dsaParameters.P = ModNumber.Stomn("DFC017474A4FAF7A5E094FC31BD901731AD1823C5FAF5A06433AB3928FF9BC2EF5A0A90FC0AADB4FEB3294175089DCCE2509E2F7E5E7B7D7FB0CD54A025C6C6DCB599DA196BB9729B824BC811E67F6C1DB95F40DB8BD8A5C8F0C98A2B887127B975279B7F744D0392DD76350F989B2FF212B58557AADD9A718B8EF5453C71F2F4989EC458DF17F1126BE8FA0A48848F028A9808FBF524BF24E0960912640FD0A610C064A243299F992245DEBBAF084601ED8164875B0ABB7EDE528053A4753AC6C91CEB4DA8DE85DC80CDF4CF95A31A03667C5B774E9F7C9436E706F08C4F9FF58ABFECA29D255D732F57CCBB92CDDBD5A56DDAF50ADA60FF932239CCFF8B039", 16);
            dsaParameters.Q = ModNumber.Stomn("8B50F6EA9476593FC9295A3D28CA0E80E46164C8E5F16E80E97007C8612CDE93", 16);
            dsaParameters.g = ModNumber.Stomn("DDF1F59904EE5F22270307B45C75153D296965D17FA6C5A12F71D3FDC18BD7FB8C11337958500124550B3084E651FD62407C1E82374641742745169685DA6A9E2060D1A1BBC9E747A2B1A19A004BF527DE7859E75703111E81AD9A0DB62E78208E91CAF32635A0F669DC3A51ED18D8EBB5613355364CF2FBB4D16D2379E81532A7246D824B85AA74CDA49D7EB0582F44C75FDEB0709CEC1F826B8117B6D4348EF58CCE26443A383DD5717CD84F692C587D915B80481838BF27ABFD7B52D37443A52D912CF744FC313F8DDE5D322D054DF9A75717164EBB99CC345E9C39530C3776E8981B5FACB727F57A3BB072DA4C24DDB011B5E2BE8A2D77247B302ADFAB68", 16);
            dsaParameters.x = ModNumber.Stomn("258A57C6E1F497C6B2D546E21B9C4F24A9666CE4DEF69E1666433B626B818437", 16);
            dsaParameters.y = ModNumber.Stomn("9A071F438C87B44175A58DF2C0469B7048221E6D920B3983961BFF559099CC8E9C6D312EC9798E8F55CB4FCD85551DB50B91EA9BE12A8D709562D9253395692E9AF2E6F357F291AC5172818292979CDC4FD0A809833E431C5A29DD91D319EA62C1CE24A5F04558A0A918D8CB3D181B5A22D7BA8B5BCCC58120AE7D0D2C90A706C70BE62C5F0ED4D364D6F805A4D5C4F568F3772E5763411B153130EAB0CFC3D681E6C321B99D3EAA91CA3BCA2BD9F6C22A4169AE17750095A75C74FBC4C0A0FB5945DDE9FB2BA4868EDAE6B41940D8A47AF036A9E0DCEDF456DC48A61A0ABF9745F71CA6AEC24CC5783C76D2732662B351EDD3D98C6F83F84B472189C07DC519", 16);

#elif SMALLMOD
            dsaParameters.P = ModNumber.Stomn("C18006893BCE75DC77605271C0AA43F5BFF9FD7B24F3DE9817A1B8230650542B1B30B66727EA3D44FBB476451D96B869555780D447729A7DC1ED7216B4F5C6A1A544F361723C3ACACBEFFA393389F42A7DE7F42106A30C04B0275DE550C8FF91392AA44FFBEBA94FE07CD19B50C5D66814BB31D741628503B0E579C76D32FF2D", 16);
            dsaParameters.Q = ModNumber.Stomn("859D7C3729301E8865F41654C0041A3C562A682F", 16);
            dsaParameters.g = ModNumber.Stomn("847AE11402C2FF443350DE8EB7665060DEC027A5AC1103C98C7F2CCD9C88D127185B7A0189F1674A0039FBD5DCFF4AE152F6C100D122ABF3B3E177F5693673408A0457C35667A3C80E51350F94F9696E8C5AC376FFC51E8CA9FD38169E6A1D0025EA1747D434C8DC8DE47B61E39AE13FCD8AB9ADEBEC982E5B3D249618CAD307", 16);
            dsaParameters.x = ModNumber.Stomn("334F55231BC70F896CE4BB081583812440110F1A", 16);
            dsaParameters.y = ModNumber.Stomn("3C7E5B1749C4D012C70A78D303CF9ED5C765DFDC0DB94DB0780704ABC0659B4D00BC13E02BE8CCB4F63E90EE76391CAFF69FCE225094275C912987AECCC21A52C3D2FE5FD338093CCFDEB35D097390609FC44AA8059CA745E161BB96A80AC0F93D6FDDEB2D0520AE3A6C8F7DBA4140B4057AF54C8AD5004FED3327FAB91136B9", 16);

#endif
#if !LARGEMOD
            string hash = "25BDECAE5C8BC7905CBBDA89485AFEC7C607D60AC0B1D4EA66C3CA01D7593D87";
            ModNumber mHash = ModNumber.Stomn(hash, 16);
            byte[] hashBigEndian = mHash.convertEndianess();
            ModularCalculation.DSA dsa = new ModularCalculation.DSA(dsaParameters);
            string signature = dsa.Sign(hashBigEndian, false);
            Assert.IsTrue(dsa.Verify(hashBigEndian, signature, false));

#endif
        }
        [TestMethod]
        public void TestRSAEncryptAndDecrypt()
        {
            ModularCalculation.RSAParameters rsaParameters;

#if LARGEMOD
            rsaParameters.PubExp = ModNumber.Stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.Stomn("A26960E1102074E8E1E7D0754F6BED06BC5AAFEEA086704ACB33B99289FBC180874418D11E02B1B250313EE873E7307D932DA7C30A5E703CBE86EC588B318B46C20BD8BB01F51549635641E8958B0A109E59B06752ABED027539FBEA8BB700E58AD1985CE405F57BD01F4FB91E88BC8B50C4F0208EA02BB72612F41DE302DB3C7BFF5EB12C277417068CE2CA387BEBDE35320D045438951AD51D34CE1F52B10E1F7A9023E9C647582D266CEF6D2E067B60C1BED9C781ADC520223091210854A356C6CAE5B1874FDC59A643441C228F30E4329345AD96A2605972524BFEE8764D207435E42001C678028C48167610A6FCDDF43BC789DD04A6AC3B337E4F6D8085A404E759936355BCA8E7ED93AB6BB031830CEB586D1CC5A90E87F497B8096AC6ABE37BC701E33C9D9B61FBC8AF73446AAC90301ACDCE9E0DEBD322A032F1C90CA8D766285723B5DE918E65F1A987B32E2D51DB7E2C8EB8F2A28C7292E14BD56FB0EC779F44C138C9DF08BEDBD0568DC8FB9F65B7DD20CE80652C9194E160DCAA6C5F96BB5D357BE742B971B3F73923447C36C906E12147C0153CFB2551AB581513B38F2D3B90F2F3FDD17CC799224AA7DDBCAF41309008637CBAD18C27E226E849532B3CEE58CF3C9B3E147438935BCECB35C47B50E29623F76687A51792B75E077BC3D1240C73F6AB79B33577014C7BE2738BBE1F8B179517C5B5BEC23D7C9D", 16);
            rsaParameters.Prime1 = ModNumber.Stomn("CDA901CDEFC7392972350E5160DCE36D9E3415ABA270EFA599B72EA3A36E4BECF727A46DDE4B39181D2A1B76CA2A79DBFCAEE2DF3D46648BB6F5766DE8FBDB9B97915A5D6BC9E3C2AB30DF0251B7DF103CB8BB9C8873188E395C01CD8C374D72B6B65FBFF47331232D6F86E3EC22B3F866D35C74DF6D76D35836363D020571C86104307324EE0A704FF5BB77DBBCDC7A2D98BBDB0C65B60F22F4EFD5EE8555715F2C79CCC438484C4E1B2136E881C2B04AB27E015AD354AECDF0D717A78205CA7DF0353F2312FD54EF8FB51DA12E35115AFDFA0659C4386DF841C0FF51BB5EC37D555CEDF88FC783FB568FA6F5736060837B4020CACD9D9BAC5F75348673EDCB", 16);
            rsaParameters.Prime2 = ModNumber.Stomn("CA2A5948D61356DEDDCF8F5D180855C87E6981FDE13892F7CF593547B36D1CD5682699FD73D1036A24893EB2B8BCDBA8FC256F65E6644E2FC1E80C1BCADA0E21F6DED17EE6EA989BEB5E912E7FDB448573EC376817C4741C33EF78DCCE9DDE4F13FF6F29FB1117FF0873E5EB0B91AD52E41E2148DED2820C840574608F78E6D0E17DAF74F2B0A6171503EDA87D4B713819F86F07DE94CFFFBC2E8799DF78F138E28205AB7AF0286059980C241CD74C48FE897368E4CE6A096C4677BB2A37CF494395A1C3F695201863AFA632D2BBD61755779C20DD87AD78EE5E8E3EDA91D52FE2D0B91971197AA215EEE3E1F00DE0ED93B42E386C7685B71C1A25BDA0337237", 16);
            rsaParameters.Exp1 = ModNumber.Stomn("92F32C22E62DC81E61489FCE6E13E73DB0492F22D09339E96E0881FED30C409C358DD1E3DF6E88813E7593E2315A60705C00EA82F0D997866D58BF2291D3447BCCB54388753870D1EC58A34098FB751BB90796A6A6A8F6E6C8FD811C4A1049FAA9AB6E874E37F0E85DD2CEE3951AC6D5BB435350FF44CC1EFF6CDD1C82913958205A49F43C730A6A6DF7FBD80CDEC9FB53114B499D761FBC7911E33D4D8D601BFE115D867EBE688A7D54CD4ADA9FBD691236AFEA6FE7891ECE704480D6640597469D98AE6075752677768BC77C1FF4334E7D2365DE4662BBD123CAAFA4CAE94F6E02CCC00D574F0C734ECA9E55CBF7157F5D55D918900B09ED04D84E8BB3D239", 16);
            rsaParameters.Exp2 = ModNumber.Stomn("76DEF5C6CD116C07F9769CFE892E057AB151CA7C845C66964D964FACCC44C12A7E660C2FAA54E0E41D7D919F36DEDD6DFC6CEA968924BB907B3CBD39EE49CEFB8B9432ACA28C98953D8D4A62840D62B9F19298759D5C0745BA07FA12236F52133F57F77E40C12951ED1F70B68703EE5D0A02E961569B829A675A69B68945F6BA258C6889A455D82344591DF9EF4A57D53BF8071922786FE1183E67BF435672C816EF8AF3EC829FCB1F8336C859290312FC8FC78B8AD610256044030CCB632B8011BC3EF4E1DD8794AFB0CD519EF5918A00D826B6706BE1C574ABA68A30AC8524F601F284922B001FA433E64CA881109BC138B5E0B96DEB6BE29B414BC191DE7B", 16);
            rsaParameters.Coefficient = ModNumber.Stomn("80B8FE74BA8F322BB18AD25B5EA75E1B50F54521E648960084FA90895634016A7C3D0997C52A1BC766C58BFFE8CD8953146A188D29480311E624048F83ADAEE157EFC4AE9EFFB144BA8F41E2CACDACD50A51367BB9D013E29B6C859AFC030CAE31B38740F287D61F4CDEB78659E91D404FD6976FD7F5E34C1B7FF530B655AA36161C928BD128D56A6FC1469CF8DB7D308CAFAE0AFDA03A71A399C9EC05889427AAE189EA9DD2BB4A4132FB16622FC9394759F337C8AAD5AF6E3BF659D0624EB10238BA58BDAF858ACF954FBFF03AD12FC0F55C79EDC318FC96D6BC53DEB0688E2C1B80572EC942E37848C75BDAEE6366FA5C11396F8E18DD6AB0F61AD71265D4", 16);
            rsaParameters.PrivExp = ModNumber.Stomn("6DD1E9C365DF3C8E78AB02A740640F2B7DD3A8A12804A00BC3299FB2FA00B5508EA6BA4D3508C75576AA460A72CD0259F75A68D516BED310101935D98D8D9BCAE5CD92E8A9232B18ABC618DD07B9B3BD9705B8A5B51E322E764C160EFC186BCFCB357CDC5B07B87DD73644F73D3162A4A5A1F5695E67EA0777045BF3959C49D8B553F198C5ED8BAF422BA3F2EF8CC3EA4678289B23C3FAF6E0E255E18706A8588F6A95EAE2D9C34659896121E8E6B5555A10C857847328A861D91C94160D070647D8363C68F5C147938B3C72E2BF6FA9B4B48A271481061D367E7A2D8F196B5D2DB3C6F911B7819786C98AB93D6E772C077D6DAAB88B2DE8F20F39FB9003D061F9C8CB3A24B6F650C3C6393117201B765546B40453313045C0C67F9A404016F7073DE6F6877E7D9100EC77B7252D1E064EA99B102E73095D80049D127932C4A63D894DBF37446A5222CBA22B9B1B70B9C09C3EBA69EE4C302B6A59F7EE6C70C1F9283E9E171C14457FB78FAAC615AB15F691318A4CB4E55F1F1A1E635EAE217565A4D94A3505095232A4CB211E579122C2DCB67944A9E2514CEEA5C3E85CEFA916887B9D7B0BC4042EB4622A5E283566105E997FACFCC3EDBD152620E2A472E8CE47464BB0892642AE410CB9A5F283693E2F736444C54338978E21775E616BC2839208E89E5312CD26686C21E964F6AA29874E84FC1776751C1FAF0369B57075", 16);
#elif MEDMOD
            rsaParameters.PubExp = ModNumber.Stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.Stomn("CA75BD1F951E545F8BA1BF6C985398C48BE5CD45E1794AD7D151CF8871D4088C32B1E88D8E4A9106714EACD38C284D70A1E15EBCC289BED8EE90C771B7A45A86DA92C7D5D4936782A701CCC6C7BF75A9CF22E2C079B2E94A4C7ED28B2983AD71B8D24D4E43501CA1FE0C15204A311BD4EEFC9B62A9D1078D27CC5C213A4D328002100FB350D8749DCCC982D736F6ED95B70B146152C966661FF48FFDF8CB8180E0A49D0D62EDED2FBE40D8C639B4F3817EDE2D7D5D9AAE4E0ECE362598DA62613E9B7B9BF7D93F6C232AE9BFC34142A65774DB2241D8B810734FED65C97370FC84599E002680901B55D0E991E51C6C4856FC361FD526193EB972E2C29467F211", 16);
            rsaParameters.Prime1 = ModNumber.Stomn("E0162AC55685C9903231BE3A9A65D9C5E28F7D12683FD835B240DB86A0E3077ADA211CA866C00748430835882D586F44B0FF5784B4980C0EC73C52C7A195D2C514DC5A59E8683F6C0C4B614FD63F510505429928679F7807D7689E48966FBC29728CE81ACE99CDD69F48B0BE47E22AD0A65451FD80DE1E7BD93FF7755EA0FEFB", 16);
            rsaParameters.Prime2 = ModNumber.Stomn("E74B192C59634F62C4E14DF1CF88B26163567EAB1A8DF1B4002CA14DAAEEF3F99DD2A8493DBA4BA79055300D1CA685A9964D69E3F5CEAB0CF728914B66DCDB41063170DDC0254088C33134EE4ACAF86AA4F50458804FC55F306654E221A33566FE9F2ECD22B72527DD402AC3A5FE715EBA7C4FB23998C3E59C2A35F08DB45563", 16);
            rsaParameters.Exp1 = ModNumber.Stomn("4B3AE5ED0E59280C42EDB4419468312E601B700FE7F3D7305FC1AA718E3DE9DAF2B43F1CC23221BFBED158B77911C1793C3ADD34EE344588089755EB33724B2A63107F3CC3EB8BAF48895E0821936C2DD550407EF21AF57F1C704103798DD58BCCAB45990A575465836032060FB7FC4BC42B76C336AA5B6D94C806D9DE850A55", 16);
            rsaParameters.Exp2 = ModNumber.Stomn("AA0EE371623FCC8CB86957DFE55182A66C5D9A40C00381A571435F4E35D1FF7C96F7DE580B94C02F71C5D0D0B33214B0BE1C29313CF839780CF9BCEA8DB4F1587BEF3A273131B2A35BB10053FC9603D49578B93B410C65F83923E461678E12A3A9AEECC05338098FA7F397FDC63CB0AA8FBEFAF1F5BB46BDB9F6A736395A5035", 16);
            rsaParameters.Coefficient = ModNumber.Stomn("6F2C5D2D9A587AA5657032960ABBB1246A0D7B51196625693284F6B8B8222C5B2E8BE3139FEA29D20CC70FD6C8740CA5DE0A94C5ED1D56E6CCE8BC67AAE9153C57A02EEBDC5764B11608AED772B661243E265FBA989B74DB2B9DAC0CEC7AE9C322232C56AFD4BDD8746C8095BBCFDF04C553219092C8E243C94E84757FDA30AC", 16);
            rsaParameters.PrivExp = ModNumber.Stomn("0B5BF1C17CDF57C2889609A471237795C7779C3DC879C2AE3B2B48E691E4609A6519C03F78F54C0FBD79B98EA0F57DEACCDA55336802CC776563301D7C3D376B0B8662B9E79461A15DF766E2DC402CBCDB474CCB99C864F86B344E61CA717DE0BC8AE815750760A3E21A11715F5844047D6824DBB3456C140A6097E6B15BB572899E5706451E48D95BE641E03162BFFCAF5272807661DA94CC9C6081DD179EE2C158F2B713C76EA71969AB83DA5F2EAA59DAC9E29B6F55F637B6C109139B0DEE731F36FF59C799357BB7732DF9C8F5D09085B8187E6FF016E3563833EA34ACD1867DEE6F7844749F32BDAD344E1DC7FE2E89A1696CC5D9E12E3902DBF0FE2265", 16);
#elif SMALLMOD
            rsaParameters.PubExp = ModNumber.Stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.Stomn("B08B5EF115AED8F91EE2FB82E7E8C8611DF101B4138B37787A18D1E72FC75437342A8961F7C462CBD2208DE6EC0472882CE6410BCE3AE096BB2CA0506B5FBF5865FC80465653F2AEA678C8A8638AF8CF98F105166B6CFA71A61567765DF9521FAE5616DBBF26F99512EEBF857C166941FC2402C5E27258B10206A8AF016C45E9", 16);
            rsaParameters.Prime1 = ModNumber.Stomn("C882E9B30B9F87F47AD7653886EF5CD53D9CAA96D2B83C24C643F024B86CDFE61DE6627F0C63B3CBB885DD3212C77CBE47A3469E5EA2FAD245984A75D866AAF3", 16);
            rsaParameters.Prime2 = ModNumber.Stomn("E1668918F4DD83B3066B32577933BD27B1FFA8F49907637863750304EFCBDB1DCC86362EEBC81ACD68B7E0C0BCE35B1BC0F353F16B59C5E7636AF1464805CAB3", 16);
            rsaParameters.Exp1 = ModNumber.Stomn("A8130C8077D2BEECD0F7B2B242716C5896EFB712FC0950E8E684D64C818749DB23DF155B37F06F2AEA3ED39E809D55544ECA6F8C9AF974D4436017B256B620BF", 16);
            rsaParameters.Exp2 = ModNumber.Stomn("74EDCA456611DC166D4802EF99F29B381280FC571763DBD842501B66DE3734CE7AA14D4E7E066AD110780362391B00D29F001C15EB88C0BFCEB555C10DEFE33D", 16);
            rsaParameters.Coefficient = ModNumber.Stomn("89C0F663B05BC7C1CE547866B45F137DC73739DF9C159B5BA648A5C7818BC46A9CD12C41922A82DB150F9D34F08D69B4A94A947848E88DC5735800C882C856A1", 16);
            rsaParameters.PrivExp = ModNumber.Stomn("9E7C2F39FBFE1FD7DC2B662009328717EFFA184E61311C15F27DEF893BF2141F0E9C9502369BBD193E446D3EFD67ACADA4A8FB81AE9C5A5BD621E4B4ECFF625469B82CE442C50E56F2C7E860FD7414AB46C9BA2C8F043FC2FAF5408E50A758BFDB2AF454020A1E77586C4F7E2D7CCF66E354715606B5223C31538AEDAAA0DB85", 16);
#endif
#if !LARGEMODSIGNATURE
            ModularCalculation.RSA rsa = new ModularCalculation.RSA(rsaParameters);
            string message = "Dit is een test";
            ModNumber convertedMessage = ModNumber.fromText(message);
            ModNumber encryptedMessage = rsa.Encrypt(convertedMessage);
            ModNumber decryptedMessage = rsa.Decrypt(encryptedMessage);
            string decryptedString = decryptedMessage.getText();
            Assert.IsTrue(message == decryptedString);
#endif
        }
        [TestMethod]
        public void TestRSASignAndVerify()
        {
            ModularCalculation.RSAParameters rsaParameters;

#if LARGEMOD
            rsaParameters.PubExp = ModNumber.Stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.Stomn("A26960E1102074E8E1E7D0754F6BED06BC5AAFEEA086704ACB33B99289FBC180874418D11E02B1B250313EE873E7307D932DA7C30A5E703CBE86EC588B318B46C20BD8BB01F51549635641E8958B0A109E59B06752ABED027539FBEA8BB700E58AD1985CE405F57BD01F4FB91E88BC8B50C4F0208EA02BB72612F41DE302DB3C7BFF5EB12C277417068CE2CA387BEBDE35320D045438951AD51D34CE1F52B10E1F7A9023E9C647582D266CEF6D2E067B60C1BED9C781ADC520223091210854A356C6CAE5B1874FDC59A643441C228F30E4329345AD96A2605972524BFEE8764D207435E42001C678028C48167610A6FCDDF43BC789DD04A6AC3B337E4F6D8085A404E759936355BCA8E7ED93AB6BB031830CEB586D1CC5A90E87F497B8096AC6ABE37BC701E33C9D9B61FBC8AF73446AAC90301ACDCE9E0DEBD322A032F1C90CA8D766285723B5DE918E65F1A987B32E2D51DB7E2C8EB8F2A28C7292E14BD56FB0EC779F44C138C9DF08BEDBD0568DC8FB9F65B7DD20CE80652C9194E160DCAA6C5F96BB5D357BE742B971B3F73923447C36C906E12147C0153CFB2551AB581513B38F2D3B90F2F3FDD17CC799224AA7DDBCAF41309008637CBAD18C27E226E849532B3CEE58CF3C9B3E147438935BCECB35C47B50E29623F76687A51792B75E077BC3D1240C73F6AB79B33577014C7BE2738BBE1F8B179517C5B5BEC23D7C9D", 16);
            rsaParameters.Prime1 = ModNumber.Stomn("CDA901CDEFC7392972350E5160DCE36D9E3415ABA270EFA599B72EA3A36E4BECF727A46DDE4B39181D2A1B76CA2A79DBFCAEE2DF3D46648BB6F5766DE8FBDB9B97915A5D6BC9E3C2AB30DF0251B7DF103CB8BB9C8873188E395C01CD8C374D72B6B65FBFF47331232D6F86E3EC22B3F866D35C74DF6D76D35836363D020571C86104307324EE0A704FF5BB77DBBCDC7A2D98BBDB0C65B60F22F4EFD5EE8555715F2C79CCC438484C4E1B2136E881C2B04AB27E015AD354AECDF0D717A78205CA7DF0353F2312FD54EF8FB51DA12E35115AFDFA0659C4386DF841C0FF51BB5EC37D555CEDF88FC783FB568FA6F5736060837B4020CACD9D9BAC5F75348673EDCB", 16);
            rsaParameters.Prime2 = ModNumber.Stomn("CA2A5948D61356DEDDCF8F5D180855C87E6981FDE13892F7CF593547B36D1CD5682699FD73D1036A24893EB2B8BCDBA8FC256F65E6644E2FC1E80C1BCADA0E21F6DED17EE6EA989BEB5E912E7FDB448573EC376817C4741C33EF78DCCE9DDE4F13FF6F29FB1117FF0873E5EB0B91AD52E41E2148DED2820C840574608F78E6D0E17DAF74F2B0A6171503EDA87D4B713819F86F07DE94CFFFBC2E8799DF78F138E28205AB7AF0286059980C241CD74C48FE897368E4CE6A096C4677BB2A37CF494395A1C3F695201863AFA632D2BBD61755779C20DD87AD78EE5E8E3EDA91D52FE2D0B91971197AA215EEE3E1F00DE0ED93B42E386C7685B71C1A25BDA0337237", 16);
            rsaParameters.Exp1 = ModNumber.Stomn("92F32C22E62DC81E61489FCE6E13E73DB0492F22D09339E96E0881FED30C409C358DD1E3DF6E88813E7593E2315A60705C00EA82F0D997866D58BF2291D3447BCCB54388753870D1EC58A34098FB751BB90796A6A6A8F6E6C8FD811C4A1049FAA9AB6E874E37F0E85DD2CEE3951AC6D5BB435350FF44CC1EFF6CDD1C82913958205A49F43C730A6A6DF7FBD80CDEC9FB53114B499D761FBC7911E33D4D8D601BFE115D867EBE688A7D54CD4ADA9FBD691236AFEA6FE7891ECE704480D6640597469D98AE6075752677768BC77C1FF4334E7D2365DE4662BBD123CAAFA4CAE94F6E02CCC00D574F0C734ECA9E55CBF7157F5D55D918900B09ED04D84E8BB3D239", 16);
            rsaParameters.Exp2 = ModNumber.Stomn("76DEF5C6CD116C07F9769CFE892E057AB151CA7C845C66964D964FACCC44C12A7E660C2FAA54E0E41D7D919F36DEDD6DFC6CEA968924BB907B3CBD39EE49CEFB8B9432ACA28C98953D8D4A62840D62B9F19298759D5C0745BA07FA12236F52133F57F77E40C12951ED1F70B68703EE5D0A02E961569B829A675A69B68945F6BA258C6889A455D82344591DF9EF4A57D53BF8071922786FE1183E67BF435672C816EF8AF3EC829FCB1F8336C859290312FC8FC78B8AD610256044030CCB632B8011BC3EF4E1DD8794AFB0CD519EF5918A00D826B6706BE1C574ABA68A30AC8524F601F284922B001FA433E64CA881109BC138B5E0B96DEB6BE29B414BC191DE7B", 16);
            rsaParameters.Coefficient = ModNumber.Stomn("80B8FE74BA8F322BB18AD25B5EA75E1B50F54521E648960084FA90895634016A7C3D0997C52A1BC766C58BFFE8CD8953146A188D29480311E624048F83ADAEE157EFC4AE9EFFB144BA8F41E2CACDACD50A51367BB9D013E29B6C859AFC030CAE31B38740F287D61F4CDEB78659E91D404FD6976FD7F5E34C1B7FF530B655AA36161C928BD128D56A6FC1469CF8DB7D308CAFAE0AFDA03A71A399C9EC05889427AAE189EA9DD2BB4A4132FB16622FC9394759F337C8AAD5AF6E3BF659D0624EB10238BA58BDAF858ACF954FBFF03AD12FC0F55C79EDC318FC96D6BC53DEB0688E2C1B80572EC942E37848C75BDAEE6366FA5C11396F8E18DD6AB0F61AD71265D4", 16);
            rsaParameters.PrivExp = ModNumber.Stomn("6DD1E9C365DF3C8E78AB02A740640F2B7DD3A8A12804A00BC3299FB2FA00B5508EA6BA4D3508C75576AA460A72CD0259F75A68D516BED310101935D98D8D9BCAE5CD92E8A9232B18ABC618DD07B9B3BD9705B8A5B51E322E764C160EFC186BCFCB357CDC5B07B87DD73644F73D3162A4A5A1F5695E67EA0777045BF3959C49D8B553F198C5ED8BAF422BA3F2EF8CC3EA4678289B23C3FAF6E0E255E18706A8588F6A95EAE2D9C34659896121E8E6B5555A10C857847328A861D91C94160D070647D8363C68F5C147938B3C72E2BF6FA9B4B48A271481061D367E7A2D8F196B5D2DB3C6F911B7819786C98AB93D6E772C077D6DAAB88B2DE8F20F39FB9003D061F9C8CB3A24B6F650C3C6393117201B765546B40453313045C0C67F9A404016F7073DE6F6877E7D9100EC77B7252D1E064EA99B102E73095D80049D127932C4A63D894DBF37446A5222CBA22B9B1B70B9C09C3EBA69EE4C302B6A59F7EE6C70C1F9283E9E171C14457FB78FAAC615AB15F691318A4CB4E55F1F1A1E635EAE217565A4D94A3505095232A4CB211E579122C2DCB67944A9E2514CEEA5C3E85CEFA916887B9D7B0BC4042EB4622A5E283566105E997FACFCC3EDBD152620E2A472E8CE47464BB0892642AE410CB9A5F283693E2F736444C54338978E21775E616BC2839208E89E5312CD26686C21E964F6AA29874E84FC1776751C1FAF0369B57075", 16);
#elif MEDMOD
            rsaParameters.PubExp = ModNumber.Stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.Stomn("CA75BD1F951E545F8BA1BF6C985398C48BE5CD45E1794AD7D151CF8871D4088C32B1E88D8E4A9106714EACD38C284D70A1E15EBCC289BED8EE90C771B7A45A86DA92C7D5D4936782A701CCC6C7BF75A9CF22E2C079B2E94A4C7ED28B2983AD71B8D24D4E43501CA1FE0C15204A311BD4EEFC9B62A9D1078D27CC5C213A4D328002100FB350D8749DCCC982D736F6ED95B70B146152C966661FF48FFDF8CB8180E0A49D0D62EDED2FBE40D8C639B4F3817EDE2D7D5D9AAE4E0ECE362598DA62613E9B7B9BF7D93F6C232AE9BFC34142A65774DB2241D8B810734FED65C97370FC84599E002680901B55D0E991E51C6C4856FC361FD526193EB972E2C29467F211", 16);
            rsaParameters.Prime1 = ModNumber.Stomn("E0162AC55685C9903231BE3A9A65D9C5E28F7D12683FD835B240DB86A0E3077ADA211CA866C00748430835882D586F44B0FF5784B4980C0EC73C52C7A195D2C514DC5A59E8683F6C0C4B614FD63F510505429928679F7807D7689E48966FBC29728CE81ACE99CDD69F48B0BE47E22AD0A65451FD80DE1E7BD93FF7755EA0FEFB", 16);
            rsaParameters.Prime2 = ModNumber.Stomn("E74B192C59634F62C4E14DF1CF88B26163567EAB1A8DF1B4002CA14DAAEEF3F99DD2A8493DBA4BA79055300D1CA685A9964D69E3F5CEAB0CF728914B66DCDB41063170DDC0254088C33134EE4ACAF86AA4F50458804FC55F306654E221A33566FE9F2ECD22B72527DD402AC3A5FE715EBA7C4FB23998C3E59C2A35F08DB45563", 16);
            rsaParameters.Exp1 = ModNumber.Stomn("4B3AE5ED0E59280C42EDB4419468312E601B700FE7F3D7305FC1AA718E3DE9DAF2B43F1CC23221BFBED158B77911C1793C3ADD34EE344588089755EB33724B2A63107F3CC3EB8BAF48895E0821936C2DD550407EF21AF57F1C704103798DD58BCCAB45990A575465836032060FB7FC4BC42B76C336AA5B6D94C806D9DE850A55", 16);
            rsaParameters.Exp2 = ModNumber.Stomn("AA0EE371623FCC8CB86957DFE55182A66C5D9A40C00381A571435F4E35D1FF7C96F7DE580B94C02F71C5D0D0B33214B0BE1C29313CF839780CF9BCEA8DB4F1587BEF3A273131B2A35BB10053FC9603D49578B93B410C65F83923E461678E12A3A9AEECC05338098FA7F397FDC63CB0AA8FBEFAF1F5BB46BDB9F6A736395A5035", 16);
            rsaParameters.Coefficient = ModNumber.Stomn("6F2C5D2D9A587AA5657032960ABBB1246A0D7B51196625693284F6B8B8222C5B2E8BE3139FEA29D20CC70FD6C8740CA5DE0A94C5ED1D56E6CCE8BC67AAE9153C57A02EEBDC5764B11608AED772B661243E265FBA989B74DB2B9DAC0CEC7AE9C322232C56AFD4BDD8746C8095BBCFDF04C553219092C8E243C94E84757FDA30AC", 16);
            rsaParameters.PrivExp = ModNumber.Stomn("0B5BF1C17CDF57C2889609A471237795C7779C3DC879C2AE3B2B48E691E4609A6519C03F78F54C0FBD79B98EA0F57DEACCDA55336802CC776563301D7C3D376B0B8662B9E79461A15DF766E2DC402CBCDB474CCB99C864F86B344E61CA717DE0BC8AE815750760A3E21A11715F5844047D6824DBB3456C140A6097E6B15BB572899E5706451E48D95BE641E03162BFFCAF5272807661DA94CC9C6081DD179EE2C158F2B713C76EA71969AB83DA5F2EAA59DAC9E29B6F55F637B6C109139B0DEE731F36FF59C799357BB7732DF9C8F5D09085B8187E6FF016E3563833EA34ACD1867DEE6F7844749F32BDAD344E1DC7FE2E89A1696CC5D9E12E3902DBF0FE2265", 16);
#elif SMALLMOD
            rsaParameters.PubExp = ModNumber.Stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.Stomn("B08B5EF115AED8F91EE2FB82E7E8C8611DF101B4138B37787A18D1E72FC75437342A8961F7C462CBD2208DE6EC0472882CE6410BCE3AE096BB2CA0506B5FBF5865FC80465653F2AEA678C8A8638AF8CF98F105166B6CFA71A61567765DF9521FAE5616DBBF26F99512EEBF857C166941FC2402C5E27258B10206A8AF016C45E9", 16);
            rsaParameters.Prime1 = ModNumber.Stomn("C882E9B30B9F87F47AD7653886EF5CD53D9CAA96D2B83C24C643F024B86CDFE61DE6627F0C63B3CBB885DD3212C77CBE47A3469E5EA2FAD245984A75D866AAF3", 16);
            rsaParameters.Prime2 = ModNumber.Stomn("E1668918F4DD83B3066B32577933BD27B1FFA8F49907637863750304EFCBDB1DCC86362EEBC81ACD68B7E0C0BCE35B1BC0F353F16B59C5E7636AF1464805CAB3", 16);
            rsaParameters.Exp1 = ModNumber.Stomn("A8130C8077D2BEECD0F7B2B242716C5896EFB712FC0950E8E684D64C818749DB23DF155B37F06F2AEA3ED39E809D55544ECA6F8C9AF974D4436017B256B620BF", 16);
            rsaParameters.Exp2 = ModNumber.Stomn("74EDCA456611DC166D4802EF99F29B381280FC571763DBD842501B66DE3734CE7AA14D4E7E066AD110780362391B00D29F001C15EB88C0BFCEB555C10DEFE33D", 16);
            rsaParameters.Coefficient = ModNumber.Stomn("89C0F663B05BC7C1CE547866B45F137DC73739DF9C159B5BA648A5C7818BC46A9CD12C41922A82DB150F9D34F08D69B4A94A947848E88DC5735800C882C856A1", 16);
            rsaParameters.PrivExp = ModNumber.Stomn("9E7C2F39FBFE1FD7DC2B662009328717EFFA184E61311C15F27DEF893BF2141F0E9C9502369BBD193E446D3EFD67ACADA4A8FB81AE9C5A5BD621E4B4ECFF625469B82CE442C50E56F2C7E860FD7414AB46C9BA2C8F043FC2FAF5408E50A758BFDB2AF454020A1E77586C4F7E2D7CCF66E354715606B5223C31538AEDAAA0DB85", 16);
#endif
#if !LARGEMODSIGNATURE
            ModularCalculation.RSA rsa = new ModularCalculation.RSA(rsaParameters);
            byte[] hashBigEndian = { 0x87, 0x3D, 0x59, 0xD7, 0x01, 0xCA, 0xC3, 0x66, 0xEA, 0xD4, 0xB1, 0xC0, 0x0A, 0xD6, 0x07, 0xC6, 0xC7, 0xFE, 0x5A, 0x48, 0x89, 0xDA, 0xBB, 0x5C, 0x90, 0xC7, 0x8B, 0x5C, 0xAE, 0xEC, 0xBD, 0x25 };
            ModNumber signature = rsa.EncryptSignature(hashBigEndian, "2.16.840.1.101.3.4.2.1");
            ModNumber decryptedHash = rsa.DecryptSignature(signature);
            byte[] hashLittleEndian = new byte[hashBigEndian.Length];
            for (int i = 0; i < hashLittleEndian.Length;i++)
                hashLittleEndian[i] = (byte)hashBigEndian[hashBigEndian.Length - 1 - i];
            ModNumber originalHash = new ModNumber(hashLittleEndian);
            Assert.IsTrue(originalHash == decryptedHash);
#endif
        }
        [TestMethod]
        public void TestECIsOnCurveA0B17Point00isFalse()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm,g,mzero, mzero, new ModNumber(17ul));
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = mzero;
            pt.y = mzero;
            Assert.IsFalse(myEC.IsOnCurve(pt));

        }
        [TestMethod]
        public void TestECIsOnCurveA0B17PointMinus2And3isTrue()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = mgm.Diff(mzero,new ModNumber(2ul));
            pt.y = new ModNumber(3ul);
            Assert.IsTrue(myEC.IsOnCurve(pt));

        }
        [TestMethod]
        public void TestECIsOnCurveA0B17PointMinus2AndMinus3isTrue()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = mgm.Diff(mzero,new ModNumber(2ul));
            pt.y = mgm.Diff(mzero,new ModNumber(3ul));
            Assert.IsTrue(myEC.IsOnCurve(pt));

        }
        [TestMethod]
        public void TestECIsOnCurveA0B17PointMinus2And4isFalse()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = mgm.Diff(mzero, new ModNumber(2ul));
            pt.y = new ModNumber(4ul);
            Assert.IsFalse(myEC.IsOnCurve(pt));

        }
        [TestMethod]
        public void TestECIsOnCurveA0B17PointMinus1And4isTrue()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = mgm.Diff(mzero, new ModNumber(1ul));
            pt.y = new ModNumber(4ul);
            Assert.IsTrue(myEC.IsOnCurve(pt));

        }
        [TestMethod]
        public void TestECIsOnCurveA0B17PointMinus1AndMinus4isTrue()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = mgm.Diff(mzero, new ModNumber(1ul));
            pt.y = mgm.Diff(mzero, new ModNumber(4ul));
            Assert.IsTrue(myEC.IsOnCurve(pt));

        }
        [TestMethod]
        public void TestECIsOnCurveA0B17Point2And5isTrue()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = new ModNumber(2ul);
            pt.y = new ModNumber(5ul);
            Assert.IsTrue(myEC.IsOnCurve(pt));

        }
        [TestMethod]
        public void TestECIsOnCurveA0B17Point2AndMinus5isTrue()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = new ModNumber(2ul);
            pt.y = mgm.Diff(mzero, new ModNumber(5ul));
            Assert.IsTrue(myEC.IsOnCurve(pt));

        }
        [TestMethod]
        public void TestECCalculateYOnCurveA0B17Point2is5()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModNumber x = new ModNumber(2ul);
            ModNumber exp = new ModNumber(5ul);
            Assert.IsTrue(exp == myEC.CalculateY(x));

        }
        [TestMethod]
        public void TestECCalculateYOnCurveA0B17Point52is375()
        {
            ModNumber p = new ModNumber(1000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModNumber x = new ModNumber(52ul);
            ModNumber exp = new ModNumber(375ul);
            Assert.IsTrue(exp == myEC.CalculateY(x));

        }
        [TestMethod]
        public void TestECCalculateYOnCurveA0B17Point5234is378661()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17ul));
            ModNumber x = new ModNumber(5234ul);
            ModNumber exp = new ModNumber(378661ul);
            Assert.IsTrue(exp == myEC.CalculateY(x));

        }
        [TestMethod]
        public void TestECCalculateYOnCurveAMinus2B0Point0is0()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero,new ModNumber(2ul)), mzero);
            ModNumber x = new ModNumber(0ul);
            ModNumber exp = new ModNumber(0ul);
            Assert.IsTrue(exp == myEC.CalculateY(x));

        }
        [TestMethod]
        public void TestECCalculateYOnCurveAMinus2B0PointMinus1is1()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2ul)), mzero);
            ModNumber x = mgm.Diff(mzero, new ModNumber(1ul));
            ModNumber exp = new ModNumber(1ul);
            Assert.IsTrue(exp == myEC.CalculateY(x));

        }
        [TestMethod]
        public void TestECCalculateYOnCurveAMinus2B0Point2is2()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2ul)), mzero);
            ModNumber x = new ModNumber(2ul);
            ModNumber exp = new ModNumber(2ul);
            Assert.IsTrue(exp == myEC.CalculateY(x));

        }
        [TestMethod]
        public void TestECCalculateYOnCurveAMinus2B0Point338is6214()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2ul)), mzero);
            ModNumber x = new ModNumber(338ul);
            ModNumber exp = new ModNumber(6214ul);
            Assert.IsTrue(exp == myEC.CalculateY(x));

        }
        [TestMethod]
        public void TestECIsOnCurveAMinus2B0Point338AndMinus6214()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2ul)), mzero);
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = new ModNumber(338ul);
            pt.y = mgm.Diff(mzero, new ModNumber(6214ul));
            Assert.IsTrue(myEC.IsOnCurve(pt));

        }
        [TestMethod]
        public void TestECAddCurveAMinus2B0Point00And00IsAtInfinity()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2ul)), mzero);
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = mzero;
            pt.y = mzero;
            ModularCalculation.ECPoint exp = new ModularCalculation.ECPoint();
            exp.IsAtInfinity = true;
            Assert.IsTrue(exp == myEC.Add(pt, pt));

        }
        [TestMethod]
        public void TestECAddCurveAMinus2B0Point00AndAtInfinityIs00()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2ul)), mzero);
            ModularCalculation.ECPoint pt1 = new ModularCalculation.ECPoint();
            pt1.x = mzero;
            pt1.y = mzero;
            ModularCalculation.ECPoint pt2 = new ModularCalculation.ECPoint();
            pt2.IsAtInfinity = true;
            ModularCalculation.ECPoint exp = new ModularCalculation.ECPoint();
            exp.x = mzero;
            exp.y = mzero;
            Assert.IsTrue(exp == myEC.Add(pt1, pt2));

        }
        [TestMethod]
        public void TestECCurveAMinus2B0Point00Mult3IsPoint00()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2ul)), mzero);
            ModularCalculation.ECPoint pt = new ModularCalculation.ECPoint();
            pt.x = mzero;
            pt.y = mzero;
            ModularCalculation.ECPoint exp = new ModularCalculation.ECPoint();
            exp.x = mzero;
            exp.y = mzero;
            Assert.IsTrue(exp == myEC.Mult(pt, new ModNumber(3ul)));

        }
        [TestMethod]
        public void TestECAddCurveAMinus2B0PointAtInfinityAndAtInfinityIsAtInfinity()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2ul)), mzero);
            ModularCalculation.ECPoint pt1 = new ModularCalculation.ECPoint();
            pt1.IsAtInfinity = true;
            ModularCalculation.ECPoint pt2 = new ModularCalculation.ECPoint();
            pt2.IsAtInfinity = true;
            ModularCalculation.ECPoint exp = new ModularCalculation.ECPoint();
            exp.IsAtInfinity = true;
            Assert.IsTrue(exp == myEC.Add(pt1, pt2));

        }
        [TestMethod]
        public void TestECAddCurveAMinus2B0Point00AndMinus1And1IsPoint22()
        {
            ModNumber p = new ModNumber(0x10000000000ul);
            MultGroupMod mgm = new MultGroupMod(p);
            ModularCalculation.ECPoint g = new ModularCalculation.ECPoint();
            g.IsAtInfinity = true;
            ModNumber mzero = new ModNumber(0ul);
            EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2ul)), mzero);
            ModularCalculation.ECPoint pt1 = new ModularCalculation.ECPoint();
            pt1.x = mzero;
            pt1.y = mzero;
            ModularCalculation.ECPoint pt2 = new ModularCalculation.ECPoint();
            pt2.x = mgm.Diff(new ModNumber(0ul),new ModNumber(1ul));
            pt2.y = new ModNumber(1ul);
            ModularCalculation.ECPoint exp = new ModularCalculation.ECPoint();
            exp.x = new ModNumber(2ul);
            exp.y = new ModNumber(2ul);
            Assert.IsTrue(exp == myEC.Add(pt1, pt2));

        }

        [TestMethod]
        public void TestRSADecryptSymmetricKey()
        {
#if LARGEMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSAKey4096", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSAKey4096");
            
#elif MEDMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSAKey2048", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSAKey2048");
#elif SMALLMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSAKey1024", false);
            CngKey cngKey = CngKey.Open("MyCoolRSAKey1024");
#endif

#if !LARGEMODSIGNATURE
            ModularCalculation.RSA rsa = new ModularCalculation.RSA(rsaParameters);
            ModNumber symmetricKey = ModNumber.Stomn("DB278FB45AE1C1D78FA27EBEA3730432DA100140A40F0CCE71A7F95D027C2D15", 16);
            RSACng rsaCng = new RSACng(cngKey);
            byte[] dataBigEndian = symmetricKey.convertEndianess();
            byte[] encryptedKey = rsaCng.Encrypt(dataBigEndian, RSAEncryptionPadding.Pkcs1);
            byte[] encryptedKeyLittleEndian = ModNumber.convertEndianess(encryptedKey);
            ModNumber encryptedKeyModNumber = new ModNumber(encryptedKeyLittleEndian);
            ModNumber decryptedKeyModNumber = rsa.Decrypt(encryptedKeyModNumber);
            Assert.IsTrue(symmetricKey == decryptedKeyModNumber);
            cngKey.Dispose();
#endif
        }

        [TestMethod]
        public void TestRSAEncrypt()
        {
#if LARGEMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSAKey4096", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSAKey4096");
            
#elif MEDMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSAKey2048", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSAKey2048");
#elif SMALLMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSAKey1024", false);
            CngKey cngKey = CngKey.Open("MyCoolRSAKey1024");
#endif

#if !LARGEMODSIGNATURE
            ModularCalculation.RSA rsa = new ModularCalculation.RSA(rsaParameters);
            RSACng rsaCng = new RSACng(cngKey);
            string message = "Dit is een test";
            ModNumber convertedMessage = ModNumber.fromText(message);
            ModNumber encryptedMessage = rsa.Encrypt(convertedMessage);
            byte[] encryptedMessageBigEndian = encryptedMessage.convertEndianess();
            byte[] decryptedMessageBigEndian = rsaCng.Decrypt(encryptedMessageBigEndian,RSAEncryptionPadding.Pkcs1);
            byte[] decryptedMessage = ModNumber.convertEndianess(decryptedMessageBigEndian);
            ModNumber decryptedMessageModNumber = new ModNumber(decryptedMessage);
            string decryptedString = decryptedMessageModNumber.getText();
            Assert.IsTrue(message == decryptedString);
            cngKey.Dispose();
#endif
        }
        [TestMethod]
        public void TestRSADecrypt()
        {
#if LARGEMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSAKey4096", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSAKey4096");
            
#elif MEDMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSAKey2048", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSAKey2048");
#elif SMALLMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSAKey1024", false);
            CngKey cngKey = CngKey.Open("MyCoolRSAKey1024");
#endif

#if !LARGEMODSIGNATURE
            ModularCalculation.RSA rsa = new ModularCalculation.RSA(rsaParameters);
            RSACng rsaCng = new RSACng(cngKey);
            string message = "Dit is een test";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte[] convertedMessageBigEndian = convertedMessage.convertEndianess();
            byte[] encryptedMessageBigEndian = rsaCng.Encrypt(convertedMessageBigEndian,RSAEncryptionPadding.Pkcs1);
            byte[] encryptedMessage = ModNumber.convertEndianess(encryptedMessageBigEndian);
            ModNumber encryptedMessageModNumber = new ModNumber(encryptedMessage);
            ModNumber decryptedMessage = rsa.Decrypt(encryptedMessageModNumber);
            string decryptedString = decryptedMessage.getText();
            Assert.IsTrue(message == decryptedString);
            cngKey.Dispose();
#endif
        }
        [TestMethod]
        public void TestSignatureRSAVerifySHA256()
        {
            string message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            SHA256 sha256 = SHA256.Create();
            byte[] hash =  sha256.ComputeHash(messageBytes);
#if LARGEMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey4096", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey4096");
            
#elif MEDMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey2048", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey2048");
#elif SMALLMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey1024", false);
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey1024");
#endif

#if !LARGEMODSIGNATURE
            ModularCalculation.RSA rsa = new ModularCalculation.RSA(rsaParameters);
            RSACng rsaCng = new RSACng(cngKey);
            byte[] signatureBigEndian = rsaCng.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            byte[] signature = ModNumber.convertEndianess(signatureBigEndian);
            ModNumber signatureModNumber = new ModNumber(signature);
            ModNumber decryptedSignature = rsa.DecryptSignature(signatureModNumber);
            byte[] hashLittleEndian = ModNumber.convertEndianess(hash);
            ModNumber hashModNumber = new ModNumber(hashLittleEndian);
            Assert.IsTrue(hashModNumber == decryptedSignature);
            cngKey.Dispose();
#endif
        }
        [TestMethod]
        public void TestSignatureRSACreateSHA256()
        {
            string message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(messageBytes);
#if LARGEMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey4096", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey4096");
            
#elif MEDMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey2048", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey2048");
#elif SMALLMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey1024", false);
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey1024");
#endif

#if !LARGEMODSIGNATURE
            ModularCalculation.RSA rsa = new ModularCalculation.RSA(rsaParameters);
            RSACng rsaCng = new RSACng(cngKey);
            ModNumber encryptedSignature = rsa.EncryptSignature(hash, "2.16.840.1.101.3.4.2.1");
            byte[] signature = encryptedSignature.convertEndianess();
            bool result = rsaCng.VerifyHash(hash, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            Assert.IsTrue(result);
            cngKey.Dispose();
#endif
        }
        [TestMethod]
        public void TestSignatureRSAVerifySHA512()
        {
            string message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            SHA512 sha512 = SHA512.Create();
            byte[] hash = sha512.ComputeHash(messageBytes);
#if LARGEMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey4096", false);
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey4096");

#elif MEDMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey2048", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey2048");
#elif SMALLMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey1024", false);
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey1024");
#endif

#if !LARGEMODSIGNATURE
            ModularCalculation.RSA rsa = new ModularCalculation.RSA(rsaParameters);
            RSACng rsaCng = new RSACng(cngKey);
            byte[] signatureBigEndian = rsaCng.SignHash(hash, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            byte[] signature = ModNumber.convertEndianess(signatureBigEndian);
            ModNumber signatureModNumber = new ModNumber(signature);
            ModNumber decryptedSignature = rsa.DecryptSignature(signatureModNumber);
            byte[] hashLittleEndian = ModNumber.convertEndianess(hash);
            ModNumber hashModNumber = new ModNumber(hashLittleEndian);
            Assert.IsTrue(hashModNumber == decryptedSignature);
            cngKey.Dispose();
#endif
        }
        [TestMethod]
        public void TestSignatureRSACreateSHA512()
        {
            string message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            SHA512 sha512 = SHA512.Create();
            byte[] hash = sha512.ComputeHash(messageBytes);
#if LARGEMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey4096", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey4096");
            
#elif MEDMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey2048", false) ;
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey2048");
#elif SMALLMOD
            ModularCalculation.RSAParameters rsaParameters = ModularCalculation.RSA.GetRSAKey("MyCoolRSASignatureKey1024", false);
            CngKey cngKey = CngKey.Open("MyCoolRSASignatureKey1024");
#endif

#if !LARGEMODSIGNATURE
            ModularCalculation.RSA rsa = new ModularCalculation.RSA(rsaParameters);
            RSACng rsaCng = new RSACng(cngKey);
            ModNumber encryptedSignature = rsa.EncryptSignature(hash, "2.16.840.1.101.3.4.2.3");
            byte[] signature = encryptedSignature.convertEndianess();
            bool result = rsaCng.VerifyHash(hash, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            Assert.IsTrue(result);
            cngKey.Dispose();
#endif
        }
        [TestMethod]
        public void TestSignatureDSAVerifySHA256()
        {
            string message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(messageBytes);
#if LARGEMODSIGNATURE
            ModularCalculation.DSAParameters dsaParameters = ModularCalculation.DSA.GetDSAKey("MyCoolDSAKey3072", false) ;
            CngKey cngKey = CngKey.Open("MyCoolDSAKey3072");
            
#elif MEDMOD
            ModularCalculation.DSAParameters dsaParameters = ModularCalculation.DSA.GetDSAKey("MyCoolDSAKey2048", false) ;
            CngKey cngKey = CngKey.Open("MyCoolDSAKey2048");
#elif SMALLMOD
            ModularCalculation.DSAParameters dsaParameters = ModularCalculation.DSA.GetDSAKey("MyCoolDSAKey1024", false);
            CngKey cngKey = CngKey.Open("MyCoolDSAKey1024");
#endif

#if !LARGEMOD
            ModularCalculation.DSA dsa = new ModularCalculation.DSA(dsaParameters);
            DSACng dsaCng = new DSACng(cngKey);
            byte[] signatureBigEndian = dsaCng.CreateSignature(hash,DSASignatureFormat.Rfc3279DerSequence);
            byte[] signature = ModNumber.convertEndianess(signatureBigEndian);
            ModNumber signatureModNumber = new ModNumber(signature);
            string signatureStr = signatureModNumber.ToString(16);
            bool valid = dsa.Verify(hash,signatureStr);
            Assert.IsTrue(valid);
            cngKey.Dispose();
#endif
        }
        [TestMethod]
        public void TestSignatureDSACreateSHA256()
        {
            string message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(messageBytes);
#if LARGEMODSIGNATURE
            ModularCalculation.DSAParameters dsaParameters = ModularCalculation.DSA.GetDSAKey("MyCoolDSAKey3072", false) ;
            CngKey cngKey = CngKey.Open("MyCoolDSAKey3072");
            
#elif MEDMOD
            ModularCalculation.DSAParameters dsaParameters = ModularCalculation.DSA.GetDSAKey("MyCoolDSAKey2048", false) ;
            CngKey cngKey = CngKey.Open("MyCoolDSAKey2048");
#elif SMALLMOD
            ModularCalculation.DSAParameters dsaParameters = ModularCalculation.DSA.GetDSAKey("MyCoolDSAKey1024", true);
            CngKey cngKey = CngKey.Open("MyCoolDSAKey1024");
#endif

#if !LARGEMOD
            ModularCalculation.DSA dsa = new ModularCalculation.DSA(dsaParameters);
            DSACng dsaCng = new DSACng(cngKey);
            string encryptedSignature = dsa.Sign(hash, true);
            ModNumber signatureModNumber = ModNumber.Stomn(encryptedSignature, 16);
            byte[] signatureBytes = signatureModNumber.convertEndianess();
            bool result = dsaCng.VerifySignature(hash, signatureBytes, DSASignatureFormat.Rfc3279DerSequence);
            Assert.IsTrue(result);
            cngKey.Dispose();
#endif
        }


    }


}