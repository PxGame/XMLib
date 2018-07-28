using DG.Tweening;
using System;
using XM.Services;

public class TestUIPanel : UIPanel
{
    protected void Initialize(string msg)
    {
        Debug(XM.DebugType.GG, msg);
    }

    protected override void OnForceCompleteOperation()
    {
        base.OnForceCompleteOperation();

        _canvasGroup.DOKill(true);
    }

    internal override void OnEnter(Action complete)
    {
        _canvasGroup.DOFade(1.0f, 1.5f).ChangeValues(0.0f, 1.0f).OnComplete(() =>
        {
            base.OnEnter(complete);
        }).Play();
    }

    internal override void OnLeave(Action complete)
    {
        _canvasGroup.DOFade(0.0f, 1.5f).ChangeValues(1.0f, 0.0f).OnComplete(() =>
         {
             base.OnLeave(complete);
         }).Play();
    }

    internal override void OnResume(Action complete)
    {
        _canvasGroup.DOFade(1.0f, 1.0f).ChangeValues(0.5f, 1.0f).OnComplete(() =>
         {
             base.OnResume(complete);
         }).Play();
    }

    internal override void OnPause(Action complete)
    {
        _canvasGroup.DOFade(0.5f, 1.0f).ChangeValues(1.0f, 0.5f).OnComplete(() =>
        {
            base.OnPause(complete);
        }).Play();
    }
}