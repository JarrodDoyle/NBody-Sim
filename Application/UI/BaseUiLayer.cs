namespace Application.UI;

public abstract class BaseUiLayer
{
    public bool Open { get; set; }

    public abstract void Attach();
    public abstract void Detach();
    public abstract void Render();
    public abstract void Update();
}