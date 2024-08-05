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
        if (this.mx.equals(mzero))
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

    public static ModNumber CalculateECDHSharedSecret(ECKeyPair pair1, ECKeyPair pair2)
    {
        ECPoint resultPt = pair1.ec.Mult(pair1.y, pair2.mx);
        if (resultPt.IsAtInfinity)
            throw new IllegalArgumentException("Invalid keypair combination");
        return resultPt.x;
    }

    public EC ec;
    public ECPoint y;
    public ModNumber mx;
}

