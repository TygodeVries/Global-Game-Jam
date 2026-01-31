using UnityEngine;

public class FIreStarter : MonoBehaviour
{
    [SerializeField] GameObject fire;
    private void Start()
    {
        lastDropPlace = transform.position;
    }

    Vector3 lastDropPlace;
    public void Update()
    {
        if (Vector3.Distance(lastDropPlace, transform.position) > 1)
        {
            GameObject.Instantiate(fire, transform.position, Quaternion.identity);
            lastDropPlace = transform.position;
        }
    }
}
