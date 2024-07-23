package modularcalculation;

import java.util.Random;
import java.util.concurrent.*;

abstract class DSABase
{
    public abstract ModNumber CalcR(ModNumber mk);
    public byte [] CalculateDSASignature(ModNumber Q, ModNumber x, byte[] hash, boolean DEREncoded)
    {
        byte[] hashLittleEndian;

        hashLittleEndian = ModNumber.convertEndianess(hash);

        ModNumber mHash = new ModNumber(hashLittleEndian);
        int nLen = (int)Q.getByteCount();
        if (hash.length > nLen)
            mHash = ModNumber.getLeftMostBytes(mHash, nLen);
        long[] k = new long[ModNumber.LCOUNT];
        Random random = new Random();
        ModNumber r;
        ModNumber s;
        ModNumber mk;
        ModNumber mzero = new ModNumber(0L);
        ExecutorService executorService = Executors.newFixedThreadPool(2);
        int lSize = nLen / ModNumber.LSIZE;
        do
        {
            do
            {
                for (int i = 0; i < lSize; i++)
                    k[i] = (random.nextLong());

                mk = new ModNumber(k);
                if (mk == mzero)
                    ModNumber.addAssignScalar(mk, (int)(random.nextInt() + 1));
                while (ModNumber.greaterThanOrEqual(mk, Q))
                {
                    mk.num[lSize-1] -= random.nextLong();
                }
                r = CalcR(mk);
            } while (r == mzero
            || (r.num[lSize-1] & 0x8000000000000000L) != 0 // necessary only for BouncyCastle ECDSA implementation
            );
            final ModNumber mkRef = mk;
            final ModNumber rRef = r;
            final ModNumber mHashRef = mHash;
            ModNumber kInverse = null;
            final MultGroupMod mgm = new MultGroupMod(Q);
            Future<ModNumber> result1 = executorService.submit(() ->  mgm.Inverse(mkRef));
            ModNumber hashPlusXr = null;
            Future<ModNumber> result2 = executorService.submit(() ->
                    {
                            ModNumber xr = mgm.Mult(x, rRef);
                            return mgm.Add(mHashRef, xr);
                });
            try {
                kInverse = result1.get();
                hashPlusXr = result2.get();
            }
            catch (InterruptedException | ExecutionException e)
            {
                e.printStackTrace();
            }
            s = mgm.Mult(kInverse, hashPlusXr);
        } while (s == mzero
                || (s.num[lSize-1] & 0x8000000000000000L) != 0 // necessary only for BouncyCastle ECDSA implementation
        );
        executorService.shutdown();
        if (!(ModNumber.lessThan(s, Q) && ModNumber.lessThan(r, Q)))
            throw new IllegalArgumentException("Wrong signature");
        byte[] rBigEndian = r.convertEndianess(nLen);
        int i;
        for (i = 0; i < rBigEndian.length; i++)
            if (rBigEndian[i] != 0)
                break;
        if (i > 0){
            byte [] rBigEndianSmaller = new byte[rBigEndian.length - i];
            System.arraycopy(rBigEndian, 0, rBigEndianSmaller, 0, rBigEndianSmaller.length);
            rBigEndian = rBigEndianSmaller;
        }
        byte[] sBigEndian = s.convertEndianess(nLen);
        for (i = 0; i < sBigEndian.length; i++)
            if (sBigEndian[i] != 0)
                break;
        if (i > 0){
            byte [] sBigEndianSmaller = new byte[sBigEndian.length - i];
            System.arraycopy(sBigEndian, 0, sBigEndianSmaller, 0, sBigEndianSmaller.length);
            sBigEndian = sBigEndianSmaller;
        }

        if (DEREncoded)
        {
            return CreateBERASNStringForDSASignature(rBigEndian, sBigEndian);
        }
        else
        {
            byte[] rs = new byte[rBigEndian.length + sBigEndian.length];
            for (int j = 0; j < rBigEndian.length; j++)
                rs[j] = rBigEndian[j];
            for (int j = 0; j < sBigEndian.length; j++)
                rs[rBigEndian.length + j] = sBigEndian[j];
            return rs;
        }
    }
    byte [] CreateBERASNStringForDSASignature(byte[] r, byte[] s)
    {
        int nLen = r.length  + s.length;
        int extraBytes = 4;
        int lenSize;
        byte[] retValue;
        int index = 0;

        if (nLen + extraBytes > 0x7F) {
            int numLenBytes = 1;
            int tmpLen = nLen + extraBytes;
            while ((tmpLen >>>= 8) != 0)
                numLenBytes++;
            lenSize = numLenBytes + nLen + extraBytes + 2;
            retValue = new byte[lenSize];
            retValue[index++] = (byte)(ASNElementType.SEQUENCE.getElementNumber() | 0x20);
            retValue[index++] = (byte)(0x80 | numLenBytes);
            index += numLenBytes;
            tmpLen = nLen + extraBytes;
            for (int i = 0; i < numLenBytes; i++)
            {
                retValue[index - i - 1] = (byte)(tmpLen & 0xFF);
                tmpLen >>>= 8;
            }
        }
        else {
            lenSize = nLen + extraBytes + 2;
            retValue = new byte[lenSize];
            retValue[index++] = (byte)(ASNElementType.SEQUENCE.getElementNumber() | 0x20);
            retValue[index++] = (byte)(r.length + s.length + extraBytes);
        }
        retValue[index++] = (byte)ASNElementType.INTEGER_VALUE.getElementNumber();
        retValue[index++] = (byte)r.length;
        for (int i = 0; i < r.length; i++)
            retValue[index + i] = r[i];
        retValue[index++ + r.length] = (byte)ASNElementType.INTEGER_VALUE.getElementNumber();
        retValue[index++ + r.length] = (byte)s.length;
        for (int i = 0; i < s.length; i++)
            retValue[index + r.length + i] = s[i];
        return retValue;
    }

    public String ConvertSignatureToString(byte [] signature, boolean DEREncoded)
    {
        if (DEREncoded)
        {
            StringBuilder sb = new StringBuilder(signature.length * 2);
            for (int i = 0; i < signature.length; i++)
            {
                String tmp = String.format("%02X", signature[i]);
                sb.append(tmp);
            }
            return sb.toString();
        }
        else
        {
            StringBuilder sb = new StringBuilder(signature.length / 2);
            sb.append(" ".repeat(signature.length / 2));
            for (int i = 0; i < signature.length; i += 2)
            {
                char c = (char)signature[i];
                c |= (char)(signature[i + 1] << 8);
                sb.setCharAt(i / 2,  c);
            }
            return sb.toString();
        }
    }

}
