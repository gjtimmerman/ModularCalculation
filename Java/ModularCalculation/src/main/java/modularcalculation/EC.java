package modularcalculation;

public class EC
{
    public EC(MultGroupMod mgm, ECPoint g, ModNumber n, ModNumber a, ModNumber b)
    {
        this.mgm = mgm;
        this.g = g;
        this.n = n;
        this.a = a;
        this.b = b;
        this.nLen = n.getByteCount();
    }
    public boolean IsOnCurve(ECPoint p)
    {
        if (p.IsAtInfinity)
            return true;
        ModNumber yKwad = mgm.Kwad(p.y);
        return yKwad.equals(CalculateRhs(p.x));
    }
    public ModNumber CalculateRhs(ModNumber x)
    {
        ModNumber xPower3 = mgm.Exp(x, new ModNumber(3L));
        ModNumber aTimesX = mgm.Mult(x,a);
        ModNumber rhs = mgm.Add(mgm.Add(xPower3, aTimesX), b);
        return rhs;
    }
    public ModNumber CalculateY(ModNumber x)
    {
        return CalculateRhs(x).sqrt();
    }
    public ECPoint Times2(ECPoint p)
    {
        if (!IsOnCurve(p))
            throw new IllegalArgumentException("The point is not on the curve!");
        if (p.IsAtInfinity)
            return new ECPoint(p);
        ECPoint result = new ECPoint();
        ModNumber mzero = new ModNumber(0L);
        if (p.y.equals(mzero))
        {
            result.IsAtInfinity = true;
            return result;
        }
        ModNumber xpSquared = mgm.Kwad(p.x);
        ModNumber xpSquaredTimes3 = mgm.Mult(xpSquared, new ModNumber(3L));
        ModNumber xpSquaredTimes3PlusA = mgm.Add(xpSquaredTimes3, a);
        ModNumber ypTimes2 = mgm.Mult(p.y, new ModNumber(2L));
        ModNumber ypTimes2Inverse = mgm.Inverse(ypTimes2);
        ModNumber lambda = mgm.Mult(xpSquaredTimes3PlusA, ypTimes2Inverse);
        ModNumber xpTimes2 = mgm.Mult(p.x, new ModNumber(2L));
        ModNumber lambdaSquaredMinusXpTimes2 = mgm.Diff(mgm.Kwad(lambda), xpTimes2);
        ModNumber xrMinusXp = mgm.Diff(lambdaSquaredMinusXpTimes2, p.x);
        ModNumber lambdaTimesXrMinusXp = mgm.Mult(lambda, xrMinusXp);
        ModNumber yPPlusLambdaTimesXrMinusXp = mgm.Add(p.y, lambdaTimesXrMinusXp);
        result.x = lambdaSquaredMinusXpTimes2;
        result.y = mgm.Diff(mzero, yPPlusLambdaTimesXrMinusXp);
        return result;

    }
    public ECPoint Add(ECPoint p, ECPoint q)
    {
        if (!IsOnCurve(p) || !IsOnCurve(q))
            throw new IllegalArgumentException("One of the points is not on the curve!");
        ECPoint result = new ECPoint();
        if (p.equals(q))
            return Times2(p);
        if (p.IsAtInfinity)
            return new ECPoint(q);
        if (q.IsAtInfinity)
            return new ECPoint(p);
        if (p.x.equals(q.x))
        {
            result.IsAtInfinity = true;
            return result;
        }
        ModNumber mzero = new ModNumber(0L);
        ModNumber xDelta = mgm.Diff(q.x, p.x);
        ModNumber yDelta = mgm.Diff(q.y, p.y);
        ModNumber xDeltaInverse = mgm.Inverse(xDelta);
        ModNumber lambda = mgm.Mult(yDelta,xDeltaInverse);
        result.x = mgm.Diff(mgm.Diff(mgm.Kwad(lambda), p.x), q.x);
        result.y = mgm.Diff(mzero, mgm.Add(p.y, mgm.Mult(lambda, mgm.Diff(result.x, p.x))));

        return result;
    }
    public ECPoint Mult(ECPoint p, ModNumber mn)
    {
        if (!IsOnCurve(p))
            throw new IllegalArgumentException("The point is not on the curve!");
        if (p.IsAtInfinity)
            return new ECPoint(p);
        ModNumber n = new ModNumber(mn);
        ECPoint pCopy = new ECPoint(p);
        ModNumber mzero = new ModNumber(0L);
        ECPoint result = new ECPoint();
        result.IsAtInfinity = true;
        long mask = 0x01L;
        while (ModNumber.greaterThan(n, mzero))
        {
            long first = n.num[0];
            if ((first & mask) != 0)
            {
                result = Add(result, pCopy);
            }
            pCopy = Times2(pCopy);
            ModNumber.shiftRightAssign(n, 1);
        }
        return result;
    }
    public MultGroupMod mgm;
    public ECPoint g;
    public ModNumber n;
    public ModNumber a;
    public ModNumber b;
    public int nLen;

}
