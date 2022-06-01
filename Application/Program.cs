using System.Numerics;
using Application.UI;
using Raylib_cs;
using ImGuiNET;

namespace Application;

internal static class Program
{
    private static void InitWindow(int width, int height, string title)
    {
        Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT |
                              ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.SetTraceLogLevel(TraceLogLevel.LOG_WARNING);
        Raylib.InitWindow(width, height, title);
        Raylib.SetWindowMinSize(640, 480);
    }

    private static void Main()
    {
        InitWindow(1280, 720, "Raylib + Dear ImGui app");

        ImGuiController.Setup();
        var uiLayers = new List<BaseUiLayer> {new SimControllerLayer {Open = true}};
        foreach (BaseUiLayer layer in uiLayers)
            layer.Attach();

        // Create some bodies
        var rnd = new Random();
        var numBodies = 1000;
        for (int i = 0; i < numBodies; i++)
        {
            var position = new Vector3(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
            var velocity = new Vector3(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
            var mass = rnd.Next(1, 100);
            var radius = 1;
            World.InitialBodies.Add(new Body(position, velocity, mass, radius));
        }
        // World.InitialBodies.Add(new Body(Vector3.Zero, Vector3.Zero, 100, 2));
        // World.InitialBodies.Add(new Body(new Vector3(0, 0, 2), new Vector3(0, 7.5f, 0), 1, 1));
        World.Reset();

        // Camera woo!
        var camera = new Camera3D
        {
            position = new Vector3(-10, 0, 0),
            target = Vector3.Zero,
            up = Vector3.UnitY,
            fovy = 60,
            projection = CameraProjection.CAMERA_PERSPECTIVE
        };
        Raylib.SetCameraMode(camera, CameraMode.CAMERA_FREE);

        var playing = false;

        while (!Raylib.WindowShouldClose())
        {
            foreach (BaseUiLayer layer in uiLayers)
                layer.Update();

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE)) playing = !playing;

            // Update bodies
            if (playing) World.Update(Raylib.GetFrameTime() / 1000);

            Raylib.UpdateCamera(ref camera);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            Raylib.BeginMode3D(camera);
            foreach (var body in World.Bodies)
                Raylib.DrawSphere(body.Position * 10, body.Radius, Color.RED);
            Raylib.EndMode3D();

            ImGuiController.Begin();
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);
            foreach (BaseUiLayer layer in uiLayers)
                layer.Render();
            ImGuiController.End();

            Raylib.DrawFPS(0, 0);
            Raylib.EndDrawing();
        }

        ImGuiController.Shutdown();
        Raylib.CloseWindow();
    }
}