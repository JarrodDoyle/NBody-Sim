using ImGuiNET;

namespace Application.UI;

public class SimControllerLayer : Panel
{
    private int _numBodies = 1000;
    
    public override void Attach()
    {
        Open = true;
    }

    public override void Detach()
    {
        Open = false;
    }

    public override void Render()
    {
        var isOpen = Open;
        if (!isOpen) return;

        ImGui.Begin("Sim Controller", ref isOpen);
        if (ImGui.Button("Reset")) World.Reset();
        ImGui.Checkbox("Playing", ref Program.Playing);
        if (ImGui.Button("Generate Random Bodies"))
        {
            World.GenerateBodies(_numBodies);
            World.Reset();
        }
        ImGui.SameLine();
        if (ImGui.InputInt("##num_bodies", ref _numBodies))
            _numBodies = Math.Clamp(_numBodies, 1, 1000);
        ImGui.Value("Existing bodies", World.InitialBodies.Count);
        ImGui.End();

        if (!isOpen) Detach();
    }

    public override void Update()
    {
    }
}