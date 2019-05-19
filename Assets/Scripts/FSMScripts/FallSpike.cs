using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpike : StoppableObject
{
    public float fallSpeed = 15.0f;

    private float minY;
    private float maxY;

    public bool up;

    private void Awake()
    {
        minY = GameObject.FindGameObjectWithTag("OuterBorder").GetComponent<OuterBorder>().buttonBorder();
        maxY = GameObject.FindGameObjectWithTag("OuterBorder").GetComponent<OuterBorder>().upBorder();
    }

    protected override void EnableSetting()
    {
        transform.localPosition = Vector2.zero;
    }

    protected override void Update()
    {
        if (on)
        {
            Fall();
        }
    }

    private void Fall()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        
        if (up)
        {
            // disable
            if (transform.position.y > maxY)
            {
                gameObject.SetActive(false);
            }
        }
        else    // down
        {
            // disable
            if (transform.position.y < minY)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
