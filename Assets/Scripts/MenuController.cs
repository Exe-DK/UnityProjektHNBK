using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public void ButtonClickedStartGame(string Visualization)
    {
        Debug.Log("ButtonClicked for Scene:" + Visualization);
        SceneManager.LoadScene(Visualization);
    }

    public void ButtonClickedStartInformation(string Information)
    {
        Debug.Log("ButtonClicked for Scene:" + Information);
        SceneManager.LoadScene(Information);
    }

    public void ButtonClickedLoadWe(string We)
    {
        Debug.Log("ButtonClicked for Scene:" + We);
        SceneManager.LoadScene(We);
    } 
    
    public void ButtonClickedLoadMenu(string Menu)
    {
        Debug.Log("ButtonClicked for Scene" + Menu);
        SceneManager.LoadScene(Menu);
        
    }
   
}


