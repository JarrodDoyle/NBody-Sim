using System.Numerics;
using Application.UI;
using Raylib_cs;
using ImGuiNET;

namespace Application;

internal static class Program
{
    public static bool Playing;
    
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
        var uiLayers = new List<BaseUiLayer>
        {
            new SimControllerLayer {Open = true},
            new BodyEditorLayer()
        };
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
            var radius = Math.Clamp(rnd.NextSingle() * 2, 0.2f, 2f);
            var color = new Color(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256), 255);
            World.InitialBodies.Add(new Body(position, velocity, mass, radius, color));
        }

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

        var bodyModel = Raylib.LoadModelFromMesh(Raylib.GenMeshSphere(1, 16, 16));

        while (!Raylib.WindowShouldClose())
        {
            HandleInput(ref Playing, camera);

            foreach (BaseUiLayer layer in uiLayers)
                layer.Update();

            // Update bodies
            if (Playing) World.Update(Raylib.GetFrameTime() / 1000);

            Raylib.UpdateCamera(ref camera);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            Raylib.BeginMode3D(camera);
            foreach (var body in World.Bodies)
                Raylib.DrawModel(bodyModel, body.Position * 100, body.Radius, body.Color);

            if (World.SelectedIndex != -1)
            {
                var body = World.Bodies[World.SelectedIndex];
                Raylib.DrawModelWires(bodyModel, body.Position * 100, body.Radius + 0.1f, Color.WHITE);
            }

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

    private static void HandleInput(ref bool playing, Camera3D camera)
    {
        var io = ImGui.GetIO();
        if (io.WantCaptureMouse || io.WantCaptureKeyboard) return;

        if (Raylib.IsKeyPressed(KeyboardKey.KEY_R)) World.Reset();
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE)) playing = !playing;
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            World.SelectedIndex = -1;
            var ray = Raylib.GetMouseRay(Raylib.GetMousePosition(), camera);
            var dist = float.MaxValue;
            for (var i = 0; i < World.Bodies.Count; i++)
            {
                var body = World.Bodies[i];
                var collision = Raylib.GetRayCollisionSphere(ray, body.Position * 100, body.Radius);
                if (collision.hit && collision.distance < dist)
                {
                    dist = collision.distance;
                    World.SelectedIndex = i;
                }
            }
        }
    }
}