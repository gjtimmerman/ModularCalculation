package modularcalculation;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class ECDSA extends DSABase
{
public ECDSA (ECKeyPair keyPair)
        {
        this.ecKeyPair = keyPair;
        }
public ECKeyPair ecKeyPair;
@Override
public  ModNumber CalcR(ModNumber mk)
{
        ECPoint ecPoint = ecKeyPair.ec.Mult(ecKeyPair.ec.g, mk);
        if (ecPoint.IsAtInfinity)
            throw new IllegalArgumentException("Wrong parameters!");
        return ModNumber.modulo(ecPoint.x, ecKeyPair.ec.n);
}
public String sign(byte[] hash, boolean DEREncoded)
{
        byte [] signature =  CalculateDSASignature(ecKeyPair.ec.n, ecKeyPair.mx, hash, DEREncoded);
        return ConvertSignatureToString(signature, DEREncoded);
}
public byte[] sign(byte[] hash)
{
        byte[] signature = CalculateDSASignature(ecKeyPair.ec.n, ecKeyPair.mx, hash, false);
        return signature;
}

public boolean verify(byte [] hash, byte[] signature)
        {
        byte[] r = new byte[signature.length/2];
        byte[] s = new byte[signature.length/2];
        for (int i = 0; i < signature.length/2; i++)
            r[i] = signature[i];
        for (int i = 0; i < signature.length / 2; i++)
            s[i] = signature[i + r.length];
        DSACalculateU1U2MrResult result = DSA.DSACalculateU1U2Mr(ecKeyPair.ec.n, hash, r, s);
        ECPoint ptv = CalculateV(hash, result.u1(), result.u2(), result.mr());
        if (ptv.IsAtInfinity)
            return false;
        return ptv.x.equals( result.mr());
        }
public boolean verify(byte[] hash, String signature, boolean DEREncoded)
{
        DSACalculateU1U2MrResult result = DSA.DSACalculateU1U2Mr(ecKeyPair.ec.n, hash, signature, DEREncoded);
        ECPoint ptv = CalculateV(hash, result.u1(), result.u2(), result.mr());
        if (ptv.IsAtInfinity)
            return false;
        return ptv.x.equals( result.mr());
}
public  ECPoint CalculateV(byte[] hash, ModNumber u1, ModNumber u2, ModNumber mr)
{
        ECPoint pt1 = null;
        ECPoint pt2 = null;
        ExecutorService executorService = Executors.newFixedThreadPool(2);
        Future<ECPoint> result1 = executorService.submit(() -> ecKeyPair.ec.Mult(ecKeyPair.ec.g, u1) );

        Future<ECPoint> result2 = executorService.submit(() -> ecKeyPair.ec.Mult(ecKeyPair.y, u2));
        try {
            pt1 = result1.get();
            pt2 = result2.get();
        }
        catch (InterruptedException | ExecutionException e)
        {
            e.printStackTrace();
        }
        executorService.shutdown();
        ECPoint ptv = ecKeyPair.ec.Add(pt1, pt2);
        return ptv;
        }
}
