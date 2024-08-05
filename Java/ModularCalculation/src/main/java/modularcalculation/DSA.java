package modularcalculation;

import java.util.List;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class DSA extends DSABase
{
        public DSA(DSAParameters parameters)
        {
            P = parameters.P;
            Q = parameters.Q;
            g = parameters.g;
            x = parameters.x;
            y = parameters.y;
        }
        @Override
        public  ModNumber CalcR(ModNumber mk)
        {
            MultGroupMod mgm = new MultGroupMod(P);
            return ModNumber.modulo(mgm.Exp(g, mk), Q);
        }
public static DSACalculateU1U2MrResult DSACalculateU1U2Mr(ModNumber Q, byte[] hash, String signature, boolean DEREncoded)
{
        byte[] r;
        byte[] s;
        if (DEREncoded)
        {
            byte [] signatureBytesBigEndian = ModNumber.stringToBytes(signature);
            byte [] signatureBytesLittleEndian = ModNumber.convertEndianess(signatureBytesBigEndian);
            List<Object> signatureOctets;
            signatureOctets = ModNumber.ParseBERASNString(signatureBytesLittleEndian);
            r = (byte[])signatureOctets.get(0);
            s = (byte[])signatureOctets.get(1);
        }
        else
        {
            r = new byte[signature.length()];
            for (int i = 0; i < signature.length() / 2; i++)
            {
                r[i * 2] = (byte)signature.charAt(i);
                r[i * 2 + 1] = (byte)(signature.charAt(i) >>> 8);
                }
            s = new byte[signature.length()];
            for (int i = 0; i < signature.length() / 2; i++)
            {
                s[i * 2] = (byte)signature.charAt(signature.length() / 2 + i);
                s[i * 2 + 1] = (byte)(signature.charAt(signature.length() / 2 + i) >>> 8);
            }
        }
        return DSACalculateU1U2Mr(Q, hash, r, s);
}

public static DSACalculateU1U2MrResult DSACalculateU1U2Mr(ModNumber Q, byte[] hash, byte[] r, byte[] s)
{
        byte[] rLittleEndian;
        byte[] sLittleEndian;
        rLittleEndian = ModNumber.convertEndianess(r);
        sLittleEndian = ModNumber.convertEndianess(s);

        ModNumber mHash;
        int bcQ = Q.getByteCount();
        byte[] hashLittleEndian = ModNumber.convertEndianess(hash);
        mHash = new ModNumber(hashLittleEndian);
        if (hash.length > bcQ)
        {
            mHash = ModNumber.getLeftMostBytes(mHash, bcQ);
        }
        final ModNumber mHashRef = mHash;
        final ModNumber mr = new ModNumber(rLittleEndian);
        final ModNumber ms = new ModNumber(sLittleEndian);
        if (!(ModNumber.lessThan(mr, Q) && ModNumber.lessThan(ms, Q)))
            throw new IllegalArgumentException("Invalid signature");
        final MultGroupMod mgm = new MultGroupMod(Q);
        final ModNumber sInverse = mgm.Inverse(ms);
        ModNumber u1 = null;
        ExecutorService executorService = Executors.newFixedThreadPool(2);
        Future<ModNumber> resultU1 = executorService.submit(() -> mgm.Mult(mHashRef, sInverse));

        ModNumber u2 = null;
        Future<ModNumber> resultU2 = executorService.submit(() -> mgm.Mult(mr, sInverse));
        try {
            u1 = resultU1.get();
            u2 = resultU2.get();
        }
        catch (InterruptedException | ExecutionException e)
        {
            e.printStackTrace();
        }
        executorService.shutdown();
        return new DSACalculateU1U2MrResult(u1, u2, mr);
}
public String sign(byte[] hash, boolean DEREncoded)
{
        byte[] signature = CalculateDSASignature(Q, x, hash, DEREncoded);
        return ConvertSignatureToString(signature, DEREncoded);
}
public boolean verify(byte[] hash, String signature, boolean DEREndoded)
{
        final MultGroupMod mgm = new MultGroupMod(P);
        ModNumber mv1 = null;
        final DSACalculateU1U2MrResult u1U2MrResult = DSACalculateU1U2Mr(Q, hash, signature, DEREndoded);
        ExecutorService executorService = Executors.newFixedThreadPool(2);
        Future<ModNumber> mv1Result = executorService.submit(() ->  mgm.Exp(g, u1U2MrResult.u1()));
         ModNumber mv2 = null;
        Future<ModNumber> mv2Result = executorService.submit(() -> mgm.Exp(y, u1U2MrResult.u2()));
        try {
            mv1 = mv1Result.get();
            mv2 = mv2Result.get();
        }
        catch (InterruptedException | ExecutionException e)
        {
            e.printStackTrace();
        }
        executorService.shutdown();
        ModNumber mv = ModNumber.modulo(mgm.Mult(mv1, mv2), Q);
        return mv.equals(u1U2MrResult.mr());
}

    private ModNumber P;
    private ModNumber Q;
    private ModNumber g;
    private ModNumber x;
    private ModNumber y;
}
