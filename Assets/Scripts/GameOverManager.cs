using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = "Score: " + ((int)GameManager.finalScore).ToString();
    }

    public void OnPressPlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnPressMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
