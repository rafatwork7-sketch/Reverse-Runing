using UnityEngine;

public class PlayerDataButtonUIScript : ManagerButtonUIScript
{
    public enum PlayerUiData
    {
        Male,
        Female,
        Cowboy,
        Ninja
    }

    [SerializeField] private PlayerUiData playerUiData;

    private string playerName;

    public override void Start()
    {
        base.Start();

        // Register button event
        selectButton.onClick.AddListener(SelectButtonActived);
    }

    public override void SelectButtonActived()
    {
        base.SelectButtonActived();

        // Determine selected player
        switch (playerUiData)
        {
            case PlayerUiData.Male:
                playerName = GameStrings.MaleDummy;
                break;

            case PlayerUiData.Female:
                playerName = GameStrings.FemaleDummy;
                break;

            case PlayerUiData.Cowboy:
                playerName = GameStrings.Cowboy;
                break;

            case PlayerUiData.Ninja:
                playerName = GameStrings.Ninja;
                break;
        }

        // Save only if the selected character has changed
        if (playerName != PlayerPrefs.GetString(GameStrings.PlayerData))
        {
            PlayerPrefs.SetString(GameStrings.PlayerData, playerName);
            GameUi.Instance.storeUiScript.IsSelected = true;
        }
    }
}