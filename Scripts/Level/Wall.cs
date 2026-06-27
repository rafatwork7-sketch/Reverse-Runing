using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject wallExplosionPrefab;

    public void CreateWallExplosion()
    {
        Vector3 cretePos = transform.position;
        cretePos.z += .2f;
        GameObject wall = Instantiate(wallExplosionPrefab, cretePos, Quaternion.identity) as GameObject;
    }
  
}
