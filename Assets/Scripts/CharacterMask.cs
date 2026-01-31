using UnityEngine;

public class CharacterMask : MonoBehaviour
{
    [SerializeField] private GameObject mask;

    public void Update()
    {
        if (transform.position.x > 0)
        {
            mask.SetActive(true);
        }
        else
        {
            mask.SetActive(false);
        }
    }
}
