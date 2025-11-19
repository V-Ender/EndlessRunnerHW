using UnityEngine;

public class MasterInfo : MonoBehaviour
{
    public static int gemCount = 0;
    [SerializeField] GameObject gemDisplay;
    public static int score = 0;
    [SerializeField] GameObject scoreDisplay;
    
    void Update()
    {
        gemDisplay.GetComponent<TMPro.TMP_Text>().text = "Gems: " + gemCount;
        scoreDisplay.GetComponent<TMPro.TMP_Text>().text = "Score: " + score;
    }
}
