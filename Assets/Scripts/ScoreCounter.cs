using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TMP_Text finalText;
    [SerializeField] TMP_Text scoreText;
    int fires = 0;
    int score = 0;
    int currentShownScore = 0;
    public void AddFire()
    {
        fires++;
        UpdateBoard();
    }

    public void AddScore(int score)
    {
        this.score += score;
        UpdateBoard();
    }

    public void UpdateBoard()
    {
        float destroyed = fires / 40f;
        finalText.text = $"Evidence Destroyed: {Mathf.Round(Mathf.Clamp01(destroyed) * 100f)}%\n{score} Points!";
    }

    IEnumerator Start()
    {
        while (true)
        {
            if (score - currentShownScore > 100)
            {
                currentShownScore += 100;
            }

            if (score - currentShownScore > 1000)
            {
                currentShownScore += 1000;
            }

            if (currentShownScore != score)
            {
                currentShownScore++;
                scoreText.text = $"{currentShownScore}";
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
