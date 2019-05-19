using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToon : MonoBehaviour
{	
    public Material toonMaterial;
	private int minPartition = 4;
	private int maxPartition = 12;
    public int speed = 1;    

    private bool on;
	private bool fadeIn = true;

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        // UnityEditor.EditorApplication.update += Update;

        // initial variable
        toonMaterial.SetFloat("_Partition", maxPartition);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, toonMaterial);
    }

    public bool GetOn()
    {
        return on;
    }

    public void TurnOnOrOffShader()
    {
        if (on)
        {
            Debug.LogWarning("Usage: toon shader running");
            return;
        }

        on = !on;
    }

    void Update()
    {
        if (on)
        {
            float partition = toonMaterial.GetFloat("_Partition");
			// float newPartition = partition ;

            if (fadeIn)	// Decrease partition to 4
            {
				float newPartition = partition - speed * Time.unscaledDeltaTime;

                if (newPartition >= minPartition)	// fading in
                {
					toonMaterial.SetFloat("_Partition", newPartition);
                }
				else	// finish fade in
				{
					on = false;
					fadeIn = false;

					toonMaterial.SetFloat("_Partition", minPartition);
				}
            }
            else	// Increase partition to 64
            {
				float newPartition = partition + speed * Time.unscaledDeltaTime;
				
                if (newPartition <= maxPartition)	// fading out
                {
					toonMaterial.SetFloat("_Partition", newPartition);
                }
				else	// finish fade out
				{
					on = false;
					fadeIn = true;

					toonMaterial.SetFloat("_Partition", maxPartition);
				}
            }
        }
    }

}