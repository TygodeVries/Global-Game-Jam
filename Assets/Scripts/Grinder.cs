using System.Collections.Generic;
using UnityEngine;

public class Grinder : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawns;
    [SerializeField] private Transform output;
    [SerializeField] private ParticleSystem blood;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Grindable>() != null)
        {
            blood.Play();
            Destroy(collision.gameObject);

            for (int i = 0; i < 5; i++)
            {
                GameObject spawn = spawns[Random.Range(0, spawns.Count)];
                GameObject.Instantiate(spawn, output.position, Quaternion.identity);
            }
        }
        else
        {
            collision.gameObject.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, -3);
        }
    }
}
