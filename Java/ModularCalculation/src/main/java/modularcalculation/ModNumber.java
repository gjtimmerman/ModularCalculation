package modularcalculation;

public class ModNumber {
    public final static int MaxMod = 1024/8;

    public final static int LSIZE = 8;
    public final static int ISIZE = 4;
    public final static int NCOUNT = MaxMod + LSIZE;
    public final static int COUNTMOD = MaxMod / LSIZE;
    public final static int LCOUNT = NCOUNT / LSIZE;
    public final static int ICOUNT = NCOUNT / ISIZE;
    public final static int NSIZE = NCOUNT * LSIZE;
    public final static int HexStringLength = NCOUNT * 2;
    public final static int OctalStringLength = (NSIZE % 3 == 0) ? (NSIZE / 3) : (NSIZE / 3 + 1);
    public final static int DecimalStringLength = (int)(NSIZE * 0.30102999566398119521373889472449) + 1; // log(2)

    long [] num = new long[LCOUNT];

    public ModNumber(long n)
    {
        num[0] = n;
    }
    public ModNumber(long[] num)
    {
        this.num = num;
    }
    public ModNumber(ModNumber m)
    {
        num = m.num.clone();
    }
    public ModNumber(byte[] n)
    {
        int lCount = n.length / LSIZE;
        long tmp;
        for (int i = 0; i < lCount; i++)
        {
            tmp = 0L;
            for (int j = 0; j < LSIZE; j++)
            {
                tmp |= ((long)n[i * LSIZE + j] << j * 8);
            }
            num[i] = tmp;
        }
        int lSize = n.length % LSIZE;
        if (lSize == 0)
            return;
        tmp = 0L;
        for (int j = 0; j < lSize; j++)
        {
            tmp |= ((long)n[lCount * LSIZE + j] << j * 8);
        }
        num[lCount] = tmp;
    }
    public ModNumber (long [] num, int byteLen)
    {
        int lCount = byteLen / LSIZE;
        for (int i = 0; i < lCount; i++)
            this.num[i] = num[i];
        int lSize = byteLen % LSIZE;
        if (lSize == 0)
            return;
        long tmp = 0L;
        long mask = 0xff;
        for (int j = 0; j < lSize; j++)
        {
            long masked = num[lCount] & mask;
            tmp |= masked;
            mask <<= 8;
        }
        this.num[lCount] = tmp;
    }
    public ModNumber (int[] arr)
    {
        fromIntArray(arr);
    }
    public int[] toIntArray()
    {
        int []result = new int[ICOUNT];
        long lomask = 0xffffffff;
        long himask = lomask << ISIZE * 8;
        for (int i = 0; i < LCOUNT; i++)
        {
            result[i * 2] = (int)(num[i] & lomask);
            result[i * 2 + 1] = (int)((num[i] & himask) >> ISIZE * 8);
        }
        return result;
    }
    public void fromIntArray(int[] arr)
    {
        for (int i = 0; i < LCOUNT; i++)
        {
            num[i] = arr[i * 2];
            num[i] |= ((long)arr[i * 2 + 1]) << ISIZE * 8;
        }

    }

    @Override
    public  boolean equals(Object other)
    {
        if (other instanceof ModNumber om)
        {
            for (int i = 0; i < om.num.length; i++)
            {
                if (num[i] != om.num[i])
                    return false;
            }
            return true;
        }
        return false;
    }
    @Override
    public int hashCode()
    {
        return num.hashCode();
    }
     void checkMax(int size)
    {
        for (int i = size; i < LCOUNT; i++)
            if (num[i] != 0L)
        throw new IllegalArgumentException("Modulus is too large!");
    }


    public static ModNumber subtractScalar(ModNumber l, int r) {
        int[] resInt = l.toIntArray();
        int carry = 0;
        int ltmp = resInt[0];
        long lr = 0;
        lr |= r;
        if (ltmp < lr)
            carry = 1;
        resInt[0] = (int)(ltmp - lr);
        for (int i = 1; carry > 0 && i < ICOUNT; i++)
        {
            if (carry <= resInt[i])
            {
                resInt[i] -= carry;
                carry = 0;
            }
            else
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
}
