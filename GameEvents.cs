using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameEvents : MonoBehaviour
{
    private const int DIR_TIMER = 5;
    private const int COIN_VALUE = 100000;

    string highScoresPath = "C:\\Users\\maxwe\\OneDrive\\Documents\\Unity Projects\\MyGame\\Assets\\Scripts\\HighScores.csv";
    
    private UIManager uiMan;
    private int gameState = 0;

    private Vector2[] BloodDirections = new Vector2[] {new Vector2(.001f,.001f), new Vector2(0,-.001f), new Vector2(-.001f,.001f), new Vector2(0,-.001f)};
    private int bloodDir = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        uiMan = GetComponent<UIManager>();
        gameState = SceneManager.GetActiveScene().buildIndex;
        
        if (gameState > 0 && SceneManager.GetActiveScene().name != "LoseScene")
        {
            GetComponent<WorldGenerator>().SpawnWorld();
        }

        if (gameState == 1)
        {
            StartCoroutine(ChangeBloodDir());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == 0 && Input.GetKeyUp(KeyCode.P))
        {
            SceneManager.LoadScene("MicroScene");
        } 
    }

    IEnumerator ChangeBloodDir()
    {
        yield return new WaitForSecondsRealtime(DIR_TIMER);
        BloodDirection.dirX = BloodDirections[bloodDir].x;
        BloodDirection.dirY = BloodDirections[bloodDir].y;

        bloodDir++;
        if (bloodDir > 3)
        {
            bloodDir = 0;
        }

        StartCoroutine(ChangeBloodDir());
    }

    public void StartGame()
    {
        GetComponent<WorldGenerator>().SpawnWorld();
    }
    
    public void PlayerDied(int score)
    {
        // add this score to our high-scores list
        using (StreamWriter sw = File.AppendText(highScoresPath)) 
        {
            sw.WriteLine("null" + ',' + score.ToString());
        }
        
        PlayerPrefs.SetInt("NewExtras", score/COIN_VALUE);

        // load lose scene
        SceneManager.LoadScene("LoseScene");
    }

    public void ResetGame()
    {
        // load menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
