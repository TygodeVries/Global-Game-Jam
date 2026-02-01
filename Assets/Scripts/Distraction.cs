using UnityEngine;

public class Distraction : MonoBehaviour
{
    [SerializeField] public float Range;
    [SerializeField] private Transform Goal;

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    public void Update()
    {
        Visitor[] visitors = FindObjectsByType<Visitor>(FindObjectsSortMode.None);


        foreach (Visitor visitor in visitors)
        {
            if (visitor.visitorType == VisitorType.Monster)
                continue;

            float distance = Vector3.Distance(visitor.transform.position, transform.position);
            if (distance < Range)
            {
                visitor.Distract(Goal, transform, 1);
            }
        }
    }
}
