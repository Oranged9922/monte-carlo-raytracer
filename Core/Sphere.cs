using Core;
public class Sphere : IHittable
{
    public Vector3 Center { get; }
    public float Radius { get; }

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public bool Hit(Ray r, float tMin, float tMax, out HitRecord rec)
    {
        rec = new HitRecord();
        Vector3 oc = r.Origin - Center;
        float a = r.Direction.LengthSquared();
        float half_b = Vector3.Dot(oc, r.Direction);
        float c = oc.LengthSquared() - Radius * Radius;
        float discriminant = half_b * half_b - a * c;

        if (discriminant > 0)
        {
            float root = (float)Math.Sqrt(discriminant);
            float temp = (-half_b - root) / a;
            if (temp < tMax && temp > tMin)
            {
                rec.T = temp;
                rec.Point = r.At(rec.T);
                Vector3 outwardNormal = (rec.Point - Center) / Radius;
                rec.SetFaceNormal(r, outwardNormal);
                return true;
            }
            temp = (-half_b + root) / a;
            if (temp < tMax && temp > tMin)
            {
                rec.T = temp;
                rec.Point = r.At(rec.T);
                Vector3 outwardNormal = (rec.Point - Center) / Radius;
                rec.SetFaceNormal(r, outwardNormal);
                return true;
            }
        }
        return false;
    }
}
