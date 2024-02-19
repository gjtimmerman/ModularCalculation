package modularcalculation;

import java.util.Arrays;

public class ModNumber {
    public final static int MaxMod = 1024 / 8;

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

    public static ModNumber addAssignScalar(ModNumber l, int scalar) {
        ModNumber mres = new ModNumber(l);
        mres.addAssignScalar(0, scalar);
        return mres;
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
            long tmp = ((long) nInt[j + words + 1]) << ((ISIZE * 8) - i);
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
            tmp |= nInt[i];
            if (tmp < 0)
                tmp >>>= ModNumber.ISIZE * 8;
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
        if (s.charAt(i) == '+')
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

}