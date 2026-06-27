using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnvironmentDataButtonUIScript : ManagerButtonUIScript
{
    public enum EnvironmentUiData
    {
       Street,Terrain, Beach
    }
    public EnvironmentUiData environmentUiData;

    private string environmentName;
    private int environmentNumber;
    public override void Start()
    {
        base.Start();
        selectButton.onClick.AddListener(SelectButtonActived);
    }

  
    public override void SelectButtonActived()
    {
        base.SelectButtonActived();

        // Determine which environment was selected
        switch (environmentUiData)
        {
            case EnvironmentUiData.Street:
                environmentNumber = GameData.Instance.data.levelData[0].number;
                environmentName = GameStrings.Street;
                break;

            case EnvironmentUiData.Terrain:
                environmentNumber = GameData.Instance.data.levelData[1].number;
                environmentName = GameStrings.Terrain;
                break;

            case EnvironmentUiData.Beach:
                environmentNumber = GameData.Instance.data.levelData[2].number;
                environmentName = GameStrings.Beach;
                break;
        }

        // Save only if the environment has changed
        if (environmentName != PlayerPrefs.GetString(GameStrings.EnvironmentData))
        {
            PlayerPrefs.SetInt(GameStrings.EnvironmentData, environmentNumber);
            GameUi.Instance.storeUiScript.IsSelected = true;
        }

        // Clear saved level data when changing environment
        GameData.Instance.currentLevelData.ResetData();
        GameData.Instance.currentLevelData.CanRespawn = false;
    }
}
