using UnityEngine;
using DG.Tweening;

public class HowToPlayUIScripts : MonoBehaviour
{
    [Header("Tutorial Panels")]
    [SerializeField] private GameObject howToPlayMoving;
    [SerializeField] private GameObject howToPlayJumping;

    [Header("Pointers")]
    [SerializeField] private RectTransform pointerHorizontal;
    [SerializeField] private RectTransform pointerJump;

    public int FingerMove { get; set; }
    public int FingerJump { get; set; }

    public void HowToPlayMoveActivated()
    {
        // Show movement tutorial animation
        howToPlayMoving.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(pointerHorizontal.DOAnchorPosX(100f, 0.6f));
        sequence.Append(pointerHorizontal.DOAnchorPosX(0f, 0.6f));
        sequence.AppendInterval(0.1f);
        sequence.Append(pointerHorizontal.DOAnchorPosX(-100f, 0.6f));
        sequence.Append(pointerHorizontal.DOAnchorPosX(0f, 0.6f));

        sequence.SetEase(Ease.Linear);
        sequence.OnComplete(() => howToPlayMoving.SetActive(false));
    }

    public void HowToPlayJumpActivated()
    {
        // Show jump tutorial animation
        howToPlayJumping.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(pointerJump.DOAnchorPosY(0f, 0.6f));
        sequence.Append(pointerJump.DOAnchorPosY(-135f, 0.6f));
        sequence.Append(pointerJump.DOAnchorPosY(0f, 0.6f));
        sequence.Append(pointerJump.DOAnchorPosY(-135f, 0.6f));

        sequence.SetEase(Ease.Linear);
        sequence.OnComplete(() => howToPlayJumping.SetActive(false));
    }
}