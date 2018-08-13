using UnityEngine;
using XM;

public class Test : MonoBehaviour
{
    private void Awake()
    {
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FindObjectOfType<AppEntryTest>().Remove<TestService>();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<AppEntryTest>().Remove<TestService2>();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            FindObjectOfType<AppEntryTest>().Add<TestService>();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            FindObjectOfType<AppEntryTest>().Add<TestService2>();
        }
    }
}