using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Characters, CharacterState
{
    public static PlayerController Instance;

    [Header("Shooting")]
    public Shooting shoot;

    [Header("Input")]
    [HideInInspector] public bool playerIsHit = false;
    public bool isDragPlayer = false;

    [SerializeField] private float distanceTouchOnX = 50f;
    [SerializeField] private float distanceTouchOnY = 80f;

    [Header("Hit Settings")]
    [SerializeField] private float changeRunSpeedTime = 0.3f;

    private Vector3 dragOrigin;
    private Vector3 firstTouch;

    private bool animationIsActive = true;
    private bool oneTouch = false;
    private bool canTouch = true;
    private bool coroutineAllowed = true;

    private float defaultRunSpeed;
    private float defaultWalkSpeed;
    private float newPointX;

    private int clickCount = 0;
    private float firstClickTime;
    private float timeBetweenClicks = 0.2f;
    private float maxDoubleTapTime = 0.2f;
    private float newTime;

    private Touch touch;

    public int PlayerHitWall { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        PlayerHitWall = 0;
        laneState = LaneState.Mid;

        defaultWalkSpeed = walkSpeed;

        SetPlayerMovementSpeed();

        animationIsActive = true;
    }

    private void SetPlayerMovementSpeed()
    {
        // Load movement speed from game settings
        speedRun = GameManager.Instance.PlayerSpeed;
        defaultRunSpeed = speedRun;
    }

    public override void FixedUpdate()
    {
        if (LevelManager.Instance.GameIsFinished())
        {
            base.FixedUpdate();
        }
    }

    private void Update()
    {
        DoubleClickForShooting();
        MovePlayerOnXPosition();
    }

    private void MovePlayerOnXPosition()
    {
        if (shoot.IsShoot)
            return;

#if UNITY_EDITOR
        DragPlayer();
#else
        TouchInput();
#endif
    }

    private void DoubleClickForShooting()
    {
#if UNITY_EDITOR
        DoubleTapOnClick();
#else
        DoubleTapOnTouch();
#endif
    }

    private void DoubleTapOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;
        }

        if (clickCount == 1 && coroutineAllowed)
        {
            firstClickTime = Time.time;
            StartCoroutine(DoubleClick());
        }
    }

    private IEnumerator DoubleClick()
    {
        coroutineAllowed = false;

        while (Time.time < firstClickTime + timeBetweenClicks)
        {
            if (clickCount == 2)
            {
                shoot.Shoot();
            }

            yield return new WaitForEndOfFrame();
        }

        clickCount = 0;
        firstClickTime = 0f;
        coroutineAllowed = true;
    }

    private void DoubleTapOnTouch()
    {
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                clickCount++;
            }

            if (clickCount == 1)
            {
                newTime = Time.time + maxDoubleTapTime;
            }
            else if (clickCount == 2 && Time.time <= newTime)
            {
                shoot.Shoot();
                clickCount = 0;
            }
        }

        if (Time.time > newTime)
        {
            clickCount = 0;
        }
    }

    private void DragPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            oneTouch = false;
            firstTouch = Input.mousePosition;
            characterPosition = transform.position;
        }

        isDragPlayer = Input.GetMouseButton(0);

        if (isDragPlayer && !oneTouch && !playerIsHit)
        {
            HorizontalMoving(Input.mousePosition);
        }
    }

    private void TouchInput()
    {
        if (Input.touchCount <= 0)
            return;

        touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            oneTouch = false;
            firstTouch = touch.position;
            characterPosition = transform.position;
        }

        if (touch.phase == TouchPhase.Moved && !oneTouch && !playerIsHit)
        {
            HorizontalMoving(touch.position);
        }
    }

    private void HorizontalMoving(Vector3 position)
    {
        if (position.x > firstTouch.x + distanceTouchOnX && IsGrounded() && canTouch && !playerIsHit)
        {
            clickCount = 0;
            canTouch = false;
            ChangeLane(oneStep);
        }
        else if (position.x < firstTouch.x - distanceTouchOnX && IsGrounded() && canTouch && !playerIsHit)
        {
            clickCount = 0;
            canTouch = false;
            ChangeLane(-oneStep);
        }
        else if (position.y > firstTouch.y + distanceTouchOnY && !playerIsHit && LevelManager.Instance.LevelCount >= 20)
        {
            clickCount = 0;
            Jump(true);
        }
    }

    private void ChangeLane(float value)
    {
        AudioManager.PlaySound(audio, AudioManager.Instance.slashPlayer);

        walkSpeed = defaultWalkSpeed;

        newPointX = value + transform.position.x;

        if (newPointX == oneStep)
            laneState = LaneState.Right;
        else if (newPointX == -oneStep)
            laneState = LaneState.Left;
        else if (newPointX == 0f)
            laneState = LaneState.Mid;

        newPointX = Mathf.Clamp(newPointX, -oneStep, oneStep);

        rigidbody.DOMoveX(newPointX, walkSpeed)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                canTouch = true;
                oneTouch = true;
            });
    }

    private void WinPlayerAnimation()
    {
        animator.SetTrigger(GameStrings.IsWin);
    }

    private void HitPlayer()
    {
        Sequence sequence = DOTween.Sequence();

        float currentRunSpeed = speedRun;
        float currentWalkSpeed = walkSpeed;

        sequence.AppendCallback(() =>
        {
            transform.DOMoveZ(transform.position.z - 2f, 0.4f).SetEase(Ease.Linear);

            PlayerHitWall++;
            playerIsHit = true;

            animator.SetTrigger(GameStrings.IsHit);
            PlayHitSound();

            speedRun = 0f;
            walkSpeed = 0f;
        });

        sequence.AppendInterval(changeRunSpeedTime);

        sequence.AppendCallback(() =>
        {
            rigidbody.isKinematic = false;

            speedRun = currentRunSpeed;
            walkSpeed = currentWalkSpeed;

            playerIsHit = false;

            UpdatePlayerLane();
        });
    }

    private void HitPlayerWallJump()
    {
        Sequence sequence = DOTween.Sequence();

        float currentRunSpeed = speedRun;
        float currentWalkSpeed = walkSpeed;

        sequence.AppendCallback(() =>
        {
            animator.SetTrigger(GameStrings.IsHit);
            PlayHitSound();

            speedRun = 0f;
            walkSpeed = 0f;

            transform.DOMoveZ(transform.position.z - 3f, 0.6f).SetEase(Ease.Linear);
        });

        sequence.AppendInterval(changeRunSpeedTime);

        sequence.AppendCallback(() =>
        {
            speedRun = currentRunSpeed;
            walkSpeed = currentWalkSpeed;

            UpdatePlayerLane();
        });
    }

    private void UpdatePlayerLane()
    {
        if (laneState == LaneState.Left)
            rigidbody.DOMoveX(-oneStep, walkSpeed).SetEase(Ease.Linear);
        else if (laneState == LaneState.Right)
            rigidbody.DOMoveX(oneStep, walkSpeed).SetEase(Ease.Linear);
        else
            rigidbody.DOMoveX(0f, walkSpeed).SetEase(Ease.Linear);
    }

    private void PlayHitSound()
    {
        if (gameObject.name == GameStrings.FemaleDummy)
            AudioManager.PlaySound(audio, AudioManager.Instance.hitPlayerGirl);
        else
            AudioManager.PlaySound(audio, AudioManager.Instance.hitPlayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameStrings.Win))
        {
            LevelManager.Instance.Win();
            WinPlayerAnimation();
        }
        else if (other.CompareTag(GameStrings.Slow))
        {
            speedRun = (speedRun / 4f) * 3f;
        }
        else if (other.CompareTag(GameStrings.WallJump))
        {
            HitPlayerWallJump();
        }
        else if (other.CompareTag(GameStrings.Wall))
        {
            other.enabled = false;
            rigidbody.isKinematic = true;

            HitPlayer();

            Wall wall = other.GetComponent<Wall>();

            Destroy(other.gameObject, 0.1f);

            if (wall != null)
            {
                wall.CreateWallExplosion();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameStrings.Slow))
        {
            speedRun = defaultRunSpeed;
        }
    }

    public void Win()
    {
        animator.SetTrigger(GameStrings.IsWin);
    }

    public void Lose()
    {
        if (!animationIsActive)
            return;

        if (gameObject.name == GameStrings.FemaleDummy)
            AudioManager.PlaySound(audio, AudioManager.Instance.losePlayerGirl);
        else
            AudioManager.PlaySound(audio, AudioManager.Instance.losePlayer);

        rigidbody.isKinematic = true;
        animator.SetTrigger(GameStrings.IsDead);

        animationIsActive = false;
    }
}