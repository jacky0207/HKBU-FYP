using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableBoard : StoppableObject
{	
	public float rotateSpeed = 360.0f;
    public float radius = 2.0f;
	public bool anticlockrise;

	public float angle;

    protected override void Update()
    {
        if (on)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
		angle = anticlockrise ? (angle - rotateSpeed * Time.deltaTime) % -360 : (angle + rotateSpeed * Time.deltaTime) % 360;
		
        Vector3 previousPosition = transform.localPosition;
        Vector3 newPosition = new Vector2(radius * Mathf.Sin(angle * Mathf.Deg2Rad), radius * Mathf.Cos(angle * Mathf.Deg2Rad));
        
		// Player position
        if (DetectPlayerAbove())
		{
			Vector2 deltaPosition = newPosition - previousPosition;
			player.Translate(deltaPosition);
		}

        // Move
		transform.localPosition = new Vector2(radius * Mathf.Sin(angle * Mathf.Deg2Rad), radius * Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    // public void OnTriggerEnter2D(Collider2D col)
    // {
    //     // Collide player
    //     if (col.gameObject.tag == "Player")
    //     {
    //         player = col.gameObject.transform;
    //     }
    // }

    // public void OnTriggerExit2D(Collider2D col)
    // {
    //     // Collide player
    //     if (col.gameObject.tag == "Player")
    //     {
    //         player = null;
    //     }
    // }

    void OnDrawGizmos()
    {
        int vertexCount = 20;
        float deltaTheta = (2f * Mathf.PI) / vertexCount;

        float theta = deltaTheta;
        Vector3 previousPos = transform.parent.position + new Vector3(0, radius, 0);

        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 newPos = transform.parent.position + new Vector3(radius * Mathf.Sin(theta), radius * Mathf.Cos(theta), 0);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(previousPos, newPos);
            // Handles.color = Color.yellow;
            // Handles.DrawAAPolyLine(20, previousPos, newPos);

            previousPos = newPos;
            theta += deltaTheta;
        }
    }

}
