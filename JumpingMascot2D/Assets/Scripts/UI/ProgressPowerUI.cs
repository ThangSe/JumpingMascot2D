using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPowerUI : MonoBehaviour
{
    private const float HOLD_DURATION_BASE = 0f;
    private float holdDuration = HOLD_DURATION_BASE;
    [SerializeField] private Image barImageMax;
    [SerializeField] private Image barImageMin;
    [SerializeField] private Image barImageFill;
    private float multipler = 2f;
    private float maxHoldValue = 4f;

    private void Start()
    {
        Player.Instance.OnTriggerCol += Player_OnTriggerCol;
        GameInput.Instance.OnReleaseButton += GameInput_OnReleaseButton;
    }

    private void GameInput_OnReleaseButton(object sender, GameInput.OnReleaseButtonEventArgs e)
    {
        if(Player.Instance.IsGrounded())
        {
            holdDuration = HOLD_DURATION_BASE;
            barImageFill.fillAmount = holdDuration;
        }
    }

    private void Player_OnTriggerCol(object sender, Player.OnTriggerColEventArgs e)
    {
        barImageMin.fillAmount = e.minPercent;
        barImageMax.fillAmount = e.maxPercent;
    }

    private void IncreaseHoldDuration()
    {
        if (Player.Instance.IsHold())
        {
            holdDuration += Time.deltaTime;
            barImageFill.fillAmount = holdDuration * multipler / maxHoldValue;
        }
    }

    private void Update()
    {
        IncreaseHoldDuration();
    }
}
