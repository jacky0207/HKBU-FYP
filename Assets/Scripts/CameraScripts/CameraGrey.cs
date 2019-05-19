using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGrey : MonoBehaviour
{
    public Material cameraMaterial;
    public bool on;
    public bool invisibleMode;  // Some object is invisible when time freeze
    public float greySpeed = 0.05f;

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        // UnityEditor.EditorApplication.update += Update;

        // initial grey radius
        cameraMaterial.SetFloat("_Radius", 0);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, cameraMaterial);
    }

    public bool GetOn()
    {
        return on;
    }

    public void TurnOnOrOffShader()
    {
        on = !on;
    }

    void Update()
    {
        float radius = cameraMaterial.GetFloat("_Radius");

        if (on)
        {
            if (radius <= 1)
            {
                cameraMaterial.SetFloat("_Radius", radius + greySpeed);
            }
        }
        else
        {
            if (radius >= 0)
            {
                cameraMaterial.SetFloat("_Radius", radius - greySpeed);
            }
        }
    }
    
}
