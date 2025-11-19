using UnityEngine;

public static class SaveManager
{
    public static void SaveHighScore(int score)
    {
        int current = PlayerPrefs.GetInt("HighScore", 0);
        if (score > current)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public static void AddGems(int gems)
    {
        int current = PlayerPrefs.GetInt("TotalGems", 0);
        PlayerPrefs.SetInt("TotalGems", current + gems);
        PlayerPrefs.Save();
    }

    public static int GetTotalGems()
    {
        return PlayerPrefs.GetInt("TotalCGems", 0);
    }
}

