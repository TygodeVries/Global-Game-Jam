using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    int score = 0;

    [SerializeField] private TMP_Text scoreText;

    public void Give(int points)
    {
        score += points;

        scoreText.text = $"{score}";
    }
}
