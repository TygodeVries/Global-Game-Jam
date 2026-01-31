using System.Collections;
using UnityEngine;

public class VisitorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject visitorPrefab;

    public IEnumerator Start()
    {
        while (true)
        {
            Visitor[] visitors = FindObjectsByType<Visitor>(FindObjectsSortMode.None);
            Visitor prefab = visitorPrefab.GetComponent<Visitor>();
            int count = 0;
            foreach (Visitor visitor in visitors)
            {
                if (visitor.visitorType == prefab.visitorType)
                    count++;
            }

            if (count < 2)
            {
                GameObject.Instantiate(visitorPrefab, transform.position, transform.rotation);
            }

            yield return new WaitForSeconds(3f);
        }
    }
}
