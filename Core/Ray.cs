namespace Core;
public class Ray
{
    public Vector3 Origin { get; }
    public Vector3 Direction { get; }

    public Ray(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction.Normalize(); // Ensuring the direction is normalized
    }

    // Compute a point along the ray at parameter t
    public Vector3 At(float t)
    {
        return Origin + t * Direction;
    }
}
