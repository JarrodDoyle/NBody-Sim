using ImGuiNET;

namespace Application.UI;

public class SimControllerLayer : Panel
{
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
        ImGui.End();

        if (!isOpen) Detach();
    }

    public override void Update()
    {
    }
}