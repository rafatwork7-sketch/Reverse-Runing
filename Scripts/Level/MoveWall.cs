using UnityEngine;
using DG.Tweening;

public class MoveWall : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float stayDuration = 1f;

    private void Start()
    {
        Sequence sequence = DOTween.Sequence();

        float oneStep = LevelManager.Instance.oneStep;

        // Randomly choose movement direction
        bool startRight = Random.Range(0, 2) == 0;

        float firstTarget = startRight ? oneStep : -oneStep;
        float secondTarget = -firstTarget;

        sequence.Append(transform.DOLocalMoveX(firstTarget, moveDuration));
        sequence.AppendInterval(stayDuration);

        sequence.Append(transform.DOLocalMoveX(0f, moveDuration));
        sequence.AppendInterval(stayDuration);

        sequence.Append(transform.DOLocalMoveX(secondTarget, moveDuration));
        sequence.AppendInterval(stayDuration);

        sequence.Append(transform.DOLocalMoveX(0f, moveDuration));
        sequence.AppendInterval(stayDuration);

        sequence.SetEase(Ease.Linear);
        sequence.SetLoops(-1, LoopType.Restart);
    }
}