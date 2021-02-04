/// <summary>
/// Structure for defining angles agnostic of representation
/// </summary>
public readonly struct Angle
{
    public float Radians { get; }
    public float Degrees { get; }

    private const float Rad2Deg = 57.29578f;
    private const float Deg2Rad = 0.01745329f;

    private Angle(float radians)
    {
        Radians = radians;
        Degrees = radians * Rad2Deg;
    }
    
    public static Angle FromDegrees(float degrees)
    {
        return new Angle(degrees * Deg2Rad);
    }
    
    public static Angle FromRadians(float radians)
    {
        return new Angle(radians);
    }

    #region Operators
    
    public static Angle operator *(Angle a, float f)
    {
        return new Angle(a.Radians * f);
    }
    
    public static Angle operator -(Angle a, float f)
    {
        return new Angle(a.Radians - f);
    }
    
    public static Angle operator +(Angle a, float f)
    {
        return new Angle(a.Radians + f);
    }
    
    public static Angle operator /(Angle a, float f)
    {
        return new Angle(a.Radians / f);
    }
    
    public static Angle operator -(Angle a)
    {
        return new Angle(-a.Radians);
    }
    
    public static implicit operator float(Angle d)
    {
        return d.Radians;
    }

    #endregion
    
}
