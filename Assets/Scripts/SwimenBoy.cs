using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;

public class SwimenBoy : MonoBehaviour
{
    [SerializeField] private GameObject[] BoysModesl;
    Rigidbody rb;
    Collider Col;
    Sequence Mysequence;
    List<Vector3> NewPos;
    private GameObject FinishObject;
    private bool RotateFinishActive = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Col = GetComponent<BoxCollider>();
        for (int i = 0; i < BoysModesl.Length; i++)
        {
            BoysModesl[i].SetActive(false);
        }
        BoysModesl[Random.Range(0, BoysModesl.Length)].SetActive(true);
    }
    public void Move(Vector3[] Pos,GameObject _finish)
    {
        FinishObject = _finish;


        Mysequence = DOTween.Sequence();
        float dist = 0;
        NewPos = Pos.ToList();
        Vector3 StartPos = NewPos[5];

        for (int i = 0; i < 5; i++)
        {
            NewPos.RemoveAt(0);
        }
        
        for (int i = 0; i < NewPos.Count; i++)
        {
            if (i + 1 < NewPos.Count)
            {
                dist += Vector3.Distance(NewPos[i], NewPos[i + 1]);
            }
        }

        Col.enabled = true;
        rb.isKinematic = false;
        Mysequence.Append(transform.DOJump(StartPos, 1, 1, 0.3f))
        .Append(transform.DOPath(NewPos.ToArray(), dist / 3).OnUpdate(() =>
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + Time.deltaTime * 120, 0);
        })
        .SetEase(Ease.Linear)).OnComplete(() =>
        {
            Col.enabled = false;
            rb.isKinematic = true;
            RotateFinishActive = true;
            Vector3 Dir = FinishObject.transform.position - transform.position;

            transform.DOMove(transform.position + (Dir/8), 0.1f);
        });


    }
    private void Update()
    {
        if (RotateFinishActive)
        {
            transform.RotateAround(FinishObject.transform.position, Vector3.up, 180*Time.deltaTime);
        }
    }
    private void LookAtPos(int i)
    {
        if (i > 0)
        {
            Vector3 dir = NewPos[i] - transform.position;
            Quaternion Rotate = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, Rotate, Time.deltaTime * 250);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<SwimenBoy>() || collision.gameObject.tag == "Border" )
        {
            // VibrationController.Instance.VibrateWithTypeHAIDIMPACT();
            rb.isKinematic = true;
            //  rb.AddForce(collision.contacts[0].normal.normalized * 20, ForceMode.Impulse);
            Vector3 Dir = collision.contacts[0].normal.normalized;
            Dir.y = 0;
            Vector3 RandomSphere = Random.insideUnitCircle ;
            RandomSphere.z = RandomSphere.y;
            RandomSphere.y = 0;
            transform.DOJump(transform.position + Dir + RandomSphere, 1, 1, 0.5f);
            Mysequence.Kill();
            GameController.Instance.LoseGame();

            Col.enabled = false;
            rb.isKinematic = true;

            //    Instantiate(GameController.Instance.EffectCollider, pos, rot);
            //  Destroy(gameObject);

        }
    }

}
