using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    public Slider calSlider;

    public Text extrasEarned;
    
    public List <Text> topScoreText = new List<Text>();
    
    List<int> sortedScores = new List<int>();

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            UpdateTopScores();
        } else if (SceneManager.GetActiveScene().name == "LoseScene")
        {
            UpdateExtras();
        }
    }

    public void UpdateExtras()
    {
        extrasEarned.text = PlayerPrefs.GetInt("NewExtras").ToString() + " Extras";
    }
    
    public void UpdateTopScores()
    {
        StreamReader strReader = new StreamReader("C:\\Users\\maxwe\\OneDrive\\Documents\\Unity Projects\\MyGame\\Assets\\Scripts\\HighScores.csv");
        bool endOfFile = false;
        // skip a line
        strReader.ReadLine();
        while (!endOfFile)
        {
            string data = strReader.ReadLine();
            if (data == null)
            {
                endOfFile = true;
                break;
            }

            var dataValues = data.Split(',');

            sortedScores.Add(int.Parse(dataValues[1]));
        }

        // sorted scores are in ascending
        sortedScores.Sort();
        // reverse the list for descending values
        sortedScores.Reverse();

        for (int i = 0; i < 5; i++)
        {
            if (sortedScores[i] > 0)
            {
                topScoreText[i].text = sortedScores[i].ToString();   
            }
            else
            {
                topScoreText[i].gameObject.SetActive(false);
            }
        }
    }
    
    public void UpdateScore(int newCals, int totalCals, int reqCals)
    {
        score += newCals;
        scoreText.text = score.ToString();

        calSlider.value = (float)totalCals / reqCals;
    }
}
