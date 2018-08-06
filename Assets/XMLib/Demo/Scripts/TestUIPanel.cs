using System;
using XM.Services.UI;

public class TestUIPanel : UIPanel
{
    protected void Initialize(string msg)
    {
        Debug(XM.DebugType.GG, msg);
    }

    protected override void OnForceCompleteOperation()
    {
        base.OnForceCompleteOperation();
    }

    internal override void OnEnter(Action complete)
    {
        base.OnEnter(complete);
    }

    internal override void OnLeave(Action complete)
    {
        base.OnLeave(complete);
    }

    internal override void OnResume(Action complete)
    {
        base.OnResume(complete);
    }

    internal override void OnPause(Action complete)
    {
        base.OnPause(complete);
    }
}