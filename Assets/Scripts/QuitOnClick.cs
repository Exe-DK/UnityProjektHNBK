using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour
{
    public void OnApplicationQuit()
    {
        Debug.Log("Beenden");
        Application.Quit();

    }
}
