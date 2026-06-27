using UnityEngine;
using DG.Tweening;

public class Enemy : Characters, CharacterState
{
    [Header("Detection")]
    [SerializeField] private float wallDistance = 3.5f;
    [SerializeField] private float playerDistance = 3.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask wallLayer;

    public string enemyName;
    public bool canMove = true;
    public bool isFirstEnemy { get; set; }

    private RaycastHit hitPlayer;
    private RaycastHit hitWall;

    public override void Start()
    {
        base.Start();

        SetEnemyMovementSpeed();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public override void FixedUpdate()
    {
        if (!LevelManager.Instance.GameIsFinished())
            return;

        base.FixedUpdate();

        CheckPlayer();
        CheckWall();
    }

    private void SetEnemyMovementSpeed()
    {
        speedRun = GameManager.Instance.EnemySpeed;
    }

    private void CheckPlayer()
    {
        Vector3 rayStart = transform.position + transform.up;

        if (Physics.Raycast(rayStart, transform.forward, out hitPlayer, playerDistance, playerLayer) ||
            Physics.Raycast(rayStart, transform.right, out hitPlayer, playerDistance * 2f, playerLayer) ||
            Physics.Raycast(rayStart, -transform.right, out hitPlayer, playerDistance * 2f, playerLayer))
        {
            MoveToPlayer(hitPlayer);
        }
    }

    private void MoveToPlayer(RaycastHit hit)
    {
        if (!hit.collider.CompareTag(GameStrings.Player))
            return;

        float targetX = hit.transform.position.x;

        rigidbody.DOMoveX(targetX, walkSpeed).SetEase(Ease.Linear);
        AttackPlayer();
    }

    private void AttackPlayer()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            animator.SetTrigger(GameStrings.IsAttack);
        });

        sequence.AppendInterval(0.32f);

        sequence.AppendCallback(() =>
        {
            LevelManager.Instance.Lose();
        });
    }

    private void CheckWall()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out hitWall, wallDistance, wallLayer))
            return;

        if (hitWall.collider.CompareTag(GameStrings.Wall) && canMove)
        {
            DodgeWall();
        }
        else if (hitWall.collider.CompareTag(GameStrings.WallJump))
        {
            Jump(false);
        }
    }

    private void DodgeWall()
    {
        canMove = false;

        if (laneState == LaneState.Right || laneState == LaneState.Left)
        {
            laneState = LaneState.Mid;

            rigidbody.DOMoveX(0f, walkSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => canMove = true);
        }
        else
        {
            int random = Random.Range(0, 2);
            laneState = random == 1 ? LaneState.Right : LaneState.Left;

            float targetX = laneState == LaneState.Right ? oneStep : -oneStep;

            rigidbody.DOMoveX(targetX, walkSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => canMove = true);
        }
    }

    private void MoveToLane()
    {
        float targetX = 0f;

        if (laneState == LaneState.Right)
            targetX = oneStep;
        else if (laneState == LaneState.Left)
            targetX = -oneStep;

        rigidbody.DOMoveX(targetX, walkSpeed)
            .SetEase(Ease.Linear)
            .OnComplete(() => canMove = true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameStrings.Bullet))
        {
            KillEnemy();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag(GameStrings.Win))
        {
            LevelManager.Instance.Lose();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GameStrings.Wall))
        {
            MoveToLane();
        }
    }

    private void KillEnemy()
    {
        speedRun = 0f;
        rigidbody.isKinematic = true;

        PlayDeadSound();

        Destroy(gameObject, 1.2f);
        EnemyManager.Instance.Enemies.Remove(this);

        if (isFirstEnemy && EnemyManager.Instance.Enemies.Count > 0)
        {
            EnemyManager.Instance.Enemies[0].isFirstEnemy = true;

            IncreaseFrontEnemySpeed();
        }

        capsuleCollider.enabled = false;
        animator.SetTrigger(GameStrings.IsDead);
    }

    private void IncreaseFrontEnemySpeed()
    {
        if (LevelManager.Instance.LevelCount >= 20)
            return;

        float addSpeed = 15f;

        if (EnemyManager.Instance.Enemies.Count >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                EnemyManager.Instance.Enemies[i].speedRun += addSpeed;
                addSpeed -= 5f;
            }
        }
        else
        {
            EnemyManager.Instance.Enemies[0].speedRun += addSpeed;
        }
    }

    private void PlayDeadSound()
    {
        if (enemyName == GameStrings.Skeleton)
        {
            AudioManager.PlaySound(audio, AudioManager.Instance.skeletonDead);
        }
        else
        {
            AudioManager.PlaySound(audio, AudioManager.Instance.zombieDead);
        }
    }

    public void Win()
    {
        EnemyManager.Instance.WinEnemy();
    }

    public void Lose()
    {
        animator.SetTrigger(GameStrings.IsWin);
    }
}