using System.Collections;
using UnityEngine;

public class HealthInspector : MonoBehaviour
{
    public GameObject fireStarter;
    public GameObject scoreCanvas;

    public bool isComing = false;
    public void PanicStarts(Visitor starter)
    {
        StartCoroutine(C_Panic(starter));
    }
    private IEnumerator C_Panic(Visitor starter)
    {
        if (isComing)
            yield break;

        isComing = true;
        GetComponent<Animator>().SetTrigger("Start");
        FindAnyObjectByType<Camera>().GetComponent<CameraMotion>().SetTarget(starter.transform);

        yield return new WaitForSeconds(3);
        foreach (Visitor visitor in FindObjectsByType<Visitor>(FindObjectsSortMode.None))
        {
            visitor.StopAllCoroutines();
            visitor.LeaveNow();
        }

        GameObject gm = null;
        for (int i = 0; i < 3; i++)
        {
            gm = GameObject.Instantiate(fireStarter);
        }

        yield return new WaitForSeconds(3);

        FindAnyObjectByType<Camera>().GetComponent<CameraMotion>().SetTarget(gm.transform);

        yield return new WaitForSeconds(2);
        FindAnyObjectByType<Camera>().GetComponent<CameraMotion>().SetTarget(null);

        yield return new WaitForSeconds(20);
        scoreCanvas.SetActive(true);

        foreach (PlayerMovement playerMovement in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
        {
            Destroy(playerMovement.gameObject);
        }

        Destroy(gameObject);

    }
}
