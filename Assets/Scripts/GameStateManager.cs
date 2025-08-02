using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public event Action<int> OnScoreChanged;

    // Inspector-editable values
    [SerializeField] private int bigAsteroidScore = 20;
    [SerializeField] private int mediumAsteroidScore = 50;
    [SerializeField] private int smallAsteroidScore = 100;

    public int BigAsteroidScore => bigAsteroidScore;
    public int MediumAsteroidScore => mediumAsteroidScore;
    public int SmallAsteroidScore => smallAsteroidScore;

    // Game state variables
    public int lives = 3;
    public int score = 0;
    public int level = 1;

    // Example: unlocked weapons, can expand as needed
    public bool hasSpreadShot = false;
    public bool hasLaser = false;

    void Awake()
    {
        // Singleton pattern: only one instance allowed!
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Example: Methods to change state
    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);  // Fire the event
    }

    public void LoseLife()
    {
        lives--;
        // Game over check could go here
    }

    public void NextLevel()
    {
        level++;
        // Spawn new asteroids, etc.
    }
}
