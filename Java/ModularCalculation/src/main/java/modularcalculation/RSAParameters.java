package modularcalculation;

public class RSAParameters
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
