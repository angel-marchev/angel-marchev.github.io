using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float maxX = 3.45f;
    float minX = -7.95f;
    float maxY = 2.85f;
    float minY = -3.2f;

    public GameObject sprite, eyes, selection;
    public Sprite[] sprites1, sprites2, sprites3;
    public Sprite[] eyesSelected, eyesNonSelected;
    public GadGenerator generator;

    public bool selected = false;

    private float interval = 1f;
    private float nextTime = 0;
    private int spriteType = 0;
    private int eyesType = 0;
    private int animationIndex = 0;

    public Color color;

    private float moveSpeed = 1;

    private float tChange = 0; // force new direction in the first Update
    private float randomX;
    private float randomY;
     
    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponentInParent<GadGenerator>();
        interval = Random.Range(0.1f, 0.3f);
        spriteType = Random.Range(0, 4);
        SwitchAnimation(0);
        eyesType = Random.Range(0, 2);
        SwitchEyes(false);
    }

    // Update is called once per frame
    void Update()
    {
        // change to random direction at random intervals
        if (Time.time >= tChange)
        {
            randomX = Random.Range(-2.0f, 2.0f); // with float parameters, a random float
            randomY = Random.Range(-2.0f, 2.0f); //  between -2.0 and 2.0 is returned
                                               // set a random interval between 0.5 and 1.5
            tChange = Time.time + Random.Range(0.5f, 1.5f);
        }
        transform.Translate(new Vector2(randomX, randomY) * moveSpeed * Time.deltaTime);
        // if object reached any border, revert the appropriate direction
        if (transform.position.x >= maxX || transform.position.x <= minX)
        {
            randomX = -randomX;
        }
        if (transform.position.y >= maxY || transform.position.y <= minY)
        {
            randomY = -randomY;
        }
        // make sure the position is inside the borders
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY));

        if (Time.time >= nextTime)
        {
            nextTime += interval;
            SwitchAnimation(animationIndex % 3);
            animationIndex++;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (selected)
            {
                generator.selectedGads--;
                generator.selectedGadsObjects.Remove(gameObject);
                selected = !selected;
                SwitchEyes(selected);
            }
            else if (generator.isDay && !selected && generator.selectedGads < 5)
            {
                generator.selectedGads++;
                generator.selectedGadsObjects.Add(gameObject);
                selected = !selected;
                SwitchEyes(selected);
            }
        }
    }

    void SwitchAnimation(int index)
    {
        switch(index)
        {
            case 0:
                sprite.GetComponent<SpriteRenderer>().sprite = sprites1[spriteType];
                break;
            case 1:
                sprite.GetComponent<SpriteRenderer>().sprite = sprites2[spriteType];
                break;
            case 2:
                sprite.GetComponent<SpriteRenderer>().sprite = sprites3[spriteType];
                break;
            default:
                sprite.GetComponent<SpriteRenderer>().sprite = sprites1[spriteType];
                break;
        }
    }

    public void SwitchEyes(bool selected)
    {
        selection.SetActive(selected);
        if (selected)
        {
            eyes.GetComponent<SpriteRenderer>().sprite = eyesSelected[eyesType];
        } else
        {
            eyes.GetComponent<SpriteRenderer>().sprite = eyesNonSelected[eyesType];
        }
    }

    public void SetColor(Color colorParam)
    {
        color = colorParam;
        sprite.GetComponent<SpriteRenderer>().color = colorParam;
    }
}