using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
  public void OpenWebsiteHNBK()
    {
        Application.OpenURL("https://www.HNBK.de");
    }
    
  public void OpenWebsiteLenze()
    {
        Application.OpenURL("https://www.lenze.com/de-de/produkte/umrichter/frequenzumrichter/8400-stateline/");
    }
}
