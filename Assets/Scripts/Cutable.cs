using UnityEngine;

public class Cutable : MonoBehaviour
{
    [SerializeField] private Mesh cutMesh;
    public void TagApplied(Tags tag)
    {
        if (tag == Tags.Cut)
        {
            GetComponent<MeshFilter>().mesh = cutMesh;
        }
    }
}
