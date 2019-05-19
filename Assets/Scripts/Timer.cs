using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public IEnumerator DoCoroutine(IEnumerator cor)
    {
        while (cor.MoveNext())
            yield return cor.Current;
    }

}
