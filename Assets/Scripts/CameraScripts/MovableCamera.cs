using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCamera : MonoBehaviour
{
	private Transform player;

    public Transform source;
    public bool setSourceBorder = true;
    public Transform destination;
    public bool setDestinationBorder = true;

    // Follow player mode
    public float offsetX = 3.0f;
    public float smoothTime = 0.3F;
    public Vector3 velocity = Vector3.zero;

    // Map view mode
    public float mapViewMoveSpeed = 20.0f;
    private float screenWidth = Screen.width;
    private float lastTouchPositionX = -9999;

    private bool mapViewMode;   // Map viewing in pause menu

	void Awake ()
	{
		// Find player position
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Follow player movement
	void Update ()
    {
        // Map view mode
        if (mapViewMode)
        {
            // Move camera by touch
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);    // first touch

                float touchPositionX = touch.position.x;

                if (touch.phase == TouchPhase.Moved)
                {
                    if (lastTouchPositionX != -9999)
                    {
                        // Move by finger move
                        float moveNormalizedDistance = (lastTouchPositionX - touchPositionX) / screenWidth; // 0 to 1
                        float minX = (source == null || !setSourceBorder) ? moveNormalizedDistance : source.position.x;
                        float maxX = (destination == null || !setDestinationBorder) ? moveNormalizedDistance : destination.position.x;
                        
                        float newPositionX = transform.position.x + moveNormalizedDistance * mapViewMoveSpeed;
                        newPositionX = Mathf.Clamp(newPositionX, minX, maxX);   // limit by border
                        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
                    }

                    // Update last touch position
                    lastTouchPositionX = touchPositionX;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    // Update last touch position
                    lastTouchPositionX = touchPositionX;
                }
            }
            else
            {
                // Reset last touch position to initial
                lastTouchPositionX = -9999;
            }
        }
        // Follow player mode
        else
        {
            float cameraNextPosition = player.position.x + offsetX;
            float minX = (source == null || !setSourceBorder) ? cameraNextPosition : source.position.x;
            float maxX = (destination == null || !setDestinationBorder) ? cameraNextPosition : destination.position.x;

            // Bounded position
            float targetPositionX = Mathf.Clamp(cameraNextPosition + offsetX, minX, maxX);        
            Vector3 targetPosition = new Vector3(targetPositionX, transform.position.y, transform.position.z);
            // Vector3 targetPosition = new Vector3(targetPositionX, source.position.y, -10);

            // Set position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
	}

    // Change camera border
    public void SetBorder(Transform source, Transform destination)
    {
        this.source = source;
        this.destination = destination;
        transform.position = new Vector3(player.position.x + offsetX, source.position.y, transform.position.z);  // camera position
    }

    // Change camera view mode
    public void SwitchCameraViewMode()
    {
        mapViewMode = !mapViewMode;
    }

    public bool IsMapViewMode()
    {
        return mapViewMode;
    }

    public IEnumerator MoveToPlayer()
    {
        float cameraNextPosition = player.position.x + offsetX;
        float minX = (source == null || !setSourceBorder) ? cameraNextPosition : source.position.x;
        float maxX = (destination == null || !setDestinationBorder) ? cameraNextPosition : destination.position.x;

        float targetPositionX = Mathf.Clamp(cameraNextPosition + offsetX, minX, maxX); 

        while (Mathf.Abs(transform.position.x - targetPositionX) > 0.01f)
        {
            Vector3 targetPosition = new Vector3(targetPositionX, transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, 10000, Time.unscaledDeltaTime);

            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

}
