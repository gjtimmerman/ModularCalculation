package modularcalculation;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;

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
    public void TestModuloDivideProductOfPrimesByBothPrimesAndByBothPrimesMinusOne()
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

}