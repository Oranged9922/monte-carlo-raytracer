using Core;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Image
        const float aspectRatio = 16.0f / 9.0f;
        const int imageWidth = 1920;
        const int imageHeight = (int)(imageWidth / aspectRatio);
        const int samplesPerPixel = 50; // For anti-aliasing
        const int maxDepth = 2; // Max recursion depth for rays

        // World
        HittableList world = new HittableList();
        world.Add(new Sphere(new Vector3(0, 0, -1), 0.5f));
        world.Add(new Sphere(new Vector3(0, 0, -2), 0.5f));
        world.Add(new Sphere(new Vector3(1.5f, 0, -1), 0.5f));
        world.Add(new Sphere(new Vector3(0, -100.5f, -1), 100));

        // Camera
        // Assuming a focus distance of 1.0 for simplicity, and no aperture effect
        float focusDist = 1.0f; // This should ideally be the distance between lookfrom and lookat for accurate depth of field simulation
        float aperture = 0.0f; // No aperture effect

        Camera camera = new Camera(new Vector3(-2, 2, 1), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90, aspectRatio, aperture, focusDist);

        // Render

        // Image buffer to store colors
        Vector3[,] imageBuffer = new Vector3[imageHeight, imageWidth];

        // Use Parallel.For to distribute the rows across multiple threads
        Parallel.For(0, imageHeight, j =>
        {
            Console.WriteLine($"Processing scanline {j} on thread {Thread.CurrentThread.ManagedThreadId}");
            for (int i = 0; i < imageWidth; i++)
            {
                Vector3 pixelColor = new Vector3(0, 0, 0);
                for (int s = 0; s < samplesPerPixel; s++)
                {
                    float u = (i + Random.Shared.NextSingle()) / (imageWidth - 1);
                    float v = (j + Random.Shared.NextSingle()) / (imageHeight - 1);
                    Ray ray = camera.GetRay(u, v);
                    pixelColor += RayColor(ray, world, maxDepth);
                }
                imageBuffer[j, i] = pixelColor / samplesPerPixel; // Store the averaged color
            }
        });

        // Now write the imageBuffer to the file, in order
        using (StreamWriter sw = new StreamWriter($"image_samples-{samplesPerPixel}_depth-{maxDepth}.ppm"))
        {
            sw.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");
            for (int j = imageHeight-1 ; j > 0; j--)
            {
                for (int i = 0; i < imageWidth; i++)
                {
                    Vector3 pixelColor = imageBuffer[j, i];
                    // Apply gamma correction and clamp
                    pixelColor = new Vector3((float)Math.Sqrt(pixelColor.X), (float)Math.Sqrt(pixelColor.Y), (float)Math.Sqrt(pixelColor.Z));
                    int ir = (int)(256 * Clamp(pixelColor.X, 0.0f, 0.999f));
                    int ig = (int)(256 * Clamp(pixelColor.Y, 0.0f, 0.999f));
                    int ib = (int)(256 * Clamp(pixelColor.Z, 0.0f, 0.999f));
                    sw.WriteLine($"{ir} {ig} {ib}");
                }
            }
        }
        Console.WriteLine("Done.");
    }

    static Vector3 RayColor(Ray r, HittableList world, int depth)
    {
        // If we've exceeded the ray bounce limit, no more light is gathered.
        if (depth <= 0)
            return new Vector3(0, 0, 0);

        HitRecord rec;
        if (world.Hit(r, 0.001f, float.MaxValue, out rec))
        {
            Vector3 target = rec.Point + rec.Normal + RandomInUnitSphere();
            return 0.5f * RayColor(new Ray(rec.Point, target - rec.Point), world, depth - 1);
        }

        Vector3 unitDirection = r.Direction.Normalize();
        float t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
    }

    static Vector3 RandomInUnitSphere()
    {
        while (true)
        {
            var p = new Vector3(
                (float)(Random.Shared.NextDouble() * 2 - 1), // Generate a random X between -1.0 and 1.0
                (float)(Random.Shared.NextDouble() * 2 - 1), // Generate a random Y between -1.0 and 1.0
                (float)(Random.Shared.NextDouble() * 2 - 1)  // Generate a random Z between -1.0 and 1.0
            );
            if (p.LengthSquared() >= 1) continue; // Check if the point is inside the unit sphere
            return p; // Return the valid point
        }
    }


    static void WriteColor(StreamWriter sw, Vector3 pixelColor, int samplesPerPixel)
    {
        // Divide the color by the number of samples and gamma-correct for gamma=2.0.
        float scale = 1.0f / samplesPerPixel;
        float r = (float)Math.Sqrt(scale * pixelColor.X);
        float g = (float)Math.Sqrt(scale * pixelColor.Y);
        float b = (float)Math.Sqrt(scale * pixelColor.Z);

        // Write the translated [0,255] value of each color component.
        sw.WriteLine($"{(int)(256 * Clamp(r, 0, 0.999f))} {(int)(256 * Clamp(g, 0, 0.999f))} {(int)(256 * Clamp(b, 0, 0.999f))}");
    }

    static float Clamp(float x, float min, float max)
    {
        if (x < min) return min;
        if (x > max) return max;
        return x;
    }
}
