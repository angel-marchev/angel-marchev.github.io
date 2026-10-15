using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Draws the two charts in the grey boxes on the right of the main screen:
// top - fitness of every GAD (bar chart), bottom - best R, G, B per generation vs. the objective (6 time series)
public class GadCharts : MonoBehaviour
{
    // world-space centers of the grey boxes in new_new_elements_01.png
    private static readonly Vector2 barBoxCenter = new Vector2(6.41f, 2.74f);
    private static readonly Vector2 plotBoxCenter = new Vector2(6.41f, -0.29f);
    private const float boxWidth = 3.6f;
    private const float boxHeight = 2.4f;
    private const float margin = 0.25f;
    private const float titleHeight = 0.3f;

    private const int maxGads = 15;
    private const int minGenerations = 10;
    private const int sortingOrder = 1;

    private static readonly Color axisColor = new Color(0.35f, 0.35f, 0.35f);
    private static readonly Color[] channelColors =
    {
        new Color(0.9f, 0.2f, 0.2f),
        new Color(0.2f, 0.65f, 0.2f),
        new Color(0.2f, 0.35f, 0.9f)
    };

    private Sprite whiteSprite;
    private Material lineMaterial;
    private Font font;
    private Transform root;

    private List<SpriteRenderer> bars = new List<SpriteRenderer>();
    private List<SpriteRenderer> barOutlines = new List<SpriteRenderer>();

    private List<Color> bestHistory = new List<Color>();
    private Color target;
    private LineRenderer[] bestLines = new LineRenderer[3];
    private List<GameObject> plotMarks = new List<GameObject>();

    void Awake()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        whiteSprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0f), 1f);
        lineMaterial = new Material(Shader.Find("Sprites/Default"));
        font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        root = new GameObject("Charts").transform;

        BuildBarChart();
        BuildPlot();
    }

    void BuildBarChart()
    {
        Rect area = ChartArea(barBoxCenter);
        CreateLabel("Fitness", new Vector2(area.xMin, area.yMax + titleHeight));
        CreateLine("barAxis", axisColor, 0.02f, new Vector3(area.xMin, area.yMin), new Vector3(area.xMax, area.yMin));

        for (int i = 0; i < maxGads; i++)
        {
            barOutlines.Add(CreateQuad("barOutline" + i, axisColor, sortingOrder));
            bars.Add(CreateQuad("bar" + i, Color.white, sortingOrder + 1));
        }
    }

    void BuildPlot()
    {
        Rect area = ChartArea(plotBoxCenter);
        CreateLabel("Best RGB vs objective", new Vector2(area.xMin, area.yMax + titleHeight));
        CreateLine("plotAxisX", axisColor, 0.02f, new Vector3(area.xMin, area.yMin), new Vector3(area.xMax, area.yMin));
        CreateLine("plotAxisY", axisColor, 0.02f, new Vector3(area.xMin, area.yMin), new Vector3(area.xMin, area.yMax));

        for (int c = 0; c < 3; c++)
        {
            bestLines[c] = CreateLine("best" + c, channelColors[c], 0.04f);
        }
    }

    // top chart: one bar per GAD, height = closeness to the objective, tinted with the GAD's color, best first
    public void UpdateBars(List<GameObject> gads, Color targetColor)
    {
        List<Color> colors = new List<Color>();
        foreach (GameObject gad in gads)
        {
            if (gad != null)
            {
                colors.Add(gad.GetComponent<Movement>().color);
            }
        }
        colors.Sort((a, b) => Fitness(b, targetColor).CompareTo(Fitness(a, targetColor)));

        Rect area = ChartArea(barBoxCenter);
        float slot = area.width / maxGads;
        float barWidth = slot * 0.7f;
        for (int i = 0; i < maxGads; i++)
        {
            bool visible = i < colors.Count;
            bars[i].gameObject.SetActive(visible);
            barOutlines[i].gameObject.SetActive(visible);
            if (!visible) continue;

            float height = Mathf.Max(Fitness(colors[i], targetColor) * area.height, 0.02f);
            Vector2 position = new Vector2(area.xMin + slot * (i + 0.5f), area.yMin);
            Color color = colors[i];
            color.a = 1f;
            bars[i].color = color;
            bars[i].transform.position = position;
            bars[i].transform.localScale = new Vector3(barWidth, height, 1f);
            barOutlines[i].transform.position = position;
            barOutlines[i].transform.localScale = new Vector3(barWidth + 0.04f, height + 0.02f, 1f);
        }
    }

    // bottom chart: adds the best GAD of a generation and redraws the 6 series
    public void AddGeneration(Color best, Color targetColor)
    {
        target = targetColor;
        bestHistory.Add(best);
        RedrawPlot();
    }

    void RedrawPlot()
    {
        foreach (GameObject mark in plotMarks)
        {
            Destroy(mark);
        }
        plotMarks.Clear();

        Rect area = ChartArea(plotBoxCenter);
        int columns = Mathf.Max(bestHistory.Count, minGenerations);
        float step = area.width / (columns - 1);

        for (int c = 0; c < 3; c++)
        {
            // objective as a dashed horizontal line
            float targetY = area.yMin + target[c] * area.height;
            for (float x = area.xMin; x < area.xMax; x += 0.2f)
            {
                SpriteRenderer dash = CreateQuad("objectiveDash" + c, channelColors[c], sortingOrder);
                dash.sprite = CornerSprite();
                dash.transform.position = new Vector2(x, targetY - 0.0125f);
                dash.transform.localScale = new Vector3(Mathf.Min(0.1f, area.xMax - x), 0.025f, 1f);
                plotMarks.Add(dash.gameObject);
            }

            // best GAD per generation as a solid line with point markers
            bestLines[c].positionCount = bestHistory.Count;
            for (int g = 0; g < bestHistory.Count; g++)
            {
                Vector3 point = new Vector3(area.xMin + g * step, area.yMin + bestHistory[g][c] * area.height);
                bestLines[c].SetPosition(g, point);
                SpriteRenderer marker = CreateQuad("bestMarker" + c, channelColors[c], sortingOrder + 1);
                marker.transform.position = point - new Vector3(0f, 0.04f);
                marker.transform.localScale = new Vector3(0.08f, 0.08f, 1f);
                plotMarks.Add(marker.gameObject);
            }
        }
    }

    // 1 = identical to the objective, 0 = opposite corner of the RGB cube
    public static float Fitness(Color color, Color targetColor)
    {
        return 1f - GadGenerator.Distance(color, targetColor) / Mathf.Sqrt(3f);
    }

    Rect ChartArea(Vector2 boxCenter)
    {
        float width = boxWidth - 2 * margin;
        float height = boxHeight - 2 * margin - titleHeight;
        return new Rect(boxCenter.x - width / 2, boxCenter.y - boxHeight / 2 + margin, width, height);
    }

    // dashes are anchored on their left edge so they tile from xMin
    private Sprite cornerSprite;
    Sprite CornerSprite()
    {
        if (cornerSprite == null)
        {
            cornerSprite = Sprite.Create(whiteSprite.texture, new Rect(0, 0, 1, 1), new Vector2(0f, 0f), 1f);
        }
        return cornerSprite;
    }

    SpriteRenderer CreateQuad(string name, Color color, int order)
    {
        GameObject quad = new GameObject(name);
        quad.transform.SetParent(root, false);
        SpriteRenderer renderer = quad.AddComponent<SpriteRenderer>();
        renderer.sprite = whiteSprite;
        renderer.color = color;
        renderer.sortingOrder = order;
        return renderer;
    }

    LineRenderer CreateLine(string name, Color color, float width, params Vector3[] points)
    {
        GameObject line = new GameObject(name);
        line.transform.SetParent(root, false);
        LineRenderer renderer = line.AddComponent<LineRenderer>();
        renderer.material = lineMaterial;
        renderer.startColor = color;
        renderer.endColor = color;
        renderer.startWidth = width;
        renderer.endWidth = width;
        renderer.useWorldSpace = true;
        renderer.sortingOrder = sortingOrder + 1;
        renderer.numCapVertices = 2;
        renderer.positionCount = points.Length;
        renderer.SetPositions(points);
        return renderer;
    }

    void CreateLabel(string text, Vector2 topLeft)
    {
        GameObject label = new GameObject(text);
        label.transform.SetParent(root, false);
        label.transform.position = topLeft;
        TextMesh mesh = label.AddComponent<TextMesh>();
        mesh.text = text;
        mesh.font = font;
        mesh.fontSize = 48;
        mesh.characterSize = 0.04f;
        mesh.anchor = TextAnchor.UpperLeft;
        mesh.color = axisColor;
        MeshRenderer renderer = label.GetComponent<MeshRenderer>();
        renderer.material = font.material;
        renderer.sortingOrder = sortingOrder + 1;
    }
}
