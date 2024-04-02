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
            ModNumber mSignature = ModNumber.stomn(signature, 16);
            List<Object> signatureOctets;
            signatureOctets = mSignature.ParseBERASNString();
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
        return new DSACalculateU1U2MrResult(u1, u2, mr);
}
public String Sign(byte[] hash, boolean DEREncoded)
{
        byte[] signature = CalculateDSASignature(Q, x, hash, DEREncoded);
        return ConvertSignatureToString(signature, DEREncoded);
}
public boolean Verify(byte[] hash, String signature, boolean DEREndoded)
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

        ModNumber mv = ModNumber.modulo(mgm.Mult(mv1, mv2), Q);
        return mv.equals(u1U2MrResult.mr());
}
//public static DSAParameters GetDSAKey(String keyName, boolean createIfNotExists)
//        {
//        CngKey cngKey;
//        if (!CngKey.Exists(keyName))
//        if (createIfNotExists)
//        {
//        CngKeyCreationParameters parametersSigningDsa = new CngKeyCreationParameters();
//        parametersSigningDsa.KeyUsage = CngKeyUsages.Signing;
//        byte[] keyLen = new byte[4];
//        keyLen[0] = (ModNumber.MaxMod * 8) % 0x100;
//        keyLen[1] = (ModNumber.MaxMod * 8) / 0x100;
//        CngPropertyOptions keyOptions = new CngPropertyOptions();
//
//        CngProperty prop = new CngProperty("Length", keyLen, keyOptions);
//        parametersSigningDsa.Parameters.Add(prop);
//        byte[] exportp = new byte[4];
//        exportp[0] = 0x03;
//        prop = new CngProperty("Export Policy", exportp, keyOptions);
//        parametersSigningDsa.Parameters.Add(prop);
//
//        cngKey = CngKey.Create(new CngAlgorithm("DSA"), keyName, parametersSigningDsa);
//        }
//        else
//        throw new ApplicationException("Key does not exist!");
//        else
//        cngKey = CngKey.Open(keyName);
//        DSAParameters dsaParameters;
//        CngPropertyOptions options = new CngPropertyOptions();
//        CngProperty property = cngKey.GetProperty("Length", options);
//        byte[]? length = property.GetValue();
//        int? keyLength = (int?)(length?[1] * 0x100);
//        keyLength += length?[0];
//        if (keyLength > ModNumber.MaxMod * 8)
//        throw new ApplicationException("Keylength not less or equal to MAXMOD!");
//        byte[] keyBlob = cngKey.Export(new CngKeyBlobFormat("DSAPRIVATEBLOB"));
//        #if SMALLMOD
//        if (keyBlob[0] != 0x44 && keyBlob[1] != 0x53 && keyBlob[2] != 0x50 && keyBlob[3] != 0x56)
//        throw new ApplicationException("Key structure not of type RSA Full Private!");
//        int byteLength = keyBlob[5] * 0x100;
//        byteLength += keyBlob[4];
//        if (byteLength != ModNumber.MaxMod)
//        throw new ApplicationException("Keylength not equal to MAXMOD!");
//        #endif
//        #if SMALLMOD && UNSAFE
//        unsafe
//        {
//        fixed (byte* pData = &keyBlob[32])
//        {
//        byte* p = pData;
//        byte[] qLittleEndian = ModNumber.convertEndianess(p, 20);
//        dsaParameters.Q = new ModNumber(qLittleEndian);
//        p += 20;
//        byte[] pLittleEndian = ModNumber.convertEndianess(p, byteLength);
//        dsaParameters.P = new ModNumber(pLittleEndian);
//        p += byteLength;
//        byte[] gLittleEndian = ModNumber.convertEndianess(p, byteLength);
//        dsaParameters.g = new ModNumber(gLittleEndian);
//        p += byteLength;
//        byte[] yLittleEndian = ModNumber.convertEndianess(p, byteLength);
//        dsaParameters.y = new ModNumber(yLittleEndian);
//        p += byteLength;
//        byte[] xLittleEndian = ModNumber.convertEndianess(p, 20);
//        dsaParameters.x = new ModNumber(xLittleEndian);
//        }
//        }
//        #endif
//        #if SMALLMOD && !UNSAFE
//        int offset = 32;
//        byte[] qLittleEndian = ModNumber.convertEndianess(keyBlob, 20, offset);
//        dsaParameters.Q = new ModNumber(qLittleEndian);
//        offset += 20;
//        byte[] pLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
//        dsaParameters.P = new ModNumber(pLittleEndian);
//        offset += byteLength;
//        byte[] gLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
//        dsaParameters.g = new ModNumber(gLittleEndian);
//        offset += byteLength;
//        byte[] yLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
//        dsaParameters.y = new ModNumber(yLittleEndian);
//        offset += byteLength;
//        byte[] xLittleEndian = ModNumber.convertEndianess(keyBlob, 20, offset);
//        dsaParameters.x = new ModNumber(xLittleEndian);
//
//        #endif
//        #if MEDMOD || LARGEMODSIGNATURE
//        if (keyBlob[0] != 0x44 && keyBlob[1] != 0x50 && keyBlob[2] != 0x56 && keyBlob[3] != 0x32)
//        throw new ApplicationException("Key structure not of type RSA Full Private!");
//        int byteLength = keyBlob[5] * 0x100;
//        byteLength += keyBlob[4];
//        if (byteLength != ModNumber.MaxMod)
//        throw new ApplicationException("Keylength not equal to MAXMOD!");
//        int seedLength = keyBlob[17] * 0x100;
//        seedLength += keyBlob[16];
//        int groupSize = keyBlob[21] * 0x100;
//        groupSize += keyBlob[20];
//        #endif
//        #if (MEDMOD || LARGEMODSIGNATURE) && UNSAFE
//        unsafe
//        {
//        fixed (byte* pData = &keyBlob[28 + seedLength])
//        {
//        byte* p = pData;
//        byte[] qLittleEndian = ModNumber.convertEndianess(p, groupSize);
//        dsaParameters.Q = new ModNumber(qLittleEndian);
//        p += groupSize;
//        byte[] pLittleEndian = ModNumber.convertEndianess(p, byteLength);
//        dsaParameters.P = new ModNumber(pLittleEndian);
//        p += byteLength;
//        byte[] gLittleEndian = ModNumber.convertEndianess(p, byteLength);
//        dsaParameters.g = new ModNumber(gLittleEndian);
//        p += byteLength;
//        byte[] yLittleEndian = ModNumber.convertEndianess(p, byteLength);
//        dsaParameters.y = new ModNumber(yLittleEndian);
//        p += byteLength;
//        byte[] xLittleEndian = ModNumber.convertEndianess(p, groupSize);
//        dsaParameters.x = new ModNumber(xLittleEndian);
//        }
//        }
//        #endif
//        #if (MEDMOD || LARGEMODSIGNATURE) && !UNSAFE
//        int offset = 28 + seedLength;
//        byte[] qLittleEndian = ModNumber.convertEndianess(keyBlob, groupSize, offset);
//        dsaParameters.Q = new ModNumber(qLittleEndian);
//        offset += groupSize;
//        byte[] pLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
//        dsaParameters.P = new ModNumber(pLittleEndian);
//        offset += byteLength;
//        byte[] gLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
//        dsaParameters.g = new ModNumber(gLittleEndian);
//        offset += byteLength;
//        byte[] yLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
//        dsaParameters.y = new ModNumber(yLittleEndian);
//        offset += byteLength;
//        byte[] xLittleEndian = ModNumber.convertEndianess(keyBlob, groupSize, offset);
//        dsaParameters.x = new ModNumber(xLittleEndian);
//
//        #endif
//        return dsaParameters;
//        }
    private ModNumber P;
    private ModNumber Q;
    private ModNumber g;
    private ModNumber x;
    private ModNumber y;
}
