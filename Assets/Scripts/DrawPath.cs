using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class DrawPath : MonoBehaviour
{
    private bool flag = false;
    private Vector3 endPoint;
    public float duration = 50.0f;
    private float yAxis;
    public GameObject CurrentSlide;
    [SerializeField] private GameObject Line;
    private LineRenderer CurrentLine;
    private Vector3 LineLastPos;
    private List<Vector3> ArrayLinePos = new List<Vector3>();
    private float numberCar;
    private bool FinishToFind = false;

  
    void Start()
    {

    }
    private void Update()
    {
        if(GameController.Instance.StayGame == GameController.Stay.Defauld )
            CreateLine();
    }
    private void CreateLine()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) )
            {
                if (hit.collider.gameObject.GetComponent<SlideObject>() && !hit.collider.gameObject.GetComponent<SlideObject>().GetReady)
                {
                    MeshDeformation.Instance.MouseClick();
                    CurrentSlide = hit.collider.gameObject;
                    numberCar = (float)CurrentSlide.GetComponent<SlideObject>().Number / 1000;
                    CurrentSlide.GetComponent<SlideObject>().DestroyLineAndPath();

                    ArrayLinePos.Clear();
                    yAxis = CurrentSlide.transform.position.y;
                    var line = Instantiate(Line, hit.point, Line.transform.rotation);
                    CurrentLine = line.GetComponent<LineRenderer>();
                    CurrentLine.material = CurrentSlide.GetComponent<SlideObject>().GetMaterialLine;
                    CurrentLine.SetPosition(0, hit.point);
                    CurrentLine.SetPosition(1, hit.point);
                    LineLastPos = hit.point;
                }
            }
        }
        if (Input.GetMouseButton(0) && CurrentSlide && !FinishToFind)
        {
            RaycastHit hit;
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) )
            {

                flag = true;
                endPoint = hit.point;
                endPoint.y = yAxis;
                bool Distans = Vector3.Distance(LineLastPos, endPoint) > 0.06f && Vector3.Distance(LineLastPos, endPoint) < 1f;
                if (Distans && hit.collider.gameObject.tag == "Plane")
                {
                    MeshDeformation.Instance.ActiveMeshDeformation();
                    ArrayLinePos.Add(endPoint);
                    CurrentLine.positionCount = ArrayLinePos.Count;
                    for (int i = 0; i < ArrayLinePos.Count; i++)
                    {
                        CurrentLine.SetPosition(i, new Vector3(ArrayLinePos[i].x, -0.15f + numberCar, ArrayLinePos[i].z));
                    }
                    LineLastPos = endPoint;
                }
                if (hit.collider.gameObject.GetComponent<FinishObject>() && CurrentSlide.GetComponent<SlideObject>().Number == hit.collider.gameObject.GetComponent<FinishObject>().Number && Distans)
                {
                    FinishToFind = true;

                  //  ArrayLinePos.Add(hit.collider.gameObject.transform.position);
                   // CurrentLine.positionCount = ArrayLinePos.Count;
                    //for (int i = 0; i < ArrayLinePos.Count; i++)
                    //{
                    //    CurrentLine.SetPosition(i, new Vector3(ArrayLinePos[i].x, -0.15f + numberCar, ArrayLinePos[i].z));
                    //}
                }
                //VibrationController.Instance.VibrateWithTypeSelection();
            }
        }
        if (Input.GetMouseButtonUp(0) && flag)
        {
            RaycastHit hit;
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (FinishToFind)
            {
                Vector3[] Array = ArrayLinePos.ToArray();
                CurrentSlide.GetComponent<SlideObject>().PathAndLine(Array, CurrentLine.gameObject);
               GameController.Instance.MoveAllCar();
                FinishToFind = false;
            }
            else if (!CurrentSlide.GetComponent<SlideObject>().Buldozer)
            {
                Destroy(CurrentLine.gameObject); //сделать анимацию уменьшения
                MeshDeformation.Instance.CliearVerticies();
            }
            else
            {
                Vector3[] Array = ArrayLinePos.ToArray();
                CurrentSlide.GetComponent<SlideObject>().PathAndLine(Array, CurrentLine.gameObject);
                //  FinishToFind = false;
                CurrentSlide.GetComponent<SlideObject>().Move();
            }

            flag = false;
            CurrentSlide = null;
            CurrentLine = null;
        }

    }
}
