using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : StoppableObject
{
	public float jetSpeed = 15.0f;
    public bool right;
    public bool up;
    
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    public void SetLeftBound(Transform bound)
    {
        minX = bound.position.x;
        minY = bound.position.y;
    }

    public void SetRightBound(Transform bound)
    {
        maxX = bound.position.x;
        maxY = bound.position.y;
    }

    private void Awake()
    {
        minX = GameObject.FindGameObjectWithTag("OuterBorder").GetComponent<OuterBorder>().leftBorder();
        maxX = GameObject.FindGameObjectWithTag("OuterBorder").GetComponent<OuterBorder>().rightBorder();
    }

    protected override void EnableSetting()
    {
        transform.localPosition = Vector2.zero;
    }

    private void Update()
    {
        if (on)
        {
            Jet();
        }
    }

    private void Jet()
    {
        float speed = right ? -1 * jetSpeed * Time.deltaTime : jetSpeed * Time.deltaTime;

		// Player position
		// if (player != null)
        if (DetectPlayerAbove())
		{
			player.Translate(Vector2.left * speed);
		}

        // Move
        transform.Translate(Vector2.left * speed);

        // disable
        if (!up)
        {
            if (transform.position.x < minX || transform.position.x > maxX)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (transform.position.y < minY || transform.position.y > maxY)
            {
                gameObject.SetActive(false);
            }  
        }
    }

    // public void OnCollisionEnter2D(Collision2D col)
    // {
    //     // Collide player
    //     if (col.gameObject.tag == "Player")
    //     {
    //         player = col.gameObject.transform;
    //     }
    // }

    // public void OnCollisionExit2D(Collision2D col)
    // {
    //     // Collide player
    //     if (col.gameObject.tag == "Player")
    //     {
    //         player = null;
    //     }
    // }
}
