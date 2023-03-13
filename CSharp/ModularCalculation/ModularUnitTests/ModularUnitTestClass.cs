using ModularCalculation;

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
            for (int i = 0; i < ModNumber.LCOUNT-1; i++)
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
            for (int i = 0;i < ModNumber.LCOUNT;i++)
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
            l[ModNumber.LCOUNT-1] = 1ul;
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
            exp[ModNumber.LCOUNT-1] = 0x12345ul;
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
        [DataRow(0x12345ul, 0x0ul, ModNumber.ISIZE*8)]
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
        [DataRow(0x12345ul, 0x0ul, ModNumber.ISIZE*8)]
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
        public void TestModulo(ulong l, ulong r, ulong exp)
        {
            ModNumber ml = new ModNumber(l);
            ModNumber mr = new ModNumber(r);
            ModNumber mexp = new ModNumber(exp);
            Assert.IsTrue(mexp == ml % mr);
        }


    }
}