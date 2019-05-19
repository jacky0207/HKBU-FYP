using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAnimation : MonoBehaviour
{	
    private Material material;
    // public bool on;
    public float speed = 0.01f;

	private void Awake()
	{
		material = GetComponent<Renderer>().material;
	}

    void Start()
    {
        // initialize material variable
        material.SetFloat("_Process", 0);
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
    //     float process = material.GetFloat("_Process");
    //     if (on)
    //     {
    //         if (process <= 1)
    //         {
	// 			material.SetFloat("_Process", process + speed);
    //         }
    //     }
    // }

    public IEnumerator PlayAnimation()
    {
        while(true)
        {
            float process = material.GetFloat("_Process");
            if (process <= 1)
            {
                material.SetFloat("_Process", process + speed);
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
