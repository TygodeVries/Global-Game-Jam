using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private GameObject lights;

    int count = 0;
    public void OnTriggerEnter(Collider other)
    {
        count++;
        lights.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        count--;

        if (count == 0)
            lights.SetActive(false);
    }
}
