using System.Numerics;
using Raylib_cs;

namespace Application;

public static class World
{
    public static List<Body> InitialBodies { get; set; } = new();
    public static List<Body> Bodies { get; } = new();
    public static int SelectedIndex { get; set; } = -1;
    private static readonly List<Task> Tasks = new();
    private static readonly Random Rnd = new();

    public static void Reset()
    {
        Bodies.Clear();
        foreach (var body in InitialBodies)
            Bodies.Add(new Body(body.Position, body.Velocity, body.Mass, body.Radius, body.Color));
        Tasks.Capacity = InitialBodies.Count;
    }

    public static void Update(float timeStep)
    {
        Tasks.Clear();
        var bodiesArr = Bodies.ToArray();
        foreach (var body in Bodies)
            Tasks.Add(Task.Run(() => body.UpdateVelocity(bodiesArr, timeStep)));
        Task.WaitAll(Tasks.ToArray());

        foreach (var body in Bodies)
            body.UpdatePosition(timeStep);
    }

    public static void GenerateBodies(int bodyCount)
    {
        for (var i = 0; i < bodyCount; i++)
        {
            var direction = new Vector3(Rnd.NextSingle(), Rnd.NextSingle(), Rnd.NextSingle());
            var scaledNormalDirection = Vector3.Normalize(direction * 2 - Vector3.One);
            var position = scaledNormalDirection * Rnd.NextSingle();
            var velocity = new Vector3(Rnd.NextSingle(), Rnd.NextSingle(), Rnd.NextSingle());
            var mass = Rnd.Next(1, 100);
            var radius = Math.Clamp(Rnd.NextSingle() * 2, 0.2f, 2f);
            var color = new Color(Rnd.Next(0, 256), Rnd.Next(0, 256), Rnd.Next(0, 256), 255);
            InitialBodies.Add(new Body(position, velocity, mass, radius, color));
        }
    }
}