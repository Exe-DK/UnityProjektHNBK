using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cable
{
    public string cableName;      // z.B. "L1"
    public Color cableColor = Color.white;
    public string labelA;         // z.B. "A_L1"
    public string labelB;         // z.B. "B_L1"

    [HideInInspector] public SocketManager socketA; // Wo A steckt
    [HideInInspector] public SocketManager socketB; // Wo B steckt
    [HideInInspector] public bool aUsed = false;
    [HideInInspector] public bool bUsed = false;
}

public class CableManager : MonoBehaviour
{
    public static CableManager Instance;

    [Header("Alle verfügbaren Kabel")]
    public List<Cable> cables = new List<Cable>();

    void Awake()
    {
        Instance = this;
    }

    public Cable FindCableByLabel(string label)
    {
        foreach (Cable c in cables)
        {
            if (c.labelA == label || c.labelB == label)
                return c;
        }
        return null;
    }

    public bool IsPlugUsed(string label)
    {
        Cable c = FindCableByLabel(label);
        if (c == null) return true;

        if (c.labelA == label) return c.aUsed;
        if (c.labelB == label) return c.bUsed;
        return true;
    }

    public void SetPlugUsed(string label, SocketManager socket)
    {
        Cable c = FindCableByLabel(label);
        if (c == null) return;

        if (c.labelA == label)
        {
            c.aUsed = true;
            c.socketA = socket;
        }
        else if (c.labelB == label)
        {
            c.bUsed = true;
            c.socketB = socket;
        }
    }

    public void SetPlugFree(string label)
    {
        Cable c = FindCableByLabel(label);
        if (c == null) return;

        if (c.labelA == label)
        {
            c.aUsed = false;
            c.socketA = null;
        }
        else if (c.labelB == label)
        {
            c.bUsed = false;
            c.socketB = null;
        }
    }

    public Color GetColorForLabel(string label)
    {
        Cable c = FindCableByLabel(label);
        return c != null ? c.cableColor : Color.white;
    }

    /// <summary>
/// Findet ein Kabel anhand eines seiner Labels (A oder B)
/// </summary>
public Cable GetCableByLabel(string label)
{
    foreach (Cable cable in cables)
    {
        if (cable.labelA == label || cable.labelB == label)
            return cable;
    }
    return null;
}

}
