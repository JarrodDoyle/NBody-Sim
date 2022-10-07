using System.Numerics;

namespace Application;

public class Body
{
    public Vector3 Position { get; set; }
    public Vector3 Velocity { get; set; }
    public float Mass { get; set; }
    public float Radius { get; set; }
    public Raylib_cs.Color Color { get; set; }

    private static readonly float _g = 1f;
    private static readonly float _e2 = 0.0001f;

    public Body(Vector3 position, Vector3 velocity, float mass, float radius, Raylib_cs.Color color)
    {
        Position = position;
        Velocity = velocity;
        Mass = mass;
        Radius = radius;
        Color = color;
    }

    public void UpdateVelocity(Body[] bodies, float timeStep)
    {
        var totalForce = Vector3.Zero;
        foreach (var other in bodies)
        {
            if (other == this) continue;
            var dst = other.Position - Position;
            totalForce += other.Mass * dst / (dst.Length() * dst.LengthSquared());
        }

        var accel = totalForce * _g;
        Velocity += accel * timeStep;
    }

    public void UpdatePosition(float timeStep)
    {
        Position += Velocity * timeStep;
    }
}