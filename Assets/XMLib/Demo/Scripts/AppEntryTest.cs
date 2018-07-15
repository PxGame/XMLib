using System;
using System.Collections.Generic;
using UnityEngine;
using XM;
using XM.Services;

[DefaultExecutionOrder(-500)]
public class AppEntryTest : IAppEntry<AppEntryTest>
{
    public void Test(string msg)
    {
        Debug(DebugType.Normal, msg);
    }

    /// <summary>
    /// 开始关闭
    /// </summary>
    protected override void OnTerminal()
    {
        //static变量，服务关闭，引用设null
        _event = null;
        _pool = null;
        _task = null;
        _ui = null;
        _scene = null;
        _input = null;
    }

    /// <summary>
    /// 初始化默认服务列表
    /// </summary>
    /// <returns></returns>
    protected override List<Type> DefaultServices()
    {
        List<Type> services = base.DefaultServices();

        services.Add(typeof(EventService<AppEntryTest>));
        services.Add(typeof(PoolService<AppEntryTest>));
        services.Add(typeof(TaskService<AppEntryTest>));
        services.Add(typeof(UIService<AppEntryTest>));
        services.Add(typeof(SceneService<AppEntryTest>));
        services.Add(typeof(InputService<AppEntryTest>));
        services.Add(typeof(TestService));

        //默认服务
        return services;
    }

    #region Specific Custom

    //服务引用

    private static EventService<AppEntryTest> _event;
    private static PoolService<AppEntryTest> _pool;
    private static TaskService<AppEntryTest> _task;
    private static UIService<AppEntryTest> _ui;
    private static SceneService<AppEntryTest> _scene;
    private static InputService<AppEntryTest> _input;

    //

    /// <summary>
    /// 事件服务
    /// </summary>
    public static EventService<AppEntryTest> Event
    {
        get
        {
            if (null == _event)
            {
                _event = Inst.Get<EventService<AppEntryTest>>();
            }
            return _event;
        }
    }

    /// <summary>
    /// 对象池服务
    /// </summary>
    public static PoolService<AppEntryTest> Pool
    {
        get
        {
            if (null == _pool)
            {
                _pool = Inst.Get<PoolService<AppEntryTest>>();
            }
            return _pool;
        }
    }

    /// <summary>
    /// 任务服务
    /// </summary>
    public static TaskService<AppEntryTest> Task
    {
        get
        {
            if (null == _task)
            {
                _task = Inst.Get<TaskService<AppEntryTest>>();
            }

            return _task;
        }
    }

    /// <summary>
    /// UI服务
    /// </summary>
    public static UIService<AppEntryTest> UI
    {
        get
        {
            if (null == _ui)
            {
                _ui = Inst.Get<UIService<AppEntryTest>>();
            }

            return _ui;
        }
    }

    /// <summary>
    /// 场景服务
    /// </summary>
    public static SceneService<AppEntryTest> Scene
    {
        get
        {
            if (null == _scene)
            {
                _scene = Inst.Get<SceneService<AppEntryTest>>();
            }

            return _scene;
        }
    }

    /// <summary>
    /// 输入服务
    /// </summary>
    public static InputService<AppEntryTest> Input
    {
        get
        {
            if (null == _input)
            {
                _input = Inst.Get<InputService<AppEntryTest>>();
            }

            return _input;
        }
    }

    #endregion Specific Custom
}