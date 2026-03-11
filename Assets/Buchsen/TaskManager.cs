using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    [System.Serializable]
    public class SocketTask
    {
        [Tooltip("Die Buchse die belegt werden soll")]
        public SocketManager socket;

        [Tooltip("Der Name des Kabels das hier hin gehört (z.B. 'L1')")]
        public string correctCableName;
    }

    [Header("Aufgaben-Definition")]
    public List<SocketTask> tasks = new List<SocketTask>();

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Prüft alle Aufgaben und gibt zurück ob alles richtig ist
    /// </summary>
    public bool CheckAll()
    {
        bool allCorrect = true;

        foreach (SocketTask task in tasks)
        {
            bool correct = IsTaskCorrect(task);
            string status = correct ? "✅" : "❌";
            string current = task.socket.isOccupied ? task.socket.currentPlug : "leer";


            Debug.Log(status + " Buchse '" + task.socket.socketName + "': soll=" 
                + task.correctCableName + ", ist=" + current);

            if (!correct)
                allCorrect = false;
        }

        if (allCorrect)
            Debug.Log("🎉 Alles richtig!");
        else
            Debug.Log("⚠️ Noch nicht alles korrekt.");

        return allCorrect;
    }

    /// <summary>
    /// Prüft ob auf einer Buchse das richtige Kabel liegt (egal welches Ende)
    /// </summary>
    bool IsTaskCorrect(SocketTask task)
    {
        if (!task.socket.isOccupied)
            return false;

        // Aktuelles Label auf der Buchse (z.B. "L1" oder "L1'")
       string plugLabel = task.socket.currentPlug;


        // Kabel anhand des Labels finden
        Cable cable = CableManager.Instance.GetCableByLabel(plugLabel);

        if (cable == null)
            return false;

        // Prüfen ob der Kabelname übereinstimmt
        return cable.cableName == task.correctCableName;
    }
}
