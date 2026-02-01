using UnityEngine;

public class Bill : MonoBehaviour
{
    Camera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform.position);
        transform.Rotate(180, 0, 180);
    }
}
