using UnityEngine;
using System.Collections.Generic;

public class SaveStateLoader : MonoBehaviour
{
    [Header("JSON Spielstand")]
    public TextAsset saveFile;

    [System.Serializable]
    private class PlugEntry
    {
        public string label;
        public string socket;
    }

    [System.Serializable]
    private class SaveData
    {
        public List<PlugEntry> stecker;
    }

    void Start()
    {
        if (saveFile == null) return;
        Invoke(nameof(LoadState), 0.1f);
    }

    void LoadState()
    {
        Debug.Log($"[SaveLoader] Alle Kabel im CableManager:");
foreach (Cable c in CableManager.Instance.cables)
{
    Debug.Log($"  Kabel: {c.cableName}, labelA={c.labelA}, labelB={c.labelB}, aUsed={c.aUsed}, bUsed={c.bUsed}");
}

        Debug.Log($"[SaveLoader] LoadState aufgerufen von {gameObject.name}", gameObject);

        SaveData data = JsonUtility.FromJson<SaveData>(saveFile.text);

        SocketManager[] allSockets = FindObjectsOfType<SocketManager>();
        Dictionary<string, SocketManager> socketMap = new Dictionary<string, SocketManager>();

        foreach (SocketManager sm in allSockets)
        {
            if (!socketMap.ContainsKey(sm.socketName))
                socketMap.Add(sm.socketName, sm);
        }

        foreach (PlugEntry entry in data.stecker)
        {
            if (socketMap.TryGetValue(entry.socket, out SocketManager socket))
            {
                Debug.Log($"[SaveLoader] Versuche '{entry.label}' in '{entry.socket}': isOccupied={socket.isOccupied}, IsPlugUsed={CableManager.Instance.IsPlugUsed(entry.label)}");

                if (!socket.isOccupied && !CableManager.Instance.IsPlugUsed(entry.label))
                {
                    socket.InsertPlug(entry.label);
                    Debug.Log($"[SaveLoader] {entry.label} → {entry.socket} gesteckt");
                }
                else
                {
                    Debug.LogWarning($"[SaveLoader] Konnte {entry.label} nicht in {entry.socket} stecken (belegt)");
                }
            }
            else
            {
                Debug.LogWarning($"[SaveLoader] Buchse '{entry.socket}' nicht gefunden!");
            }
        }
    }
}
