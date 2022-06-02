namespace Application;

public static class World
{
    public static List<Body> InitialBodies { get; set; } = new();
    public static List<Body> Bodies { get; } = new();
    private static List<Task> _tasks = new();

    public static void Reset()
    {
        Bodies.Clear();
        foreach (var body in InitialBodies)
            Bodies.Add(new Body(body.Position, body.Velocity, body.Mass, body.Radius));
        _tasks.Capacity = InitialBodies.Count;
    }

    public static void Update(float timeStep)
    {
        _tasks.Clear();
        var bodiesArr = Bodies.ToArray();
        foreach (var body in Bodies)
            _tasks.Add(Task.Run(() => body.UpdateVelocity(bodiesArr, timeStep)));
        Task.WaitAll(_tasks.ToArray());

        foreach (var body in Bodies)
            body.UpdatePosition(timeStep);
    }
}