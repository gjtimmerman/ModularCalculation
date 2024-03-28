package modularcalculation;

import java.util.ArrayList;
import java.util.List;

public class MultGroupMod
{
    public MultGroupMod(ModNumber n)
    {
        ModNumber modzero = new ModNumber(0L);
        if (n.equals(modzero))
            throw new IllegalArgumentException("Modulus cannot be zero!");
        ModNumber modone = new ModNumber(1L);
        if (n.equals(modone))
            throw new IllegalArgumentException("Modulus cannot be one!");
        n.checkMax(ModNumber.COUNTMOD);
        this.n = new ModNumber(n);
    }
    private ModNumber n;
    public ModNumber Mult(ModNumber l, ModNumber r)
    {
        ModNumber res = new ModNumber(0L);
        ModNumber lMod = ModNumber.modulo(l , n);
        ModNumber rMod = ModNumber.modulo(r,  n);

        int[] rModInt = rMod.toIntArray();
        int limit;
        for (limit = ModNumber.ICOUNT - 1; limit >= 0; limit--)
        {
            if (rModInt[limit] != 0)
                break;
        }
        for (int i = 0; i <= limit; i++)
        {
            ModNumber tmp = ModNumber.productScalar(lMod, rModInt[i]);
            for (int j = 0; j < i; j++)
            {
                ModNumber.moduloAssign(tmp, n);
                ModNumber.shiftLeftAssign(tmp, ModNumber.ISIZE * 8);
            }
            ModNumber.moduloAssign(tmp, n);
            ModNumber.addAssign(res, tmp);
        }

        ModNumber.moduloAssign(res, n);
        return res;
    }
    public ModNumber Kwad(ModNumber x)
    {
        ModNumber l = x;
        ModNumber r = x;
        return Mult(l, r);
    }
    public ModNumber Exp(ModNumber x, ModNumber e)
    {
        ModNumber res = new ModNumber(1L);
        ModNumber xMod = ModNumber.modulo(x, n);
        int top;
        for (top = ModNumber.LCOUNT - 1; top >= 0; top--)
            if (e.num[top] != 0)
                break;
        for (int i = 0; i <= top; i++)
        {
            long mask = 1L;
            long tmp = e.num[i];
            for (int j = 0; j < ModNumber.LSIZE * 8 && (tmp != 0L || i < top); j++)
            {
                if ((tmp & mask) != 0)
                    res = Mult(res, xMod);
                xMod = Kwad(xMod);
                tmp >>>= 1;
            }
        }
        return res;
    }
    public ModNumber Add (ModNumber l, ModNumber r)
    {
        ModNumber lMod = ModNumber.modulo(l, n);
        ModNumber rMod = ModNumber.modulo(r, n);
        return ModNumber.modulo(ModNumber.add(lMod, rMod), n);
    }
    public ModNumber Diff(ModNumber l, ModNumber r)
    {
        ModNumber lMod = ModNumber.modulo(l,  n);
        ModNumber rMod = ModNumber.modulo(r , n);
        if (lMod.equals(rMod))
            return new ModNumber(0L);
        if (ModNumber.greaterThan(lMod,  rMod))
            return ModNumber.subtract(lMod , rMod);
        else
            return ModNumber.add(ModNumber.subtract(n , rMod) , lMod);
    }
    public ModNumber Inverse (ModNumber mx)
    {
        ModNumber mzero = new ModNumber(0L);
        ModNumber mone = new ModNumber(1L);
        if (mx.equals(mzero))
            throw new IllegalArgumentException("Zero does not have an inverse.");
        if (mx.equals(mone))
            return mone;
        if (mx.equals(n))
            throw new IllegalArgumentException("Zero does not have an inverse.");
        ModNumber r = ModNumber.modulo(mx , n);
        ModNumber l = new ModNumber(n);
        ArrayList<ModNumber> divisors = new ArrayList<ModNumber>();
        DivideAndModuloResult divideAndModuloResult = ModNumber.divideAndModulo(l, r, false);
        while(!(divideAndModuloResult.mod().equals(mone)))
        {
            if (divideAndModuloResult.mod().equals(mzero))
                throw new IllegalArgumentException("Numbers are not relative prime, so there is no inverse");
            divisors.add(divideAndModuloResult.div());
            l = r;
            r = divideAndModuloResult.mod();
            divideAndModuloResult = ModNumber.divideAndModulo(l, r, false);
        }
        divisors.add(divideAndModuloResult.div());
        ModNumber tmp1 = mzero;
        ModNumber tmp2 = mone;
        List<ModNumber> divisorsReversed = new ArrayList<>();
        for (int i = divisors.size() - 1; i >= 0; i--)
        {
            divisorsReversed.add(divisors.get(i));
        }
        for(ModNumber it : divisorsReversed)
        {
            ModNumber tmp = tmp2;
            ModNumber product = Mult(tmp2, it);
            tmp2 = Diff(tmp1, product);
            tmp1 = tmp;
        }
        return tmp2;
    }
}
