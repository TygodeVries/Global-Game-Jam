using UnityEngine;

public class Poisonable : MonoBehaviour
{
    public void TagApplied(Tags tag)
    {
        if (tag == Tags.Poison)
        {
            GetComponent<ParticleSystem>().Play();
        }
    }
}
