﻿using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Formats.Asn1;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography;
using System.Text;
using static System.Formats.Asn1.AsnWriter;


namespace ModularCalculation
{
    public enum ASNElementType : byte
    {
        INTEGER_VALUE = 0x02,
        OCTET_STRING = 0x04,
        NULL_VALUE = 0x05,
        OBJECT_IDENTIFIER = 0x06,
        SEQUENCE = 0x10
    }

    public class ModNumber
    {
#if LARGEMOD
        public const int MaxMod = 4096 / 8;

#elif LARGEMODSIGNATURE
        public const int MaxMod = 3072 / 8;

#elif MEDMOD
        public const int MaxMod = 2048 / 8;

#elif SMALLMOD
        public const int MaxMod = 1024 / 8;
#endif
        public const int NCOUNT = MaxMod + LSIZE;
        public const int COUNTMOD = MaxMod / LSIZE;
        public const int LSIZE = sizeof(ulong);
        public const int ISIZE = sizeof(uint);
        public const int LCOUNT = NCOUNT / LSIZE;
        public const int ICOUNT = NCOUNT / ISIZE;
        public const int NSIZE = NCOUNT * LSIZE;
        public ulong[] num = new ulong[LCOUNT];
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
        public ModNumber(byte[] n)
        {
#if UNSAFE
            unsafe
            {
                fixed (ulong* pN = &num[0])
                {
                    byte* pByte = (byte*)pN;
                    for (int i = 0; i < n.Length; i++)
                    {
                        pByte[i] = n[i];
                    }
                }
            }
#else

            int lCount = n.Length / LSIZE;
            ulong tmp;
            for (int i = 0; i < lCount; i++)
            {
                tmp = 0ul;
                for (int j = 0; j < LSIZE; j++)
                {
                    tmp |= ((ulong)n[i * LSIZE + j] << j * 8);
                }
                num[i] = tmp;
            }
            int lSize = n.Length % LSIZE;
            if (lSize == 0)
                return;
            tmp = 0ul;
            for (int j = 0; j < lSize; j++)
            {
                tmp |= ((ulong)n[lCount * LSIZE + j] << j * 8);
            }
            num[lCount] = tmp;
#endif
        }

#if UNSAFE
        unsafe public ModNumber(byte* pNumber, int len)
        {
            fixed (ulong* p = num)
            {
                byte* pB = (byte*)p;
                for (int i = 0; i < len; i++)
                    pB[i] = pNumber[i];
            }


        }
#else
        public ModNumber (ulong [] num, int byteLen)
        {
            int lCount = byteLen / LSIZE;
            for (int i = 0; i < lCount; i++)
                this.num[i] = num[i];
            int lSize = byteLen % LSIZE;
            if (lSize == 0)
                return;
            ulong tmp = 0ul;
            ulong mask = 0xff;
            for (int j = 0; j < lSize; j++)
            {
                ulong masked = num[lCount] & mask;
                tmp |= masked;
                mask <<= 8;
            }
            this.num[lCount] = tmp;
        }
#endif
        public override bool Equals(object? other)
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
        public static bool operator ==(ModNumber om1, ModNumber om2)
        {
            return om1.Equals(om2);
        }
        public static bool operator !=(ModNumber om1, ModNumber om2)
        {
            return !(om1.Equals(om2));
        }
        internal void CheckMax(int size)
        {
            for (int i = size; i < ModNumber.LCOUNT; i++)
                if (num[i] != 0ul)
                    throw new ArgumentException("Modulus is too large!");
        }

#if UNSAFE
        unsafe public static byte[] convertEndianess(byte * b, int cb)
        {
            byte[] res = new byte[cb];
            for (int i = 0; (i < cb); i++)
            {
                res[i] = b[cb - i - 1];
            }
            return res;
        }
#else
        public static byte[] convertEndianess(byte[] b, int cb, int start = 0)
        {
            byte[] res = new byte[cb];
            for (int i = 0; i < cb; i++)
            {
                res[i] = b[(cb+start) - i - 1];
            }
            return res;
        }

#endif
        public static byte[] convertEndianess(byte[] b)
        {
            byte[] res = new byte[b.Length];
            for (int i = 0; i < b.Length; i++)
                res[i] = b[b.Length - i - 1];
            return res;
        }
        public byte[] convertEndianess(int cb = 0)
        {
#if UNSAFE
            unsafe
            {
                fixed(ulong *p = &num[0])
                {
                    byte* pB = (byte*)p;
                    int n = cb > 0 ? cb : (int)GetByteCount();
                    return convertEndianess(pB, n);
                }
            }
#else
            int n = cb > 0 ? cb : (int)GetByteCount();
            byte[] res = new byte[n];
            int lCount = n / LSIZE;
            int lSize = n % LSIZE;
            ulong mask;
            for (int i = 0; i < lCount; i++)
            {
                mask = 0xff;
                for(int j = 0; j < LSIZE; j++)
                {
                    ulong tmp = num[i] & mask;
                    res [n - (i * LSIZE + j) - 1] = (byte)(tmp >> j * 8);
                    mask <<= 8;
                }
            }
            mask = 0xff;
            for (int j = 0; j < lSize; j++)
            {
                ulong tmp = num[lCount] & mask;
                res[n - (lCount * LSIZE + j) - 1] = (byte)(tmp >> j * 8);
                mask <<= 8;
            }
            return res;
#endif
        }

#if !UNSAFE
        public uint[] ToUintArray()
        {
            uint []result = new uint[ICOUNT];
            ulong lomask = 0xffffffff;
            ulong himask = lomask << ISIZE * 8;
            for (int i = 0; i < LCOUNT; i++)
            {
                result[i * 2] = (uint)(num[i] & lomask);
                result[i * 2 + 1] = (uint)((num[i] & himask) >> ISIZE * 8);
            }
            return result;
        }
        public void FromUintArray(uint[] arr)
        {
            for (int i = 0; i < LCOUNT; i++)
            {
                num[i] = arr[i * 2];
                num[i] |= ((ulong)arr[i * 2 + 1]) << ISIZE * 8;
            }

        }
        public byte[] ToByteArray()
        {
            byte[] result = new byte[num.Length*LSIZE];
            for (int i = 0; i < num.Length; i++)
            {
                ulong byteMask = 0xfful;
                for (int j = 0; j < LSIZE; j++)
                {
                    byte bValue = (byte)((num[i] & byteMask) >> j * 8);
                    result[i * LSIZE + j] = bValue;
                    byteMask <<= 8;
                }
            }
            return result;
        }
        public char[] ToCharArray()
        {
            char[] result = new char[num.Length * LSIZE/sizeof(char)];
            for (int i = 0; i < num.Length; i++)
            {
                ulong charMask = 0xfffful;
                for (int j = 0; j < LSIZE/sizeof(char); j++)
                {
                    char cValue = (char)((num[i] & charMask) >> j * sizeof(char) * 8);
                    result[i * LSIZE/sizeof(char) + j] = cValue;
                    charMask <<= 8 * sizeof(char);
                }
            }
            return result;
        }
        public static byte[] HexStringToByteArray(string str)
        {
            if (str.Length % 2 != 0)
                throw new ArgumentException("String is not a valid Hex representation");
            byte[] result = new byte[str.Length / 2];
            for (int i = 0; i < str.Length; i+=2)
            {
                string tmp = str.Substring(i, 2);
                int resultInt;
                bool ret = int.TryParse(tmp, System.Globalization.NumberStyles.HexNumber, null, out resultInt);
                if (!ret)
                    throw new ArgumentException("String is not a valid Hex representation");
                int index = str.Length / 2 - i / 2 - 1;
                result[str.Length/2 - i/2 - 1] = (byte)resultInt;
            }
            return result;
        }
        public ModNumber (uint[] arr)
        {
            FromUintArray(arr);
        }
        public static uint[] ShiftUintArrayR(uint[] arr, int bitCount=1)
        {
            if (bitCount == 0)
                return arr;
            for (int i = 0; i < bitCount; i++)
            {
                arr[0] >>= 1;
                if ((arr[1] & 0x01) != 0)
                    arr[0] |= 0x80000000;
                arr[1] >>= 1;
            }
            return arr;
        }
        public static uint[] ShiftUintArrayL(uint[] arr, int bitCount = 1)
        {
            if (bitCount == 0)
                return arr;
            for (int i = 0; i < bitCount; i++)
            {
                arr[1] <<= 1;
                if ((arr[0] & 0x80000000) != 0)
                    arr[1] |= 0x01;
                arr[0] <<= 1;
            }
            return arr;
        }

#endif
        public static ModNumber operator -(ModNumber l, uint r)
        {
#if UNSAFE
            ModNumber result = new ModNumber();
            l.num.CopyTo(result.num, 0);
            unsafe
            {
                fixed (ulong* pNuml = &result.num[0])
                {
                    uint* pNumi = (uint*)pNuml;
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
#else
            uint[] resUint = l.ToUintArray();
            uint carry = 0;
            uint ltmp = resUint[0];
            if (ltmp < r)
                carry = 1;
            resUint[0] = ltmp - r;
            for (int i = 1; carry > 0 && i < ICOUNT; i++)
            {
                if (carry <= resUint[i])
                {
                    resUint[i] -= carry;
                    carry = 0;
                }
                else
                    resUint[i] -= carry;

            }
            ModNumber result = new ModNumber(resUint); 
#endif
        
            return result;
        }
        public static ModNumber operator -(ModNumber l, ModNumber r)
        {
            if (l == r)
                return new ModNumber();              // Optimization
#if UNSAFE
            ModNumber result = new ModNumber();
            l.num.CopyTo(result.num, 0);
            unsafe
            {
                fixed (ulong* pNuml = &l.num[0])
                fixed (ulong* pNumr = &r.num[0])
                fixed (ulong* pNumresult = &result.num[0])
                {
                    uint* pNumli = (uint*)pNuml;
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
#else
            uint[] resUint = l.ToUintArray();
            uint[] lUint = l.ToUintArray();
            uint[] rUint = r.ToUintArray();
            uint carry = 0;
            for (int i = 0; i < ICOUNT; i++)
            {
                uint ltmp = lUint[i];
                uint rtmp = rUint[i];
                if (ltmp >= carry)
                {
                    ltmp -= carry;
                    carry = 0;
                }
                else
                    ltmp -= carry;
                if (ltmp < rtmp)
                    carry = 1;
                resUint[i] = ltmp - rtmp;
            }
            ModNumber result = new ModNumber(resUint);
#endif
            return result;
        }

        private ModNumber AddAssignScalar(int lpos, uint scalar)
        {
            if (lpos >= ICOUNT)
                throw new ArgumentException("lpos out of range");
            ulong res = 0ul;
#if UNSAFE
            unsafe
            {
                fixed (ulong* pNum = &num[0])
                {
                    uint* pNumi = (uint*)pNum;
                    res = ((ulong)pNumi[lpos]) + scalar;
                    uint* pResi = (uint*)&res;
                    pNumi[lpos++] = pResi[0];
                    while (pResi[1] > 0u && lpos < ICOUNT)
                    {
                        res = (ulong)pNumi[lpos] + (ulong)pResi[1];
                        pNumi[lpos++] = pResi[0];
                    }
                }
            }
#else
            ulong lomask = 0xffffffff;
            ulong himask = lomask << ISIZE * 8;
            uint[] thisUintArray = ToUintArray();
            res = ((ulong)thisUintArray[lpos]) + scalar;
            thisUintArray[lpos++] = (uint)(res & lomask);
            while ((res & himask) > 0ul && lpos < ICOUNT)
            {
                res = thisUintArray[lpos] + ((res & himask) >> ISIZE * 8);
                thisUintArray[lpos++] = (uint)(res & lomask);
            }
            FromUintArray(thisUintArray);
#endif
            return this;
        }
        public static ModNumber operator +(ModNumber l, uint scalar)
        {
            ModNumber mres = new ModNumber(l);
            mres.AddAssignScalar(0, scalar);
            return mres;
        }
        public static ModNumber operator +(ModNumber l, ModNumber r)
        {
            ModNumber mres = new ModNumber(l);

#if UNSAFE
            unsafe
            {
                fixed (ulong* pRNum = &r.num[0])
                {
                    uint* pRNumi = (uint*)pRNum;
                    for (int i = 0; i < ICOUNT; i++)
                    {
                        mres.AddAssignScalar(i, pRNumi[i]);
                    }
                }
            }
#else
            uint[] rUint = r.ToUintArray();
            for (int i = 0; i < ICOUNT; i++)
            {
                mres.AddAssignScalar(i, rUint[i]);
            }

#endif
            return mres;
        }
        public static ModNumber operator *(ModNumber l, uint scalar)
        {
            ModNumber mres = new ModNumber(0ul);
#if UNSAFE
            unsafe
            {
                fixed (ulong* pNum = &l.num[0])
                {
                    uint* pNumi = (uint*)pNum;
                    int firstNonzeroWord;
                    for (firstNonzeroWord = ICOUNT - 1; firstNonzeroWord >= 0; firstNonzeroWord--)
                    {
                        if (pNumi[firstNonzeroWord] != 0)
                            break;
                    }
                    for (int i = 0; i <= firstNonzeroWord; i++)
                    {
                        ulong tmpres = ((ulong)pNumi[i]) * scalar;
                        uint* pTmpres = (uint*)&tmpres;
                        mres.AddAssignScalar(i, pTmpres[0]);
                        if (i < ICOUNT - 1)
                            mres.AddAssignScalar(i + 1, pTmpres[1]);
                    }

                }
            }
#else
            ulong lomask = 0xffffffff;
            ulong himask = lomask << ISIZE * 8;
            uint[] lUint = l.ToUintArray();
            int firstNonzeroWord;
            for (firstNonzeroWord = ICOUNT - 1; firstNonzeroWord >= 0; firstNonzeroWord--)
            {
                if (lUint[firstNonzeroWord] != 0)
                    break;
            }
            for (int i = 0; i <= firstNonzeroWord; i++)
            {
                ulong tmpres = ((ulong)lUint[i]) * scalar;
                mres.AddAssignScalar(i, (uint)(tmpres & lomask));
                if (i < ICOUNT - 1)
                    mres.AddAssignScalar(i + 1, (uint)((tmpres & himask) >> ISIZE * 8));
            }

#endif
            return mres;
        }
        public static ModNumber operator *(ModNumber ml, ModNumber mr)
        {
            ModNumber mres = new ModNumber();
#if UNSAFE
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
#else
            uint[] rUint = mr.ToUintArray();
            int firstNonzeroWord;
            for (firstNonzeroWord = ICOUNT - 1; firstNonzeroWord >= 0; firstNonzeroWord--)
            {
                if (rUint[firstNonzeroWord] != 0)
                    break;
            }
            for (int i = 0; i <= firstNonzeroWord; i++)
            {
                mres += ((ml * rUint[i]) << ISIZE * 8 * i);
            }

#endif
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
        public static bool operator <(ModNumber l, ModNumber r)
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

        public static ModNumber operator <<(ModNumber n, int i)
        {
            int words = 0;
            if (i >= ISIZE * 8)
            {
                if (i >= NSIZE)
                    return new ModNumber();
                words = i / (ISIZE * 8);
                i %= ISIZE * 8;
            }
#if UNSAFE
            ModNumber mres = new ModNumber();
            unsafe
            {
                fixed (ulong* pN = &n.num[0])
                fixed (ulong* pRes = &mres.num[0])
                {
                    uint* piN = (uint*)pN;
                    uint* piRes = (uint*)pRes;
                    piRes[ICOUNT - 1] = piN[ICOUNT - words - 1] << i;
                    for (int j = ICOUNT - 2; j >= words; j--)
                    {
                        ulong tmp = ((ulong)(piN[j - words])) << i;
                        uint* pTmp = (uint*)&tmp;
                        piRes[j + 1] |= pTmp[1];
                        piRes[j] = pTmp[0];
                    }
                }

            }
#else
            ulong lomask = 0xffffffff;
            ulong himask = lomask << ISIZE * 8;
            uint[] resUint = new uint[ICOUNT];
            uint[] nUint = n.ToUintArray();
            resUint[ICOUNT - 1] = nUint[ICOUNT - words - 1] << i;
            for (int j = ICOUNT - 2; j >= words; j--)
            {
                ulong tmp = ((ulong)(nUint[j - words])) << i;
                resUint[j + 1] |= (uint)((tmp & himask) >> ISIZE * 8);
                resUint[j] = (uint)(tmp & lomask);
            }
            ModNumber mres = new ModNumber(resUint);
#endif
            return mres;
        }

        public static ModNumber operator >>(ModNumber n, int i)
        {
            int words = 0;
            if (i >= ISIZE * 8)
            {
                if (i >= NSIZE)
                {
                    return new ModNumber();
                }
                words = i / (ISIZE * 8);
                i %= ISIZE * 8;
            }
#if UNSAFE
            ModNumber mres = new ModNumber();
            unsafe
            {
                fixed (ulong* pN = &n.num[0])
                fixed (ulong* pRes = &mres.num[0])
                {
                    uint* pNi = (uint*)pN;
                    uint* pResi = (uint*)pRes;
                    pResi[0] = pNi[words] >> i;
                    for (int j = 0; j < ICOUNT - words - 1; j++)
                    {
                        ulong tmp = ((ulong)pNi[j + words + 1]) << ((ISIZE * 8) - i);
                        uint* pTmpi = (uint*)&tmp;
                        pResi[j] |= pTmpi[0];
                        pResi[j + 1] = pTmpi[1];
                    }
                }
            }
#else
            ulong lomask = 0xffffffff;
            ulong himask = lomask << ISIZE * 8;
            uint[] resUint = new uint[ICOUNT];
            uint[] nUint = n.ToUintArray();
            resUint[0] = nUint[words] >> i;
            for (int j = 0; j < ICOUNT - words - 1; j++)
            {
                ulong tmp = ((ulong)nUint[j + words + 1]) << ((ISIZE * 8) - i);
                resUint[j] |= (uint)(tmp & lomask);
                resUint[j + 1] = (uint)((tmp & himask) >> ISIZE * 8);
            }
            ModNumber mres = new ModNumber(resUint);
#endif
            return mres;
        }
        public (ModNumber, uint) DivideAndModulo(uint scalar, bool onlyModulo)
        {
            if (scalar == 0)
                throw new DivideByZeroException("Division by zero not allowed!");
            uint modRes;
#if UNSAFE
            ModNumber mres = new ModNumber();
            unsafe
            {
                fixed (ulong* pN = &num[0])
                fixed (ulong* pRes = &mres.num[0])
                {
                    uint* piN = (uint*)pN;
                    uint* piRes = (uint*)pRes;
                    ulong tmp = 0ul;
                    for (int i = ModNumber.ICOUNT - 1; i >= 0; i--)
                    {
                        *((uint*)&tmp) = piN[i];
                        if (scalar <= tmp)
                        {
                            if (!onlyModulo)
                                piRes[i] = (uint)(tmp / scalar);
                            tmp %= scalar;

                        }
                        tmp <<= ModNumber.ISIZE * 8;
                    }
                    modRes = ((uint*)&tmp)[1];
                }
            }
#else
            ulong himask = 0xfffffffful << ISIZE * 8;
            uint[] resUint = new uint[ICOUNT];
            uint[] nUint = ToUintArray();
            ulong tmp = 0ul;
            for (int i = ModNumber.ICOUNT - 1; i >= 0; i--)
            {
                tmp |= nUint[i];
                if (scalar <= tmp)
                {
                    if (!onlyModulo)
                        resUint[i] = (uint)(tmp / scalar);
                    tmp %= scalar;

                }
                tmp <<= ModNumber.ISIZE * 8;
            }
            modRes = (uint)((tmp & himask) >> ISIZE * 8);
            ModNumber mres = new ModNumber(resUint);

#endif
            return (mres, modRes);
        }

        public static ModNumber operator /(ModNumber n, uint scalar)
        {
            (ModNumber mres, uint divRes) = n.DivideAndModulo(scalar, false);
            return mres;
        }

        public static uint operator %(ModNumber n, uint scalar)
        {
            (ModNumber div, uint modulo) = n.DivideAndModulo(scalar, true);
            return modulo;

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
                return (divRes, modRes);
            }
            l.num.CopyTo(modRes.num, 0);
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
                    while (modRes >= rShiftedLeftBits)
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
        public static ModNumber operator /(ModNumber ml, ModNumber mr)
        {
            (ModNumber div, ModNumber mod) = DivideAndModulo(ml, mr, false);
            return div;
        }
        private static void Swap(ModNumber ml, ModNumber mr)
        {
            ModNumber tmp = ml;
            ml = mr;
            mr = tmp;
        }
        public static ModNumber Gcd(ModNumber ml, ModNumber mr)
        {
            ModNumber mzero = new ModNumber(0ul);
            ModNumber mone = new ModNumber(1ul);
            if (ml == mzero || mr == mzero)
                throw new ArgumentException("Division by Zero not allowed!");
            if (ml == mone)
                return mone;
            if (mr == mone)
                return mone;
            if (ml == mr)
                return new ModNumber(ml);
            ModNumber lcopy = new ModNumber(ml);
            ModNumber rcopy = new ModNumber(mr);
            if (lcopy < rcopy)
                Swap(lcopy, rcopy);
            ModNumber tmp = lcopy % rcopy;
            while (!(tmp == mone))
            {
                if (tmp == mzero)
                    return new ModNumber(rcopy);
                lcopy = rcopy;
                rcopy = tmp;
                tmp = lcopy % rcopy;
            }
            return new ModNumber(tmp);
        }
        public static ModNumber Lcm(ModNumber ml, ModNumber mr)
        {
            ModNumber GcdRes = Gcd(ml, mr);
            ModNumber lDivGcd = ml / GcdRes;
            return lDivGcd * mr;
        }
        public uint GetByteCount()
        {
#if UNSAFE
            unsafe
            {
                fixed (ulong* p = &this.num[0])
                {
                    byte* pb = (byte*)p;
                    for (int i = NCOUNT - 1; i >= 0; i--)
                    {
                        if (pb[i] != 0)
                            return (uint)i + 1;
                    }
                }
            }
#else
            for (int i = LCOUNT - 1; i >= 0; i--)
            {
                if (num[i] != 0)
                {
                    ulong mask = 0xfful << (LSIZE - 1) * 8;
                    for (int j = 0; j < LSIZE; j++)
                    {
                        if ((num[i] & mask) != 0ul)
                        {
                            return (uint)(i * LSIZE + (LSIZE - j));
                        }
                        mask >>= 8;
                    }
                }
            }
#endif
            return 0;
        }
        public ushort GetDoubleByteValue(int cb)
        {
#if UNSAFE
            unsafe
            {
                fixed(ulong *p = &this.num[0])
                {
                    ushort *ps = (ushort*)p;
                    return ps[cb];
                }
            }
#else
            int lCount = cb / 4;
            int sCount = cb % 4;
            ulong mask = 0xfffful << (sCount*16);
            return (ushort)((num[lCount] & mask) >> (sCount*16));

#endif
        }
        public ModNumber Sqrt()
        {
            ModNumber mzero = new ModNumber(0ul);
            if (this == mzero)
                return new ModNumber(mzero);
            ModNumber mone = new ModNumber(1ul);
            if (this == mone)
                return new ModNumber(mone);
            ModNumber mres = new ModNumber(0ul);
            ModNumber mdivisor = new ModNumber(0ul);
            ModNumber mremainder = new ModNumber(0ul);
            uint doubleByteCount = (GetByteCount() - 1u) / 2u;
            for (int i = (int)doubleByteCount; i >= 0; i--)
            {
                ushort tmp = GetDoubleByteValue(i);
                mremainder <<= 16;
                mdivisor <<= 8;
                mres <<= 8;
#if UNSAFE
                unsafe
                {
                    fixed (ulong* p = &mremainder.num[0])
                    {
                        ushort *ps = (ushort*)p;
                        *ps |= tmp;
                    }

                }
#else
                mremainder.num[0] |= tmp;
#endif
                if (mremainder == mzero)
                    continue;
                ushort counter = 1;
                mdivisor += 1;
                ModNumber mDivisorTimesCounter = mdivisor * counter;
                while (mDivisorTimesCounter < mremainder)
                {
                    mdivisor += 1;
                    mDivisorTimesCounter = mdivisor * ++counter;
                }
                if (mDivisorTimesCounter == mremainder)
                {
                    mremainder = new ModNumber(0ul);

                }
                else
                {
                    mdivisor -= 1;
                    mDivisorTimesCounter = mdivisor * --counter;
                    mremainder -= mDivisorTimesCounter;
                }
                mdivisor += counter;
                mres += counter;
            }
            return mres;
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
                bool result = ulong.TryParse(tmp, System.Globalization.NumberStyles.HexNumber, null, out n[LCOUNT - (i / (LSIZE * 2)) - 1]);
                if (!result)
                    throw new ArgumentException("Illegal Hex character");
            }
            ModNumber mres = new ModNumber(n);
            return mres;
        }
        public static ModNumber StomnOctalBase(string s)
        {
            ulong[] n = new ulong[LCOUNT];
            ulong buffer = 0ul;
            int bitCount = 0;
            int firstBits = (ModNumber.NSIZE % 3 == 0) ? 0 : 3 - ModNumber.NSIZE % 3;
            ulong mask = 4ul;
            string expandedStr = AdjustStringLength(s, ModNumber.OctalStringLength);
            for (int i = 0; i < ModNumber.OctalStringLength; i++)
            {
                if (!char.IsDigit(expandedStr[i]) || expandedStr[i] == '8' || expandedStr[i] == '9')
                    throw new ArgumentException("Only octal digits allowed!");
                ulong digit = (ulong)expandedStr[i] - '0';
                for (int j = 0; j < 3; j++)
                {
                    buffer <<= 1;
                    ulong res = digit & mask;
                    res >>= 2;
                    buffer |= res;
                    digit <<= 1;
                    if (firstBits != 0)
                    {
                        firstBits--;
                        continue;
                    }
                    bitCount++;
                    if (bitCount % (8 * ModNumber.LSIZE) == 0)
                    {
                        n[ModNumber.LCOUNT - (bitCount / (8 * ModNumber.LSIZE))] = buffer;
                        buffer = 0ul;
                    }
                }
            }
            return new ModNumber(n);

        }
        public static ModNumber StomnDecimalBase(string s)
        {
            ModNumber mres = new ModNumber();
            s = AdjustStringLength(s, DecimalStringLength);
            for(int i = 0; i < DecimalStringLength;i++)
            {
                if (!Char.IsDigit(s[i]))
                    throw new ArgumentException("Only digits allowed");
                uint digit = (uint)s[i] - '0';
                mres *= 10u;
                mres += digit;
            }
            return mres;

        }

        public static ModNumber Stomn(string s, int digitBase = 10)
        {
            if (!(digitBase == 8 || digitBase == 10 || digitBase == 16))
                throw new ArgumentException("Only base 8, 10 and 16 are valid");
            if (s.Length == 0)
                return new ModNumber(0);
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
            if (s[0] == '+')
                s = s.Substring(1);
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
        public string ToString_HexBase()
        {
            StringBuilder res = new StringBuilder(ModNumber.HexStringLength + 1);
            int buflen = ModNumber.LSIZE * 2;
            string formatStr = "{" + $"0,{buflen}:X{buflen}" + "}";
            for (int i = ModNumber.LCOUNT - 1; i >= 0; i--)
            {
                string digits = string.Format(formatStr, num[i]);
                res.Append(digits);
            }
            return res.ToString();
        }
        public string ToString_DecBase()            
        {
            StringBuilder res = new StringBuilder(ModNumber.DecimalStringLength);
            res.Append('0', ModNumber.DecimalStringLength);
            ModNumber tmp = new ModNumber(this);
            for (int i = 0; i < ModNumber.DecimalStringLength; i++)
            {
                uint digit = tmp % 10u;
                tmp /= 10u;
                res[ModNumber.DecimalStringLength - i - 1] = (char)('0' + (char)digit); 
            }
            return res.ToString();
        }
        public string ToString_OctBase()
        {
            StringBuilder res = new StringBuilder(new string('0', ModNumber.OctalStringLength));
            uint mask = 7u;
#if UNSAFE
            unsafe
            {
                fixed (ulong* pN = &num[0])
                {
                    uint *pIN = (uint *)pN;
                    uint [] buffer = new uint[2];
                    fixed(uint *piBuffer = &buffer[0])
                    {
                        ulong *pBuffer = (ulong *)piBuffer;
                        buffer[0] = pIN[0];
                        int tripleCount = 0;
                        int wordCount = 0;
                        for(int i = 0; i < ModNumber.NSIZE; i++)
                        {
                            if ((wordCount++ % (8 * ModNumber.ISIZE)) == 0)
                            {
                                if((wordCount/(8 * ModNumber.ISIZE) + 1) < ModNumber.ICOUNT)
                                {
                                    buffer[1] = pIN[wordCount/(8 * ModNumber.ISIZE)+1];
                                }
                            }
                            if(tripleCount++ % 3 == 0)
                            {
                                uint numToPrint = buffer[0] & mask;
                                char charToPrint = (char)('0' + numToPrint);
                                res[ModNumber.OctalStringLength - (tripleCount/3) - 1] = charToPrint;
                            }
                            (*pBuffer) >>= 1;
                        }
                    }
                }
            }
#else
            uint[] thisUint = ToUintArray();
            uint[] buffer = new uint[2];
            buffer[0] = thisUint[0];
            int tripleCount = 0;
            int wordCount = 0;
            for (int i = 0; i < ModNumber.NSIZE; i++)
            {
                if ((wordCount++ % (8 * ModNumber.ISIZE)) == 0)
                {
                    if ((wordCount / (8 * ModNumber.ISIZE) + 1) < ModNumber.ICOUNT)
                    {
                        buffer[1] = thisUint[wordCount / (8 * ModNumber.ISIZE) + 1];
                    }
                }
                if (tripleCount++ % 3 == 0)
                {
                    uint numToPrint = buffer[0] & mask;
                    char charToPrint = (char)('0' + numToPrint);
                    res[ModNumber.OctalStringLength - (tripleCount / 3) - 1] = charToPrint;
                }
                buffer = ShiftUintArrayR(buffer);
            }

#endif
            return res.ToString();
        }

        public string ToString(int nBase = 10)
        {
            switch(nBase)
            {
                case 8:
                    return ToString_OctBase();
                case 10:
                    return ToString_DecBase();
                case 16:
                    return ToString_HexBase();
                default:
                    throw new ArgumentException("Base must be 8, 10 or 16");
            }
        }
        public void Write(StreamWriter sw, int numBase = 10)
        {
            sw.WriteLine(ToString(numBase));
        }
        public static ModNumber Read(StreamReader sr, int numBase = 10)
        {
            return Stomn(sr.ReadLine() ?? "", numBase);
        }
        public ModNumber GetPKCS1Mask(bool stable = false, int modulusSize = MaxMod)
        {
            uint keyByteSize = (uint)modulusSize;
            uint mSize = GetByteCount();
            uint mCount = mSize / LSIZE;
            if (keyByteSize - 11 < mSize)
                throw new ArgumentException("Message size greater than Key Byte size minus 11");
            ModNumber res = new ModNumber();
            Random random = new Random();
            uint padSize = keyByteSize - mSize - 3;
            uint totalBytesLeft = keyByteSize % LSIZE;
            uint totalNumWords = keyByteSize / LSIZE;
            if (totalBytesLeft > 1)
                totalNumWords++;
            ulong tmp = stable ? 0x0001u : 0x0002u;
            uint totalBytesShift = totalBytesLeft;
            if (totalBytesLeft < 2)
                totalBytesShift += 8;
            for (uint i = 0; i < totalBytesShift - 2; i++)
            {
                tmp <<= 8;
                byte mask = stable ? (byte)0xFF : (byte)(((byte)random.Next() % 0xFF) + 1u);
                tmp |= mask;
            }
            res.num[totalNumWords - 1] = tmp;
            uint padLeft = padSize - (totalBytesShift - 2);
            uint padLeftCount = padLeft / LSIZE;
            uint padLeftOver = padLeft % LSIZE;
            for (uint i = 0; i < padLeftCount; i++)
            {
                tmp = 0;
                for (int j = 0; j < LSIZE; j++)
                {
                    tmp <<= 8;
                    byte mask = stable ? (byte)0xFF : (byte)(((byte)random.Next() % 0xFF) + 1u);
                    tmp |= mask;

                }
                res.num[totalNumWords - i - 2] = tmp;
            }
            tmp = 0;
            for (uint j = 0; j < padLeftOver; j++)
            {
                byte mask = stable ? (byte)0xFF : (byte)(((byte)random.Next() % 0xFF) + 1u);
                tmp |= mask;
                tmp <<= 8;

            }
            tmp <<= (int)((LSIZE - padLeftOver - 1) * 8); // have to add size parameter!
            tmp |= num[mCount];
            res.num[totalNumWords - padLeftCount - 2] = tmp;
            for (uint i = 0; i < mCount; i++)
            {
                res.num[i] = num[i];
            }
            return res;
        }
        public ModNumber RemovePKCS1Mask()
        {
            ModNumber res;
#if UNSAFE
            unsafe
            {
                fixed(ulong *p = num)
                {
                    byte *pB = (byte *)p;
                    int i;
                    for (i = MaxMod - 1; i >= 0; i--)
                        if (pB[i] != 0)
                            break;
                    if (pB[i+1] != 0x00u)
                        throw new ArgumentException("Not a valid PKCS1 Mask");
                    if (pB[i] == 0x01u)
                        while (pB[--i] == 0xFF && i >= 0)
                            ;
                    else if (pB[i] == 0x02u)
                        while (pB[i] != 0x00u && i >= 0)
                            i--;
                    else
                        throw new ArgumentException("Not a valid PKCS1 Mask");
                    if (pB[i] != 0x00u)
                        throw new ArgumentException("Not a valid PKCS1 Mask");
                    res = new ModNumber(pB, i);
                }
            }
#else
            byte[] thisBytes = ToByteArray();
            int i;
            for (i = num.Length * LSIZE - 1; i >= 0; i--)
                if (thisBytes[i] != 0)
                    break;
            if (thisBytes[i + 1] != 0x00u)
                throw new ArgumentException("Not a valid PKCS1 Mask");
            if (thisBytes[i] == 0x01u)
                while (thisBytes[--i] == 0xFF && i >= 0)
                    ;
            else if (thisBytes[i] == 0x02u)
                while (thisBytes[i] != 0x00u && i >= 0)
                    i--;
            else
                throw new ArgumentException("Not a valid PKCS1 Mask");
            if (thisBytes[i] != 0x00u)
                throw new ArgumentException("Not a valid PKCS1 Mask");
            res = new ModNumber(num, i);

#endif
            return res;
        }
        public static ModNumber fromText(string text)
        {
            ulong[] res = new ulong[LCOUNT];
            int textSize = (text.Length * sizeof(char)) / LSIZE;
            int textLeft = (text.Length * sizeof(char)) % LSIZE;
            if (textSize > LCOUNT)
                throw new ArgumentException("Text message too long!");
            if (textSize == LCOUNT && textLeft > 0)
                throw new ArgumentException("Text message too long!");
#if UNSAFE
            unsafe
            {
                fixed (char* p = text)
                {
                    ulong* pUL = (ulong*)p;
                    for (int i = 0; i < textSize; i++)
                        res[i] = pUL[i];
                }
            }
#else
            for (int i = 0; i < textSize; i++)
            {
                for (int j = 0; j < LSIZE/sizeof(char); j++)
                {
                    res[i] |= ((ulong)text[i * LSIZE / sizeof(char) + j]) << (j * sizeof(char)*8);
                }
            }
#endif
            ulong tmp = 0ul;
            for (int i = 0; i < textLeft / sizeof(char); i++)
            {
                ulong c = text[(textSize * LSIZE) / sizeof(char) + i];
                c <<= sizeof(char) * 8 * i;
                tmp |= c;
            }
            if (textLeft > 0)
                res[textSize] = tmp;

            
            return new ModNumber(res);
        }
        public string getText()
        {
            uint byteCount = GetByteCount();
            if (byteCount % sizeof(char) != 0)
                byteCount += sizeof(char) - byteCount % sizeof(char);
            StringBuilder res = new StringBuilder((int)byteCount/sizeof(char));
#if UNSAFE
            unsafe
            {
                fixed(ulong* p = &this.num[0])
                {
                    char *pC = (char*)p;
                    for (int i = 0; i < byteCount / sizeof(char); i++)
                        res.Append(pC[i]);
                }
            }
#else
            char[] thisChar = ToCharArray();
            for (int i = 0; i < byteCount / sizeof(char); i++)
                res.Append(thisChar[i]);

#endif
            return res.ToString();
        }
#if UNSAFE
        unsafe (ASNElementType, uint, uint) ReadASNElement(byte *p, uint i )
#else
        static (ASNElementType, uint, uint) ReadASNElement(byte[] p, uint i)
#endif
        {
            if (i == 0)
                throw new ArgumentException("Not a valid BER encodign");
            switch(p[i] >> 6)
            {
                case 0:
                    switch(p[i] >> 5)
                    {
                        case 0:
                        case 1:
                        
                            {
                                byte mask = (byte)0x1Fu;
                                byte masked = (byte)(p[i] & mask);
                                switch(masked)
                                {
                                    case (byte)ASNElementType.SEQUENCE:
                                        switch(p[i-1] >> 7)
                                        {
                                            case 0:
                                                {
                                                    byte mask2 = (byte)0x7Fu;
                                                    byte masked2 = (byte)(p[i-1] & mask2);
                                                    return (ASNElementType.SEQUENCE, masked2, i - 2);
                                                }
                                            case 1:
                                                {
                                                    uint nLen = p[i - 1] & 0x7Fu;
                                                    uint byteLen = 0u;
                                                    for (int j = 0; j < nLen; j++)
                                                    {
                                                        uint tmp = p[i - nLen - 1];
                                                        byteLen |= (tmp << (j * 8));

                                                    }
                                                    return (ASNElementType.SEQUENCE, byteLen, i - nLen - 2);
                                                }
                                        }
                                        break;
                                    case (byte)ASNElementType.OBJECT_IDENTIFIER:
                                        switch(p[i-1] >> 7)
                                        {
                                            case 0:
                                                byte mask2 = (byte)0x7Fu;
                                                byte masked2 = (byte)(p[i - 1] & mask2);
                                                return (ASNElementType.OBJECT_IDENTIFIER, masked2, i - 2);
                                            default:
                                                throw new ArgumentException("Not a short length specifier!");
                                        }
                                    case (byte)ASNElementType.NULL_VALUE:
                                        if (p[i - 1] != 0)
                                            throw new ArgumentException("Not a valid NULL object");
                                        return (ASNElementType.NULL_VALUE, p[i - 1], i - 2);
                                    case (byte)ASNElementType.OCTET_STRING:
                                        switch(p[i-1] >> 7)
                                        {
                                            case 0:
                                                byte mask2 = (byte)0x7Fu;
                                                byte masked2 = (byte)(p[i - 1] & mask2);
                                                return (ASNElementType.OCTET_STRING, masked2, i - 2);
                                            default:
                                                throw new ArgumentException("Not a short length specifier!");
                                        }
                                    case (byte)ASNElementType.INTEGER_VALUE:
                                        switch(p[i-1] >> 7)
                                        {
                                            case 0:
                                                byte mask2 = (byte)0x7Fu;
                                                byte masked2 = (byte)(p[i-1] & mask2);
                                                return (ASNElementType.INTEGER_VALUE, masked2, i - 2);
                                            default:
                                                throw new ArgumentException("Not a short length specifier!");

                                        }

                                }
                                

                            }
                            break;
                        default:
                            throw new ArgumentException("Not a constructed ASN.1 type!");
                    }
                    break;
                default:
                    throw new ArgumentException("Not a native ASN.1 type!");

            }
            throw new ArgumentException("Error");

        }
        public static List<object> ParseBERASNString(string BERASNString)
        {
            List<object> res = new List<object>();

                    byte[] pC = HexStringToByteArray(BERASNString);
                    uint i;
                    for (i = (uint)pC.Length - 1; i > 0; i--)
                        if (pC[i] != 0)
                            break;
                    (ASNElementType type, uint len, uint index) ASNElement1 = ReadASNElement(pC, i);
                    if (ASNElement1.type == ASNElementType.SEQUENCE)
                    {
                        (ASNElementType type, uint len, uint index) ASNElement2 = ReadASNElement(pC, ASNElement1.index);
                        if (ASNElement2.type == ASNElementType.SEQUENCE)
                        {
                            (ASNElementType type, uint len, uint index) ASNElement3 = ReadASNElement(pC, ASNElement2.index);
                            if (ASNElement3.type == ASNElementType.OBJECT_IDENTIFIER)
                            {
                                byte b = pC[ASNElement3.index];
                                string s = string.Format("{0:D}", (int)(b / 40));
                                s += ".";
                                s += string.Format("{0:D}", (int)(b % 40));
                                s += ".";
                                ulong number = 0ul;
                                for (int k = 1; k < ASNElement3.len; k++)
                                {
                                    byte mask = 0x80;
                                    b = pC[ASNElement3.index - k];
                                    number <<= 7;
                                    number |= (byte)(b & (byte)~mask);
                                    if ((b & mask) == 0)
                                    {
                                        s += string.Format("{0:D}", number);
                                        s += ".";
                                        number = 0ul;
                                    }
                                }
                                res.Add(s);
                            }
                            (ASNElementType type, uint len, uint index) ASNElement4 = ReadASNElement(pC, ASNElement3.index - ASNElement3.len);
                            (ASNElementType type, uint len, uint index) ASNElement5 = ReadASNElement(pC, ASNElement4.index);
                            if (ASNElement5.type == ASNElementType.OCTET_STRING)
                            {
                                byte [] bytes = new byte [ASNElement5.len];
                                for (int k = 0; k < ASNElement5.len; k++)
                                {
                                    bytes[k] = pC[ASNElement5.index - k];
                                }
                                res.Add(bytes);

                            }
                        }
                        else if (ASNElement2.type == ASNElementType.INTEGER_VALUE)
                        {
                            byte[] bytes = new byte[ASNElement2.len];
                            for (int k = 0; k < ASNElement2.len; k++)
                            {
                                bytes[k] = pC[ASNElement2.index - k];
                            }
                            res.Add(bytes);
                            (ASNElementType type, uint len, uint index) ASNElement3 = ReadASNElement(pC, ASNElement2.index- ASNElement2.len);
                            if (ASNElement3.type == ASNElementType.INTEGER_VALUE)
                            {
                                byte [] bytes2 = new byte[ASNElement3.len];
                                for (int k = 0; k < ASNElement3.len; k++)
                                {
                                    bytes2[k] = pC[ASNElement3.index - k];

                                }
                                res.Add(bytes2);
                            }
                        }
                    }

            return res;
        }
        public static ModNumber CreateBERASNString(byte[] hashBigEndian, string plainOid)
        {
            List<int> oidNumbers = new List<int>();
            int oldPos = 0;
            int pos = plainOid.IndexOf('.', oldPos);
            while (pos != -1)
            {
                oidNumbers.Add(int.Parse(plainOid.Substring(oldPos, pos - oldPos)));
                oldPos = pos + 1;
                pos = plainOid.IndexOf('.',oldPos);
            }
            oidNumbers.Add(int.Parse(plainOid.Substring(oldPos)));
            byte first = (byte)(oidNumbers[0] * 40);
            first += (byte)oidNumbers[1];
            List<byte> encodedOid = new List<byte>();
            encodedOid.Add(first);
            for (int i = 2; i < oidNumbers.Count; i++)
            {
                uint oidPart = (uint)oidNumbers[i];
                byte divisor = (byte)0x80u;
                byte mask = (byte)0x7F;
                List<byte> oidByteList = new List<byte>();
                byte oidByte = (byte)(oidPart % divisor);
                oidByteList.Add(oidByte);
                oidPart >>= 7;
                while (oidPart > 0)
                {
                    oidByte = (byte)((oidPart & mask) | divisor);
                    oidByteList.Add(oidByte);
                    oidPart >>= 7;
                }
                for (int j = oidByteList.Count - 1; j >= 0; j--)
                    encodedOid.Add(oidByteList[j]);
            }
            List<byte> innerASN = new List<byte>();
            List<byte> outerASN = new List<byte>();
            innerASN.Add((byte)ASNElementType.OBJECT_IDENTIFIER);
            innerASN.Add((byte)encodedOid.Count);
            innerASN.AddRange(encodedOid);
            innerASN.Add((byte)ASNElementType.NULL_VALUE);
            innerASN.Add(0);
            outerASN.Add((byte)ASNElementType.SEQUENCE | 0x20);
            outerASN.Add((byte)innerASN.Count);
            outerASN.AddRange(innerASN);
            outerASN.Add((byte)(ASNElementType.OCTET_STRING));
            outerASN.Add((byte)hashBigEndian.Length);
            outerASN.AddRange(hashBigEndian);
            List<byte> result = new List<byte>();
            result.Add((byte)ASNElementType.SEQUENCE | 0x20);
            result.Add((byte)outerASN.Count);
            result.AddRange(outerASN);
            byte[] resultArray = result.ToArray();
            byte[] resultLittleEndian;
#if UNSAFE
            unsafe
            {
                fixed(byte *p = resultArray)
                {
                    resultLittleEndian = ModNumber.convertEndianess(p, resultArray.Length);
                }
            }
#else
            resultLittleEndian = ModNumber.convertEndianess(resultArray);

#endif
            ModNumber res = new ModNumber(resultLittleEndian);
            return res;
        }
        public static ModNumber GetLeftMostBytes(ModNumber mn, int leftBytes)
        {
            byte[] leftMostBytes = new byte[leftBytes];
#if UNSAFE
            unsafe
            {
                fixed(ulong *p = mn.num)
                {
                    byte* pB = (byte*)p;
                    uint numBytes = mn.GetByteCount();
                    for (int i = 0; i < leftBytes; i++)
                        leftMostBytes[i] = pB[numBytes - leftBytes + i];

                }
            }
#else
            byte[] thisBytes = mn.ToByteArray();
            uint numBytes = mn.GetByteCount();
            for (int i = 0; i < leftBytes; i++)
                leftMostBytes[i] = thisBytes[numBytes - leftBytes + i];

#endif
            return new ModNumber(leftMostBytes);
        }
    }
    public class ScaledNumber
    {
        public ScaledNumber(ModNumber mn, int scale, bool alreadyScaled = false)
        {
            if (alreadyScaled)
                this.mn = mn;
            else
                this.mn = mn << (8 * scale);
            this.scale = scale;
        }
        public int scale;
        private ModNumber mn;
        public static bool operator ==(ScaledNumber l, ScaledNumber  r)
        {
            return l.scale == r.scale && l.mn == r.mn;
        }
        public static bool operator !=(ScaledNumber l, ScaledNumber r)
        {
            return l.scale != r.scale || l.mn != r.mn;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            else
            {
                ScaledNumber other = (obj as ScaledNumber)!;
                return scale == other.scale && mn == other.mn;
            }
        }
        public override int GetHashCode()
        {
            return scale.GetHashCode() + mn.GetHashCode();
        }
        public string ToString(int nBase = 10)
        {
            switch (nBase)
            {
                case 8:
                    return ToString_OctBase();
                case 10:
                    return ToString_DecBase();
                case 16:
                    return ToString_HexBase();
                default:
                    throw new ArgumentException("Base must be 8, 10 or 16");
            }
        }
        public (int,int) CalculateOctalStringLength()
        {
            return ((ModNumber.NCOUNT - scale) * 8 % 3 == 0 ? (ModNumber.NCOUNT - scale) * 8 / 3 : (ModNumber.NCOUNT - scale) * 8 / 3 + 1, (scale * 8 % 3) == 0 ? (scale * 8 / 3) : (scale * 8 / 3 + 1));
        }
        private string ToString_OctBase()
        {
            StringBuilder res = new StringBuilder(ModNumber.OctalStringLength+3);
            uint mask = 7u;
            int wordsToSkip = scale / ModNumber.ISIZE;
            int bitsToSkip = (scale % ModNumber.ISIZE) * 8;
#if UNSAFE
            unsafe
            {
                fixed(ulong *pN = &mn.num[0])
                {
                    uint* piNStart = (uint*)pN;
                    uint* piN = piNStart + wordsToSkip; 
                    uint[] buffer = new uint[2];
                    fixed(uint *piBuffer = &buffer[0])
                    {
                        ulong *pBuffer = (ulong *)piBuffer;
                        buffer[0] = piN[0];
                        buffer[1] = piN[1];
                        int tripleCount = 0;
                        (int digitsLeft, int digitsToSkip) = CalculateOctalStringLength();
                        res.Append(new string('0', digitsToSkip + digitsLeft + 1));
                        res[digitsLeft] = '.';
                        int wordCount = bitsToSkip;
                        (*pBuffer) >>= bitsToSkip;
                        for (int i = scale * 8; i < ModNumber.NSIZE; i++)
                        {
                            if((wordCount++ % (8 * ModNumber.ISIZE)) == 0)
                            {
                                if (wordCount / (8 * ModNumber.ISIZE) + 1 < ModNumber.ICOUNT - wordsToSkip)
                                    buffer[1] = piN[wordCount / (8 * ModNumber.ISIZE) + 1];
                                else
                                    buffer[1] = 0;
                            }
                            if (tripleCount++ % 3 == 0)
                            {
                                uint numToPrint = buffer[0] & mask;
                                char charToPrint = (char)('0'+numToPrint);
                                res[digitsLeft - (tripleCount / 3) - 1] = charToPrint;
                            }
                            (*pBuffer) >>= 1;
                        }
                        buffer[1] = piN[0];
                        if (wordsToSkip > 0)
                        {
                            buffer[0] = piN[-1];
                        }
                        else
                            buffer[0] = 0u;
                        (*pBuffer) <<= (ModNumber.ISIZE * 8) - bitsToSkip;
                        wordCount = ModNumber.ISIZE * 8 - bitsToSkip;
                        tripleCount = 0;
                        for (int i = 0; i < scale * 8; i++)
                        {
                            if ((wordCount++ % (8 * ModNumber.ISIZE)) == 0)
                                if (wordCount < (wordsToSkip * ModNumber.ISIZE * 8))
                                    buffer[0] = piN[-1 - (wordCount / (8 * ModNumber.ISIZE))];
                                else
                                    buffer[0] = 0;
                            if (tripleCount++ % 3 == 0)
                            {
                                uint numToPrint = (buffer[1] >> (32 - 3)) & mask;
                                char charToPrint = (char) ('0'+ numToPrint);
                                res[digitsLeft + (tripleCount / 3) + 1] = charToPrint;
                            }
                            (*pBuffer) <<= 1;
                        }
                    }
                }
            }
#else
            uint[] mnUint = mn.ToUintArray();
            uint[] buffer = new uint[2];
            buffer[0] = mnUint[wordsToSkip];
            buffer[1] = mnUint[wordsToSkip + 1];
            int tripleCount = 0;
            (int digitsLeft, int digitsToSkip) = CalculateOctalStringLength();
            res.Append(new string('0', digitsToSkip + digitsLeft + 1));
            res[digitsLeft] = '.';
            int wordCount = bitsToSkip;
            buffer = ModNumber.ShiftUintArrayR(buffer, wordCount);
            for (int i = scale * 8; i < ModNumber.NSIZE; i++)
            {
                if ((wordCount++ % (8 * ModNumber.ISIZE)) == 0)
                {
                    if (wordCount / (8 * ModNumber.ISIZE) + 1 < ModNumber.ICOUNT - wordsToSkip)
                        buffer[1] = mnUint[wordsToSkip+wordCount / (8 * ModNumber.ISIZE) + 1];
                    else
                        buffer[1] = 0;
                }
                if (tripleCount++ % 3 == 0)
                {
                    uint numToPrint = buffer[0] & mask;
                    char charToPrint = (char)('0' + numToPrint);
                    res[digitsLeft - (tripleCount / 3) - 1] = charToPrint;
                }
                buffer = ModNumber.ShiftUintArrayR(buffer);
            }
            buffer[1] = mnUint[wordsToSkip];
            if (wordsToSkip > 0)
            {
                buffer[0] = mnUint[wordsToSkip-1];
            }
            else
                buffer[0] = 0u;
            buffer = ModNumber.ShiftUintArrayL(buffer, (ModNumber.ISIZE * 8) - bitsToSkip);
            wordCount = ModNumber.ISIZE * 8 - bitsToSkip;
            tripleCount = 0;
            for (int i = 0; i < scale * 8; i++)
            {
                if ((wordCount++ % (8 * ModNumber.ISIZE)) == 0)
                    if (wordCount < (wordsToSkip * ModNumber.ISIZE * 8))
                        buffer[0] = mnUint[wordsToSkip -1 - (wordCount / (8 * ModNumber.ISIZE))];
                    else
                        buffer[0] = 0;
                if (tripleCount++ % 3 == 0)
                {
                    uint numToPrint = (buffer[1] >> (32 - 3)) & mask;
                    char charToPrint = (char)('0' + numToPrint);
                    res[digitsLeft + (tripleCount / 3) + 1] = charToPrint;
                }
                buffer = ModNumber.ShiftUintArrayL(buffer);
            }


#endif
            return res.ToString();
        }
        public int calculateDecimalStringLengthLeft()
        {
            return (int)Math.Ceiling((ModNumber.NSIZE - scale * 8) * 0.30102999566398119521373889472449); // log(2)
        }
        private string ToString_DecBase()
        {
            int IntegerStringLength = calculateDecimalStringLengthLeft();
            StringBuilder sb = new StringBuilder(ModNumber.DecimalStringLength+3);
            sb.Append('0', IntegerStringLength);
            ModNumber tmp = mn >> scale * 8;
            for (int i = 0; i < IntegerStringLength; i++)
            {
                uint digit = tmp % 10u;
                tmp /= 10u;
                sb[IntegerStringLength - i - 1] += (char)digit;

            }
            if (scale > 0)
            {
                sb.Append('.');
                ModNumber tmpFraction = new ModNumber(mn);
                ModNumber divisor = new ModNumber(1ul);
                for (int i = 0; i < scale * 2; i++)
                    divisor *= 10u;
                tmpFraction *= divisor;
                tmpFraction >>= scale * 8;
                tmpFraction %= divisor;
                for (int i = 0; i < scale * 2; i++)
                {
                    divisor /= 10u;
                    ModNumber resTmp = tmpFraction / divisor;
                    uint digit = resTmp % 10u;
                    sb.Append((char)('0' + digit),1);
                }

            }
            return sb.ToString();
        }
        private string ToString_HexBase()
        {
            int bufLen = ModNumber.LSIZE * 2;
            StringBuilder sb = new StringBuilder(bufLen * ModNumber.LCOUNT + 1);
            string formatStr = "X" + bufLen.ToString();
            for (int i = ModNumber.LCOUNT - 1; i >= 0; i--)
            {
                string tmp = this.mn.num[i].ToString(formatStr);
                sb.Append(tmp);
            }
            if (scale > 0)
            {
                int pos = bufLen * ModNumber.LCOUNT - (scale * 2);
                sb.Insert(pos, '.');
            }
            return sb.ToString();
        }
        public ScaledNumber Sqrt()
        {
            ModNumber rt = mn.Sqrt();
            return new ScaledNumber(rt, scale / 2, true);
        }
    }
    public class MultGroupMod
    {
        public MultGroupMod(ModNumber n)
        {
            ModNumber modzero = new ModNumber(0ul);
            if (n == modzero)
                throw new ArgumentException("Modulus cannot be zero!");
            ModNumber modone = new ModNumber(1ul);
            if (n == modone)
                throw new ArgumentException("Modulus cannot be one!");
            n.CheckMax(ModNumber.COUNTMOD);
            this.n = new ModNumber(n);
        }
        private ModNumber n;
        public ModNumber Mult(ModNumber l, ModNumber r)
        {
            ModNumber res = new ModNumber(0ul);
            ModNumber lMod = l % n;
            ModNumber rMod = r % n;
#if UNSAFE
            unsafe
            {
                fixed(ulong *pR = &rMod.num[0])
                {
                    uint* pRint = (uint*)pR;
                    int limit;
                    for (limit = ModNumber.ICOUNT-1; limit >= 0; limit--)
                    {
                        if (pRint[limit] != 0)
                            break;
                    }
                    for (int i = 0; i <= limit; i++)
                    {
                        ModNumber tmp = lMod * pRint[i];
                        for(int j = 0; j < i; j++)
                        {
                            tmp %= n;
                            tmp <<= ModNumber.ISIZE * 8;
                        }
                        tmp %= n;
                        res += tmp;
                    }
                }    
            }
#else
            uint[] rModUint = rMod.ToUintArray();
            int limit;
            for (limit = ModNumber.ICOUNT - 1; limit >= 0; limit--)
            {
                if (rModUint[limit] != 0)
                    break;
            }
            for (int i = 0; i <= limit; i++)
            {
                ModNumber tmp = lMod * rModUint[i];
                for (int j = 0; j < i; j++)
                {
                    tmp %= n;
                    tmp <<= ModNumber.ISIZE * 8;
                }
                tmp %= n;
                res += tmp;
            }

#endif
            res %= n;
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
            ModNumber res = new ModNumber(1ul);
            ModNumber xMod = x % n;
            int top;
            for (top = ModNumber.LCOUNT - 1; top >= 0; top--)
                if (e.num[top] != 0)
                    break;
            for (int i = 0; i <= top; i++)
            {
                ulong mask = 1ul;
                ulong tmp = e.num[i];
                for (int j = 0; j < ModNumber.LSIZE * 8 && (tmp != 0ul || i < top); j++)
                {
                    if ((tmp & mask) != 0)
                        res = Mult(res, xMod);
                    xMod = Kwad(xMod);
                    tmp >>= 1;
                }
            }
            return res;
        }
        public ModNumber Add (ModNumber l, ModNumber r)
        {
            ModNumber lMod = l % n;
            ModNumber rMod = r % n;
            return (lMod + rMod) % n;
        }
        public ModNumber Diff(ModNumber l, ModNumber r)
        {
            ModNumber lMod = l % n;
            ModNumber rMod = r % n;
            if (lMod == rMod)
                return new ModNumber(0ul);
            if (lMod > rMod)
                return lMod - rMod;
            else
                return n - rMod + lMod;
        }
        public ModNumber Inverse (ModNumber mx)
        {
            ModNumber mzero = new ModNumber(0ul);
            ModNumber mone = new ModNumber(1ul);
            if (mx == mzero)
                throw new ArgumentException("Zero does not have an inverse.");
            if (mx == mone)
                return mone;
            if (mx == n)
                throw new ArgumentException("Zero does not have an inverse.");
            ModNumber r = mx % n;
            ModNumber l = new ModNumber(n);
            List<ModNumber> divisors = new List<ModNumber>();
            (ModNumber divRes, ModNumber modRes) = ModNumber.DivideAndModulo(l, r, false);
            while(!(modRes == mone))
            {
                if (modRes == mzero)
                    throw new ArgumentException("Numbers are not relative prime, so there is no inverse");
                divisors.Add(divRes);
                l = r;
                r = modRes;
                (divRes, modRes) = ModNumber.DivideAndModulo(l, r, false);
            }
            divisors.Add(divRes);
            ModNumber tmp1 = mzero;
            ModNumber tmp2 = mone;
            divisors.Reverse();
            foreach(ModNumber it in divisors)
            {
                ModNumber tmp = tmp2;
                ModNumber product = Mult(tmp2, it);
                tmp2 = Diff(tmp1, product);
                tmp1 = tmp;
            }
            return tmp2;
        }
    }
    public struct RSAParameters
    {
        public ModNumber PubExp;
        public ModNumber Modulus;
        public ModNumber Prime1;
        public ModNumber Prime2;
        public ModNumber Exp1;          // DP
        public ModNumber Exp2;          // DQ
        public ModNumber Coefficient;   // InverseQ
        public ModNumber PrivExp;
    }
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
        public ModNumber Encrypt(ModNumber m)
        {
            ModNumber masked = m.GetPKCS1Mask();
            MultGroupMod mgm = new MultGroupMod(Modulus);
            return mgm.Exp(masked, PubExp);
        }
        public ModNumber Decrypt(ModNumber c)
        {
            MultGroupMod mgmp = new MultGroupMod(Prime1);
            MultGroupMod mgmq = new MultGroupMod(Prime2);
            MultGroupMod mgmn = new MultGroupMod(Modulus);
            ModNumber ?m1 = null;
            ModNumber? m2 = null;
            Thread th1 = new Thread(() => { m1 = mgmp.Exp(c, Exp1); });
            Thread th2 = new Thread(() => { m2 = mgmq.Exp(c, Exp2); });
            th1.Start();
            th2.Start();
            th1.Join();
            th2.Join();
            ModNumber diff = mgmp.Diff(m1!, m2!);
            ModNumber h = mgmp.Mult(Coefficient, diff);
            ModNumber hq = mgmn.Mult(h, Prime2);
            ModNumber res = mgmn.Add(m2!, hq);
            return res.RemovePKCS1Mask();
        }
        public ModNumber EncryptSignature(byte [] hashBigendian, string hashOid)
        {
            ModNumber unmaskedResult = ModNumber.CreateBERASNString(hashBigendian, hashOid);
            ModNumber maskedResult = unmaskedResult.GetPKCS1Mask(true);
            MultGroupMod mgmp = new MultGroupMod(Prime1);
            MultGroupMod mgmq = new MultGroupMod(Prime2);
            MultGroupMod mgmn = new MultGroupMod(Modulus);
            ModNumber? m1 = null;
            ModNumber? m2 = null;
            Thread th1 = new Thread(() => { m1 = mgmp.Exp(maskedResult, Exp1); });
            Thread th2 = new Thread(() => { m2 = mgmq.Exp(maskedResult, Exp2); });
            th1.Start();
            th2.Start();
            th1.Join();
            th2.Join();
            ModNumber diff = mgmp.Diff(m1!, m2!);
            ModNumber h = mgmp.Mult(Coefficient, diff);
            ModNumber hq = mgmn.Mult(h, Prime2);
            ModNumber res = mgmn.Add(m2!, hq);
            return res;
        }
        public ModNumber DecryptSignature(ModNumber signature)
        {
            MultGroupMod mgm = new MultGroupMod(Modulus);
            ModNumber decryptedSignature = mgm.Exp(signature, PubExp);
            ModNumber removedMask = decryptedSignature.RemovePKCS1Mask();
            List<object> result = ModNumber.ParseBERASNString(removedMask.ToString(16));
            byte [] hashBigEndian = (byte [])result[1];
            byte[] hashLittleEndian;
#if UNSAFE
            unsafe
            {
                fixed(byte* pb = hashBigEndian)
                {
                    hashLittleEndian = ModNumber.convertEndianess(pb, hashBigEndian.Length);

                }
            }
#else
            hashLittleEndian = ModNumber.convertEndianess(hashBigEndian);

#endif
            ModNumber retval = new ModNumber(hashLittleEndian);
            return retval;

        }
        public static RSAParameters GetRSAKey(string keyName, bool createIfNotExists, CngKeyUsages usage = CngKeyUsages.Decryption)
        {
            RSAParameters rsaParameters;
            CngKey cngKey;
            if (CngKey.Exists(keyName))
                cngKey = CngKey.Open(keyName);
            else
            {
                if (createIfNotExists)
                {
                    CngKeyCreationParameters parametersEncryptionRsa = new CngKeyCreationParameters();
                    parametersEncryptionRsa.KeyUsage = CngKeyUsages.Decryption;
                    byte[] keyLen = new byte[4];
                    keyLen[0] = (ModNumber.MaxMod * 8) % 0x100;
                    keyLen[1] = (ModNumber.MaxMod * 8) / 0x100;
                    CngPropertyOptions propOptions = new CngPropertyOptions();

                    CngProperty prop = new CngProperty("Length", keyLen, propOptions);
                    parametersEncryptionRsa.Parameters.Add(prop);
                    byte[] exportp = new byte[4];
                    exportp[0] = 0x03;
                    prop = new CngProperty("Export Policy", exportp, propOptions);
                    parametersEncryptionRsa.Parameters.Add(prop);

                    cngKey = CngKey.Create(CngAlgorithm.Rsa, keyName, parametersEncryptionRsa);
                }
                else
                    throw new ApplicationException("Key does not exist!");
            }
            CngPropertyOptions options = new CngPropertyOptions();
            CngProperty property = cngKey.GetProperty("Length", options);
            byte [] ?length = property.GetValue();
            int ?keyLength = (int?)(length?[1] * 0x100);
            keyLength += length?[0];
            if (keyLength > ModNumber.MaxMod * 8)
                throw new ApplicationException("Keylength not less or equal to MAXMOD!");
            byte[] keyBlob = cngKey.Export(new CngKeyBlobFormat("RSAFULLPRIVATEBLOB"));
            if (keyBlob[0] != 0x52 && keyBlob[1] != 0x53 && keyBlob[2] != 0x41 && keyBlob[3] != 0x33)
                throw new ApplicationException("Key structure not of type RSA Full Private!");
            int bitLength = keyBlob[5] * 0x100;
            bitLength += keyBlob[4];
            if (bitLength > ModNumber.MaxMod * 8)
                throw new ApplicationException("Keylength not less or equal to MAXMOD!");
            int cbPubExp = keyBlob[9] * 0x100;
            cbPubExp += keyBlob[8];
            int cbModulus = keyBlob[13] * 0x100;
            cbModulus += keyBlob[12];
            int cbPrime1 = keyBlob[17] * 0x100;
            cbPrime1 += keyBlob[16];
            int cbPrime2 = keyBlob[21] * 0x100;
            cbPrime2 += keyBlob[20];
#if UNSAFE
            unsafe
            {
                fixed(byte *pData = &keyBlob[24])
                {
                    byte* p = pData;
                    byte[] pubExpLittleEndian = ModNumber.convertEndianess(p, cbPubExp);
                    rsaParameters.PubExp = new ModNumber(pubExpLittleEndian);
                    p += cbPubExp;
                    byte[] modulusLittleEndian = ModNumber.convertEndianess(p, cbModulus);
                    rsaParameters.Modulus = new ModNumber(modulusLittleEndian);
                    p += cbModulus;
                    byte[] prime1LittleEndian = ModNumber.convertEndianess(p, cbPrime1);
                    rsaParameters.Prime1 = new ModNumber(prime1LittleEndian);
                    p += cbPrime1;
                    byte[] prime2LittleEndian = ModNumber.convertEndianess(p, cbPrime2);
                    rsaParameters.Prime2 = new ModNumber( prime2LittleEndian);
                    p += cbPrime2;
                    byte[] exp1LittleEndian = ModNumber.convertEndianess(p, cbPrime1);
                    rsaParameters.Exp1 = new ModNumber(exp1LittleEndian);
                    p += cbPrime1;
                    byte[] exp2LittleEndian = ModNumber.convertEndianess(p, cbPrime2);
                    rsaParameters.Exp2 = new ModNumber( exp2LittleEndian);
                    p += cbPrime2;
                    byte[] coefficientLittleEndian = ModNumber.convertEndianess(p, cbPrime1);
                    rsaParameters.Coefficient = new ModNumber(coefficientLittleEndian);
                    p += cbPrime1;
                    byte[] privExpLittleEndian = ModNumber.convertEndianess(p, cbModulus);
                    rsaParameters.PrivExp = new ModNumber(privExpLittleEndian);
                }
            }
#else
            int offset = 24;
            byte[] pubExpLittleEndian = ModNumber.convertEndianess(keyBlob, cbPubExp, offset);
            rsaParameters.PubExp = new ModNumber(pubExpLittleEndian);
            offset += cbPubExp;
            byte[] modulusLittleEndian = ModNumber.convertEndianess(keyBlob, cbModulus, offset);
            rsaParameters.Modulus = new ModNumber(modulusLittleEndian);
            offset += cbModulus;
            byte[] prime1LittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime1, offset);
            rsaParameters.Prime1 = new ModNumber(prime1LittleEndian);
            offset += cbPrime1;
            byte[] prime2LittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime2, offset);
            rsaParameters.Prime2 = new ModNumber(prime2LittleEndian);
            offset += cbPrime2;
            byte[] exp1LittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime1, offset);
            rsaParameters.Exp1 = new ModNumber(exp1LittleEndian);
            offset += cbPrime1;
            byte[] exp2LittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime2, offset);
            rsaParameters.Exp2 = new ModNumber(exp2LittleEndian);
            offset += cbPrime2;
            byte[] coefficientLittleEndian = ModNumber.convertEndianess(keyBlob, cbPrime1, offset);
            rsaParameters.Coefficient = new ModNumber(coefficientLittleEndian);
            offset += cbPrime1;
            byte[] privExpLittleEndian = ModNumber.convertEndianess(keyBlob, cbModulus, offset);
            rsaParameters.PrivExp = new ModNumber(privExpLittleEndian);

#endif
            return rsaParameters;
        }
        private ModNumber PubExp;
        private ModNumber Modulus;
        private ModNumber Prime1;
        private ModNumber Prime2;
        private ModNumber Exp1;          // DP
        private ModNumber Exp2;          // DQ
        private ModNumber Coefficient;   // InverseQ
        private ModNumber PrivExp;

    }
    public struct DSAParameters
    {
        public ModNumber P;
        public ModNumber Q;
        public ModNumber g;
        public ModNumber x;
        public ModNumber y;
    }
    public abstract class DSABase
    {
        public abstract ModNumber CalcR(ModNumber mk);
        public byte [] CalculateDSASignature(ModNumber Q, ModNumber x, byte[] hash, bool DEREncoded)
        {
            byte[] hashLittleEndian;
#if UNSAFE
            unsafe
            {
                fixed(byte* p = &hash[0])
                {
                    hashLittleEndian = ModNumber.convertEndianess(p, hash.Length);
                }
            }
#else
            hashLittleEndian = ModNumber.convertEndianess(hash);

#endif
            ModNumber mHash = new ModNumber(hashLittleEndian);
            int nLen = (int)Q.GetByteCount();
            if (hash.Length > nLen)
                mHash = ModNumber.GetLeftMostBytes(mHash, nLen);
            ulong[] k = new ulong[ModNumber.LCOUNT];
            Random random = new Random();
            ModNumber r;
            ModNumber s;
            ModNumber mk;
            ModNumber mzero = new ModNumber(0ul);
            int lSize = nLen / ModNumber.LSIZE;
            do
            {
                do
                {
                    for (int i = 0; i < lSize; i++)
                        k[i] = (ulong)(random.NextInt64());
                    mk = new ModNumber(k);
                    if (mk == mzero)
                        mk += (uint)(random.Next() + 1);
                    while (mk >= Q)
                    {
                        mk.num[lSize-1] -= (ulong)random.NextInt64();
                    }
                    r = CalcR(mk);
                } while (r == mzero);
                ModNumber ?kInverse = null;
                MultGroupMod mgm = new MultGroupMod(Q);
                Thread th1 = new Thread(() => { kInverse = mgm.Inverse(mk); });
                th1.Start();
                ModNumber ?hashPlusXr = null;
                Thread th2 = new Thread(() =>
                {
                    ModNumber xr = mgm.Mult(x, r);
                    hashPlusXr = mgm.Add(mHash, xr);
                });
                th2.Start();
                th1.Join();
                th2.Join();
                s = mgm.Mult(kInverse!, hashPlusXr!);
            } while (s == mzero);

            if (!(s < Q && r < Q))
                throw new ArgumentException("Wrong signature");
            byte[] rBigEndian = r.convertEndianess(nLen);
            byte[] sBigEndian = s.convertEndianess(nLen);
            if (DEREncoded)
            {
                return CreateBERASNStringForDSASignature(rBigEndian, sBigEndian);
            }
            else
            {
                byte[] rs = new byte[rBigEndian.Length + sBigEndian.Length];
                for (int i = 0; i < rBigEndian.Length; i++)
                    rs[i] = rBigEndian[i];
                for (int i = 0; i < sBigEndian.Length; i++)
                    rs[rBigEndian.Length + i] = sBigEndian[i];
                return rs;
            }
        }
        byte [] CreateBERASNStringForDSASignature(byte[] r, byte[] s)
        {
            byte[] retValue;
            int index = 0;
            int nLen = r.Length + s.Length;
            int extra = 2 + 2;
            int totalLength = nLen + extra;
            if (totalLength > 0x7F)
            {
                int lenBytes = 1;
                int tmp = totalLength;
                while ((tmp >>= 8) != 0)
                    lenBytes++;
                retValue = new byte[2 + totalLength + lenBytes];
                retValue[index++] = (byte)ASNElementType.SEQUENCE | (byte)0x20u;
                retValue[index++] = (byte)(0x80 | lenBytes);
                tmp = totalLength;
                for (int j = 0; j < lenBytes; j++)
                    retValue[index + lenBytes - j - 1] = (byte)((tmp >> 8 * j) & 0xFF);
                index += lenBytes;
            }
            else
            {
                retValue = new byte[2 + totalLength];
                retValue[index++] = (byte)ASNElementType.SEQUENCE | (byte)0x20u;
                retValue[index++] = (byte)(totalLength);
            }
            retValue[index++] = (byte)ASNElementType.INTEGER_VALUE;
            retValue[index++] = (byte)r.Length;
            for (int i = 0; i < r.Length; i++)
                retValue[index + i] = r[i];
            index += r.Length;
            retValue[index++] = (byte)ASNElementType.INTEGER_VALUE;
            retValue[index++] = (byte)s.Length;
            for (int i = 0; i < s.Length; i++)
                retValue[index + i] = s[i];
            return retValue;
        }
        public string ConvertSignatureToString(byte [] signature, bool DEREncoded)
        {
            if (DEREncoded)
            {
                StringBuilder sb = new StringBuilder(signature.Length * 2);
                for (int i = 0; i < signature.Length; i++)
                {
                    string tmp = string.Format("{0:X2}", (int)signature[i]);
                    sb.Append(tmp);
                }
                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder(signature.Length / 2);
                sb.Append(new string(' ', signature.Length / 2));
                for (int i = 0; i < signature.Length; i += 2)
                {
                    char c = (char)signature[i];
                    c |= (char)(signature[i + 1] << 8);
                    sb[i / 2] = c;
                }
                return sb.ToString();
            }
        }

    }
    public class DSA : DSABase
    {
        public DSA(DSAParameters parameters)
        {
            P = parameters.P;
            Q = parameters.Q;
            g = parameters.g;
            x = parameters.x;
            y = parameters.y;
        }
        public override ModNumber CalcR(ModNumber mk)
        {
            MultGroupMod mgm = new MultGroupMod(P);
            return mgm.Exp(g, mk) % Q;
        }
        public static (ModNumber, ModNumber, ModNumber) DSACalculateU1U2Mr(ModNumber Q, byte[] hash, string signature, bool DEREncoded)
        {
            byte[] r;
            byte[] s;
            if (DEREncoded)
            {
                List<object> signatureOctets;
                signatureOctets = ModNumber.ParseBERASNString(signature);
                r = (byte[])signatureOctets[0];
                s = (byte[])signatureOctets[1];
            }
            else
            {
                r = new byte[signature.Length];
                for (int i = 0; i < signature.Length / 2; i++)
                {
                    r[i * 2] = (byte)signature[i];
                    r[i * 2 + 1] = (byte)(signature[i] >> 8);
                }
                s = new byte[signature.Length];
                for (int i = 0; i < signature.Length / 2; i++)
                {
                    s[i * 2] = (byte)signature[signature.Length / 2 + i];
                    s[i * 2 + 1] = (byte)(signature[signature.Length / 2 + i] >> 8);
                }
            }
            return DSACalculateU1U2Mr(Q, hash, r, s);
        }

        public static (ModNumber, ModNumber, ModNumber) DSACalculateU1U2Mr(ModNumber Q, byte[] hash, byte[] r, byte[] s)
        {
            byte[] rLittleEndian;
            byte[] sLittleEndian;
#if UNSAFE
            unsafe
            {
                fixed (byte* pr = r)
                fixed (byte* ps = s)
                {
                    rLittleEndian = ModNumber.convertEndianess(pr, r.Length);
                    sLittleEndian = ModNumber.convertEndianess(ps, s.Length);
                }
            }
#else
            rLittleEndian = ModNumber.convertEndianess(r);
            sLittleEndian = ModNumber.convertEndianess(s);

#endif
            ModNumber mHash;
            int bcQ = (int)Q.GetByteCount();
#if UNSAFE
            unsafe
            {
                fixed (byte* p = hash)
                {
                    byte[] hashLittleEndian = ModNumber.convertEndianess(p, hash.Length);
                    mHash = new ModNumber(hashLittleEndian);
                    if (hash.Length > bcQ)
                    {
                        mHash = ModNumber.GetLeftMostBytes(mHash, bcQ);
                    }
                }
            }
#else
            byte[] hashLittleEndian = ModNumber.convertEndianess(hash);
            mHash = new ModNumber(hashLittleEndian);
            if (hash.Length > bcQ)
            {
                mHash = ModNumber.GetLeftMostBytes(mHash, bcQ);
            }

#endif
            ModNumber mr = new ModNumber(rLittleEndian);
            ModNumber ms = new ModNumber(sLittleEndian);
            if (!(mr < Q && ms < Q))
                throw new ArgumentException("Invalid signature");
            MultGroupMod mgm = new MultGroupMod(Q);
            ModNumber sInverse = mgm.Inverse(ms);
            ModNumber? u1 = null;
            Thread th1 = new Thread(() => { u1 = mgm.Mult(mHash, sInverse); });
            th1.Start();
            ModNumber? u2 = null;
            Thread th2 = new Thread(() => { u2 = mgm.Mult(mr, sInverse); });
            th2.Start();
            th1.Join();
            th2.Join();
            return (u1!, u2!, mr);

        }
        public string Sign(byte[] hash, bool DEREncoded)
        {
            byte[] signature = CalculateDSASignature(Q, x, hash, DEREncoded);
            return ConvertSignatureToString(signature, DEREncoded);
         }
        public bool Verify(byte[] hash, string signature, bool DEREndoded = true)
        {
            MultGroupMod mgm = new MultGroupMod(P);
            ModNumber ?mv1 = null;
            (ModNumber u1, ModNumber u2, ModNumber mr) = DSACalculateU1U2Mr(Q, hash, signature, DEREndoded);
            Thread th3 = new Thread(() => { mv1 = mgm.Exp(g, u1!); });
            th3.Start();
            ModNumber ?mv2 = null;
            Thread th4 = new Thread(() => { mv2 = mgm.Exp(y, u2!); });
            th4.Start();
            th3.Join();
            th4.Join();
            ModNumber mv = mgm.Mult(mv1!, mv2!) % Q;
            return mv == mr;
        }
        public static DSAParameters GetDSAKey(string keyName, bool createIfNotExists)
        {
            CngKey cngKey;
            if (!CngKey.Exists(keyName))
                if (createIfNotExists)
                {
                    CngKeyCreationParameters parametersSigningDsa = new CngKeyCreationParameters();
                    parametersSigningDsa.KeyUsage = CngKeyUsages.Signing;
                    byte[] keyLen = new byte[4];
                    keyLen[0] = (ModNumber.MaxMod * 8) % 0x100;
                    keyLen[1] = (ModNumber.MaxMod * 8) / 0x100;
                    CngPropertyOptions keyOptions = new CngPropertyOptions();

                    CngProperty prop = new CngProperty("Length", keyLen, keyOptions);
                    parametersSigningDsa.Parameters.Add(prop);
                    byte[] exportp = new byte[4];
                    exportp[0] = 0x03;
                    prop = new CngProperty("Export Policy", exportp, keyOptions);
                    parametersSigningDsa.Parameters.Add(prop);

                    cngKey = CngKey.Create(new CngAlgorithm("DSA"), keyName, parametersSigningDsa);
                }
                else
                    throw new ApplicationException("Key does not exist!");
            else
                cngKey = CngKey.Open(keyName);
            DSAParameters dsaParameters;
            CngPropertyOptions options = new CngPropertyOptions();
            CngProperty property = cngKey.GetProperty("Length", options);
            byte[]? length = property.GetValue();
            int? keyLength = (int?)(length?[1] * 0x100);
            keyLength += length?[0];
            if (keyLength > ModNumber.MaxMod * 8)
                throw new ApplicationException("Keylength not less or equal to MAXMOD!");
            byte[] keyBlob = cngKey.Export(new CngKeyBlobFormat("DSAPRIVATEBLOB"));
#if SMALLMOD
            if (keyBlob[0] != 0x44 && keyBlob[1] != 0x53 && keyBlob[2] != 0x50 && keyBlob[3] != 0x56)
                throw new ApplicationException("Key structure not of type RSA Full Private!");
            int byteLength = keyBlob[5] * 0x100;
            byteLength += keyBlob[4];
            if (byteLength != ModNumber.MaxMod)
                throw new ApplicationException("Keylength not equal to MAXMOD!");
#endif
#if SMALLMOD && UNSAFE
            unsafe
            {
                fixed (byte* pData = &keyBlob[32])
                {
                    byte* p = pData;
                    byte[] qLittleEndian = ModNumber.convertEndianess(p, 20);
                    dsaParameters.Q = new ModNumber(qLittleEndian);
                    p += 20;
                    byte[] pLittleEndian = ModNumber.convertEndianess(p, byteLength);
                    dsaParameters.P = new ModNumber(pLittleEndian);
                    p += byteLength;
                    byte[] gLittleEndian = ModNumber.convertEndianess(p, byteLength);
                    dsaParameters.g = new ModNumber(gLittleEndian);
                    p += byteLength;
                    byte[] yLittleEndian = ModNumber.convertEndianess(p, byteLength);
                    dsaParameters.y = new ModNumber(yLittleEndian);
                    p += byteLength;
                    byte[] xLittleEndian = ModNumber.convertEndianess(p, 20);
                    dsaParameters.x = new ModNumber(xLittleEndian);
                }
            }
#endif
#if SMALLMOD && !UNSAFE
            int offset = 32;
            byte[] qLittleEndian = ModNumber.convertEndianess(keyBlob, 20, offset);
            dsaParameters.Q = new ModNumber(qLittleEndian);
            offset += 20;
            byte[] pLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
            dsaParameters.P = new ModNumber(pLittleEndian);
            offset += byteLength;
            byte[] gLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
            dsaParameters.g = new ModNumber(gLittleEndian);
            offset += byteLength;
            byte[] yLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
            dsaParameters.y = new ModNumber(yLittleEndian);
            offset += byteLength;
            byte[] xLittleEndian = ModNumber.convertEndianess(keyBlob, 20, offset);
            dsaParameters.x = new ModNumber(xLittleEndian);

#endif
#if MEDMOD || LARGEMODSIGNATURE
            if (keyBlob[0] != 0x44 && keyBlob[1] != 0x50 && keyBlob[2] != 0x56 && keyBlob[3] != 0x32)
                throw new ApplicationException("Key structure not of type RSA Full Private!");
            int byteLength = keyBlob[5] * 0x100;
            byteLength += keyBlob[4];
            if (byteLength != ModNumber.MaxMod)
                throw new ApplicationException("Keylength not equal to MAXMOD!");
            int seedLength = keyBlob[17] * 0x100;
            seedLength += keyBlob[16];
            int groupSize = keyBlob[21] * 0x100;
            groupSize += keyBlob[20];
#endif
#if (MEDMOD || LARGEMODSIGNATURE) && UNSAFE
            unsafe
            {
                fixed (byte* pData = &keyBlob[28 + seedLength])
                {
                    byte* p = pData;
                    byte[] qLittleEndian = ModNumber.convertEndianess(p, groupSize);
                    dsaParameters.Q = new ModNumber(qLittleEndian);
                    p += groupSize;
                    byte[] pLittleEndian = ModNumber.convertEndianess(p, byteLength);
                    dsaParameters.P = new ModNumber(pLittleEndian);
                    p += byteLength;
                    byte[] gLittleEndian = ModNumber.convertEndianess(p, byteLength);
                    dsaParameters.g = new ModNumber(gLittleEndian);
                    p += byteLength;
                    byte[] yLittleEndian = ModNumber.convertEndianess(p, byteLength);
                    dsaParameters.y = new ModNumber(yLittleEndian);
                    p += byteLength;
                    byte[] xLittleEndian = ModNumber.convertEndianess(p, groupSize);
                    dsaParameters.x = new ModNumber(xLittleEndian);
                }
            }
#endif
#if (MEDMOD || LARGEMODSIGNATURE) && !UNSAFE
            int offset = 28 + seedLength;
            byte[] qLittleEndian = ModNumber.convertEndianess(keyBlob, groupSize, offset);
            dsaParameters.Q = new ModNumber(qLittleEndian);
            offset += groupSize;
            byte[] pLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
            dsaParameters.P = new ModNumber(pLittleEndian);
            offset += byteLength;
            byte[] gLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
            dsaParameters.g = new ModNumber(gLittleEndian);
            offset += byteLength;
            byte[] yLittleEndian = ModNumber.convertEndianess(keyBlob, byteLength, offset);
            dsaParameters.y = new ModNumber(yLittleEndian);
            offset += byteLength;
            byte[] xLittleEndian = ModNumber.convertEndianess(keyBlob, groupSize, offset);
            dsaParameters.x = new ModNumber(xLittleEndian);

#endif
            return dsaParameters;
        }
        private ModNumber P;
        private ModNumber Q;
        private ModNumber g;
        private ModNumber x;
        private ModNumber y;
    }

    public struct ECPoint
    {
        public ModNumber ?x;
        public ModNumber ?y;
        public bool IsAtInfinity;
        public ECPoint(ECPoint p)
        {
            this.IsAtInfinity = p.IsAtInfinity;
            if (!IsAtInfinity)
            {
                this.x = new ModNumber(p.x!);
                this.y = new ModNumber(p.y!);
            }
            else
            {
                this.x = null;
                this.y = null;
            }
        }
        public static bool operator ==(ECPoint l, ECPoint r)
        {
            if (l.IsAtInfinity && r.IsAtInfinity)
                return true;
            if (l.IsAtInfinity || r.IsAtInfinity)
                return false;
            return l.x! == r.x! && l.y! == r.y!;
        }

        public static bool operator !=(ECPoint l, ECPoint r)
        {
            return !(l == r);
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            ECPoint ecPointObj = (ECPoint)obj!;
            return this == ecPointObj;    
        }
        public override int GetHashCode()
        {
            return x?.GetHashCode() ?? 0 + y?.GetHashCode() ?? 0 + IsAtInfinity.GetHashCode();
        }

    }
    public class EC
    {
        public EC(MultGroupMod mgm, ECPoint g, ModNumber n, ModNumber a, ModNumber b)
        {
            this.mgm = mgm;
            this.g = g;
            this.n = n;
            this.a = a;
            this.b = b;
            this.nLen = n.GetByteCount();
        }
        public bool IsOnCurve(ECPoint p)
        {
            if (p.IsAtInfinity)
                return true;
            ModNumber yKwad = mgm.Kwad(p.y!);
            return yKwad == CalculateRhs(p.x!);
        }
        public ModNumber CalculateRhs(ModNumber x)
        {
            ModNumber xPower3 = mgm.Exp(x, new ModNumber(3ul));
            ModNumber aTimesX = mgm.Mult(x,a);
            ModNumber rhs = mgm.Add(mgm.Add(xPower3, aTimesX), b);
            return rhs;
        }
        public ModNumber CalculateY(ModNumber x)
        {
            return CalculateRhs(x).Sqrt();
        }
        public ECPoint Times2(ECPoint p)
        {
            if (!IsOnCurve(p))
                throw new ArgumentException("The point is not on the curve!");
            if (p.IsAtInfinity)
                return new ECPoint(p);
            ECPoint result = new ECPoint();
            ModNumber mzero = new ModNumber(0ul);
            if (p.y! == mzero)
            {
                result.IsAtInfinity = true;
                return result;
            }
            ModNumber xpSquared = mgm.Kwad(p.x!);
            ModNumber xpSquaredTimes3 = mgm.Mult(xpSquared, new ModNumber(3ul));
            ModNumber xpSquaredTimes3PlusA = mgm.Add(xpSquaredTimes3, a);
            ModNumber ypTimes2 = mgm.Mult(p.y!, new ModNumber(2ul));
            ModNumber ypTimes2Inverse = mgm.Inverse(ypTimes2);
            ModNumber lambda = mgm.Mult(xpSquaredTimes3PlusA, ypTimes2Inverse);
            ModNumber xpTimes2 = mgm.Mult(p.x!, new ModNumber(2ul));
            ModNumber lambdaSquaredMinusXpTimes2 = mgm.Diff(mgm.Kwad(lambda), xpTimes2);
            ModNumber xrMinusXp = mgm.Diff(lambdaSquaredMinusXpTimes2, p.x!);
            ModNumber lambdaTimesXrMinusXp = mgm.Mult(lambda, xrMinusXp);
            ModNumber yPPlusLambdaTimesXrMinusXp = mgm.Add(p.y!, lambdaTimesXrMinusXp);
            result.x = lambdaSquaredMinusXpTimes2;
            result.y = mgm.Diff(mzero, yPPlusLambdaTimesXrMinusXp);
            return result;

        }
        public ECPoint Add(ECPoint p, ECPoint q)
        {
            if (!IsOnCurve(p) || !IsOnCurve(q))
                throw new ArgumentException("One of the points is not on the curve!");
            ECPoint result = new ECPoint();
            if (p == q)
                return Times2(p);
            if (p.IsAtInfinity)
                return new ECPoint(q);
            if (q.IsAtInfinity)
                return new ECPoint(p);
            if (p.x! == q.x!)
            {
                result.IsAtInfinity = true;
                return result;
            }
            ModNumber mzero = new ModNumber(0ul);
            ModNumber xDelta = mgm.Diff(q.x!, p.x!);
            ModNumber yDelta = mgm.Diff(q.y!, p.y!);
            ModNumber xDeltaInverse = mgm.Inverse(xDelta);
            ModNumber lambda = mgm.Mult(yDelta,xDeltaInverse);
            result.x = mgm.Diff(mgm.Diff(mgm.Kwad(lambda), p.x!), q.x!);
            result.y = mgm.Diff(mzero, mgm.Add(p.y!, mgm.Mult(lambda, mgm.Diff(result.x, p.x!))));

            return result;
        }
        public ECPoint Mult(ECPoint p, ModNumber n)
        {
            if (!IsOnCurve(p))
                throw new ArgumentException("The point is not on the curve!");
            if (p.IsAtInfinity)
                return new ECPoint(p);
            ECPoint pCopy = new ECPoint(p);
            ModNumber mzero = new ModNumber(0ul);
            ECPoint result = new ECPoint();
            result.IsAtInfinity = true;
            ulong mask = 1ul;
            while (n > mzero)
            {
                ulong first = n.num[0];
                if ((first & mask) != 0)
                {
                    result = Add(result, pCopy);
                }
                pCopy = Times2(pCopy);
                n >>= 1;
            }
            return result;
        }
        public MultGroupMod mgm;
        public ECPoint g;
        public ModNumber n;
        public ModNumber a;
        public ModNumber b;
        public uint nLen;

    }
    public class ECKeyPair
    {
        public ECKeyPair(EC ec, ModNumber ?mx = null, ECPoint ?y = null)
        {
            this.ec = ec;
            if ((object?)mx != null)
            {
                this.mx = new ModNumber(mx);
            }
            else
                this.mx = new ModNumber(0ul);
            ModNumber mzero = new ModNumber(0ul);
            if (this.mx == mzero)
            {
                ulong[] x = new ulong[ModNumber.LCOUNT];
                uint lSize = ec.nLen / ModNumber.LSIZE;
                Random random = new Random();
                ModNumber mxTmp;
                for (uint i = 0; i < lSize; i++)
                {
                    x[i] = (ulong)random.NextInt64();
                }
                mxTmp = new ModNumber(x);
                if (mxTmp == mzero)
                    mxTmp += (uint)random.NextInt64() + 1;
                while (mxTmp >= ec.n)
                {
                    mxTmp.num[lSize - 1] -= (ulong)random.NextInt64();
                }
                this.mx = mxTmp;
            }
            if (y == null)
                this.y = CalcPublicKey(this.mx);
            else
                this.y = (ECPoint)y!;

        }
        public ECPoint CalcPublicKey(ModNumber x)
        {
            ECPoint retval = ec.Mult(ec.g, x);
            return retval;
        }
        public ECDsaCng ImportECKeyPair(string curveName = "secP256k1")
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
        public static ModNumber CalculateECDHSharedSecret(ECKeyPair pair1, ECKeyPair pair2)
        {
            ModularCalculation.ECPoint resultPt = pair1.ec.Mult(pair1.y, pair2.mx);
            if (resultPt.IsAtInfinity)
                throw new ArgumentException("Invalid keypair combination");
            return resultPt.x!;
        }
        public static byte[] GetSecretECDHAgreement(ECKeyPair pair1, ECKeyPair pair2, string curveName, byte[] magicNr )
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
        public EC ec;
        public ECPoint y;
        public ModNumber mx;
    }
    public class ECDSA : DSABase
    {
        public ECDSA (ECKeyPair keyPair)
        {
            this.ecKeyPair = keyPair;
        }
        public ECKeyPair ecKeyPair;
        public override ModNumber CalcR(ModNumber mk)
        {
            ECPoint ecPoint = ecKeyPair.ec.Mult(ecKeyPair.ec.g, mk);
            if (ecPoint.IsAtInfinity)
                throw new ArgumentException("Wrong parameters!");
            return ecPoint.x! % ecKeyPair.ec.n;
        }
        public string Sign(byte[] hash, bool DEREncoded)
        {
            byte [] signature =  CalculateDSASignature(ecKeyPair.ec.n, ecKeyPair.mx, hash, DEREncoded);
            return ConvertSignatureToString(signature, DEREncoded);
        }
        public byte[] Sign(byte[] hash)
        {
            byte[] signature = CalculateDSASignature(ecKeyPair.ec.n, ecKeyPair.mx, hash, false);
            return signature;
        }

        public bool Verify(byte [] hash, byte[] signature)
        {
            byte[] r = new byte[signature.Length/2];
            byte[] s = new byte[signature.Length/2];
            for (int i = 0; i < signature.Length/2; i++)
                r[i] = signature[i];
            for (int i = 0; i < signature.Length / 2; i++)
                s[i] = signature[i + r.Length];
            (ModNumber u1, ModNumber u2, ModNumber mr) = DSA.DSACalculateU1U2Mr(ecKeyPair.ec.n, hash, r, s);
            ModularCalculation.ECPoint ptv = CalculateV(hash, u1, u2, mr);
            if (ptv.IsAtInfinity)
                return false;
            return ptv.x! == mr;
        }
        public bool Verify(byte[] hash, string signature, bool DEREncoded = true)
        {
            (ModNumber u1, ModNumber u2, ModNumber mr) = DSA.DSACalculateU1U2Mr(ecKeyPair.ec.n, hash, signature, DEREncoded);
            ModularCalculation.ECPoint ptv = CalculateV(hash, u1, u2, mr);
            if (ptv.IsAtInfinity)
                return false;
            return ptv.x! == mr;
        }
        public  ModularCalculation.ECPoint CalculateV(byte[] hash, ModNumber u1, ModNumber u2, ModNumber mr)
        {
            ModularCalculation.ECPoint? pt1 = null;
            ModularCalculation.ECPoint? pt2 = null;
            Thread th1 = new Thread(() => { pt1 = ecKeyPair.ec.Mult(ecKeyPair.ec.g, u1); });
            th1.Start();
            Thread th2 = new Thread(() => { pt2 = ecKeyPair.ec.Mult(ecKeyPair.y, u2); });
            th2.Start();
            th1.Join();
            th2.Join();
            ECPoint ptv = ecKeyPair.ec.Add((ECPoint)pt1!, (ECPoint)pt2!);
            return ptv;
        }
    }
}