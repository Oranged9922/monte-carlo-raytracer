using Core;
using System.Collections.Generic;

public class HittableList : IHittable
{
    private List<IHittable> _objects;

    public HittableList()
    {
        _objects = new List<IHittable>();
    }

    public void Add(IHittable obj)
    {
        _objects.Add(obj);
    }

    public void Clear()
    {
        _objects.Clear();
    }

    public bool Hit(Ray ray, float tMin, float tMax, out HitRecord hitRecord)
    {
        hitRecord = new HitRecord();
        bool hitAnything = false;
        float closestSoFar = tMax;

        foreach (var obj in _objects)
        {
            if (obj.Hit(ray, tMin, closestSoFar, out HitRecord tempRec))
            {
                hitAnything = true;
                closestSoFar = tempRec.T;
                hitRecord = tempRec;
            }
        }

        return hitAnything;
    }
}
