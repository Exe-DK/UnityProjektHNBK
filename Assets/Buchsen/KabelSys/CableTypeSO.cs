using UnityEngine;

[CreateAssetMenu(fileName = "NewCableType", menuName = "Wiring/Cable Type")]
public class CableTypeSO : ScriptableObject
{
    public string cableName = "L1";
    public CableCategory category = CableCategory.L1;
    public Color cableColor = Color.black;
    public string labelA = "A_L1";
    public string labelB = "B_L1";
}
