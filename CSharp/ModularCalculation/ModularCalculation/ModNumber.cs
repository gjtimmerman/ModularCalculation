using System.Data.Common;
using System.Formats.Asn1;

namespace ModularCalculation
{
    public class ModNumber
    {
        public const int MaxMod = 4096 / 8;
        public const int LSIZE = sizeof(ulong) ;
        public const int ISIZE = sizeof(uint);
        public const int LCOUNT = MaxMod / LSIZE;
        public const int ICOUNT = MaxMod / ISIZE;
        public ulong []num = new ulong[LCOUNT];
        public ModNumber(ulong n = 0)
        {
            num[0] = n;
        }
        public ModNumber(ulong[] num)
        {
            this.num = num;
        }
        public override bool Equals(object ?other)
        {
            if (other is ModNumber om)
            {
                for (int i = 0; i < om.num.Length; i++)
                {
                    if (num[i] != om.num[i])
                        return false;
                }
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return num.GetHashCode();
        }
        public static bool operator== (ModNumber om1, ModNumber om2)
        {
            return om1.Equals(om2);
        }
        public static bool operator !=(ModNumber om1, ModNumber om2)
        {
            return !(om1.Equals(om2));
        }
        public static ModNumber operator- (ModNumber l, uint r)
        {
            ModNumber result = new ModNumber ();
            l.num.CopyTo(result.num, 0);
            unsafe
            {
                fixed(ulong* pNuml = &result.num[0])
                {
                    uint *pNumi = (uint *)pNuml;
                    uint carry = 0;
                    uint ltmp = pNumi[0];
                    if (ltmp < r)
                        carry = 1;
                    pNumi[0] = ltmp - r;
                    for (int i = 1; carry > 0 && i < ICOUNT; i++)
                    {
                        if (carry <= pNumi[i])
                        {
                            pNumi[i] -= carry;
                            carry = 0;
                        }
                        else
                            pNumi[i] -= carry;
                            
                    }

                }
            }
            return result;
        }
    }
}