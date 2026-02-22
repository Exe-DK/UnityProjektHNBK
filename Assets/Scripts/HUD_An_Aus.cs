using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class HUD_An_Aus : MonoBehaviour
{
    // Referenz auf das Canvas
    public Canvas CanvasHUD;

    // Funktion zum Einblenden oder Ausblenden aller untergeordneten GameObjects des Canvas
    public void ToggleCanvasVisibility()
    {
        // Überprüfe, ob das Canvas gültig ist
        if (CanvasHUD != null)
        {
            // Iteriere durch alle untergeordneten GameObjects des Canvas
            foreach (Transform child in CanvasHUD.transform)
            {
                // Aktiviere oder deaktiviere jedes GameObject
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }
    }
}
*/

public class HUD_An_Aus : MonoBehaviour
{
    public GameObject[] objectsToToggle; // Array für die zu ein- und ausblendenden Objekte. Werden in Unity zugeordnet

    // Methode, die aufgerufen wird, wenn der Button gedrückt wird
    public void ToggleObjects()
    {
        // Iteriere durch jedes Objekt im Array und ändere die Sichtbarkeit
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(!obj.activeSelf); // Setze die Sichtbarkeit um (einblenden/ausblenden)
        }
    }
}