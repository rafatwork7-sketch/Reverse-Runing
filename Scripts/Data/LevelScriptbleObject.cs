using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Current Level Data", menuName = "current Level")]
public class LevelScriptbleObject : ScriptableObject
{
    public CurrentLevelData currentLevelData;
    private bool canRespawnLevel = false;
    public bool CanRespawn
    {
        get => canRespawnLevel;
        set => canRespawnLevel = value;
    }

    private void OnEnable()
    {
        canRespawnLevel = false;
    }
    public  void ResetData()
    {
        currentLevelData.grounds.Clear();
        currentLevelData.groundsPosition.Clear();
        currentLevelData.walls.Clear();
        currentLevelData.wallsPositions.Clear();
    }

    public  void CreateLevel()
    {
        for (int i = 0; i < currentLevelData.grounds.Count; i++)
        {
            Instantiate(currentLevelData.grounds[i], currentLevelData.groundsPosition[i], Quaternion.identity);
        }

        for (int i = 0; i < currentLevelData.walls.Count; i++)
        {
            Instantiate(currentLevelData.walls[i], currentLevelData.wallsPositions[i], Quaternion.identity);
        }
    }
}


[System.Serializable]
public class CurrentLevelData
{
    
    public List<GameObject> grounds = new List<GameObject>();
    public List<Vector3> groundsPosition = new List<Vector3>();
    public List<GameObject> walls = new List<GameObject>();
    public List<Vector3> wallsPositions = new List<Vector3>();
}
