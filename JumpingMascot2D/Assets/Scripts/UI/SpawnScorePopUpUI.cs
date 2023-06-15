using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnScorePopUpUI : MonoBehaviour
{
    public static void SpawnPopUp(Vector3 position)
    {
        Transform scorePopUpTransform = Instantiate(GameAssets.i.pfScorePopup, position, Quaternion.identity);
        SpawnScorePopUpUI scorePopUp = scorePopUpTransform.GetComponent<SpawnScorePopUpUI>();
        scorePopUp.SetUp();
    }

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private Vector3 moveVector;
    private int incScore;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        incScore = GameAssets.i.incScoreAmount;
    }
    public void SetUp()
    {
        Color newColor;
        ColorUtility.TryParseHtmlString("#FFFFFF", out newColor);
        textColor = newColor;
        textMesh.SetText("+" + incScore.ToString());
        textMesh.color = newColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        moveVector = Vector3.up * 20f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a <0)
            {
                Destroy(gameObject);
            }
        }
    }
}
