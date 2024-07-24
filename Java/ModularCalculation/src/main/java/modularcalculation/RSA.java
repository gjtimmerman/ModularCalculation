package modularcalculation;

import java.util.List;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class RSA
{
    public RSA(RSAParameters rsaParameters)
    {
        PubExp = rsaParameters.PubExp;
        Modulus = rsaParameters.Modulus;
        Prime1 = rsaParameters.Prime1;
        Prime2 = rsaParameters.Prime2;
        Exp1 = rsaParameters.Exp1;
        Exp2 = rsaParameters.Exp2;
        Coefficient = rsaParameters.Coefficient;
        PrivExp = rsaParameters.PrivExp;
    }
    public ModNumber encrypt(ModNumber m, int size)
    {
        ModNumber masked = m.getPKCS1Mask(false, ModNumber.MaxMod, size);
        MultGroupMod mgm = new MultGroupMod(Modulus);
        return mgm.Exp(masked, PubExp);
    }
    public ModNumber decrypt(ModNumber c)
    {
        MultGroupMod mgmp = new MultGroupMod(Prime1);
        MultGroupMod mgmq = new MultGroupMod(Prime2);
        MultGroupMod mgmn = new MultGroupMod(Modulus);
        ModNumber m1 = null;
        ModNumber m2 = null;
        ExecutorService executorService = Executors.newFixedThreadPool(2);
        Future<ModNumber> result1 = executorService.submit(() -> mgmp.Exp(c, Exp1));
        Future<ModNumber> result2 = executorService.submit(() -> mgmq.Exp(c, Exp2));
        try
        {
            m1 = result1.get();
            m2 = result2.get();
        }
        catch (InterruptedException | ExecutionException e)
        {
            e.printStackTrace();
        }
        executorService.shutdown();
        ModNumber diff = mgmp.Diff(m1, m2);
        ModNumber h = mgmp.Mult(Coefficient, diff);
        ModNumber hq = mgmn.Mult(h, Prime2);
        ModNumber res = mgmn.Add(m2, hq);
        return res.removePKCS1Mask();
    }
    public ModNumber encryptSignature(byte [] hashBigendian, String hashOid)
    {
        ModNumber unmaskedResult = ModNumber.createBERASNString(hashBigendian, hashOid);
        ModNumber maskedResult = unmaskedResult.getPKCS1Mask(true, ModNumber.MaxMod, 0);
        MultGroupMod mgmp = new MultGroupMod(Prime1);
        MultGroupMod mgmq = new MultGroupMod(Prime2);
        MultGroupMod mgmn = new MultGroupMod(Modulus);
        ModNumber m1 = null;
        ModNumber m2 = null;
        ExecutorService executorService = Executors.newFixedThreadPool(2);
        Future<ModNumber> result1 = executorService.submit(() -> mgmp.Exp(maskedResult, Exp1));
        Future<ModNumber> result2 = executorService.submit(() -> mgmq.Exp(maskedResult, Exp2));
        try
        {
            m1 = result1.get();
            m2 = result2.get();
        }
        catch (InterruptedException | ExecutionException e)
        {
            e.printStackTrace();
        }
        executorService.shutdown();
       ModNumber diff = mgmp.Diff(m1, m2);
        ModNumber h = mgmp.Mult(Coefficient, diff);
        ModNumber hq = mgmn.Mult(h, Prime2);
        ModNumber res = mgmn.Add(m2, hq);
        return res;
    }
    public ModNumber decryptSignature(ModNumber signature)
    {
        MultGroupMod mgm = new MultGroupMod(Modulus);
        ModNumber decryptedSignature = mgm.Exp(signature, PubExp);
        ModNumber removedMask = decryptedSignature.removePKCS1Mask();
        byte [] removedMaskBytes = removedMask.toByteArray();
        List<Object> result = ModNumber.ParseBERASNString(removedMaskBytes);
        byte [] hashBigEndian = (byte [])result.get(1);
        byte[] hashLittleEndian;

        hashLittleEndian = ModNumber.convertEndianess(hashBigEndian);


        ModNumber retval = new ModNumber(hashLittleEndian);
        return retval;

    }

    private ModNumber PubExp;
    private ModNumber Modulus;
    private ModNumber Prime1;
    private ModNumber Prime2;
    private ModNumber Exp1;          // DP
    private ModNumber Exp2;          // DQ
    private ModNumber Coefficient;   // InverseQ
    private ModNumber PrivExp;

}
