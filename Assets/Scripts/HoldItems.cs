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

    float throwPower;

    public void Update()
    {
        if (pickupAction.IsPressed())
        {
            PowerUpThrow();
        }
        if (pickupAction.WasReleasedThisFrame())
        {
            if (item != null)
                DropItem();
            else
                PickupItem();

            throwPower = 0;
        }
    }

    public void PowerUpThrow()
    {
        throwPower += Time.deltaTime;
        item.GetComponent<Rigidbody>().angularVelocity = new Vector3(4, 4) * Mathf.Clamp(throwPower, 0, 2);
    }

    float reach = 2;

    [HideInInspector] public GameObject item;
    [SerializeField] private GameObject holdPoint;

    public void PickupItem()
    {
        // Check all place points
        foreach (PlacePoint placePoint in placePoints)
        {
            if (placePoint.GetPlacedObject() == null)
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
        item = placePoint.GetPlacedObject();
        item.transform.SetParent(holdPoint.transform);
        item.transform.position = holdPoint.transform.position;

        item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        placePoint.SetPlacedObject(null);
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
        gm.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
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
            if (placePoint.GetPlacedObject() != null)
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
        placePoint.SetPlacedObject(item);
        item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        item = null;
    }

    private void DropOnFloor()
    {
        // Add physics, remove parent
        Rigidbody body = item.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.None;
        body.linearVelocity = ((transform.forward.normalized * 3) + new Vector3(0, 2, 0)) * Mathf.Clamp(throwPower, 0, 2) * 1.5f;

        item.transform.SetParent(null);
        item = null;
    }
}
