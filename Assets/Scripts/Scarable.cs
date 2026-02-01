using UnityEngine;

public class Scarable : MonoBehaviour
{

    [SerializeField] private float visionCone = 0.6f;

    public void OnDrawGizmos()
    {
        Vector3 lookDirection = transform.forward.normalized;

        for (float t = 0; t < 2; t += 0.01f)
        {
            float x = Mathf.Cos(t * Mathf.PI);
            float y = Mathf.Sin(t * Mathf.PI);

            float vision = Vector2.Dot(new Vector2(x, y), -lookDirection);
            if (vision < visionCone)
            {
                Debug.DrawLine(transform.position, transform.position + new Vector3(x, 0, y), Color.coral);
            }

        }
    }

    float susMeter = 0;
    public void Update()
    {


        Scarer[] scarers = FindObjectsByType<Scarer>(FindObjectsSortMode.None);

        Vector3 eye = transform.position + new Vector3(0, 0.5f, 0);


        bool isSus = false;
        foreach (Scarer scare in scarers)
        {
            Vector3 point = scare.transform.position;

            Vector3 rayDirection = (point - eye).normalized;
            Vector3 lookDirection = transform.forward.normalized;

            Debug.DrawLine(transform.position, transform.position + lookDirection, Color.yellow);
            Debug.DrawLine(transform.position, transform.position + rayDirection, Color.blue);

            float vision = Vector2.Dot(rayDirection, lookDirection);


            // Not in the vision 
            if (vision < visionCone)
            {
                continue;
            }

            RaycastHit hit;
            bool anything = Physics.Raycast(eye, rayDirection, out hit, 10);
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
            wasSus = true;
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
            visitor.toughts.text = "AAAAHHAHHAHAHAHAHHAHH!!!!!!";
            FindAnyObjectByType<HealthInspector>().PanicStarts(visitor);
            visitor.LeaveNow();
        }
    }

    bool wasSus;
}
