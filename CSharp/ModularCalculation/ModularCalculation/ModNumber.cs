using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Formats.Asn1;
using System.Reflection;

namespace ModularCalculation
{
    public class ModNumber
    {
        public const int MaxMod = 4096 / 8;
        public const int NCOUNT = MaxMod + LSIZE;
        public const int LSIZE = sizeof(ulong) ;
        public const int ISIZE = sizeof(uint);
        public const int LCOUNT = NCOUNT / LSIZE;
        public const int ICOUNT = NCOUNT / ISIZE;
        public const int NSIZE = NCOUNT * 8;
        public ulong []num = new ulong[LCOUNT];
        public const int HexStringLength = NCOUNT * 2;
        public const int OctalStringLength = (NSIZE % 3 == 0) ? (NSIZE / 3) : (NSIZE / 3 + 1);
        public const int DecimalStringLength = (int)(NSIZE * 0.30102999566398119521373889472449) + 1; // log(2)
        public ModNumber(ulong n = 0)
        {
            num[0] = n;
        }
        public ModNumber(ulong[] num)
        {
            this.num = num;
        }
        public ModNumber(ModNumber m)
        {
            m.num.CopyTo(num, 0);
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
        public static ModNumber operator- (ModNumber l, ModNumber r)
        {
            ModNumber result = new ModNumber();
            if (l == r)
                return result;              // Optimization
            l.num.CopyTo(result.num, 0);
            unsafe
            {
                fixed (ulong* pNuml = &l.num[0])
                    fixed (ulong *pNumr = &r.num[0])
                        fixed(ulong *pNumresult = &result.num[0])
                        {
                            uint *pNumli = (uint *)pNuml;
                            uint* pNumri = (uint*)pNumr;
                            uint* pNumresulti = (uint*)pNumresult;
                            uint carry = 0;
                            for (int i = 0; i < ICOUNT; i++)
                            {
                                uint ltmp = pNumli[i];
                                uint rtmp = pNumri[i];
                                if (ltmp >= carry)
                                {
                                    ltmp -= carry;
                                    carry = 0;
                                }
                                else
                                    ltmp -= carry;
                                if (ltmp < rtmp)
                                    carry = 1;
                                pNumresulti[i] = ltmp - rtmp;
                            }
                    
                        }

            }
            return result;
        }

        private ModNumber AddAssignScalar(int lpos, uint scalar)
        {
            if (lpos >= ICOUNT)
                throw new ArgumentException("lpos out of range");
            ulong res = 0ul;
            unsafe
            {
                fixed(ulong *pNum = &num[0])
                {
                    uint* pNumi = (uint*)pNum;
                    res = ((ulong)pNumi[lpos]) + scalar;
                    uint* pResi = (uint*)&res;
                    pNumi[lpos++] = pResi[0];
                    while (pResi[1] > 0u && lpos < ICOUNT)
                    {
                        res = pNumi[lpos] + pResi[1];
                        pNumi[lpos++] = pResi[0];
                    }
                }
            }
            return this;
        }
        public static ModNumber operator + (ModNumber l, uint scalar)
        {
            ModNumber mres = new ModNumber(l);
            mres.AddAssignScalar(0, scalar);
            return mres;
        }
        public static ModNumber operator +(ModNumber l, ModNumber r)
        {
            ModNumber mres = new ModNumber(l);
            unsafe
            {
                fixed(ulong *pRNum = &r.num[0])
                {
                    uint *pRNumi = (uint*)pRNum;
                    for (int i = 0; i < ICOUNT; i++)
                    {
                        mres.AddAssignScalar(i, pRNumi[i]);
                    }
                }
            }
            return mres;
        }
        public static ModNumber operator*(ModNumber l, uint scalar)
        {
            ModNumber mres = new ModNumber(0ul);
            unsafe
            {
                fixed(ulong *pNum = &l.num[0])
                    fixed(ulong *pRes = &mres.num[0])
                {
                    uint *pNumi = (uint*)pNum;
                    uint *pResi = (uint*)pRes;
                    int firstNonzeroWord;
                    for (firstNonzeroWord = LCOUNT - 1; firstNonzeroWord >= 0; firstNonzeroWord--)
                    {
                        if (pNumi[firstNonzeroWord] != 0)
                            break;
                    }
                    for (int i = 0; i <= firstNonzeroWord; i++)
                    {
                        ulong tmpres = ((ulong)pNumi[i]) * scalar;
                        uint *pTmpres = (uint*)&tmpres;
                        mres.AddAssignScalar(i, pTmpres[0]);
                        if (i <  LCOUNT - 1)
                            mres.AddAssignScalar(i + 1, pTmpres[1]);
                    }

                }
            }
            return mres;
        }
        public static ModNumber operator*(ModNumber ml, ModNumber mr)
        {
            ModNumber mres = new ModNumber();
            unsafe
            {
                fixed (ulong* pr = &mr.num[0])
                {
                    uint* pri = (uint*)pr;
                    int firstNonzeroWord;
                    for (firstNonzeroWord = ICOUNT - 1; firstNonzeroWord >= 0; firstNonzeroWord--)
                    {
                        if (pri[firstNonzeroWord] != 0)
                            break;
                    }
                    for (int i = 0; i <= firstNonzeroWord; i++)
                    {
                        mres += ((ml * pri[i]) << ISIZE * 8 * i);
                    }
                }
            }
            return mres;
        }
        private int FindFirstNonZeroBitInWord(uint word)
        {
            ulong mask = 1ul << (LSIZE * 8 - 1);
            for (int i = 0; i < LSIZE * 8; i++)
            {
                if ((num[word] & mask) != 0)
                    return i;
                mask >>= 1;
            }
            return LSIZE * 8;
        }
        public static bool operator < (ModNumber l, ModNumber r)
        {
            for (int i = LCOUNT - 1; i >= 0; i--)
            {
                if (l.num[i] == r.num[i])
                    continue;
                else
                    return l.num[i] < r.num[i];
            }
            return false;
        }
        public static bool operator >(ModNumber l, ModNumber r)
        {
            for (int i = LCOUNT - 1; i >= 0; i--)
            {
                if (l.num[i] == r.num[i])
                    continue;
                else
                    return l.num[i] > r.num[i];
            }
            return false;
        }
        public static bool operator <=(ModNumber l, ModNumber r)
        {
            for (int i = LCOUNT - 1; i >= 0; i--)
            {
                if (l.num[i] == r.num[i])
                    continue;
                else
                    return l.num[i] < r.num[i];
            }
            return true;
        }
        public static bool operator >=(ModNumber l, ModNumber r)
        {
            for (int i = LCOUNT - 1; i >= 0; i--)
            {
                if (l.num[i] == r.num[i])
                    continue;
                else
                    return l.num[i] > r.num[i];
            }
            return true;
        }

        public static ModNumber operator << (ModNumber n, int i)
        {
            ModNumber mres = new ModNumber();
            int words = 0;
            if (i >= ISIZE * 8)
            {
                if (i >= NSIZE)
                    return new ModNumber();
                words = i / (ISIZE * 8);
                i %= ISIZE * 8;
            }
            unsafe
            {
                fixed (ulong *pN = &n.num[0])
                    fixed(ulong *pRes = &mres.num[0])
                {
                    uint* piN = (uint*)pN;
                    uint* piRes = (uint*)pRes;
                    piRes[ICOUNT - 1] = piN[ICOUNT - words - 1] << i;
                    for (int j = ICOUNT - 2; j >= words; j--)
                    {
                        ulong tmp = ((ulong)(piN[j - words])) << i;
                        uint *pTmp = (uint*)&tmp;
                        piRes[j + 1] |= pTmp[1];
                        piRes[j] = pTmp[0];
                    }
                }

            }
            return mres;
        }

        public static ModNumber operator >> (ModNumber n, int i)
        {
            ModNumber mres = new ModNumber();
            int words = 0;
            if (i >= ISIZE * 8)
            {
                if (i >= NSIZE)
                {
                    return mres;
                }
                words = i / (ISIZE * 8);
                i %= ISIZE * 8;
            }
            unsafe
            {
                fixed(ulong *pN = &n.num[0])
                    fixed(ulong *pRes = &mres.num[0])
                {
                    uint* pNi = (uint*)pN;
                    uint* pResi = (uint*)pRes;
                    pResi[0] = pNi[words] >> i;
                    for (int j = 0; j < ICOUNT - words - 1; j++)
                    {
                        ulong tmp = ((ulong)pNi[j + words + 1]) << ((ISIZE * 8) - i);
                        uint* pTmpi = (uint *)&tmp;
                        pResi[j] |= pTmpi[0];
                        pResi[j + 1] = pTmpi[1];
                    }
                }
            }
            return mres;
        }

        public static (ModNumber, ModNumber) DivideAndModulo(ModNumber l, ModNumber r, bool onlyModulo)
        {
            ModNumber divRes = new ModNumber();
            ModNumber mzero = new ModNumber();
            if (r == mzero)
                throw new DivideByZeroException();
            ModNumber mone = new ModNumber(1ul);
            if (r == mone)                      // Optimization
            {
                l.num.CopyTo(divRes.num, 0);
                return (divRes, new ModNumber());
            }
            ModNumber mtwo = new ModNumber(2ul);
            ModNumber modRes = new ModNumber();
            if (r == mtwo)                      // Optimization
            {
                divRes = l >> 1;
                if ((l.num[0] & 0x1ul) > 0)
                    modRes = new ModNumber(1ul);
                return (divRes,modRes);
            }
            l.num.CopyTo(modRes.num,0);
            if (l < r)                        
            {

                return (mzero, modRes);
            }
            if (l == r)
            {
                return (mone, mzero);
            }
            uint firstNonzeroWordl = 0;
            uint firstNonzeroWordr = 0;
            for (uint i = LCOUNT - 1; i >= 0; i--)
                if (l.num[i] > 0)
                {
                    firstNonzeroWordl = i;
                    break;
                }
            for (uint i = LCOUNT - 1; i >= 0; i--)
                if (r.num[i] > 0)
                {
                    firstNonzeroWordr = i;
                    break;
                }
            uint nonZeroDifference = firstNonzeroWordl - firstNonzeroWordr;
            for (uint i = 0; i <= nonZeroDifference; i++)
            {
                ulong[] divisor = new ulong[LCOUNT];
                ulong[] rShiftedLeft = new ulong[LCOUNT];
                for (int j = 0; j <= firstNonzeroWordr; j++)
                {
                    rShiftedLeft[j + nonZeroDifference - i] = r.num[j];
                }
                divisor[nonZeroDifference - i] = 1ul;
                ModNumber mRShiftedLeft = new ModNumber(rShiftedLeft);
                ModNumber mDivisor = new ModNumber(divisor);
                int firstBitl = (int)((LSIZE * 8 - modRes.FindFirstNonZeroBitInWord(firstNonzeroWordl)) + (LSIZE * 8 * firstNonzeroWordl));
                int firstBitr = (int)((LSIZE * 8 - mRShiftedLeft.FindFirstNonZeroBitInWord(nonZeroDifference + firstNonzeroWordr - i)) + (LSIZE * 8 * (nonZeroDifference + firstNonzeroWordr - i)));
                int firstBitDifference = firstBitl - firstBitr;
                for (int j = 0; j <= firstBitDifference; j++)
                {
                    ModNumber rShiftedLeftBits = new ModNumber(mRShiftedLeft);
                    ModNumber mDivisorShiftedLeftBits = new ModNumber(mDivisor);
                    rShiftedLeftBits <<= firstBitDifference - j;
                    if (!onlyModulo)
                        mDivisorShiftedLeftBits <<= firstBitDifference - j;
                    while (modRes >= rShiftedLeftBits )
                    {
                        modRes -= rShiftedLeftBits;
                        if (!onlyModulo) 
                            divRes += mDivisorShiftedLeftBits;
                    }
                }
            }
            return (divRes, modRes);
        }
        public static ModNumber operator %(ModNumber ml, ModNumber mr)
        {
            (ModNumber div, ModNumber mod) = DivideAndModulo(ml, mr, true);
            return mod;
        }
        public static string AdjustStringLength(string s, int desiredLength)
        {
            if (desiredLength < s.Length)
            {
                throw new ArgumentException("Value to large");
            }
            if (desiredLength > s.Length)
            {
                string tmp = new string('0', desiredLength - s.Length);
                s = tmp + s;
            }
            return s;
        }
        public static ModNumber StomnHexBase(string s)
        {
            s = AdjustStringLength(s, HexStringLength);
            ulong[] n = new ulong[LCOUNT];
            for (int i = 0; i < HexStringLength; i += LSIZE*2)
            {
                string tmp = s.Substring(i, LSIZE * 2);
                ulong.TryParse(tmp, System.Globalization.NumberStyles.HexNumber, null, out n[LCOUNT - (i / (LSIZE * 2)) - 1]);
            }
            ModNumber mres = new ModNumber(n);
            return mres;
        }
        public static ModNumber StomnOctalBase(string s)
        {
            ModNumber mres = new ModNumber();
            return mres;

        }
        public static ModNumber StomnDecimalBase(string s)
        {
            ModNumber mres = new ModNumber();
            return mres;

        }

        public static ModNumber Stomn(string s, int digitBase)
        {
            int i;
            for (i = 0; i < s.Length; i++)
            {
                if (!char.IsWhiteSpace(s[i]))
                    break;
            }
            if (i > 0)
                s = s.Substring(i);
            if (s[0] == '-')
                throw new ArgumentException("Only positive numbers allowed");
            switch(digitBase)
            {
                case 8:
                    return StomnOctalBase(s);
                case 10: 
                    return StomnDecimalBase(s);
                case 16:
                    return StomnHexBase(s);
            }
            throw new ArgumentException("Only base 8, 10 and 16 are valid");
        }
    }
}