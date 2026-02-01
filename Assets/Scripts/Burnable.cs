using UnityEngine;

public class Burnable : MonoBehaviour
{
    [SerializeField] private Material cookedMaterial;
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private int materialCount;
    public void TagApplied(Tags tag)
    {
        if (tag == Tags.Cooked)
        {
            GetComponent<MeshRenderer>().materials[materialCount] = cookedMaterial;
        }

        if (tag == Tags.Burned)
        {
            GetComponent<MeshRenderer>().materials[materialCount] = burnedMaterial;
        }
    }
}
