using UnityEngine;

public class Test : MonoBehaviour
{
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AppEntryTest.Pool.Pop("Capsule");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject obj = GameObject.Find("Capsule");
            if (obj != null)
            {
                AppEntryTest.Pool.Push(obj);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AppEntryTest.Pool.Clear();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Open Panel1");
            AppEntryTest.UI.Open("Panel1", "MMP");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Open Panel2");
            AppEntryTest.UI.Open("Panel2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("CloseAll");
            AppEntryTest.UI.CloseAll(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("CloseTop");
            AppEntryTest.UI.CloseTop(false, null);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("SwitchTop");
            AppEntryTest.UI.SwitchTop("Panel1", false, null, "MMP2");
        }
    }
}