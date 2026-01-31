using UnityEngine;

public class Knive : MonoBehaviour
{
    public void Update()
    {
        Visitor[] visitors = FindObjectsByType<Visitor>(FindObjectsSortMode.None);

        foreach (Visitor visitor in visitors)
        {
            Vector3 v = visitor.transform.position;
            v.y = 0;

            Vector3 m = transform.position;
            m.y = 0;

            if (Vector3.Distance(v, m) < 1)
            {
                GetComponentInChildren<ParticleSystem>().Play();
                visitor.Die();
            }
        }
    }
}
