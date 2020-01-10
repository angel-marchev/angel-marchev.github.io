using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadGenerator : MonoBehaviour
{

    public List<GameObject> gads;
    public int selectedGads = 0;
    public List<GameObject> selectedGadsObjects;
    public GameObject gadPrefab;

    public bool isDay = true;
    public int generation = 1;
    public Color targetColor;
    public GameObject targetColorDisplay, targetColorText, bestColorText, generationText;

    public GameObject dayBackground, nightBackground;
    public GameObject youWin, youLose;

    private bool nightCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        generationText.GetComponent<TextMesh>().text = generation + "";
        bestColorText.GetComponent<TextMesh>().text = "0, 0, 0";
        targetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        targetColorDisplay.GetComponent<SpriteRenderer>().color = targetColor;
        targetColorText.GetComponent<TextMesh>().text = "" + (int) (targetColor.r * 256 - 1) + ", " + (int) (targetColor.g * 256 - 1) + ", " + (int) (targetColor.b * 256 - 1);
        for (int i = 0; i < 15; i++)
        {
            gads.Add(Instantiate(gadPrefab, new Vector2(Random.Range(-7.95f, 3.05f), Random.Range(-3.2f, 2.45f)), Quaternion.identity, transform));
            gads[gads.Count - 1].GetComponent<Movement>().SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void killGad(GameObject gad)
    {
        if (!gad.GetComponent<Movement>().selected)
        {
            Destroy(gad);
        }
    }

    public void onStopLaser()
    {
        List<GameObject> temp = new List<GameObject>();
        for (int i = 0; i < gads.Count; i++)
        {
            if (gads[i].gameObject)
            {
                temp.Add(gads[i]);
            }
        }
        gads = temp;
        for (int i = 0; i < gads.Count; i++)
        {
            gads[i].GetComponent<Movement>().selected = false;
            gads[i].GetComponent<Movement>().SwitchEyes(false);
        }
        selectedGadsObjects.Clear();
        selectedGads = 0;
        StartCoroutine(StartRepopulation());
    }

    public IEnumerator StartRepopulation()
    {
        for (int i = 0; i < 4; i++)
        {
            Color gadsI = gads[i].GetComponent<Movement>().color;
            for (int j = i + 1; j < 5; j++)
            {
                yield return new WaitForSeconds(0.3f); 
                Color gadsJ = gads[j].GetComponent<Movement>().color;
                gads.Add(Instantiate(gadPrefab, new Vector2(Random.Range(-7.95f, 3.05f), Random.Range(-3.2f, 2.45f)), Quaternion.identity, transform));
                gads[gads.Count - 1].GetComponent<Movement>().SetColor(new Color((gadsI.r + gadsJ.r) / 2, (gadsI.g + gadsJ.g) / 2, (gadsI.b + gadsJ.b) / 2));
            }
        }
        yield return new WaitForSeconds(0.3f);
        generation++;
        if (generation == 10)
        {
            //lose condition
            youLose.SetActive(true);
        }
        generationText.GetComponent<TextMesh>().text = generation + "";

        List<float> euclids = new List<float>();
        int bestEuclidIndex = 0;
        for (int i = 0; i < 15; i++)
        {
            Color color = gads[i].GetComponent<Movement>().color;
            euclids.Add(Mathf.Sqrt(Mathf.Pow((color.r - targetColor.r), 2) + Mathf.Pow((color.g - targetColor.g), 2) + Mathf.Pow((color.b - targetColor.b), 2)));
            if (euclids[euclids.Count - 1] < euclids[bestEuclidIndex])
            {
                bestEuclidIndex = euclids.Count - 1;
            }
        }
        Color bestColor = gads[bestEuclidIndex].GetComponent<Movement>().color;
        bestColorText.GetComponent<TextMesh>().text = "" + (int)(bestColor.r * 256 - 1) + ", " + (int)(bestColor.g * 256 - 1) + ", " + (int)(bestColor.b * 256 - 1);
        if (bestColor.r - targetColor.r < 0.04 && bestColor.r - targetColor.r > -0.04 &&
            bestColor.g - targetColor.g < 0.04 && bestColor.g - targetColor.g > -0.04 &&
            bestColor.b - targetColor.b < 0.04 && bestColor.b - targetColor.b > -0.04)
        {
            //win condition
            youWin.SetActive(true);
        }
        nightCompleted = true;
    }

    public void onPressNight()
    {
        if (selectedGads == 5)
        {
            isDay = false;
            dayBackground.SetActive(false);
            nightBackground.SetActive(true);
            GetComponentInChildren<Laser>().StartLaser();
        }
    }

    public void onPressDay()
    {
        if (nightCompleted)
        {
            nightCompleted = false;
            dayBackground.SetActive(true);
            nightBackground.SetActive(false);
            isDay = true;
        }
    }

    IEnumerator FlashDay()
    {
        dayBackground.SetActive(true);
        nightBackground.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        dayBackground.SetActive(false);
        nightBackground.SetActive(true);
    }

    public void onPressMutation()
    {
        StartCoroutine(FlashDay());
        const float mutationMargin = 0.15f;
        if (!isDay)
        {
            for (int i = 0; i < 15; i++)
            {
                Color color = gads[i].GetComponent<Movement>().color;
                float red = color.r + Random.Range(-mutationMargin, mutationMargin);
                if (red < 0) red = 0;
                if (red > 1) red = 1;
                float green = color.g + Random.Range(-mutationMargin, mutationMargin);
                if (green < 0) green = 0;
                if (green > 1) green = 1;
                float blue = color.b + Random.Range(-mutationMargin, mutationMargin);
                if (blue < 0) blue = 0;
                if (blue > 1) blue = 1;
                gads[i].GetComponent<Movement>().SetColor(new Color(red, green, blue));
            }
        }
    }
}
