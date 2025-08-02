using UnityEngine;
using TMPro;  // If using TextMeshPro

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        Debug.Log("ScoreUI Start called");
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnScoreChanged += UpdateScoreUI;
            // Immediately update score display on start
            UpdateScoreUI(GameStateManager.Instance.score);
        }

        // Initialize UI at startup
        if (scoreText != null && GameStateManager.Instance != null)
            scoreText.text = "SCORE: " + GameStateManager.Instance.score;
    }
    void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnScoreChanged -= UpdateScoreUI;
    }

    private void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + newScore;
    }
}
