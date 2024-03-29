package modularcalculation;

import org.junit.jupiter.api.Test;

import java.io.*;

import static org.junit.jupiter.api.Assertions.*;

public class ModularCalculationTest {

    @Test
    public void subtractScalarSimple()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = 2L;
            exp[i] = 2L;
        }
        ModNumber ml = new ModNumber(l);
        exp[0] = 1L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.subtractScalar(ml,1);
        assertEquals(mexp, mres);
    }
    @Test
    public void subtractAssignScalarWithCarry()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = 0L;
        l[1] = 1L;
        ModNumber ml = new ModNumber(l);
        int r = 1;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = ~0L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber.subtractAssignScalar(ml, r);
        assertEquals(mexp, ml);
    }
    @Test
    public void subtractAssignScalarWithCarryAcrossMultipleSections()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = 1L;
        int r = 1;
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
        {
            exp[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.subtractAssignScalar(ml, r);
        assertEquals(mexp, ml);
    }
    @Test
    public void subtractAssignScalarOneFromZero()
    {
        ModNumber ml = new ModNumber(0L);
        int r = 1;
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            exp[i] = ~0L;
        }
        ModNumber mexp = new ModNumber(exp);
        ModNumber.subtractAssignScalar(ml, r);
        assertEquals(mexp, ml);
    }
    @Test
    public void subtractAssignScalarAllFsFromZero()
    {
        ModNumber ml = new ModNumber(0L);
        int r = ~0;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = (~0L << (ModNumber.ISIZE * 8));
        exp[0] += 1;
        for (int i = 1; i < ModNumber.LCOUNT; i++)
            exp[i] = ~0L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber.subtractAssignScalar(ml, r);
        assertEquals(mexp, ml);
    }
    @Test
    public void subtractAssignScalarAllFsFromAllFs()
    {
        int r = ~0;
        long [] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
            l[i] = ~0L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = (~0L << (ModNumber.ISIZE * 8));
        for (int i = 1; i < ModNumber.LCOUNT; i++)
            exp[i] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.subtractAssignScalar(ml, r);
        assertEquals(mexp, ml);
    }
    @Test
    public void subtractSimple()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = 2;
            r[i] = 1;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mres = ModNumber.subtract(ml, mr);
        assertEquals(mres, mr);
    }
    @Test
    public void subtractAssignSimple()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = 2;
            r[i] = 1;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber.subtractAssign(ml, mr);
        assertEquals(ml, mr);

    }
    @Test
    public void subtractWithCarry()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        l[0] = 0L;
        l[1] = 1L;
        r[0] = 1L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.subtract(ml, mr);
        assertEquals(mexp, mres);

    }
    @Test
    public void subtractAssignWithCarry()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        l[0] = 0L;
        l[1] = 1L;
        r[0] = 1L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.subtractAssign(ml, mr);
        assertEquals(mexp, ml);

    }
    @Test
    public void subtractWithCarryAcrossMultipleSections()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = 1L;
        r[0] = 1L;
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
        {
            exp[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.subtract(ml, mr);
        assertEquals(mexp, mres);

    }
    @Test
    public void subtractAssignWithCarryAcrossMultipleSections()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = 1L;
        r[0] = 1L;
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
        {
            exp[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.subtractAssign(ml, mr);
        assertEquals(mexp, ml);

    }
    @Test
    public void subtractOneFromZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(1L);
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            exp[i] = ~0L;
        }
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.subtract(ml, mr);
        assertEquals(mexp, mres);

    }
    @Test
    public void subtractAssignOneFromZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(1L);
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            exp[i] = ~0L;
        }
        ModNumber mexp = new ModNumber(exp);
        ModNumber.subtractAssign(ml, mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void subtractLeftFsFromZero()
    {
        ModNumber ml = new ModNumber(0L);
        long[] r = new long[ModNumber.LCOUNT];
        r[ModNumber.LCOUNT - 1] = ~0L;
        ModNumber mr = new ModNumber(r);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[ModNumber.LCOUNT - 1] = 1L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.subtract(ml, mr);
        assertEquals(mexp, mres);

    }
    @Test
    public void subtractAssignLeftFsFromZero()
    {
        ModNumber ml = new ModNumber(0L);
        long[] r = new long[ModNumber.LCOUNT];
        r[ModNumber.LCOUNT - 1] = ~0L;
        ModNumber mr = new ModNumber(r);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[ModNumber.LCOUNT - 1] = 1L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber.subtractAssign(ml, mr);
        assertEquals(mexp, ml);

    }
    @Test
    public void subtractAllFsFromZero()
    {
        ModNumber ml = new ModNumber(0L);
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            r[i] = ~0L;
        }
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = ModNumber.subtract(ml, mr);
        assertEquals(mexp, mres);

    }
    @Test
    public void subtractAssignAllFsFromZero()
    {
        ModNumber ml = new ModNumber(0L);
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            r[i] = ~0L;
        }
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(1L);
        ModNumber.subtractAssign(ml, mr);
        assertEquals(mexp, ml);

    }
    @Test
    public void shiftLeftZeroBits()
    {
        ModNumber ml = new ModNumber(0x12345678L);
        ModNumber mexp = new ModNumber(0x12345678L);
        assertEquals(mexp, ModNumber.shiftLeft(ml,  0));

    }
    @Test
    public void shiftLeftOneBit()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber(0x2468aL);
        assertEquals(mexp, ModNumber.shiftLeft(ml,  1));

    }
    @Test
    public void shiftLeft32Bits()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber(0x1234500000000L);
        assertEquals(mexp, ModNumber.shiftLeft(ml,  32));

    }
    @Test
    public void shiftLeftNSIZEBits()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber(0x0L);
        assertEquals(mexp, ModNumber.shiftLeft(ml,  ModNumber.NSIZE));

    }
    @Test
    public void shiftLeftAssignZeroBits()
    {
        ModNumber ml = new ModNumber(0x12345678L);
        ModNumber mexp = new ModNumber(0x12345678L);
        ModNumber.shiftLeftAssign(ml,  0);
        assertEquals(mexp, ml);

    }
    @Test
    public void shiftLeftAssignOneBit()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber(0x2468aL);
        ModNumber.shiftLeftAssign(ml,  1);
        assertEquals(mexp, ml);

    }
    @Test
    public void shiftLeftAssign32Bits()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber(0x1234500000000L);
        ModNumber.shiftLeftAssign(ml,  32);
        assertEquals(mexp, ml);

    }
    @Test
    public void shiftLeftAssignNSIZEBits()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber(0x0L);
        ModNumber.shiftLeftAssign(ml,  ModNumber.NSIZE);
        assertEquals(mexp, ml);

    }
    @Test
    public void shiftLeft60()
    {
        ModNumber ml = new ModNumber(0x0102030405060708L);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x8000000000000000L;
        exp[1] = 0x0010203040506070L;
        ModNumber mexp = new ModNumber(exp);
        assertEquals(mexp ,ModNumber.shiftLeft(ml, 60));
    }
    @Test
    public void shiftLeftAssign60()
    {
        ModNumber ml = new ModNumber(0x0102030405060708L);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x8000000000000000L;
        exp[1] = 0x0010203040506070L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber.shiftLeftAssign(ml, 60);
        assertEquals(mexp, ml);
    }
    @Test
    public void shiftLeft65()
    {
        ModNumber ml = new ModNumber(0x12345L);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[1] = 0x2468aL;
        ModNumber mexp = new ModNumber(exp);
        assertEquals(mexp ,ModNumber.shiftLeft(ml, 65));
    }
    @Test
    public void shiftLeftAssign65()
    {
        ModNumber ml = new ModNumber(0x12345L);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[1] = 0x2468aL;
        ModNumber mexp = new ModNumber(exp);
        ModNumber.shiftLeftAssign(ml, 65);
        assertEquals(mexp, ml);
    }
    @Test
    public void shiftLeftNSIZEMinusLSIZETimes8()
    {
        ModNumber ml = new ModNumber(0x12345L);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[ModNumber.LCOUNT - 1] = 0x12345L;
        ModNumber mexp = new ModNumber(exp);
        assertEquals(mexp ,ModNumber.shiftLeft(ml, ModNumber.NSIZE - ModNumber.LSIZE * 8));
    }
    @Test
    public void shiftLeftAssignNSIZEMinusLSIZETimes8()
    {
        ModNumber ml = new ModNumber(0x12345L);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[ModNumber.LCOUNT - 1] = 0x12345L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber.shiftLeftAssign(ml, ModNumber.NSIZE - ModNumber.LSIZE * 8);
        assertEquals(mexp, ml);
    }
    @Test
    public void shiftRightZeroBits()
    {
        ModNumber ml = new ModNumber(0x12345678L);
        ModNumber mexp = new ModNumber(0x12345678L);
        assertEquals(mexp, ModNumber.shiftRight(ml,  0));

    }
    @Test
    public void shiftRightOneBit()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber( 0x091a2L);
        assertEquals(mexp, ModNumber.shiftRight(ml,  1));

    }
    @Test
    public void shiftRight32Bits()
    {
        ModNumber ml = new ModNumber(0x1234500000000L);
        ModNumber mexp = new ModNumber(0x12345L);
        assertEquals(mexp, ModNumber.shiftRight(ml,  32));

    }
    @Test
    public void shiftRightISIZEBytes()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber(0x0L);
        assertEquals(mexp, ModNumber.shiftRight(ml,ModNumber.ISIZE * 8));

    }
    @Test
    public void shiftRightAssignZeroBits()
    {
        ModNumber ml = new ModNumber(0x12345678L);
        ModNumber mexp = new ModNumber(0x12345678L);
        ModNumber.shiftRightAssign(ml,  0);
        assertEquals(mexp, ml);

    }
    @Test
    public void shiftRightAssignOneBit()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber(0x091a2L);
        ModNumber.shiftRightAssign(ml,  1);
        assertEquals(mexp, ml);

    }
    @Test
    public void shiftRightAssign32Bits()
    {
        ModNumber ml = new ModNumber(0x1234500000000L);
        ModNumber mexp = new ModNumber(0x12345L);
        ModNumber.shiftRightAssign(ml,  32);
        assertEquals(mexp, ml);

    }
    @Test
    public void shiftRightAssignISIZEBytes()
    {
        ModNumber ml = new ModNumber(0x12345L);
        ModNumber mexp = new ModNumber(0x0L);
        ModNumber.shiftRightAssign(ml,  ModNumber.ISIZE * 8);
        assertEquals(mexp, ml);

    }
    @Test
    public void shiftRight60()
    {
        ModNumber mexp = new ModNumber(0x0102030405060708L);
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = 0x8000000000000000L;
        l[1] = 0x0010203040506070L;
        ModNumber ml = new ModNumber(l);
        assertEquals(mexp ,ModNumber.shiftRight(ml, 60));
    }
    @Test
    public void shiftRightAssign60()
    {
        ModNumber mexp = new ModNumber(0x0102030405060708L);
        long[] l= new long[ModNumber.LCOUNT];
        l[0] = 0x8000000000000000L;
        l[1] = 0x0010203040506070L;
        ModNumber ml = new ModNumber(l);
        ModNumber.shiftRightAssign(ml, 60);
        assertEquals(mexp, ml);
    }
    @Test
    public void shiftRight65()
    {
        ModNumber mexp = new ModNumber(0x12345L);
        long[] l = new long[ModNumber.LCOUNT];
        l[1] = 0x2468aL;
        ModNumber ml = new ModNumber(l);
        assertEquals(mexp ,ModNumber.shiftRight(ml, 65));
    }
    @Test
    public void shiftRightAssign65()
    {
        ModNumber mexp = new ModNumber(0x12345L);
        long[] l = new long[ModNumber.LCOUNT];
        l[1] = 0x2468aL;
        ModNumber ml = new ModNumber(l);
        ModNumber.shiftRightAssign(ml, 65);
        assertEquals(mexp, ml);
    }
    @Test
    public void shiftRightNSIZEMinusLSIZETimes8()
    {
        ModNumber mexp = new ModNumber(0x12345L);
        long[] l = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = 0x12345L;
        ModNumber ml = new ModNumber(l);
        assertEquals(mexp ,ModNumber.shiftRight(ml, ModNumber.NSIZE - ModNumber.LSIZE * 8));
    }
    @Test
    public void shiftRightAssignNSIZEMinusLSIZETimes8()
    {
        ModNumber mexp = new ModNumber(0x12345L);
        long[] l = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = 0x12345L;
        ModNumber ml = new ModNumber(l);
        ModNumber.shiftRightAssign(ml, ModNumber.NSIZE - ModNumber.LSIZE * 8);
        assertEquals(mexp, ml);
    }
    @Test
    public void moduloDivideByZero()
    {
        ModNumber ml = new ModNumber(1L);
        ModNumber mr = new ModNumber(0L);
        assertThrows(ArithmeticException.class,() ->  ModNumber.modulo(ml, mr));
    }
    @Test
    public void modulo1000By1()
    {
        ModNumber ml = new ModNumber(1000L);
        ModNumber mr = new ModNumber(1l);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp, ModNumber.modulo(ml,  mr));
    }
    @Test
    public void moduloZeroBy1()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(1l);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp, ModNumber.modulo(ml,  mr));
    }
    @Test
    public void modulo1000By2()
    {
        ModNumber ml = new ModNumber(1000L);
        ModNumber mr = new ModNumber(2l);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp, ModNumber.modulo(ml,  mr));
    }
    @Test
    public void modulo1001By2()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(2l);
        ModNumber mexp = new ModNumber(1L);
        assertEquals(mexp, ModNumber.modulo(ml,  mr));
    }
    @Test
    public void modulo1001By2001()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(2001l);
        ModNumber mexp = new ModNumber(1001L);
        assertEquals(mexp, ModNumber.modulo(ml,  mr));
    }
    @Test
    public void modulo1001By1001()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(1001l);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp, ModNumber.modulo(ml,  mr));
    }
    @Test
    public void modulo101By5()
    {
        ModNumber ml = new ModNumber(101L);
        ModNumber mr = new ModNumber(5l);
        ModNumber mexp = new ModNumber(1L);
        assertEquals(mexp, ModNumber.modulo(ml,  mr));
    }
    @Test
    public void moduloDivide2Pow64ByEight()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[1] = 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(8L);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp,  ModNumber.modulo(ml, mr));

    }
    @Test
    public void moduloDivideAllFsBy2Pow16()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
            l[i] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(65536L);
        ModNumber mexp = new ModNumber(65535L);
        assertEquals(mexp,  ModNumber.modulo(ml, mr));

    }
    @Test
    public void moduloDivideAllFsByAllFs()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
            l[i] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp,  ModNumber.modulo(ml, ml));

    }
    @Test
    public void moduloDivideAllFsByAllFsAndZeroLowWord()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = ~0L;
            r[i] = ~0L;
        }
        r[0] = 0L;
        exp[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        assertEquals(mexp,  ModNumber.modulo(ml, mr));

    }
    @Test
    public void moduloDivideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne()
    {
        ModNumber mnprime1 = new ModNumber(355687428095999L);
        int prime2 = 39916799;
        ModNumber mnprime2 = new ModNumber(prime2);
        ModNumber product = ModNumber.productScalar(mnprime1, prime2);
        ModNumber res1 = ModNumber.modulo(product, mnprime1);
        ModNumber res2 = ModNumber.modulo(product, mnprime2);
        ModNumber mexp1 = new ModNumber(0L);
        assertEquals(mexp1, res1);
        assertEquals(mexp1, res2);
        ModNumber mone = new ModNumber(1L);
        ModNumber mnprime1MinusOne = ModNumber.subtract(mnprime1,mone);
        ModNumber mnprime2MinusOne = ModNumber.subtract(mnprime2, mone);
        ModNumber productMinusPrime1 = ModNumber.subtract(product, mnprime1);
        ModNumber productMinusPrime2 = ModNumber.subtract(product, mnprime2);
        ModNumber res3 = ModNumber.modulo(productMinusPrime1, mnprime2MinusOne);
        ModNumber res4 = ModNumber.modulo(productMinusPrime2, mnprime1MinusOne);
        ModNumber res5 = ModNumber.modulo(productMinusPrime1, mnprime1);
        ModNumber res6 = ModNumber.modulo(productMinusPrime2, mnprime2);
        assertEquals(mexp1, res3);
        assertEquals(mexp1, res4);
        assertEquals(mexp1, res5);
        assertEquals(mexp1, res6);

    }
    @Test
    public void moduloAssignDivideByZero()
    {
        ModNumber ml = new ModNumber(1L);
        ModNumber mr = new ModNumber(0L);
        assertThrows(ArithmeticException.class,() ->  ModNumber.moduloAssign(ml, mr));
    }
    @Test
    public void moduloAssign1000By1()
    {
        ModNumber ml = new ModNumber(1000L);
        ModNumber mr = new ModNumber(1l);
        ModNumber mexp = new ModNumber(0L);
        ModNumber.moduloAssign(ml,  mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void moduloAssignZeroBy1()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(1l);
        ModNumber mexp = new ModNumber(0L);
        ModNumber.moduloAssign(ml,  mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void moduloAssign1000By2()
    {
        ModNumber ml = new ModNumber(1000L);
        ModNumber mr = new ModNumber(2l);
        ModNumber mexp = new ModNumber(0L);
        ModNumber.moduloAssign(ml,  mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void moduloAssign1001By2()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(2l);
        ModNumber mexp = new ModNumber(1L);
        ModNumber.moduloAssign(ml,  mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void moduloAssign1001By2001()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(2001l);
        ModNumber mexp = new ModNumber(1001L);
        ModNumber.moduloAssign(ml,  mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void moduloAssign1001By1001()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(1001l);
        ModNumber mexp = new ModNumber(0L);
        ModNumber.moduloAssign(ml,  mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void moduloAssign101By5()
    {
        ModNumber ml = new ModNumber(101L);
        ModNumber mr = new ModNumber(5l);
        ModNumber mexp = new ModNumber(1L);
        ModNumber.moduloAssign(ml,  mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void moduloAssignDivide2Pow64ByEight()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[1] = 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(8L);
        ModNumber mexp = new ModNumber(0L);
        ModNumber.moduloAssign(ml, mr);
        assertEquals(mexp, ml);

    }
    @Test
    public void moduloAssignDivideAllFsBy2Pow16()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
            l[i] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(65536L);
        ModNumber mexp = new ModNumber(65535L);
        ModNumber.moduloAssign(ml, mr);
        assertEquals(mexp, ml);

    }
    @Test
    public void moduloAssignDivideAllFsByAllFs()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
            l[i] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(0L);
        ModNumber.moduloAssign(ml, ml);
        assertEquals(mexp, ml);

    }
    @Test
    public void moduloAssignDivideAllFsByAllFsAndZeroLowWord()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = ~0L;
            r[i] = ~0L;
        }
        r[0] = 0L;
        exp[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.moduloAssign(ml, mr);
        assertEquals(mexp, ml);

    }
    @Test
    public void moduloAssignDivideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne()
    {
        ModNumber mnprime1 = new ModNumber(355687428095999L);
        int prime2 = 39916799;
        ModNumber mnprime2 = new ModNumber(prime2);
        ModNumber product = ModNumber.productScalar(mnprime1, prime2);
        ModNumber res1 = new ModNumber(product);
        ModNumber.moduloAssign(res1, mnprime1);
        ModNumber res2 = new ModNumber(product);
        ModNumber.moduloAssign(res2, mnprime2);
        ModNumber mexp1 = new ModNumber(0L);
        assertEquals(mexp1, res1);
        assertEquals(mexp1, res2);
        ModNumber mone = new ModNumber(1L);
        ModNumber mnprime1MinusOne = ModNumber.subtract(mnprime1,mone);
        ModNumber mnprime2MinusOne = ModNumber.subtract(mnprime2, mone);
        ModNumber productMinusPrime1 = ModNumber.subtract(product, mnprime1);
        ModNumber productMinusPrime2 = ModNumber.subtract(product, mnprime2);
        ModNumber res3 = new ModNumber(productMinusPrime1);
        ModNumber.moduloAssign(res3, mnprime2MinusOne);
        ModNumber res4 = new ModNumber(productMinusPrime2);
        ModNumber.moduloAssign(res4, mnprime1MinusOne);
        ModNumber res5 = new ModNumber(productMinusPrime1);
        ModNumber.moduloAssign(res5, mnprime1);
        ModNumber res6 = new ModNumber(productMinusPrime2);
        ModNumber.moduloAssign(res6, mnprime2);
        assertEquals(mexp1, res3);
        assertEquals(mexp1, res4);
        assertEquals(mexp1, res5);
        assertEquals(mexp1, res6);

    }
    @Test
    public void divideAndModuloDivideByZero()
    {
        ModNumber ml = new ModNumber(1L);
        ModNumber mr = new ModNumber(0L);
        assertThrows(ArithmeticException.class,() ->  ModNumber.divideAndModulo(ml, mr, false));
    }
    @Test
    public void divideAndModulo1000By1()
    {
        ModNumber ml = new ModNumber(1000L);
        ModNumber mr = new ModNumber(1l);
        ModNumber mexpDiv = new ModNumber(1000L);
        ModNumber mexpMod = new ModNumber(0L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml,  mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());
    }
    @Test
    public void divideAndModuloZeroBy1()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(1l);
        ModNumber mexpDiv = new ModNumber(0L);
        ModNumber mexpMod = new ModNumber(0L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml,  mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());
    }
    @Test
    public void divideAndModulo1000By2()
    {
        ModNumber ml = new ModNumber(1000L);
        ModNumber mr = new ModNumber(2l);
        ModNumber mexpDiv = new ModNumber(500L);
        ModNumber mexpMod = new ModNumber(0L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml,  mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());
    }
    @Test
    public void divideAndModulo1001By2()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(2l);
        ModNumber mexpDiv = new ModNumber(500L);
        ModNumber mexpMod = new ModNumber(1L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml,  mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());
    }
    @Test
    public void divideAndModulo1001By2001()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(2001l);
        ModNumber mexpDiv = new ModNumber(0L);
        ModNumber mexpMod = new ModNumber(1001L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml,  mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());
    }
    @Test
    public void divideAndModulo1001By1001()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(1001l);
        ModNumber mexpDiv = new ModNumber(1L);
        ModNumber mexpMod = new ModNumber(0L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml,  mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());
    }
    @Test
    public void divideAndModulo101By5()
    {
        ModNumber ml = new ModNumber(101L);
        ModNumber mr = new ModNumber(5l);
        ModNumber mexpDiv = new ModNumber(20L);
        ModNumber mexpMod = new ModNumber(1L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml,  mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());
    }
    @Test
    public void divideAndModuloDivide2Pow64ByEight()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[1] = 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(8L);
        ModNumber mexpDiv = ModNumber.shiftRight(ml, 3);
        ModNumber mexpMod = new ModNumber(0L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml,  mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());

    }
    @Test
    public void divideAndModuloDivideAllFsBy2Pow16()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] expDiv = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = ~0L;
            expDiv[i] = ~0L;
        }
        expDiv[ModNumber.LCOUNT - 1] ^= (0xffffL << (ModNumber.LSIZE * 8 - 16));
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(65536L);
        ModNumber mexpDiv = new ModNumber(expDiv);
        ModNumber mexpMod = new ModNumber(0xffffL);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml,  mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());

    }
    @Test
    public void divideAndModuloAllFsLeftAndRightWordByAllFsRightWord()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] expDiv = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        r[0] = ~0L;
        expDiv[0] = 1L;
        l[ModNumber.LCOUNT - 1] = ~0L;
        expDiv[ModNumber.LCOUNT - 1] = 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexpDiv = new ModNumber(expDiv);
        ModNumber mexpMod = new ModNumber(0L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml, mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());

    }

    @Test
    public void divideAndModuloDivideAllFsByAllFs()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = ~0L;
            r[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexpDiv = new ModNumber(1L);
        ModNumber mexpMod = new ModNumber(0L);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml, mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());

    }

    @Test
    public void divideAndModuloDivideAllFsByAllFsAndZeroLowWord()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] expMod = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = ~0L;
            r[i] = ~0L;
        }
        r[0] = 0L;
        expMod[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexpDiv = new ModNumber(1L);
        ModNumber mexpMod = new ModNumber(expMod);
        DivideAndModuloResult result = ModNumber.divideAndModulo(ml, mr, false);
        assertEquals(mexpDiv, result.div());
        assertEquals(mexpMod, result.mod());

    }
    @Test
    public void divideAndModuloDivideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne()
    {
        ModNumber mnprime1 = new ModNumber(355687428095999L);
        int prime2 = 39916799;
        ModNumber mnprime2 = new ModNumber(prime2);
        ModNumber product = ModNumber.productScalar(mnprime1, prime2);
        DivideAndModuloResult result1 = ModNumber.divideAndModulo(product, mnprime1, false);
        DivideAndModuloResult result2 = ModNumber.divideAndModulo(product, mnprime2, false);
        ModNumber mexpDiv1 = new ModNumber(mnprime2);
        ModNumber mexpDiv2 = new ModNumber(mnprime1);
        ModNumber mexpMod = new ModNumber(0L);
        assertEquals(mexpDiv1, result1.div());
        assertEquals(mexpDiv2, result2.div());
        assertEquals(mexpMod, result1.mod());
        assertEquals(mexpMod, result2.mod());
        ModNumber mone = new ModNumber(1L);
        ModNumber mnprime1MinusOne = ModNumber.subtract(mnprime1,mone);
        ModNumber mnprime2MinusOne = ModNumber.subtract(mnprime2, mone);
        ModNumber productMinusPrime1 = ModNumber.subtract(product, mnprime1);
        ModNumber productMinusPrime2 = ModNumber.subtract(product, mnprime2);
        ModNumber mexpDiv3 = new ModNumber(mnprime1);
        ModNumber mexpDiv4 = new ModNumber(mnprime2);
        ModNumber mexpDiv5 = new ModNumber(mnprime2MinusOne);
        ModNumber mexpDiv6 = new ModNumber(mnprime1MinusOne);

        DivideAndModuloResult result3 = ModNumber.divideAndModulo(productMinusPrime1, mnprime2MinusOne, false);
        DivideAndModuloResult result4 = ModNumber.divideAndModulo(productMinusPrime2, mnprime1MinusOne, false);
        DivideAndModuloResult result5 = ModNumber.divideAndModulo(productMinusPrime1, mnprime1, false);
        DivideAndModuloResult result6 = ModNumber.divideAndModulo(productMinusPrime2, mnprime2, false);
        assertEquals(mexpDiv3, result3.div());
        assertEquals(mexpDiv4, result4.div());
        assertEquals(mexpDiv5, result5.div());
        assertEquals(mexpDiv6, result6.div());
        assertEquals(mexpMod, result3.mod());
        assertEquals(mexpMod, result4.mod());
        assertEquals(mexpMod, result5.mod());
        assertEquals(mexpMod, result6.mod());

    }
    @Test
    public void divideAndModuloDivideProductOfLargePrimesByBothPrimesAndByBothPrimesMinusOne()
    {
        ModNumber mnprime1 = ModNumber.stomn("C882E9B30B9F87F47AD7653886EF5CD53D9CAA96D2B83C24C643F024B86CDFE61DE6627F0C63B3CBB885DD3212C77CBE47A3469E5EA2FAD245984A75D866AAF3", 16);
        ModNumber mnprime2 = ModNumber.stomn("E1668918F4DD83B3066B32577933BD27B1FFA8F49907637863750304EFCBDB1DCC86362EEBC81ACD68B7E0C0BCE35B1BC0F353F16B59C5E7636AF1464805CAB3", 16);
        ModNumber product = ModNumber.product(mnprime1, mnprime2);
        DivideAndModuloResult result1 = ModNumber.divideAndModulo(product, mnprime1, false);
        DivideAndModuloResult result2 = ModNumber.divideAndModulo(product, mnprime2, false);
        ModNumber mexpDiv1 = new ModNumber(mnprime2);
        ModNumber mexpDiv2 = new ModNumber(mnprime1);
        ModNumber mexpMod = new ModNumber(0L);
        assertEquals(mexpDiv1, result1.div());
        assertEquals(mexpDiv2, result2.div());
        assertEquals(mexpMod, result1.mod());
        assertEquals(mexpMod, result2.mod());
        ModNumber mone = new ModNumber(1L);
        ModNumber mnprime1MinusOne = ModNumber.subtract(mnprime1,mone);
        ModNumber mnprime2MinusOne = ModNumber.subtract(mnprime2, mone);
        ModNumber productMinusPrime1 = ModNumber.subtract(product, mnprime1);
        ModNumber productMinusPrime2 = ModNumber.subtract(product, mnprime2);
        ModNumber mexpDiv3 = new ModNumber(mnprime1);
        ModNumber mexpDiv4 = new ModNumber(mnprime2);
        ModNumber mexpDiv5 = new ModNumber(mnprime2MinusOne);
        ModNumber mexpDiv6 = new ModNumber(mnprime1MinusOne);

        DivideAndModuloResult result3 = ModNumber.divideAndModulo(productMinusPrime1, mnprime2MinusOne, false);
        DivideAndModuloResult result4 = ModNumber.divideAndModulo(productMinusPrime2, mnprime1MinusOne, false);
        DivideAndModuloResult result5 = ModNumber.divideAndModulo(productMinusPrime1, mnprime1, false);
        DivideAndModuloResult result6 = ModNumber.divideAndModulo(productMinusPrime2, mnprime2, false);
        assertEquals(mexpDiv3, result3.div());
        assertEquals(mexpDiv4, result4.div());
        assertEquals(mexpDiv5, result5.div());
        assertEquals(mexpDiv6, result6.div());
        assertEquals(mexpMod, result3.mod());
        assertEquals(mexpMod, result4.mod());
        assertEquals(mexpMod, result5.mod());
        assertEquals(mexpMod, result6.mod());

    }
    @Test
    public void divideByZero()
    {
        ModNumber ml = new ModNumber(1L);
        ModNumber mr = new ModNumber(0L);
        assertThrows(ArithmeticException.class,() ->  ModNumber.divide(ml, mr));
    }
    @Test
    public void divide1000By1()
    {
        ModNumber ml = new ModNumber(1000L);
        ModNumber mr = new ModNumber(1l);
        ModNumber mexp = new ModNumber(1000L);
        assertEquals(mexp, ModNumber.divide(ml,  mr));
    }
    @Test
    public void divideZeroBy1()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(1l);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp, ModNumber.divide(ml,  mr));
    }
    @Test
    public void divide1000By2()
    {
        ModNumber ml = new ModNumber(1000L);
        ModNumber mr = new ModNumber(2l);
        ModNumber mexp = new ModNumber(500L);
        assertEquals(mexp, ModNumber.divide(ml,  mr));
    }
    @Test
    public void divide1001By2()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(2l);
        ModNumber mexp = new ModNumber(500L);
        assertEquals(mexp, ModNumber.divide(ml,  mr));
    }
    @Test
    public void divide1001By2001()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(2001l);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp, ModNumber.divide(ml,  mr));
    }
    @Test
    public void divide1001By1001()
    {
        ModNumber ml = new ModNumber(1001L);
        ModNumber mr = new ModNumber(1001l);
        ModNumber mexp = new ModNumber(1L);
        assertEquals(mexp, ModNumber.divide(ml,  mr));
    }
    @Test
    public void divide101By5()
    {
        ModNumber ml = new ModNumber(101L);
        ModNumber mr = new ModNumber(5l);
        ModNumber mexp = new ModNumber(20L);
        assertEquals(mexp, ModNumber.divide(ml,  mr));
    }
    @Test
    public void divide2Pow64ByEight()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[1] = 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(8L);
        ModNumber mexp = ModNumber.shiftRight(ml,3);
        assertEquals(mexp,  ModNumber.divide(ml, mr));

    }
    @Test
    public void divideAllFsBy2Pow16()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = ~0L;
            exp[i] = ~0L;
        }
        exp[ModNumber.LCOUNT - 1] ^= (0xffffL << (ModNumber.LSIZE * 8 - 16));
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(65536L);
        ModNumber mexp = new ModNumber(exp);
        assertEquals(mexp,  ModNumber.divide(ml, mr));

    }
    @Test
    public void divideAllFsLeftAndRightWordByAllFsRightWord()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        r[0] = ~0L;
        exp[0] = 1L;
        l[ModNumber.LCOUNT - 1] = ~0L;
        exp[ModNumber.LCOUNT - 1] = 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        assertEquals(mexp, ModNumber.divide(ml, mr));

    }

    @Test
    public void divideAllFsByAllFs()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
            l[i] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(1L);
        assertEquals(mexp,  ModNumber.divide(ml, ml));

    }
    @Test
    public void divideAllFsByAllFsAndZeroLowWord()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = ~0L;
            r[i] = ~0L;
        }
        r[0] = 0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(1L);
        assertEquals(mexp,  ModNumber.divide(ml, mr));

    }
    @Test
    public void divideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne()
    {
        ModNumber mnprime1 = new ModNumber(355687428095999L);
        int prime2 = 39916799;
        ModNumber mnprime2 = new ModNumber(prime2);
        ModNumber product = ModNumber.productScalar(mnprime1, prime2);
        ModNumber res1 = ModNumber.divide(product, mnprime1);
        ModNumber res2 = ModNumber.divide(product, mnprime2);
        ModNumber mexp1 = new ModNumber(prime2);
        ModNumber mexp2 = new ModNumber(mnprime1);
        assertEquals(mexp1, res1);
        assertEquals(mexp2, res2);
        ModNumber mone = new ModNumber(1L);
        ModNumber mnprime1MinusOne = ModNumber.subtract(mnprime1,mone);
        ModNumber mnprime2MinusOne = ModNumber.subtract(mnprime2, mone);
        ModNumber productMinusPrime1 = ModNumber.subtract(product, mnprime1);
        ModNumber productMinusPrime2 = ModNumber.subtract(product, mnprime2);
        ModNumber res3 = ModNumber.divide(productMinusPrime1, mnprime2MinusOne);
        ModNumber res4 = ModNumber.divide(productMinusPrime2, mnprime1MinusOne);
        ModNumber res5 = ModNumber.divide(productMinusPrime1, mnprime1);
        ModNumber res6 = ModNumber.divide(productMinusPrime2, mnprime2);
        ModNumber mexp3 = new ModNumber(mnprime1);
        ModNumber mexp4 = new ModNumber(prime2);
        ModNumber mexp5 = new ModNumber(mnprime2MinusOne);
        ModNumber mexp6 = new ModNumber(mnprime1MinusOne);
        assertEquals(mexp3, res3);
        assertEquals(mexp4, res4);
        assertEquals(mexp5, res5);
        assertEquals(mexp6, res6);

    }
    @Test
    public void equalTrue()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(ml);
        assertEquals(ml, mr);
    }
    @Test
    public void subtractEqualNumbers()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(ml);
        ModNumber mres = ModNumber.subtract(ml, mr);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp, mres);
    }
    @Test
    public void equalNotTrueFirstSection()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
            r[i] = i;
        }
        r[0] -= 1;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        assertNotEquals(ml, mr);

    }
    @Test
    public void equalNotTrueLastSection()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
            r[i] = i;
        }
        r[ModNumber.LCOUNT - 1] -= 1;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        assertNotEquals(ml, mr);

    }
    @Test
    public void lessThanTrue()
    {
        ModNumber ml = new ModNumber(1L);
        ModNumber mr = new ModNumber(2L);
        assertTrue(ModNumber.lessThan(ml,  mr));
    }
    @Test
    public void lessThanFalse()
    {
        ModNumber ml = new ModNumber(1L);
        ModNumber mr = new ModNumber(2L);
        assertFalse(ModNumber.lessThan(mr,  ml));
    }
    @Test
    public void greaterThanTrueForLargeDifference()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = 1L;
        r[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        assertTrue(ModNumber.greaterThan(ml,  mr));

    }
    @Test
    public void lessThanFalseForLargeDifference()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = 1L;
        r[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        assertFalse(ModNumber.lessThan(ml,  mr));

    }
    @Test
    public void lessThanFalseForEquality()
    {
        ModNumber ml = new ModNumber(1234L);
        ModNumber mr = new ModNumber(ml);
        assertFalse( ModNumber.lessThan(ml, mr));

    }
    @Test
    public void lessThanFalseForEqualityOfZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(ml);
        assertFalse( ModNumber.lessThan(ml, mr));

    }
    @Test
    public void lessOrEqualTrueForLessThan()
    {
        ModNumber ml = new ModNumber(1L);
        ModNumber mr = new ModNumber(2L);
        assertTrue( ModNumber.lessThanOrEqual(ml, mr));

    }
    @Test
    public void lessOrEqualFalseForGreaterThan()
    {
        ModNumber ml = new ModNumber(1L);
        ModNumber mr = new ModNumber(2L);
        assertFalse( ModNumber.lessThanOrEqual(mr, ml));

    }
    @Test
    public void lessOrEqualTrueForEquality()
    {
        ModNumber ml = new ModNumber(1234L);
        ModNumber mr = new ModNumber(ml);
        assertTrue( ModNumber.lessThanOrEqual(ml, mr));

    }
    @Test
    public void lessOrEqualTrueForEqualityOfZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(ml);
        assertTrue( ModNumber.lessThanOrEqual(ml, mr));

    }
    @Test
    public void greaterThanTrue()
    {
        ModNumber ml = new ModNumber(2L);
        ModNumber mr = new ModNumber(1L);
        assertTrue(ModNumber.greaterThan(ml,  mr));
    }
    @Test
    public void greaterThanFalse()
    {
        ModNumber ml = new ModNumber(1L);
        ModNumber mr = new ModNumber(2L);
        assertFalse(ModNumber.greaterThan(ml,  mr));
    }


    @Test
    public void greaterThanFalseForEquality()
    {
        ModNumber ml = new ModNumber(1234L);
        ModNumber mr = new ModNumber(ml);
        assertFalse( ModNumber.greaterThan(ml, mr));

    }
    @Test
    public void greaterThanFalseForEqualityOfZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(ml);
        assertFalse( ModNumber.greaterThan(ml, mr));

    }
    @Test
    public void greaterOrEqualTrueForGreaterThan()
    {
        ModNumber ml = new ModNumber(2L);
        ModNumber mr = new ModNumber(1L);
        assertTrue( ModNumber.greaterThanOrEqual(ml, mr));

    }
    @Test
    public void greaterOrEqualFalseForLessThan()
    {
        ModNumber ml = new ModNumber(2L);
        ModNumber mr = new ModNumber(1L);
        assertFalse( ModNumber.greaterThanOrEqual(mr, ml));

    }
    @Test
    public void greaterOrEqualTrueForEquality()
    {
        ModNumber ml = new ModNumber(1234L);
        ModNumber mr = new ModNumber(ml);
        assertTrue( ModNumber.greaterThanOrEqual(ml, mr));

    }
    @Test
    public void greaterOrEqualTrueForEqualityOfZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(ml);
        assertTrue( ModNumber.greaterThanOrEqual(ml, mr));

    }
    @Test
    public void addAssignScalarOneToZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber.addAssignScalar(ml, 1);
        assertEquals(mexp, ml);
    }
    @Test
    public void addScalarOneToZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = ModNumber.addScalar(ml, 1);
        assertEquals(mexp, mres);
    }
    @Test
    public void addAssignScalarOneToFirstSectionMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[1] = 1L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber.addAssignScalar(ml,1);
        assertEquals(mexp, ml);
    }
    @Test
    public void addScalarOneToFirstSectionMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[1] = 1L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres =  ModNumber.addScalar(ml,1);
        assertEquals(mexp, mres);
    }
    @Test
    public void addAssignScalarOneToMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(0L);
        ModNumber.addAssignScalar(ml, 1);
        assertEquals(mexp, ml);
    }
    @Test
    public void addScalarOneToMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = ModNumber.addScalar(ml, 1);
        assertEquals(mexp, mres);
    }
    @Test
    public void addAssignScalarMaxToMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(0xFEL);
        ModNumber.addAssignScalar(ml, 0xFF);
        assertEquals(mexp, ml);
    }
    @Test
    public void addScalarMaxToMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(0xFEL);
        ModNumber mres =  ModNumber.addScalar(ml, 0xFF);
        assertEquals(mexp, mres);
    }
    @Test
    public void addAssignOneToZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(1L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber.addAssign(ml, mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void addOneToZero()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mr = new ModNumber(1L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = ModNumber.add(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void addAssignOneToFirstSectionMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(1L);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[1] = 1L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber.addAssign(ml,mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void addOneToFirstSectionMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(1L);
        long[] exp = new long[ModNumber.LCOUNT];
        exp[1] = 1L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres =  ModNumber.add(ml,mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void addAssignOneToMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(1L);
        ModNumber mexp = new ModNumber(0L);
        ModNumber.addAssign(ml, mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void addSelfAssignMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
            exp[i] = ~0L;
        }
        exp[0] -= 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.addAssign(ml, ml);
        assertEquals(mexp, ml);
    }
    @Test
    public void addAssignFirstWordMaxToMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(~0L);
        ModNumber mexp = new ModNumber(~0L - 1);
        ModNumber.addAssign(ml, mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void addFirstWordMaxToMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(~0L);
        ModNumber mexp = new ModNumber(~0L - 1);
        ModNumber mres = ModNumber.add(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void addAssignMaxToMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
            r[i] = ~0L;
            exp[i] = ~0L;
        }
        exp[0] -= 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.addAssign(ml, mr);
        assertEquals(mexp, ml);
    }
    @Test
    public void addMaxToMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
            r[i] = ~0L;
            exp[i] = ~0L;
        }
        exp[0] -= 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.add(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void productAssignScalarByZero()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(0L);
        ModNumber.productAssignScalar(ml, 0);
        assertEquals(mexp, ml);
    }
    @Test
    public void productScalarByZero()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = ModNumber.productScalar(ml, 0);
        assertEquals(mexp, mres);
    }
    @Test
    public void productAssignScalarByOne()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(ml);
        ModNumber.productAssignScalar(ml, 1);
        assertEquals(mexp, ml);
    }
    @Test
    public void productScalarByOne()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(ml);
        ModNumber mres = ModNumber.productScalar(ml, 1);
        assertEquals(mexp, mres);
    }
    @Test
    public void productAssignScalarByTwo()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
            exp[i] = i * 2;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.productAssignScalar(ml, 2);
        assertEquals(mexp, ml);
    }
    @Test
    public void productScalarByTwo()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = i;
            exp[i] = i * 2;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.productScalar(ml, 2);
        assertEquals(mexp, mres);
    }
    @Test
    public void productAssignScalarAllFFFFByTwo()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
            exp[i] = ~0L;
        }
        exp[0] ^= 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.productAssignScalar(ml, 2);
        assertEquals(mexp, ml);
    }
    @Test
    public void productScalarAllFFFFByTwo()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
            exp[i] = ~0L;
        }
        exp[0] ^= 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.productScalar(ml, 2);
        assertEquals(mexp, mres);
    }
    @Test
    public void productAssignScalarFsBy2Pow16()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        exp[1] = ~0L >>> (ModNumber.LSIZE * 8 - 16);
        exp[0] = ~0L << 16;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.productAssignScalar(ml, 65536);
        assertEquals(mexp, ml);
    }
    @Test
    public void productScalarFsBy2Pow16()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        exp[1] = ~0L >>> (ModNumber.LSIZE * 8 - 16);
        exp[0] = ~0L << 16;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.productScalar(ml, 65536);
        assertEquals(mexp, mres);
    }
    @Test
    public void productAssignScalarAllAsByTwo()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = 0xaaaaaaaaaaaaaaaaL;
            exp[i] = (l[i] << 1) + 1;
        }
        exp[0] = l[0] << 1;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber.productAssignScalar(ml, 2);
        assertEquals(mexp, ml);
    }
    @Test
    public void productScalarAllAsByTwo()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = 0xaaaaaaaaaaaaaaaaL;
            exp[i] = (l[i] << 1) + 1;
        }
        exp[0] = l[0] << 1;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.productScalar(ml, 2);
        assertEquals(mexp, mres);
    }
    @Test
    public void productAssignScalar9sDecBy9sDec()
    {
        ModNumber ml = new ModNumber(99999999L);
        ModNumber mexp = ModNumber.stomn("9999999800000001", 10);
        ModNumber.productAssignScalar(ml, 99999999);
        assertEquals(mexp, ml);
    }
    @Test
    public void productScalar9sDecBy9sDec()
    {
        ModNumber ml = new ModNumber(99999999L);
        ModNumber mexp = ModNumber.stomn("9999999800000001", 10);
        ModNumber mres = ModNumber.productScalar(ml, 99999999);
        assertEquals(mexp, mres);
    }
    @Test
    public void divideScalarByZero()
    {
        ModNumber ml = new ModNumber(1L);
        assertThrows(ArithmeticException.class,() ->  ml.divide( 0));
    }
    @Test
    public void divideScalarZeroBy1()
    {
        ModNumber ml = new ModNumber(0L);
        ModNumber mexp = new ModNumber(0L);
        assertEquals(mexp, ml.divide(1));
    }
    @Test
    public void divideScalarNonZeroBy1()
    {
        ModNumber ml = new ModNumber(123456L);
        ModNumber mexp = new ModNumber(123456L);
        assertEquals(mexp, ml.divide(1));
    }
    @Test
    public void divideScalarNonZeroBy2()
    {
        ModNumber ml = new ModNumber(24690L);
        ModNumber mexp = new ModNumber(12345L);
        assertEquals(mexp, ml.divide(2));
    }
    @Test
    public void divideScalarAll9sBy3()
    {
        long []l = new long[ModNumber.LCOUNT];
        long []exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = 999999999999999999L;
            exp[i] = 333333333333333333L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        assertEquals(mexp, ml.divide(3));
    }
    @Test
    public void divideScalarAll9sBy2AndMultipliedBy2()
    {
        long []l = new long[ModNumber.LCOUNT];
        long []exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = 999999999999999999L;
            exp[i] = l[i];
        }
        exp[0] -= 1;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres1 = ml.divide(2);
        ModNumber mres2 = ModNumber.productScalar(mres1,2);
        assertEquals(mexp, mres2);
    }
    @Test
    public void divideScalarMaxUlongTimes2By2()
    {
        long []l = new long[ModNumber.LCOUNT];
        long []exp = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        l[1] = 1;
        exp[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres1 = ml.divide(2);
        assertEquals(mexp, mres1);
    }
    @Test
    public void divideScalarAllAsBy2()
    {
        long []l = new long[ModNumber.LCOUNT];
        long []exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = 0xaaaaaaaaaaaaaaaaL;
            exp[i] = 0x5555555555555555L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);
        assertEquals(mexp, ml.divide(2));
    }
    @Test
    public void toStringIllegalBase()
    {
        final ModNumber ml = new ModNumber(0L);
        assertThrows(IllegalArgumentException.class, () -> ml.toString(11));
    }
    @Test
    public void toStringOctalForZero()
    {
        ModNumber ml = new ModNumber(0L);
        String exp = "0".repeat(ModNumber.OctalStringLength);
        String res = ml.toString(8);
        assertEquals(exp, res);
    }

    @Test
    public void toStringOctalForOne()
    {
        ModNumber ml = new ModNumber(1L);
        String exp = "0".repeat(ModNumber.OctalStringLength - 1);
        exp += "1";
        String res = ml.toString(8);
        assertEquals(exp, res);
    }
    @Test
    public void toStringOctalForEight()
    {
        ModNumber ml = new ModNumber(8L);
        String exp = "0".repeat(ModNumber.OctalStringLength - 2);
        exp += "10";
        String res = ml.toString(8);
        assertEquals(exp, res);
    }
    @Test
    public void toStringOctalForEightScale6()
    {
        ModNumber ml = new ModNumber(8L);
        ScaledNumber sn = new ScaledNumber(ml, 6, false);
        StringLengthResult stringLengthResult = sn.CalculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft() - 2);
        exp += "10.0000000000000000";
        String res = sn.toString(8);
        assertEquals(exp, res);
    }
    @Test
    public void toStringOctalFor0x012345678910Scale6()
    {
        ModNumber ml = new ModNumber(0x012345678910L);
        ScaledNumber sn = new ScaledNumber(ml, 6, true);
        StringLengthResult stringLengthResult = sn.CalculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft());
        exp += ".0022150531704420";
        String res = sn.toString(8);
        assertEquals(exp, res);
    }
    @Test
    public void toStringOctalForMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        String exp = "7".repeat(ModNumber.OctalStringLength - 1);
        int left = ModNumber.NCOUNT * 8 % 3;
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
        String res = ml.toString(8);
        assertEquals(exp, res);
    }
    @Test
    public void toStringOctalForMaxScale6()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ScaledNumber sn = new ScaledNumber(ml, 6, true);
        StringLengthResult stringLengthResult = sn.CalculateOctalStringLength();
        String exp = "7".repeat(stringLengthResult.digitsLeft() - 1);
        int left = (ModNumber.NCOUNT - 6) * 8 % 3;
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
        String res = sn.toString(8);
        assertEquals(exp, res);
    }
    @Test
    public void toStringOctalForMaxesAndZeros()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT - 6; i += 6)
        {
            for (int j = 0; j < 3; j++)
                l[i + j] = ~0L;
            for (int j = 3; j < 6; j++)
                l[i + j] = 0L;
        }
        for (int i = ModNumber.LCOUNT - (ModNumber.LCOUNT % 6); i < ModNumber.LCOUNT; i++)
            l[i] = 0;
        ModNumber ml = new ModNumber(l);
        StringBuilder exp = new StringBuilder(ModNumber.OctalStringLength);
        exp.append("0".repeat(ModNumber.OctalStringLength % 128));
        for (int i = ModNumber.OctalStringLength % 128; i < ModNumber.OctalStringLength; i += 128)
        {
            exp.append("0".repeat(64));
            exp.append("7".repeat(64));
        }
        String res = ml.toString(8);
        assertEquals(exp.toString(), res);

    }
    @Test
    public void toStringHexForZero()
    {
        ModNumber ml = new ModNumber(0L);
        String exp = "0".repeat(ModNumber.HexStringLength);
        String res = ml.toString(16);
        assertEquals(exp, res);
    }

    @Test
    public void toStringHexForOne()
    {
        ModNumber ml = new ModNumber(1L);
        String exp = "0".repeat(ModNumber.HexStringLength - 1);
        exp += "1";
        String res = ml.toString(16);
        assertEquals(exp, res);
    }
    @Test
    public void toStringHexForSixteen()
    {
        ModNumber ml = new ModNumber(16L);
        String exp = "0".repeat(ModNumber.HexStringLength - 2);
        exp += "10";
        String res = ml.toString(16);
        assertEquals(exp, res);
    }
    @Test
    public void toStringHexForMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        String exp = "F".repeat(ModNumber.HexStringLength);
        String res = ml.toString(16);
        assertEquals(exp, res);
    }
    @Test
    public void toStringDecimalForZero()
    {
        ModNumber ml = new ModNumber(0L);
        String exp = "0".repeat(ModNumber.DecimalStringLength);
        String res = ml.toString(10);
        assertEquals(exp, res);
    }

    @Test
    public void toStringDecimalForOne()
    {
        ModNumber ml = new ModNumber(1L);
        String exp = "0".repeat(ModNumber.DecimalStringLength - 1);
        exp += "1";
        String res = ml.toString(10);
        assertEquals(exp, res);
    }
    @Test
    public void toStringDecimalForTen()
    {
        ModNumber ml = new ModNumber(10L);
        String exp = "0".repeat(ModNumber.DecimalStringLength - 2);
        exp += "10";
        String res = ml.toString(10);
        assertEquals(exp, res);
    }
    @Test
    public void toStringDecimalForMaxIntPlusOne()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = 1L << ModNumber.ISIZE * 8;
        ModNumber ml = new ModNumber(l);
        String exp = "0".repeat(ModNumber.DecimalStringLength - 10);
        exp += "4294967296";
        String res = ml.toString(10);
        assertEquals(exp, res);
    }
    @Test
    public void toStringDecimalForMaxInt()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long mask = 0xffffffffL;
        l[0] = ~0 & mask;
        ModNumber ml = new ModNumber(l);
        String exp = "0".repeat(ModNumber.DecimalStringLength - 10);
        exp += "4294967295";
        String res = ml.toString(10);
        assertEquals(exp, res);
    }
    @Test
    public void toStringDecimalForMax()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        String res = ml.toString(10);
        ModNumber exp = ModNumber.stomn(res, 10);
        assertEquals(exp, ml);
    }
    @Test
    public void toModularNumberIllegalBase()
    {
        final String s = "";
        assertThrows(IllegalArgumentException.class, () -> ModNumber.stomn(s, 11));
    }
    @Test
    public void toModularNumberIllegalChar()
    {
        final String s = "123456789ABCDEFG";
        assertThrows(IllegalArgumentException.class, () -> ModNumber.stomn(s, 16));
    }
    @Test
    public void toModularNumberHexForEmptyString()
    {
        final String s = "";
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = ModNumber.stomn(s, 16);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberHexForOne()
    {
        final String s = "1";
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = ModNumber.stomn(s, 16);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberHexForSixteen()
    {
        final String s = "10";
        ModNumber mexp = new ModNumber(16L);
        ModNumber mres = ModNumber.stomn(s, 16);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberHexForSixteenWithLeadingZeros()
    {
        final String s = "000000000000000000000000010";
        ModNumber mexp = new ModNumber(16L);
        ModNumber mres = ModNumber.stomn(s, 16);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberHexForSixteenWithLeadingMinusSign()
    {
        final String s = "-10";
        assertThrows(IllegalArgumentException.class, () -> ModNumber.stomn(s, 16));
    }
    @Test
    public void toModularNumberHexForSixteenWithLeadingPlusSign()
    {
        final String s = "+10";
        ModNumber mexp = new ModNumber(16L);
        ModNumber mres = ModNumber.stomn(s, 16);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberHexForMax()
    {
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            exp[i] = ~0L;
        }
        ModNumber mexp = new ModNumber(exp);
        String str = "F".repeat(ModNumber.HexStringLength);
        ModNumber mres = ModNumber.stomn(str, 16);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberHexIncreasingSequenceByteInput()
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
            String s = String.format("%02X", (ModNumber.HexStringLength % 32) / 2 - i - 1);
            stringBuilder.append(s);
        }
        for (int i = ModNumber.HexStringLength % 32; i < ModNumber.HexStringLength; i += 32)
        {
            stringBuilder.append("0F0E0D0C0B0A09080706050403020100");
        }
        ModNumber mres = ModNumber.stomn(stringBuilder.toString(), 16);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberHexIncreasingSequence()
    {
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i + 1 < ModNumber.LCOUNT; i+=2)
        {
            exp[i] = 0x0706050403020100L;
            exp[i + 1] = 0x0F0E0D0C0B0A0908L;
        }
        ModNumber mexp = new ModNumber(exp);
        StringBuilder stringBuilder = new StringBuilder(ModNumber.HexStringLength);
        for (int i = 0; i  < ModNumber.HexStringLength % 32; i++)
        {
            stringBuilder.append("0");
        }
        for (int i = ModNumber.HexStringLength % 32; i < ModNumber.HexStringLength; i += 32)
        {
            stringBuilder.append("0F0E0D0C0B0A09080706050403020100");
        }
        ModNumber mres = ModNumber.stomn(stringBuilder.toString(), 16);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberHexIncreasingSequenceSwitched()
    {
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i + 1 < ModNumber.LCOUNT; i+=2)
        {
            exp[i] = 0x08090A0B0C0D0E0FL;
            exp[i + 1] = 0x0001020304050607L;
        }
        ModNumber mexp = new ModNumber(exp);
        StringBuilder stringBuilder = new StringBuilder(ModNumber.HexStringLength);
        for (int i = 0; i  < ModNumber.HexStringLength % 32; i++)
        {
            stringBuilder.append("0");
        }
        for (int i = ModNumber.HexStringLength % 32; i < ModNumber.HexStringLength; i += 32)
        {
            stringBuilder.append("000102030405060708090A0B0C0D0E0F");
        }
        ModNumber mres = ModNumber.stomn(stringBuilder.toString(), 16);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberDecimalForEmptyString()
    {
        final String s = "";
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = ModNumber.stomn(s, 10);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberDecimalIllegalChar()
    {
        final String s = "123456789A";
        assertThrows(IllegalArgumentException.class, () -> ModNumber.stomn(s, 10));
    }
    @Test
    public void toModularNumberDecimalWithLeadingSpaces()
    {
        final String s = "        9";
        ModNumber mexp = new ModNumber(9L);
        ModNumber mres = ModNumber.stomn(s, 10);
        assertEquals(mexp, mres);
    }

    @Test
    public void toModularNumberDecimalForOne()
    {
        final String s = "1";
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = ModNumber.stomn(s, 10);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberDecimalForTen()
    {
        final String s = "10";
        ModNumber mexp = new ModNumber(10L);
        ModNumber mres = ModNumber.stomn(s, 10);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberDecimalForTenWithLeadingZeros()
    {
        final String s = "000000000000000000000000010";
        ModNumber mexp = new ModNumber(10L);
        ModNumber mres = ModNumber.stomn(s, 10);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberDecimalForTenWithLeadingMinusSign()
    {
        final String s = "-10";
        assertThrows(IllegalArgumentException.class, () -> ModNumber.stomn(s, 10));
    }
    @Test
    public void toModularNumberDecimalForTenWithLeadingPlusSign()
    {
        final String s = "+10";
        ModNumber mexp = new ModNumber(10L);
        ModNumber mres = ModNumber.stomn(s, 10);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberDecimalForTenNines()
    {
        ModNumber mexp = new ModNumber(0x2540BE3FFL);
        StringBuilder s = new StringBuilder(ModNumber.DecimalStringLength);
        s.append("0".repeat(ModNumber.DecimalStringLength - 10));
        s.append("9".repeat(10));
        ModNumber mres = ModNumber.stomn(s.toString(), 10);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberDecimalForEighteenNines()
    {
        ModNumber mexp = new ModNumber(0xDE0B6B3A763FFFFL);
        StringBuilder s = new StringBuilder(ModNumber.DecimalStringLength);
        s.append("0".repeat(ModNumber.DecimalStringLength - 18));
        s.append("9".repeat(18));
        ModNumber mres = ModNumber.stomn(s.toString(), 10);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberOctalForEmptyString()
    {
        final String s = "";
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = ModNumber.stomn(s, 8);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberOctalIllegalChar()
    {
        final String s = "123456789A";
        assertThrows(IllegalArgumentException.class, () -> ModNumber.stomn(s, 8));
    }

    @Test
    public void toModularNumberOctalForOne()
    {
        final String s = "1";
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = ModNumber.stomn(s, 8);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberOctalForSixteen()
    {
        final String s = "20";
        ModNumber mexp = new ModNumber(16L);
        ModNumber mres = ModNumber.stomn(s, 8);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberOctalForEightWithLeadingZeros()
    {
        final String s = "000000000000000000000000010";
        ModNumber mexp = new ModNumber(8L);
        ModNumber mres = ModNumber.stomn(s, 8);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberOctalForMax()
    {
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            exp[i] = ~0L;
        }
        ModNumber mexp = new ModNumber(exp);
        StringBuilder s = new StringBuilder(ModNumber.OctalStringLength);
        s.append("7".repeat(ModNumber.OctalStringLength));
        switch (ModNumber.NSIZE % 3)
        {
            case 0:
                break;
            case 1:
                s.setCharAt(0, '1' );
                break;
            case 2:
                s.setCharAt(0, '3');
                break;
        }

        ModNumber mres = ModNumber.stomn(s.toString(), 8);
        assertEquals(mexp, mres);
    }
    @Test
    public void toModularNumberOctalForStringTooLarge()
    {
        StringBuilder s = new StringBuilder(ModNumber.OctalStringLength + 16);
        for (int i = 0; i < ModNumber.OctalStringLength + 1; i += 16)
            s.append("0706050403020100");
        assertThrows(IllegalArgumentException.class, () -> ModNumber.stomn(s.toString(), 8));
    }
    @Test
    public void toModularNumberOctalIncreasingSequenceByteInput()
    {
        StringBuilder s = new StringBuilder(ModNumber.OctalStringLength);
        s.append("0".repeat(ModNumber.OctalStringLength % 16));
        for (int i = ModNumber.OctalStringLength % 16; i < ModNumber.OctalStringLength; i += 16)
            s.append("0001020304050607");
        ModNumber mres = ModNumber.stomn(s.toString(), 8);
        String resStr = mres.toString(8);
        assertEquals(s.toString(), resStr);
    }

    @Test
    public void serializationHexForZero() {
        ModNumber mzero = new ModNumber(0L);
        ModNumber mexp = new ModNumber(0L);
        String filename = "TestSerializationHexForZero.txt";
        try {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            mzero.write(bw, 16);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 16);
            assertEquals(mexp, mres);
        } catch (IOException e) {
            fail();
        }
    }
    @Test
    public void serializationHexForOne()
    {
        ModNumber mone = new ModNumber(1L);
        ModNumber mexp = new ModNumber(1L);
        String filename = "TestSerializationHexForOne.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            mone.write(bw, 16);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 16);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }

    }
    @Test
    public void serializationHexForSixteen()
    {
        ModNumber ml = new ModNumber(16L);
        ModNumber mexp = new ModNumber(16L);
        String filename = "TestSerializationHexForSixteen.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            ml.write(bw, 16);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 16);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }

    }
    @Test
    public void serializationHexForAllFF()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
            exp[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);

        String filename = "TestSerializationHexForAllFF.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            ml.write(bw, 16);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 16);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }

    }
    @Test
    public void serializationHexForIncreasingSequence()
    {
        byte[] l = new byte[ModNumber.NCOUNT];
        byte[] exp = new byte[ModNumber.NCOUNT];
        for (int i = 0; i < ModNumber.NCOUNT; i++)
        {
            l[i] = (byte)i;
            exp[i] = (byte)i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);

        String filename = "TestSerializationHexForIncreasingSequence.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            ml.write(bw, 16);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 16);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }

    }
    @Test
    public void serializationOctForZero() {
        ModNumber mzero = new ModNumber(0L);
        ModNumber mexp = new ModNumber(0L);
        String filename = "TestSerializationOctForZero.txt";
        try {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            mzero.write(bw, 8);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 8);
            assertEquals(mexp, mres);
        } catch (IOException e) {
            fail();
        }
    }
    @Test
    public void serializationOctForOne()
    {
        ModNumber mone = new ModNumber(1L);
        ModNumber mexp = new ModNumber(1L);
        String filename = "TestSerializationOctForOne.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            mone.write(bw, 8);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 8);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }

    }
    @Test
    public void serializationOctForSixteen()
    {
        ModNumber ml = new ModNumber(16L);
        ModNumber mexp = new ModNumber(16L);
        String filename = "TestSerializationOctForSixteen.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            ml.write(bw, 8);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 8);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }

    }
    @Test
    public void serializationOctForAllFF()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
            exp[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);

        String filename = "TestSerializationOctForAllFF.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            ml.write(bw, 8);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 8);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }

    }
    @Test
    public void serializationOctForIncreasingSequence()
    {
        byte[] l = new byte[ModNumber.NCOUNT];
        byte[] exp = new byte[ModNumber.NCOUNT];
        for (int i = 0; i < ModNumber.NCOUNT; i++)
        {
            l[i] = (byte)i;
            exp[i] = (byte)i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);

        String filename = "TestSerializationOctForIncreasingSequence.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            ml.write(bw, 8);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 8);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }

    }
    @Test
    public void serializationDecForZero() {
        ModNumber mzero = new ModNumber(0L);
        ModNumber mexp = new ModNumber(0L);
        String filename = "TestSerializationDecForZero.txt";
        try {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            mzero.write(bw, 10);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 10);
            assertEquals(mexp, mres);
        } catch (IOException e) {
            fail();
        }
    }
    @Test
    public void serializationDecForOne()
    {
        ModNumber mone = new ModNumber(1L);
        ModNumber mexp = new ModNumber(1L);
        String filename = "TestSerializationDecForOne.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            mone.write(bw, 10);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 10);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }
    }
    @Test
    public void serializationDecForSixteen()
    {
        ModNumber ml = new ModNumber(16L);
        ModNumber mexp = new ModNumber(16L);
        String filename = "TestSerializationDecForSixteen.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            ml.write(bw, 10);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 10);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }
    }
    @Test
    public void serializationDecForAllFF()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
            exp[i] = ~0L;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);

        String filename = "TestSerializationDecForAllFF.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            ml.write(bw, 10);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 10);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }
    }
    @Test
    public void serializationDecForIncreasingSequence()
    {
        byte[] l = new byte[ModNumber.NCOUNT];
        byte[] exp = new byte[ModNumber.NCOUNT];
        for (int i = 0; i < ModNumber.NCOUNT; i++)
        {
            l[i] = (byte)i;
            exp[i] = (byte)i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(exp);

        String filename = "TestSerializationDecForIncreasingSequence.txt";
        try
        {
            FileWriter fw = new FileWriter(filename);
            BufferedWriter bw = new BufferedWriter(fw);

            ml.write(bw, 10);
            bw.close();
            FileReader fr = new FileReader(filename);
            BufferedReader br = new BufferedReader(fr);
            ModNumber mres = ModNumber.read(br, 10);
            assertEquals(mexp, mres);
        }
        catch (IOException   e) {
            fail();
        }
    }
    @Test
    public void multiplyByZero()
    {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
            l[i] = (long)i;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(0L);
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyByOne()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = (long) i;
            exp[i] = (long) i;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(1L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyByTwo()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = (long) i;
            exp[i] = (long) i * 2;
        }
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(2L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyAllFFByTwo()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT - 1; i++) {
            l[i] = ~0L;
            exp[i] = ~0L;
        }
        exp[ModNumber.LCOUNT - 1] = 1L;
        exp[0] -= 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(2L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyFsByTwoPower16()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[1] = ~0L >>> (ModNumber.LSIZE * 8 - 16);
        exp[0] = ~0L << 16;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(65536L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyAllFFByAllFF()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++) {
            l[i] = ~0L;
            r[i] = ~0L;
        }
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyEightsByTwo()
    {
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x1111111111111110L;
        exp[1] = 0x1L;
        ModNumber ml = new ModNumber(0x8888888888888888L);
        ModNumber mr = new ModNumber(2L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyNinesDecByNinesDec()
    {
        ModNumber ml = new ModNumber(9999999999999999L);
        ModNumber mr = new ModNumber(9999999999999999L);
        String exp = "99999999999999980000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyTwoBlockNinesDecByTwoBlockNinesDec()
    {
        String tmp = "9999999999999999";
        String l = tmp + tmp;
        String r = l;
        ModNumber ml = ModNumber.stomn(l, 10);
        ModNumber mr = ModNumber.stomn(r, 10);
        String exp = "9999999999999999999999999999999800000000000000000000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyFourBlockNinesDecByFourBlockNinesDec()
    {
        String tmp = "9999999999999999";
        String tmp2 = tmp + tmp;
        String l = tmp2 + tmp2;
        String r = l;
        ModNumber ml = ModNumber.stomn(l, 10);
        ModNumber mr = ModNumber.stomn(r, 10);
        String exp = "99999999999999999999999999999999999999999999999999999999999999980000000000000000000000000000000000000000000000000000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyEightBlockNinesDecByEightBlockNinesDec()
    {
        String tmp = "9999999999999999";
        String tmp2 = tmp + tmp;
        String tmp3 = tmp2 + tmp2;
        String l = tmp3 + tmp3;
        String r = l;
        ModNumber ml = ModNumber.stomn(l, 10);
        ModNumber mr = ModNumber.stomn(r, 10);
        String exp = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = ModNumber.product(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplySixteenBlockNinesDecBySixteenBlockNinesDec()
    {
        if (ModNumber.MaxMod > 1024 / 8) {
            String tmp = "9999999999999999";

            String tmp2 = tmp + tmp;
            String tmp3 = tmp2 + tmp2;
            String tmp4 = tmp3 + tmp3;
            String l = tmp4 + tmp4;
            String r = l;
            ModNumber ml = ModNumber.stomn(l, 10);
            ModNumber mr = ModNumber.stomn(r, 10);
            String exp = "99999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999980000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001";
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = ModNumber.product(ml, mr);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void multiplyThirtyOneBlockNinesDecByThirtyOneBlockNinesDec()
    {
        if (ModNumber.MaxMod == 4096 / 8) {
            String tmp1 = "9999999999999999";

            String tmp2 = tmp1 + tmp1;
            String tmp3 = tmp2 + tmp2;
            String tmp4 = tmp3 + tmp3;
            String tmp5 = tmp4 + tmp4;
            String l =tmp5 + tmp4 + tmp3 + tmp2 + tmp1;
            String r = l;
            ModNumber ml = ModNumber.stomn(l, 10);
            ModNumber mr = ModNumber.stomn(r, 10);
            String exp = "99999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999980000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001";
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = ModNumber.product(ml, mr);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void multiplyThirtyTwoBlockNinesDecByThirtyTwoBlockNinesDec()
    {
        if (ModNumber.MaxMod == 4096 / 8) {
            String tmp1 = "9999999999999999";

            String tmp2 = tmp1 + tmp1;
            String tmp3 = tmp2 + tmp2;
            String tmp4 = tmp3 + tmp3;
            String tmp5 = tmp4 + tmp4;
            String l =tmp5 + tmp5;
            String r = l;
            ModNumber ml = ModNumber.stomn(l, 10);
            ModNumber mr = ModNumber.stomn(r, 10);
            String exp = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001";
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = ModNumber.product(ml, mr);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void multGroupModAboveMax()
    {
        long[] n = new long[ModNumber.LCOUNT];
        n[ModNumber.COUNTMOD] = 1L;
        final ModNumber mn = new ModNumber(n);
        assertThrows(IllegalArgumentException.class,() -> new MultGroupMod(mn));
    }
    @Test
    public void multiplyMultGroupModByZero()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++)
        {
            l[i] = (long)i;
            n[i] = (long)i;
        }
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(0L);
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModByOne()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++)
        {
            l[i] = (long)i;
            n[i] = (long)i;
        }
        n[0] += 1;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(1L);
        ModNumber mexp = new ModNumber(ml);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModByTwoResultEqMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++)
        {
            l[i] = (long)i;
            n[i] = (long)i * 2;
        }
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(2L);
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModByTwoResultLessMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++)
        {
            l[i] = (long)i;
            n[i] = (long)i * 2;
            exp[i] = (long)i * 2;
        }
        n[0] += 1;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(2L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModAllFFFFByTwoResultEqMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++)
        {
            l[i] = ~0L;
            n[i] = ~0L;
        }
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(2L);
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModAllFFFFByTwoResultLessMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD - 1; i++)
        {
            l[i] = ~0L;
            n[i] = ~0L;
            exp[i] = ~0L;
        }
        n[ModNumber.COUNTMOD - 1] = 1L;
        exp[ModNumber.COUNTMOD - 1] = 1L;
        exp[0] -= 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(2L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModFsByPow16ResultLessMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        n[2] = 1L;
        exp[1] = ~0L >>> (ModNumber.LSIZE * 8 - 16);
        exp[0] = ~0L << 16;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(65536L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModFsByFsResultModOne3rdBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[0] = ~0L;
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        n[2] = 1L;
        exp[1] = ~0L - 1L;
        exp[0] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModFsByFsResultModOne2ndBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[0] = ~0L;
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        n[1] = 1L;
        exp[0] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModFsByFsResultModEs1thBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[0] = ~0L;
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        n[0] = 0xEEEEEEEEEEEEEEEEL;
        exp[0] = 0x1111111111111111L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModAllFsByAllFsResultLessMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++)
        {
            l[i] = ~0L;
            r[i] = ~0L;
        }
        long[] n = new long[ModNumber.LCOUNT];
        n[ModNumber.LCOUNT - 3] = 1L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModAllFsByAllFsResultGreaterMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++)
        {
            l[i] = ~0L;
            r[i] = ~0L;
        }
        long[] n = new long[ModNumber.LCOUNT];
        n[1] = 1L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupMod8sBy2ResultLessMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = 0x8888888888888888L;
        long[] r = new long[ModNumber.LCOUNT];
        r[0] = 2L;
        long[] n = new long[ModNumber.LCOUNT];
        n[2] = 1L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x1111111111111110L;
        exp[1] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupMod8sBy2ResultGreaterMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = 0x8888888888888888L;
        long[] r = new long[ModNumber.LCOUNT];
        r[0] = 2L;
        long[] n = new long[ModNumber.LCOUNT];
        n[1] = 1L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x1111111111111110L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupMod9sDecBy9sDecResultLessMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[0] = 9999999999999999L;
        long[] r = new long[ModNumber.LCOUNT];
        r[0] = 9999999999999999L;
        long[] n = new long[ModNumber.LCOUNT];
        n[4] = 1L;
        String exp = "99999999999999980000000000000001";
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModTwoBlock9sDecByTwoBlock9sDecResultLessMod()
    {
        String lstr = "9999999999999999";
        lstr += lstr;
        String rstr = lstr;
        long[] n = new long[ModNumber.LCOUNT];
        n[4] = 1L;
        String exp = "9999999999999999";
        exp += "9999999999999998";
        exp += "0000000000000000";
        exp += "0000000000000001";
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mr = ModNumber.stomn(rstr, 10);
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModTwoBlock9sDecByTwoBlock9sDecResultGreaterMod()
    {
        String lstr = "9999999999999999";
        lstr += lstr;
        String rstr = lstr;
        String nstr = "10000000000000000";
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 1L;
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mr = ModNumber.stomn(rstr, 10);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModFourBlock9sDecByFourBlock9sDecResultLessMod()
    {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        String rstr = lstr;
        long[] n = new long[ModNumber.LCOUNT];
        n[8] = 1L;
        String exp = "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999998";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000001";
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mr = ModNumber.stomn(rstr, 10);
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModFourBlock9sDecByFourBlock9sDecResultGreaterMod()
    {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        String rstr = lstr;
        String nstr = "10000000000000000";
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 1L;
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mr = ModNumber.stomn(rstr, 10);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModEightBlock9sDecByEightBlock9sDecResultLessMod()
    {
        if (ModNumber.MaxMod >= 2048 /8) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            String rstr = lstr;
            long[] n = new long[ModNumber.LCOUNT];
            n[16] = 1L;
            String exp = "9999999999999999";
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
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber mr = ModNumber.stomn(rstr, 10);
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = mgm.Mult(ml, mr);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void multiplyMultGroupModEightBlock9sDecByEightBlock9sDecResultGreaterMod()
    {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        String rstr = lstr;
        String nstr = "10000000000000000";
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 1L;
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mr = ModNumber.stomn(rstr, 10);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModSixteenBlock9sDecBySixteenBlock9sDecResultLessMod()
    {
        if (ModNumber.MaxMod >= 4096 /8) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            String rstr = lstr;
            long[] n = new long[ModNumber.LCOUNT];
            n[32] = 1L;
            String exp = "9999999999999999";
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
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber mr = ModNumber.stomn(rstr, 10);
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = mgm.Mult(ml, mr);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void multiplyMultGroupModSixteenBlock9sDecBySixteenBlock9sDecResultGreaterMod()
    {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        String rstr = lstr;
        String nstr = "10000000000000000";
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 1L;
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mr = ModNumber.stomn(rstr, 10);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Mult(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void multiplyMultGroupModThirtyOneBlock9sDecByThirtyOneBlock9sDecResultLessMod()
    {
        if (ModNumber.MaxMod >= 4096 /8) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.substring(0, ModNumber.NCOUNT - 24);
            String rstr = lstr;
            long[] n = new long[ModNumber.LCOUNT];
            n[ModNumber.LCOUNT - 2] = 1L;
            String exp = "";
            for (int i = 0; i < 30; i++)
                exp += "9999999999999999";
            exp += "9999999999999998";
            for (int i = 0; i < 30; i++)
                exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber mr = ModNumber.stomn(rstr, 10);
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = mgm.Mult(ml, mr);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void multiplyMultGroupModThirtyOneBlock9sDecByThirtyOneBlock9sDecResultGreaterMod()
    {
        if (ModNumber.MaxMod >= 2048 /8) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.substring(0, ModNumber.NCOUNT - 24);
            String rstr = lstr;
            String nstr = "10000000000000000";
            long[] exp = new long[ModNumber.LCOUNT];
            exp[0] = 1L;
            ModNumber mn = ModNumber.stomn(nstr, 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber mr = ModNumber.stomn(rstr, 10);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void multiplyMultGroupModThirtyTwoBlock9sDecByThirtyTwoBlock9sDecResultGreaterMod()
    {
        if (ModNumber.MaxMod >= 2048 /8) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            String rstr = lstr;
            String nstr = "10000000000000000";
            long[] exp = new long[ModNumber.LCOUNT];
            exp[0] = 1L;
            ModNumber mn = ModNumber.stomn(nstr, 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber mr = ModNumber.stomn(rstr, 10);
            ModNumber mexp = new ModNumber(exp);
            ModNumber mres = mgm.Mult(ml, mr);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void gcdOfOneAndZero()
    {
        ModNumber mzero = new ModNumber(0L);
        ModNumber mone = new ModNumber(1L);
        assertThrows(IllegalArgumentException.class,() -> ModNumber.gcd(mone, mzero));
    }
    @Test
    public void gcdOfZeroAndOne()
    {
        ModNumber mzero = new ModNumber(0L);
        ModNumber mone = new ModNumber(1L);
        assertThrows(IllegalArgumentException.class,() -> ModNumber.gcd(mzero, mone));
    }
    @Test
    public void gcdOf100OneAnd()
    {
        ModNumber monehundred = new ModNumber(100L);
        ModNumber mone = new ModNumber(1L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = ModNumber.gcd(monehundred, mone);
        assertEquals(mexp, mres);
    }

    @Test
    public void gcdOfOneAnd100()
    {
        ModNumber monehundred = new ModNumber(100L);
        ModNumber mone = new ModNumber(1L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = ModNumber.gcd(mone, monehundred);
        assertEquals(mexp, mres);
    }
    @Test
    public void gcdOf101And100()
    {
        ModNumber monehundred = new ModNumber(100L);
        ModNumber monehundredandone = new ModNumber(101L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = ModNumber.gcd(monehundredandone, monehundred);
        assertEquals(mexp, mres);
    }
    @Test
    public void gcdOf102And100()
    {
        ModNumber monehundred = new ModNumber(100L);
        ModNumber monehundredandtwo = new ModNumber(102L);
        ModNumber mexp = new ModNumber(2L);
        ModNumber mres = ModNumber.gcd(monehundredandtwo, monehundred);
        assertEquals(mexp, mres);
    }
    @Test
    public void gcdOf100And102()
    {
        ModNumber monehundred = new ModNumber(100L);
        ModNumber monehundredandtwo = new ModNumber(102L);
        ModNumber mexp = new ModNumber(2L);
        ModNumber mres = ModNumber.gcd(monehundred, monehundredandtwo);
        assertEquals(mexp, mres);
    }
    @Test
    public void gcdOf400And600()
    {
        ModNumber mfourhundred = new ModNumber(400L);
        ModNumber msixhundred = new ModNumber(600L);
        ModNumber mexp = new ModNumber(200L);
        ModNumber mres = ModNumber.gcd(mfourhundred, msixhundred);
        assertEquals(mexp, mres);
    }
    @Test
    public void gcdOfAllFin2ndBlockAndAllFin1thBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[1] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[0] = ~0L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.gcd(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void gcdOfAllFinLastBlockAndAllFin2ndBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[1] = ~0L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[1] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.gcd(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void gcdOfAllFinLastBlockAndAllAin3rdBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[2] = 0xaaaaaaaaaaaaaaaaL;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[2] = 0xaaaaaaaaaaaaaaaaL;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.gcd(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void gcdOfAllFinLastBlockAndAllBin3rdBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[2] = 0xbbbbbbbbbbbbbbbbL;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[2] = 0x1111111111111111L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.gcd(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void lcmOf101And100()
    {
        ModNumber monehundred = new ModNumber(100L);
        ModNumber monehundredandone = new ModNumber(101L);
        ModNumber mexp = new ModNumber(10100L);
        ModNumber mres = ModNumber.lcm(monehundredandone, monehundred);
        assertEquals(mexp, mres);
    }
    @Test
    public void lcmOf102And100()
    {
        ModNumber monehundred = new ModNumber(100L);
        ModNumber monehundredandtwo = new ModNumber(102L);
        ModNumber mexp = new ModNumber(5100L);
        ModNumber mres = ModNumber.lcm(monehundredandtwo, monehundred);
        assertEquals(mexp, mres);
    }
    @Test
    public void lcmOf100And102()
    {
        ModNumber monehundred = new ModNumber(100L);
        ModNumber monehundredandtwo = new ModNumber(102L);
        ModNumber mexp = new ModNumber(5100L);
        ModNumber mres = ModNumber.lcm(monehundred,monehundredandtwo );
        assertEquals(mexp, mres);
    }
    @Test
    public void lcmOf400And600()
    {
        ModNumber msixhundred = new ModNumber(600L);
        ModNumber mfourhundred = new ModNumber(400L);
        ModNumber mexp = new ModNumber(1200L);
        ModNumber mres = ModNumber.lcm(mfourhundred,msixhundred );
        assertEquals(mexp, mres);
    }
    @Test
    public void lcmOfAllFin2ndBlockAndAllFin1thBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[1] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[0] = ~0L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[1] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.lcm(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void lcmOfAllFinLastBlockAndAllFin2ndBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[1] = ~0L;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[ModNumber.LCOUNT - 1] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.lcm(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void lcmOfAllFinLastBlockAndAllAin3rdBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[2] = 0xaaaaaaaaaaaaaaaaL;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[ModNumber.LCOUNT - 1] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.lcm(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void lcmOfAllFinLastBlockAndAllBin3rdBlock()
    {
        long[] l = new long[ModNumber.LCOUNT];
        l[ModNumber.LCOUNT - 1] = ~0L;
        long[] r = new long[ModNumber.LCOUNT];
        r[2] = 0xbbbbbbbbbbbbbbbbL;
        long[] exp = new long[ModNumber.LCOUNT];
        exp[ModNumber.LCOUNT - 1] = 0xfffffffffffffff5L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.lcm(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOfZero()
    {
        ModNumber mn = new ModNumber(0L);
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOfOne()
    {
        ModNumber mn = new ModNumber(1L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOfFour()
    {
        ModNumber mn = new ModNumber(4L);
        ModNumber mexp = new ModNumber(2L);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOfTwentyFive()
    {
        ModNumber mn = new ModNumber(25L);
        ModNumber mexp = new ModNumber(5L);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOfOneHundredAndSixtyNine()
    {
        ModNumber mn = new ModNumber(169L);
        ModNumber mexp = new ModNumber(13L);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOf152399025()
    {
        ModNumber mn = new ModNumber(152399025L);
        ModNumber mexp = new ModNumber(12345L);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOf1524157875019052100()
    {
        ModNumber mn = new ModNumber(1524157875019052100L);
        ModNumber mexp = new ModNumber(1234567890L);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOf152415787532374345526722756()
    {
        ModNumber mn = ModNumber.stomn("152415787532374345526722756", 10);
        ModNumber mexp = new ModNumber(12345678901234L);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOfAllFsSquared()
    {
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        int numInts = ModNumber.LCOUNT % 2 == 0 ? ModNumber.LCOUNT : ModNumber.LCOUNT - 1;
        for (int i = numInts / 2; i < numInts; i++)
            n[i] = ~0L;
        n[numInts / 2] <<= 1;
        n[0] = 1;
        for (int i = 0; i < numInts / 2; i++)
            exp[i] = ~0L;
        ModNumber mn = new ModNumber(n);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtOf1InLastWord()
    {
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        int numInts = ModNumber.LCOUNT % 2 == 1 ? ModNumber.LCOUNT : ModNumber.LCOUNT - 1;
        n[numInts-1] = 1;
        exp[(numInts -1 )/2] = 1L;
        ModNumber mn = new ModNumber(n);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mn.sqrt();
        assertEquals(mexp, mres);
    }
    @Test
    public void sqrtPrecision18Of2StrHex()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 18, false);
        String exp = "0".repeat(ModNumber.HexStringLength - 19);
        exp += "1.6A09E667F3BCC908B2";
        ScaledNumber snres = sn.sqrt();
        String resStr = snres.toString(16);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision18Of2StrDec()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 18, false);
        ScaledNumber snres = sn.sqrt();
        int integerStringLength = snres.calculateDecimalStringLengthLeft();
        String exp = "0".repeat(integerStringLength - 1);
        exp += "1.414213562373095048";
        assertEquals(18/2, snres.scale);
        String resStr = snres.toString(10);
        assertEquals(exp, resStr);
    }

}
