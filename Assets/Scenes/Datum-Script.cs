using UnityEngine;
using TMPro;
using System;

public class DigitalDate : MonoBehaviour
{
    public TMP_Text datumText;

    void Update()
    {
        datumText.text = DateTime.Now.ToString("dd.MM.yyyy");
    }
}
