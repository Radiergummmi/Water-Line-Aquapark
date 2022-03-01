using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishObject : MonoBehaviour
{
    public int Number;
    [SerializeField] private GameObject WinEffect;
    public void ActiveEffect()
    {
        WinEffect.SetActive(true);
    }
}
