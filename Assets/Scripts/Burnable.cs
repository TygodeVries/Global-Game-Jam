using UnityEngine;

public class Burnable : MonoBehaviour
{
    [SerializeField] private Material cookedMaterial;
    [SerializeField] private Material burnedMaterial;

    public void TagApplied(Tags tag)
    {
        if (tag == Tags.Cooked)
        {
            GetComponent<MeshRenderer>().material = cookedMaterial;
        }

        if (tag == Tags.Burned)
        {
            GetComponent<MeshRenderer>().material = burnedMaterial;
        }
    }
}
