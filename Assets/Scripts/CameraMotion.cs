using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMotion : MonoBehaviour
{
    List<PlayerInput> playerInputs = new List<PlayerInput>();
    Vector3 startPoint;

    private void Start()
    {
        startPoint = transform.position;
    }

    public void Join(PlayerInput player)
    {
        playerInputs.Add(player);
    }

    public void Leave(PlayerInput player)
    {
        playerInputs.Remove(player);
    }

    public void Update()
    {
        Vector3 center = new Vector3(0, 0, 0);

        for (int i = 0; i < playerInputs.Count; i++)
        {
            center += playerInputs[i].transform.position;
        }

        center /= playerInputs.Count;

        transform.position = Vector3.Lerp(transform.position, startPoint + center.normalized, Time.deltaTime);
    }
}
