using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroupUI;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private Image Star1;
    [SerializeField] private Image Star2;
    private Color textColor;

    private const int SCORE_MAX = 100;
    private const string PLAYER_PREFS_SCORE = "TotalScore";
    private int score;

    private void Awake()
    {
        score = PlayerPrefs.GetInt(PLAYER_PREFS_SCORE);
    }

    private void Start()
    {
        if(score == SCORE_MAX) {
            SoundManager.Instance.PlayWinningSound();
            Star1.color = GetColor();
            Star2.color = GetColor();
            textMesh.SetText("YOU WIN");
        } else if(score < SCORE_MAX)
        {
            float percentTwoStar = .75f;
            if(score >= SCORE_MAX * percentTwoStar) Star1.color = GetColor();
            textMesh.SetText("YOU LOSE");
        }
        ScoreText.SetText(score.ToString());
    }

    private Color GetColor()
    {
        Color newColor;
        ColorUtility.TryParseHtmlString("#FFFFFF", out newColor);
        textColor = newColor;
        return textColor;
    }
    public void PlayAgainButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SubmitButton()
    {
        Debug.Log("Submit");
    }
}
