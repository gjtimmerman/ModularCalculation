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
    public ModNumber encrypt(ModNumber m)
    {
        ModNumber masked = m.getPKCS1Mask(false, ModNumber.MaxMod);
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
        ModNumber maskedResult = unmaskedResult.getPKCS1Mask(true, ModNumber.MaxMod);
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
        List<Object> result = removedMask.ParseBERASNString();
        byte [] hashBigEndian = (byte [])result.get(1);
        byte[] hashLittleEndian;

        hashLittleEndian = ModNumber.convertEndianess(hashBigEndian);


        ModNumber retval = new ModNumber(hashLittleEndian);
        return retval;

    }
//    public static RSAParameters GetRSAKey(String keyName, boolean createIfNotExists, CngKeyUsages usage = CngKeyUsages.Decryption)
//    {
//        RSAParameters rsaParameters;
//        CngKey cngKey;
//        if (CngKey.Exists(keyName))
//            cngKey = CngKey.Open(keyName);
//        else
//        {
//            if (createIfNotExists)
//            {
//                CngKeyCreationParameters parametersEncryptionRsa = new CngKeyCreationParameters();
//                parametersEncryptionRsa.KeyUsage = CngKeyUsages.Decryption;
//                byte[] keyLen = new byte[4];
//                keyLen[0] = (ModNumber.MaxMod * 8) % 0x100;
//                keyLen[1] = (ModNumber.MaxMod * 8) / 0x100;
//                CngPropertyOptions propOptions = new CngPropertyOptions();
//
//                CngProperty prop = new CngProperty("Length", keyLen, propOptions);
//                parametersEncryptionRsa.Parameters.Add(prop);
//                byte[] exportp = new byte[4];
//                exportp[0] = 0x03;
//                prop = new CngProperty("Export Policy", exportp, propOptions);
//                parametersEncryptionRsa.Parameters.Add(prop);
//
//                cngKey = CngKey.Create(CngAlgorithm.Rsa, keyName, parametersEncryptionRsa);
//            }
//            else
//                throw new ApplicationException("Key does not exist!");
//        }
//        CngPropertyOptions options = new CngPropertyOptions();
//        CngProperty property = cngKey.GetProperty("Length", options);
//        byte [] ?length = property.GetValue();
//        int ?keyLength = (int?)(length?[1] * 0x100);
//        keyLength += length?[0];
//        if (keyLength > ModNumber.MaxMod * 8)
//            throw new ApplicationException("Keylength not less or equal to MAXMOD!");
//        byte[] keyBlob = cngKey.Export(new CngKeyBlobFormat("RSAFULLPRIVATEBLOB"));
//        if (keyBlob[0] != 0x52 && keyBlob[1] != 0x53 && keyBlob[2] != 0x41 && keyBlob[3] != 0x33)
//            throw new ApplicationException("Key structure not of type RSA Full Private!");
//        int bitLength = keyBlob[5] * 0x100;
//        bitLength += keyBlob[4];
//        if (bitLength > ModNumber.MaxMod * 8)
//            throw new ApplicationException("Keylength not less or equal to MAXMOD!");
//        int cbPubExp = keyBlob[9] * 0x100;
//        cbPubExp += keyBlob[8];
//        int cbModulus = keyBlob[13] * 0x100;
//        cbModulus += keyBlob[12];
//        int cbPrime1 = keyBlob[17] * 0x100;
//        cbPrime1 += keyBlob[16];
//        int cbPrime2 = keyBlob[21] * 0x100;
//        cbPrime2 += keyBlob[20];
//#if UNSAFE
//            unsafe
//        {
//            fixed(byte *pData = &keyBlob[24])
//            {
//                byte* p = pData;
//                byte[] pubExpLittleEndian = ModNumber.convertEndianess(p, cbPubExp);
//                rsaParameters.PubExp = new ModNumber(pubExpLittleEndian);
//                p += cbPubExp;
//                byte[] modulusLittleEndian = ModNumber.convertEndianess(p, cbModulus);
//                rsaParameters.Modulus = new ModNumber(modulusLittleEndian);
//                p += cbModulus;
//                byte[] prime1LittleEndian = ModNumber.convertEndianess(p, cbPrime1);
//                rsaParameters.Prime1 = new ModNumber(prime1LittleEndian);
//                p += cbPrime1;
//                byte[] prime2LittleEndian = ModNumber.convertEndianess(p, cbPrime2);
//                rsaParameters.Prime2 = new ModNumber( prime2LittleEndian);
//                p += cbPrime2;
//                byte[] exp1LittleEndian = ModNumber.convertEndianess(p, cbPrime1);
//                rsaParameters.Exp1 = new ModNumber(exp1LittleEndian);
//                p += cbPrime1;
//                byte[] exp2LittleEndian = ModNumber.convertEndianess(p, cbPrime2);
//                rsaParameters.Exp2 = new ModNumber( exp2LittleEndian);
//                p += cbPrime2;
//                byte[] coefficientLittleEndian = ModNumber.convertEndianess(p, cbPrime1);
//                rsaParameters.Coefficient = new ModNumber(coefficientLittleEndian);
//                p += cbPrime1;
//                byte[] privExpLittleEndian = ModNumber.convertEndianess(p, cbModulus);
//                rsaParameters.PrivExp = new ModNumber(privExpLittleEndian);
//            }
//        }
//#else
//        int offset = 24;
//        byte[] pubExpLittleEndian = ModNumber.convertEndianess(keyBlob, cbPubExp, offset);
//        rsaParameters.PubExp = new ModNumber(pubExpLittleEndian);
//        offset += cbPubExp;
//        byte[] modulusLittleEndian = ModNumber.convertEndianess(keyBlob, cbModulus, offset);
//        rsaParameters.Modulus = new ModNumber(modulusLittleEndian);
//        offset += cbModulus;
//        byte[] prime1LittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime1, offset);
//        rsaParameters.Prime1 = new ModNumber(prime1LittleEndian);
//        offset += cbPrime1;
//        byte[] prime2LittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime2, offset);
//        rsaParameters.Prime2 = new ModNumber(prime2LittleEndian);
//        offset += cbPrime2;
//        byte[] exp1LittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime1, offset);
//        rsaParameters.Exp1 = new ModNumber(exp1LittleEndian);
//        offset += cbPrime1;
//        byte[] exp2LittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime2, offset);
//        rsaParameters.Exp2 = new ModNumber(exp2LittleEndian);
//        offset += cbPrime2;
//        byte[] coefficientLittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime1, offset);
//        rsaParameters.Coefficient = new ModNumber(coefficientLittleEndian);
//        offset += cbPrime1;
//        byte[] privExpLittleEndian = ModNumber.convertEndianess(keyBlob, cbModulus, offset);
//        rsaParameters.PrivExp = new ModNumber(privExpLittleEndian);
//
//#endif
//        return rsaParameters;
//    }
    private ModNumber PubExp;
    private ModNumber Modulus;
    private ModNumber Prime1;
    private ModNumber Prime2;
    private ModNumber Exp1;          // DP
    private ModNumber Exp2;          // DQ
    private ModNumber Coefficient;   // InverseQ
    private ModNumber PrivExp;

}
