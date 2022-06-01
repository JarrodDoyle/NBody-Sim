namespace Application;

public static class World
{
    public static List<Body> InitialBodies { get; set; } = new();
    public static List<Body> Bodies { get; } = new();

    public static void Reset()
    {
        Bodies.Clear();
        foreach (var body in InitialBodies)
            Bodies.Add(new Body(body.Position, body.Velocity, body.Mass, body.Radius));
    }

    public static void Update(float timeStep)
    {
        foreach (var body in Bodies)
            body.UpdateVelocity(Bodies, timeStep);
        foreach (var body in Bodies)
            body.UpdatePosition(timeStep);
    }
}