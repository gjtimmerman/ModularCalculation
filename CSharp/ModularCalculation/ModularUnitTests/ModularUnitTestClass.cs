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
            r[ModNumber.LCOUNT-1] -= 1;
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
            l[ModNumber.LCOUNT-1] = 1ul;
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
            for (int i=0; i<ModNumber.LCOUNT; i++)
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
            string exp = new string('0', ModNumber.OctalStringLength-1);
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
            string exp = new string('7', ModNumber.OctalStringLength-1);
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
            uint left = (ModNumber.NCOUNT-6) * 8 % 3;
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
            ulong []l = new ulong[ModNumber.LCOUNT];
            for (int i = 0; i < ModNumber.LCOUNT - 6;i+=6)
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
            for (int i = ModNumber.OctalStringLength % 128; i < ModNumber.OctalStringLength; i+=128)
            {
                exp.Append(new string('0', 64));
                exp.Append(new string ('7', 64));
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
            string exp = new string('0', ModNumber.HexStringLength-1);
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
            string exp = new string('0', ModNumber.DecimalStringLength-1);
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
            string exp = new string('0', ModNumber.DecimalStringLength-10);
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
    }
}