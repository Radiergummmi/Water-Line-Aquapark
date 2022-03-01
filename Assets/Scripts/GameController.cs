using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : Singleton<GameController>
{
    public event Action Action_Win;
    public event Action Action_Lose;
    [SerializeField] private Material[] Colors;
    public GameObject EffectCollider;

    public enum Stay
    {
        Defauld,
        Move,
        Win,
        Lose,
    }
    public Stay StayGame = Stay.Defauld;
    private List<GameObject> AllCar = new List<GameObject>();
    private int CountCarsToFinish;
    bool Lose;
    void Start()
    {
      
    }
    public void ChecWinGame()
    {
        CountCarsToFinish++;
        if (CountCarsToFinish == AllCar.Count)
        {
            Action_Win?.Invoke();
        }
    }

    public GameObject SetCar
    {
        set { AllCar.Add(value); }
    }
    public void LoseGame()
    {
        if (!Lose)
        {
            Debug.Log("Lose");
            Action_Lose?.Invoke();
            Lose = true;
        }
    }
    public void MoveAllCar()
    {
        int CountReady = 0;
        for (int i = 0; i < AllCar.Count; i++)
        {
            CountReady += AllCar[i].GetComponent<SlideObject>().GetReady ? 1 : 0;
        }
        if (CountReady == AllCar.Count)
        {
            StayGame = Stay.Move;
            for (int i = 0; i < AllCar.Count; i++)
            {
                AllCar[i].GetComponent<SlideObject>().Move();
            }
        }
    }
    void Update()
    {

    }
}
