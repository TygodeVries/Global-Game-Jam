using UnityEngine;

public class Scarable : MonoBehaviour
{

    float susMeter = 0;
    public void Update()
    {

        Scarer[] scarers = FindObjectsByType<Scarer>(FindObjectsSortMode.None);

        Vector3 eye = transform.position + new Vector3(0, 0.5f, 0);


        bool isSus = false;
        foreach (Scarer scare in scarers)
        {
            Vector3 point = scare.transform.position;

            RaycastHit hit;
            bool anything = Physics.Raycast(eye, point - eye, out hit, 10);
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
            visitor.toughts.text = "AAAAHHAHHAHAHAHAHHAHH!!!!!!";
            FindAnyObjectByType<HealthInspector>().PanicStarts(visitor);
            visitor.LeaveNow();
        }
    }

    bool wasSus;
}
