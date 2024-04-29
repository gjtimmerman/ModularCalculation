package modularcalculation;

import java.util.Random;

public class ECKeyPair
{
    public ECKeyPair(EC ec, ModNumber mx, ECPoint y)
    {
        this.ec = ec;
        if (mx != null)
        {
            this.mx = new ModNumber(mx);
        }
        else
            this.mx = new ModNumber(0L);
        ModNumber mzero = new ModNumber(0L);
        if (this.mx == mzero)
        {
            long[] x = new long[ModNumber.LCOUNT];
            int lSize = ec.nLen / ModNumber.LSIZE;
            Random random = new Random();
            ModNumber mxTmp;
            for (int i = 0; i < lSize; i++)
            {
                x[i] = random.nextLong();
            }
            mxTmp = new ModNumber(x);
            if (mxTmp == mzero)
                ModNumber.addAssignScalar(mxTmp, random.nextInt() + 1);
            while (ModNumber.greaterThanOrEqual(mxTmp, ec.n))
            {
                mxTmp.num[lSize - 1] -= random.nextLong();
            }
            this.mx = mxTmp;
        }
        if (y == null)
            this.y = CalcPublicKey(this.mx);
        else
            this.y = new ECPoint(y);

    }
    public ECPoint CalcPublicKey(ModNumber x)
    {
        ECPoint retval = ec.Mult(ec.g, x);
        return retval;
    }
 /*   public ECDsaCng ImportECKeyPair(string curveName = "secP256k1")
    {
        ECDsaCng ecDsaCng = new ECDsaCng();
        ECParameters ecParameters = new ECParameters();
        ecParameters.Curve = ECCurve.CreateFromFriendlyName(curveName);
        byte [] privateKeyBigEndian = mx.convertEndianess((int)ec.nLen);
        System.Security.Cryptography.ECPoint ecPoint = new System.Security.Cryptography.ECPoint();
        ecPoint.X = y.x!.convertEndianess((int)ec.nLen);
        ecPoint.Y = y.y!.convertEndianess((int)ec.nLen);
        ecParameters.Q = ecPoint;
        ecParameters.D = privateKeyBigEndian;
        ecDsaCng.ImportParameters(ecParameters);
        return ecDsaCng;
    }

  */
    public static ModNumber CalculateECDHSharedSecret(ECKeyPair pair1, ECKeyPair pair2)
    {
        ECPoint resultPt = pair1.ec.Mult(pair1.y, pair2.mx);
        if (resultPt.IsAtInfinity)
            throw new IllegalArgumentException("Invalid keypair combination");
        return resultPt.x;
    }
/*
    public static byte[] GetSecretECDHAgreement(ECKeyPair pair1, ECKeyPair pair2, String curveName, byte[] magicNr )
    {
        ECDiffieHellmanCng ecDiffieHellmanCng1 = new ECDiffieHellmanCng();
        ecDiffieHellmanCng1.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
        ECParameters ecParameters1 = new ECParameters();
        ecParameters1.Curve = ECCurve.CreateFromFriendlyName(curveName);
        byte[] privateKey1BigEndian = pair1.mx.convertEndianess((int)pair1.ec.nLen);
        ecParameters1.D = privateKey1BigEndian;
        byte[] publicKey1XBigEndian = pair1.y.x!.convertEndianess((int)pair1.ec.nLen);
        byte[] publicKey1YBigEndian = pair1.y.y!.convertEndianess((int)pair1.ec.nLen);
        ecParameters1.Q.X = publicKey1XBigEndian;
        ecParameters1.Q.Y = publicKey1YBigEndian;
        ecDiffieHellmanCng1.ImportParameters(ecParameters1);
        ECParameters ecParameters2 = new ECParameters();
        ecParameters2.Curve = ECCurve.CreateFromFriendlyName(curveName);
        byte[] publicKeyBlob = new byte[pair2.ec.nLen * 2 + 8];
        for (int i = 0; i < 4; i++)
            publicKeyBlob[i] = magicNr[i];
        publicKeyBlob[4] = (byte)(pair1.ec.nLen % 0x100);
        publicKeyBlob[5] = (byte)(pair1.ec.nLen / 0x100);
        byte[] publicKey2XBigEndian = pair2.y.x!.convertEndianess((int)pair2.ec.nLen);
        byte[] publicKey2YBigEndian = pair2.y.y!.convertEndianess((int)pair2.ec.nLen);
        publicKey2XBigEndian.CopyTo(publicKeyBlob, 8);
        publicKey2YBigEndian.CopyTo(publicKeyBlob, pair2.ec.nLen + 8);
        ECDiffieHellmanPublicKey ecDiffieHellmanPublicKey = ECDiffieHellmanCngPublicKey.FromByteArray(publicKeyBlob,CngKeyBlobFormat.EccPublicBlob);
        byte [] sharedSecret =  ecDiffieHellmanCng1.DeriveKeyFromHash(ecDiffieHellmanPublicKey, HashAlgorithmName.SHA384, null, null);

        return sharedSecret;
    }
 */
    public EC ec;
    public ECPoint y;
    public ModNumber mx;
}

