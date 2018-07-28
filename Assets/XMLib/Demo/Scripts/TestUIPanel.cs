using XM.Services;

public class TestUIPanel : UIPanel
{
    protected void Initialize(string msg)
    {
        Debug(XM.DebugType.GG, msg);
    }
}