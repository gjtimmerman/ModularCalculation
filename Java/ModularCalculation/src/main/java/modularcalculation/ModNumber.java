package modularcalculation;

import java.io.*;
import java.util.Arrays;

public class ModNumber {
    public final static int MaxMod = 4096 / 8;

    public final static int LSIZE = 8;
    public final static int ISIZE = 4;
    public final static int NCOUNT = MaxMod + LSIZE;
    public final static int COUNTMOD = MaxMod / LSIZE;
    public final static int LCOUNT = NCOUNT / LSIZE;
    public final static int ICOUNT = NCOUNT / ISIZE;
    public final static int NSIZE = NCOUNT * LSIZE;
    public final static int HexStringLength = NCOUNT * 2;
    public final static int OctalStringLength = (NSIZE % 3 == 0) ? (NSIZE / 3) : (NSIZE / 3 + 1);
    public final static int DecimalStringLength = (int) (NSIZE * 0.30102999566398119521373889472449) + 1; // log(2)

    long[] num = new long[LCOUNT];

    public ModNumber(long n) {
        num[0] = n;
    }

    public ModNumber(long[] num) {
        this.num = num;
    }

    public ModNumber(ModNumber m) {
        num = m.num.clone();
    }

    public ModNumber(byte[] n) {
        int lCount = n.length / LSIZE;
        long tmp;
        for (int i = 0; i < lCount; i++) {
            tmp = 0L;
            for (int j = 0; j < LSIZE; j++) {
                tmp |= ((long) n[i * LSIZE + j] << j * 8);
            }
            num[i] = tmp;
        }
        int lSize = n.length % LSIZE;
        if (lSize == 0)
            return;
        tmp = 0L;
        for (int j = 0; j < lSize; j++) {
            tmp |= ((long) n[lCount * LSIZE + j] << j * 8);
        }
        num[lCount] = tmp;
    }

    public ModNumber(long[] num, int byteLen) {
        int lCount = byteLen / LSIZE;
        for (int i = 0; i < lCount; i++)
            this.num[i] = num[i];
        int lSize = byteLen % LSIZE;
        if (lSize == 0)
            return;
        long tmp = 0L;
        long mask = 0xffL;
        for (int j = 0; j < lSize; j++) {
            long masked = num[lCount] & mask;
            tmp |= masked;
            mask <<= 8;
        }
        this.num[lCount] = tmp;
    }

    public ModNumber(int[] arr) {
        fromIntArray(arr);
    }

    public int[] toIntArray() {
        int[] result = new int[ICOUNT];
        long lomask = 0xffffffffL;
        long himask = lomask << ISIZE * 8;
        for (int i = 0; i < LCOUNT; i++) {
            result[i * 2] = (int) (num[i] & lomask);
            result[i * 2 + 1] = (int) ((num[i] & himask) >> ISIZE * 8);
        }
        return result;
    }

    public void fromIntArray(int[] arr) {
        for (int i = 0; i < LCOUNT; i++) {
            num[i] = arr[i * 2];
            num[i] <<= ISIZE * 8;
            num[i] >>>= ISIZE * 8;
            num[i] |= ((long) arr[i * 2 + 1]) << ISIZE * 8;
        }

    }
    public static int[] ShiftIntArrayR(int[] arr, int bitCount)
    {
        if (bitCount == 0)
            return arr;
        for (int i = 0; i < bitCount; i++)
        {
            arr[0] >>>= 1;
            if ((arr[1] & 0x01) != 0)
                arr[0] |= 0x80000000;
            arr[1] >>>= 1;
        }
        return arr;
    }
    public static int[] ShiftIntArrayL(int[] arr, int bitCount)
    {
        if (bitCount == 0)
            return arr;
        for (int i = 0; i < bitCount; i++)
        {
            arr[1] <<= 1;
            if ((arr[0] & 0x80000000) != 0)
                arr[1] |= 0x01;
            arr[0] <<= 1;
        }
        return arr;
    }

    @Override
    public boolean equals(Object other) {
        if (other instanceof ModNumber om) {
            for (int i = 0; i < om.num.length; i++) {
                if (num[i] != om.num[i])
                    return false;
            }
            return true;
        }
        return false;
    }

    @Override
    public int hashCode() {
        return num.hashCode();
    }

    void checkMax(int size) {
        for (int i = size; i < LCOUNT; i++)
            if (num[i] != 0L)
                throw new IllegalArgumentException("Modulus is too large!");
    }

    public int getByteCount()
    {
        for (int i = LCOUNT - 1; i >= 0; i--)
        {
            if (num[i] != 0)
            {
                long mask = 0xffL << (LSIZE - 1) * 8;
                for (int j = 0; j < LSIZE; j++)
                {
                    if ((num[i] & mask) != 0L)
                    {
                        return (int)(i * LSIZE + (LSIZE - j));
                    }
                    mask >>>= 8;
                }
            }
        }
        return 0;
    }
    public short getDoubleByteValue(int cb)
    {
        int lCount = cb / 4;
        int sCount = cb % 4;
        long mask = 0xffffL << (sCount*16);
        return (short)((num[lCount] & mask) >>> (sCount*16));

    }

    public static ModNumber subtractScalar(ModNumber l, int r) {
        int[] resInt = l.toIntArray();
        long lomask = 0xffffffffL;
        int carry = 0;
        long ltmp = resInt[0];
        ltmp &= lomask;
        long lr = 0;
        lr = r;
        lr &= lomask;
        if (ltmp < lr)
            carry = 1;
        resInt[0] = (int) (ltmp - lr);
        for (int i = 1; carry > 0 && i < ICOUNT; i++) {
            if (carry <= resInt[i]) {
                resInt[i] -= carry;
                carry = 0;
            } else
                resInt[i] -= carry;

        }
        ModNumber result = new ModNumber(resInt);
        return result;
    }

    public static ModNumber subtractAssignScalar(ModNumber l, int r) {
        ModNumber result = subtractScalar(l, r);
        l.num = result.num;
        return l;
    }

    public static ModNumber subtract(ModNumber l, ModNumber r) {
        if (l.equals(r))
            return new ModNumber(0L);              // Optimization
        int[] resInt = l.toIntArray();
        int[] lInt = l.toIntArray();
        int[] rInt = r.toIntArray();
        int carry = 0;
        long lomask = 0xffffffffL;
        for (int i = 0; i < ICOUNT; i++) {
            long ltmp = lInt[i];
            ltmp &= lomask;
            long rtmp = rInt[i];
            rtmp &= lomask;
            if (ltmp >= carry) {
                ltmp -= carry;
                carry = 0;
            } else
                ltmp -= carry;
            if (ltmp < rtmp)
                carry = 1;
            resInt[i] = (int) (ltmp - rtmp);
        }
        ModNumber result = new ModNumber(resInt);
        return result;
    }

    public static ModNumber subtractAssign(ModNumber ml, ModNumber mr) {
        ModNumber mResult = ModNumber.subtract(ml, mr);
        ml.num = mResult.num;
        return mResult;
    }

    private ModNumber addAssignScalar(int lpos, int scalar) {
        if (lpos >= ICOUNT)
            throw new IllegalArgumentException("lpos out of range");
        long res = 0L;
        long lomask = 0xffffffffL;
        long himask = lomask << ISIZE * 8;
        int[] thisIntArray = toIntArray();
        long tmp = thisIntArray[lpos];
        tmp &= lomask;
        long scalarLong = scalar & lomask;
        res = tmp + scalarLong;
        thisIntArray[lpos++] = (int) (res & lomask);
        while ((res & himask) != 0L && lpos < ICOUNT) {
            tmp = thisIntArray[lpos];
            tmp &= lomask;
            res = tmp + ((res & himask) >> ISIZE * 8);
            thisIntArray[lpos++] = (int) (res & lomask);
        }
        fromIntArray(thisIntArray);
        return this;
    }
    public static ModNumber addScalar(ModNumber l, int scalar) {
        ModNumber mres = new ModNumber(l);
        mres.addAssignScalar(0, scalar);
        return mres;
    }

    public static ModNumber addAssignScalar(ModNumber l, int scalar) {
        l.addAssignScalar(0, scalar);
        return l;
    }

    public static ModNumber add(ModNumber l, ModNumber r) {
        ModNumber mres = new ModNumber(l);
        int[] rUint = r.toIntArray();
        for (int i = 0; i < ICOUNT; i++) {
            mres.addAssignScalar(i, rUint[i]);
        }
        return mres;
    }

    public static ModNumber addAssign(ModNumber l, ModNumber r) {
        ModNumber mres = add(l, r);
        l.num = mres.num;
        return mres;
    }

    public static ModNumber productScalar(ModNumber l, int scalar) {
        ModNumber mres = new ModNumber(0L);
        long lomask = 0xffffffffL;
        long himask = lomask << ISIZE * 8;
        long scalarLong = scalar;
        scalarLong &= lomask;
        int[] lInt = l.toIntArray();
        int firstNonzeroWord;
        for (firstNonzeroWord = ICOUNT - 1; firstNonzeroWord >= 0; firstNonzeroWord--) {
            if (lInt[firstNonzeroWord] != 0)
                break;
        }
        for (int i = 0; i <= firstNonzeroWord; i++) {
            long lintTmp = lInt[i];
            lintTmp &= lomask;
            long tmpres = lintTmp * scalarLong;
            mres.addAssignScalar(i, (int) (tmpres & lomask));
            if (i < ICOUNT - 1) {
                lintTmp = tmpres & himask;
                lintTmp >>>= ISIZE * 8;
                mres.addAssignScalar(i + 1, (int) lintTmp);
            }
        }
        return mres;
    }
    public static ModNumber productAssignScalar(ModNumber l, int scalar) {
        ModNumber result = productScalar(l, scalar);
        l.num = result.num;
        return result;
    }

    public static ModNumber product(ModNumber ml, ModNumber mr) {
        ModNumber mres = new ModNumber(0L);
        int[] rInt = mr.toIntArray();
        int firstNonzeroWord;
        for (firstNonzeroWord = ICOUNT - 1; firstNonzeroWord >= 0; firstNonzeroWord--) {
            if (rInt[firstNonzeroWord] != 0)
                break;
        }
        for (int i = 0; i <= firstNonzeroWord; i++) {
            addAssign(mres, ModNumber.shiftLeft(ModNumber.productScalar(ml, rInt[i]), ISIZE * 8 * i));
        }
        return mres;
    }

    public static ModNumber shiftLeft(ModNumber n, int i) {
        int words = 0;
        if (i >= ISIZE * 8) {
            if (i >= NSIZE)
                return new ModNumber(0L);
            words = i / (ISIZE * 8);
            i %= ISIZE * 8;
        }
        long lomask = 0xffffffffL;
        long himask = lomask << ISIZE * 8;
        int[] resInt = new int[ICOUNT];
        int[] nInt = n.toIntArray();
        resInt[ICOUNT - 1] = nInt[ICOUNT - words - 1] << i;
        for (int j = ICOUNT - 2; j >= words; j--) {
            long nIntLong = nInt[j - words];
            nIntLong &= lomask;
            nIntLong <<= i;
            long tmp = nIntLong;
            resInt[j + 1] |= (int) ((tmp & himask) >>> ISIZE * 8);
            resInt[j] = (int) (tmp & lomask);
        }
        ModNumber mres = new ModNumber(resInt);
        return mres;
    }

    public static ModNumber shiftLeftAssign(ModNumber n, int i) {
        ModNumber mres = shiftLeft(n, i);
        n.num = mres.num;
        return mres;
    }

    public static ModNumber shiftRight(ModNumber n, int i) {
        int words = 0;
        if (i >= ISIZE * 8) {
            if (i >= NSIZE) {
                return new ModNumber(0L);
            }
            words = i / (ISIZE * 8);
            i %= ISIZE * 8;
        }
        long lomask = 0xffffffff;
        long himask = lomask << ISIZE * 8;
        int[] resInt = new int[ICOUNT];
        int[] nInt = n.toIntArray();
        resInt[0] = nInt[words] >>> i;
        for (int j = 0; j < ICOUNT - words - 1; j++) {
            long mask = 0xFFFFFFFFL;
            long nIntjpluswordsplus1 = ((long)nInt[j + words + 1]) & mask;
            long tmp = nIntjpluswordsplus1 << ((ISIZE * 8) - i);
            resInt[j] |= (int) (tmp & lomask);
            resInt[j + 1] = (int) ((tmp & himask) >>> ISIZE * 8);
        }
        ModNumber mres = new ModNumber(resInt);
        return mres;
    }

    public static ModNumber shiftRightAssign(ModNumber n, int i) {
        ModNumber mres = shiftRight(n, i);
        n.num = mres.num;
        return mres;
    }

    public DivideModuloScalarResult divideAndModulo(int scalar, boolean onlyModulo) {
        if (scalar == 0)
            throw new ArithmeticException("Division by zero not allowed!");
        int modRes;
        long himask = 0xffffffffL << ISIZE * 8;
        int[] resInt = new int[ICOUNT];
        int[] nInt = toIntArray();
        long tmp = 0L;
        for (int i = ModNumber.ICOUNT - 1; i >= 0; i--) {
            long mask = 0xffffffffL;
            long nLong = nInt[i] & mask;
            tmp |= nLong;
            if (scalar <= tmp) {
                if (!onlyModulo)
                    resInt[i] = (int) (tmp / scalar);
                tmp %= scalar;

            }
            tmp <<= ModNumber.ISIZE * 8;
        }
        modRes = (int) ((tmp & himask) >>> ISIZE * 8);
        ModNumber mres = new ModNumber(resInt);
        return new DivideModuloScalarResult(mres, modRes);
    }

    public ModNumber divide(int scalar) {
        DivideModuloScalarResult result = divideAndModulo(scalar, false);
        return result.div();
    }

    public int modulo(int scalar) {
        DivideModuloScalarResult result = divideAndModulo(scalar, true);
        return result.mod();
    }

    public static boolean lessThan(ModNumber l, ModNumber r) {
        for (int i = LCOUNT - 1; i >= 0; i--) {
            if (l.num[i] == r.num[i])
                continue;
            else if ((l.num[i] >= 0L && r.num[i] >= 0L) || (l.num[i] < 0L && r.num[i] < 0L))
                return l.num[i] < r.num[i];
            else if (l.num[i] < 0L && r.num[i] >= 0L) {
                return false;
            } else if (l.num[i] >= 0L && r.num[i] < 0L) {
                return true;
            }

        }
        return false;
    }

    public static boolean lessThanOrEqual(ModNumber l, ModNumber r) {
        for (int i = LCOUNT - 1; i >= 0; i--) {
            if (l.num[i] == r.num[i])
                continue;
            else if ((l.num[i] >= 0L && r.num[i] >= 0L) || (l.num[i] < 0L && r.num[i] < 0L))
                return l.num[i] < r.num[i];
            else if (l.num[i] < 0L && r.num[i] >= 0L) {
                return false;
            } else if (l.num[i] >= 0L && r.num[i] < 0L) {
                return true;
            }

        }
        return true;
    }

    public static boolean greaterThan(ModNumber l, ModNumber r) {
        for (int i = LCOUNT - 1; i >= 0; i--) {
            if (l.num[i] == r.num[i])
                continue;
            else if ((l.num[i] >= 0L && r.num[i] >= 0L) || (l.num[i] < 0L && r.num[i] < 0L))
                return l.num[i] > r.num[i];
            else if (l.num[i] < 0L && r.num[i] >= 0L) {
                return true;
            } else if (l.num[i] >= 0L && r.num[i] < 0L) {
                return false;
            }
        }
        return false;
    }

    public static boolean greaterThanOrEqual(ModNumber l, ModNumber r) {
        for (int i = LCOUNT - 1; i >= 0; i--) {
            if (l.num[i] == r.num[i])
                continue;
            else if ((l.num[i] >= 0L && r.num[i] >= 0L) || (l.num[i] < 0L && r.num[i] < 0L))
                return l.num[i] > r.num[i];
            else if (l.num[i] < 0L && r.num[i] >= 0L) {
                return true;
            } else if (l.num[i] >= 0L && r.num[i] < 0L) {
                return false;
            }
        }
        return true;
    }

    private int FindFirstNonZeroBitInWord(int word) {
        long mask = 1L << (LSIZE * 8 - 1);
        for (int i = 0; i < LSIZE * 8; i++) {
            if ((num[word] & mask) != 0)
                return i;
            mask >>>= 1;
        }
        return LSIZE * 8;
    }

    public static DivideAndModuloResult divideAndModulo(ModNumber l, ModNumber r, boolean onlyModulo) {
        ModNumber divRes = new ModNumber(0L);
        ModNumber mzero = new ModNumber(0L);
        if (r.equals(mzero))
            throw new ArithmeticException();
        ModNumber mone = new ModNumber(1L);
        if (r.equals(mone))                      // Optimization
        {
            divRes.num = Arrays.copyOf(l.num, l.num.length);
            return new DivideAndModuloResult(divRes, new ModNumber(0L));
        }
        ModNumber mtwo = new ModNumber(2L);
        ModNumber modRes = new ModNumber(0L);
        if (r.equals(mtwo))                      // Optimization
        {
            divRes = ModNumber.shiftRight(l, 1);
            if ((l.num[0] & 0x1L) > 0)
                modRes = new ModNumber(1L);
            return new DivideAndModuloResult(divRes, modRes);
        }
        modRes.num = Arrays.copyOf(l.num, l.num.length);
        if (ModNumber.lessThan(l, r)) {

            return new DivideAndModuloResult(mzero, modRes);
        }
        if (l.equals(r)) {
            return new DivideAndModuloResult(mone, mzero);
        }
        int firstNonzeroWordl = 0;
        int firstNonzeroWordr = 0;
        for (int i = LCOUNT - 1; i >= 0; i--)
            if (l.num[i] != 0) {
                firstNonzeroWordl = i;
                break;
            }
        for (int i = LCOUNT - 1; i >= 0; i--)
            if (r.num[i] != 0) {
                firstNonzeroWordr = i;
                break;
            }
        int nonZeroDifference = firstNonzeroWordl - firstNonzeroWordr;
        for (int i = 0; i <= nonZeroDifference; i++) {
            long[] divisor = new long[LCOUNT];
            long[] rShiftedLeft = new long[LCOUNT];
            for (int j = 0; j <= firstNonzeroWordr; j++) {
                rShiftedLeft[j + nonZeroDifference - i] = r.num[j];
            }
            divisor[nonZeroDifference - i] = 1L;
            ModNumber mRShiftedLeft = new ModNumber(rShiftedLeft);
            ModNumber mDivisor = new ModNumber(divisor);
            int firstBitl = (int) ((LSIZE * 8 - modRes.FindFirstNonZeroBitInWord(firstNonzeroWordl)) + (LSIZE * 8 * firstNonzeroWordl));
            int firstBitr = (int) ((LSIZE * 8 - mRShiftedLeft.FindFirstNonZeroBitInWord(nonZeroDifference + firstNonzeroWordr - i)) + (LSIZE * 8 * (nonZeroDifference + firstNonzeroWordr - i)));
            int firstBitDifference = firstBitl - firstBitr;
            for (int j = 0; j <= firstBitDifference; j++) {
                ModNumber rShiftedLeftBits = new ModNumber(mRShiftedLeft);
                ModNumber mDivisorShiftedLeftBits = new ModNumber(mDivisor);
                ModNumber.shiftLeftAssign(rShiftedLeftBits, firstBitDifference - j);
                if (!onlyModulo)
                    ModNumber.shiftLeftAssign(mDivisorShiftedLeftBits, firstBitDifference - j);
                while (ModNumber.greaterThanOrEqual(modRes, rShiftedLeftBits)) {
                    ModNumber.subtractAssign(modRes, rShiftedLeftBits);
                    if (!onlyModulo)
                        ModNumber.addAssign(divRes, mDivisorShiftedLeftBits);
                }
            }
        }
        return new DivideAndModuloResult(divRes, modRes);
    }

    public static ModNumber divide(ModNumber ml, ModNumber mr) {
        DivideAndModuloResult result = divideAndModulo(ml, mr, false);
        return result.div();
    }

    public static ModNumber modulo(ModNumber ml, ModNumber mr) {
        DivideAndModuloResult result = divideAndModulo(ml, mr, true);
        return result.mod();
    }

    public static ModNumber moduloAssign(ModNumber ml, ModNumber mr) {
        DivideAndModuloResult result = divideAndModulo(ml, mr, true);
        ml.num = result.mod().num;
        return result.mod();
    }
    private static void swap(ModNumber ml, ModNumber mr)
    {
        ModNumber tmp = ml;
        ml = mr;
        mr = tmp;
    }

    public static ModNumber gcd(ModNumber ml, ModNumber mr)
    {
        ModNumber mzero = new ModNumber(0L);
        ModNumber mone = new ModNumber(1L);
        if (ml.equals(mzero) || mr.equals(mzero))
            throw new IllegalArgumentException("Division by Zero not allowed!");
        if (ml.equals(mone))
            return mone;
        if (mr.equals(mone))
            return mone;
        if (ml.equals(mr))
            return new ModNumber(ml);
        ModNumber lcopy = new ModNumber(ml);
        ModNumber rcopy = new ModNumber(mr);
        if (ModNumber.lessThan(lcopy, rcopy))
            swap(lcopy, rcopy);
        ModNumber tmp = ModNumber.modulo(lcopy , rcopy);
        while (!(tmp.equals(mone)))
        {
            if (tmp.equals(mzero))
                return new ModNumber(rcopy);
            lcopy = rcopy;
            rcopy = tmp;
            tmp = ModNumber.modulo(lcopy, rcopy);
        }
        return new ModNumber(tmp);
    }
    public static ModNumber lcm(ModNumber ml, ModNumber mr)
    {
        ModNumber GcdRes = gcd(ml, mr);
        ModNumber lDivGcd = ModNumber.divide(ml , GcdRes);
        return ModNumber.product(lDivGcd , mr);
    }
    public ModNumber sqrt()
    {
        ModNumber mzero = new ModNumber(0L);
        if (this.equals(mzero))
            return new ModNumber(mzero);
        ModNumber mone = new ModNumber(1L);
        if (this.equals(mone))
            return new ModNumber(mone);
        ModNumber mres = new ModNumber(0L);
        ModNumber mdivisor = new ModNumber(0L);
        ModNumber mremainder = new ModNumber(0L);
        int doubleByteCount = (getByteCount() - 1) / 2;
        long longMask = 0xFFFFL;
        for (int i = (int)doubleByteCount; i >= 0; i--)
        {
            short tmp = getDoubleByteValue(i);
            ModNumber.shiftLeftAssign(mremainder, 16);
            ModNumber.shiftLeftAssign(mdivisor, 8);
            ModNumber.shiftLeftAssign(mres, 8);
            long ltmp = ((long)tmp) & longMask;
            mremainder.num[0] |= ltmp;

            if (mremainder.equals(mzero))
                continue;
            short counter = 1;
            ModNumber.addAssignScalar(mdivisor, 1);
            ModNumber mDivisorTimesCounter = ModNumber.productScalar(mdivisor, counter);
            while (ModNumber.lessThan(mDivisorTimesCounter, mremainder))
            {
                ModNumber.addAssignScalar(mdivisor, 1);
                mDivisorTimesCounter = ModNumber.productScalar(mdivisor, ++counter);
            }
            if (mDivisorTimesCounter.equals(mremainder))
            {
                mremainder = new ModNumber(0L);

            }
            else
            {
                ModNumber.subtractAssignScalar(mdivisor, 1);
                mDivisorTimesCounter = ModNumber.productScalar(mdivisor, --counter);
                ModNumber.subtractAssign(mremainder , mDivisorTimesCounter);
            }
            ModNumber.addAssignScalar(mdivisor,counter);
            ModNumber.addAssignScalar(mres, counter);
        }
        return mres;
    }

    public static String adjustStringLength(String s, int desiredLength) {
        StringBuilder result;
        if (desiredLength < s.length()) {
            throw new IllegalArgumentException("Value to large");
        }
        result = new StringBuilder("0".repeat(desiredLength - s.length()));
        result.append(s);

        return result.toString();

    }
    public static ModNumber stomnHexBase(String s) {
        s = adjustStringLength(s, HexStringLength);
        long[] n = new long[LCOUNT];
        for (int i = 0; i < HexStringLength; i += LSIZE*2)
        {
            String tmp = s.substring(i, i + LSIZE);
            long tmpLong = Long.parseLong(tmp, 16);
            tmpLong <<= ISIZE * 8;
            tmp = s.substring(i + LSIZE, i + LSIZE * 2);
            tmpLong |= Long.parseLong(tmp, 16);
            n[LCOUNT - i/(LSIZE*2) - 1] = tmpLong;
        }
        ModNumber mres = new ModNumber(n);
        return mres;
    }
    public static ModNumber stomnOctalBase(String s) {
        long[] n = new long[LCOUNT];
        long buffer = 0L;
        int bitCount = 0;
        int firstBits = (ModNumber.NSIZE % 3 == 0) ? 0 : 3 - ModNumber.NSIZE % 3;
        long mask = 4L;
        String expandedStr = adjustStringLength(s, ModNumber.OctalStringLength);
        for (int i = 0; i < ModNumber.OctalStringLength; i++)
        {
            if (!Character.isDigit(expandedStr.charAt(i)) || expandedStr.charAt(i) == '8' || expandedStr.charAt(i) == '9')
                throw new IllegalArgumentException("Only octal digits allowed!");
            long digit = (long)expandedStr.charAt(i) - '0';
            for (int j = 0; j < 3; j++)
            {
                buffer <<= 1;
                long res = digit & mask;
                res >>>= 2;
                buffer |= res;
                digit <<= 1;
                if (firstBits != 0)
                {
                    firstBits--;
                    continue;
                }
                bitCount++;
                if (bitCount % (8 * ModNumber.LSIZE) == 0)
                {
                    n[ModNumber.LCOUNT - (bitCount / (8 * ModNumber.LSIZE))] = buffer;
                    buffer = 0L;
                }
            }
        }
        return new ModNumber(n);

    }
    public static ModNumber stomnDecimalBase(String s) {
        ModNumber mres = new ModNumber(0L);
        s = adjustStringLength(s, DecimalStringLength);
        for(int i = 0; i < DecimalStringLength;i++)
        {
            if (!Character.isDigit(s.charAt(i)))
                throw new IllegalArgumentException("Only digits allowed");
            int digit = (int)s.charAt(i) - '0';
            ModNumber.productAssignScalar(mres, 10);
            mres.addAssignScalar(0, digit);
        }
        return mres;

    }
    public static ModNumber stomn(String s, int digitBase)
    {
        if (!(digitBase == 8 || digitBase == 10 || digitBase == 16))
            throw new IllegalArgumentException("Only base 8, 10 and 16 are valid");
        if (s.length() == 0)
            return new ModNumber(0);
        int i;
        for (i = 0; i < s.length(); i++)
        {
            if (!Character.isWhitespace(s.charAt(i)))
                break;
        }
        if (i > 0)
            s = s.substring(i);
        if (s.charAt(0) == '-')
            throw new IllegalArgumentException("Only positive numbers allowed");
        if (s.charAt(0) == '+')
            s = s.substring(1);
        switch(digitBase)
        {
            case 8:
                return stomnOctalBase(s);
            case 10:
                return stomnDecimalBase(s);
            case 16:
                return stomnHexBase(s);
        }
        throw new IllegalArgumentException("Only base 8, 10 and 16 are valid");
    }
    public String toString_OctBase() {
        StringBuilder res = new StringBuilder("0".repeat( ModNumber.OctalStringLength));
        int mask = 7;
        int[] thisInt = toIntArray();
        int[] buffer = new int[2];
        buffer[0] = thisInt[0];
        int tripleCount = 0;
        int wordCount = 0;
        for (int i = 0; i < ModNumber.NSIZE; i++)
        {
            if ((wordCount++ % (8 * ModNumber.ISIZE)) == 0)
            {
                if ((wordCount / (8 * ModNumber.ISIZE) + 1) < ModNumber.ICOUNT)
                {
                    buffer[1] = thisInt[wordCount / (8 * ModNumber.ISIZE) + 1];
                }
            }
            if (tripleCount++ % 3 == 0)
            {
                int numToPrint = buffer[0] & mask;
                char charToPrint = (char)('0' + numToPrint);
                res.setCharAt(ModNumber.OctalStringLength - (tripleCount / 3) - 1,charToPrint);
            }
            buffer = ShiftIntArrayR(buffer, 1);
        }
        return res.toString();
    }
    public String toString_DecBase() {
        StringBuilder res = new StringBuilder(ModNumber.DecimalStringLength);
        res.append("0".repeat(ModNumber.DecimalStringLength));
        ModNumber tmp = new ModNumber(this);
        for (int i = 0; i < ModNumber.DecimalStringLength; i++)
        {
            int digit = tmp.modulo( 10);
            tmp = tmp.divide( 10);
            res.setCharAt(ModNumber.DecimalStringLength - i - 1, (char)('0' + (char)digit));
        }
        return res.toString();

    }
    public String toString_HexBase() {
        StringBuilder res = new StringBuilder(ModNumber.HexStringLength + 1);
        int buflen = ModNumber.LSIZE * 2;
        String formatStr = "%0" + buflen + "X";
        for (int i = ModNumber.LCOUNT - 1; i >= 0; i--)
        {
            String digits = String.format(formatStr, num[i]);
            res.append(digits);
        }
        return res.toString();
    }
    public String toString(int nBase)
    {
        switch(nBase)
        {
            case 8:
                return toString_OctBase();
            case 10:
                return toString_DecBase();
            case 16:
                return toString_HexBase();
            default:
                throw new IllegalArgumentException("Base must be 8, 10 or 16");
        }
    }
    public void write(BufferedWriter osw, int base) throws IOException {
        osw.write(toString(base));
    }
    public static ModNumber read(BufferedReader isr, int base) throws IOException {
        String inputStr = isr.readLine();
        return ModNumber.stomn(inputStr, base);
    }
}