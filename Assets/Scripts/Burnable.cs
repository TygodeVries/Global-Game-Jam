using System.Collections.Generic;
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
            List<Material> materials = new List<Material>();
            GetComponent<MeshRenderer>().GetMaterials(materials);
            materials[materialCount] = cookedMaterial;
            GetComponent<MeshRenderer>().SetMaterials(materials);
        }

        if (tag == Tags.Burned)
        {
            List<Material> materials = new List<Material>();
            GetComponent<MeshRenderer>().GetMaterials(materials);
            materials[materialCount] = burnedMaterial;
            GetComponent<MeshRenderer>().SetMaterials(materials);
        }
    }
}
