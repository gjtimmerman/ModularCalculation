package modularcalculation;

public enum ASNElementType
{
INTEGER_VALUE(0x02),
OCTET_STRING(0x04),
NULL_VALUE(0x05),
OBJECT_IDENTIFIER(0x06),
SEQUENCE(0x10);

    private int elementNumber;
    ASNElementType(int elementNumber)
    {
        this.elementNumber = elementNumber;
    }

    public int getElementNumber()
    {
        return elementNumber;
    }
    public static ASNElementType fromByte(byte t)
    {
        switch (t)
        {
            case 0x02:
                return INTEGER_VALUE;
            case 0x04:
                return OCTET_STRING;
            case 0x05:
                return NULL_VALUE;
            case 0x06:
                return OBJECT_IDENTIFIER;
            case 0x10:
                return SEQUENCE;
            default:
                throw new IllegalArgumentException("Illegal ASNElementtype");
        }
    }
}

