using System.Collections.Generic;
using UnityEngine;

public class Scarable : MonoBehaviour
{
    [SerializeField] private float visionSize = 10;
    [SerializeField] private float visionCone = 0.6f;
    [SerializeField] private MeshFilter coneFilter;
    public void OnDrawGizmos()
    {
        return;
        Vector3 lookDirection = transform.forward.normalized;
        Vector2 lookDirection2 = new Vector2(lookDirection.x, lookDirection.z);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + lookDirection);

        for (float t = 0; t < 2; t += 0.01f)
        {
            float x = Mathf.Cos(t * Mathf.PI);
            float y = Mathf.Sin(t * Mathf.PI);

            if (InVision(new Vector3(x, 0, y)))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + (new Vector3(x, 0, y) * visionSize));
            }
        }
    }


    [SerializeField] private MeshRenderer coneRenderer;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color susColor;

    public void Destroy()
    {
        Destroy(coneRenderer);
        Destroy(this);
    }

    public void UpdateMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(transform.InverseTransformPoint(transform.position));

        for (float t = 0; t < 2; t += 0.02f)
        {
            float x = Mathf.Cos(t * Mathf.PI);
            float y = Mathf.Sin(t * Mathf.PI);

            if (InVision(new Vector3(x, 0, y)))
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, new Vector3(x, 0, y), out hit, visionSize))
                {
                    vertices.Add(transform.InverseTransformPoint(hit.point));
                }
                else
                {
                    vertices.Add(transform.InverseTransformPoint(transform.position + (new Vector3(x, 0, y) * visionSize)));
                }
            }
        }

        mesh.vertices = vertices.ToArray();

        int trisCount = vertices.Count - 2;
        int[] triangles = new int[trisCount * 3];

        for (int i = 0; i < trisCount; i++)
        {
            triangles[(i * 3) + 0] = 0;
            triangles[(i * 3) + 2] = i + 1;
            triangles[(i * 3) + 1] = i + 2;
        }

        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        coneFilter.mesh = mesh;
    }

    public bool InVision(Vector3 shotDirection)
    {
        Vector2 ld2d = new Vector2(transform.forward.x, transform.forward.z);

        float vision = Vector2.Dot(new Vector2(shotDirection.x, shotDirection.z), -ld2d);
        return vision < visionCone;
    }

    float susMeter = 0;
    public void Update()
    {
        UpdateMesh();

        coneRenderer.material.color = Color.Lerp(normalColor, susColor, susMeter);

        Scarer[] scarers = FindObjectsByType<Scarer>(FindObjectsSortMode.None);

        Vector3 eye = transform.position + new Vector3(0, 0.5f, 0);


        bool isSus = false;
        foreach (Scarer scare in scarers)
        {
            Vector3 point = scare.transform.position;

            Vector3 rayDirection = (point - eye).normalized;

            if (!InVision(rayDirection))
            {
                Debug.DrawLine(transform.position, transform.position + rayDirection, Color.white);
                continue;
            }

            Debug.DrawLine(transform.position, transform.position + rayDirection, Color.green);


            RaycastHit hit;
            bool anything = Physics.Raycast(eye, rayDirection, out hit, visionSize);
            if (!anything)
            {
                continue;
            }

            if (hit.collider.gameObject == scare.gameObject)
            {
                Debug.DrawLine(eye, hit.point, Color.green);
                isSus = true;
            }
            else
            {
                Debug.DrawLine(eye, hit.point, Color.red);
            }
        }

        if (isSus)
        {
            susMeter += Time.deltaTime;
        }
        else if (susMeter > 0)
        {
            susMeter -= Time.deltaTime;
        }

        if (susMeter > 0)
        {
            if (!wasSus)
            {
                what.Play();
                Debug.LogWarning("What??");
                wasSus = true;
            }
            GetComponent<Visitor>().toughts.text = "What is that...";
        }
        else if (wasSus)
        {
            wasSus = false;
            GetComponent<Visitor>().toughts.text = "Must have been the wind..";
        }

        if (susMeter > 1)
        {
            Visitor visitor = GetComponent<Visitor>();
            visitor.gameObject.GetComponentInChildren<Animator>().SetBool("Panic", true);

            FindAnyObjectByType<HealthInspector>().PanicStarts(visitor);
            Destroy(coneRenderer);
            visitor.LeaveNow();
        }
    }

    bool wasSus = false;

    [SerializeField] private AudioSource what;
}
