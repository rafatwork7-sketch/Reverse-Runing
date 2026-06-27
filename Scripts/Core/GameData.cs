using UnityEngine;

public class GameData : Singleton<GameData>
{
    [Header("Game Database")]
    public MyScriptbleObject data;
    [Header("Current Level Data")]
    public LevelScriptbleObject currentLevelData;

    protected override void Awake()
    {
       base.Awake();
        // Clear previously saved level data on startup
        currentLevelData.ResetData();
    }
}
