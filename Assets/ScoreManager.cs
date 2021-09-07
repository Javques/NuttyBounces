using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int HighScore;
    public static Text scoreText, highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public static void getHighScore(int globalScore)
    {
        if (globalScore > HighScore) HighScore = globalScore;
        
        PlayerPrefs.SetInt("HighScore", HighScore);
       
    }
    void OnGUI()
    {
        GUI.Label(new Rect(0, 5, 100, 50), "HighScore: " + PlayerPrefs.GetInt("HighScore").ToString());

    }
}
