#define LARGEMOD


using ModularCalculation;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace ModularUnitTests
{
    [TestClass]
    public class ModularUnitTestClass
    {
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

    }

}