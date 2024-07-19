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
        byte[] sBigEndian = s.convertEndianess(nLen);
        if (DEREncoded)
        {
            return CreateBERASNStringForDSASignature(rBigEndian, sBigEndian);
        }
        else
        {
            byte[] rs = new byte[rBigEndian.length + sBigEndian.length];
            for (int i = 0; i < rBigEndian.length; i++)
                rs[i] = rBigEndian[i];
            for (int i = 0; i < sBigEndian.length; i++)
                rs[rBigEndian.length + i] = sBigEndian[i];
            return rs;
        }
    }
    byte [] CreateBERASNStringForDSASignature(byte[] r, byte[] s)
    {
        int nLen = r.length  + s.length;
        int lenSize;
        if (nLen + 4 > 127)
            lenSize = nLen + 6 + 2;
        else
            lenSize = nLen + 6;
        byte[] retValue = new byte[lenSize];
        int index = 0;
        retValue[index++] = (byte)(ASNElementType.SEQUENCE.getElementNumber() | 0x20);
        if (r.length  + s.length > 123)
        {
            retValue[index++] = (byte)0x82;
            retValue[index++] = (byte)0x0;
            retValue[index++] = (byte)(r.length + 2 + s.length + 2);
        }
        else
            retValue[index++] = (byte)(r.length + 2 + s.length + 2);
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
