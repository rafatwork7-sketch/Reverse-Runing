using UnityEngine;

public class EnemyDataButtonUIScript : ManagerButtonUIScript
{
    public enum EnemyUiData
    {
        Skeleton,
        Zombie
    }

    [SerializeField] private EnemyUiData enemyUiData;

    private string enemyName;

    public override void Start()
    {
        base.Start();

        // Register button event
        selectButton.onClick.AddListener(SelectButtonActived);
    }

    public override void SelectButtonActived()
    {
        base.SelectButtonActived();

        // Select enemy data
        if (enemyUiData == EnemyUiData.Skeleton)
        {
            enemyName = GameStrings.Skeleton;
        }
        else
        {
            enemyName = GameStrings.Zombie;
        }

        // Save only if enemy selection changed
        if (enemyName != PlayerPrefs.GetString(GameStrings.EnemyData))
        {
            PlayerPrefs.SetString(GameStrings.EnemyData, enemyName);
            GameUi.Instance.storeUiScript.IsSelected = true;
        }
    }
}