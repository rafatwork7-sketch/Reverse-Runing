using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{


    [Header("Enemy Settings")]
    public Enemy enemy;
    [SerializeField] private float spawnDelay = 0.2f;

    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();

    private Vector3 enemySpawnPosition;

  

    public IEnumerator SpawnEnemies(int enemyCount, Transform parentTransform, float groundHeight)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            int enemyLane = Random.Range(0, 3);

            enemySpawnPosition = GetEnemySpawnPosition(enemyLane, groundHeight);

            yield return new WaitForSeconds(spawnDelay);

            if (Enemies.Count < enemyCount)
            {
                CreateEnemy(enemySpawnPosition, parentTransform, enemyLane);
            }
        }
    }

    private Vector3 GetEnemySpawnPosition(int enemyLane, float groundHeight)
    {
        if (enemyLane == 1)
        {
            return new Vector3(LevelManager.Instance.oneStep, 0.5f, groundHeight);
        }

        if (enemyLane == 2)
        {
            return new Vector3(-LevelManager.Instance.oneStep, 0.5f, groundHeight);
        }

        return new Vector3(0f, 0.5f, groundHeight);
    }

    private void CreateEnemy(Vector3 enemyPosition, Transform parentTransform, int enemyLane)
    {
        Enemy newEnemy = Instantiate(enemy, enemyPosition, Quaternion.identity, parentTransform);

        newEnemy.name = GameStrings.Enemy;

        SetEnemyLane(enemyLane, newEnemy);

        Enemies.Add(newEnemy);

        if (Enemies.Count <= 3)
        {
            newEnemy.isFirstEnemy = true;
        }
    }

    private void SetEnemyLane(int enemyLane, Enemy newEnemy)
    {
        if (enemyLane == 1)
        {
            newEnemy.laneState = Enemy.LaneState.Right;
        }
        else if (enemyLane == 2)
        {
            newEnemy.laneState = Enemy.LaneState.Left;
        }
        else
        {
            newEnemy.laneState = Enemy.LaneState.Mid;
        }
    }

    public void WinEnemy()
    {
        foreach (Enemy enemyItem in Enemies)
        {
            enemyItem.GetComponent<Animator>().SetTrigger(GameStrings.IsWin);
        }
    }

    public void LoseEnemy()
    {
        foreach (Enemy enemyItem in Enemies)
        {
            enemyItem.GetComponent<Animator>().SetTrigger(GameStrings.IsLose);
        }
    }
}