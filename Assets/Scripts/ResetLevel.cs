using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public IEnumerator Start()
    {
        for (int i = 10; i > 0; i--)
        {
            GetComponent<TMP_Text>().text = $"Restarting in {i} seconds...";
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(0);
    }
}
