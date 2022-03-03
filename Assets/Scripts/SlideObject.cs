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
    public bool GetReady
    {
        get { return Ready; }
    }

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        //for (int i = 0; i < Cars.Length; i++)
        //{
        //    Cars[i].SetActive(false);
        //}
        //if (!Buldozer)
        //{
        //    Cars[PlayerPrefs.GetInt("NumberCar", 0)].SetActive(true);
           GameController.Instance.SetCar = gameObject;
        //}
       
    }

    // Update is called once per frame
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
        //Mysequence = DOTween.Sequence();
        //float dist = 0;
        //for (int i = 0; i < NewPos.Length; i++)
        //{
        //    if (i + 1 < NewPos.Length)
        //    {
        //        dist += Vector3.Distance(NewPos[i], NewPos[i + 1]);
        //    }
        //}
        //Mysequence.Append(transform.DOPath(NewPos, dist / 3).OnWaypointChange(LookAtPos).SetEase(Ease.Linear));
    }
    private IEnumerator TimerMove()
    {
        for (int i = 0; i < SwimenBoys.Length; i++)
        {
            SwimenBoys[i].GetComponent<SwimenBoy>().Move(NewPos);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void DestroyLineAndPath()
    {
        Ready = false;
        NewPos = new Vector3[0];
        Destroy(CurrenLine);
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FinishObject>() && other.gameObject.GetComponent<FinishObject>().Number == Number && !FinishHim)
        {
            FinishHim = true;
          //  other.gameObject.GetComponent<FinishObject>().ActiveEffect();
           GameController.Instance.ChecWinGame();
            rb.useGravity = true;
          //  VibrationController.Instance.VibrateWithTypeLIGHTIMPACT();
            // Mysequence.Kill();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!Buldozer)
        {
            if (collision.gameObject.GetComponent<SlideObject>() || collision.gameObject.tag == "Border" || collision.gameObject.tag == "Box" || collision.gameObject.tag == "NewBorder")
            {
               // VibrationController.Instance.VibrateWithTypeHAIDIMPACT();
                rb.useGravity = true;
                rb.AddForce(collision.contacts[0].normal.normalized * 20, ForceMode.Impulse);
                Mysequence.Kill();
                GameController.Instance.LoseGame();

                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
            //    Instantiate(GameController.Instance.EffectCollider, pos, rot);
                //  Destroy(gameObject);

            }
        }
    }
}
