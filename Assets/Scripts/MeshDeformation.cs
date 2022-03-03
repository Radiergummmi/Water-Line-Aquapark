using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformation : Singleton<MeshDeformation>
{
    [Range(0.1f, 5)]
    public float Radius = 2;

    [Range(0.1f, 5)]
    public float DeformationString = 2;


    [SerializeField] private float Size;
    private Mesh m_Mesh;

    private Vector3[]  ModifiedVerts;
    private List<Vector3> DopVerticies = new List<Vector3>();
    private void Start()
    {
        m_Mesh = GetComponentInChildren<MeshFilter>().mesh;
      
        ModifiedVerts = m_Mesh.vertices;

    }
    private void RecalculateMesh()
    {
        m_Mesh.vertices = ModifiedVerts;
        GetComponentInChildren<MeshCollider>().sharedMesh = m_Mesh;
        m_Mesh.RecalculateNormals();
    }
    private void Update()
    {
       
    }
    public void MouseClick()
    {
        DopVerticies = new List<Vector3>();
        for (int i = 0; i < ModifiedVerts.Length; i++)
        {
            DopVerticies.Add(ModifiedVerts[i]);
        }
    }
    public void ActiveMeshDeformation()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            for (int i = 0; i < ModifiedVerts.Length; i++)
            {
                Vector3 Distance = ModifiedVerts[i] - hit.point;
                float force = DeformationString / (1f + hit.point.sqrMagnitude);

                if (Distance.sqrMagnitude < Radius)
                {

                    if (Input.GetMouseButton(0))
                    {
                        if (ModifiedVerts[i].y > -Size)
                        {
                            // ModifiedVerts[i] = ModifiedVerts[i] + (Vector3.down * force) / smoothingFactor;
                            Vector3 Vert = ModifiedVerts[i];
                            Vector3 Center = hit.point;
                            // Vert.y = Center.y = 0 ;

                            float dist = Vector3.Distance(Vert, Center);

                            if (dist < Radius)
                            {
                                float NewY = -Size * (Radius - dist / Radius);
                                if (ModifiedVerts[i].y > NewY)
                                {
                                    ModifiedVerts[i].y = NewY;
                                }
                            }
                        }
                    }
                }
            }
        }
        RecalculateMesh();
    }
    public void CliearVerticies()
    {
        ModifiedVerts = DopVerticies.ToArray();
        RecalculateMesh();
    }
}
