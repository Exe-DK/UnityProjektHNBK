using UnityEngine;
using TMPro;
using System;

public class DigitalClock : MonoBehaviour
{
    public TMP_Text uhrzeitText;

    void Update()
    {
        uhrzeitText.text = DateTime.Now.ToString("HH:mm:ss");
    }
    
}
