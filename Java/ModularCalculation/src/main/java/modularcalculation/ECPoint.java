package modularcalculation;

public class ECPoint
{
    public ModNumber x;
    public ModNumber y;
    public boolean IsAtInfinity;
    public ECPoint()
    {

    }
    public ECPoint(ECPoint p) {
            this.IsAtInfinity = p.IsAtInfinity;
            if (!IsAtInfinity) {
                this.x = new ModNumber(p.x);
                this.y = new ModNumber(p.y);
            }
            else {
                this.x = null;
               this.y = null;
            }
    }
    @Override
    public boolean equals( Object obj)
    {
        if (obj instanceof ECPoint ecPointObj)
        {
            if (this.IsAtInfinity && ecPointObj.IsAtInfinity)
                return true;
            if (this.IsAtInfinity || ecPointObj.IsAtInfinity)
                return false;
            return this.x.equals(ecPointObj.x) && this.y.equals(ecPointObj.y);

        }
        else
            return false;
    }
    @Override
    public int hashCode()
        {
            if (IsAtInfinity)
                return Boolean.valueOf(IsAtInfinity).hashCode();
            else
                return x.hashCode() + y.hashCode();
        }

}

