using UnityEngine;
using UnityEngine.Events;

public class PlacePoint : MonoBehaviour
{

    GameObject placed;

    public void SetPlacedObject(GameObject gm)
    {
        placed = gm;

        OnObjectChanged.Invoke(placed);

    }

    public GameObject GetPlacedObject()
    {
        return placed;
    }

    [SerializeField] UnityEvent<GameObject> OnObjectChanged;
}
