
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Levels")]
public class MyScriptbleObject : ScriptableObject
{
    [Header("Level Data")]
    public List<LevelData> levelData = new List<LevelData>();
    [Header("Player Data")]
    public PlayerData playerData ;
    [Header("Enemy Data")]
    public EnemyData enemyData ;
}


[System.Serializable]
public class LevelData
{
    public int number;
    public List<GameObject> grounds = new List<GameObject>();
    public List<GameObject> walls = new List<GameObject>();
}
[System.Serializable]
public class PlayerData
{
    public GameObject maleDummy;
    public GameObject femaleDummy;
    public GameObject cowboy;
    public GameObject ninja;
}
[System.Serializable]
public class EnemyData
{
    public Enemy skeleton;
    public Enemy zombie;
}
