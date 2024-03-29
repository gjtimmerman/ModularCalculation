package modularcalculation;

public class ScaledNumber
{
    public ScaledNumber(ModNumber mn, int scale, boolean alreadyScaled)
    {
        if (alreadyScaled)
            this.mn = mn;
        else
            this.mn = ModNumber.shiftLeft(mn, 8 * scale);
        this.scale = scale;
    }
    public int scale;
    private ModNumber mn;
    @Override
    public boolean equals(Object obj) {
        if (obj == null)
            return false;
        else {
            if (obj instanceof ScaledNumber other)
                return scale == other.scale && mn.equals(other.mn);
            else
                return false;
        }
    }
    @Override
    public int hashCode() {
        return scale + mn.hashCode();
    }
    public String toString(int nBase)
    {
        switch (nBase)
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
    public StringLengthResult calculateOctalStringLength() {
        int integralDigits;
        int fractionDigits;

        if ((ModNumber.NCOUNT - scale) * 8 % 3 == 0)
            integralDigits = (ModNumber.NCOUNT - scale) * 8 / 3;
        else
            integralDigits =  (ModNumber.NCOUNT - scale) * 8 / 3 + 1;
        if ((scale * 8 % 3) == 0)
            fractionDigits = (scale * 8 / 3);
        else
            fractionDigits = (scale * 8 / 3 + 1);
        return new StringLengthResult(integralDigits, fractionDigits);
    }
    private String toString_OctBase()
    {
        StringBuilder res = new StringBuilder(ModNumber.OctalStringLength+3);
        int mask = 7;
        int wordsToSkip = scale / ModNumber.ISIZE;
        int bitsToSkip = (scale % ModNumber.ISIZE) * 8;
        int[] mnInt = mn.toIntArray();
        int[] buffer = new int[2];
        buffer[0] = mnInt[wordsToSkip];
        buffer[1] = mnInt[wordsToSkip + 1];
        int tripleCount = 0;
        StringLengthResult stringLengthResult = calculateOctalStringLength();
        res.append("0".repeat( stringLengthResult.digitsToSkip() + stringLengthResult.digitsLeft() + 1));
        res.setCharAt(stringLengthResult.digitsLeft(), '.');
        int wordCount = bitsToSkip;
        buffer = ModNumber.ShiftIntArrayR(buffer, wordCount);
        for (int i = scale * 8; i < ModNumber.NSIZE; i++)
        {
            if ((wordCount++ % (8 * ModNumber.ISIZE)) == 0)
            {
                if (wordCount / (8 * ModNumber.ISIZE) + 1 < ModNumber.ICOUNT - wordsToSkip)
                    buffer[1] = mnInt[wordsToSkip+wordCount / (8 * ModNumber.ISIZE) + 1];
                else
                    buffer[1] = 0;
            }
            if (tripleCount++ % 3 == 0)
            {
                int numToPrint = buffer[0] & mask;
                char charToPrint = (char)('0' + numToPrint);
                res.setCharAt( stringLengthResult.digitsLeft()- (tripleCount / 3) - 1, charToPrint);
            }
            buffer = ModNumber.ShiftIntArrayR(buffer, 1);
        }
        buffer[1] = mnInt[wordsToSkip];
        if (wordsToSkip > 0)
        {
            buffer[0] = mnInt[wordsToSkip-1];
        }
        else
            buffer[0] = 0;
        buffer = ModNumber.ShiftIntArrayL(buffer, (ModNumber.ISIZE * 8) - bitsToSkip);
        wordCount = ModNumber.ISIZE * 8 - bitsToSkip;
        tripleCount = 0;
        for (int i = 0; i < scale * 8; i++)
        {
            if ((wordCount++ % (8 * ModNumber.ISIZE)) == 0)
                if (wordCount < (wordsToSkip * ModNumber.ISIZE * 8))
                    buffer[0] = mnInt[wordsToSkip -1 - (wordCount / (8 * ModNumber.ISIZE))];
                else
                    buffer[0] = 0;
            if (tripleCount++ % 3 == 0)
            {
                int numToPrint = (buffer[1] >> (32 - 3)) & mask;
                char charToPrint = (char)('0' + numToPrint);
                res.setCharAt(stringLengthResult.digitsLeft() + (tripleCount / 3) + 1, charToPrint);
            }
            buffer = ModNumber.ShiftIntArrayL(buffer, 1);
        }
        return res.toString();
    }
    public int calculateDecimalStringLengthLeft()
    {
        return (int)Math.ceil((ModNumber.NSIZE - scale * 8) * 0.30102999566398119521373889472449); // log(2)
    }
    private String toString_DecBase()
    {
        int IntegerStringLength = calculateDecimalStringLengthLeft();
        StringBuilder sb = new StringBuilder(ModNumber.DecimalStringLength+3);
        sb.append("0".repeat(IntegerStringLength));
        ModNumber tmp = ModNumber.shiftRight(mn , scale * 8);
        for (int i = 0; i < IntegerStringLength; i++)
        {
            int digit = tmp.modulo( 10);
            tmp = tmp.divide(10);
            sb.setCharAt(IntegerStringLength - i - 1, (char)(sb.charAt(IntegerStringLength - i - 1) + ((char)digit)));

        }
        if (scale > 0)
        {
            sb.append('.');
            ModNumber tmpFraction = new ModNumber(mn);
            String tmpFractionStr1 = tmpFraction.toString_DecBase();
            ModNumber divisor = new ModNumber(1L);
            for (int i = 0; i < scale * 2; i++)
                ModNumber.productAssignScalar(divisor, 10);
            tmpFraction = ModNumber.product(tmpFraction, divisor);
            String tmpFractionStr2 = tmpFraction.toString_DecBase();
            ModNumber.shiftRightAssign(tmpFraction, scale * 8);
            String tmpFractionStr3 = tmpFraction.toString_DecBase();
            ModNumber.moduloAssign(tmpFraction, divisor);
            String tmpFractionStr4 = tmpFraction.toString_DecBase();
            for (int i = 0; i < scale * 2; i++)
            {
                divisor = divisor.divide(10);
                ModNumber resTmp = ModNumber.divide(tmpFraction, divisor);
                String resTmpStr = resTmp.toString_DecBase();
                int digit = resTmp.modulo(10);
                sb.append((char)('0' + digit));
            }

        }
        return sb.toString();
    }
    private String toString_HexBase()
    {
        int bufLen = ModNumber.LSIZE * 2;
        StringBuilder sb = new StringBuilder(bufLen * ModNumber.LCOUNT + 1);
        String formatStr = "%0" + bufLen + "X";
        for (int i = ModNumber.LCOUNT - 1; i >= 0; i--)
        {
            String tmp = String.format(formatStr,this.mn.num[i]);
            sb.append(tmp);
        }
        if (scale > 0)
        {
            int pos = bufLen * ModNumber.LCOUNT - (scale * 2);
            sb.insert(pos, '.');
        }
        return sb.toString();
    }
    public ScaledNumber sqrt()
    {
        ModNumber rt = mn.sqrt();
        return new ScaledNumber(rt, scale / 2, true);
    }
}

