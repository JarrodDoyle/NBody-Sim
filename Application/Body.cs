using System.Numerics;

namespace Application;

public class Body
{
    public Vector3 Position { get; set; }
    public Vector3 Velocity { get; set; }
    public float Mass { get; set; }
    public float Radius { get; set; }

    private static readonly float _g = 1f;

    public Body(Vector3 position, Vector3 velocity, float mass, float radius)
    {
        Position = position;
        Velocity = velocity;
        Mass = mass;
        Radius = radius;
    }

    public void UpdateVelocity(IEnumerable<Body> bodies, float timeStep)
    {
        foreach (var other in bodies)
        {
            if (other == this) continue;
            
            var dst = other.Position - Position;
            Velocity += timeStep * _g * Vector3.Normalize(dst) * other.Mass / dst.LengthSquared();
        }
    }

    public void UpdatePosition(float timeStep)
    {
        Position += Velocity * timeStep;
    }
}