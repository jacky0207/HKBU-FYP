using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPixelation : MonoBehaviour
{	
    public Material switchStageMaterial;

    // public bool on;

	public int maxPixelNumber = 2000;
    public int pixelationSpeed = 10;
	public int fastSpeedPixel = 500;
    public int fastPixelationSpeed = 200;

    public float brightnessSpeed = 0.02f;

    private IEnumerator coroutine;  // Stop if start up is running

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        // UnityEditor.EditorApplication.update += Update;

        // initial grey radius
        switchStageMaterial.SetFloat("_PixelNumber", 0);
        switchStageMaterial.SetFloat("_Brightness", 0);

        // Beginning shader
        coroutine = StartUp();
        StartCoroutine(coroutine);
    }

    private IEnumerator StartUp()
    {
        while(true)
        {
            int pixelNumber = (int)switchStageMaterial.GetFloat("_PixelNumber");
            float brightness = switchStageMaterial.GetFloat("_Brightness");
            
            if (pixelNumber <= maxPixelNumber)
            {
                int newPixelNumber = (pixelNumber >= fastSpeedPixel) ? pixelNumber + fastPixelationSpeed : pixelNumber + pixelationSpeed;
                switchStageMaterial.SetFloat("_PixelNumber", newPixelNumber);

                // Brightness
                if (pixelNumber < fastSpeedPixel)
                {
                    float newBrightness = brightness + brightnessSpeed;
                    switchStageMaterial.SetFloat("_Brightness", newBrightness);
                }

                yield return new WaitForSeconds(Time.deltaTime);
            }
            else
            {
                break;
            }
        }

        coroutine = null; // start up finish
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, switchStageMaterial);
    }

    // public bool GetOn()
    // {
    //     return on;
    // }

    // public void TurnOnOrOffShader()
    // {
    //     on = !on;
    // }

    // void Update()
    // {
    //     int pixelNumber = (int)switchStageMaterial.GetFloat("_PixelNumber");
    //     float brightness = switchStageMaterial.GetFloat("_Brightness");

    //     if (on)
    //     {
    //         // Pixelation
    //         if (pixelNumber >= 0)
    //         {
    //             // Pixel number
	// 			int newPixelNumber = (pixelNumber >= fastSpeedPixel) ? pixelNumber - fastPixelationSpeed : pixelNumber - pixelationSpeed;
    //             switchStageMaterial.SetFloat("_PixelNumber", newPixelNumber);

    //             // Brightness
    //             if (pixelNumber < fastSpeedPixel)
    //             {
    //                 float newBrightness = brightness - brightnessSpeed;
    //                 switchStageMaterial.SetFloat("_Brightness", newBrightness);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         // To normal
    //         if (pixelNumber <= maxPixelNumber)
    //         {
    //             int newPixelNumber = (pixelNumber >= fastSpeedPixel) ? pixelNumber + fastPixelationSpeed : pixelNumber + pixelationSpeed;
    //             switchStageMaterial.SetFloat("_PixelNumber", newPixelNumber);

    //             // Brightness
    //             if (pixelNumber < fastSpeedPixel)
    //             {
    //                 float newBrightness = brightness + brightnessSpeed;
    //                 switchStageMaterial.SetFloat("_Brightness", newBrightness);
    //             }
    //         }
    //     }
    // }

    public IEnumerator PlayAnimation()
    {
        // Stop start up first
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        while(true)
        {
            int pixelNumber = (int)switchStageMaterial.GetFloat("_PixelNumber");
            float brightness = switchStageMaterial.GetFloat("_Brightness");

            if (pixelNumber >= 0)
            {
                // Pixel number
				int newPixelNumber = (pixelNumber >= fastSpeedPixel) ? pixelNumber - fastPixelationSpeed : pixelNumber - pixelationSpeed;
                switchStageMaterial.SetFloat("_PixelNumber", newPixelNumber);

                // Brightness
                if (pixelNumber < fastSpeedPixel)
                {
                    float newBrightness = brightness - brightnessSpeed;
                    switchStageMaterial.SetFloat("_Brightness", newBrightness);
                }

                yield return new WaitForSeconds(Time.deltaTime);
            }
            else
            {
                break;
            }
        }
        // yield return null;
    }

}