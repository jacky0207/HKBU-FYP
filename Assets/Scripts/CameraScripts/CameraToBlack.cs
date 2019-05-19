using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToBlack : MonoBehaviour
{	
    public Material switchStageMaterial;
    public bool on;
    // private bool start = false;
    private bool fadeIn = true;
    public float blackSpeed = 0.01f;

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        // UnityEditor.EditorApplication.update += Update;

        // initial grey radius
        switchStageMaterial.SetFloat("_Percentage", 0);
        // switchStageMaterial.SetFloat("_StartTransition", 1);
        switchStageMaterial.SetFloat("_StartTransition", 0);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, switchStageMaterial);
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
        if (on)
        {
            float percentage = switchStageMaterial.GetFloat("_Percentage");
            int transitionStage = switchStageMaterial.GetInt("_StartTransition");

            // Add percentage
            float newPixelNumber = percentage + blackSpeed;
            switchStageMaterial.SetFloat("_Percentage", newPixelNumber);

            if (fadeIn)
            {
                if (percentage > 1)
                {
                    fadeIn = false;
                    switchStageMaterial.SetFloat("_Percentage", 0);
                    switchStageMaterial.SetInt("_StartTransition", 1);
                }
            }
            else
            {
                if (percentage > 1)
                {
                    on = false;
                    fadeIn = true;
                    switchStageMaterial.SetFloat("_Percentage", 0);
                    switchStageMaterial.SetInt("_StartTransition", 0);
                }
            }
        }

        // float percentage = switchStageMaterial.GetFloat("_Percentage");
        // int transitionStage = switchStageMaterial.GetInt("_StartTransition");
        
        // if (start)
        // {
        //     if (on)
        //     {
        //         if (percentage <= 1)
        //         {
        //             float newPixelNumber = percentage + blackSpeed;
        //             switchStageMaterial.SetFloat("_Percentage", newPixelNumber);
        //         }
        //         else
        //         {
        //             start = false;
        //             switchStageMaterial.SetFloat("_Percentage", 0);
        //             switchStageMaterial.SetInt("_StartTransition", 1);
        //         }
        //     }
        // }
        // else
        // {
        //     if (!on)
        //     {
        //         if (percentage <= 1)
        //         {
        //             float newPixelNumber = percentage + blackSpeed;
        //             switchStageMaterial.SetFloat("_Percentage", newPixelNumber);
        //         }
        //         else
        //         {
        //             start = true;
        //             switchStageMaterial.SetFloat("_Percentage", 0);
        //             switchStageMaterial.SetInt("_StartTransition", 0);
        //         }
        //     }
        // }
    }

}