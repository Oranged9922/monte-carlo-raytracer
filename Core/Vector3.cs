namespace Core;

public class Vector3(float x, float y, float z)
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;
    public float Z { get; set; } = z;

    // Vector addition
    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    // Vector subtraction
    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    // Negation
    public static Vector3 operator -(Vector3 v)
    {
        return new Vector3(-v.X, -v.Y, -v.Z);
    }


    // Vector scaling (multiplication by a scalar)
    public static Vector3 operator *(Vector3 v, float scalar)
    {
        return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
    }

    public static Vector3 operator *(float scalar, Vector3 v)
    {
        return v * scalar;
    }

    // Vector division by a scalar
    public static Vector3 operator /(Vector3 v, float scalar)
    {
        return new Vector3(v.X / scalar, v.Y / scalar, v.Z / scalar);
    }

    // Dot product
    public static float Dot(Vector3 a, Vector3 b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    }

    // Cross product
    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);
    }

    // Normalize the vector
    public Vector3 Normalize()
    {
        float length = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        return this / length;
    }

    // Length of the vector
    public float Length()
    {
        return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    // Squared length of the vector (useful for performance-sensitive code)
    public float LengthSquared()
    {
        return X * X + Y * Y + Z * Z;
    }

    // ToString method for easy debugging
    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}