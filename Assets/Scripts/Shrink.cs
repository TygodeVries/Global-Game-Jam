using UnityEngine;

public class Shrink : MonoBehaviour
{
    private Distraction distraction;

    public void Start()
    {
        distraction = GetComponent<Distraction>();
    }

    public void Update()
    {
        if (distraction.Range > 0)
            distraction.Range -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        distraction.Range = 3;
    }
}
