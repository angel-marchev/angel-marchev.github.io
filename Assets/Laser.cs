using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private const float minX = -8.5f;
    private const float maxX = 4.25f;

    private bool goingLeft = true, moving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (transform.position.x < minX)
            {
                goingLeft = false;
            }
            transform.Translate(new Vector3(goingLeft ? -4f : 7f, 0f) * Time.deltaTime);
            if (transform.position.x > maxX && !goingLeft)
            {
                moving = false;
                transform.position = new Vector2(maxX, 0);
                goingLeft = true;
                GetComponentInParent<GadGenerator>().onStopLaser();
            }
        }
    }

    public void StartLaser()
    {
        moving = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Gad"))
        {
            GadGenerator generator = GetComponentInParent<GadGenerator>();
            generator.killGad(other.gameObject);
            //kill
        }
    }
}
