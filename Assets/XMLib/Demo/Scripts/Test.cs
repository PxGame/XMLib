using UnityEngine;
using XM;
using XM.Services;

public class Test : MonoBehaviour
{
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AppEntry.Pool.Pop("Capsule");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject obj = GameObject.Find("Capsule");
            if (obj != null)
            {
                AppEntry.Pool.Push(obj);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AppEntry.Pool.Clear();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Open Panel1");
            AppEntry.UI.Open("Panel1", "MMP");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Open Panel2");
            AppEntry.UI.Open("Panel2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("CloseAll");
            AppEntry.UI.CloseAll(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("CloseTop");
            AppEntry.UI.CloseTop(false, null);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("SwitchTop");
            AppEntry.UI.SwitchTop("Panel1", false, null, "MMP2");
        }
    }
}