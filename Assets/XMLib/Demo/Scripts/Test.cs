using UnityEngine;

public class Test : MonoBehaviour
{
    public string str;

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool isOK = System.Text.RegularExpressions.Regex.IsMatch(str, "^.+(P|p)anel$");
            Debug.Log(isOK);
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
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    AppEntryTest.UI.ShowPanel("Panel1", "MMP");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    AppEntryTest.UI.ShowPanel("Panel2");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    AppEntryTest.UI.HidePanel();
        //}
    }
}