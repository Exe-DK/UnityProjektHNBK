using UnityEngine;

public class CableConnection2
{
    public SocketController socketA;
    public SocketController socketB;
    public CableTypeSO cableType;
    public LineRenderer lineRenderer;

    public CableConnection2(SocketController a, SocketController b, CableTypeSO type, LineRenderer line)
    {
        socketA = a;
        socketB = b;
        cableType = type;
        lineRenderer = line;
    }
}
