package modularcalculation;

import org.bouncycastle.jce.provider.BouncyCastleProvider;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import java.io.*;
import java.math.BigInteger;
import java.security.*;
import java.security.interfaces.*;
import java.security.spec.*;
import java.util.Arrays;
import java.util.List;


import static org.junit.jupiter.api.Assertions.*;

public class ModularCalculationTest {
    KeyStore myKeyStore;
//    @BeforeAll
//    public void initalizeAll()
//    {
//        try {
//            myKeyStore = KeyStore.getInstance(KeyStore.getDefaultType());
//
//        }
//        catch (KeyStoreException e)
//        {
//            e.printStackTrace();
//        }
//    }
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
        StringLengthResult stringLengthResult = sn.calculateOctalStringLength();
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
        StringLengthResult stringLengthResult = sn.calculateOctalStringLength();
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
        StringLengthResult stringLengthResult = sn.calculateOctalStringLength();
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
    public void sqrtPrecision18Of2()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 18, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x09e667f3bcc908b2L;
        exp[1] = 0x016aL;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 18/2,true);
        assertEquals(snexp, snres);
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
    @Test
    public void sqrtPrecision18Of2StrOctal()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 18, false);
        ScaledNumber snres = sn.sqrt();
        StringLengthResult stringLengthResult = snres.calculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft() - 1);
        exp += "1.324047463177167462204262";
        assertEquals(18/2, snres.scale);
        String resStr = snres.toString(8);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision16Of2()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 16, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x6a09e667f3bcc908L;
        exp[1] = 0x01L;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 16/2,true);
        assertEquals(snexp, snres);
    }

    @Test
    public void sqrtPrecision16Of2StrHex()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 16, false);
        String exp = "0".repeat(ModNumber.HexStringLength - 17);
        exp += "1.6A09E667F3BCC908";
        ScaledNumber snres = sn.sqrt();
        String resStr = snres.toString(16);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision16Of2StrDec()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 16, false);
        ScaledNumber snres = sn.sqrt();
        int integerStringLength = snres.calculateDecimalStringLengthLeft();
        String exp = "0".repeat(integerStringLength - 1);
        exp += "1.4142135623730950";
        assertEquals(16/2, snres.scale);
        String resStr = snres.toString(10);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision16Of2StrOctal()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 16, false);
        ScaledNumber snres = sn.sqrt();
        StringLengthResult stringLengthResult = snres.calculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft() - 1);
        exp += "1.3240474631771674622040";
        assertEquals(16/2, snres.scale);
        String resStr = snres.toString(8);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision14Of2()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 14, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x016a09e667f3bcc9L;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 14/2,true);
        assertEquals(snexp, snres);
    }

    @Test
    public void sqrtPrecision14Of2StrHex()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 14, false);
        String exp = "0".repeat(ModNumber.HexStringLength - 15);
        exp += "1.6A09E667F3BCC9";
        ScaledNumber snres = sn.sqrt();
        String resStr = snres.toString(16);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision14Of2StrDec()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 14, false);
        ScaledNumber snres = sn.sqrt();
        int integerStringLength = snres.calculateDecimalStringLengthLeft();
        String exp = "0".repeat(integerStringLength - 1);
        exp += "1.41421356237309";
        assertEquals(14/2, snres.scale);
        String resStr = snres.toString(10);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision14Of2StrOctal()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 14, false);
        ScaledNumber snres = sn.sqrt();
        StringLengthResult stringLengthResult = snres.calculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft() - 1);
        exp += "1.3240474631771674622";
        assertEquals(14/2, snres.scale);
        String resStr = snres.toString(8);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision12Of2()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 12, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x016a09e667f3bcL;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 12/2,true);
        assertEquals(snexp, snres);
    }

    @Test
    public void sqrtPrecision12Of2StrHex()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 12, false);
        String exp = "0".repeat(ModNumber.HexStringLength - 13);
        exp += "1.6A09E667F3BC";
        ScaledNumber snres = sn.sqrt();
        String resStr = snres.toString(16);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision12Of2StrDec()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 12, false);
        ScaledNumber snres = sn.sqrt();
        int integerStringLength = snres.calculateDecimalStringLengthLeft();
        String exp = "0".repeat(integerStringLength - 1);
        exp += "1.414213562373";
        assertEquals(12/2, snres.scale);
        String resStr = snres.toString(10);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision12Of2StrOctal()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 12, false);
        ScaledNumber snres = sn.sqrt();
        StringLengthResult stringLengthResult = snres.calculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft() - 1);
        exp += "1.3240474631771674";
        assertEquals(12/2, snres.scale);
        String resStr = snres.toString(8);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision10Of2()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 10, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x016a09e667f3L;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 10/2,true);
        assertEquals(snexp, snres);
    }

    @Test
    public void sqrtPrecision10Of2StrHex()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 10, false);
        String exp = "0".repeat(ModNumber.HexStringLength - 11);
        exp += "1.6A09E667F3";
        ScaledNumber snres = sn.sqrt();
        String resStr = snres.toString(16);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision10Of2StrDec()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 10, false);
        ScaledNumber snres = sn.sqrt();
        int integerStringLength = snres.calculateDecimalStringLengthLeft();
        String exp = "0".repeat(integerStringLength - 1);
        exp += "1.4142135623";
        assertEquals(10/2, snres.scale);
        String resStr = snres.toString(10);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision10Of2StrOctal()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 10, false);
        ScaledNumber snres = sn.sqrt();
        StringLengthResult stringLengthResult = snres.calculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft() - 1);
        exp += "1.32404746317714";
        assertEquals(10/2, snres.scale);
        String resStr = snres.toString(8);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision8Of2()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x016a09e667L;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 8/2,true);
        assertEquals(snexp, snres);
    }

    @Test
    public void sqrtPrecision8Of2StrHex()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        String exp = "0".repeat(ModNumber.HexStringLength - 9);
        exp += "1.6A09E667";
        ScaledNumber snres = sn.sqrt();
        String resStr = snres.toString(16);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision8Of2StrDec()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        int integerStringLength = snres.calculateDecimalStringLengthLeft();
        String exp = "0".repeat(integerStringLength - 1);
        exp += "1.41421356";
        assertEquals(8/2, snres.scale);
        String resStr = snres.toString(10);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision8Of2StrOctal()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        StringLengthResult stringLengthResult = snres.calculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft() - 1);
        exp += "1.32404746316";
        assertEquals(8/2, snres.scale);
        String resStr = snres.toString(8);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision4Of2()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 4, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x016a09L;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 4/2,true);
        assertEquals(snexp, snres);
    }

    @Test
    public void sqrtPrecision4Of2StrHex()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 4, false);
        String exp = "0".repeat(ModNumber.HexStringLength - 5);
        exp += "1.6A09";
        ScaledNumber snres = sn.sqrt();
        String resStr = snres.toString(16);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision4Of2StrDec()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 4, false);
        ScaledNumber snres = sn.sqrt();
        int integerStringLength = snres.calculateDecimalStringLengthLeft();
        String exp = "0".repeat(integerStringLength - 1);
        exp += "1.4141";
        assertEquals(4/2, snres.scale);
        String resStr = snres.toString(10);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision4Of2StrOctal()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 4, false);
        ScaledNumber snres = sn.sqrt();
        StringLengthResult stringLengthResult = snres.calculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft() - 1);
        exp += "1.324044";
        assertEquals(4/2, snres.scale);
        String resStr = snres.toString(8);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision2Of2()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 2, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x016aL;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 2/2,true);
        assertEquals(snexp, snres);
    }

    @Test
    public void sqrtPrecision2Of2StrHex()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 2, false);
        String exp = "0".repeat(ModNumber.HexStringLength - 3);
        exp += "1.6A";
        ScaledNumber snres = sn.sqrt();
        String resStr = snres.toString(16);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision2Of2StrDec()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 2, false);
        ScaledNumber snres = sn.sqrt();
        int integerStringLength = snres.calculateDecimalStringLengthLeft();
        String exp = "0".repeat(integerStringLength - 1);
        exp += "1.41";
        assertEquals(2/2, snres.scale);
        String resStr = snres.toString(10);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision2Of2StrOctal()
    {
        ModNumber mn = new ModNumber(2L);
        ScaledNumber sn = new ScaledNumber(mn, 2, false);
        ScaledNumber snres = sn.sqrt();
        StringLengthResult stringLengthResult = snres.calculateOctalStringLength();
        String exp = "0".repeat(stringLengthResult.digitsLeft() - 1);
        exp += "1.324";
        assertEquals(2/2, snres.scale);
        String resStr = snres.toString(8);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision8Of3()
    {
        ModNumber mn = new ModNumber(3L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x01bb67ae85L;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 8/2,true);
        assertEquals(snexp, snres);
    }
    @Test
    public void sqrtPrecision8Of3StrHex()
    {
        ModNumber mn = new ModNumber(3L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        String exp = "0".repeat(ModNumber.HexStringLength - 9);
        exp += "1.BB67AE85";
        ScaledNumber snres = sn.sqrt();
        String resStr = snres.toString(16);
        assertEquals(exp, resStr);
    }
    @Test
    public void sqrtPrecision8Of5()
    {
        ModNumber mn = new ModNumber(5L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x023c6ef372L;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 8/2,true);
        assertEquals(snexp, snres);
    }
    @Test
    public void sqrtPrecision8Of7()
    {
        ModNumber mn = new ModNumber(7L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x02a54ff53aL;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 8/2,true);
        assertEquals(snexp, snres);
    }
    @Test
    public void sqrtPrecision8Of11()
    {
        ModNumber mn = new ModNumber(11L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x03510e527fL;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 8/2,true);
        assertEquals(snexp, snres);
    }
    @Test
    public void sqrtPrecision8Of13()
    {
        ModNumber mn = new ModNumber(13L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x039b05688cL;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 8/2,true);
        assertEquals(snexp, snres);
    }
    @Test
    public void sqrtPrecision8Of17()
    {
        ModNumber mn = new ModNumber(17L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x041f83d9abL;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 8/2,true);
        assertEquals(snexp, snres);
    }
    @Test
    public void sqrtPrecision8Of19()
    {
        ModNumber mn = new ModNumber(19L);
        ScaledNumber sn = new ScaledNumber(mn, 8, false);
        ScaledNumber snres = sn.sqrt();
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x045be0cd19L;
        ModNumber mexp = new ModNumber(exp);
        ScaledNumber snexp = new ScaledNumber(mexp, 8/2,true);
        assertEquals(snexp, snres);
    }
    @Test
    public void multGroupModOfZero() {
        ModNumber mzero = new ModNumber(0L);
        assertThrows(IllegalArgumentException.class, () -> new MultGroupMod(mzero));

    }
    @Test
    public void multGroupModOfOne() {
        ModNumber mone = new ModNumber(1L);
        assertThrows(IllegalArgumentException.class, () -> new MultGroupMod(mone));

    }
    @Test
    public void kwadMultGroupEqualMod() {
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
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = mgm.Kwad(ml);
        assertEquals(mexp, mres);
    }
    @Test
    public void kwadMultGroupLessMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        l[0] = 1L;
        n[0] = 2L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber mexp = new ModNumber(ml);
        ModNumber mres = mgm.Kwad(ml);
        assertEquals(mexp, mres);
    }
    @Test
    public void kwadMultGroupMod9sDecResultLessMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        l[0] = 9999999999999999L;
        n[4] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        String exp = "99999999999999980000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = mgm.Kwad(ml);
        assertEquals(mexp, mres);
    }
    @Test
    public void kwadMultGroupModTwoBlock9sDecResultLessMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        long[] n = new long[ModNumber.LCOUNT];
        n[4] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        String exp = "9999999999999999";
        exp += "9999999999999998";
        exp += "0000000000000000";
        exp += "0000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = mgm.Kwad(ml);
        assertEquals(mexp, mres);
    }
    @Test
    public void kwadMultGroupModTwoBlock9sDecResultGreaterMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        String nstr = "10000000000000000";
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mgm.Kwad(ml);
        assertEquals(mexp, mres);
    }
    @Test
    public void kwadMultGroupModFourBlock9sDecResultLessMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        long[] n = new long[ModNumber.LCOUNT];
        n[8] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        String exp = "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999998";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = mgm.Kwad(ml);
        assertEquals(mexp, mres);
    }
    @Test
    public void kwadMultGroupModFourBlock9sDecResultGreaterMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        String nstr = "10000000000000000";
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mgm.Kwad(ml);
        assertEquals(mexp, mres);
    }
    @Test
    public void kwadMultGroupModEightBlock9sDecResultLessMod() {
        if (ModNumber.MaxMod >= 2048) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            long[] n = new long[ModNumber.LCOUNT];
            n[16] = 1L;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
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
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = mgm.Kwad(ml);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void kwadMultGroupModEightBlock9sDecResultGreaterMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        String nstr = "10000000000000000";
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mgm.Kwad(ml);
        assertEquals(mexp, mres);
    }
    @Test
    public void kwadMultGroupModSixteenBlock9sDecResultLessMod() {
        if (ModNumber.MaxMod >= 4096) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            long[] n = new long[ModNumber.LCOUNT];
            n[32] = 1L;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
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
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = mgm.Kwad(ml);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void kwadMultGroupModSixteenBlock9sDecResultGreaterMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        String nstr = "10000000000000000";
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mgm.Kwad(ml);
        assertEquals(mexp, mres);
    }
    @Test
    public void kwadMultGroupModThirtyOneBlock9sDecResultLessMod() {
        if (ModNumber.MaxMod >= 4096) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.substring(0, 496);
            long[] n = new long[ModNumber.LCOUNT];
            n[ModNumber.LCOUNT-2] = 1L;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            String exp = "";
            for (int i = 0; i < 30; i++)
                exp += "9999999999999999";
            exp += "9999999999999998";
            for (int i = 0; i < 30; i++)
                exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = mgm.Kwad(ml);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void kwadMultGroupModThirtyOneBlock9sDecResultGreaterMod() {
        if (ModNumber.MaxMod >= 2048) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.substring(0, 496);
            String nstr = "10000000000000000";
            ModNumber mn = ModNumber.stomn(nstr, 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber mexp = new ModNumber(1L);
            ModNumber mres = mgm.Kwad(ml);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void kwadMultGroupModThirtyTwoBlock9sDecResultGreaterMod() {
        if (ModNumber.MaxMod >= 2048) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            String nstr = "10000000000000000";
            ModNumber mn = ModNumber.stomn(nstr, 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber mexp = new ModNumber(1L);
            ModNumber mres = mgm.Kwad(ml);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expZeroMultGroupAllOnesLessMod() {
        long[] l = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
            l[i] = ~0L;
        ModNumber mn = new ModNumber(1000L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber me = new ModNumber(0L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expOneMultGroupEqualMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++) {
            l[i] = i;
            n[i] = i;
        }
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber me = new ModNumber(1L);
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expOneMultGroupLessMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++) {
            l[i] = i;
            n[i] = i + 1;
        }
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber me = new ModNumber(1L);
        ModNumber mexp = new ModNumber(ml);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupEqualMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.COUNTMOD; i++) {
            l[i] = i;
            n[i] = i;
        }
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber me = new ModNumber(2L);
        ModNumber mexp = new ModNumber(0L);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupLessMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        l[0] = 1L;
        n[0] = 2L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber me = new ModNumber(2L);
        ModNumber mexp = new ModNumber(ml);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTenOneMultGroupLessMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        l[0] = 1L;
        n[0] = 2L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber me = new ModNumber(10L);
        ModNumber mexp = new ModNumber(ml);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupMod9sDecResultLessMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        l[0] = 9999999999999999L;
        n[4] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = new ModNumber(l);
        ModNumber me = new ModNumber(2L);
        String exp = "99999999999999980000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupModTwoBlock9sDecResultLessMod() {
        long[] n = new long[ModNumber.LCOUNT];
        String lstr = "9999999999999999";
        lstr += lstr;
        n[4] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber me = new ModNumber(2L);
        String exp = "9999999999999999";
        exp += "9999999999999998";
        exp += "0000000000000000";
        exp += "0000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupModTwoBlock9sDecResultGreaterMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        String nstr = "10000000000000000";
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber me = new ModNumber(2L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupModFourBlock9sDecResultLessMod() {
        long[] n = new long[ModNumber.LCOUNT];
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        n[8] = 1L;
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber me = new ModNumber(2L);
        String exp = "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999999";
        exp += "9999999999999998";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000000";
        exp += "0000000000000001";
        ModNumber mexp = ModNumber.stomn(exp, 10);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupModFourBlock9sDecResultGreaterMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        String nstr = "10000000000000000";
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber me = new ModNumber(2L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupModEightBlock9sDecResultLessMod() {
        if (ModNumber.MaxMod >= 2048/8) {
            long[] n = new long[ModNumber.LCOUNT];
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            n[16] = 1L;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber me = new ModNumber(2L);
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
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = mgm.Exp(ml, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expTwoMultGroupModEightBlock9sDecResultGreaterMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        String nstr = "10000000000000000";
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber me = new ModNumber(2L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupModSixteenBlock9sDecResultLessMod() {
        if (ModNumber.MaxMod >= 4096/8) {
            long[] n = new long[ModNumber.LCOUNT];
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            n[32] = 1L;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber me = new ModNumber(2L);
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
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = mgm.Exp(ml, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expTwoMultGroupModSixteenBlock9sDecResultGreaterMod() {
        String lstr = "9999999999999999";
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        lstr += lstr;
        String nstr = "10000000000000000";
        ModNumber mn = ModNumber.stomn(nstr, 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber ml = ModNumber.stomn(lstr, 10);
        ModNumber me = new ModNumber(2L);
        ModNumber mexp = new ModNumber(1L);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoMultGroupModThirtyOneBlock9sDecResultLessMod() {
        if (ModNumber.MaxMod >= 4096/8) {
            long[] n = new long[ModNumber.LCOUNT];
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.substring(0, 496);
            n[ModNumber.LCOUNT - 2] = 1L;
            ModNumber mn = new ModNumber(n);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber me = new ModNumber(2L);
            String exp = "";
            for (int i = 0; i < 30; i++)
                exp += "9999999999999999";
            exp += "9999999999999998";
            for (int i = 0; i < 30; i++)
                exp += "0000000000000000";
            exp += "0000000000000001";
            ModNumber mexp = ModNumber.stomn(exp, 10);
            ModNumber mres = mgm.Exp(ml, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expTwoMultGroupModThirtyOneBlock9sDecResultGreaterMod() {
        if (ModNumber.MaxMod >= 2048/8) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr = lstr.substring(0, 496);
            String nstr = "10000000000000000";
            ModNumber mn = ModNumber.stomn(nstr, 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber me = new ModNumber(2L);
            ModNumber mexp = new ModNumber(1L);
            ModNumber mres = mgm.Exp(ml, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expTwoMultGroupModThirtyTwoBlock9sDecResultGreaterMod() {
        if (ModNumber.MaxMod >= 2048/8) {
            String lstr = "9999999999999999";
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            lstr += lstr;
            String nstr = "10000000000000000";
            ModNumber mn = ModNumber.stomn(nstr, 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber ml = ModNumber.stomn(lstr, 10);
            ModNumber me = new ModNumber(2L);
            ModNumber mexp = new ModNumber(1L);
            ModNumber mres = mgm.Exp(ml, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expMultGroupModTwoPowerThirteenModTenThousand() {
        ModNumber ml = new ModNumber(2L);
        ModNumber mn = new ModNumber(10000L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber me = new ModNumber(13L);
        ModNumber mexp = new ModNumber(8192L);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expMultGroupModFirstBlockAllOnesPowerTwoLessMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        n[2] = 1L;
        exp[0] = 1L;
        exp[1] = ~0L - 1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber me = new ModNumber(2L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expMultGroupModFirstTwoBlockAllOnesPowerTwoLessMod() {
        long[] l = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        l[1] = ~0L;
        n[4] = 1L;
        exp[0] = 1L;
        exp[1] = 0L;
        exp[2] = ~0L - 1L;
        exp[3] = ~0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mn = new ModNumber(n);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber me = new ModNumber(2L);
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = mgm.Exp(ml, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void groupPropertiesGroupModEleven() {
        ModNumber mx = new ModNumber(2L);
        ModNumber mn = new ModNumber(11L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber me = new ModNumber(2L);
        ModNumber mexp = new ModNumber(4L);
        ModNumber mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
        ModNumber.addAssignScalar(me, 1);
        mexp = new ModNumber(8L);
        mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
        ModNumber.addAssignScalar(me, 1);
        mexp = new ModNumber(5L);
        mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
        ModNumber.addAssignScalar(me, 1);
        mexp = new ModNumber(10L);
        mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
        ModNumber.addAssignScalar(me, 1);
        mexp = new ModNumber(9L);
        mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
        ModNumber.addAssignScalar(me, 1);
        mexp = new ModNumber(7L);
        mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
        ModNumber.addAssignScalar(me, 1);
        mexp = new ModNumber(3L);
        mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
        ModNumber.addAssignScalar(me, 1);
        mexp = new ModNumber(6L);
        mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
        ModNumber.addAssignScalar(me, 1);
        mexp = new ModNumber(1L);
        mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void gcdLargeNumbers() {
        ModNumber mx = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        ModNumber my = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mres = ModNumber.gcd(mx, my);
        ModNumber mexp = new ModNumber(1L);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwoLargeNumberxModLargeNumbery() {
        ModNumber mx = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        ModNumber my = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp = ModNumber.stomn("113638967017082606674052656070205979468957500859029166099119331283109255964689", 10);
        MultGroupMod mgm = new MultGroupMod(my);
        ModNumber me = new ModNumber(2L);
        ModNumber mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expThreeLargeNumberxModLargeNumbery() {
        ModNumber mx = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        ModNumber my = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp = ModNumber.stomn("959159918151361352804382650352706011784068412944254732668580895529266851090113", 10);
        MultGroupMod mgm = new MultGroupMod(my);
        ModNumber me = new ModNumber(3L);
        ModNumber mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expFourLargeNumberxModLargeNumbery() {
        ModNumber mx = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        ModNumber my = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp = ModNumber.stomn("43252875649600596472804069974401733886601470807478913480533005345660321341646", 10);
        MultGroupMod mgm = new MultGroupMod(my);
        ModNumber me = new ModNumber(4L);
        ModNumber mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expFiveLargeNumberxModLargeNumbery() {
        ModNumber mx = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        ModNumber my = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp = ModNumber.stomn("161704454599373732186620154854028099753067612674910409272189542689295694371582", 10);
        MultGroupMod mgm = new MultGroupMod(my);
        ModNumber me = new ModNumber(5L);
        ModNumber mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expNineLargeNumberxModLargeNumbery() {
        ModNumber mx = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        ModNumber my = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp = ModNumber.stomn("229491963118798811250703753130022542480533480148820031152039444666272832238347", 10);
        MultGroupMod mgm = new MultGroupMod(my);
        ModNumber me = new ModNumber(9L);
        ModNumber mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expFourteenLargeNumberxModLargeNumbery() {
        ModNumber mx = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        ModNumber my = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp = ModNumber.stomn("512694095576449334310364050646143389495558145452890366434851422723699646134904", 10);
        MultGroupMod mgm = new MultGroupMod(my);
        ModNumber me = new ModNumber(14L);
        ModNumber mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void expTwentyThreeLargeNumberxModLargeNumbery() {
        ModNumber mx = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        ModNumber my = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp = ModNumber.stomn("762907244846308926487595589728323262016384923154412381938714858362885520672888", 10);
        MultGroupMod mgm = new MultGroupMod(my);
        ModNumber me = new ModNumber(23);
        ModNumber mres = mgm.Exp(mx, me);
        assertEquals(mexp, mres);
    }
    @Test
    public void hugeNumberxModHugeNumbery1() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("67885277291854912938587378017853691459496193677756552480553436557302300320101312718354512366504982574838958861888542718272904603102504548837304584105562307267284676071603679712110072004987020967899883804718561430292652099594457858875289933946756413441096538132475400931050009417996185648350176662387902186108621517311830327723099115016623424851268094682681323829709292907275341890111203138351101069715278419043245098727421641629718536436838918204954343308555448232382368388580601057201273701188613232106352300442738562367465251389869660183071271610913707341230465789657707599097586310256539211176484567156528847506639917990006758261522292135816044123533114985744902968420423817686515256802404411459420510055567668821773480759839945638865270345325327032377340654619518821029267132265318694685281072806300858177007288594225758039156380947826545771278835135058430286184244368381139666029873832564755473361825936566690845345140947317051183032581586950151989519809420014114182249129767248172760729288655451472427647241152820746675436044819097064699101275497442767142366134874787841654717249301013092609819939109466282790957714894233169284428494458512543531352888077862617075549676041201530943138909340541678084859095964562913434445958610945", 10);
            ModNumber my = ModNumber.stomn("134726165702296673370426386890283366245915523314703970967013679118347220188155182690330977225943173840726855105111482919931305956440257849275520470254003646345501461780481515668802531224809023461784856160970148295396834447238617115943625107572025345987403272485443712847152864396338847358873706819626414636709346223482144399011474564965089003187992678881573427552175300955270709084383966983460129498021945865772279351116095761583530346030439079292145399495672419313497076142325458060700017164710304404461962683345576478284228376073670571646356401162299439184862813044493573253596123455176468974714188846054112527108968938884292759762043717438585685314138246495541580159383671282098306977461412321590398143395408933484974952209631431062016316646415085049130090980939899431089399294190073420318461005695618030273786677969332954501892209068829746374066239722536639068395204248225513550233334961960928605977988464525551842393134197366398782490722703917222543927938228662256671720674599120818771544202337991100431921007170249357400706732178943189857754690035153719671012851905711621053028628593731588178463669914156357640449019966758018645637841011686702148326897940500377576684094649809133657149348148237929343755825113110373434920873754635", 10);
            ModNumber mexp = ModNumber.stomn("66840888410441760431839008872429674786419329636947418486460242561044919868053869971976464859438191265887896243222940201658401353337753300438215886148441339078216785708877835956692459219822002493884972356251586865104182347644159257068335173625268932546306734352968311916102854978342661710523530157238512450600724706170314071288375449948465578336724584198892103722466008047995367194272763845109028428306667446729034252388674119953811809593600161087191056187116971081114707753744857003498743463521691172355610382902837915916763124683800911463285129551385731843632347254835865654498537144919929763537704278897583679602329020894286001500521425302769641190605131509796677190963247464411791720659007910130977633339841264663201471449791485423151046301089758016752750326320380610060132161924754725633179932889317172096779389375107196462735828121003200602787404587478208782210959879844373884203461129396173132616162527958860997047993250049347599458141116967070554408128808648142489471544831872646010814913682539628004273766017428610725270687359846125158653414537710952528646717030923779398311379292718495568643730804690074849491305072524849361209346553174158616974009862637760501134418608607602714010438807696251258896729148547460000474915143690", 10);
            ModNumber mres = ModNumber.modulo(my, mx);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void hugeNumberxModHugeNumbery2() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("66840888410441760431839008872429674786419329636947418486460242561044919868053869971976464859438191265887896243222940201658401353337753300438215886148441339078216785708877835956692459219822002493884972356251586865104182347644159257068335173625268932546306734352968311916102854978342661710523530157238512450600724706170314071288375449948465578336724584198892103722466008047995367194272763845109028428306667446729034252388674119953811809593600161087191056187116971081114707753744857003498743463521691172355610382902837915916763124683800911463285129551385731843632347254835865654498537144919929763537704278897583679602329020894286001500521425302769641190605131509796677190963247464411791720659007910130977633339841264663201471449791485423151046301089758016752750326320380610060132161924754725633179932889317172096779389375107196462735828121003200602787404587478208782210959879844373884203461129396173132616162527958860997047993250049347599458141116967070554408128808648142489471544831872646010814913682539628004273766017428610725270687359846125158653414537710952528646717030923779398311379292718495568643730804690074849491305072524849361209346553174158616974009862637760501134418608607602714010438807696251258896729148547460000474915143690", 10);
            ModNumber my = ModNumber.stomn("67885277291854912938587378017853691459496193677756552480553436557302300320101312718354512366504982574838958861888542718272904603102504548837304584105562307267284676071603679712110072004987020967899883804718561430292652099594457858875289933946756413441096538132475400931050009417996185648350176662387902186108621517311830327723099115016623424851268094682681323829709292907275341890111203138351101069715278419043245098727421641629718536436838918204954343308555448232382368388580601057201273701188613232106352300442738562367465251389869660183071271610913707341230465789657707599097586310256539211176484567156528847506639917990006758261522292135816044123533114985744902968420423817686515256802404411459420510055567668821773480759839945638865270345325327032377340654619518821029267132265318694685281072806300858177007288594225758039156380947826545771278835135058430286184244368381139666029873832564755473361825936566690845345140947317051183032581586950151989519809420014114182249129767248172760729288655451472427647241152820746675436044819097064699101275497442767142366134874787841654717249301013092609819939109466282790957714894233169284428494458512543531352888077862617075549676041201530943138909340541678084859095964562913434445958610945", 10);
            ModNumber mexp = ModNumber.stomn("1044388881413152506748369145424016673076864040809133994093193996257380452047442746378047507066791308951062618665602516614503249764751248399088697957120968189067890362725843755417612785165018474014911448466974565188469751950298601806954760321487480894789803779507089014947154439653523937826646505149389735507896811141516256434723665068157846514543510483789220107243284859279974695838439293242072641408610972314210846338747521675906726843238757117763287121438477151267660634835744053702530237666922059750741917539900646450702126706068748719786142059527975497598118534821841944599049165336609447638780288258945167904310897095720756761000866833046402932927983475948225777457176353274723536143396501328442876715726404158572009310048460215714224044235569015624590328299138210969134970340563969052101139916983686080227899219118561576420552826823345168491430547580221503973284488536765781826412703168582340745663408607829848297147697267703583574440469983081435111680611365971692777584935375526749914374972911844423373475135392135950165357459250939540447860959731814613719417843864062256405870008294597041176208304776207941466409821708319923219147905338384914378878215224856574415257432593928229128470532845426825962366816015453433971043467255", 10);
            ModNumber mres = ModNumber.modulo(my, mx);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void gcdHugeNumbersTooLargeForModGroup() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("67885277291854912938587378017853691459496193677756552480553436557302300320101312718354512366504982574838958861888542718272904603102504548837304584105562307267284676071603679712110072004987020967899883804718561430292652099594457858875289933946756413441096538132475400931050009417996185648350176662387902186108621517311830327723099115016623424851268094682681323829709292907275341890111203138351101069715278419043245098727421641629718536436838918204954343308555448232382368388580601057201273701188613232106352300442738562367465251389869660183071271610913707341230465789657707599097586310256539211176484567156528847506639917990006758261522292135816044123533114985744902968420423817686515256802404411459420510055567668821773480759839945638865270345325327032377340654619518821029267132265318694685281072806300858177007288594225758039156380947826545771278835135058430286184244368381139666029873832564755473361825936566690845345140947317051183032581586950151989519809420014114182249129767248172760729288655451472427647241152820746675436044819097064699101275497442767142366134874787841654717249301013092609819939109466282790957714894233169284428494458512543531352888077862617075549676041201530943138909340541678084859095964562913434445958610945", 10);
            ModNumber my = ModNumber.stomn("134726165702296673370426386890283366245915523314703970967013679118347220188155182690330977225943173840726855105111482919931305956440257849275520470254003646345501461780481515668802531224809023461784856160970148295396834447238617115943625107572025345987403272485443712847152864396338847358873706819626414636709346223482144399011474564965089003187992678881573427552175300955270709084383966983460129498021945865772279351116095761583530346030439079292145399495672419313497076142325458060700017164710304404461962683345576478284228376073670571646356401162299439184862813044493573253596123455176468974714188846054112527108968938884292759762043717438585685314138246495541580159383671282098306977461412321590398143395408933484974952209631431062016316646415085049130090980939899431089399294190073420318461005695618030273786677969332954501892209068829746374066239722536639068395204248225513550233334961960928605977988464525551842393134197366398782490722703917222543927938228662256671720674599120818771544202337991100431921007170249357400706732178943189857754690035153719671012851905711621053028628593731588178463669914156357640449019966758018645637841011686702148326897940500377576684094649809133657149348148237929343755825113110373434920873754626", 10);
            ModNumber mexp = new ModNumber(1L);
            ModNumber mres = ModNumber.gcd(mx, my);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void gcdHugeNumbersFitForModGroup() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber my = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp = new ModNumber(1L);
            ModNumber mres = ModNumber.gcd(mx, my);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expTwoHugeNumberxModHugeNumbery() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber my = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp = ModNumber.stomn("5603130552131808582980129320151431593287387887422304019872729976542631800068788204319579898052711893227272328490995919630013482288843181272802592367798898524753249376917995287069518581823351056941728232218068059475906712986411367854982914146900624999167769127442072642482993253159299132482762489032243580550536702583738240633476465093340828218312246935203167115248772557253480443158870340161116689390922962852167364803089439715091390938186175699671994828460433891219975915334762748122803062377109012273998249380330075478906670900287087012812096397970441489185240044951680317263972659517703324836533035198756630319129921923499685884740053891793070603801214192199848233151436265254188036888028127957406260369087189527978590594475592951159744836132820316608295381151982751459808466074838264192706066312469551538268619096100114127505148129379927320373232121684138190307396104482210039324618282627066824207318147380670441172882452441196009256198174338127429977948883866986080614196689277860961306323052769429682104967016727143083349244333982457816038315667725514706061625905397835791561560354555405798114663550095985095263976338037301461279067122567382168347234118084620013800495920079606189028067751569874497346329975073", 10);
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(2L);
            ModNumber mres = mgm.Exp(mx, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expThreeHugeNumberxModHugeNumbery() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber my = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp = ModNumber.stomn("3250884643230911848040993900898243652249531513932618832535257978042483452600091491071448010142398100478042105296260000778740749600718579122776183390547427824993235206147742097168345798614754365320984010059029219682439536967146512822002373439785534655521802047121803418848012395656429274333098159369154311644233907570422616787627723475926083938361636819678257422496690100421348413270866873065352260789155886255064675768143173146339319044846435864434413724869138551196165618880665585280600177108055462531571760767361621253748226656844565125994418973371936230561763833035606088131573663901610503899191493774161996270592967697542825122532247346414087618074103430674872167210384103188364874320914572308032916218625415657467011179515861941549549325736647746798691179745994168171424216744438039562800153093515133179758626811333952347511710966151695995849060660173034462576806791324819774083226059537296440455387776103603089949810718851894140440522457547023362034434091138265699012346903498488388949729603118123456025773082476853228076714602761291459028567417244212052579099284228737307579246003530093230740016729330631060411372234970037396164109907373323398534755180276564538643086778729169896704343882921981536979169501836", 10);
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(3L);
            ModNumber mres = mgm.Exp(mx, me);
            assertEquals(mexp, mres);
        }
    }

    @Test
    public void expFourHugeNumberxModHugeNumbery() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber my = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp = ModNumber.stomn("3080268413759030384341460902419062434997721043480105848866779760898068393567503640358696717983838966080526546827579170602983272318149414856875115837808486303722109044485795416198041955727945032540579727215123999690418312813355477277356083910442602752355111136928004841925810195818970742970436174828277478215405315973827710555567088163666229286786112475168136234677576560906615486193027124434823092257664145072814066899657089093522999907680713320331963071391531429767880888740895725104586646572927945523938405860480632775436157861081467981082184225014600032040927373958816953213700926896334768412238419934449779068554365771989177623416767861333527128330993153583146562417568714073880278726400518735126270015465444477429492644113567415231915774290228343428776481054611704559704237363844385486009720675254300380072203334120524517665759003680100816318161530169334907964499853628607313832400971196369426432557562516135301618512093776568587542511257533363363369929170820208909917939777029863549314112729980339793894778468238622396334080091491340293772514425601232034375252947892247868057474710141942798184238957688757577035052595292194615849826082321587207703091038792505545536888544063897477993799779043563238023394522327", 10);
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(4L);
            ModNumber mres = mgm.Exp(mx, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expFiveHugeNumberxModHugeNumbery() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber my = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp = ModNumber.stomn("5638711490193321968095582821025837950605444226618375138154713166364105598945660174618956327722878472004540065631638033121477000705571056940596828925718926195830996924121418082037133433987071497831290651435693069630617850623228685502937852675280449423766186849426063726462699651472782151270116860694640035896326187491580654641826172411776043924735160566067399148505252115983531709173811670864367402770737994364134054792446295788679175027471719898343002506661556535699568207586460454047069502939531995819982412227908066828322909893757005530027454140572188979488150548883493656102634585283004388269040778035648844374061133441256485694130373153695441673018808503905692775382098592090808337455691437278762153452127026625199516921413226458123714150014291085888648479717581069331505291998640223669211538320227057005969174493070497899969544813875034244416118133376225061639085905224935975593017160559344785335531808006629894396222402748085358182197175776506465368280926618778577016465379764389402423788365712927902557021317967548482940328717296549354184303249524202412120909360058136253604395042854633260658145671789247912207096954542398140903123427771094175794366551714347951366259653252568398361177136455047293213701876695", 10);
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(5L);
            ModNumber mres = mgm.Exp(mx, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expNineHugeNumberxModHugeNumbery() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber my = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp = ModNumber.stomn("6526644289055956736858877068519403631010685992730245336265829496694821013414538499486146315974113032228573239209724596217544324651093152616548529250049120999940655678333315395885001604404430674368413549625865049250132470591380208475377609647874790029400479150205957827909009703086882670985349192731759896918001420494741246424227032373058800952891431686568886131564077128377252007511518433882952722716274723386681179475425448262171359024756092838125772314791606694286983792124323139563528579059823470875200334900733517999777734332795930037234961019202348402151535637919629289729883257595638888662496455827375541928600802304845756950166077359813701732089741989302650908674314893619941837738332749876333210736274793708466037828681236498813341528164998456696630564895982403091586560317432213911618061336210329685952960886797529954329889775089979546107202598911341674985812880959595722541767420107480885582726866206084550433759213495839929831622331550209479075977807000372939391980195376179776796433632330315367306866629459555355570369718663720700763486289795413786830649415088063124797019327343416815040790000844014394638687874096152492020345144138425194906563561203752366115273693820073660712451776789995538713644424100", 10);
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(9L);
            ModNumber mres = mgm.Exp(mx, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expFourteenHugeNumberxModHugeNumbery() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber my = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp = ModNumber.stomn("6039455007496128416719267954008821642644933296482285601007280340638849216598327471873836884804919649701022781562170836586122709784265816820825575096191834212185018926216348978275581175822063777722482497113140804514895036442142544430881667557825900078592252798965327258914990338583787210599073287488662673805301914377003451438375705278297648482094110689330320376315698672834604231446800836077962070330168937134865160500778390669728454718569045019256569641694928118011409355278801328133547165356294609337610812695213078600632123496284216488466986412259260415850761842086790286642444976955387290025589459443340299552333990449300537311605581899291492541042318438402365969580472495963876590196668452923761989123797416243373802832579730850755435214147890009913750329406321245312028874106538784143568655391804665025694862487948422738665059792179032606285837274072496216497163641530842989068220287114503203934200124660090185998949932049482606796394057023799906848265652644062953681718535513811220343255677529230782090689257816403377077217366329694164624949416603154344694634920762252017370512301667484263576212344489948933525466419603573310889134534166584747655988525572669221990755765536427726512600894509425046404685796958", 10);
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(14L);
            ModNumber mres = mgm.Exp(mx, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expTwentyThreeHugeNumberxModHugeNumbery() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber my = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp = ModNumber.stomn("1910558067664321416396680278112421351311128155508056072368901508695988642856892709484053518717865300498833651493446191878311109463208734254327768677827631662480165138464607173165845236905892018996909578572567001105120710562754908421918976912431771111407349953372447903465407154463492560142565541044022453166310527846151093678811771978142693883611628484850315390633830071712500390333553961144075202278876666115263024160232594658499786859669890239340154655186150173362284474209855367200301366985476358028405918824749647709592321300691313415640628857337008354181546978239419012926142357108578893452157019470600385662609894172793751782975445692878550393789212014876536630398912024388301225226840243278763130519406654670836585560317047461520560082897582974145444240845524666069689002899325216988866056408910609616722220391670072906927908320209990818476307264086217589874274441162728455864826783695133239286440580764813761080633506062001385591618688515524216363685876312727637894283515606949804806273854084400015421161488551836257404747281713334156197917858069802946835981863333047175489480127714118264832628367050972187495226968652928612714283155344928655133603656002771958262019707727171589382556221257208131632394242127", 10);
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(23L);
            ModNumber mres = mgm.Exp(mx, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void expThirtySevenHugeNumberxModHugeNumbery() {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber my = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp = ModNumber.stomn("242760452789498187560222116076891445988028458456992017072504963165600161058896002310018225049894657031384427443541488709065472392086152010601151272712242277440209738146286759300402140345293651269163161826881724438362631376103663291045572406831762311376767187296205676842143574047066926958813256927930059355363901562101377907285379657399980663671306578018989929389619907873845508787293871920494666862217995428202920280527033368535241047172354652925147293331298874308471570694445084800591737290938767976686091604826314011132217253235234948408366413966084924212867402650142726851051817862713774158899264782638102048541460003811706714953941909081746204720033432076260814059663672110883330201609933877960327204186047392396598121000669809974553616720356084495585767909271572461836652890116273387079425214687517318304739386849804331869959580502653626045661944656270423012246002104141776443539286778518133774966685946958812610957974544771791195158152619653737875223424316716008726667030501736269703567752542972625389592056282107982296394621743371100502549330863244492025351194876359303058058710155058508915698932387363209783346367328313138661688531785868590170495058360789323129007712138201241342346426980353430114603198996", 10);
            MultGroupMod mgm = new MultGroupMod(my);
            ModNumber me = new ModNumber(37L);
            ModNumber mres = mgm.Exp(mx, me);
            assertEquals(mexp, mres);
        }
    }
    @Test
    public void diffLGreaterRLessMod()
    {
        ModNumber ml = new ModNumber(100L);
        ModNumber mr = new ModNumber(90L);
        ModNumber mn = new ModNumber(110L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(10L);
        ModNumber mres = mgm.Diff(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void diffLGreaterRGreaterMod()
    {
        ModNumber ml = new ModNumber(100L);
        ModNumber mr = new ModNumber(90L);
        ModNumber mn = new ModNumber(60L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(10L);
        ModNumber mres = mgm.Diff(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void diffLlessRLessMod()
    {
        ModNumber ml = new ModNumber(90L);
        ModNumber mr = new ModNumber(100L);
        ModNumber mn = new ModNumber(110L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(100L);
        ModNumber mres = mgm.Diff(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void diffLlessRGreaterMod()
    {
        ModNumber ml = new ModNumber(90L);
        ModNumber mr = new ModNumber(100L);
        ModNumber mn = new ModNumber(60L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(50L);
        ModNumber mres = mgm.Diff(ml, mr);
        assertEquals(mexp, mres);
    }
    @Test
    public void inverseOfZero()
    {
        ModNumber mzero = new ModNumber(0L);
        ModNumber mtwo = new ModNumber(2L);
        MultGroupMod mgm = new MultGroupMod(mtwo);
        assertThrows(IllegalArgumentException.class, () -> mgm.Inverse(mzero));
    }
    @Test
    public void inverseThreeAndSevenModTwenty()
    {
        ModNumber mx = new ModNumber(3L);
        ModNumber mn = new ModNumber(20L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(7L);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp, mres);
    }
    @Test
    public void inverseSevenAndThreeModTwenty()
    {
        ModNumber mx = new ModNumber(7L);
        ModNumber mn = new ModNumber(20L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(3L);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp, mres);
    }
    @Test
    public void inverseTwoAndSixModEleven()
    {
        ModNumber mx = new ModNumber(2L);
        ModNumber mn = new ModNumber(11L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(6L);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp, mres);
    }
    @Test
    public void inverseFiveAndFiveModTwelve()
    {
        ModNumber mx = new ModNumber(5L);
        ModNumber mn = new ModNumber(12L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(5L);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp, mres);
    }
    @Test
    public void inverseSevenAndSevenModTwelve()
    {
        ModNumber mx = new ModNumber(7L);
        ModNumber mn = new ModNumber(12L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(7L);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp, mres);
    }
    @Test
    public void inverseElevenAndElevenModTwelve()
    {
        ModNumber mx = new ModNumber(11L);
        ModNumber mn = new ModNumber(12L);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mexp = new ModNumber(11L);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp, mres);
    }
    @Test
    public void inverseTwentyAndFifteen()
    {
        ModNumber mx = new ModNumber(15L);
        ModNumber mn = new ModNumber(20L);
        MultGroupMod mgm = new MultGroupMod(mn);
        assertThrows(IllegalArgumentException.class, () -> mgm.Inverse(mx));
    }
    @Test
    public void inverseOfLargeNumber1()
    {
        ModNumber mx = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        ModNumber mn = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp1 = ModNumber.stomn("990338159586064333264351853726399202032439273344500090039361795769103042626153", 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp1, mres);
        ModNumber product = mgm.Mult(mx, mres);
        ModNumber mexp2 = new ModNumber(1L);
        assertEquals(mexp2, product);
    }
    @Test
    public void inverseOfLargeNumber1Inverse()
    {
        ModNumber mx = ModNumber.stomn("990338159586064333264351853726399202032439273344500090039361795769103042626153", 10);
        ModNumber mn = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp1 = ModNumber.stomn("578960446186580977142963331984986262322713928121796301252124359127864509988867", 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp1, mres);
        ModNumber product = mgm.Mult(mx, mres);
        ModNumber mexp2 = new ModNumber(1L);
        assertEquals(mexp2, product);
    }
    @Test
    public void inverseOfLargeNumber2()
    {
        ModNumber mx = ModNumber.stomn("113638967017082606674052656070205979468957500859029166099119331283109255964689", 10);
        ModNumber mn = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp1 = ModNumber.stomn("916563989161488767790915831324982323599332376517448274768723301425613841973609", 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp1, mres);
        ModNumber product = mgm.Mult(mx, mres);
        ModNumber mexp2 = new ModNumber(1L);
        assertEquals(mexp2, product);
    }
    @Test
    public void inverseOfLargeNumber2Inverse()
    {
        ModNumber mx = ModNumber.stomn("916563989161488767790915831324982323599332376517448274768723301425613841973609", 10);
        ModNumber mn = ModNumber.stomn("1042128803135845758856078577225897936027981799787823433585206957451846378061825", 10);
        ModNumber mexp1 = ModNumber.stomn("113638967017082606674052656070205979468957500859029166099119331283109255964689", 10);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mres = mgm.Inverse(mx);
        assertEquals(mexp1, mres);
        ModNumber product = mgm.Mult(mx, mres);
        ModNumber mexp2 = new ModNumber(1L);
        assertEquals(mexp2, product);
    }
    @Test
    public void inverseOfHugeNumber1()
    {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            ModNumber mn = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp1 = ModNumber.stomn("7077938970216339316645445575285346683424941653237210340139516526256813509979606177021530849277742732364801030056254685283272967813034126654684370996868817665975637877200499989860765830543219806691253546378869794068270590583892833700153879570795112599739140513412553864621285534205670943364750922097762621895818680253818070884230571117040150011896031221708030875177847284891792635438074811694778675171964272643019638156031482913499567423251969964170693617285474555961748092945378333120890780361163560901493876218729987658462900795199003929229716100635333728591704523260412802193993472731422243051758869939898387278562993320264471554982496798519107136733940096336315302043051078369874122713882086967410973438379654109746059534714870547090057664281203709444590153178636867403107145141196420110304550068844058322769189735671101637680650707903615234635200908408342995823505703193770467699263824297761978530688995807637960633835113674050660862505648679547630512260845352364885624977257360366150205959045524481094456933930287176995345532173558521752894170223063256872758780959875487778634305779963228215875539131006201580009220506095752025694701229168899054377957651680451709904249939778196108148618292542907928847184822275", 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            assertEquals(mexp1, mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1L);
            assertEquals(mexp2, product);
        }
    }
    @Test
    public void inverseOfHugeNumber1Inverse()
    {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("7077938970216339316645445575285346683424941653237210340139516526256813509979606177021530849277742732364801030056254685283272967813034126654684370996868817665975637877200499989860765830543219806691253546378869794068270590583892833700153879570795112599739140513412553864621285534205670943364750922097762621895818680253818070884230571117040150011896031221708030875177847284891792635438074811694778675171964272643019638156031482913499567423251969964170693617285474555961748092945378333120890780361163560901493876218729987658462900795199003929229716100635333728591704523260412802193993472731422243051758869939898387278562993320264471554982496798519107136733940096336315302043051078369874122713882086967410973438379654109746059534714870547090057664281203709444590153178636867403107145141196420110304550068844058322769189735671101637680650707903615234635200908408342995823505703193770467699263824297761978530688995807637960633835113674050660862505648679547630512260845352364885624977257360366150205959045524481094456933930287176995345532173558521752894170223063256872758780959875487778634305779963228215875539131006201580009220506095752025694701229168899054377957651680451709904249939778196108148618292542907928847184822275", 10);
            ModNumber mn = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp1 = ModNumber.stomn("3623451821273106591798517489676606755578260143845296301950078023180769590730141282681101925449990633803712329620427964365194238395223564200797472596262529819986568646252079277462059253947924925578191375033606530406588179464358283303804024066598160166416546249888476055909758106454030300673970483814514121636550249054256463712295430209547798325012557638476505264072997146949677059505179107650701152116736177191476248721131146483387072427043160544696821279571719968759573415371470046368831500393803498355689605400972866503654045641726018511844763754643779438332896420729736800160415848314536197400092495105645793557849722836110349563096430758182052585270067831537444822914697881264514142796499119438662519790874601833966388961954352281510713032675495105141289394787132243620045180775271114567767776105235089149956335809833925051572948003409790245702402899511351167742717078049692412472084918583001296444904905031382535553468088776536194245183660463846810800236280508426858763790540100859275947999768202895413844655238344301543340473005508347115202620610490322321448428666307933949589839902646637760860939456939175463701460911385840335769192993205741708217442282129582719584719489728705911513230117256871308240936239105", 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            assertEquals(mexp1, mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1L);
            assertEquals(mexp2, product);
        }
    }
    @Test
    public void inverseOfHugeNumber2()
    {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("5603130552131808582980129320151431593287387887422304019872729976542631800068788204319579898052711893227272328490995919630013482288843181272802592367798898524753249376917995287069518581823351056941728232218068059475906712986411367854982914146900624999167769127442072642482993253159299132482762489032243580550536702583738240633476465093340828218312246935203167115248772557253480443158870340161116689390922962852167364803089439715091390938186175699671994828460433891219975915334762748122803062377109012273998249380330075478906670900287087012812096397970441489185240044951680317263972659517703324836533035198756630319129921923499685884740053891793070603801214192199848233151436265254188036888028127957406260369087189527978590594475592951159744836132820316608295381151982751459808466074838264192706066312469551538268619096100114127505148129379927320373232121684138190307396104482210039324618282627066824207318147380670441172882452441196009256198174338127429977948883866986080614196689277860961306323052769429682104967016727143083349244333982457816038315667725514706061625905397835791561560354555405798114663550095985095263976338037301461279067122567382168347234118084620013800495920079606189028067751569874497346329975073", 10);
            ModNumber mn = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp1 = ModNumber.stomn("6739138816136043312649272213174713769262983880649218872965179549677556315677506972615490310113406336693569234718740510119663919502539189540509251055798460074325514337939662564750863101055411914158661300731270123866093947731042804706074093172689096545237738202130653344811784515331242745926904461789015551948920613465874894376131240703947702355076603139914962929019780686185414664545329663218317295135211022489224303293900015601419851026256029933643673808719974409805841004170959181758705284178201118626199154465204469896041437537889130973291884948000755501999889301784901971136592495490062001754886483556366544831945270673733954935411162006328088367540078380415454971364899079826871715550246362070409406505441342293480933173058131951954050439775679821566910118564679299097353256497284984326254753897393933507219970088105495830123020354077871747461442531592169831588898411349425799974571880327159780649631710046012433252304811853839406510704163016067299984268200721162895812075320190090687162866263831468978576430892453490199685391372592054813829543523397778555188730184762400193992336282063202658915226866054763855569295020560910720893342762594098082777028627182850722798218135965067629433110731488659794290674761733", 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            assertEquals(mexp1, mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1L);
            assertEquals(mexp2, product);
        }
    }
    @Test
    public void inverseOfHugeNumber2Inverse()
    {
        if (ModNumber.MaxMod >= 4096/8) {
            ModNumber mx = ModNumber.stomn("6739138816136043312649272213174713769262983880649218872965179549677556315677506972615490310113406336693569234718740510119663919502539189540509251055798460074325514337939662564750863101055411914158661300731270123866093947731042804706074093172689096545237738202130653344811784515331242745926904461789015551948920613465874894376131240703947702355076603139914962929019780686185414664545329663218317295135211022489224303293900015601419851026256029933643673808719974409805841004170959181758705284178201118626199154465204469896041437537889130973291884948000755501999889301784901971136592495490062001754886483556366544831945270673733954935411162006328088367540078380415454971364899079826871715550246362070409406505441342293480933173058131951954050439775679821566910118564679299097353256497284984326254753897393933507219970088105495830123020354077871747461442531592169831588898411349425799974571880327159780649631710046012433252304811853839406510704163016067299984268200721162895812075320190090687162866263831468978576430892453490199685391372592054813829543523397778555188730184762400193992336282063202658915226866054763855569295020560910720893342762594098082777028627182850722798218135965067629433110731488659794290674761733", 10);
            ModNumber mn = ModNumber.stomn("7190287207838820893100135187591463297447559774717166059824270756832318486328488814752031338948818013830908982914290473938563014921177525490473011806342925882895886097473523799223635129440731232194289316956312171751893933291573672330315052262395035021957222108863546783107337685542268894846731095464393774624323738670545341850646929388739200012084857114116094857323527400524995693143441078547076749381043070621480267333111347721650354207748032979474990341369370977484950443627051005075190634017689966630089017111090781113359137315757718277312727467312085057616969674423276497466914004044619421512897899621484075965524310674554383801886980874686077091285272478817844116361194746280507045296642120093877814286607902587678536670186535158948630008158683133404028092117980309742839004587882077572372876260413011629479811794967468330342248338187799603438934256160856376709593095307957300519887059604075660729588821137917928262943607541892734844450182785572196075947525437323058412675309064498946240974268469314127702282087910782979398635858853101463257569750413467299310507641778273298930088411391215965333881021974553986041112895081398883245410772489040309209353804881728721172571367393723030500183662302997522913983201281", 10);
            ModNumber mexp1 = ModNumber.stomn("5603130552131808582980129320151431593287387887422304019872729976542631800068788204319579898052711893227272328490995919630013482288843181272802592367798898524753249376917995287069518581823351056941728232218068059475906712986411367854982914146900624999167769127442072642482993253159299132482762489032243580550536702583738240633476465093340828218312246935203167115248772557253480443158870340161116689390922962852167364803089439715091390938186175699671994828460433891219975915334762748122803062377109012273998249380330075478906670900287087012812096397970441489185240044951680317263972659517703324836533035198756630319129921923499685884740053891793070603801214192199848233151436265254188036888028127957406260369087189527978590594475592951159744836132820316608295381151982751459808466074838264192706066312469551538268619096100114127505148129379927320373232121684138190307396104482210039324618282627066824207318147380670441172882452441196009256198174338127429977948883866986080614196689277860961306323052769429682104967016727143083349244333982457816038315667725514706061625905397835791561560354555405798114663550095985095263976338037301461279067122567382168347234118084620013800495920079606189028067751569874497346329975073", 10);
            MultGroupMod mgm = new MultGroupMod(mn);
            ModNumber mres = mgm.Inverse(mx);
            assertEquals(mexp1, mres);
            ModNumber product = mgm.Mult(mx, mres);
            ModNumber mexp2 = new ModNumber(1L);
            assertEquals(mexp2, product);
        }
    }
    @Test
    public void multGroupModAddAllFsFirstBlockBothLessMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        r[0] = ~0L;
        n[1] = 0x10L;
        exp[0] = ~0L << 1;
        exp[1] = 0x1L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mn = new ModNumber(n);
        ModNumber mexp = new ModNumber(exp);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mres = mgm.Add(ml, mr);
        assertEquals(mexp, mres);

    }
    @Test
    public void multGroupModAddAllFsFirstBlockBothGreaterMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        l[0] = ~0L;
        r[0] = ~0L;
        n[1] = 0x01L;
        exp[0] = ~0L << 1;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mn = new ModNumber(n);
        ModNumber mexp = new ModNumber(exp);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mres = mgm.Add(ml, mr);
        assertEquals(mexp, mres);

    }
    @Test
    public void multGroupModAddAllFsBothGreaterMod()
    {
        long[] l = new long[ModNumber.LCOUNT];
        long[] r = new long[ModNumber.LCOUNT];
        long[] n = new long[ModNumber.LCOUNT];
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            l[i] = ~0L;
            r[i] = ~0L;
            n[i] = ~0L;
        }
        exp[0] = ~0L << 1;
        exp[1] = 0x1L;
        n[ModNumber.LCOUNT - 1] = 0L;
        ModNumber ml = new ModNumber(l);
        ModNumber mr = new ModNumber(r);
        ModNumber mn = new ModNumber(n);
        ModNumber mexp = new ModNumber(exp);
        MultGroupMod mgm = new MultGroupMod(mn);
        ModNumber mres = mgm.Add(ml, mr);
        assertEquals(mexp, mres);

    }
    @Test
    public void calculateRSAKeyGivenTwoPrimesAndChosenExponent()
    {
        if (ModNumber.MaxMod == 4096/8) {
            ModNumber mExponent = ModNumber.stomn("010001", 16);
            ModNumber mModulusExp = ModNumber.stomn("A26960E1102074E8E1E7D0754F6BED06BC5AAFEEA086704ACB33B99289FBC180874418D11E02B1B250313EE873E7307D932DA7C30A5E703CBE86EC588B318B46C20BD8BB01F51549635641E8958B0A109E59B06752ABED027539FBEA8BB700E58AD1985CE405F57BD01F4FB91E88BC8B50C4F0208EA02BB72612F41DE302DB3C7BFF5EB12C277417068CE2CA387BEBDE35320D045438951AD51D34CE1F52B10E1F7A9023E9C647582D266CEF6D2E067B60C1BED9C781ADC520223091210854A356C6CAE5B1874FDC59A643441C228F30E4329345AD96A2605972524BFEE8764D207435E42001C678028C48167610A6FCDDF43BC789DD04A6AC3B337E4F6D8085A404E759936355BCA8E7ED93AB6BB031830CEB586D1CC5A90E87F497B8096AC6ABE37BC701E33C9D9B61FBC8AF73446AAC90301ACDCE9E0DEBD322A032F1C90CA8D766285723B5DE918E65F1A987B32E2D51DB7E2C8EB8F2A28C7292E14BD56FB0EC779F44C138C9DF08BEDBD0568DC8FB9F65B7DD20CE80652C9194E160DCAA6C5F96BB5D357BE742B971B3F73923447C36C906E12147C0153CFB2551AB581513B38F2D3B90F2F3FDD17CC799224AA7DDBCAF41309008637CBAD18C27E226E849532B3CEE58CF3C9B3E147438935BCECB35C47B50E29623F76687A51792B75E077BC3D1240C73F6AB79B33577014C7BE2738BBE1F8B179517C5B5BEC23D7C9D", 16);
            ModNumber mPrime1 = ModNumber.stomn("CDA901CDEFC7392972350E5160DCE36D9E3415ABA270EFA599B72EA3A36E4BECF727A46DDE4B39181D2A1B76CA2A79DBFCAEE2DF3D46648BB6F5766DE8FBDB9B97915A5D6BC9E3C2AB30DF0251B7DF103CB8BB9C8873188E395C01CD8C374D72B6B65FBFF47331232D6F86E3EC22B3F866D35C74DF6D76D35836363D020571C86104307324EE0A704FF5BB77DBBCDC7A2D98BBDB0C65B60F22F4EFD5EE8555715F2C79CCC438484C4E1B2136E881C2B04AB27E015AD354AECDF0D717A78205CA7DF0353F2312FD54EF8FB51DA12E35115AFDFA0659C4386DF841C0FF51BB5EC37D555CEDF88FC783FB568FA6F5736060837B4020CACD9D9BAC5F75348673EDCB", 16);
            ModNumber mPrime2 = ModNumber.stomn("CA2A5948D61356DEDDCF8F5D180855C87E6981FDE13892F7CF593547B36D1CD5682699FD73D1036A24893EB2B8BCDBA8FC256F65E6644E2FC1E80C1BCADA0E21F6DED17EE6EA989BEB5E912E7FDB448573EC376817C4741C33EF78DCCE9DDE4F13FF6F29FB1117FF0873E5EB0B91AD52E41E2148DED2820C840574608F78E6D0E17DAF74F2B0A6171503EDA87D4B713819F86F07DE94CFFFBC2E8799DF78F138E28205AB7AF0286059980C241CD74C48FE897368E4CE6A096C4677BB2A37CF494395A1C3F695201863AFA632D2BBD61755779C20DD87AD78EE5E8E3EDA91D52FE2D0B91971197AA215EEE3E1F00DE0ED93B42E386C7685B71C1A25BDA0337237", 16);
            ModNumber mExp1Exp = ModNumber.stomn("92F32C22E62DC81E61489FCE6E13E73DB0492F22D09339E96E0881FED30C409C358DD1E3DF6E88813E7593E2315A60705C00EA82F0D997866D58BF2291D3447BCCB54388753870D1EC58A34098FB751BB90796A6A6A8F6E6C8FD811C4A1049FAA9AB6E874E37F0E85DD2CEE3951AC6D5BB435350FF44CC1EFF6CDD1C82913958205A49F43C730A6A6DF7FBD80CDEC9FB53114B499D761FBC7911E33D4D8D601BFE115D867EBE688A7D54CD4ADA9FBD691236AFEA6FE7891ECE704480D6640597469D98AE6075752677768BC77C1FF4334E7D2365DE4662BBD123CAAFA4CAE94F6E02CCC00D574F0C734ECA9E55CBF7157F5D55D918900B09ED04D84E8BB3D239", 16);
            ModNumber mExp2Exp = ModNumber.stomn("76DEF5C6CD116C07F9769CFE892E057AB151CA7C845C66964D964FACCC44C12A7E660C2FAA54E0E41D7D919F36DEDD6DFC6CEA968924BB907B3CBD39EE49CEFB8B9432ACA28C98953D8D4A62840D62B9F19298759D5C0745BA07FA12236F52133F57F77E40C12951ED1F70B68703EE5D0A02E961569B829A675A69B68945F6BA258C6889A455D82344591DF9EF4A57D53BF8071922786FE1183E67BF435672C816EF8AF3EC829FCB1F8336C859290312FC8FC78B8AD610256044030CCB632B8011BC3EF4E1DD8794AFB0CD519EF5918A00D826B6706BE1C574ABA68A30AC8524F601F284922B001FA433E64CA881109BC138B5E0B96DEB6BE29B414BC191DE7B", 16);
            ModNumber mCoefficientExp = ModNumber.stomn("80B8FE74BA8F322BB18AD25B5EA75E1B50F54521E648960084FA90895634016A7C3D0997C52A1BC766C58BFFE8CD8953146A188D29480311E624048F83ADAEE157EFC4AE9EFFB144BA8F41E2CACDACD50A51367BB9D013E29B6C859AFC030CAE31B38740F287D61F4CDEB78659E91D404FD6976FD7F5E34C1B7FF530B655AA36161C928BD128D56A6FC1469CF8DB7D308CAFAE0AFDA03A71A399C9EC05889427AAE189EA9DD2BB4A4132FB16622FC9394759F337C8AAD5AF6E3BF659D0624EB10238BA58BDAF858ACF954FBFF03AD12FC0F55C79EDC318FC96D6BC53DEB0688E2C1B80572EC942E37848C75BDAEE6366FA5C11396F8E18DD6AB0F61AD71265D4", 16);
            ModNumber mPrivExpExp = ModNumber.stomn("6DD1E9C365DF3C8E78AB02A740640F2B7DD3A8A12804A00BC3299FB2FA00B5508EA6BA4D3508C75576AA460A72CD0259F75A68D516BED310101935D98D8D9BCAE5CD92E8A9232B18ABC618DD07B9B3BD9705B8A5B51E322E764C160EFC186BCFCB357CDC5B07B87DD73644F73D3162A4A5A1F5695E67EA0777045BF3959C49D8B553F198C5ED8BAF422BA3F2EF8CC3EA4678289B23C3FAF6E0E255E18706A8588F6A95EAE2D9C34659896121E8E6B5555A10C857847328A861D91C94160D070647D8363C68F5C147938B3C72E2BF6FA9B4B48A271481061D367E7A2D8F196B5D2DB3C6F911B7819786C98AB93D6E772C077D6DAAB88B2DE8F20F39FB9003D061F9C8CB3A24B6F650C3C6393117201B765546B40453313045C0C67F9A404016F7073DE6F6877E7D9100EC77B7252D1E064EA99B102E73095D80049D127932C4A63D894DBF37446A5222CBA22B9B1B70B9C09C3EBA69EE4C302B6A59F7EE6C70C1F9283E9E171C14457FB78FAAC615AB15F691318A4CB4E55F1F1A1E635EAE217565A4D94A3505095232A4CB211E579122C2DCB67944A9E2514CEEA5C3E85CEFA916887B9D7B0BC4042EB4622A5E283566105E997FACFCC3EDBD152620E2A472E8CE47464BB0892642AE410CB9A5F283693E2F736444C54338978E21775E616BC2839208E89E5312CD26686C21E964F6AA29874E84FC1776751C1FAF0369B57075", 16);
            ModNumber mModulusProduct = ModNumber.product(mPrime1, mPrime2);
            ModNumber mPminus1 = ModNumber.subtract(mPrime1, new ModNumber(1L));
            ModNumber mQminus1 = ModNumber.subtract(mPrime2, new ModNumber(1L));
            ModNumber mPhiPQ = ModNumber.product(mPminus1, mQminus1);
            MultGroupMod mgmPhiPQ = new MultGroupMod(mPhiPQ);
            ModNumber mPrivExpCalc = mgmPhiPQ.Inverse(mExponent);
            ModNumber mDpCalc = ModNumber.modulo(mPrivExpCalc, mPminus1);
            ModNumber mDqCalc = ModNumber.modulo(mPrivExpCalc, mQminus1);
            MultGroupMod mgmP = new MultGroupMod(mPrime1);
            ModNumber mInverseQCalc = mgmP.Inverse(mPrime2);
            assertEquals(mModulusExp, mModulusProduct);
            assertEquals(mPrivExpExp, mPrivExpCalc);
            assertEquals(mExp1Exp, mDpCalc);
            assertEquals(mExp2Exp, mDqCalc);
            assertEquals(mCoefficientExp, mInverseQCalc);
        }
        else if (ModNumber.MaxMod == 2048/8) {
            ModNumber mExponent = ModNumber.stomn("010001", 16);
            ModNumber mModulusExp = ModNumber.stomn("CA75BD1F951E545F8BA1BF6C985398C48BE5CD45E1794AD7D151CF8871D4088C32B1E88D8E4A9106714EACD38C284D70A1E15EBCC289BED8EE90C771B7A45A86DA92C7D5D4936782A701CCC6C7BF75A9CF22E2C079B2E94A4C7ED28B2983AD71B8D24D4E43501CA1FE0C15204A311BD4EEFC9B62A9D1078D27CC5C213A4D328002100FB350D8749DCCC982D736F6ED95B70B146152C966661FF48FFDF8CB8180E0A49D0D62EDED2FBE40D8C639B4F3817EDE2D7D5D9AAE4E0ECE362598DA62613E9B7B9BF7D93F6C232AE9BFC34142A65774DB2241D8B810734FED65C97370FC84599E002680901B55D0E991E51C6C4856FC361FD526193EB972E2C29467F211", 16);
            ModNumber mPrime1 = ModNumber.stomn("E0162AC55685C9903231BE3A9A65D9C5E28F7D12683FD835B240DB86A0E3077ADA211CA866C00748430835882D586F44B0FF5784B4980C0EC73C52C7A195D2C514DC5A59E8683F6C0C4B614FD63F510505429928679F7807D7689E48966FBC29728CE81ACE99CDD69F48B0BE47E22AD0A65451FD80DE1E7BD93FF7755EA0FEFB", 16);
            ModNumber mPrime2 = ModNumber.stomn("E74B192C59634F62C4E14DF1CF88B26163567EAB1A8DF1B4002CA14DAAEEF3F99DD2A8493DBA4BA79055300D1CA685A9964D69E3F5CEAB0CF728914B66DCDB41063170DDC0254088C33134EE4ACAF86AA4F50458804FC55F306654E221A33566FE9F2ECD22B72527DD402AC3A5FE715EBA7C4FB23998C3E59C2A35F08DB45563", 16);
            ModNumber mExp1Exp = ModNumber.stomn("4B3AE5ED0E59280C42EDB4419468312E601B700FE7F3D7305FC1AA718E3DE9DAF2B43F1CC23221BFBED158B77911C1793C3ADD34EE344588089755EB33724B2A63107F3CC3EB8BAF48895E0821936C2DD550407EF21AF57F1C704103798DD58BCCAB45990A575465836032060FB7FC4BC42B76C336AA5B6D94C806D9DE850A55", 16);
            ModNumber mExp2Exp = ModNumber.stomn("AA0EE371623FCC8CB86957DFE55182A66C5D9A40C00381A571435F4E35D1FF7C96F7DE580B94C02F71C5D0D0B33214B0BE1C29313CF839780CF9BCEA8DB4F1587BEF3A273131B2A35BB10053FC9603D49578B93B410C65F83923E461678E12A3A9AEECC05338098FA7F397FDC63CB0AA8FBEFAF1F5BB46BDB9F6A736395A5035", 16);
            ModNumber mCoefficientExp = ModNumber.stomn("6F2C5D2D9A587AA5657032960ABBB1246A0D7B51196625693284F6B8B8222C5B2E8BE3139FEA29D20CC70FD6C8740CA5DE0A94C5ED1D56E6CCE8BC67AAE9153C57A02EEBDC5764B11608AED772B661243E265FBA989B74DB2B9DAC0CEC7AE9C322232C56AFD4BDD8746C8095BBCFDF04C553219092C8E243C94E84757FDA30AC", 16);
            ModNumber mPrivExpExp = ModNumber.stomn("0B5BF1C17CDF57C2889609A471237795C7779C3DC879C2AE3B2B48E691E4609A6519C03F78F54C0FBD79B98EA0F57DEACCDA55336802CC776563301D7C3D376B0B8662B9E79461A15DF766E2DC402CBCDB474CCB99C864F86B344E61CA717DE0BC8AE815750760A3E21A11715F5844047D6824DBB3456C140A6097E6B15BB572899E5706451E48D95BE641E03162BFFCAF5272807661DA94CC9C6081DD179EE2C158F2B713C76EA71969AB83DA5F2EAA59DAC9E29B6F55F637B6C109139B0DEE731F36FF59C799357BB7732DF9C8F5D09085B8187E6FF016E3563833EA34ACD1867DEE6F7844749F32BDAD344E1DC7FE2E89A1696CC5D9E12E3902DBF0FE2265", 16);
            ModNumber mModulusProduct = ModNumber.product(mPrime1, mPrime2);
            ModNumber mPminus1 = ModNumber.subtract(mPrime1, new ModNumber(1L));
            ModNumber mQminus1 = ModNumber.subtract(mPrime2, new ModNumber(1L));
            ModNumber mPhiPQ = ModNumber.product(mPminus1, mQminus1);
            MultGroupMod mgmPhiPQ = new MultGroupMod(mPhiPQ);
            ModNumber mPrivExpCalc = mgmPhiPQ.Inverse(mExponent);
            ModNumber mDpCalc = ModNumber.modulo(mPrivExpCalc, mPminus1);
            ModNumber mDqCalc = ModNumber.modulo(mPrivExpCalc, mQminus1);
            MultGroupMod mgmP = new MultGroupMod(mPrime1);
            ModNumber mInverseQCalc = mgmP.Inverse(mPrime2);
            assertEquals(mModulusExp, mModulusProduct);
            assertEquals(mPrivExpExp, mPrivExpCalc);
            assertEquals(mExp1Exp, mDpCalc);
            assertEquals(mExp2Exp, mDqCalc);
            assertEquals(mCoefficientExp, mInverseQCalc);
        }
        else if (ModNumber.MaxMod == 1024/8) {
            ModNumber mExponent = ModNumber.stomn("010001", 16);
            ModNumber mModulusExp = ModNumber.stomn("B08B5EF115AED8F91EE2FB82E7E8C8611DF101B4138B37787A18D1E72FC75437342A8961F7C462CBD2208DE6EC0472882CE6410BCE3AE096BB2CA0506B5FBF5865FC80465653F2AEA678C8A8638AF8CF98F105166B6CFA71A61567765DF9521FAE5616DBBF26F99512EEBF857C166941FC2402C5E27258B10206A8AF016C45E9", 16);
            ModNumber mPrime1 = ModNumber.stomn("C882E9B30B9F87F47AD7653886EF5CD53D9CAA96D2B83C24C643F024B86CDFE61DE6627F0C63B3CBB885DD3212C77CBE47A3469E5EA2FAD245984A75D866AAF3", 16);
            ModNumber mPrime2 = ModNumber.stomn("E1668918F4DD83B3066B32577933BD27B1FFA8F49907637863750304EFCBDB1DCC86362EEBC81ACD68B7E0C0BCE35B1BC0F353F16B59C5E7636AF1464805CAB3", 16);
            ModNumber mExp1Exp = ModNumber.stomn("A8130C8077D2BEECD0F7B2B242716C5896EFB712FC0950E8E684D64C818749DB23DF155B37F06F2AEA3ED39E809D55544ECA6F8C9AF974D4436017B256B620BF", 16);
            ModNumber mExp2Exp = ModNumber.stomn("74EDCA456611DC166D4802EF99F29B381280FC571763DBD842501B66DE3734CE7AA14D4E7E066AD110780362391B00D29F001C15EB88C0BFCEB555C10DEFE33D", 16);
            ModNumber mCoefficientExp = ModNumber.stomn("89C0F663B05BC7C1CE547866B45F137DC73739DF9C159B5BA648A5C7818BC46A9CD12C41922A82DB150F9D34F08D69B4A94A947848E88DC5735800C882C856A1", 16);
            ModNumber mPrivExpExp = ModNumber.stomn("9E7C2F39FBFE1FD7DC2B662009328717EFFA184E61311C15F27DEF893BF2141F0E9C9502369BBD193E446D3EFD67ACADA4A8FB81AE9C5A5BD621E4B4ECFF625469B82CE442C50E56F2C7E860FD7414AB46C9BA2C8F043FC2FAF5408E50A758BFDB2AF454020A1E77586C4F7E2D7CCF66E354715606B5223C31538AEDAAA0DB85", 16);
            ModNumber mModulusProduct = ModNumber.product(mPrime1, mPrime2);
            ModNumber mPminus1 = ModNumber.subtract(mPrime1, new ModNumber(1L));
            ModNumber mQminus1 = ModNumber.subtract(mPrime2, new ModNumber(1L));
            ModNumber mPhiPQ = ModNumber.product(mPminus1, mQminus1);
            MultGroupMod mgmPhiPQ = new MultGroupMod(mPhiPQ);
            ModNumber mPrivExpCalc = mgmPhiPQ.Inverse(mExponent);
            ModNumber mDpCalc = ModNumber.modulo(mPrivExpCalc, mPminus1);
            ModNumber mDqCalc = ModNumber.modulo(mPrivExpCalc, mQminus1);
            MultGroupMod mgmP = new MultGroupMod(mPrime1);
            ModNumber mInverseQCalc = mgmP.Inverse(mPrime2);
            assertEquals(mModulusExp, mModulusProduct);
            assertEquals(mPrivExpExp, mPrivExpCalc);
            assertEquals(mExp1Exp, mDpCalc);
            assertEquals(mExp2Exp, mDqCalc);
            assertEquals(mCoefficientExp, mInverseQCalc);

        }
    }
    @Test
    public void getPKCS1MaskMessageTooLong()
    {
        ModNumber message = ModNumber.stomn("FFFFFFFFFFFF", 16);
        assertThrows(IllegalArgumentException.class,() -> message.getPKCS1Mask(false, 16, 0));
    }
    @Test
    public void getPKCS1MaskMessageEmptyModulus26Fs()
    {
        ModNumber message = new ModNumber(0L);
        ModNumber mres = message.getPKCS1Mask(false, 13, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.HexStringLength - 26), "0".repeat(ModNumber.HexStringLength - 26));
        assertEquals(resStr.substring(ModNumber.HexStringLength - 26,ModNumber.HexStringLength - 26 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.HexStringLength - 2,ModNumber.HexStringLength - 2 + 2), "00");
    }
    @Test
    public void getPKCS1MaskMessageFourFsModulus26Fs()
    {
        ModNumber message = ModNumber.stomn("FFFF", 16);
        ModNumber mres = message.getPKCS1Mask(false, 13, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.HexStringLength - 26), "0".repeat(ModNumber.HexStringLength - 26));
        assertEquals(resStr.substring(ModNumber.HexStringLength - 26,ModNumber.HexStringLength - 26 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.HexStringLength - 6,ModNumber.HexStringLength - 6 + 6), "00FFFF");
    }
    @Test
    public void getPKCS1MaskMessageEightFsModulus30Fs()
    {
        ModNumber message = ModNumber.stomn("FFFFFFFF", 16);
        ModNumber mres = message.getPKCS1Mask(false, 15, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.HexStringLength - 30), "0".repeat(ModNumber.HexStringLength - 30));
        assertEquals(resStr.substring(ModNumber.HexStringLength - 30,ModNumber.HexStringLength - 30 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.HexStringLength - 10,ModNumber.HexStringLength - 10 + 10), "00FFFFFFFF");
    }
    @Test
    public void getPKCS1MaskMessageEightFsModulus32Fs()
    {
        ModNumber message = ModNumber.stomn("FFFFFFFF", 16);
        ModNumber mres = message.getPKCS1Mask(false, 16, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.HexStringLength - 32), "0".repeat(ModNumber.HexStringLength - 32));
        assertEquals(resStr.substring(ModNumber.HexStringLength - 32,ModNumber.HexStringLength - 32 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.HexStringLength - 10,ModNumber.HexStringLength - 10 + 10), "00FFFFFFFF");
    }
    @Test
    public void getPKCS1MaskMessageEightFsModulus34Fs()
    {
        ModNumber message = ModNumber.stomn("FFFFFFFF", 16);
        ModNumber mres = message.getPKCS1Mask(false, 17, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.HexStringLength - 34), "0".repeat(ModNumber.HexStringLength - 34));
        assertEquals(resStr.substring(ModNumber.HexStringLength - 34,ModNumber.HexStringLength - 34 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.HexStringLength - 10,ModNumber.HexStringLength - 10 + 10), "00FFFFFFFF");
    }
    @Test
    public void getPKCS1MaskMessageEightFsModulus36Fs()
    {
        ModNumber message = ModNumber.stomn("FFFFFFFF", 16);
        ModNumber mres = message.getPKCS1Mask(false, 18, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.HexStringLength - 36), "0".repeat(ModNumber.HexStringLength - 36));
        assertEquals(resStr.substring(ModNumber.HexStringLength - 36,ModNumber.HexStringLength - 36 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.HexStringLength - 10,ModNumber.HexStringLength - 10 + 10), "00FFFFFFFF");
    }
    @Test
    public void getPKCS1MaskMessageTwentyFsModulus72Fs()
    {
        ModNumber message = ModNumber.stomn("FFFFFFFFFFFFFFFFFFFF", 16);
        ModNumber mres = message.getPKCS1Mask(false, 36, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.HexStringLength - 72), "0".repeat(ModNumber.HexStringLength - 72));
        assertEquals(resStr.substring(ModNumber.HexStringLength - 72,ModNumber.HexStringLength - 72 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.HexStringLength - 22,ModNumber.HexStringLength - 22 + 22), "00FFFFFFFFFFFFFFFFFFFF");
    }
    @Test
    public void getPKCS1MaskMessageMaxFs()
    {
        ModNumber message = ModNumber.stomn("F".repeat(ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 22), 16);
        ModNumber mres = message.getPKCS1Mask(false, ModNumber.MaxMod, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.LSIZE * 2), "0".repeat(ModNumber.LSIZE * 2));
        assertEquals(resStr.substring(ModNumber.LSIZE * 2,ModNumber.LSIZE * 2 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.LSIZE * 2 + 4 + 16,ModNumber.LSIZE * 2 + 4 + 16 + 2), "00");
        assertEquals(resStr.substring(ModNumber.LSIZE * 2 + 4 + 16 + 2,ModNumber.LSIZE * 2 + 4 + 16 + 2 + ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 22), "F".repeat(ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 22));
    }
    @Test
    public void getPKCS1MaskMessageMaxFsMinus2()
    {
        ModNumber message = ModNumber.stomn("F".repeat(ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 24), 16);
        ModNumber mres = message.getPKCS1Mask(false, ModNumber.MaxMod, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.LSIZE * 2), "0".repeat(ModNumber.LSIZE * 2));
        assertEquals(resStr.substring(ModNumber.LSIZE * 2,ModNumber.LSIZE * 2 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.LSIZE * 2 + 4 + 18,ModNumber.LSIZE * 2 + 4 + 18 + 2), "00");
        assertEquals(resStr.substring(ModNumber.LSIZE * 2 + 4 + 18 + 2,ModNumber.LSIZE * 2 + 4 + 18 + 2 + ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 24), "F".repeat(ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 24));
    }
    @Test
    public void getPKCS1MaskMessageMaxFsMinus4()
    {
        ModNumber message = ModNumber.stomn("F".repeat(ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 26), 16);
        ModNumber mres = message.getPKCS1Mask(false, ModNumber.MaxMod, 0);
        String resStr = mres.toString(16);
        assertEquals(resStr.substring(0, ModNumber.LSIZE * 2), "0".repeat(ModNumber.LSIZE * 2));
        assertEquals(resStr.substring(ModNumber.LSIZE * 2,ModNumber.LSIZE * 2 + 4), "0002");
        assertEquals(resStr.substring(ModNumber.LSIZE * 2 + 4 + 20,ModNumber.LSIZE * 2 + 4 + 20 + 2), "00");
        assertEquals(resStr.substring(ModNumber.LSIZE * 2 + 4 + 20 + 2,ModNumber.LSIZE * 2 + 4 + 20 + 2 + ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 26), "F".repeat(ModNumber.HexStringLength - ModNumber.LSIZE * 2 - 26));
    }
    @Test
    public void fromTextTextOneCharTooLong()
    {
        String s = "a".repeat ((ModNumber.LCOUNT * ModNumber.LSIZE) / Character.BYTES + 1);
        assertThrows(IllegalArgumentException.class,() -> ModNumber.fromText(s));
    }
    @Test
    public void fromTextTextEightCharsTooLong()
    {
        String s = "a".repeat ((ModNumber.LCOUNT + 1) / Character.BYTES * ModNumber.LSIZE);
        assertThrows(IllegalArgumentException.class,() -> ModNumber.fromText(s));
    }
    @Test
    public void fromTextTextMaxSizeAllAs()
    {
        String s = "\u6161".repeat ((ModNumber.LCOUNT * ModNumber.LSIZE) / Character.BYTES );
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
            exp[i] = 0x6161616161616161L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.fromText(s);
        assertEquals(mexp, mres);
    }
    @Test
    public void fromTextTextMaxSizeMinusOneAllAs()
    {
        String s = "\u6161".repeat ((ModNumber.LCOUNT * ModNumber.LSIZE - 1) / Character.BYTES );
        long[] exp = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
            exp[i] = 0x6161616161616161L;
        exp[ModNumber.LCOUNT - 1] = 0x616161616161L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.fromText(s);
        assertEquals(mexp, mres);
    }
    @Test
    public void fromTextWholeAlphabet()
    {
        String s = "abcdefghijklmnopqrstuvwxyz";
        long[] exp = new long[ModNumber.LCOUNT];
        exp[0] = 0x0064006300620061L;
        exp[1] = 0x0068006700660065L;
        exp[2] = 0x006c006b006a0069L;
        exp[3] = 0x0070006f006e006dL;
        exp[4] = 0x0074007300720071L;
        exp[5] = 0x0078007700760075L;
        exp[6] = 0x007a0079L;
        ModNumber mexp = new ModNumber(exp);
        ModNumber mres = ModNumber.fromText(s);
        assertEquals(mexp, mres);
    }
    @Test
    public void getTextTextEmpty()
    {
        ModNumber mzero = new ModNumber(0);
        String res = mzero.getText();
        String exp = "";
        assertEquals(exp, res);

    }
    @Test
    public void getTextTextSingleChar()
    {
        ModNumber mzero = new ModNumber(0x0061L);
        String res = mzero.getText();
        String exp = "a";
        assertEquals(exp, res);

    }
    @Test
    public void getTextTextMaxSizeAllAs()
    {
        long[] n = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT; i++)
        {
            n[i] = 0x0061006100610061L;
        }
        ModNumber mn = new ModNumber(n);

        String res = mn.getText();
        String exp = "a".repeat(ModNumber.LCOUNT * ModNumber.LSIZE / Character.BYTES);
        assertEquals(exp, res);

    }
    @Test
    public void getTextTextMaxSizeMinusOneAllAs()
    {
        long[] n = new long[ModNumber.LCOUNT];
        for (int i = 0; i < ModNumber.LCOUNT - 1; i++)
        {
            n[i] = 0x0061006100610061L;
        }
        n[ModNumber.LCOUNT - 1] = 0x006100610061L;
        ModNumber mn = new ModNumber(n);

        String res = mn.getText();
        String exp = "a".repeat(ModNumber.LCOUNT * ModNumber.LSIZE / Character.BYTES - 1);
        assertEquals(exp, res);

    }
    @Test
    public void getTextWholeAlphabet()
    {
        String exp = "abcdefghijklmnopqrstuvwxyz";
        long[] n = new long[ModNumber.LCOUNT];
        n[0] = 0x0064006300620061L;
        n[1] = 0x0068006700660065L;
        n[2] = 0x006c006b006a0069L;
        n[3] = 0x0070006f006e006dL;
        n[4] = 0x0074007300720071L;
        n[5] = 0x0078007700760075L;
        n[6] = 0x007a0079L;
        ModNumber mn = new ModNumber(n);
        String res = mn.getText();
        assertEquals(exp, res);
    }
    @Test
    public void readDSASignature()
    {
        ModNumber signature = ModNumber.stomn("302C021427FBE13628A0AA7053E3C11CE6B4E7F40624C18F02146D9F22C0AA16841B26969166C692E92B41176232", 16);
        List<Object> results = signature.ParseBERASNString();
        int[] exp1 = { 0x27, 0xFB, 0xE1, 0x36, 0x28, 0xA0, 0xAA, 0x70, 0x53, 0xE3, 0xC1, 0x1C, 0xE6, 0xB4, 0xE7, 0xF4, 0x06, 0x24, 0xC1, 0x8F };
        int[] exp2 = { 0x6D, 0x9F, 0x22, 0xC0, 0xAA, 0x16, 0x84, 0x1B, 0x26, 0x96, 0x91, 0x66, 0xC6, 0x92, 0xE9, 0x2B, 0x41, 0x17, 0x62, 0x32 };
        byte[] result1 = (byte[])results.get(0);
        byte[] result2 = (byte[])results.get(1);
        boolean results1Equal = true;
        for (int i = 0; i < result1.length; i++)
        {
            int result1Int = result1[i] & 0xFF;
            if (exp1[i] != result1Int)
            {
                results1Equal = false;
                break;
            }
        }
        boolean results2Equal = true;
        for (int i = 0; i < result2.length; i++)
        {
            int result2Int = result2[i] & 0xFF;
            if (exp2[i] != result2Int)
            {
                results2Equal = false;
                break;
            }
        }
        assertTrue(results1Equal);
        assertTrue(results2Equal);

    }
    @Test
    public void dsaParameters()
    {
        ModNumber P = null;
        ModNumber Q = null;
        ModNumber g = null;
        ModNumber x = null;
        ModNumber y = null;
        if (ModNumber.MaxMod == 3072/8) {
            P = ModNumber.stomn("BCAE0FB273D98B3CDDDFBB1BE44470A592AAC3EA0885D06C272C5333D8C62AA7F8E5A09D56FF462BFF1032870B306F1A997CE635B920630B128C9030EE5506B7C19992EEBEDE39892D0107E0F6BBC417FE1EF3F1FE436A30E7FB1FD9043D4BAC3A240D6E287FE33090351746BC25639FAD56D3E2922D47092D0EF4AC7B0F0F85E747CF55A11E702276A6C40E285C3FBB61E50755EE3F27764B748623EC67D8E2D8B2E5A6142D244A5C4500C3A580A174FDC9D48BDC9CD93AEF8930563A28FBCBA5E52D8DDCCCE9FDB249E910594B02AB4A9283DEE01B42B405E8D71EC0B613ADCBB7095991256F6B38AE318AFEBB2432D708BCB913E6D67729E8B0F97B30CB30F5223980F792B9C8ECBDB19236FDEE155F1D8F4D0540B63EED872599DFF8B48A3A8F3F966F6CDC389781DAAFBC7DBFF8F787F0ACE60680370F6C94C0DC615A437068583E7C31FF381E1BAB1F8311EA3FD588FD74D12ACC15E6217E118E76C2C66686215D07BE8A54EA754ECECA6377E0BDFA8289F1FC3380C517E0A2CD06B8A7", 16);
            Q = ModNumber.stomn("BEF143E5D8624534038989D4AC6A76FC83A44E501C20BA8E6D3FC092F0EE36A7", 16);
            g = ModNumber.stomn("402F21E6DCBC4614C35105555518FA9C7F1E78330AA227BE42F4E63C127A677D0736D814FC76F2E411B06EB860C6F9D7014193ED8E5052D02E8E0045940A3C9E4C5DE8C3219A27C912D439B7F8EDFD7039989A18C7254985608B667E642E8526A069CE050E488D94F8AA35BA272EADA29DD42D9C40C8E82A69C07E8CC29801C5463100631D51810306B60BF75BDCEFA779C6C4469575C0F8B4DC100AD74052DF124987E1C6859E03FD79CD4A1AF7BD58EEB3B86D124BEB77ED2FBF82F5104E3B3CB524842A24C8048BEBBB3D153F0A9FB9B98422F8144EB1D243C1ED82F858277E68ACC83D877CD6403573A88C2CA532899631C8D68E811C16939C663AB9A5A3A3DADBAB2587DA7796D620068B26C96FBCFDC616BC0AE10829D9F6ED207FA0B2B1DFE78E3BD9A1AD45C261DC3DB231AC34B971E0B3017509504949C743EBC1D47520459FACECCCB5F442E40CAF35390DFCDB524FE23508B03EDFEDB435DF9A0E4305CFF095E81CFEDA0C1F75B45DAA63A1DAE14AC31629A2E3C9CF6EF0191418", 16);
            x = ModNumber.stomn("8BD650804F276A2EE4FAF859F63E25389614D8541119EF81CA482DEE4F312499", 16);
            y = ModNumber.stomn("A3FCC9FDEE022201CD3F90D8348E71C4CB433B1015BCD63CE124763F94DCA05C431FCB1061445516436F6167B772EE4865189B019323D200FEBA62CB4EF7EA3F553F46699CDB8E4803290FF50145DAB9D77E98F50CEDF12FB3564D7F7B5C88874F7AF5BE5793331BD60B2E32EEF8E88DBAFA738D15D52A3B186E20FAACFCFA383D4B1CD5863B0719CCC332CCCEE0E9739720121C3AB0DE5C754D700A6ABC5953CFEB17BE1DD0E45E519D306A1AF01D89ACAE636DC73617A70AFD7D73926AADD6D1C6706ACAFE60AD8E411FA9D07BE96CC2C91CF22CC7C3AE32DBFF4F5660B9CDFFDB3E975E2169208F400A9017A669469EEAB21CCD0018D67FABCE18697AB775A7EFBE3A08EEF72E0C2A5E026A892799231BB592B23F02C1E829E445CACB29379D2FF6AC1B44DCE032ADBB67C585B0E182E5C9902EC8919BC4BA01E7E2568B0F4134B71529D8F56E0C2DFA4CAE327E14E6473DB59AB4A32294452284B27366A33CFF27DD341D89EC114F3929DC78B0CE719789617A0696AB0A75459FD7E2B25C", 16);
        }
        else if (ModNumber.MaxMod == 2048 / 8) {
            P = ModNumber.stomn("DFC017474A4FAF7A5E094FC31BD901731AD1823C5FAF5A06433AB3928FF9BC2EF5A0A90FC0AADB4FEB3294175089DCCE2509E2F7E5E7B7D7FB0CD54A025C6C6DCB599DA196BB9729B824BC811E67F6C1DB95F40DB8BD8A5C8F0C98A2B887127B975279B7F744D0392DD76350F989B2FF212B58557AADD9A718B8EF5453C71F2F4989EC458DF17F1126BE8FA0A48848F028A9808FBF524BF24E0960912640FD0A610C064A243299F992245DEBBAF084601ED8164875B0ABB7EDE528053A4753AC6C91CEB4DA8DE85DC80CDF4CF95A31A03667C5B774E9F7C9436E706F08C4F9FF58ABFECA29D255D732F57CCBB92CDDBD5A56DDAF50ADA60FF932239CCFF8B039", 16);
            Q = ModNumber.stomn("8B50F6EA9476593FC9295A3D28CA0E80E46164C8E5F16E80E97007C8612CDE93", 16);
            g = ModNumber.stomn("DDF1F59904EE5F22270307B45C75153D296965D17FA6C5A12F71D3FDC18BD7FB8C11337958500124550B3084E651FD62407C1E82374641742745169685DA6A9E2060D1A1BBC9E747A2B1A19A004BF527DE7859E75703111E81AD9A0DB62E78208E91CAF32635A0F669DC3A51ED18D8EBB5613355364CF2FBB4D16D2379E81532A7246D824B85AA74CDA49D7EB0582F44C75FDEB0709CEC1F826B8117B6D4348EF58CCE26443A383DD5717CD84F692C587D915B80481838BF27ABFD7B52D37443A52D912CF744FC313F8DDE5D322D054DF9A75717164EBB99CC345E9C39530C3776E8981B5FACB727F57A3BB072DA4C24DDB011B5E2BE8A2D77247B302ADFAB68", 16);
            x = ModNumber.stomn("258A57C6E1F497C6B2D546E21B9C4F24A9666CE4DEF69E1666433B626B818437", 16);
            y = ModNumber.stomn("9A071F438C87B44175A58DF2C0469B7048221E6D920B3983961BFF559099CC8E9C6D312EC9798E8F55CB4FCD85551DB50B91EA9BE12A8D709562D9253395692E9AF2E6F357F291AC5172818292979CDC4FD0A809833E431C5A29DD91D319EA62C1CE24A5F04558A0A918D8CB3D181B5A22D7BA8B5BCCC58120AE7D0D2C90A706C70BE62C5F0ED4D364D6F805A4D5C4F568F3772E5763411B153130EAB0CFC3D681E6C321B99D3EAA91CA3BCA2BD9F6C22A4169AE17750095A75C74FBC4C0A0FB5945DDE9FB2BA4868EDAE6B41940D8A47AF036A9E0DCEDF456DC48A61A0ABF9745F71CA6AEC24CC5783C76D2732662B351EDD3D98C6F83F84B472189C07DC519", 16);
        }
        else if (ModNumber.MaxMod == 1024 / 8) {
            P = ModNumber.stomn("C18006893BCE75DC77605271C0AA43F5BFF9FD7B24F3DE9817A1B8230650542B1B30B66727EA3D44FBB476451D96B869555780D447729A7DC1ED7216B4F5C6A1A544F361723C3ACACBEFFA393389F42A7DE7F42106A30C04B0275DE550C8FF91392AA44FFBEBA94FE07CD19B50C5D66814BB31D741628503B0E579C76D32FF2D", 16);
            Q = ModNumber.stomn("859D7C3729301E8865F41654C0041A3C562A682F", 16);
            g = ModNumber.stomn("847AE11402C2FF443350DE8EB7665060DEC027A5AC1103C98C7F2CCD9C88D127185B7A0189F1674A0039FBD5DCFF4AE152F6C100D122ABF3B3E177F5693673408A0457C35667A3C80E51350F94F9696E8C5AC376FFC51E8CA9FD38169E6A1D0025EA1747D434C8DC8DE47B61E39AE13FCD8AB9ADEBEC982E5B3D249618CAD307", 16);
            x = ModNumber.stomn("334F55231BC70F896CE4BB081583812440110F1A", 16);
            y = ModNumber.stomn("3C7E5B1749C4D012C70A78D303CF9ED5C765DFDC0DB94DB0780704ABC0659B4D00BC13E02BE8CCB4F63E90EE76391CAFF69FCE225094275C912987AECCC21A52C3D2FE5FD338093CCFDEB35D097390609FC44AA8059CA745E161BB96A80AC0F93D6FDDEB2D0520AE3A6C8F7DBA4140B4057AF54C8AD5004FED3327FAB91136B9", 16);
        }
        if (ModNumber.MaxMod < 4096 / 8) {
            MultGroupMod mgm = new MultGroupMod(P);
            ModNumber computedY = mgm.Exp(g, x);
            assertEquals(y, computedY);
            ModNumber pModQ = ModNumber.modulo(P, Q);
            ModNumber mone = new ModNumber(1L);
            assertEquals(mone, pModQ);
            ModNumber computedGPowQ = mgm.Exp(g, Q);
            assertEquals(mone, computedGPowQ);
        }
    }
    @Test
    public void verifyDSASignature()
    {
        DSAParameters dsaParameters = new DSAParameters();
        String signature;
        if (ModNumber.MaxMod == 3072 / 8) {
            dsaParameters.P = ModNumber.stomn("BCAE0FB273D98B3CDDDFBB1BE44470A592AAC3EA0885D06C272C5333D8C62AA7F8E5A09D56FF462BFF1032870B306F1A997CE635B920630B128C9030EE5506B7C19992EEBEDE39892D0107E0F6BBC417FE1EF3F1FE436A30E7FB1FD9043D4BAC3A240D6E287FE33090351746BC25639FAD56D3E2922D47092D0EF4AC7B0F0F85E747CF55A11E702276A6C40E285C3FBB61E50755EE3F27764B748623EC67D8E2D8B2E5A6142D244A5C4500C3A580A174FDC9D48BDC9CD93AEF8930563A28FBCBA5E52D8DDCCCE9FDB249E910594B02AB4A9283DEE01B42B405E8D71EC0B613ADCBB7095991256F6B38AE318AFEBB2432D708BCB913E6D67729E8B0F97B30CB30F5223980F792B9C8ECBDB19236FDEE155F1D8F4D0540B63EED872599DFF8B48A3A8F3F966F6CDC389781DAAFBC7DBFF8F787F0ACE60680370F6C94C0DC615A437068583E7C31FF381E1BAB1F8311EA3FD588FD74D12ACC15E6217E118E76C2C66686215D07BE8A54EA754ECECA6377E0BDFA8289F1FC3380C517E0A2CD06B8A7", 16);
            dsaParameters.Q = ModNumber.stomn("BEF143E5D8624534038989D4AC6A76FC83A44E501C20BA8E6D3FC092F0EE36A7", 16);
            dsaParameters.g = ModNumber.stomn("402F21E6DCBC4614C35105555518FA9C7F1E78330AA227BE42F4E63C127A677D0736D814FC76F2E411B06EB860C6F9D7014193ED8E5052D02E8E0045940A3C9E4C5DE8C3219A27C912D439B7F8EDFD7039989A18C7254985608B667E642E8526A069CE050E488D94F8AA35BA272EADA29DD42D9C40C8E82A69C07E8CC29801C5463100631D51810306B60BF75BDCEFA779C6C4469575C0F8B4DC100AD74052DF124987E1C6859E03FD79CD4A1AF7BD58EEB3B86D124BEB77ED2FBF82F5104E3B3CB524842A24C8048BEBBB3D153F0A9FB9B98422F8144EB1D243C1ED82F858277E68ACC83D877CD6403573A88C2CA532899631C8D68E811C16939C663AB9A5A3A3DADBAB2587DA7796D620068B26C96FBCFDC616BC0AE10829D9F6ED207FA0B2B1DFE78E3BD9A1AD45C261DC3DB231AC34B971E0B3017509504949C743EBC1D47520459FACECCCB5F442E40CAF35390DFCDB524FE23508B03EDFEDB435DF9A0E4305CFF095E81CFEDA0C1F75B45DAA63A1DAE14AC31629A2E3C9CF6EF0191418", 16);
            dsaParameters.x = ModNumber.stomn("8BD650804F276A2EE4FAF859F63E25389614D8541119EF81CA482DEE4F312499", 16);
            dsaParameters.y = ModNumber.stomn("A3FCC9FDEE022201CD3F90D8348E71C4CB433B1015BCD63CE124763F94DCA05C431FCB1061445516436F6167B772EE4865189B019323D200FEBA62CB4EF7EA3F553F46699CDB8E4803290FF50145DAB9D77E98F50CEDF12FB3564D7F7B5C88874F7AF5BE5793331BD60B2E32EEF8E88DBAFA738D15D52A3B186E20FAACFCFA383D4B1CD5863B0719CCC332CCCEE0E9739720121C3AB0DE5C754D700A6ABC5953CFEB17BE1DD0E45E519D306A1AF01D89ACAE636DC73617A70AFD7D73926AADD6D1C6706ACAFE60AD8E411FA9D07BE96CC2C91CF22CC7C3AE32DBFF4F5660B9CDFFDB3E975E2169208F400A9017A669469EEAB21CCD0018D67FABCE18697AB775A7EFBE3A08EEF72E0C2A5E026A892799231BB592B23F02C1E829E445CACB29379D2FF6AC1B44DCE032ADBB67C585B0E182E5C9902EC8919BC4BA01E7E2568B0F4134B71529D8F56E0C2DFA4CAE327E14E6473DB59AB4A32294452284B27366A33CFF27DD341D89EC114F3929DC78B0CE719789617A0696AB0A75459FD7E2B25C", 16);
            signature = "3044022055386B167337291F0E404B142ED9A9DBF95AD9797C6C5CE550B7BD68BE5B075F022058D0DF7E3152DE1F7155BC76F3FB753B0605EE9D1D13B28AC0E428FB8C79003D";
        }
        else if (ModNumber.MaxMod == 2048 / 8) {
            dsaParameters.P = ModNumber.stomn("DFC017474A4FAF7A5E094FC31BD901731AD1823C5FAF5A06433AB3928FF9BC2EF5A0A90FC0AADB4FEB3294175089DCCE2509E2F7E5E7B7D7FB0CD54A025C6C6DCB599DA196BB9729B824BC811E67F6C1DB95F40DB8BD8A5C8F0C98A2B887127B975279B7F744D0392DD76350F989B2FF212B58557AADD9A718B8EF5453C71F2F4989EC458DF17F1126BE8FA0A48848F028A9808FBF524BF24E0960912640FD0A610C064A243299F992245DEBBAF084601ED8164875B0ABB7EDE528053A4753AC6C91CEB4DA8DE85DC80CDF4CF95A31A03667C5B774E9F7C9436E706F08C4F9FF58ABFECA29D255D732F57CCBB92CDDBD5A56DDAF50ADA60FF932239CCFF8B039", 16);
            dsaParameters.Q = ModNumber.stomn("8B50F6EA9476593FC9295A3D28CA0E80E46164C8E5F16E80E97007C8612CDE93", 16);
            dsaParameters.g = ModNumber.stomn("DDF1F59904EE5F22270307B45C75153D296965D17FA6C5A12F71D3FDC18BD7FB8C11337958500124550B3084E651FD62407C1E82374641742745169685DA6A9E2060D1A1BBC9E747A2B1A19A004BF527DE7859E75703111E81AD9A0DB62E78208E91CAF32635A0F669DC3A51ED18D8EBB5613355364CF2FBB4D16D2379E81532A7246D824B85AA74CDA49D7EB0582F44C75FDEB0709CEC1F826B8117B6D4348EF58CCE26443A383DD5717CD84F692C587D915B80481838BF27ABFD7B52D37443A52D912CF744FC313F8DDE5D322D054DF9A75717164EBB99CC345E9C39530C3776E8981B5FACB727F57A3BB072DA4C24DDB011B5E2BE8A2D77247B302ADFAB68", 16);
            dsaParameters.x = ModNumber.stomn("258A57C6E1F497C6B2D546E21B9C4F24A9666CE4DEF69E1666433B626B818437", 16);
            dsaParameters.y = ModNumber.stomn("9A071F438C87B44175A58DF2C0469B7048221E6D920B3983961BFF559099CC8E9C6D312EC9798E8F55CB4FCD85551DB50B91EA9BE12A8D709562D9253395692E9AF2E6F357F291AC5172818292979CDC4FD0A809833E431C5A29DD91D319EA62C1CE24A5F04558A0A918D8CB3D181B5A22D7BA8B5BCCC58120AE7D0D2C90A706C70BE62C5F0ED4D364D6F805A4D5C4F568F3772E5763411B153130EAB0CFC3D681E6C321B99D3EAA91CA3BCA2BD9F6C22A4169AE17750095A75C74FBC4C0A0FB5945DDE9FB2BA4868EDAE6B41940D8A47AF036A9E0DCEDF456DC48A61A0ABF9745F71CA6AEC24CC5783C76D2732662B351EDD3D98C6F83F84B472189C07DC519", 16);
            signature = "30440220218D53B69428D68138B2B4C66A2B6DB31CE00F16261299EE492D1A597B50341502202BB46382B6F71A663C0548692B2A7F970DE5B4691E6DE730E8CA61683EB11137";
        }
        else if (ModNumber.MaxMod == 1024 / 8) {
            dsaParameters.P = ModNumber.stomn("C18006893BCE75DC77605271C0AA43F5BFF9FD7B24F3DE9817A1B8230650542B1B30B66727EA3D44FBB476451D96B869555780D447729A7DC1ED7216B4F5C6A1A544F361723C3ACACBEFFA393389F42A7DE7F42106A30C04B0275DE550C8FF91392AA44FFBEBA94FE07CD19B50C5D66814BB31D741628503B0E579C76D32FF2D", 16);
            dsaParameters.Q = ModNumber.stomn("859D7C3729301E8865F41654C0041A3C562A682F", 16);
            dsaParameters.g = ModNumber.stomn("847AE11402C2FF443350DE8EB7665060DEC027A5AC1103C98C7F2CCD9C88D127185B7A0189F1674A0039FBD5DCFF4AE152F6C100D122ABF3B3E177F5693673408A0457C35667A3C80E51350F94F9696E8C5AC376FFC51E8CA9FD38169E6A1D0025EA1747D434C8DC8DE47B61E39AE13FCD8AB9ADEBEC982E5B3D249618CAD307", 16);
            dsaParameters.x = ModNumber.stomn("334F55231BC70F896CE4BB081583812440110F1A", 16);
            dsaParameters.y = ModNumber.stomn("3C7E5B1749C4D012C70A78D303CF9ED5C765DFDC0DB94DB0780704ABC0659B4D00BC13E02BE8CCB4F63E90EE76391CAFF69FCE225094275C912987AECCC21A52C3D2FE5FD338093CCFDEB35D097390609FC44AA8059CA745E161BB96A80AC0F93D6FDDEB2D0520AE3A6C8F7DBA4140B4057AF54C8AD5004FED3327FAB91136B9", 16);
            signature = "302D02144BB9FCEFAB5E1C25354ADD5873F2468C603027C902150080F7749B950D724EEB88384C4FFAC64F2E474A6C";
        }
        if (ModNumber.MaxMod < 4096 / 8) {
            String hash = "25BDECAE5C8BC7905CBBDA89485AFEC7C607D60AC0B1D4EA66C3CA01D7593D87";
            ModNumber mHash = ModNumber.stomn(hash, 16);
            byte[] hashBigEndian = mHash.convertEndianess(0);
            DSA dsa = new DSA(dsaParameters);
            assertTrue(dsa.verify(hashBigEndian, signature, true));
        }
    }
    @Test
    public void signAndVerifyDSASignature()
    {
        DSAParameters dsaParameters = new DSAParameters();
        if (ModNumber.MaxMod == 3072 / 8) {
            dsaParameters.P = ModNumber.stomn("BCAE0FB273D98B3CDDDFBB1BE44470A592AAC3EA0885D06C272C5333D8C62AA7F8E5A09D56FF462BFF1032870B306F1A997CE635B920630B128C9030EE5506B7C19992EEBEDE39892D0107E0F6BBC417FE1EF3F1FE436A30E7FB1FD9043D4BAC3A240D6E287FE33090351746BC25639FAD56D3E2922D47092D0EF4AC7B0F0F85E747CF55A11E702276A6C40E285C3FBB61E50755EE3F27764B748623EC67D8E2D8B2E5A6142D244A5C4500C3A580A174FDC9D48BDC9CD93AEF8930563A28FBCBA5E52D8DDCCCE9FDB249E910594B02AB4A9283DEE01B42B405E8D71EC0B613ADCBB7095991256F6B38AE318AFEBB2432D708BCB913E6D67729E8B0F97B30CB30F5223980F792B9C8ECBDB19236FDEE155F1D8F4D0540B63EED872599DFF8B48A3A8F3F966F6CDC389781DAAFBC7DBFF8F787F0ACE60680370F6C94C0DC615A437068583E7C31FF381E1BAB1F8311EA3FD588FD74D12ACC15E6217E118E76C2C66686215D07BE8A54EA754ECECA6377E0BDFA8289F1FC3380C517E0A2CD06B8A7", 16);
            dsaParameters.Q = ModNumber.stomn("BEF143E5D8624534038989D4AC6A76FC83A44E501C20BA8E6D3FC092F0EE36A7", 16);
            dsaParameters.g = ModNumber.stomn("402F21E6DCBC4614C35105555518FA9C7F1E78330AA227BE42F4E63C127A677D0736D814FC76F2E411B06EB860C6F9D7014193ED8E5052D02E8E0045940A3C9E4C5DE8C3219A27C912D439B7F8EDFD7039989A18C7254985608B667E642E8526A069CE050E488D94F8AA35BA272EADA29DD42D9C40C8E82A69C07E8CC29801C5463100631D51810306B60BF75BDCEFA779C6C4469575C0F8B4DC100AD74052DF124987E1C6859E03FD79CD4A1AF7BD58EEB3B86D124BEB77ED2FBF82F5104E3B3CB524842A24C8048BEBBB3D153F0A9FB9B98422F8144EB1D243C1ED82F858277E68ACC83D877CD6403573A88C2CA532899631C8D68E811C16939C663AB9A5A3A3DADBAB2587DA7796D620068B26C96FBCFDC616BC0AE10829D9F6ED207FA0B2B1DFE78E3BD9A1AD45C261DC3DB231AC34B971E0B3017509504949C743EBC1D47520459FACECCCB5F442E40CAF35390DFCDB524FE23508B03EDFEDB435DF9A0E4305CFF095E81CFEDA0C1F75B45DAA63A1DAE14AC31629A2E3C9CF6EF0191418", 16);
            dsaParameters.x = ModNumber.stomn("8BD650804F276A2EE4FAF859F63E25389614D8541119EF81CA482DEE4F312499", 16);
            dsaParameters.y = ModNumber.stomn("A3FCC9FDEE022201CD3F90D8348E71C4CB433B1015BCD63CE124763F94DCA05C431FCB1061445516436F6167B772EE4865189B019323D200FEBA62CB4EF7EA3F553F46699CDB8E4803290FF50145DAB9D77E98F50CEDF12FB3564D7F7B5C88874F7AF5BE5793331BD60B2E32EEF8E88DBAFA738D15D52A3B186E20FAACFCFA383D4B1CD5863B0719CCC332CCCEE0E9739720121C3AB0DE5C754D700A6ABC5953CFEB17BE1DD0E45E519D306A1AF01D89ACAE636DC73617A70AFD7D73926AADD6D1C6706ACAFE60AD8E411FA9D07BE96CC2C91CF22CC7C3AE32DBFF4F5660B9CDFFDB3E975E2169208F400A9017A669469EEAB21CCD0018D67FABCE18697AB775A7EFBE3A08EEF72E0C2A5E026A892799231BB592B23F02C1E829E445CACB29379D2FF6AC1B44DCE032ADBB67C585B0E182E5C9902EC8919BC4BA01E7E2568B0F4134B71529D8F56E0C2DFA4CAE327E14E6473DB59AB4A32294452284B27366A33CFF27DD341D89EC114F3929DC78B0CE719789617A0696AB0A75459FD7E2B25C", 16);
        }
        else if (ModNumber.MaxMod == 2048 / 8) {
            dsaParameters.P = ModNumber.stomn("DFC017474A4FAF7A5E094FC31BD901731AD1823C5FAF5A06433AB3928FF9BC2EF5A0A90FC0AADB4FEB3294175089DCCE2509E2F7E5E7B7D7FB0CD54A025C6C6DCB599DA196BB9729B824BC811E67F6C1DB95F40DB8BD8A5C8F0C98A2B887127B975279B7F744D0392DD76350F989B2FF212B58557AADD9A718B8EF5453C71F2F4989EC458DF17F1126BE8FA0A48848F028A9808FBF524BF24E0960912640FD0A610C064A243299F992245DEBBAF084601ED8164875B0ABB7EDE528053A4753AC6C91CEB4DA8DE85DC80CDF4CF95A31A03667C5B774E9F7C9436E706F08C4F9FF58ABFECA29D255D732F57CCBB92CDDBD5A56DDAF50ADA60FF932239CCFF8B039", 16);
            dsaParameters.Q = ModNumber.stomn("8B50F6EA9476593FC9295A3D28CA0E80E46164C8E5F16E80E97007C8612CDE93", 16);
            dsaParameters.g = ModNumber.stomn("DDF1F59904EE5F22270307B45C75153D296965D17FA6C5A12F71D3FDC18BD7FB8C11337958500124550B3084E651FD62407C1E82374641742745169685DA6A9E2060D1A1BBC9E747A2B1A19A004BF527DE7859E75703111E81AD9A0DB62E78208E91CAF32635A0F669DC3A51ED18D8EBB5613355364CF2FBB4D16D2379E81532A7246D824B85AA74CDA49D7EB0582F44C75FDEB0709CEC1F826B8117B6D4348EF58CCE26443A383DD5717CD84F692C587D915B80481838BF27ABFD7B52D37443A52D912CF744FC313F8DDE5D322D054DF9A75717164EBB99CC345E9C39530C3776E8981B5FACB727F57A3BB072DA4C24DDB011B5E2BE8A2D77247B302ADFAB68", 16);
            dsaParameters.x = ModNumber.stomn("258A57C6E1F497C6B2D546E21B9C4F24A9666CE4DEF69E1666433B626B818437", 16);
            dsaParameters.y = ModNumber.stomn("9A071F438C87B44175A58DF2C0469B7048221E6D920B3983961BFF559099CC8E9C6D312EC9798E8F55CB4FCD85551DB50B91EA9BE12A8D709562D9253395692E9AF2E6F357F291AC5172818292979CDC4FD0A809833E431C5A29DD91D319EA62C1CE24A5F04558A0A918D8CB3D181B5A22D7BA8B5BCCC58120AE7D0D2C90A706C70BE62C5F0ED4D364D6F805A4D5C4F568F3772E5763411B153130EAB0CFC3D681E6C321B99D3EAA91CA3BCA2BD9F6C22A4169AE17750095A75C74FBC4C0A0FB5945DDE9FB2BA4868EDAE6B41940D8A47AF036A9E0DCEDF456DC48A61A0ABF9745F71CA6AEC24CC5783C76D2732662B351EDD3D98C6F83F84B472189C07DC519", 16);
        }
        else if (ModNumber.MaxMod == 1024 / 8) {
            dsaParameters.P = ModNumber.stomn("C18006893BCE75DC77605271C0AA43F5BFF9FD7B24F3DE9817A1B8230650542B1B30B66727EA3D44FBB476451D96B869555780D447729A7DC1ED7216B4F5C6A1A544F361723C3ACACBEFFA393389F42A7DE7F42106A30C04B0275DE550C8FF91392AA44FFBEBA94FE07CD19B50C5D66814BB31D741628503B0E579C76D32FF2D", 16);
            dsaParameters.Q = ModNumber.stomn("859D7C3729301E8865F41654C0041A3C562A682F", 16);
            dsaParameters.g = ModNumber.stomn("847AE11402C2FF443350DE8EB7665060DEC027A5AC1103C98C7F2CCD9C88D127185B7A0189F1674A0039FBD5DCFF4AE152F6C100D122ABF3B3E177F5693673408A0457C35667A3C80E51350F94F9696E8C5AC376FFC51E8CA9FD38169E6A1D0025EA1747D434C8DC8DE47B61E39AE13FCD8AB9ADEBEC982E5B3D249618CAD307", 16);
            dsaParameters.x = ModNumber.stomn("334F55231BC70F896CE4BB081583812440110F1A", 16);
            dsaParameters.y = ModNumber.stomn("3C7E5B1749C4D012C70A78D303CF9ED5C765DFDC0DB94DB0780704ABC0659B4D00BC13E02BE8CCB4F63E90EE76391CAFF69FCE225094275C912987AECCC21A52C3D2FE5FD338093CCFDEB35D097390609FC44AA8059CA745E161BB96A80AC0F93D6FDDEB2D0520AE3A6C8F7DBA4140B4057AF54C8AD5004FED3327FAB91136B9", 16);
        }
        if (ModNumber.MaxMod < 4096 / 8) {
            String hash = "25BDECAE5C8BC7905CBBDA89485AFEC7C607D60AC0B1D4EA66C3CA01D7593D87";
            ModNumber mHash = ModNumber.stomn(hash, 16);
            byte[] hashBigEndian = mHash.convertEndianess(0);
            DSA dsa = new DSA(dsaParameters);
            String signature = dsa.sign(hashBigEndian,true);
            assertTrue(dsa.verify(hashBigEndian, signature, true));
        }
    }
    @Test
    public void rsaEncryptAndDecrypt()
    {
        RSAParameters rsaParameters = new RSAParameters();

        if (ModNumber.MaxMod == 4096/8) {
            rsaParameters.PubExp = ModNumber.stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.stomn("A26960E1102074E8E1E7D0754F6BED06BC5AAFEEA086704ACB33B99289FBC180874418D11E02B1B250313EE873E7307D932DA7C30A5E703CBE86EC588B318B46C20BD8BB01F51549635641E8958B0A109E59B06752ABED027539FBEA8BB700E58AD1985CE405F57BD01F4FB91E88BC8B50C4F0208EA02BB72612F41DE302DB3C7BFF5EB12C277417068CE2CA387BEBDE35320D045438951AD51D34CE1F52B10E1F7A9023E9C647582D266CEF6D2E067B60C1BED9C781ADC520223091210854A356C6CAE5B1874FDC59A643441C228F30E4329345AD96A2605972524BFEE8764D207435E42001C678028C48167610A6FCDDF43BC789DD04A6AC3B337E4F6D8085A404E759936355BCA8E7ED93AB6BB031830CEB586D1CC5A90E87F497B8096AC6ABE37BC701E33C9D9B61FBC8AF73446AAC90301ACDCE9E0DEBD322A032F1C90CA8D766285723B5DE918E65F1A987B32E2D51DB7E2C8EB8F2A28C7292E14BD56FB0EC779F44C138C9DF08BEDBD0568DC8FB9F65B7DD20CE80652C9194E160DCAA6C5F96BB5D357BE742B971B3F73923447C36C906E12147C0153CFB2551AB581513B38F2D3B90F2F3FDD17CC799224AA7DDBCAF41309008637CBAD18C27E226E849532B3CEE58CF3C9B3E147438935BCECB35C47B50E29623F76687A51792B75E077BC3D1240C73F6AB79B33577014C7BE2738BBE1F8B179517C5B5BEC23D7C9D", 16);
            rsaParameters.Prime1 = ModNumber.stomn("CDA901CDEFC7392972350E5160DCE36D9E3415ABA270EFA599B72EA3A36E4BECF727A46DDE4B39181D2A1B76CA2A79DBFCAEE2DF3D46648BB6F5766DE8FBDB9B97915A5D6BC9E3C2AB30DF0251B7DF103CB8BB9C8873188E395C01CD8C374D72B6B65FBFF47331232D6F86E3EC22B3F866D35C74DF6D76D35836363D020571C86104307324EE0A704FF5BB77DBBCDC7A2D98BBDB0C65B60F22F4EFD5EE8555715F2C79CCC438484C4E1B2136E881C2B04AB27E015AD354AECDF0D717A78205CA7DF0353F2312FD54EF8FB51DA12E35115AFDFA0659C4386DF841C0FF51BB5EC37D555CEDF88FC783FB568FA6F5736060837B4020CACD9D9BAC5F75348673EDCB", 16);
            rsaParameters.Prime2 = ModNumber.stomn("CA2A5948D61356DEDDCF8F5D180855C87E6981FDE13892F7CF593547B36D1CD5682699FD73D1036A24893EB2B8BCDBA8FC256F65E6644E2FC1E80C1BCADA0E21F6DED17EE6EA989BEB5E912E7FDB448573EC376817C4741C33EF78DCCE9DDE4F13FF6F29FB1117FF0873E5EB0B91AD52E41E2148DED2820C840574608F78E6D0E17DAF74F2B0A6171503EDA87D4B713819F86F07DE94CFFFBC2E8799DF78F138E28205AB7AF0286059980C241CD74C48FE897368E4CE6A096C4677BB2A37CF494395A1C3F695201863AFA632D2BBD61755779C20DD87AD78EE5E8E3EDA91D52FE2D0B91971197AA215EEE3E1F00DE0ED93B42E386C7685B71C1A25BDA0337237", 16);
            rsaParameters.Exp1 = ModNumber.stomn("92F32C22E62DC81E61489FCE6E13E73DB0492F22D09339E96E0881FED30C409C358DD1E3DF6E88813E7593E2315A60705C00EA82F0D997866D58BF2291D3447BCCB54388753870D1EC58A34098FB751BB90796A6A6A8F6E6C8FD811C4A1049FAA9AB6E874E37F0E85DD2CEE3951AC6D5BB435350FF44CC1EFF6CDD1C82913958205A49F43C730A6A6DF7FBD80CDEC9FB53114B499D761FBC7911E33D4D8D601BFE115D867EBE688A7D54CD4ADA9FBD691236AFEA6FE7891ECE704480D6640597469D98AE6075752677768BC77C1FF4334E7D2365DE4662BBD123CAAFA4CAE94F6E02CCC00D574F0C734ECA9E55CBF7157F5D55D918900B09ED04D84E8BB3D239", 16);
            rsaParameters.Exp2 = ModNumber.stomn("76DEF5C6CD116C07F9769CFE892E057AB151CA7C845C66964D964FACCC44C12A7E660C2FAA54E0E41D7D919F36DEDD6DFC6CEA968924BB907B3CBD39EE49CEFB8B9432ACA28C98953D8D4A62840D62B9F19298759D5C0745BA07FA12236F52133F57F77E40C12951ED1F70B68703EE5D0A02E961569B829A675A69B68945F6BA258C6889A455D82344591DF9EF4A57D53BF8071922786FE1183E67BF435672C816EF8AF3EC829FCB1F8336C859290312FC8FC78B8AD610256044030CCB632B8011BC3EF4E1DD8794AFB0CD519EF5918A00D826B6706BE1C574ABA68A30AC8524F601F284922B001FA433E64CA881109BC138B5E0B96DEB6BE29B414BC191DE7B", 16);
            rsaParameters.Coefficient = ModNumber.stomn("80B8FE74BA8F322BB18AD25B5EA75E1B50F54521E648960084FA90895634016A7C3D0997C52A1BC766C58BFFE8CD8953146A188D29480311E624048F83ADAEE157EFC4AE9EFFB144BA8F41E2CACDACD50A51367BB9D013E29B6C859AFC030CAE31B38740F287D61F4CDEB78659E91D404FD6976FD7F5E34C1B7FF530B655AA36161C928BD128D56A6FC1469CF8DB7D308CAFAE0AFDA03A71A399C9EC05889427AAE189EA9DD2BB4A4132FB16622FC9394759F337C8AAD5AF6E3BF659D0624EB10238BA58BDAF858ACF954FBFF03AD12FC0F55C79EDC318FC96D6BC53DEB0688E2C1B80572EC942E37848C75BDAEE6366FA5C11396F8E18DD6AB0F61AD71265D4", 16);
            rsaParameters.PrivExp = ModNumber.stomn("6DD1E9C365DF3C8E78AB02A740640F2B7DD3A8A12804A00BC3299FB2FA00B5508EA6BA4D3508C75576AA460A72CD0259F75A68D516BED310101935D98D8D9BCAE5CD92E8A9232B18ABC618DD07B9B3BD9705B8A5B51E322E764C160EFC186BCFCB357CDC5B07B87DD73644F73D3162A4A5A1F5695E67EA0777045BF3959C49D8B553F198C5ED8BAF422BA3F2EF8CC3EA4678289B23C3FAF6E0E255E18706A8588F6A95EAE2D9C34659896121E8E6B5555A10C857847328A861D91C94160D070647D8363C68F5C147938B3C72E2BF6FA9B4B48A271481061D367E7A2D8F196B5D2DB3C6F911B7819786C98AB93D6E772C077D6DAAB88B2DE8F20F39FB9003D061F9C8CB3A24B6F650C3C6393117201B765546B40453313045C0C67F9A404016F7073DE6F6877E7D9100EC77B7252D1E064EA99B102E73095D80049D127932C4A63D894DBF37446A5222CBA22B9B1B70B9C09C3EBA69EE4C302B6A59F7EE6C70C1F9283E9E171C14457FB78FAAC615AB15F691318A4CB4E55F1F1A1E635EAE217565A4D94A3505095232A4CB211E579122C2DCB67944A9E2514CEEA5C3E85CEFA916887B9D7B0BC4042EB4622A5E283566105E997FACFCC3EDBD152620E2A472E8CE47464BB0892642AE410CB9A5F283693E2F736444C54338978E21775E616BC2839208E89E5312CD26686C21E964F6AA29874E84FC1776751C1FAF0369B57075", 16);
        }
        else if (ModNumber.MaxMod == 2048/8) {
            rsaParameters.PubExp = ModNumber.stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.stomn("CA75BD1F951E545F8BA1BF6C985398C48BE5CD45E1794AD7D151CF8871D4088C32B1E88D8E4A9106714EACD38C284D70A1E15EBCC289BED8EE90C771B7A45A86DA92C7D5D4936782A701CCC6C7BF75A9CF22E2C079B2E94A4C7ED28B2983AD71B8D24D4E43501CA1FE0C15204A311BD4EEFC9B62A9D1078D27CC5C213A4D328002100FB350D8749DCCC982D736F6ED95B70B146152C966661FF48FFDF8CB8180E0A49D0D62EDED2FBE40D8C639B4F3817EDE2D7D5D9AAE4E0ECE362598DA62613E9B7B9BF7D93F6C232AE9BFC34142A65774DB2241D8B810734FED65C97370FC84599E002680901B55D0E991E51C6C4856FC361FD526193EB972E2C29467F211", 16);
            rsaParameters.Prime1 = ModNumber.stomn("E0162AC55685C9903231BE3A9A65D9C5E28F7D12683FD835B240DB86A0E3077ADA211CA866C00748430835882D586F44B0FF5784B4980C0EC73C52C7A195D2C514DC5A59E8683F6C0C4B614FD63F510505429928679F7807D7689E48966FBC29728CE81ACE99CDD69F48B0BE47E22AD0A65451FD80DE1E7BD93FF7755EA0FEFB", 16);
            rsaParameters.Prime2 = ModNumber.stomn("E74B192C59634F62C4E14DF1CF88B26163567EAB1A8DF1B4002CA14DAAEEF3F99DD2A8493DBA4BA79055300D1CA685A9964D69E3F5CEAB0CF728914B66DCDB41063170DDC0254088C33134EE4ACAF86AA4F50458804FC55F306654E221A33566FE9F2ECD22B72527DD402AC3A5FE715EBA7C4FB23998C3E59C2A35F08DB45563", 16);
            rsaParameters.Exp1 = ModNumber.stomn("4B3AE5ED0E59280C42EDB4419468312E601B700FE7F3D7305FC1AA718E3DE9DAF2B43F1CC23221BFBED158B77911C1793C3ADD34EE344588089755EB33724B2A63107F3CC3EB8BAF48895E0821936C2DD550407EF21AF57F1C704103798DD58BCCAB45990A575465836032060FB7FC4BC42B76C336AA5B6D94C806D9DE850A55", 16);
            rsaParameters.Exp2 = ModNumber.stomn("AA0EE371623FCC8CB86957DFE55182A66C5D9A40C00381A571435F4E35D1FF7C96F7DE580B94C02F71C5D0D0B33214B0BE1C29313CF839780CF9BCEA8DB4F1587BEF3A273131B2A35BB10053FC9603D49578B93B410C65F83923E461678E12A3A9AEECC05338098FA7F397FDC63CB0AA8FBEFAF1F5BB46BDB9F6A736395A5035", 16);
            rsaParameters.Coefficient = ModNumber.stomn("6F2C5D2D9A587AA5657032960ABBB1246A0D7B51196625693284F6B8B8222C5B2E8BE3139FEA29D20CC70FD6C8740CA5DE0A94C5ED1D56E6CCE8BC67AAE9153C57A02EEBDC5764B11608AED772B661243E265FBA989B74DB2B9DAC0CEC7AE9C322232C56AFD4BDD8746C8095BBCFDF04C553219092C8E243C94E84757FDA30AC", 16);
            rsaParameters.PrivExp = ModNumber.stomn("0B5BF1C17CDF57C2889609A471237795C7779C3DC879C2AE3B2B48E691E4609A6519C03F78F54C0FBD79B98EA0F57DEACCDA55336802CC776563301D7C3D376B0B8662B9E79461A15DF766E2DC402CBCDB474CCB99C864F86B344E61CA717DE0BC8AE815750760A3E21A11715F5844047D6824DBB3456C140A6097E6B15BB572899E5706451E48D95BE641E03162BFFCAF5272807661DA94CC9C6081DD179EE2C158F2B713C76EA71969AB83DA5F2EAA59DAC9E29B6F55F637B6C109139B0DEE731F36FF59C799357BB7732DF9C8F5D09085B8187E6FF016E3563833EA34ACD1867DEE6F7844749F32BDAD344E1DC7FE2E89A1696CC5D9E12E3902DBF0FE2265", 16);
        }
        else if (ModNumber.MaxMod == 1024/8) {
            rsaParameters.PubExp = ModNumber.stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.stomn("B08B5EF115AED8F91EE2FB82E7E8C8611DF101B4138B37787A18D1E72FC75437342A8961F7C462CBD2208DE6EC0472882CE6410BCE3AE096BB2CA0506B5FBF5865FC80465653F2AEA678C8A8638AF8CF98F105166B6CFA71A61567765DF9521FAE5616DBBF26F99512EEBF857C166941FC2402C5E27258B10206A8AF016C45E9", 16);
            rsaParameters.Prime1 = ModNumber.stomn("C882E9B30B9F87F47AD7653886EF5CD53D9CAA96D2B83C24C643F024B86CDFE61DE6627F0C63B3CBB885DD3212C77CBE47A3469E5EA2FAD245984A75D866AAF3", 16);
            rsaParameters.Prime2 = ModNumber.stomn("E1668918F4DD83B3066B32577933BD27B1FFA8F49907637863750304EFCBDB1DCC86362EEBC81ACD68B7E0C0BCE35B1BC0F353F16B59C5E7636AF1464805CAB3", 16);
            rsaParameters.Exp1 = ModNumber.stomn("A8130C8077D2BEECD0F7B2B242716C5896EFB712FC0950E8E684D64C818749DB23DF155B37F06F2AEA3ED39E809D55544ECA6F8C9AF974D4436017B256B620BF", 16);
            rsaParameters.Exp2 = ModNumber.stomn("74EDCA456611DC166D4802EF99F29B381280FC571763DBD842501B66DE3734CE7AA14D4E7E066AD110780362391B00D29F001C15EB88C0BFCEB555C10DEFE33D", 16);
            rsaParameters.Coefficient = ModNumber.stomn("89C0F663B05BC7C1CE547866B45F137DC73739DF9C159B5BA648A5C7818BC46A9CD12C41922A82DB150F9D34F08D69B4A94A947848E88DC5735800C882C856A1", 16);
            rsaParameters.PrivExp = ModNumber.stomn("9E7C2F39FBFE1FD7DC2B662009328717EFFA184E61311C15F27DEF893BF2141F0E9C9502369BBD193E446D3EFD67ACADA4A8FB81AE9C5A5BD621E4B4ECFF625469B82CE442C50E56F2C7E860FD7414AB46C9BA2C8F043FC2FAF5408E50A758BFDB2AF454020A1E77586C4F7E2D7CCF66E354715606B5223C31538AEDAAA0DB85", 16);
        }
        if (ModNumber.MaxMod != 3072/8) {
            RSA rsa = new RSA(rsaParameters);
            String message = "Dit is een test";
            ModNumber convertedMessage = ModNumber.fromText(message);
            ModNumber encryptedMessage = rsa.encrypt(convertedMessage, message.length() * Character.BYTES);
            ModNumber decryptedMessage = rsa.decrypt(encryptedMessage);
            String decryptedString = decryptedMessage.getText();
            assertEquals(message, decryptedString);
        }
    }
    @Test
    public void rsaSignAndVerify()
    {
        RSAParameters rsaParameters = new RSAParameters();

        if (ModNumber.MaxMod == 4096/8) {
            rsaParameters.PubExp = ModNumber.stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.stomn("A26960E1102074E8E1E7D0754F6BED06BC5AAFEEA086704ACB33B99289FBC180874418D11E02B1B250313EE873E7307D932DA7C30A5E703CBE86EC588B318B46C20BD8BB01F51549635641E8958B0A109E59B06752ABED027539FBEA8BB700E58AD1985CE405F57BD01F4FB91E88BC8B50C4F0208EA02BB72612F41DE302DB3C7BFF5EB12C277417068CE2CA387BEBDE35320D045438951AD51D34CE1F52B10E1F7A9023E9C647582D266CEF6D2E067B60C1BED9C781ADC520223091210854A356C6CAE5B1874FDC59A643441C228F30E4329345AD96A2605972524BFEE8764D207435E42001C678028C48167610A6FCDDF43BC789DD04A6AC3B337E4F6D8085A404E759936355BCA8E7ED93AB6BB031830CEB586D1CC5A90E87F497B8096AC6ABE37BC701E33C9D9B61FBC8AF73446AAC90301ACDCE9E0DEBD322A032F1C90CA8D766285723B5DE918E65F1A987B32E2D51DB7E2C8EB8F2A28C7292E14BD56FB0EC779F44C138C9DF08BEDBD0568DC8FB9F65B7DD20CE80652C9194E160DCAA6C5F96BB5D357BE742B971B3F73923447C36C906E12147C0153CFB2551AB581513B38F2D3B90F2F3FDD17CC799224AA7DDBCAF41309008637CBAD18C27E226E849532B3CEE58CF3C9B3E147438935BCECB35C47B50E29623F76687A51792B75E077BC3D1240C73F6AB79B33577014C7BE2738BBE1F8B179517C5B5BEC23D7C9D", 16);
            rsaParameters.Prime1 = ModNumber.stomn("CDA901CDEFC7392972350E5160DCE36D9E3415ABA270EFA599B72EA3A36E4BECF727A46DDE4B39181D2A1B76CA2A79DBFCAEE2DF3D46648BB6F5766DE8FBDB9B97915A5D6BC9E3C2AB30DF0251B7DF103CB8BB9C8873188E395C01CD8C374D72B6B65FBFF47331232D6F86E3EC22B3F866D35C74DF6D76D35836363D020571C86104307324EE0A704FF5BB77DBBCDC7A2D98BBDB0C65B60F22F4EFD5EE8555715F2C79CCC438484C4E1B2136E881C2B04AB27E015AD354AECDF0D717A78205CA7DF0353F2312FD54EF8FB51DA12E35115AFDFA0659C4386DF841C0FF51BB5EC37D555CEDF88FC783FB568FA6F5736060837B4020CACD9D9BAC5F75348673EDCB", 16);
            rsaParameters.Prime2 = ModNumber.stomn("CA2A5948D61356DEDDCF8F5D180855C87E6981FDE13892F7CF593547B36D1CD5682699FD73D1036A24893EB2B8BCDBA8FC256F65E6644E2FC1E80C1BCADA0E21F6DED17EE6EA989BEB5E912E7FDB448573EC376817C4741C33EF78DCCE9DDE4F13FF6F29FB1117FF0873E5EB0B91AD52E41E2148DED2820C840574608F78E6D0E17DAF74F2B0A6171503EDA87D4B713819F86F07DE94CFFFBC2E8799DF78F138E28205AB7AF0286059980C241CD74C48FE897368E4CE6A096C4677BB2A37CF494395A1C3F695201863AFA632D2BBD61755779C20DD87AD78EE5E8E3EDA91D52FE2D0B91971197AA215EEE3E1F00DE0ED93B42E386C7685B71C1A25BDA0337237", 16);
            rsaParameters.Exp1 = ModNumber.stomn("92F32C22E62DC81E61489FCE6E13E73DB0492F22D09339E96E0881FED30C409C358DD1E3DF6E88813E7593E2315A60705C00EA82F0D997866D58BF2291D3447BCCB54388753870D1EC58A34098FB751BB90796A6A6A8F6E6C8FD811C4A1049FAA9AB6E874E37F0E85DD2CEE3951AC6D5BB435350FF44CC1EFF6CDD1C82913958205A49F43C730A6A6DF7FBD80CDEC9FB53114B499D761FBC7911E33D4D8D601BFE115D867EBE688A7D54CD4ADA9FBD691236AFEA6FE7891ECE704480D6640597469D98AE6075752677768BC77C1FF4334E7D2365DE4662BBD123CAAFA4CAE94F6E02CCC00D574F0C734ECA9E55CBF7157F5D55D918900B09ED04D84E8BB3D239", 16);
            rsaParameters.Exp2 = ModNumber.stomn("76DEF5C6CD116C07F9769CFE892E057AB151CA7C845C66964D964FACCC44C12A7E660C2FAA54E0E41D7D919F36DEDD6DFC6CEA968924BB907B3CBD39EE49CEFB8B9432ACA28C98953D8D4A62840D62B9F19298759D5C0745BA07FA12236F52133F57F77E40C12951ED1F70B68703EE5D0A02E961569B829A675A69B68945F6BA258C6889A455D82344591DF9EF4A57D53BF8071922786FE1183E67BF435672C816EF8AF3EC829FCB1F8336C859290312FC8FC78B8AD610256044030CCB632B8011BC3EF4E1DD8794AFB0CD519EF5918A00D826B6706BE1C574ABA68A30AC8524F601F284922B001FA433E64CA881109BC138B5E0B96DEB6BE29B414BC191DE7B", 16);
            rsaParameters.Coefficient = ModNumber.stomn("80B8FE74BA8F322BB18AD25B5EA75E1B50F54521E648960084FA90895634016A7C3D0997C52A1BC766C58BFFE8CD8953146A188D29480311E624048F83ADAEE157EFC4AE9EFFB144BA8F41E2CACDACD50A51367BB9D013E29B6C859AFC030CAE31B38740F287D61F4CDEB78659E91D404FD6976FD7F5E34C1B7FF530B655AA36161C928BD128D56A6FC1469CF8DB7D308CAFAE0AFDA03A71A399C9EC05889427AAE189EA9DD2BB4A4132FB16622FC9394759F337C8AAD5AF6E3BF659D0624EB10238BA58BDAF858ACF954FBFF03AD12FC0F55C79EDC318FC96D6BC53DEB0688E2C1B80572EC942E37848C75BDAEE6366FA5C11396F8E18DD6AB0F61AD71265D4", 16);
            rsaParameters.PrivExp = ModNumber.stomn("6DD1E9C365DF3C8E78AB02A740640F2B7DD3A8A12804A00BC3299FB2FA00B5508EA6BA4D3508C75576AA460A72CD0259F75A68D516BED310101935D98D8D9BCAE5CD92E8A9232B18ABC618DD07B9B3BD9705B8A5B51E322E764C160EFC186BCFCB357CDC5B07B87DD73644F73D3162A4A5A1F5695E67EA0777045BF3959C49D8B553F198C5ED8BAF422BA3F2EF8CC3EA4678289B23C3FAF6E0E255E18706A8588F6A95EAE2D9C34659896121E8E6B5555A10C857847328A861D91C94160D070647D8363C68F5C147938B3C72E2BF6FA9B4B48A271481061D367E7A2D8F196B5D2DB3C6F911B7819786C98AB93D6E772C077D6DAAB88B2DE8F20F39FB9003D061F9C8CB3A24B6F650C3C6393117201B765546B40453313045C0C67F9A404016F7073DE6F6877E7D9100EC77B7252D1E064EA99B102E73095D80049D127932C4A63D894DBF37446A5222CBA22B9B1B70B9C09C3EBA69EE4C302B6A59F7EE6C70C1F9283E9E171C14457FB78FAAC615AB15F691318A4CB4E55F1F1A1E635EAE217565A4D94A3505095232A4CB211E579122C2DCB67944A9E2514CEEA5C3E85CEFA916887B9D7B0BC4042EB4622A5E283566105E997FACFCC3EDBD152620E2A472E8CE47464BB0892642AE410CB9A5F283693E2F736444C54338978E21775E616BC2839208E89E5312CD26686C21E964F6AA29874E84FC1776751C1FAF0369B57075", 16);
        }
        else if (ModNumber.MaxMod == 2048/8) {
            rsaParameters.PubExp = ModNumber.stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.stomn("CA75BD1F951E545F8BA1BF6C985398C48BE5CD45E1794AD7D151CF8871D4088C32B1E88D8E4A9106714EACD38C284D70A1E15EBCC289BED8EE90C771B7A45A86DA92C7D5D4936782A701CCC6C7BF75A9CF22E2C079B2E94A4C7ED28B2983AD71B8D24D4E43501CA1FE0C15204A311BD4EEFC9B62A9D1078D27CC5C213A4D328002100FB350D8749DCCC982D736F6ED95B70B146152C966661FF48FFDF8CB8180E0A49D0D62EDED2FBE40D8C639B4F3817EDE2D7D5D9AAE4E0ECE362598DA62613E9B7B9BF7D93F6C232AE9BFC34142A65774DB2241D8B810734FED65C97370FC84599E002680901B55D0E991E51C6C4856FC361FD526193EB972E2C29467F211", 16);
            rsaParameters.Prime1 = ModNumber.stomn("E0162AC55685C9903231BE3A9A65D9C5E28F7D12683FD835B240DB86A0E3077ADA211CA866C00748430835882D586F44B0FF5784B4980C0EC73C52C7A195D2C514DC5A59E8683F6C0C4B614FD63F510505429928679F7807D7689E48966FBC29728CE81ACE99CDD69F48B0BE47E22AD0A65451FD80DE1E7BD93FF7755EA0FEFB", 16);
            rsaParameters.Prime2 = ModNumber.stomn("E74B192C59634F62C4E14DF1CF88B26163567EAB1A8DF1B4002CA14DAAEEF3F99DD2A8493DBA4BA79055300D1CA685A9964D69E3F5CEAB0CF728914B66DCDB41063170DDC0254088C33134EE4ACAF86AA4F50458804FC55F306654E221A33566FE9F2ECD22B72527DD402AC3A5FE715EBA7C4FB23998C3E59C2A35F08DB45563", 16);
            rsaParameters.Exp1 = ModNumber.stomn("4B3AE5ED0E59280C42EDB4419468312E601B700FE7F3D7305FC1AA718E3DE9DAF2B43F1CC23221BFBED158B77911C1793C3ADD34EE344588089755EB33724B2A63107F3CC3EB8BAF48895E0821936C2DD550407EF21AF57F1C704103798DD58BCCAB45990A575465836032060FB7FC4BC42B76C336AA5B6D94C806D9DE850A55", 16);
            rsaParameters.Exp2 = ModNumber.stomn("AA0EE371623FCC8CB86957DFE55182A66C5D9A40C00381A571435F4E35D1FF7C96F7DE580B94C02F71C5D0D0B33214B0BE1C29313CF839780CF9BCEA8DB4F1587BEF3A273131B2A35BB10053FC9603D49578B93B410C65F83923E461678E12A3A9AEECC05338098FA7F397FDC63CB0AA8FBEFAF1F5BB46BDB9F6A736395A5035", 16);
            rsaParameters.Coefficient = ModNumber.stomn("6F2C5D2D9A587AA5657032960ABBB1246A0D7B51196625693284F6B8B8222C5B2E8BE3139FEA29D20CC70FD6C8740CA5DE0A94C5ED1D56E6CCE8BC67AAE9153C57A02EEBDC5764B11608AED772B661243E265FBA989B74DB2B9DAC0CEC7AE9C322232C56AFD4BDD8746C8095BBCFDF04C553219092C8E243C94E84757FDA30AC", 16);
            rsaParameters.PrivExp = ModNumber.stomn("0B5BF1C17CDF57C2889609A471237795C7779C3DC879C2AE3B2B48E691E4609A6519C03F78F54C0FBD79B98EA0F57DEACCDA55336802CC776563301D7C3D376B0B8662B9E79461A15DF766E2DC402CBCDB474CCB99C864F86B344E61CA717DE0BC8AE815750760A3E21A11715F5844047D6824DBB3456C140A6097E6B15BB572899E5706451E48D95BE641E03162BFFCAF5272807661DA94CC9C6081DD179EE2C158F2B713C76EA71969AB83DA5F2EAA59DAC9E29B6F55F637B6C109139B0DEE731F36FF59C799357BB7732DF9C8F5D09085B8187E6FF016E3563833EA34ACD1867DEE6F7844749F32BDAD344E1DC7FE2E89A1696CC5D9E12E3902DBF0FE2265", 16);
        }
        else if (ModNumber.MaxMod == 1024/8) {
            rsaParameters.PubExp = ModNumber.stomn("010001", 16);
            rsaParameters.Modulus = ModNumber.stomn("B08B5EF115AED8F91EE2FB82E7E8C8611DF101B4138B37787A18D1E72FC75437342A8961F7C462CBD2208DE6EC0472882CE6410BCE3AE096BB2CA0506B5FBF5865FC80465653F2AEA678C8A8638AF8CF98F105166B6CFA71A61567765DF9521FAE5616DBBF26F99512EEBF857C166941FC2402C5E27258B10206A8AF016C45E9", 16);
            rsaParameters.Prime1 = ModNumber.stomn("C882E9B30B9F87F47AD7653886EF5CD53D9CAA96D2B83C24C643F024B86CDFE61DE6627F0C63B3CBB885DD3212C77CBE47A3469E5EA2FAD245984A75D866AAF3", 16);
            rsaParameters.Prime2 = ModNumber.stomn("E1668918F4DD83B3066B32577933BD27B1FFA8F49907637863750304EFCBDB1DCC86362EEBC81ACD68B7E0C0BCE35B1BC0F353F16B59C5E7636AF1464805CAB3", 16);
            rsaParameters.Exp1 = ModNumber.stomn("A8130C8077D2BEECD0F7B2B242716C5896EFB712FC0950E8E684D64C818749DB23DF155B37F06F2AEA3ED39E809D55544ECA6F8C9AF974D4436017B256B620BF", 16);
            rsaParameters.Exp2 = ModNumber.stomn("74EDCA456611DC166D4802EF99F29B381280FC571763DBD842501B66DE3734CE7AA14D4E7E066AD110780362391B00D29F001C15EB88C0BFCEB555C10DEFE33D", 16);
            rsaParameters.Coefficient = ModNumber.stomn("89C0F663B05BC7C1CE547866B45F137DC73739DF9C159B5BA648A5C7818BC46A9CD12C41922A82DB150F9D34F08D69B4A94A947848E88DC5735800C882C856A1", 16);
            rsaParameters.PrivExp = ModNumber.stomn("9E7C2F39FBFE1FD7DC2B662009328717EFFA184E61311C15F27DEF893BF2141F0E9C9502369BBD193E446D3EFD67ACADA4A8FB81AE9C5A5BD621E4B4ECFF625469B82CE442C50E56F2C7E860FD7414AB46C9BA2C8F043FC2FAF5408E50A758BFDB2AF454020A1E77586C4F7E2D7CCF66E354715606B5223C31538AEDAAA0DB85", 16);
        }
        if (ModNumber.MaxMod != 3072/8) {
            RSA rsa = new RSA(rsaParameters);
            int[] hashBigEndianInts = { 0x0087, 0x003D, 0x0059, 0x00D7, 0x0001, 0x00CA, 0x00C3, 0x0066, 0x00EA, 0x00D4, 0x00B1, 0x00C0, 0x000A, 0x00D6, 0x0007, 0x00C6, 0x00C7, 0x00FE, 0x005A, 0x0048, 0x0089, 0x00DA, 0x00BB, 0x005C, 0x0090, 0x00C7, 0x008B, 0x005C, 0x00AE, 0x00EC, 0x00BD, 0x0025 };
            byte[] hashBigEndian = new byte[hashBigEndianInts.length];
            for (int i = 0; i < hashBigEndian.length; i++)
                hashBigEndian[i] = (byte) hashBigEndianInts[i];
            ModNumber signature = rsa.encryptSignature(hashBigEndian, "2.16.840.1.101.3.4.2.1");
            ModNumber decryptedHash = rsa.decryptSignature(signature);
            byte[] hashLittleEndian = new byte[hashBigEndian.length];
            for (int i = 0; i < hashLittleEndian.length; i++)
                hashLittleEndian[i] = (byte)hashBigEndian[hashBigEndian.length - 1 - i];
            ModNumber originalHash = new ModNumber(hashLittleEndian);
            assertEquals(originalHash, decryptedHash);
        }
    }
    @Test
    public void ecIsOnCurveA0B17Point00isFalse()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ECPoint pt = new ECPoint();
        pt.x = mzero;
        pt.y = mzero;
        assertFalse(myEC.IsOnCurve(pt));

    }
    @Test
    public void ecIsOnCurveA0B17PointMinus2And3isTrue()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ECPoint pt = new ECPoint();
        pt.x = mgm.Diff(mzero, new ModNumber(2L));
        pt.y = new ModNumber(3L);
        assertTrue(myEC.IsOnCurve(pt));

    }
    @Test
    public void ecIsOnCurveA0B17PointMinus2AndMinus3isTrue()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ECPoint pt = new ECPoint();
        pt.x = mgm.Diff(mzero, new ModNumber(2L));
        pt.y = mgm.Diff(mzero, new ModNumber(3L));
        assertTrue(myEC.IsOnCurve(pt));

    }
    @Test
    public void ecIsOnCurveA0B17PointMinus2And4isFalse()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ECPoint pt = new ECPoint();
        pt.x = mgm.Diff(mzero, new ModNumber(2L));
        pt.y = new ModNumber(4L);
        assertFalse(myEC.IsOnCurve(pt));

    }
    @Test
    public void ecIsOnCurveA0B17PointMinus1And4isTrue()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ECPoint pt = new ECPoint();
        pt.x = mgm.Diff(mzero, new ModNumber(1L));
        pt.y = new ModNumber(4L);
        assertTrue(myEC.IsOnCurve(pt));

    }
    @Test
    public void ecIsOnCurveA0B17PointMinus1AndMinus4isTrue()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ECPoint pt = new ECPoint();
        pt.x = mgm.Diff(mzero, new ModNumber(1L));
        pt.y = mgm.Diff(mzero, new ModNumber(4L));
        assertTrue(myEC.IsOnCurve(pt));

    }
    @Test
    public void ecIsOnCurveA0B17Point2And5isTrue()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(2L);
        pt.y = new ModNumber(5L);
        assertTrue(myEC.IsOnCurve(pt));

    }
    @Test
    public void ecIsOnCurveA0B17Point2AndMinus5isTrue()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(2L);
        pt.y = mgm.Diff(mzero, new ModNumber(5L));
        assertTrue(myEC.IsOnCurve(pt));

    }
    @Test
    public void ecCalculateYOnCurveA0B17Point2is5()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ModNumber x = new ModNumber(2L);
        ModNumber exp = new ModNumber(5L);
        ModNumber y = myEC.CalculateY(x);
        assertEquals(exp, y);

    }
    @Test
    public void ecCalculateYOnCurveA0B17Point52is375()
    {
        ModNumber p = new ModNumber(1000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ModNumber x = new ModNumber(52L);
        ModNumber exp = new ModNumber(375L);
        ModNumber y = myEC.CalculateY(x);
        assertEquals(exp, y);

    }
    @Test
    public void ecCalculateYOnCurveA0B17Point5234is378661()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mzero, new ModNumber(17L));
        ModNumber x = new ModNumber(5234L);
        ModNumber exp = new ModNumber(378661L);
        ModNumber y = myEC.CalculateY(x);
        assertEquals(exp, y);

    }
    @Test
    public void ecCalculateYOnCurveMinus2B0Point0is0()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ModNumber x = new ModNumber(0L);
        ModNumber exp = new ModNumber(0L);
        ModNumber y = myEC.CalculateY(x);
        assertEquals(exp, y);

    }
    @Test
    public void ecCalculateYOnCurveMinus2B0PointMinus1is1()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ModNumber x = mgm.Diff(mzero, new ModNumber(1L));
        ModNumber exp = new ModNumber(1L);
        ModNumber y = myEC.CalculateY(x);
        assertEquals(exp, y);

    }
    @Test
    public void ecCalculateYOnCurveMinus2B0Point2is2()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ModNumber x = new ModNumber(2L);
        ModNumber exp = new ModNumber(2L);
        ModNumber y = myEC.CalculateY(x);
        assertEquals(exp, y);

    }
    @Test
    public void ecCalculateYOnCurveMinus2B0Point338is6214()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ModNumber x = new ModNumber(338L);
        ModNumber exp = new ModNumber(6214L);
        ModNumber y = myEC.CalculateY(x);
        assertEquals(exp, y);

    }
    @Test
    public void ecIsOnCurveAMinus2B0Point338AndMinus6214()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(338L);
        pt.y = mgm.Diff(mzero, new ModNumber(6214L));
        assertTrue(myEC.IsOnCurve(pt));
    }
    @Test
    public void ecAddCurveAMinus2B0Point00And00IsAtInfinity()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ECPoint pt = new ECPoint();
        pt.x = mzero;
        pt.y = mzero;
        ECPoint exp = new ECPoint();
        exp.IsAtInfinity = true;
        assertEquals(exp, myEC.Add(pt, pt));
    }
    @Test
    public void ecAddCurveAMinus2B0Point00AndAtInfinityIs00()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ECPoint pt1 = new ECPoint();
        pt1.x = mzero;
        pt1.y = mzero;
        ECPoint pt2 = new ECPoint();
        pt2.IsAtInfinity = true;
        ECPoint exp = new ECPoint();
        exp.x = mzero;
        exp.y = mzero;
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecCurveAMinus2B0Point00Mult3IsPoint00()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ECPoint pt1 = new ECPoint();
        pt1.x = mzero;
        pt1.y = mzero;
        ECPoint exp = new ECPoint();
        exp.x = mzero;
        exp.y = mzero;
        assertEquals(exp, myEC.Mult(pt1, new ModNumber(3L)));
    }
    @Test
    public void ecAddCurveAMinus2B0PointAtInfinityAndAtInfinityIsAtInfinity()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ECPoint pt1 = new ECPoint();
        pt1.IsAtInfinity = true;
        ECPoint pt2 = new ECPoint();
        pt2.IsAtInfinity = true;
        ECPoint exp = new ECPoint();
        exp.IsAtInfinity = true;
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecAddCurveAMinus2B0Point00AndMinus1And1IsPoint22()
    {
        ModNumber p = new ModNumber(0x10000000000L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ECPoint pt1 = new ECPoint();
        pt1.x = mzero;
        pt1.y = mzero;
        ECPoint pt2 = new ECPoint();
        pt2.x = mgm.Diff(mzero, new ModNumber(1L));
        pt2.y = new ModNumber(1L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(2L);
        exp.y = new ModNumber(2L);
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecECAddCurveAMinus2B0Point00AndPoint22IsPointMinus1And1()
    {
        ModNumber p = new ModNumber(65535L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(2L)), mzero);
        ECPoint pt1 = new ECPoint();
        pt1.x = mzero;
        pt1.y = mzero;
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(2L);
        pt2.y = new ModNumber(2L);
        ECPoint exp = new ECPoint();
        exp.x = mgm.Diff(mzero, new ModNumber(1L));
        exp.y = new ModNumber(1L);
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecIsOnCurveP19AMinus7B10Point12()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(1L);
        pt.y = new ModNumber(2L);
        assertTrue(myEC.IsOnCurve(pt));
    }
    @Test
    public void ecIsOnCurveP19AMinus7B10Point1And17()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(1L);
        pt.y = new ModNumber(17L);
        assertTrue(myEC.IsOnCurve(pt));
    }
    @Test
    public void ecIsOnCurveP19AMinus7B10Point22()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(2L);
        pt.y = new ModNumber(2L);
        assertTrue(myEC.IsOnCurve(pt));
    }
    @Test
    public void ecIsOnCurveP19AMinus7B10Point2And17()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(2L);
        pt.y = new ModNumber(17L);
        assertTrue(myEC.IsOnCurve(pt));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point22AndPoint2And17IsPtAtInfinity()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt1 = new ECPoint();
        pt1.x = new ModNumber(2L);
        pt1.y = new ModNumber(2L);
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(2L);
        pt2.y = new ModNumber(17L);
        ECPoint exp = new ECPoint();
        exp.IsAtInfinity = true;
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point12AndPoint2And17IsPt13And8()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt1 = new ECPoint();
        pt1.x = new ModNumber(1L);
        pt1.y = new ModNumber(2L);
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(2L);
        pt2.y = new ModNumber(17L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(13L);
        exp.y = new ModNumber(8L);
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point1And17AndPoint2And17IsPt16And2()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt1 = new ECPoint();
        pt1.x = new ModNumber(1L);
        pt1.y = new ModNumber(17L);
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(2L);
        pt2.y = new ModNumber(17L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(16L);
        exp.y = new ModNumber(2L);
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point1And17AndPoint22IsPt13And11()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt1 = new ECPoint();
        pt1.x = new ModNumber(1L);
        pt1.y = new ModNumber(17L);
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(2L);
        pt2.y = new ModNumber(2L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(13L);
        exp.y = new ModNumber(11L);
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point1And17AndPoint97IsPt70()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt1 = new ECPoint();
        pt1.x = new ModNumber(1L);
        pt1.y = new ModNumber(17L);
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(9L);
        pt2.y = new ModNumber(7L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(7L);
        exp.y = new ModNumber(0L);
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point1And17Times3EqualToPt1And17AddPt18And4()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt1 = new ECPoint();
        pt1.x = new ModNumber(1L);
        pt1.y = new ModNumber(17L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(9L);
        exp.y = new ModNumber(7L);
        assertEquals(exp, myEC.Mult(pt1, new ModNumber(3L)));
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(18L);
        pt2.y = new ModNumber(4L);
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point1And17Times4EqualToPt1And17AddPt12EqualToPt1And17Times2Times2()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt1 = new ECPoint();
        pt1.x = new ModNumber(1L);
        pt1.y = new ModNumber(17L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(7L);
        exp.y = new ModNumber(0L);
        assertEquals(exp, myEC.Mult(pt1, new ModNumber(4L)));
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(9L);
        pt2.y = new ModNumber(7L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        ECPoint pt3 = new ECPoint();
        pt3.x = new ModNumber(18L);
        pt3.y = new ModNumber(4L);
        assertEquals(exp, myEC.Times2(pt3));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point1And17SubGroup()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(1L);
        pt.y = new ModNumber(17L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(18L);
        exp.y = new ModNumber(4L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(2L)));
        exp.x = new ModNumber(9L);
        exp.y = new ModNumber(7L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(3L)));
        exp.x = new ModNumber(7L);
        exp.y = new ModNumber(0L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(4L)));
        exp.x = new ModNumber(9L);
        exp.y = new ModNumber(12L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(5L)));
        exp.x = new ModNumber(18L);
        exp.y = new ModNumber(15L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(6L)));
        exp.x = new ModNumber(1L);
        exp.y = new ModNumber(2L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(7L)));
        exp.IsAtInfinity= true;
        assertEquals(exp, myEC.Mult(pt, new ModNumber(8L)));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point1And17CoSet1()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt1 = new ECPoint();
        pt1.x = new ModNumber(2L);
        pt1.y = new ModNumber(2L);
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(1L);
        pt2.y = new ModNumber(17L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(13L);
        exp.y = new ModNumber(11L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(10L);
        exp.y = new ModNumber(16L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(12L);
        exp.y = new ModNumber(18L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(17L);
        exp.y = new ModNumber(4L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(5L);
        exp.y = new ModNumber(10L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(3L);
        exp.y = new ModNumber(15L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(16L);
        exp.y = new ModNumber(17L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(2L);
        exp.y = new ModNumber(2L);
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point1And17CoSet2()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt1 = new ECPoint();
        pt1.x = new ModNumber(2L);
        pt1.y = new ModNumber(17L);
        ECPoint pt2 = new ECPoint();
        pt2.x = new ModNumber(1L);
        pt2.y = new ModNumber(17L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(16L);
        exp.y = new ModNumber(2L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(3L);
        exp.y = new ModNumber(4L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(5L);
        exp.y = new ModNumber(9L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(17L);
        exp.y = new ModNumber(15L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(12L);
        exp.y = new ModNumber(1L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(10L);
        exp.y = new ModNumber(3L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(13L);
        exp.y = new ModNumber(8L);
        assertEquals(exp, myEC.Add(pt1, pt2));
        pt1.x = exp.x;
        pt1.y = exp.y;
        exp.x = new ModNumber(2L);
        exp.y = new ModNumber(17L);
        assertEquals(exp, myEC.Add(pt1, pt2));
    }
    @Test
    public void ecAddCurveP19AMinus7B10Point22SubGroup()
    {
        ModNumber p = new ModNumber(19L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(2L);
        pt.y = new ModNumber(2L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(13L);
        exp.y = new ModNumber(8L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(2L)));
        exp.x = new ModNumber(1L);
        exp.y = new ModNumber(2L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(3L)));
        exp.x = new ModNumber(16L);
        exp.y = new ModNumber(17L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(4L)));
        exp.x = new ModNumber(10L);
        exp.y = new ModNumber(3L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(5L)));
        exp.x = new ModNumber(18L);
        exp.y = new ModNumber(15L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(6L)));
        exp.x = new ModNumber(3L);
        exp.y = new ModNumber(15L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(7L)));
        exp.x = new ModNumber(12L);
        exp.y = new ModNumber(1L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(8L)));
        exp.x = new ModNumber(9L);
        exp.y = new ModNumber(12L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(9L)));
        exp.x = new ModNumber(5L);
        exp.y = new ModNumber(10L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(10L)));
        exp.x = new ModNumber(17L);
        exp.y = new ModNumber(15L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(11L)));
        exp.x = new ModNumber(7L);
        exp.y = new ModNumber(0L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(12L)));
        exp.x = new ModNumber(17L);
        exp.y = new ModNumber(4L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(13L)));
        exp.x = new ModNumber(5L);
        exp.y = new ModNumber(9L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(14L)));
        exp.x = new ModNumber(9L);
        exp.y = new ModNumber(7L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(15L)));
        exp.x = new ModNumber(12L);
        exp.y = new ModNumber(18L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(16L)));
        exp.x = new ModNumber(3L);
        exp.y = new ModNumber(4L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(17L)));
        exp.x = new ModNumber(18L);
        exp.y = new ModNumber(4L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(18L)));
        exp.x = new ModNumber(10L);
        exp.y = new ModNumber(16L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(19L)));
        exp.x = new ModNumber(16L);
        exp.y = new ModNumber(2L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(20L)));
        exp.x = new ModNumber(1L);
        exp.y = new ModNumber(17L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(21L)));
        exp.x = new ModNumber(13L);
        exp.y = new ModNumber(11L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(22L)));
        exp.x = new ModNumber(2L);
        exp.y = new ModNumber(17L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(23L)));
        exp.IsAtInfinity= true;
        assertEquals(exp, myEC.Mult(pt, new ModNumber(24L)));
    }
    @Test
    public void ecMultCurveP97AMinus7B10Point12IsOnCurve()
    {
        ModNumber p = new ModNumber(97L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(1L);
        pt.y = new ModNumber(2L);
        assertTrue(myEC.IsOnCurve(pt));
    }
    @Test
    public void ecMultCurveP97AMinus7B10Point22IsOnCurve()
    {
        ModNumber p = new ModNumber(97L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(2L);
        pt.y = new ModNumber(2L);
        assertTrue(myEC.IsOnCurve(pt));
    }
    @Test
    public void ecMultCurveP97AMinus7B10Point12SubGroup()
    {
        ModNumber p = new ModNumber(97L);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.IsAtInfinity = true;
        ModNumber mzero = new ModNumber(0L);
        EC myEC = new EC(mgm, g, mzero, mgm.Diff(mzero, new ModNumber(7L)), new ModNumber(10L));
        ECPoint pt = new ECPoint();
        pt.x = new ModNumber(1L);
        pt.y = new ModNumber(2L);
        ECPoint exp = new ECPoint();
        exp.x = new ModNumber(96L);
        exp.y = new ModNumber(93L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(2L)));
        exp.x = new ModNumber(9L);
        exp.y = new ModNumber(71L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(3L)));
        exp.x = new ModNumber(75L);
        exp.y = new ModNumber(63L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(4L)));
        exp.x = new ModNumber(24L);
        exp.y = new ModNumber(59L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(5L)));
        exp.x = new ModNumber(29L);
        exp.y = new ModNumber(72L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(6L)));
        exp.x = new ModNumber(49L);
        exp.y = new ModNumber(72L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(7L)));
        exp.x = new ModNumber(53L);
        exp.y = new ModNumber(3L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(8L)));
        exp.x = new ModNumber(51L);
        exp.y = new ModNumber(53L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(9L)));
        exp.x = new ModNumber(37L);
        exp.y = new ModNumber(35L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(10L)));
        exp.x = new ModNumber(43L);
        exp.y = new ModNumber(8L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(11L)));
        exp.x = new ModNumber(55L);
        exp.y = new ModNumber(18L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(12L)));
        exp.x = new ModNumber(19L);
        exp.y = new ModNumber(25L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(13L)));
        exp.x = new ModNumber(46L);
        exp.y = new ModNumber(86L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(14L)));
        exp.x = new ModNumber(44L);
        exp.y = new ModNumber(60L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(15L)));
        exp.x = new ModNumber(63L);
        exp.y = new ModNumber(61L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(16L)));
        exp.x = new ModNumber(36L);
        exp.y = new ModNumber(57L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(17L)));
        exp.x = new ModNumber(11L);
        exp.y = new ModNumber(10L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(18L)));
        exp.x = new ModNumber(74L);
        exp.y = new ModNumber(56L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(19L)));
        exp.x = new ModNumber(21L);
        exp.y = new ModNumber(43L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(20L)));
        exp.x = new ModNumber(13L);
        exp.y = new ModNumber(51L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(21L)));
        exp.x = new ModNumber(2L);
        exp.y = new ModNumber(2L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(22L)));
        exp.x = new ModNumber(94L);
        exp.y = new ModNumber(95L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(23L)));
        exp.x = new ModNumber(3L);
        exp.y = new ModNumber(93L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(24L)));
        exp.x = new ModNumber(5L);
        exp.y = new ModNumber(10L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(25L)));
        exp.x = new ModNumber(95L);
        exp.y = new ModNumber(4L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(26L)));
        exp.x = new ModNumber(23L);
        exp.y = new ModNumber(45L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(27L)));
        exp.x = new ModNumber(67L);
        exp.y = new ModNumber(63L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(28L)));
        exp.x = new ModNumber(79L);
        exp.y = new ModNumber(67L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(29L)));
        exp.x = new ModNumber(15L);
        exp.y = new ModNumber(51L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(30L)));
        exp.x = new ModNumber(69L);
        exp.y = new ModNumber(51L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(31L)));
        exp.x = new ModNumber(52L);
        exp.y = new ModNumber(34L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(32L)));
        exp.x = new ModNumber(41L);
        exp.y = new ModNumber(68L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(33L)));
        exp.x = new ModNumber(73L);
        exp.y = new ModNumber(15L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(34L)));
        exp.x = new ModNumber(54L);
        exp.y = new ModNumber(76L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(35L)));
        exp.x = new ModNumber(31L);
        exp.y = new ModNumber(22L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(36L)));
        exp.x = new ModNumber(87L);
        exp.y = new ModNumber(70L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(37L)));
        exp.x = new ModNumber(40L);
        exp.y = new ModNumber(1L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(38L)));
        exp.x = new ModNumber(81L);
        exp.y = new ModNumber(10L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(39L)));
        exp.x = new ModNumber(80L);
        exp.y = new ModNumber(58L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(40L)));
        exp.x = new ModNumber(64L);
        exp.y = new ModNumber(0L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(41L)));
        exp.x = new ModNumber(80L);
        exp.y = new ModNumber(39L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(42L)));
        exp.x = new ModNumber(81L);
        exp.y = new ModNumber(87L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(43L)));
        exp.x = new ModNumber(40L);
        exp.y = new ModNumber(96L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(44L)));
        exp.x = new ModNumber(87L);
        exp.y = new ModNumber(27L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(45L)));
        exp.x = new ModNumber(31L);
        exp.y = new ModNumber(75L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(46L)));
        exp.x = new ModNumber(54L);
        exp.y = new ModNumber(21L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(47L)));
        exp.x = new ModNumber(73L);
        exp.y = new ModNumber(82L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(48L)));
        exp.x = new ModNumber(41L);
        exp.y = new ModNumber(29L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(49L)));
        exp.x = new ModNumber(52L);
        exp.y = new ModNumber(63L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(50L)));
        exp.x = new ModNumber(69L);
        exp.y = new ModNumber(46L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(51L)));
        exp.x = new ModNumber(15L);
        exp.y = new ModNumber(46L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(52L)));
        exp.x = new ModNumber(79L);
        exp.y = new ModNumber(30L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(53L)));
        exp.x = new ModNumber(67L);
        exp.y = new ModNumber(34L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(54L)));
        exp.x = new ModNumber(23L);
        exp.y = new ModNumber(52L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(55L)));
        exp.x = new ModNumber(95L);
        exp.y = new ModNumber(93L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(56L)));
        exp.x = new ModNumber(5L);
        exp.y = new ModNumber(87L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(57L)));
        exp.x = new ModNumber(3L);
        exp.y = new ModNumber(4L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(58L)));
        exp.x = new ModNumber(94L);
        exp.y = new ModNumber(2L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(59L)));
        exp.x = new ModNumber(2L);
        exp.y = new ModNumber(95L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(60L)));
        exp.x = new ModNumber(13L);
        exp.y = new ModNumber(46L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(61L)));
        exp.x = new ModNumber(21L);
        exp.y = new ModNumber(54L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(62L)));
        exp.x = new ModNumber(74L);
        exp.y = new ModNumber(41L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(63L)));
        exp.x = new ModNumber(11L);
        exp.y = new ModNumber(87L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(64L)));
        exp.x = new ModNumber(36L);
        exp.y = new ModNumber(40L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(65L)));
        exp.x = new ModNumber(63L);
        exp.y = new ModNumber(36L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(66L)));
        exp.x = new ModNumber(44L);
        exp.y = new ModNumber(37L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(67L)));
        exp.x = new ModNumber(46L);
        exp.y = new ModNumber(11L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(68L)));
        exp.x = new ModNumber(19L);
        exp.y = new ModNumber(72L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(69L)));
        exp.x = new ModNumber(55L);
        exp.y = new ModNumber(79L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(70L)));
        exp.x = new ModNumber(43L);
        exp.y = new ModNumber(89L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(71L)));
        exp.x = new ModNumber(37L);
        exp.y = new ModNumber(62L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(72L)));
        exp.x = new ModNumber(51L);
        exp.y = new ModNumber(44L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(73L)));
        exp.x = new ModNumber(53L);
        exp.y = new ModNumber(94L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(74L)));
        exp.x = new ModNumber(49L);
        exp.y = new ModNumber(25L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(75L)));
        exp.x = new ModNumber(29L);
        exp.y = new ModNumber(25L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(76L)));
        exp.x = new ModNumber(24L);
        exp.y = new ModNumber(38L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(77L)));
        exp.x = new ModNumber(75L);
        exp.y = new ModNumber(34L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(78L)));
        exp.x = new ModNumber(9L);
        exp.y = new ModNumber(26L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(79L)));
        exp.x = new ModNumber(96L);
        exp.y = new ModNumber(4L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(80L)));
        exp.x = new ModNumber(1L);
        exp.y = new ModNumber(95L);
        assertEquals(exp, myEC.Mult(pt, new ModNumber(81L)));
        exp.IsAtInfinity= true;
        assertEquals(exp, myEC.Mult(pt, new ModNumber(82L)));
    }
    @Test
    public void ecSecp256k1Parameters()
    {
        ModNumber p = ModNumber.stomn("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", 16);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.x = ModNumber.stomn("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", 16);
        g.y = ModNumber.stomn("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8", 16);
        ModNumber n = ModNumber.stomn("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", 16);
        ModNumber mzero = new ModNumber(0L);
        ModNumber a = mzero;
        ModNumber b = new ModNumber(0x07L);
        EC myEC = new EC(mgm, g, n, a, b);
        assertTrue(myEC.IsOnCurve(g));
        ECPoint gTimesN = myEC.Mult(g, n);
        assertTrue(gTimesN.IsAtInfinity);
    }
    @Test
    public void ecSecp256k1PublicPrivateKeyPair()
    {
        ModNumber p = ModNumber.stomn("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", 16);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.x = ModNumber.stomn("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", 16);
        g.y = ModNumber.stomn("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8", 16);
        ModNumber n = ModNumber.stomn("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", 16);
        ModNumber mzero = new ModNumber(0L);
        ModNumber a = mzero;
        ModNumber b = new ModNumber(0x07L);
        EC myEC = new EC(mgm, g, n, a, b);
        ModNumber privateKey = ModNumber.stomn("4eac29116c7cf6deaa31a08a8037c5ae3d72468d87a8487b695bd0740af17ae5", 16);
        ModNumber publicKeyX = ModNumber.stomn("9e89efe1f6766e013daa213a6c3aa898208f24e223e2c888b3da485c9e16825d", 16);
        ModNumber publicKeyY = ModNumber.stomn("14c060c914d55aef7e6c3330784ede0eb0004d00e3231261e800faa8470b3c6c", 16);
        ECKeyPair ecKeyPair = new ECKeyPair(myEC, privateKey, null);
        assertFalse(ecKeyPair.y.IsAtInfinity);
        assertEquals(publicKeyX, ecKeyPair.y.x);
        assertEquals(publicKeyY, ecKeyPair.y.y);
    }
    @Test
    public void signatureECDSASecp256k1SignAndVerifySHA256Valid()
    {
        int[] hashBigEndianInts = { 0x25, 0xbd, 0xec, 0xae, 0x5c, 0x8b, 0xc7, 0x90, 0x5c, 0xbb, 0xda, 0x89, 0x48, 0x5a, 0xfe, 0xc7, 0xc6, 0x07, 0xd6, 0x0a, 0xc0, 0xb1, 0xd4, 0xea, 0x66, 0xc3, 0xca, 0x01, 0xd7, 0x59, 0x3d, 0x87 };
        byte [] hashBigEndian = new byte[hashBigEndianInts.length];
        for (int i = 0; i < hashBigEndianInts.length; i++)
            hashBigEndian[i] = (byte)hashBigEndianInts[i];
        ModNumber p = ModNumber.stomn("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", 16);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.x = ModNumber.stomn("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", 16);
        g.y = ModNumber.stomn("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8", 16);
        ModNumber n = ModNumber.stomn("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", 16);
        ModNumber mzero = new ModNumber(0L);
        ModNumber a = mzero;
        ModNumber b = new ModNumber(0x07L);
        EC myEC = new EC(mgm, g, n, a, b);
        ModNumber privateKey = ModNumber.stomn("4eac29116c7cf6deaa31a08a8037c5ae3d72468d87a8487b695bd0740af17ae5", 16);
        ModNumber publicKeyX = ModNumber.stomn("9e89efe1f6766e013daa213a6c3aa898208f24e223e2c888b3da485c9e16825d", 16);
        ModNumber publicKeyY = ModNumber.stomn("14c060c914d55aef7e6c3330784ede0eb0004d00e3231261e800faa8470b3c6c", 16);
        ECPoint publicKey = new ECPoint();
        publicKey.IsAtInfinity = false;
        publicKey.x = publicKeyX;
        publicKey.y = publicKeyY;
        ECKeyPair ecKeyPair = new ECKeyPair(myEC, privateKey, publicKey);
        ECDSA ecDsa = new ECDSA(ecKeyPair);
        byte[] signature = ecDsa.sign(hashBigEndian);
        boolean valid = ecDsa.verify(hashBigEndian, signature);
        assertTrue(valid);
    }
    @Test
    public void signatureECDSASecp256k1SignAndVerifySHA256InValid()
    {
        int[] hashBigEndianInts = { 0x25, 0xbd, 0xec, 0xae, 0x5c, 0x8b, 0xc7, 0x90, 0x5c, 0xbb, 0xda, 0x89, 0x48, 0x5a, 0xfe, 0xc7, 0xc6, 0x07, 0xd6, 0x0a, 0xc0, 0xb1, 0xd4, 0xea, 0x66, 0xc3, 0xca, 0x01, 0xd7, 0x59, 0x3d, 0x87 };
        byte [] hashBigEndian = new byte[hashBigEndianInts.length];
        for (int i = 0; i < hashBigEndianInts.length; i++)
            hashBigEndian[i] = (byte)hashBigEndianInts[i];
        ModNumber p = ModNumber.stomn("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", 16);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.x = ModNumber.stomn("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", 16);
        g.y = ModNumber.stomn("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8", 16);
        ModNumber n = ModNumber.stomn("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", 16);
        ModNumber mzero = new ModNumber(0L);
        ModNumber a = mzero;
        ModNumber b = new ModNumber(0x07L);
        EC myEC = new EC(mgm, g, n, a, b);
        ModNumber privateKey = ModNumber.stomn("4eac29116c7cf6deaa31a08a8037c5ae3d72468d87a8487b695bd0740af17ae5", 16);
        ModNumber publicKeyX = ModNumber.stomn("9e89efe1f6766e013daa213a6c3aa898208f24e223e2c888b3da485c9e16825d", 16);
        ModNumber publicKeyY = ModNumber.stomn("14c060c914d55aef7e6c3330784ede0eb0004d00e3231261e800faa8470b3c6c", 16);
        ECPoint publicKey = new ECPoint();
        publicKey.IsAtInfinity = false;
        publicKey.x = publicKeyX;
        publicKey.y = publicKeyY;
        ECKeyPair ecKeyPair = new ECKeyPair(myEC, privateKey, publicKey);
        ECDSA ecDsa = new ECDSA(ecKeyPair);
        byte[] signature = ecDsa.sign(hashBigEndian);
        byte[] wrongHash = Arrays.copyOf(hashBigEndian,hashBigEndian.length);
        wrongHash[0]++;
        boolean valid = ecDsa.verify(wrongHash, signature);
        assertFalse(valid);
    }
    @Test
    public void signatureECDSANISTP521SignAndVerifySHA256Valid()
    {
        int[] hashBigEndianInts = { 0x25, 0xbd, 0xec, 0xae, 0x5c, 0x8b, 0xc7, 0x90, 0x5c, 0xbb, 0xda, 0x89, 0x48, 0x5a, 0xfe, 0xc7, 0xc6, 0x07, 0xd6, 0x0a, 0xc0, 0xb1, 0xd4, 0xea, 0x66, 0xc3, 0xca, 0x01, 0xd7, 0x59, 0x3d, 0x87 };
        byte [] hashBigEndian = new byte[hashBigEndianInts.length];
        for (int i = 0; i < hashBigEndianInts.length; i++)
            hashBigEndian[i] = (byte)hashBigEndianInts[i];
        ModNumber p = ModNumber.stomn("1ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff", 16);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.x = ModNumber.stomn("c6858e06b70404e9cd9e3ecb662395b4429c648139053fb521f828af606b4d3dbaa14b5e77efe75928fe1dc127a2ffa8de3348b3c1856a429bf97e7e31c2e5bd66", 16);
        g.y = ModNumber.stomn("11839296a789a3bc0045c8a5fb42c7d1bd998f54449579b446817afbd17273e662c97ee72995ef42640c550b9013fad0761353c7086a272c24088be94769fd16650", 16);
        ModNumber n = ModNumber.stomn("1fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffa51868783bf2f966b7fcc0148f709a5d03bb5c9b8899c47aebb6fb71e91386409", 16);
        ModNumber mzero = new ModNumber(0L);
        ModNumber a = mgm.Diff(mzero, new ModNumber(3L));
        ModNumber b = ModNumber.stomn("051953eb9618e1c9a1f929a21a0b68540eea2da725b99b315f3b8b489918ef109e156193951ec7e937b1652c0bd3bb1bf073573df883d2c34f1ef451fd46b503f00", 16);
        EC myEC = new EC(mgm, g, n, a, b);
        ECKeyPair ecKeyPair = new ECKeyPair(myEC, null, null);
        ECDSA ecDsa = new ECDSA(ecKeyPair);
        byte[] signature = ecDsa.sign(hashBigEndian);
        boolean valid = ecDsa.verify(hashBigEndian, signature);
        assertTrue(valid);
    }
    @Test
    public void signatureECDSAECDSANISTP521SignAndVerifySHA256InValid()
    {
        int[] hashBigEndianInts = { 0x25, 0xbd, 0xec, 0xae, 0x5c, 0x8b, 0xc7, 0x90, 0x5c, 0xbb, 0xda, 0x89, 0x48, 0x5a, 0xfe, 0xc7, 0xc6, 0x07, 0xd6, 0x0a, 0xc0, 0xb1, 0xd4, 0xea, 0x66, 0xc3, 0xca, 0x01, 0xd7, 0x59, 0x3d, 0x87 };
        byte [] hashBigEndian = new byte[hashBigEndianInts.length];
        for (int i = 0; i < hashBigEndianInts.length; i++)
            hashBigEndian[i] = (byte)hashBigEndianInts[i];
        ModNumber p = ModNumber.stomn("1ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff", 16);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.x = ModNumber.stomn("c6858e06b70404e9cd9e3ecb662395b4429c648139053fb521f828af606b4d3dbaa14b5e77efe75928fe1dc127a2ffa8de3348b3c1856a429bf97e7e31c2e5bd66", 16);
        g.y = ModNumber.stomn("11839296a789a3bc0045c8a5fb42c7d1bd998f54449579b446817afbd17273e662c97ee72995ef42640c550b9013fad0761353c7086a272c24088be94769fd16650", 16);
        ModNumber n = ModNumber.stomn("1fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffa51868783bf2f966b7fcc0148f709a5d03bb5c9b8899c47aebb6fb71e91386409", 16);
        ModNumber mzero = new ModNumber(0L);
        ModNumber a = mgm.Diff(mzero, new ModNumber(3L));
        ModNumber b = ModNumber.stomn("051953eb9618e1c9a1f929a21a0b68540eea2da725b99b315f3b8b489918ef109e156193951ec7e937b1652c0bd3bb1bf073573df883d2c34f1ef451fd46b503f00", 16);
        EC myEC = new EC(mgm, g, n, a, b);
        ECKeyPair ecKeyPair = new ECKeyPair(myEC, null, null);
        ECDSA ecDsa = new ECDSA(ecKeyPair);
        byte[] signature = ecDsa.sign(hashBigEndian);
        byte[] wrongHash = Arrays.copyOf(hashBigEndian,hashBigEndian.length);
        wrongHash[0]++;
        boolean valid = ecDsa.verify(wrongHash, signature);
        assertFalse(valid);
    }
    @Test
    public void signatureECDSAECDSASignAndVerifyNISTP224SHA256ValidRandomPrivateKey()
    {
        int[] hashBigEndianInts = { 0x25, 0xbd, 0xec, 0xae, 0x5c, 0x8b, 0xc7, 0x90, 0x5c, 0xbb, 0xda, 0x89, 0x48, 0x5a, 0xfe, 0xc7, 0xc6, 0x07, 0xd6, 0x0a, 0xc0, 0xb1, 0xd4, 0xea, 0x66, 0xc3, 0xca, 0x01, 0xd7, 0x59, 0x3d, 0x87 };
        byte [] hashBigEndian = new byte[hashBigEndianInts.length];
        for (int i = 0; i < hashBigEndianInts.length; i++)
            hashBigEndian[i] = (byte)hashBigEndianInts[i];
        ModNumber p = ModNumber.stomn("ffffffffffffffffffffffffffffffff000000000000000000000001", 16);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.x = ModNumber.stomn("b70e0cbd6bb4bf7f321390b94a03c1d356c21122343280d6115c1d21", 16);
        g.y = ModNumber.stomn("bd376388b5f723fb4c22dfe6cd4375a05a07476444d5819985007e34", 16);
        ModNumber n = ModNumber.stomn("ffffffffffffffffffffffffffff16a2e0b8f03e13dd29455c5c2a3d", 16);
        ModNumber mzero = new ModNumber(0L);
        ModNumber a = mgm.Diff(mzero, new ModNumber(3L));
        ModNumber b = ModNumber.stomn("b4050a850c04b3abf54132565044b0b7d7bfd8ba270b39432355ffb4", 16);
        EC myEC = new EC(mgm, g, n, a, b);
        ECKeyPair ecKeyPair = new ECKeyPair(myEC, null, null);
        ECDSA ecDsa = new ECDSA(ecKeyPair);
        byte[] signature = ecDsa.sign(hashBigEndian);
        boolean valid = ecDsa.verify(hashBigEndian, signature);
        assertTrue(valid);
    }
    @Test
    public void signatureECDSAECDSASignAndVerifyNISTP384SHA256ValidRandomPrivateKey()
    {
        int[] hashBigEndianInts = { 0x25, 0xbd, 0xec, 0xae, 0x5c, 0x8b, 0xc7, 0x90, 0x5c, 0xbb, 0xda, 0x89, 0x48, 0x5a, 0xfe, 0xc7, 0xc6, 0x07, 0xd6, 0x0a, 0xc0, 0xb1, 0xd4, 0xea, 0x66, 0xc3, 0xca, 0x01, 0xd7, 0x59, 0x3d, 0x87 };
        byte [] hashBigEndian = new byte[hashBigEndianInts.length];
        for (int i = 0; i < hashBigEndianInts.length; i++)
            hashBigEndian[i] = (byte)hashBigEndianInts[i];
        ModNumber p = ModNumber.stomn("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffeffffffff0000000000000000ffffffff", 16);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.x = ModNumber.stomn("aa87ca22be8b05378eb1c71ef320ad746e1d3b628ba79b9859f741e082542a385502f25dbf55296c3a545e3872760ab7", 16);
        g.y = ModNumber.stomn("3617de4a96262c6f5d9e98bf9292dc29f8f41dbd289a147ce9da3113b5f0b8c00a60b1ce1d7e819d7a431d7c90ea0e5f", 16);
        ModNumber n = ModNumber.stomn("ffffffffffffffffffffffffffffffffffffffffffffffffc7634d81f4372ddf581a0db248b0a77aecec196accc52973", 16);
        ModNumber mzero = new ModNumber(0L);
        ModNumber a = mgm.Diff(mzero, new ModNumber(3L));
        ModNumber b = ModNumber.stomn("b3312fa7e23ee7e4988e056be3f82d19181d9c6efe8141120314088f5013875ac656398d8a2ed19d2a85c8edd3ec2aef", 16);
        EC myEC = new EC(mgm, g, n, a, b);
        ECKeyPair ecKeyPair = new ECKeyPair(myEC, null, null);
        ECDSA ecDsa = new ECDSA(ecKeyPair);
        byte[] signature = ecDsa.sign(hashBigEndian);
        boolean valid = ecDsa.verify(hashBigEndian, signature);
        assertTrue(valid);
    }
    @Test
    public void signatureECDSAECDSASignAndVerifyNISTP384SHA256inValidRandomPrivateKey()
    {
        int[] hashBigEndianInts = { 0x25, 0xbd, 0xec, 0xae, 0x5c, 0x8b, 0xc7, 0x90, 0x5c, 0xbb, 0xda, 0x89, 0x48, 0x5a, 0xfe, 0xc7, 0xc6, 0x07, 0xd6, 0x0a, 0xc0, 0xb1, 0xd4, 0xea, 0x66, 0xc3, 0xca, 0x01, 0xd7, 0x59, 0x3d, 0x87 };
        byte [] hashBigEndian = new byte[hashBigEndianInts.length];
        for (int i = 0; i < hashBigEndianInts.length; i++)
            hashBigEndian[i] = (byte)hashBigEndianInts[i];
        ModNumber p = ModNumber.stomn("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffeffffffff0000000000000000ffffffff", 16);
        MultGroupMod mgm = new MultGroupMod(p);
        ECPoint g = new ECPoint();
        g.x = ModNumber.stomn("aa87ca22be8b05378eb1c71ef320ad746e1d3b628ba79b9859f741e082542a385502f25dbf55296c3a545e3872760ab7", 16);
        g.y = ModNumber.stomn("3617de4a96262c6f5d9e98bf9292dc29f8f41dbd289a147ce9da3113b5f0b8c00a60b1ce1d7e819d7a431d7c90ea0e5f", 16);
        ModNumber n = ModNumber.stomn("ffffffffffffffffffffffffffffffffffffffffffffffffc7634d81f4372ddf581a0db248b0a77aecec196accc52973", 16);
        ModNumber mzero = new ModNumber(0L);
        ModNumber a = mgm.Diff(mzero, new ModNumber(3L));
        ModNumber b = ModNumber.stomn("b3312fa7e23ee7e4988e056be3f82d19181d9c6efe8141120314088f5013875ac656398d8a2ed19d2a85c8edd3ec2aef", 16);
        EC myEC = new EC(mgm, g, n, a, b);
        ECKeyPair ecKeyPair = new ECKeyPair(myEC, null, null);
        ECDSA ecDsa = new ECDSA(ecKeyPair);
        byte[] signature = ecDsa.sign(hashBigEndian);
        byte[] wrongHash = Arrays.copyOf(hashBigEndian,hashBigEndian.length);
        wrongHash[0]++;
        boolean valid = ecDsa.verify(wrongHash, signature);
        assertFalse(valid);
    }
    @Test
    void rsaDecryptSymmetricKey()
    {
        try {
            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("RSA");
            keyPairGenerator.initialize(ModNumber.MaxMod * 8);
            KeyPair keyPair = keyPairGenerator.generateKeyPair();
            RSAPrivateKey rsaPrivateKey = (RSAPrivateKey) keyPair.getPrivate();
            RSAPrivateCrtKey rsaPrivateCrtKey = (RSAPrivateCrtKey)rsaPrivateKey;
            RSAPublicKey rsaPublicKey = (RSAPublicKey) keyPair.getPublic();
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = new ModNumber(rsaPublicKey.getModulus());
            rsaParameters.PrivExp = new ModNumber(rsaPrivateKey.getPrivateExponent());
            rsaParameters.PubExp = new ModNumber(rsaPublicKey.getPublicExponent());
            rsaParameters.Coefficient = new ModNumber(rsaPrivateCrtKey.getCrtCoefficient());
            rsaParameters.Prime1 = new ModNumber(rsaPrivateCrtKey.getPrimeP());
            rsaParameters.Prime2 = new ModNumber(rsaPrivateCrtKey.getPrimeQ());
            rsaParameters.Exp1 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentP());
            rsaParameters.Exp2 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentQ());
            RSA rsa = new RSA(rsaParameters);
            Cipher myCipher = Cipher.getInstance("RSA/ECB/PKCS1Padding");
            ModNumber symmetricKey = ModNumber.stomn("DB278FB45AE1C1D78FA27EBEA3730432DA100140A40F0CCE71A7F95D027C2D15", 16);
            byte [] symmetricKeyBigEndian = symmetricKey.convertEndianess(0);
            myCipher.init(Cipher.ENCRYPT_MODE, rsaPublicKey);
            byte [] encryptedKey = myCipher.doFinal(symmetricKeyBigEndian);
            byte [] encryptedKeyLittleEndian = ModNumber.convertEndianess(encryptedKey);
            ModNumber encryptedKeyModNumber = new ModNumber(encryptedKeyLittleEndian);
            ModNumber decryptedKeyModNumber = rsa.decrypt(encryptedKeyModNumber);
            assertEquals(symmetricKey, decryptedKeyModNumber);
        }
        catch (NoSuchAlgorithmException | NoSuchPaddingException | InvalidKeyException | IllegalBlockSizeException |
               BadPaddingException e)
        {
            e.printStackTrace();
        }
    }
    @Test
    void rsaEncrypt()
    {
        try {
            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("RSA");
            keyPairGenerator.initialize(ModNumber.MaxMod * 8);
            KeyPair keyPair = keyPairGenerator.generateKeyPair();
            RSAPrivateKey rsaPrivateKey = (RSAPrivateKey) keyPair.getPrivate();
            RSAPrivateCrtKey rsaPrivateCrtKey = (RSAPrivateCrtKey)rsaPrivateKey;
            RSAPublicKey rsaPublicKey = (RSAPublicKey) keyPair.getPublic();
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = new ModNumber(rsaPublicKey.getModulus());
            rsaParameters.PrivExp = new ModNumber(rsaPrivateKey.getPrivateExponent());
            rsaParameters.PubExp = new ModNumber(rsaPublicKey.getPublicExponent());
            rsaParameters.Coefficient = new ModNumber(rsaPrivateCrtKey.getCrtCoefficient());
            rsaParameters.Prime1 = new ModNumber(rsaPrivateCrtKey.getPrimeP());
            rsaParameters.Prime2 = new ModNumber(rsaPrivateCrtKey.getPrimeQ());
            rsaParameters.Exp1 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentP());
            rsaParameters.Exp2 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentQ());
            RSA rsa = new RSA(rsaParameters);
            Cipher myCipher = Cipher.getInstance("RSA/ECB/PKCS1Padding");

            String message = "Dit is een test";
            ModNumber convertedMessage = ModNumber.fromText(message);
            ModNumber encryptedMessage = rsa.encrypt(convertedMessage, message.length() * Character.BYTES);
            byte [] encryptedMessageBigEndian = encryptedMessage.convertEndianess(0);
            myCipher.init(Cipher.DECRYPT_MODE, rsaPrivateCrtKey);
            byte [] decryptedMessageBigEndian = myCipher.doFinal(encryptedMessageBigEndian);
            byte [] decryptedMessage = ModNumber.convertEndianess(decryptedMessageBigEndian);
            ModNumber decryptedMessageModNumber = new ModNumber(decryptedMessage);
            String decryptedString = decryptedMessageModNumber.getText();
            assertEquals(message, decryptedString);
        }
        catch (NoSuchAlgorithmException | NoSuchPaddingException | InvalidKeyException
               | IllegalBlockSizeException |
               BadPaddingException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void rsaDecrypt()
    {
        try {
            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("RSA");
            keyPairGenerator.initialize(ModNumber.MaxMod * 8);
            KeyPair keyPair = keyPairGenerator.generateKeyPair();
            RSAPrivateKey rsaPrivateKey = (RSAPrivateKey) keyPair.getPrivate();
            RSAPrivateCrtKey rsaPrivateCrtKey = (RSAPrivateCrtKey)rsaPrivateKey;
            RSAPublicKey rsaPublicKey = (RSAPublicKey) keyPair.getPublic();
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = new ModNumber(rsaPublicKey.getModulus());
            rsaParameters.PrivExp = new ModNumber(rsaPrivateKey.getPrivateExponent());
            rsaParameters.PubExp = new ModNumber(rsaPublicKey.getPublicExponent());
            rsaParameters.Coefficient = new ModNumber(rsaPrivateCrtKey.getCrtCoefficient());
            rsaParameters.Prime1 = new ModNumber(rsaPrivateCrtKey.getPrimeP());
            rsaParameters.Prime2 = new ModNumber(rsaPrivateCrtKey.getPrimeQ());
            rsaParameters.Exp1 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentP());
            rsaParameters.Exp2 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentQ());
            RSA rsa = new RSA(rsaParameters);
            Cipher myCipher = Cipher.getInstance("RSA/ECB/PKCS1Padding");

            String message = "Dit is een test";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            myCipher.init(Cipher.ENCRYPT_MODE, rsaPublicKey);
            byte [] encryptedMessageBigEndian = myCipher.doFinal(convertedMessageBigEndian);
            byte [] encryptedMessage = ModNumber.convertEndianess(encryptedMessageBigEndian);
            ModNumber encryptedMessageModNumber = new ModNumber(encryptedMessage);
            ModNumber decryptedMessageModNumber = rsa.decrypt(encryptedMessageModNumber);
            String decryptedString = decryptedMessageModNumber.getText();
            assertEquals(message, decryptedString);
        }
        catch (NoSuchAlgorithmException | NoSuchPaddingException | InvalidKeyException
               | IllegalBlockSizeException |
               BadPaddingException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureRsaVerifySHA256()
    {
        try {
            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("RSA");
            keyPairGenerator.initialize(ModNumber.MaxMod * 8);
            KeyPair keyPair = keyPairGenerator.generateKeyPair();
            RSAPrivateKey rsaPrivateKey = (RSAPrivateKey) keyPair.getPrivate();
            RSAPrivateCrtKey rsaPrivateCrtKey = (RSAPrivateCrtKey)rsaPrivateKey;
            RSAPublicKey rsaPublicKey = (RSAPublicKey) keyPair.getPublic();
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = new ModNumber(rsaPublicKey.getModulus());
            rsaParameters.PrivExp = new ModNumber(rsaPrivateKey.getPrivateExponent());
            rsaParameters.PubExp = new ModNumber(rsaPublicKey.getPublicExponent());
            rsaParameters.Coefficient = new ModNumber(rsaPrivateCrtKey.getCrtCoefficient());
            rsaParameters.Prime1 = new ModNumber(rsaPrivateCrtKey.getPrimeP());
            rsaParameters.Prime2 = new ModNumber(rsaPrivateCrtKey.getPrimeQ());
            rsaParameters.Exp1 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentP());
            rsaParameters.Exp2 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentQ());
            RSA rsa = new RSA(rsaParameters);
            Signature mySignature = Signature.getInstance("SHA256withRSA");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            mySignature.initSign(rsaPrivateCrtKey);
            mySignature.update(convertedMessageBigEndian);
            byte [] signatureBigEndian = mySignature.sign();
            byte [] signatureLittleEndian = ModNumber.convertEndianess(signatureBigEndian);
            ModNumber signatureModNumber = new ModNumber(signatureLittleEndian);
            ModNumber decryptedSignatureModNumber = rsa.decryptSignature(signatureModNumber);
            byte[] decryptedSignatureBigEndian = decryptedSignatureModNumber.convertEndianess(0);
            assertArrayEquals(messageDigest, decryptedSignatureBigEndian);
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureRsaCreateSHA256()
    {
        try {
            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("RSA");
            keyPairGenerator.initialize(ModNumber.MaxMod * 8);
            KeyPair keyPair = keyPairGenerator.generateKeyPair();
            RSAPrivateKey rsaPrivateKey = (RSAPrivateKey) keyPair.getPrivate();
            RSAPrivateCrtKey rsaPrivateCrtKey = (RSAPrivateCrtKey)rsaPrivateKey;
            RSAPublicKey rsaPublicKey = (RSAPublicKey) keyPair.getPublic();
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = new ModNumber(rsaPublicKey.getModulus());
            rsaParameters.PrivExp = new ModNumber(rsaPrivateKey.getPrivateExponent());
            rsaParameters.PubExp = new ModNumber(rsaPublicKey.getPublicExponent());
            rsaParameters.Coefficient = new ModNumber(rsaPrivateCrtKey.getCrtCoefficient());
            rsaParameters.Prime1 = new ModNumber(rsaPrivateCrtKey.getPrimeP());
            rsaParameters.Prime2 = new ModNumber(rsaPrivateCrtKey.getPrimeQ());
            rsaParameters.Exp1 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentP());
            rsaParameters.Exp2 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentQ());
            RSA rsa = new RSA(rsaParameters);
            Signature mySignature = Signature.getInstance("SHA256withRSA");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            ModNumber signatureModNumber = rsa.encryptSignature(messageDigest, "2.16.840.1.101.3.4.2.1");
            byte [] signatureBigEndian = signatureModNumber.convertEndianess(0);
            mySignature.initVerify(rsaPublicKey);
            mySignature.update(convertedMessageBigEndian);
            assertTrue(mySignature.verify(signatureBigEndian));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureRsaVerifySHA512()
    {
        try {
            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("RSA");
            keyPairGenerator.initialize(ModNumber.MaxMod * 8);
            KeyPair keyPair = keyPairGenerator.generateKeyPair();
            RSAPrivateKey rsaPrivateKey = (RSAPrivateKey) keyPair.getPrivate();
            RSAPrivateCrtKey rsaPrivateCrtKey = (RSAPrivateCrtKey)rsaPrivateKey;
            RSAPublicKey rsaPublicKey = (RSAPublicKey) keyPair.getPublic();
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = new ModNumber(rsaPublicKey.getModulus());
            rsaParameters.PrivExp = new ModNumber(rsaPrivateKey.getPrivateExponent());
            rsaParameters.PubExp = new ModNumber(rsaPublicKey.getPublicExponent());
            rsaParameters.Coefficient = new ModNumber(rsaPrivateCrtKey.getCrtCoefficient());
            rsaParameters.Prime1 = new ModNumber(rsaPrivateCrtKey.getPrimeP());
            rsaParameters.Prime2 = new ModNumber(rsaPrivateCrtKey.getPrimeQ());
            rsaParameters.Exp1 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentP());
            rsaParameters.Exp2 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentQ());
            RSA rsa = new RSA(rsaParameters);
            Signature mySignature = Signature.getInstance("SHA512withRSA");
            MessageDigest myDigest = MessageDigest.getInstance("SHA512");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            mySignature.initSign(rsaPrivateCrtKey);
            mySignature.update(convertedMessageBigEndian);
            byte [] signatureBigEndian = mySignature.sign();
            byte [] signatureLittleEndian = ModNumber.convertEndianess(signatureBigEndian);
            ModNumber signatureModNumber = new ModNumber(signatureLittleEndian);
            ModNumber decryptedSignatureModNumber = rsa.decryptSignature(signatureModNumber);
            byte[] decryptedSignatureBigEndian = decryptedSignatureModNumber.convertEndianess(0);
            assertArrayEquals(messageDigest, decryptedSignatureBigEndian);
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureRsaCreateSHA512()
    {
        try {
            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("RSA");
            keyPairGenerator.initialize(ModNumber.MaxMod * 8);
            KeyPair keyPair = keyPairGenerator.generateKeyPair();
            RSAPrivateKey rsaPrivateKey = (RSAPrivateKey) keyPair.getPrivate();
            RSAPrivateCrtKey rsaPrivateCrtKey = (RSAPrivateCrtKey)rsaPrivateKey;
            RSAPublicKey rsaPublicKey = (RSAPublicKey) keyPair.getPublic();
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = new ModNumber(rsaPublicKey.getModulus());
            rsaParameters.PrivExp = new ModNumber(rsaPrivateKey.getPrivateExponent());
            rsaParameters.PubExp = new ModNumber(rsaPublicKey.getPublicExponent());
            rsaParameters.Coefficient = new ModNumber(rsaPrivateCrtKey.getCrtCoefficient());
            rsaParameters.Prime1 = new ModNumber(rsaPrivateCrtKey.getPrimeP());
            rsaParameters.Prime2 = new ModNumber(rsaPrivateCrtKey.getPrimeQ());
            rsaParameters.Exp1 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentP());
            rsaParameters.Exp2 = new ModNumber(rsaPrivateCrtKey.getPrimeExponentQ());
            RSA rsa = new RSA(rsaParameters);
            Signature mySignature = Signature.getInstance("SHA512withRSA");
            MessageDigest myDigest = MessageDigest.getInstance("SHA512");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            ModNumber signatureModNumber = rsa.encryptSignature(messageDigest, "2.16.840.1.101.3.4.2.3");
            byte [] signatureBigEndian = signatureModNumber.convertEndianess(0);
            mySignature.initVerify(rsaPublicKey);
            mySignature.update(convertedMessageBigEndian);
            assertTrue(mySignature.verify(signatureBigEndian));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureDsaVerifySHA256()
    {
        try {
            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("DSA");
            keyPairGenerator.initialize(ModNumber.MaxMod * 8);
            KeyPair keyPair = keyPairGenerator.generateKeyPair();
            DSAPrivateKey dsaPrivateKey = (DSAPrivateKey) keyPair.getPrivate();
            DSAPublicKey dsaPublicKey = (DSAPublicKey) keyPair.getPublic();
            DSAParameters dsaParameters = new DSAParameters();
            dsaParameters.y = new ModNumber(dsaPublicKey.getY());
            dsaParameters.x = new ModNumber(dsaPrivateKey.getX());
            dsaParameters.P = new ModNumber(dsaPublicKey.getParams().getP());
            dsaParameters.g = new ModNumber(dsaPublicKey.getParams().getG());
            dsaParameters.Q = new ModNumber(dsaPublicKey.getParams().getQ());
            DSA dsa = new DSA(dsaParameters);
            Signature mySignature = Signature.getInstance("SHA256withDSA");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            mySignature.initSign(dsaPrivateKey);
            mySignature.update(convertedMessageBigEndian);
            byte [] signatureBigEndian = mySignature.sign();
            byte [] signatureLittleEndian = ModNumber.convertEndianess(signatureBigEndian);
            ModNumber signatureModNumber = new ModNumber(signatureLittleEndian);
            assertTrue(dsa.verify(messageDigest,signatureModNumber.toString(16), true));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureDsaCreateSHA256()
    {
        try {
            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("DSA");
            keyPairGenerator.initialize(ModNumber.MaxMod * 8);
            KeyPair keyPair = keyPairGenerator.generateKeyPair();
            DSAPrivateKey dsaPrivateKey = (DSAPrivateKey) keyPair.getPrivate();
            DSAPublicKey dsaPublicKey = (DSAPublicKey) keyPair.getPublic();
            DSAParameters dsaParameters = new DSAParameters();
            dsaParameters.y = new ModNumber(dsaPublicKey.getY());
            dsaParameters.x = new ModNumber(dsaPrivateKey.getX());
            dsaParameters.P = new ModNumber(dsaPublicKey.getParams().getP());
            dsaParameters.g = new ModNumber(dsaPublicKey.getParams().getG());
            dsaParameters.Q = new ModNumber(dsaPublicKey.getParams().getQ());
            DSA dsa = new DSA(dsaParameters);
            Signature mySignature = Signature.getInstance("SHA256withDSA");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            String signature = dsa.sign(messageDigest, true);
            byte [] signatureBigEndian = ModNumber.stringToBytes(signature);
            mySignature.initVerify(dsaPublicKey);
            mySignature.update(convertedMessageBigEndian);
            assertTrue(mySignature.verify(signatureBigEndian));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException
                e)
        {
            e.printStackTrace();
        }
    }
    @Test
    void signatureECDSAVerifySHA256Valid()
    {
        try {
            Security.addProvider(new BouncyCastleProvider());
//            KeyPairGenerator keyPairGenerator = KeyPairGenerator.getInstance("EC", "BC");
//            ECGenParameterSpec ecGenParameterSpec = new ECGenParameterSpec("secp256k1");
//
//            keyPairGenerator.initialize(ecGenParameterSpec);
//            KeyPair keyPair = keyPairGenerator.generateKeyPair();
//            ECPrivateKey ecdsaPrivateKey = (ECPrivateKey) keyPair.getPrivate();
//            ECPublicKey ecdsaPublicKey = (ECPublicKey) keyPair.getPublic();
//            java.security.spec.ECPoint pty = ecdsaPublicKey.getW();
//            ModNumber publicKeyX = new ModNumber(pty.getAffineX());
//            ModNumber publicKeyY = new ModNumber(pty.getAffineY());
//            ModNumber privateKeyGen = new ModNumber(ecdsaPrivateKey.getS());
            String p = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F";
            String gx = "79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798";
            String gy = "483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8";
            String n = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141";
            String yx = "9e89efe1f6766e013daa213a6c3aa898208f24e223e2c888b3da485c9e16825d";
            String yy = "14c060c914d55aef7e6c3330784ede0eb0004d00e3231261e800faa8470b3c6c";
            String x = "4eac29116c7cf6deaa31a08a8037c5ae3d72468d87a8487b695bd0740af17ae5";
            ECFieldFp ecFieldFp = new ECFieldFp(new BigInteger(p, 16));
            EllipticCurve ellipticCurve = new EllipticCurve(ecFieldFp, BigInteger.ZERO, new BigInteger("7"));
            java.security.spec.ECPoint g = new java.security.spec.ECPoint(new BigInteger(gx, 16), new BigInteger(gy, 16));
            ECParameterSpec ecParameterSpec = new ECParameterSpec(ellipticCurve, g, new BigInteger(n, 16), 1 );
            java.security.spec.ECPoint y = new java.security.spec.ECPoint(new BigInteger(yx, 16), new BigInteger(yy, 16));
            ECPublicKeySpec ecPublicKeySpec = new ECPublicKeySpec(y, ecParameterSpec);
            ECPrivateKeySpec ecPrivateKeySpec = new ECPrivateKeySpec(new BigInteger(x, 16), ecParameterSpec);
            KeyFactory keyFactory = KeyFactory.getInstance("EC", "BC");
            PrivateKey privateKey = keyFactory.generatePrivate(ecPrivateKeySpec);
            PublicKey publicKey = keyFactory.generatePublic(ecPublicKeySpec);
            ModNumber mp = ModNumber.stomn(p, 16);
            MultGroupMod mgm = new MultGroupMod(mp);
            ModNumber mgx = ModNumber.stomn(gx, 16);
            ModNumber mgy = ModNumber.stomn(gy, 16);
            ECPoint ecG = new ECPoint();
            ecG.x = mgx;
            ecG.y = mgy;
            ModNumber mn = ModNumber.stomn(n, 16);
            ModNumber ma = new ModNumber(0L);
            ModNumber mb = new ModNumber(0x07L);
            ModNumber mx = ModNumber.stomn(x, 16);
            ModNumber myx = ModNumber.stomn(yx, 16);
            ModNumber myy = ModNumber.stomn(yy, 16);
            ECPoint ecY = new ECPoint();
            ecY.x = myx;
            ecY.y = myy;
            EC myEC = new EC(mgm,ecG, mn, ma, mb);
            ECKeyPair ecKeyPair = new ECKeyPair(myEC, mx, ecY);
            ECDSA ecdsa = new ECDSA(ecKeyPair);
            Signature mySignature = Signature.getInstance("SHA256withECDSA", "BC");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            mySignature.initSign(privateKey);
            mySignature.update(convertedMessageBigEndian);
            byte [] signatureBigEndian = mySignature.sign();
            byte [] signatureLittleEndian = ModNumber.convertEndianess(signatureBigEndian);
            ModNumber signatureModNumber = new ModNumber(signatureLittleEndian);
            assertTrue(ecdsa.verify(messageDigest,signatureModNumber.toString(16), true));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException | InvalidKeySpecException
               // | InvalidAlgorithmParameterException
               | NoSuchProviderException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureECDSAVerifySHA256InValid()
    {
        try {
            Security.addProvider(new BouncyCastleProvider());
            String p = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F";
            String gx = "79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798";
            String gy = "483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8";
            String n = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141";
            String yx = "9e89efe1f6766e013daa213a6c3aa898208f24e223e2c888b3da485c9e16825d";
            String yy = "14c060c914d55aef7e6c3330784ede0eb0004d00e3231261e800faa8470b3c6c";
            String x = "4eac29116c7cf6deaa31a08a8037c5ae3d72468d87a8487b695bd0740af17ae5";
            ECFieldFp ecFieldFp = new ECFieldFp(new BigInteger(p, 16));
            EllipticCurve ellipticCurve = new EllipticCurve(ecFieldFp, BigInteger.ZERO, new BigInteger("7"));
            java.security.spec.ECPoint g = new java.security.spec.ECPoint(new BigInteger(gx, 16), new BigInteger(gy, 16));
            ECParameterSpec ecParameterSpec = new ECParameterSpec(ellipticCurve, g, new BigInteger(n, 16), 1 );
            java.security.spec.ECPoint y = new java.security.spec.ECPoint(new BigInteger(yx, 16), new BigInteger(yy, 16));
            ECPublicKeySpec ecPublicKeySpec = new ECPublicKeySpec(y, ecParameterSpec);
            ECPrivateKeySpec ecPrivateKeySpec = new ECPrivateKeySpec(new BigInteger(x, 16), ecParameterSpec);
            KeyFactory keyFactory = KeyFactory.getInstance("EC", "BC");
            PrivateKey privateKey = keyFactory.generatePrivate(ecPrivateKeySpec);
            PublicKey publicKey = keyFactory.generatePublic(ecPublicKeySpec);
            ModNumber mp = ModNumber.stomn(p, 16);
            MultGroupMod mgm = new MultGroupMod(mp);
            ModNumber mgx = ModNumber.stomn(gx, 16);
            ModNumber mgy = ModNumber.stomn(gy, 16);
            ECPoint ecG = new ECPoint();
            ecG.x = mgx;
            ecG.y = mgy;
            ModNumber mn = ModNumber.stomn(n, 16);
            ModNumber ma = new ModNumber(0L);
            ModNumber mb = new ModNumber(0x07L);
            ModNumber mx = ModNumber.stomn(x, 16);
            ModNumber myx = ModNumber.stomn(yx, 16);
            ModNumber myy = ModNumber.stomn(yy, 16);
            ECPoint ecY = new ECPoint();
            ecY.x = myx;
            ecY.y = myy;
            EC myEC = new EC(mgm,ecG, mn, ma, mb);
            ECKeyPair ecKeyPair = new ECKeyPair(myEC, mx, ecY);
            ECDSA ecdsa = new ECDSA(ecKeyPair);
            Signature mySignature = Signature.getInstance("SHA256withECDSA", "BC");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            String changedMessage = "Dit is een test om te zien of een signature geverifieerd kan worden";
            ModNumber convertedMessage = ModNumber.fromText(message);
            ModNumber convertedChangedMessage = ModNumber.fromText(changedMessage);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            byte [] convertedChangedMessageBigEndian = convertedChangedMessage.convertEndianess(0);
            byte [] messageDigest = myDigest.digest(convertedChangedMessageBigEndian);
            mySignature.initSign(privateKey);
            mySignature.update(convertedMessageBigEndian);
            byte [] signatureBigEndian = mySignature.sign();
            byte [] signatureLittleEndian = ModNumber.convertEndianess(signatureBigEndian);
            ModNumber signatureModNumber = new ModNumber(signatureLittleEndian);
            assertFalse(ecdsa.verify(messageDigest,signatureModNumber.toString(16), true));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException | InvalidKeySpecException
               // | InvalidAlgorithmParameterException
               | NoSuchProviderException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureECDSASignSHA256Valid()
    {
        try {
            Security.addProvider(new BouncyCastleProvider());
            String p = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F";
            String gx = "79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798";
            String gy = "483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8";
            String n = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141";
            String yx = "9e89efe1f6766e013daa213a6c3aa898208f24e223e2c888b3da485c9e16825d";
            String yy = "14c060c914d55aef7e6c3330784ede0eb0004d00e3231261e800faa8470b3c6c";
            String x = "4eac29116c7cf6deaa31a08a8037c5ae3d72468d87a8487b695bd0740af17ae5";
            ECFieldFp ecFieldFp = new ECFieldFp(new BigInteger(p, 16));
            EllipticCurve ellipticCurve = new EllipticCurve(ecFieldFp, BigInteger.ZERO, new BigInteger("7"));
            java.security.spec.ECPoint g = new java.security.spec.ECPoint(new BigInteger(gx, 16), new BigInteger(gy, 16));
            ECParameterSpec ecParameterSpec = new ECParameterSpec(ellipticCurve, g, new BigInteger(n, 16), 1 );
            java.security.spec.ECPoint y = new java.security.spec.ECPoint(new BigInteger(yx, 16), new BigInteger(yy, 16));
            ECPublicKeySpec ecPublicKeySpec = new ECPublicKeySpec(y, ecParameterSpec);
            ECPrivateKeySpec ecPrivateKeySpec = new ECPrivateKeySpec(new BigInteger(x, 16), ecParameterSpec);
            KeyFactory keyFactory = KeyFactory.getInstance("EC", "BC");
            PrivateKey privateKey = keyFactory.generatePrivate(ecPrivateKeySpec);
            PublicKey publicKey = keyFactory.generatePublic(ecPublicKeySpec);
            ModNumber mp = ModNumber.stomn(p, 16);
            MultGroupMod mgm = new MultGroupMod(mp);
            ModNumber mgx = ModNumber.stomn(gx, 16);
            ModNumber mgy = ModNumber.stomn(gy, 16);
            ECPoint ecG = new ECPoint();
            ecG.x = mgx;
            ecG.y = mgy;
            ModNumber mn = ModNumber.stomn(n, 16);
            ModNumber ma = new ModNumber(0L);
            ModNumber mb = new ModNumber(0x07L);
            ModNumber mx = ModNumber.stomn(x, 16);
            ModNumber myx = ModNumber.stomn(yx, 16);
            ModNumber myy = ModNumber.stomn(yy, 16);
            ECPoint ecY = new ECPoint();
            ecY.x = myx;
            ecY.y = myy;
            EC myEC = new EC(mgm,ecG, mn, ma, mb);
            ECKeyPair ecKeyPair = new ECKeyPair(myEC, mx, ecY);
            ECDSA ecdsa = new ECDSA(ecKeyPair);
            Signature mySignature = Signature.getInstance("SHA256withECDSA", "BC");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(message.length() * Character.BYTES);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            String signature = ecdsa.sign(messageDigest, true);
            byte [] signatureBigEndian = ModNumber.stringToBytes(signature);
            mySignature.initVerify(publicKey);
            mySignature.update(convertedMessageBigEndian);
            assertTrue(mySignature.verify(signatureBigEndian));
         }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException | InvalidKeySpecException
               // | InvalidAlgorithmParameterException
               | NoSuchProviderException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureECDSASignSHA256InValid()
    {
        try {
            Security.addProvider(new BouncyCastleProvider());
            String p = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F";
            String gx = "79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798";
            String gy = "483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8";
            String n = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141";
            String yx = "9e89efe1f6766e013daa213a6c3aa898208f24e223e2c888b3da485c9e16825d";
            String yy = "14c060c914d55aef7e6c3330784ede0eb0004d00e3231261e800faa8470b3c6c";
            String x = "4eac29116c7cf6deaa31a08a8037c5ae3d72468d87a8487b695bd0740af17ae5";
            ECFieldFp ecFieldFp = new ECFieldFp(new BigInteger(p, 16));
            EllipticCurve ellipticCurve = new EllipticCurve(ecFieldFp, BigInteger.ZERO, new BigInteger("7"));
            java.security.spec.ECPoint g = new java.security.spec.ECPoint(new BigInteger(gx, 16), new BigInteger(gy, 16));
            ECParameterSpec ecParameterSpec = new ECParameterSpec(ellipticCurve, g, new BigInteger(n, 16), 1 );
            java.security.spec.ECPoint y = new java.security.spec.ECPoint(new BigInteger(yx, 16), new BigInteger(yy, 16));
            ECPublicKeySpec ecPublicKeySpec = new ECPublicKeySpec(y, ecParameterSpec);
            ECPrivateKeySpec ecPrivateKeySpec = new ECPrivateKeySpec(new BigInteger(x, 16), ecParameterSpec);
            KeyFactory keyFactory = KeyFactory.getInstance("EC", "BC");
            PrivateKey privateKey = keyFactory.generatePrivate(ecPrivateKeySpec);
            PublicKey publicKey = keyFactory.generatePublic(ecPublicKeySpec);
            ModNumber mp = ModNumber.stomn(p, 16);
            MultGroupMod mgm = new MultGroupMod(mp);
            ModNumber mgx = ModNumber.stomn(gx, 16);
            ModNumber mgy = ModNumber.stomn(gy, 16);
            ECPoint ecG = new ECPoint();
            ecG.x = mgx;
            ecG.y = mgy;
            ModNumber mn = ModNumber.stomn(n, 16);
            ModNumber ma = new ModNumber(0L);
            ModNumber mb = new ModNumber(0x07L);
            ModNumber mx = ModNumber.stomn(x, 16);
            ModNumber myx = ModNumber.stomn(yx, 16);
            ModNumber myy = ModNumber.stomn(yy, 16);
            ECPoint ecY = new ECPoint();
            ecY.x = myx;
            ecY.y = myy;
            EC myEC = new EC(mgm,ecG, mn, ma, mb);
            ECKeyPair ecKeyPair = new ECKeyPair(myEC, mx, ecY);
            ECDSA ecdsa = new ECDSA(ecKeyPair);
            Signature mySignature = Signature.getInstance("SHA256withECDSA", "BC");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            String changedMessage = "Dit is een test om te zien of een signature geverifieerd kan worden";
            ModNumber convertedMessage = ModNumber.fromText(message);
            ModNumber convertedChangedMessage = ModNumber.fromText(changedMessage);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(0);
            byte [] convertedChangedMessageBigEndian = convertedChangedMessage.convertEndianess(0);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            String signature = ecdsa.sign(messageDigest, true);
            byte [] signatureBigEndian = ModNumber.stringToBytes(signature);
            mySignature.initVerify(publicKey);
            mySignature.update(convertedChangedMessageBigEndian);
            assertFalse(mySignature.verify(signatureBigEndian));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException | InvalidKeySpecException
               // | InvalidAlgorithmParameterException
               | NoSuchProviderException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureECDSASignSHA256CalculatedPubKey()
    {
        try {
            Security.addProvider(new BouncyCastleProvider());
            String p = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F";
            String gx = "79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798";
            String gy = "483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8";
            String n = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141";
            String x = "4eac29116c7cf6deaa31a08a8037c5ae3d72468d87a8487b695bd0740af17ae5";
            ECFieldFp ecFieldFp = new ECFieldFp(new BigInteger(p, 16));
            EllipticCurve ellipticCurve = new EllipticCurve(ecFieldFp, BigInteger.ZERO, new BigInteger("7"));
            java.security.spec.ECPoint g = new java.security.spec.ECPoint(new BigInteger(gx, 16), new BigInteger(gy, 16));
            ECParameterSpec ecParameterSpec = new ECParameterSpec(ellipticCurve, g, new BigInteger(n, 16), 1 );
            ECPrivateKeySpec ecPrivateKeySpec = new ECPrivateKeySpec(new BigInteger(x, 16), ecParameterSpec);
            KeyFactory keyFactory = KeyFactory.getInstance("EC", "BC");
            PrivateKey privateKey = keyFactory.generatePrivate(ecPrivateKeySpec);
            ModNumber mx = ModNumber.stomn(x, 16);
            ModNumber mp = ModNumber.stomn(p, 16);
            MultGroupMod mgm = new MultGroupMod(mp);
            ModNumber mgx = ModNumber.stomn(gx, 16);
            ModNumber mgy = ModNumber.stomn(gy, 16);
            ECPoint ecG = new ECPoint();
            ecG.x = mgx;
            ecG.y = mgy;
            ModNumber mn = ModNumber.stomn(n, 16);
            ModNumber ma = new ModNumber(0L);
            ModNumber mb = new ModNumber(0x07L);
            EC myEC = new EC(mgm,ecG, mn, ma, mb);
            ECKeyPair ecKeyPair = new ECKeyPair(myEC, mx, null);
            ECDSA ecdsa = new ECDSA(ecKeyPair);
            String yx = ecKeyPair.y.x.toString(16);
            String yy = ecKeyPair.y.y.toString(16);
            java.security.spec.ECPoint y = new java.security.spec.ECPoint(new BigInteger(yx, 16), new BigInteger(yy, 16));
            ECPublicKeySpec ecPublicKeySpec = new ECPublicKeySpec(y, ecParameterSpec);
            PublicKey publicKey = keyFactory.generatePublic(ecPublicKeySpec);
            Signature mySignature = Signature.getInstance("SHA256withECDSA", "BC");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(message.length() * Character.BYTES);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            String signature = ecdsa.sign(messageDigest, true);
            byte [] signatureBigEndian = ModNumber.stringToBytes(signature);
            mySignature.initVerify(publicKey);
            mySignature.update(convertedMessageBigEndian);
            assertTrue(mySignature.verify(signatureBigEndian));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException | InvalidKeySpecException
               // | InvalidAlgorithmParameterException
               | NoSuchProviderException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureECDSASignSHA256CalculatedPubAndPrivateKey()
    {
        try {
            Security.addProvider(new BouncyCastleProvider());
            String p = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F";
            String gx = "79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798";
            String gy = "483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8";
            String n = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141";
            ECFieldFp ecFieldFp = new ECFieldFp(new BigInteger(p, 16));
            EllipticCurve ellipticCurve = new EllipticCurve(ecFieldFp, BigInteger.ZERO, new BigInteger("7"));
            java.security.spec.ECPoint g = new java.security.spec.ECPoint(new BigInteger(gx, 16), new BigInteger(gy, 16));
            ECParameterSpec ecParameterSpec = new ECParameterSpec(ellipticCurve, g, new BigInteger(n, 16), 1 );
            KeyFactory keyFactory = KeyFactory.getInstance("EC", "BC");
            ModNumber mp = ModNumber.stomn(p, 16);
            MultGroupMod mgm = new MultGroupMod(mp);
            ModNumber mgx = ModNumber.stomn(gx, 16);
            ModNumber mgy = ModNumber.stomn(gy, 16);
            ECPoint ecG = new ECPoint();
            ecG.x = mgx;
            ecG.y = mgy;
            ModNumber mn = ModNumber.stomn(n, 16);
            ModNumber ma = new ModNumber(0L);
            ModNumber mb = new ModNumber(0x07L);
            EC myEC = new EC(mgm,ecG, mn, ma, mb);
            ECKeyPair ecKeyPair = new ECKeyPair(myEC, null, null);
            ECDSA ecdsa = new ECDSA(ecKeyPair);
            String x = ecKeyPair.mx.toString(16);
            String yx = ecKeyPair.y.x.toString(16);
            String yy = ecKeyPair.y.y.toString(16);
            ECPrivateKeySpec ecPrivateKeySpec = new ECPrivateKeySpec(new BigInteger(x, 16), ecParameterSpec);
            PrivateKey privateKey = keyFactory.generatePrivate(ecPrivateKeySpec);
            java.security.spec.ECPoint y = new java.security.spec.ECPoint(new BigInteger(yx, 16), new BigInteger(yy, 16));
            ECPublicKeySpec ecPublicKeySpec = new ECPublicKeySpec(y, ecParameterSpec);
            PublicKey publicKey = keyFactory.generatePublic(ecPublicKeySpec);
            Signature mySignature = Signature.getInstance("SHA256withECDSA", "BC");
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(message.length() * Character.BYTES);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            String signature = ecdsa.sign(messageDigest, true);
            byte [] signatureBigEndian = ModNumber.stringToBytes(signature);
            mySignature.initVerify(publicKey);
            mySignature.update(convertedMessageBigEndian);
            assertTrue(mySignature.verify(signatureBigEndian));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException | InvalidKeySpecException
               // | InvalidAlgorithmParameterException
               | NoSuchProviderException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureECDSASignNISTP256SHA256ValidGivenPrivateKey()
    {
        try {
            Security.addProvider(new BouncyCastleProvider());
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een\ngeheim\ndocument.";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(message.length() * Character.BYTES);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            String p = "ffffffff00000001000000000000000000000000ffffffffffffffffffffffff";
            ModNumber mp = ModNumber.stomn(p, 16);
            MultGroupMod mgm = new MultGroupMod(mp);
            String gx = "6b17d1f2e12c4247f8bce6e563a440f277037d812deb33a0f4a13945d898c296";
            String gy = "4fe342e2fe1a7f9b8ee7eb4a7c0f9e162bce33576b315ececbb6406837bf51f5";
            ModNumber mgx = ModNumber.stomn(gx, 16);
            ModNumber mgy = ModNumber.stomn(gy, 16);
            ECPoint ecG = new ECPoint();
            ecG.x = mgx;
            ecG.y = mgy;
            String n = "ffffffff00000000ffffffffffffffffbce6faada7179e84f3b9cac2fc632551";
            ModNumber mn = ModNumber.stomn(n, 16);
            ModNumber mzero = new ModNumber(0L);
            ModNumber ma = mgm.Diff(mzero, new ModNumber(3L));
            ModNumber mb = ModNumber.stomn("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b", 16);
            EC myEC = new EC(mgm,ecG, mn, ma, mb);

            String x = "AE176B08B19CC8AAF0B12BB290A651B804330B6800B6C3BA3174F81D9FCF70A7";
            ModNumber mx = ModNumber.stomn(x, 16);
            ECKeyPair ecKeyPair = new ECKeyPair(myEC, mx, null);
            String aStr = ma.toString(16);
            String bStr = mb.toString(16);

            ECFieldFp ecFieldFp = new ECFieldFp(new BigInteger(p, 16));
            EllipticCurve ellipticCurve = new EllipticCurve(ecFieldFp, new BigInteger(aStr, 16), new BigInteger(bStr, 16));
            java.security.spec.ECPoint g = new java.security.spec.ECPoint(new BigInteger(gx, 16), new BigInteger(gy, 16));
            ECParameterSpec ecParameterSpec = new ECParameterSpec(ellipticCurve, g, new BigInteger(n, 16), 1 );
            ECPrivateKeySpec ecPrivateKeySpec = new ECPrivateKeySpec(new BigInteger(x, 16), ecParameterSpec);
            KeyFactory keyFactory = KeyFactory.getInstance("EC", "BC");
            PrivateKey privateKey = keyFactory.generatePrivate(ecPrivateKeySpec);
            ECDSA ecdsa = new ECDSA(ecKeyPair);
            String yx = ecKeyPair.y.x.toString(16);
            String yy = ecKeyPair.y.y.toString(16);
            java.security.spec.ECPoint y = new java.security.spec.ECPoint(new BigInteger(yx, 16), new BigInteger(yy, 16));
            ECPublicKeySpec ecPublicKeySpec = new ECPublicKeySpec(y, ecParameterSpec);
            PublicKey publicKey = keyFactory.generatePublic(ecPublicKeySpec);
            Signature mySignature = Signature.getInstance("SHA256withECDSA", "BC");
            String signature = ecdsa.sign(messageDigest, true);
            byte [] signatureBigEndian = ModNumber.stringToBytes(signature);
            mySignature.initVerify(publicKey);
            mySignature.update(convertedMessageBigEndian);
            assertTrue(mySignature.verify(signatureBigEndian));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException | InvalidKeySpecException
               // | InvalidAlgorithmParameterException
               | NoSuchProviderException
                e)
        {
            e.printStackTrace();
        }

    }
    @Test
    void signatureECDSASignNISTP521SignSHA256ValidRandomPrivateKey()
    {
        try {
            Security.addProvider(new BouncyCastleProvider());
            MessageDigest myDigest = MessageDigest.getInstance("SHA256");
            String message = "Dit is een test om te zien of een signature geverifieerd kan worden!";
            ModNumber convertedMessage = ModNumber.fromText(message);
            byte [] convertedMessageBigEndian = convertedMessage.convertEndianess(message.length() * Character.BYTES);
            byte [] messageDigest = myDigest.digest(convertedMessageBigEndian);
            String p = "1ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff";
            ModNumber mp = ModNumber.stomn(p, 16);
            MultGroupMod mgm = new MultGroupMod(mp);
            String gx = "c6858e06b70404e9cd9e3ecb662395b4429c648139053fb521f828af606b4d3dbaa14b5e77efe75928fe1dc127a2ffa8de3348b3c1856a429bf97e7e31c2e5bd66";
            String gy = "11839296a789a3bc0045c8a5fb42c7d1bd998f54449579b446817afbd17273e662c97ee72995ef42640c550b9013fad0761353c7086a272c24088be94769fd16650";
            ModNumber mgx = ModNumber.stomn(gx, 16);
            ModNumber mgy = ModNumber.stomn(gy, 16);
            ECPoint ecG = new ECPoint();
            ecG.x = mgx;
            ecG.y = mgy;
            String n = "1fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffa51868783bf2f966b7fcc0148f709a5d03bb5c9b8899c47aebb6fb71e91386409";
            ModNumber mn = ModNumber.stomn(n, 16);
            ModNumber mzero = new ModNumber(0L);
            ModNumber ma = mgm.Diff(mzero, new ModNumber(3L));
            ModNumber mb = ModNumber.stomn("051953eb9618e1c9a1f929a21a0b68540eea2da725b99b315f3b8b489918ef109e156193951ec7e937b1652c0bd3bb1bf073573df883d2c34f1ef451fd46b503f00", 16);
            EC myEC = new EC(mgm,ecG, mn, ma, mb);
            ECKeyPair ecKeyPair = new ECKeyPair(myEC, null, null);
            String x = ecKeyPair.mx.toString(16);
            String aStr = ma.toString(16);
            String bStr = mb.toString(16);

            ECFieldFp ecFieldFp = new ECFieldFp(new BigInteger(p, 16));
            EllipticCurve ellipticCurve = new EllipticCurve(ecFieldFp, new BigInteger(aStr, 16), new BigInteger(bStr, 16));
            java.security.spec.ECPoint g = new java.security.spec.ECPoint(new BigInteger(gx, 16), new BigInteger(gy, 16));
            ECParameterSpec ecParameterSpec = new ECParameterSpec(ellipticCurve, g, new BigInteger(n, 16), 1 );
            ECPrivateKeySpec ecPrivateKeySpec = new ECPrivateKeySpec(new BigInteger(x, 16), ecParameterSpec);
            KeyFactory keyFactory = KeyFactory.getInstance("EC", "BC");
            PrivateKey privateKey = keyFactory.generatePrivate(ecPrivateKeySpec);
            ECDSA ecdsa = new ECDSA(ecKeyPair);
            String yx = ecKeyPair.y.x.toString(16);
            String yy = ecKeyPair.y.y.toString(16);
            java.security.spec.ECPoint y = new java.security.spec.ECPoint(new BigInteger(yx, 16), new BigInteger(yy, 16));
            ECPublicKeySpec ecPublicKeySpec = new ECPublicKeySpec(y, ecParameterSpec);
            PublicKey publicKey = keyFactory.generatePublic(ecPublicKeySpec);
            Signature mySignature = Signature.getInstance("SHA256withECDSA", "BC");
            String signature = ecdsa.sign(messageDigest, true);
            System.out.println(signature);
            byte [] signatureBigEndian = ModNumber.stringToBytes(signature);
            mySignature.initVerify(publicKey);
            mySignature.update(convertedMessageBigEndian);
            assertTrue(mySignature.verify(signatureBigEndian));
        }
        catch (NoSuchAlgorithmException | InvalidKeyException
               | SignatureException | InvalidKeySpecException
               // | InvalidAlgorithmParameterException
               | NoSuchProviderException
                 e) {
            e.printStackTrace();
        }

    }

}
