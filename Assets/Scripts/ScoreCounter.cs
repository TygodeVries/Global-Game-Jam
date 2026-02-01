using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    int fires = 0;
    public void AddFire()
    {
        fires++;
    }

    public void UpdateBoard()
    {
        float destroyed = fires / 20f;
        scoreText.text = $"Evidence Destroyed: {Mathf.Round(destroyed * 100)}%";
    }
}
