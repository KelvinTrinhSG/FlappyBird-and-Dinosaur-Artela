using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject dinoGameButton;
    private void Start()
    {
        dinoGameButton.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnWalletConnected()
    {
        ScoreMultiplier.Ins.xScore = 2;
        dinoGameButton.SetActive(true);
    }
}
