using ModularCalculation;
using System.Collections.Generic;

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


    }
}