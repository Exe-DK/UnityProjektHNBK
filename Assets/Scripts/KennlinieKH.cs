using UnityEngine;

public class SinusDisplay : MonoBehaviour
{
    [Header("Textur-Größe")]
    public int width = 512;
    public int height = 256;

    [Header("Darstellung")]
    public Color backgroundColor = Color.black;
    public Color lineColor = Color.green;
    public Color axisColor = Color.white;
    public Color gridColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    public Color effektivColor = Color.yellow;
    [Range(1, 4)]
    public int lineThickness = 2;

    [Header("Sinus-Parameter")]
    [Range(0.1f, 1f)]
    public float amplitude = 0.8f;

    [Header("Zeitfenster in Millisekunden")]
    public float zeitfensterMs = 20f;

    [Header("Spannung")]
    public float maxVolt = 400f;

    [Header("Animation")]
    public float scrollSpeed = 1f;

    [Header("Achsen-Beschriftung")]
    public int achsenBreite = 70;
    public int achsenHoehe = 40;
    public int randRechts = 20;
    public int randOben = 15;
    public int anzahlZeitMarken = 5;
    public int anzahlVoltMarken = 5;

    [Header("Schrift-Skalierung")]
    [Range(1, 4)]
    public int fontScale = 2;

    private Texture2D texture;
    private Color[] clearPixels;
    private float timeOffset;
    private Renderer rend;

    private int plotLeft;
    private int plotBottom;
    private int plotWidth;
    private int plotHeight;

    void Start()
    {
        rend = GetComponent<Renderer>();

        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;

        UpdatePlotArea();

        clearPixels = new Color[width * height];
        for (int i = 0; i < clearPixels.Length; i++)
            clearPixels[i] = backgroundColor;

        rend.material.mainTexture = texture;
        texture.SetPixels(clearPixels);
        texture.Apply();
    }

    void UpdatePlotArea()
    {
        plotLeft = achsenBreite;
        plotBottom = achsenHoehe;
        plotWidth = width - achsenBreite - randRechts;
        plotHeight = height - achsenHoehe - randOben;
    }

    void Update()
    {
        UpdatePlotArea();

        if (!MotorDrehung.motorLäuft)
        {
            texture.SetPixels(clearPixels);
            texture.Apply();
            timeOffset = 0f;
            return;
        }

        float hz = ReglerDrehung.Frequenz;
        timeOffset += Time.deltaTime * scrollSpeed;

        texture.SetPixels(clearPixels);

        DrawAchsen();
        DrawGrid();
        DrawEffektivLinie();
        DrawSinus(hz);

        texture.Apply();
    }

    void DrawSinus(float hz)
    {
        float zeitfensterSek = zeitfensterMs / 1000f;
        int prevY = -1;

        for (int px = 0; px < plotWidth; px++)
        {
            float zeit = ((float)px / plotWidth) * zeitfensterSek;
            float angle = zeit * hz * 2f * Mathf.PI + timeOffset * hz;
            float sinValue = Mathf.Sin(angle);

            int midY = plotBottom + plotHeight / 2;
            int y = Mathf.RoundToInt(midY + sinValue * (plotHeight * 0.5f * amplitude));
            y = Mathf.Clamp(y, plotBottom, plotBottom + plotHeight - 1);

            int x = plotLeft + px;

            for (int thick = -lineThickness / 2; thick <= lineThickness / 2; thick++)
            {
                int py = Mathf.Clamp(y + thick, plotBottom, plotBottom + plotHeight - 1);
                texture.SetPixel(x, py, lineColor);
            }

            if (prevY >= 0 && Mathf.Abs(y - prevY) > 1)
            {
                int minY = Mathf.Min(y, prevY);
                int maxY = Mathf.Max(y, prevY);
                for (int fillY = minY; fillY <= maxY; fillY++)
                {
                    for (int thick = -lineThickness / 2; thick <= lineThickness / 2; thick++)
                    {
                        int py = Mathf.Clamp(fillY + thick, plotBottom, plotBottom + plotHeight - 1);
                        texture.SetPixel(x - 1, py, lineColor);
                    }
                }
            }

            prevY = y;
        }
    }

    void DrawEffektivLinie()
{
    float uEff = Berechnung.Spannung;
    if (uEff < 0.1f) return;

    int midY = plotBottom + plotHeight / 2;
    float normPos = uEff / maxVolt;
    int yPos = Mathf.RoundToInt(midY + normPos * (plotHeight * 0.5f));
    yPos = Mathf.Clamp(yPos, plotBottom, plotBottom + plotHeight - 1);

    for (int x = plotLeft; x < plotLeft + plotWidth; x++)
    {
        if (x % 6 < 4)
        {
            for (int d = -1; d <= 1; d++)
            {
                int py = Mathf.Clamp(yPos + d, plotBottom, plotBottom + plotHeight - 1);
                texture.SetPixel(x, py, effektivColor);
            }
        }
    }

    string label = Mathf.RoundToInt(uEff).ToString() + "V";
    int labelX = plotLeft + plotWidth + 2;
    int charHeight = 5 * fontScale;
    DrawString(labelX, yPos - charHeight / 2, label, effektivColor);
}

    void DrawAchsen()
    {
        for (int y = plotBottom; y < plotBottom + plotHeight; y++)
            texture.SetPixel(plotLeft, y, axisColor);

        for (int x = plotLeft; x < plotLeft + plotWidth; x++)
            texture.SetPixel(x, plotBottom, axisColor);

        int midY = plotBottom + plotHeight / 2;
        for (int x = plotLeft; x < plotLeft + plotWidth; x++)
        {
            if (x % 4 < 2)
                texture.SetPixel(x, midY, gridColor);
        }
    }

    void DrawGrid()
    {
        for (int i = 1; i <= anzahlZeitMarken; i++)
        {
            int x = plotLeft + (plotWidth * i / anzahlZeitMarken);
            if (x >= width) continue;

            for (int y = plotBottom; y < plotBottom + plotHeight; y++)
            {
                if (y % 4 < 2)
                    texture.SetPixel(x, y, gridColor);
            }

            for (int t = 0; t < 4; t++)
            {
                if (plotBottom - t >= 0)
                    texture.SetPixel(x, plotBottom - t, axisColor);
            }

            float msWert = zeitfensterMs * i / anzahlZeitMarken;
            string label = msWert.ToString("F1");
            int labelWidth = label.Length * (4 * fontScale + fontScale);
            DrawString(x - labelWidth / 2, 2, label, axisColor);
        }

        for (int i = 0; i <= anzahlVoltMarken; i++)
        {
            int y = plotBottom + (plotHeight * i / anzahlVoltMarken);
            if (y >= height) continue;

            if (i > 0 && i < anzahlVoltMarken)
            {
                for (int x = plotLeft; x < plotLeft + plotWidth; x++)
                {
                    if (x % 4 < 2)
                        texture.SetPixel(x, y, gridColor);
                }
            }

            for (int t = 0; t < 4; t++)
            {
                if (plotLeft - t >= 0)
                    texture.SetPixel(plotLeft - t, y, axisColor);
            }

            float voltWert = -maxVolt + (2f * maxVolt * i / anzahlVoltMarken);
            string label = Mathf.RoundToInt(voltWert).ToString();
            int charHeight = 5 * fontScale;
            DrawString(2, y - charHeight / 2, label, axisColor);
        }
    }

    void DrawString(int startX, int startY, string text, Color color)
    {
        int cx = startX;
        foreach (char c in text)
        {
            bool[,] pixels = GetCharPixels(c);
            if (pixels == null)
            {
                cx += 4 * fontScale;
                continue;
            }

            int charWidth = pixels.GetLength(1);

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < charWidth; col++)
                {
                    if (pixels[row, col])
                    {
                        for (int sy = 0; sy < fontScale; sy++)
                        {
                            for (int sx = 0; sx < fontScale; sx++)
                            {
                                int px = cx + col * fontScale + sx;
                                int py = startY + (4 - row) * fontScale + sy;
                                if (px >= 0 && px < width && py >= 0 && py < height)
                                    texture.SetPixel(px, py, color);
                            }
                        }
                    }
                }
            }

            cx += (charWidth + 1) * fontScale;
        }
    }

    bool[,] GetCharPixels(char c)
    {
        switch (c)
        {
            case '0': return new bool[,] {
                {false,true,true,false},{true,false,false,true},{true,false,false,true},{true,false,false,true},{false,true,true,false}};
            case '1': return new bool[,] {
                {false,false,true,false},{false,true,true,false},{false,false,true,false},{false,false,true,false},{false,true,true,true}};
            case '2': return new bool[,] {
                {false,true,true,false},{true,false,false,true},{false,false,true,false},{false,true,false,false},{true,true,true,true}};
            case '3': return new bool[,] {
                {true,true,true,false},{false,false,false,true},{false,true,true,false},{false,false,false,true},{true,true,true,false}};
            case '4': return new bool[,] {
                {true,false,true,false},{true,false,true,false},{true,true,true,true},{false,false,true,false},{false,false,true,false}};
            case '5': return new bool[,] {
                {true,true,true,true},{true,false,false,false},{true,true,true,false},{false,false,false,true},{true,true,true,false}};
            case '6': return new bool[,] {
                {false,true,true,false},{true,false,false,false},{true,true,true,false},{true,false,false,true},{false,true,true,false}};
            case '7': return new bool[,] {
                {true,true,true,true},{false,false,false,true},{false,false,true,false},{false,true,false,false},{false,true,false,false}};
            case '8': return new bool[,] {
                {false,true,true,false},{true,false,false,true},{false,true,true,false},{true,false,false,true},{false,true,true,false}};
            case '9': return new bool[,] {
                {false,true,true,false},{true,false,false,true},{false,true,true,true},{false,false,false,true},{false,true,true,false}};
            case '-': return new bool[,] {
                {false,false,false,false},{false,false,false,false},{true,true,true,true},{false,false,false,false},{false,false,false,false}};
            case '.': return new bool[,] {
                {false,false,false,false},{false,false,false,false},{false,false,false,false},{false,false,false,false},{false,true,false,false}};
            case 'V': return new bool[,] {
                {true,false,true},{true,false,true},{true,false,true},{false,true,false},{false,true,false}};
            default: return null;
        }
    }
}
