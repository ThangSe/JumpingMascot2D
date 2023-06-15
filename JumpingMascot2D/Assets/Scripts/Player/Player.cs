using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnTriggerColEventArgs> OnTriggerCol;

    public class OnTriggerColEventArgs : EventArgs
    {
        public float minPercent;
        public float maxPercent;
    }
    public event EventHandler IncreaseScore;
    public event EventHandler OnDead;

    private float maxHoldValue = 4f;
    private float rangeCorrectValue = .7f;
    private float distanceToNextCube = 0;
    private Vector3 nextPointPosition;
    private Vector3 currentPosition;
    private Vector3 dir = Vector3.zero;

    private Rigidbody2D rb;
    private float maxJumpHeight = 0f;
    private float gravity = -9.8f;
    private Vector3 fallPosition;
    private float timeJumpUp;

    private bool isHold;
    private bool isGrounded = true;
    private bool isFall;
    private bool isDead;

    private bool firstSpawn;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = GameObject.Find("Cube").transform.GetChild(0).transform.position;
        GameInput.Instance.OnHoldButton += GameInput_OnHoldButton;
        GameInput.Instance.OnReleaseButton += GameInput_OnReleaseButton;
    }

    private void GameInput_OnHoldButton(object sender, EventArgs e)
    {
        if (IsGrounded())
        {
            isHold = true;
        }
    }

    private void GameInput_OnReleaseButton(object sender, GameInput.OnReleaseButtonEventArgs e)
    {
        float defaultPressDuration = .4f;
        float totalDuration = e.duration;
        float multipler = 2f;
        float holdDuration = (totalDuration - defaultPressDuration) * multipler;
        float holdDefault = 1f;
        float validHeight = 1f;
        if (IsGrounded())
        {
            isHold = false;
            if (holdDuration < distanceToNextCube - rangeCorrectValue)
            {
                if(holdDuration < holdDefault)
                {
                    fallPosition = transform.position + dir;
                } else
                {
                    fallPosition = transform.position + dir * holdDuration;
                }  
                maxJumpHeight = fallPosition.y - transform.position.y + validHeight;
                Jump(fallPosition, maxJumpHeight);
                
            }
            else if (holdDuration > distanceToNextCube - rangeCorrectValue && holdDuration < distanceToNextCube + rangeCorrectValue)
            {
                fallPosition = nextPointPosition;
                maxJumpHeight = fallPosition.y - transform.position.y + validHeight;
                Jump(fallPosition, maxJumpHeight);                
            }
            
            else if (holdDuration > distanceToNextCube + rangeCorrectValue)
            {
                if(holdDuration < maxHoldValue)
                {
                    fallPosition = transform.position + dir * holdDuration;
                } else
                {
                    fallPosition = transform.position + dir * maxHoldValue;
                }
                maxJumpHeight = fallPosition.y - transform.position.y + validHeight;
                Jump(fallPosition, maxJumpHeight);
            }
            isGrounded = false;
            SoundManager.Instance.PlayJumpSound();
            timeJumpUp = Mathf.Sqrt(-2 * maxJumpHeight / gravity);
        }
    }

    private void Update()
    {
        FallCheck();
        Death();
    }

    private void PercentPower(out float minPercent, out float maxPercent)
    {
        float maxIndexPercent = 1f;
        minPercent = (distanceToNextCube - rangeCorrectValue) / maxHoldValue;
        maxPercent = ((distanceToNextCube + rangeCorrectValue) / maxHoldValue > maxIndexPercent) ? maxIndexPercent : ((distanceToNextCube + rangeCorrectValue) / maxHoldValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isFall = false;
        if (firstSpawn)
        {
            if (collision.transform.GetChild(0).transform.position != nextPointPosition) return;
            IncreaseScore?.Invoke(this, EventArgs.Empty);
            SpawnScorePopUpUI.SpawnPopUp(transform.position);
            SoundManager.Instance.PlayLandingSound();
        }
        if (!firstSpawn)
        {
            firstSpawn = true;
        }
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        isGrounded = true;
        transform.position = collision.transform.GetChild(0).transform.position;
        //random spawn cube left or right with 50% chance
        float randomChance = .5f;
        Vector3 randomDir;
        float distance = UnityEngine.Random.Range(GameAssets.i.distanceSpawnMin, GameAssets.i.distanceSpawnMax);
        if (UnityEngine.Random.Range(GameAssets.i.percentSpawnMin, GameAssets.i.percentSpawnMax) > (GameAssets.i.percentSpawnMax - randomChance))
        {
            randomDir = new Vector3(Mathf.Cos(Mathf.PI / 6), Mathf.Sin(Mathf.PI / 6));
        }
        else
        {
            randomDir = new Vector3(Mathf.Cos(Mathf.PI * 5 / 6), Mathf.Sin(Mathf.PI * 5 / 6));
        }
        Vector3 randomPosition = collision.transform.position + randomDir * distance;
        Transform transformNextCube = SpawnerCube.SpawnCube(randomPosition);
        currentPosition = collision.transform.GetChild(0).transform.position;
        nextPointPosition = transformNextCube.position;
        if (ScoreManager.Instance.IsLastJump())
        {
            Instantiate(GameAssets.i.pfReward, nextPointPosition, Quaternion.identity);
        }
        distanceToNextCube = Vector3.Distance(currentPosition, nextPointPosition);
        dir = (nextPointPosition - currentPosition).normalized;
        Flip(dir.x);
        float minPercent;
        float maxPercent;
        PercentPower(out minPercent, out maxPercent);
        OnTriggerCol?.Invoke(this, new OnTriggerColEventArgs
        {
            minPercent = minPercent,
            maxPercent = maxPercent,
        });
    }

    private void Flip(float dirX)
    {
        if(dirX > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else {
            transform.localScale = new Vector3(1, 1, 1);

        }
    }

    private void Death()
    {
        float distanceFromPlace = transform.position.y - currentPosition.y;
        float placeCheck = 0f;
        if (distanceFromPlace < placeCheck)
        {
            float delay = 1f;
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            if(!isDead)
            {
                StartCoroutine(DelayWhenDead(delay));         

            }
        }
    }

    IEnumerator DelayWhenDead(float delay)
    {
        MusicManager.Instance.MuteVolume();
        SoundManager.Instance.PlayDeadSound();
        isDead = !isDead;
        yield return new WaitForSeconds(delay);
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsHold()
    {
        return isHold;
    }

    private Vector3 CalculateLaunchVelocity(Vector3 fallPosition, float maxJumpHeight)
    {
        // calculate jump
        float displacementY = fallPosition.y - transform.position.y;
        float displacementX = fallPosition.x - transform.position.x;
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * maxJumpHeight);
        Vector3 velocityX = new Vector3(displacementX, 0, 0) / (Mathf.Sqrt(-2 * maxJumpHeight / gravity) + Mathf.Sqrt(2 * (displacementY - maxJumpHeight) / gravity));
        return velocityX + velocityY;
    }

    private void Jump(Vector3 fallPosition, float maxJumpHeight)
    {
        rb.gravityScale = 1;
        rb.velocity = CalculateLaunchVelocity(fallPosition, maxJumpHeight);
    }
    private void FallCheck()
    {
        if(!IsGrounded())
        {
            timeJumpUp -= Time.deltaTime;
            if (timeJumpUp < 0)
            {
                isFall = true;
            }
        }    
    }

    public bool IsFall()
    {
        return isFall;
    }
}