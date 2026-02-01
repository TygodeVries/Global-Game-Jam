using System.Collections;
using UnityEngine;

public class VisitorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject visitorPrefab;
    [SerializeField] private GameObject warning;
    public IEnumerator Start()
    {
        while (true)
        {


            if (FindAnyObjectByType<HealthInspector>().isComing)
                yield break;

            Visitor[] visitors = FindObjectsByType<Visitor>(FindObjectsSortMode.None);
            Visitor prefab = visitorPrefab.GetComponent<Visitor>();
            int count = 0;
            foreach (Visitor visitor in visitors)
            {
                if (visitor.visitorType == prefab.visitorType)
                    count++;
            }

            if (count == 0) // 0, always spawn!
            {
                yield return Spawn();
            }

            if (count == 1)
            {
                if (Random.Range(0, 10) < 3)
                {
                    yield return Spawn();
                }
            }

            if (count == 2)
            {
                if (Random.Range(0, 20) < 3)
                {
                    yield return Spawn();
                }
            }

            yield return new WaitForSeconds(5f);
        }
    }

    public IEnumerator Spawn()
    {
        Debug.Log("Warn...");
        warning.SetActive(true);
        yield return new WaitForSeconds(3);
        Debug.Log("Spawn...");
        GameObject.Instantiate(visitorPrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        warning.SetActive(false);
    }
}
