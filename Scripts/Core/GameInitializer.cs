using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameInitializer : MonoBehaviour
{

    [SerializeField] RectTransform rect;

    [SerializeField] AudioSource audio;
    Vector2 bigIconSizeAndAnim = new Vector2(700f, 450f);
    Vector2 bigIconSizeAndAnimNew = new Vector2(600f, 400f);
   
    void Start()
    {
        LogoAninmation();
        GoToMainScene();
    }

    private void GoToMainScene()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(3.5f);
        seq.AppendCallback(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);     }); 
    }
    public void LogoAninmation()
    {
        AudioManager.PlaySound(audio, AudioManager.Instance.logoFx);
        var seq = DOTween.Sequence();
        seq.Append(rect.DOSizeDelta(bigIconSizeAndAnim, 2.0f));
        seq.Append(rect.DOSizeDelta(bigIconSizeAndAnimNew, 1.0f));

        seq.SetEase(Ease.Linear);
    }
}
