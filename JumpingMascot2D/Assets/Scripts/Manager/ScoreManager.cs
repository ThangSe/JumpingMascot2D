using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public event EventHandler OnScoreChanged;

    private int score;
    private int increaseAmount;
    private const int SCORE_MAX = 100;
    private const string PLAYER_PREFS_SCORE = "TotalScore";

    private void Awake()
    {
        Instance = this;
        increaseAmount = GameAssets.i.incScoreAmount;
    }

    private void Start()
    {
        Player.Instance.IncreaseScore += Player_IncreaseScore;
        Player.Instance.OnDead += Player_OnDead;
    }

    private void Player_IncreaseScore(object sender, System.EventArgs e)
    {
        IncScore();
        if(score == SCORE_MAX)
        {
            LoadEndScene();
        } else
        {
            OnScoreChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Player_OnDead(object sender, System.EventArgs e)
    {
        LoadEndScene();
    }

    private void LoadEndScene()
    {
        PlayerPrefs.SetInt(PLAYER_PREFS_SCORE, score);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameOverScene");
    }

    public float ScoreNormalized()
    {
        return (float)score / SCORE_MAX;
    }

    public float GetScore()
    {
        return score;
    }

    private void IncScore()
    {
        score += increaseAmount;
    }
    public bool IsLastJump()
    {
        return score == SCORE_MAX - increaseAmount ? true : false;
    }
}
