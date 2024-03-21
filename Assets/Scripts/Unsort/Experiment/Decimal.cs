using UnityEngine;

[System.Serializable]
public struct Decimal
{
    public int value; // Internal representation, multiplied by 100 to keep 2 decimal places

    public Decimal(int intValue)
    {
        value = intValue * 100;
    }

    public Decimal(float floatValue)
    {
        value = Mathf.RoundToInt(floatValue * 100f);
    }

    public readonly float ToFloat()
    {
        return (float)value / 100;
    }

    public static Decimal operator +(Decimal a, Decimal b)
    {
        return new Decimal(a.value + b.value);
    }

    public static Decimal operator -(Decimal a, Decimal b)
    {
        return new Decimal(a.value - b.value);
    }

    public static Decimal operator *(Decimal a, Decimal b)
    {
        return new Decimal((int)((long)a.value * b.value / 100));
    }

    public static Decimal operator /(Decimal a, Decimal b)
    {
        return new Decimal((int)((long)a.value * 100 / b.value));
    }

    // Additional methods can be added as needed

    public override readonly string ToString()
    {
        return ToFloat().ToString("0.00");
    }

    // You can add other methods or conversions as needed
}
