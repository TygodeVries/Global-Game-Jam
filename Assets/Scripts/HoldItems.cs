using UnityEngine;
using UnityEngine.InputSystem;

public class HoldItems : MonoBehaviour
{

    PlacePoint[] placePoints;
    InputAction pickupAction;
    private void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        pickupAction = input.actions["Pickup"];
        pickupAction.Enable();

        placePoints = FindObjectsByType<PlacePoint>(FindObjectsSortMode.None);
    }

    public void Update()
    {
        if (pickupAction.WasPressedThisFrame())
        {
            if (item != null)
                DropItem();
            else
                PickupItem();
        }
    }

    float reach = 2;

    [HideInInspector] public GameObject item;
    [SerializeField] private GameObject holdPoint;

    public void PickupItem()
    {
        // Check all place points
        foreach (PlacePoint placePoint in placePoints)
        {
            if (placePoint.placedObject == null)
                continue;

            // See if we can reach it
            float distance = Vector3.Distance(placePoint.transform.position, holdPoint.transform.position);
            if (distance < reach)
            {
                // Place it if we can
                PickupNear(placePoint);
                return;
            }
        }

        // If we are not near a table, drop it on the ground
        PickupFloor();
    }

    private void PickupNear(PlacePoint placePoint)
    {
        item = placePoint.placedObject;
        item.transform.SetParent(holdPoint.transform);
        item.transform.position = holdPoint.transform.position;
        Destroy(item.GetComponent<Rigidbody>());
    }

    private void PickupFloor()
    {
        Rigidbody[] bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
        float nearest = 9999;
        Rigidbody nearestBody = null;
        foreach (Rigidbody body in bodies)
        {
            if (!body.gameObject.CompareTag("Item"))
                continue;

            float distance = Vector3.Distance(holdPoint.transform.position, body.transform.position);
            if (distance < nearest)
            {
                nearestBody = body;
                nearest = distance;
            }
        }

        if (nearest > reach)
            return;

        GameObject gm = nearestBody.gameObject;
        Destroy(gm.GetComponent<Rigidbody>());

        item = gm;
        item.transform.SetParent(holdPoint.transform);
        item.transform.position = holdPoint.transform.position;
    }

    public void DropItem()
    {
        PlacePoint nearestPoint = null;
        float nearestDistance = 99999;

        // Check all place points
        foreach (PlacePoint placePoint in placePoints)
        {
            if (placePoint.placedObject != null)
                continue;

            // See if we can reach it
            float distance = Vector3.Distance(placePoint.transform.position, holdPoint.transform.position);
            if (distance < nearestDistance)
            {
                nearestPoint = placePoint;
                nearestDistance = distance;
            }
        }

        if (nearestDistance < reach)
        {
            PlaceOnNear(nearestPoint);
            return;
        }

        // If we are not near a table, drop it on the ground
        DropOnFloor();
    }

    private void PlaceOnNear(PlacePoint placePoint)
    {
        // Move the item to the position of the 
        item.transform.position = placePoint.transform.position;
        item.transform.SetParent(placePoint.transform);
        placePoint.placedObject = item;
        Destroy(item.GetComponent<Rigidbody>());
        item = null;
    }

    private void DropOnFloor()
    {
        // Add physics, remove parent
        item.AddComponent<Rigidbody>();
        item.transform.SetParent(null);
        item = null;
    }
}
