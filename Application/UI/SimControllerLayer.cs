using ImGuiNET;

namespace Application.UI;

public class SimControllerLayer : BaseUiLayer
{
    public override void Attach()
    {
    }

    public override void Detach()
    {
    }

    public override void Render()
    {
        var isOpen = Open;
        if (!isOpen) return;

        ImGui.Begin("Sim Controller", ref isOpen);
        if (ImGui.Button("Reset")) World.Reset();
        ImGui.End();

        Open = isOpen;
    }

    public override void Update()
    {
    }
}