using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishObject : MonoBehaviour
{
    public int Number;
    [SerializeField] private GameObject WinEffect;

    private void Start()
    {
        StartCoroutine(TimerSra());   
    }
    private IEnumerator TimerSra()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            MeshDeformation.Instance.TargetDeformation(transform.position);
        }
      
    }
    public void ActiveEffect()
    {
        WinEffect.SetActive(true);
    }
}
