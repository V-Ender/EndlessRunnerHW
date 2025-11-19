using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    public static int totalGems = 0;
    public static int highScore = 0;

    [SerializeField] GameObject totalGemsDisplay;
    [SerializeField] GameObject highScoreDisplay;

    void Start()
    {
        totalGems = PlayerPrefs.GetInt("TotalGems", 0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        totalGemsDisplay.GetComponent<TMPro.TMP_Text>().text = "Gems: " + totalGems;
        highScoreDisplay.GetComponent<TMPro.TMP_Text>().text = "High Score: " + highScore;
    }

    public void StartGame() 
    {
        SceneManager.LoadScene(1);
    }
}
