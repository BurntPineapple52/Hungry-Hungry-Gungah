using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private int highScore = 0;

    private void Start()
    {
        LoadHighScore();
    }

    public bool TrySetNewHighScore(int score)
    {
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
            return true; // New high score is set
        }
        return false; // No new high score
    }

    public int GetHighScore()
    {
        return highScore;
    }


    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}
