using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeaderScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image progressScore;

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += ScoreManager_OnScoreChanged;
    }

    private void ScoreManager_OnScoreChanged(object sender, System.EventArgs e)
    {
        textMesh.SetText(ScoreManager.Instance.GetScore().ToString());
        progressScore.fillAmount = ScoreManager.Instance.ScoreNormalized();
    }

}
