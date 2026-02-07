using System.Numerics;

namespace KnotVisualizer.Models;

public enum KnotType
{
    Trefoil,
    FigureEight,
    CinquefoilKnot,
    TorusKnot,
    Lissajous,
    Celtic
}

public class KnotParameters
{
    public KnotType Type { get; set; } = KnotType.Trefoil;
    public double TubeRadius { get; set; } = 0.3;
    public int P { get; set; } = 2;
    public int Q { get; set; } = 3; 

    // Lissajous parameters
    public int LissajousNx { get; set; } = 3;
    public int LissajousNy { get; set; } = 2;
    public int LissajousNz { get; set; } = 4;

    // Celtic / Star parameters
    public int StarPoints { get; set; } = 5; // P
    public int StarSkip { get; set; } = 2;   // Q
    public double Scale { get; set; } = 3.0;
    public int Resolution { get; set; } = 200;
    public int TubularSegments { get; set; } = 20;
}

public class KnotGeometry
{
    public static Vector3 GetKnotPosition(KnotParameters p, double t)
    {
        return p.Type switch
        {
            KnotType.Trefoil => GetTrefoilPosition(t, p.Scale),
            KnotType.FigureEight => GetFigureEightPosition(t, p.Scale),
            KnotType.CinquefoilKnot => GetCinquefoilPosition(t, p.Scale),
            KnotType.TorusKnot => GetTorusKnotPosition(t, p.Scale, p.P, p.Q),
            KnotType.Lissajous => GetLissajousPosition(t, p.Scale, p.LissajousNx, p.LissajousNy, p.LissajousNz),
            KnotType.Celtic => GetCelticPosition(t, p.Scale, p.StarPoints, p.StarSkip),
            _ => Vector3.Zero
        };
    }
    
    // Legacy helper for refactoring
    private static Vector3 GetKnotPosition(KnotType type, double t, double scale, int p, int q) 
    {
        return Vector3.Zero;
    }

    private static Vector3 GetTrefoilPosition(double t, double scale)
    {

        double x = Math.Sin(t) + 2 * Math.Sin(2 * t);
        double y = Math.Cos(t) - 2 * Math.Cos(2 * t);
        double z = -Math.Sin(3 * t);
        
        return new Vector3((float)(x * scale), (float)(y * scale), (float)(z * scale));
    }

    private static Vector3 GetFigureEightPosition(double t, double scale)
    {

        double x = (2 + Math.Cos(2 * t)) * Math.Cos(3 * t);
        double y = (2 + Math.Cos(2 * t)) * Math.Sin(3 * t);
        double z = Math.Sin(4 * t);
        
        return new Vector3((float)(x * scale * 0.6), (float)(y * scale * 0.6), (float)(z * scale));
    }

    private static Vector3 GetCinquefoilPosition(double t, double scale)
    {

        double x = Math.Sin(t) + 2 * Math.Sin(2 * t);
        double y = Math.Cos(t) - 2 * Math.Cos(2 * t);
        double z = -Math.Sin(5 * t);
        
        return new Vector3((float)(x * scale), (float)(y * scale), (float)(z * scale));
    }

    private static Vector3 GetTorusKnotPosition(double t, double scale, int p, int q)
    {

        double r = 2;
        double R = 3;
        
        double x = (R + r * Math.Cos(p * t)) * Math.Cos(q * t);
        double y = (R + r * Math.Cos(p * t)) * Math.Sin(q * t);
        double z = r * Math.Sin(p * t);
        
        return new Vector3((float)(x * scale * 0.5), (float)(y * scale * 0.5), (float)(z * scale));

    }

    private static Vector3 GetLissajousPosition(double t, double scale, int nx, int ny, int nz)
    {
        // 3D Lissajous curve
        double x = Math.Sin(nx * t);
        double y = Math.Sin(ny * t + Math.PI / 2); // Phase shift Y
        double z = Math.Sin(nz * t);
        
        return new Vector3((float)(x * scale), (float)(y * scale), (float)(z * scale));
    }

    private static Vector3 GetCelticPosition(double t, double scale, int p, int q)
    {
        // "Celtic" Star Knot / Torus variation
        // Using a modulated torus knot to create a woven star effect
        double r = 1.5 + 0.5 * Math.Cos(q * t); // Modulated radius
        double R = 3;
        
        double x = (R + r * Math.Cos(p * t)) * Math.Cos(t);
        double y = (R + r * Math.Cos(p * t)) * Math.Sin(t);
        double z = r * Math.Sin(p * t) + 1.0 * Math.Sin(q * t); // Added vertical modulation
        
        return new Vector3((float)(x * scale * 0.4), (float)(y * scale * 0.4), (float)(z * scale * 0.4));
    }

    public static Vector3 GetTangent(KnotParameters p, double t, double dt = 0.001)
    {
        Vector3 p1 = GetKnotPosition(p, t - dt);
        Vector3 p2 = GetKnotPosition(p, t + dt);
        return Vector3.Normalize(p2 - p1);
    }
    
    public static (Vector3 normal, Vector3 binormal) GetFrenetFrame(KnotParameters p, double t)
    {
        Vector3 tangent = GetTangent(p, t);
        

        Vector3 arbitrary = Math.Abs(tangent.Y) < 0.9f ? new Vector3(0, 1, 0) : new Vector3(1, 0, 0);
        Vector3 binormal = Vector3.Normalize(Vector3.Cross(tangent, arbitrary));
        Vector3 normal = Vector3.Cross(binormal, tangent);
        
        return (normal, binormal);
    }

    public static List<Vector3> GenerateTubeVertices(KnotParameters parameters)
    {
        var vertices = new List<Vector3>();
        int steps = parameters.Resolution;
        int radialSegments = parameters.TubularSegments;
        
        for (int i = 0; i <= steps; i++)
        {
            double t = 2 * Math.PI * i / steps;

            Vector3 position = GetKnotPosition(parameters, t);
            var (normal, binormal) = GetFrenetFrame(parameters, t);
            
            for (int j = 0; j <= radialSegments; j++)
            {
                double angle = 2 * Math.PI * j / radialSegments;
                double x = parameters.TubeRadius * Math.Cos(angle);
                double y = parameters.TubeRadius * Math.Sin(angle);
                
                Vector3 offset = (float)x * normal + (float)y * binormal;
                vertices.Add(position + offset);
            }
        }
        
        return vertices;
    }

    public static List<int> GenerateTubeIndices(int steps, int radialSegments)
    {
        var indices = new List<int>();
        
        for (int i = 0; i < steps; i++)
        {
            for (int j = 0; j < radialSegments; j++)
            {
                int a = i * (radialSegments + 1) + j;
                int b = a + radialSegments + 1;
                int c = a + 1;
                int d = b + 1;
                
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                
                indices.Add(c);
                indices.Add(b);
                indices.Add(d);
            }
        }
        
        return indices;
    }
}
