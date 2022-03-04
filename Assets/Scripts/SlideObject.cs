using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObject : MonoBehaviour
{
    public int Number;
    public bool GoMove;
    private GameObject CurrenLine;
    private Vector3[] NewPos;
    private bool Ready = false;
    Sequence Mysequence;
    Rigidbody rb;
    public Material LineColor;
    public bool Buldozer = false;
    private bool FinishHim = false;
    [SerializeField] private GameObject[] SwimenBoys;
    [SerializeField] private GameObject FinishObject;
    public bool GetReady
    {
        get { return Ready; }
    }

    void Start()
    {

        rb = GetComponent<Rigidbody>();
      GameController.Instance.SetCar = gameObject;
       
    }

    void Update()
    {
       
    }
    public void PathAndLine(Vector3[] Path, GameObject line)
    {
        Ready = true;
        NewPos = Path;
        CurrenLine = line;
    }
    public void Move()
    {
        StartCoroutine(TimerMove());
       
    }
    private IEnumerator TimerMove()
    {
        for (int i = 0; i < SwimenBoys.Length; i++)
        {
            SwimenBoys[i].GetComponent<SwimenBoy>().Move(NewPos, FinishObject);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void DestroyLineAndPath()
    {
        Ready = false;
        NewPos = new Vector3[0];
        Destroy(CurrenLine);
    }
  
}
