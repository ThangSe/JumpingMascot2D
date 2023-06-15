using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject mainMenuWindow;
    [SerializeField] GameObject tutorialWindow;

    public void PlayButton()
    {
        mainMenuWindow.SetActive(false);
    }

    public void StartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
}
