using System.Numerics;
using ImGuiNET;

namespace Application.UI;

public class BodyEditorLayer : BaseUiLayer
{
    public override void Attach()
    {
    }

    public override void Detach()
    {
    }

    public override void Render()
    {
        if (!Open) return;

        ImGui.Begin("Body Editor");

        var initialBody = World.InitialBodies[World.SelectedIndex];
        var liveBody = World.Bodies[World.SelectedIndex];
        
        var initialPosition = initialBody.Position;
        if (ImGui.InputFloat3("Initial Position", ref initialPosition))
            initialBody.Position = initialPosition;

        var initialVelocity = initialBody.Velocity;
        if (ImGui.InputFloat3("Initial Velocity", ref initialVelocity))
            initialBody.Velocity = initialVelocity;
        
        var livePosition = liveBody.Position;
        if (ImGui.InputFloat3("Initial Position", ref livePosition))
            liveBody.Position = livePosition;

        var liveVelocity = liveBody.Velocity;
        if (ImGui.InputFloat3("Initial Velocity", ref liveVelocity))
            liveBody.Velocity = liveVelocity;

        var mass = initialBody.Mass;
        if (ImGui.DragFloat("Mass", ref mass, 1, 1000))
        {
            initialBody.Mass = mass;
            liveBody.Mass = mass;
        }
        
        var radius = initialBody.Radius;
        if (ImGui.DragFloat("Radius", ref radius, 1, 1000))
        {
            initialBody.Radius = radius;
            liveBody.Radius = radius;
        }

        var color = initialBody.Color;
        var colorVec3 = new Vector3(color.r, color.g, color.b) / 255;
        if (ImGui.ColorEdit3("Color", ref colorVec3))
        {
            colorVec3 *= 255;
            color.r = (byte)colorVec3.X;
            color.g = (byte)colorVec3.Y;
            color.b = (byte)colorVec3.Z;
            initialBody.Color = color;
            liveBody.Color = color;
        }
        
        ImGui.End();
    }

    public override void Update()
    {
        Open = World.SelectedIndex != -1;
    }
}