﻿using System.Numerics;
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
        UI.UiManager.Setup();

        // Create some bodies
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
        var frustum = new Frustum();

        while (!Raylib.WindowShouldClose())
        {
            HandleInput(ref Playing, camera);
            UI.UiManager.Update();

            // Update bodies
            if (Playing) World.Update(Raylib.GetFrameTime() / 1000);

            Raylib.UpdateCamera(ref camera);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            Raylib.BeginMode3D(camera);
            frustum.UpdatePlanes();
            foreach (var body in World.Bodies)
            {
                if (frustum.SphereInFrustum(body.Position * 100, body.Radius))
                    Raylib.DrawModel(bodyModel, body.Position * 100, body.Radius, body.Color);
            }

            if (World.SelectedIndex != -1)
            {
                var body = World.Bodies[World.SelectedIndex];
                if (frustum.SphereInFrustum(body.Position * 100, body.Radius))
                    Raylib.DrawModelWires(bodyModel, body.Position * 100, body.Radius + 0.1f, Color.WHITE);
            }

            Raylib.EndMode3D();

            UI.UiManager.Render();

            Raylib.DrawFPS(0, 0);
            Raylib.EndDrawing();
        }

        UI.UiManager.Shutdown();
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