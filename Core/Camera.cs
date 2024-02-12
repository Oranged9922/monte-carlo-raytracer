namespace Core;

public class Camera
{
    public Vector3 Origin { get; }
    public Vector3 LowerLeftCorner { get; }
    public Vector3 Horizontal { get; }
    public Vector3 Vertical { get; }
    private Vector3 u, v, w;
    private float lensRadius;

    public Camera(Vector3 lookfrom, Vector3 lookat, Vector3 vup, float vfov, float aspectRatio, float aperture, float focusDist)
    {
        var theta = DegreesToRadians(vfov);
        var h = (float)Math.Tan(theta / 2);
        var viewportHeight = 2.0f * h;
        var viewportWidth = aspectRatio * viewportHeight;

        w = (lookfrom - lookat).Normalize();
        u = Vector3.Cross(vup, w).Normalize();
        v = Vector3.Cross(w, u);

        Origin = lookfrom;
        Horizontal = focusDist * viewportWidth * u;
        Vertical = focusDist * viewportHeight * v;
        LowerLeftCorner = Origin - Horizontal / 2 - Vertical / 2 - focusDist * w;

        lensRadius = aperture / 2;
    }

    public Ray GetRay(float s, float t)
    {
        Vector3 rd = lensRadius * RandomInUnitDisk();
        Vector3 offset = u * rd.X + v * rd.Y;
        return new Ray(Origin + offset, LowerLeftCorner + s * Horizontal + t * Vertical - Origin - offset);
    }

    private float DegreesToRadians(float degrees)
    {
        return (float)(degrees * Math.PI / 180.0);
    }

    private Vector3 RandomInUnitDisk()
    {
        while (true)
        {
            var p = new Vector3((float)(new Random().NextDouble() * 2 - 1), (float)(new Random().NextDouble() * 2 - 1), 0);
            if (p.LengthSquared() >= 1) continue;
            return p;
        }
    }
}
