using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct RequiredConnection
{
    public SocketController socketA;
    public SocketController socketB;
    public CableCategory requiredType;
}

public class ConnectionValidator2 : MonoBehaviour
{
    [Header("Soll-Verbindungen")]
    public List<RequiredConnection> requiredConnections = new List<RequiredConnection>();

    /// <summary>
    /// Prüft alle Verbindungen und gibt Ergebnis zurück.
    /// </summary>
    public bool ValidateAll()
    {
        var actual = CableManager2.Instance.GetAllConnections();
        int correctCount = 0;

        foreach (var req in requiredConnections)
        {
            bool found = false;
            foreach (var conn in actual)
            {
                bool socketsMatch =
                    (conn.socketA == req.socketA && conn.socketB == req.socketB) ||
                    (conn.socketA == req.socketB && conn.socketB == req.socketA);

                bool typeMatch = conn.cableType.category == req.requiredType;

                if (socketsMatch && typeMatch)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                correctCount++;
                Debug.Log($"✓ {req.socketA.name} <-> {req.socketB.name} ({req.requiredType}) korrekt");
            }
            else
            {
                Debug.Log($"✗ {req.socketA.name} <-> {req.socketB.name} ({req.requiredType}) fehlt/falsch");
            }
        }

        bool allCorrect = correctCount == requiredConnections.Count && actual.Count == requiredConnections.Count;
        Debug.Log(allCorrect ? "ALLES RICHTIG!" : $"{correctCount}/{requiredConnections.Count} korrekt");
        return allCorrect;
    }
}
