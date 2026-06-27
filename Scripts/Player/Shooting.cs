using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shooting : MonoBehaviour
{
    public static Shooting Instance;

    [Header("References")]
    [SerializeField] private AudioSource audio;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;

    [Header("Pool Settings")]
    [SerializeField] private int spawnBulletCount = 10;
    [SerializeField] private bool canGrowPool = true;

    [Header("Shooting Settings")]
    [SerializeField] private float shootingTime = 0.2f;
    [SerializeField] private float bulletLifeTime = 0.7f;
    [SerializeField] private float muzzleLifeTime = 0.2f;

    private readonly List<GameObject> bullets = new List<GameObject>();
    private readonly List<GameObject> muzzleFlashes = new List<GameObject>();

    private GameObject bulletParent;

    private int currentAmmo;
    private float timeForShooting;

    public int CurrentAmmo
    {
        get => currentAmmo;
        set => currentAmmo = value;
    }

    public bool IsShoot { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bulletParent = new GameObject("Bullets And Muzzle");

        currentAmmo = LevelManager.Instance.MaxAmmo;
        GameUi.Instance.GetAmmoText(currentAmmo);

        CreateObjects(bulletPrefab, bullets);
        CreateObjects(muzzleFlashPrefab, muzzleFlashes);
    }

    private void Update()
    {
        timeForShooting += Time.deltaTime;
    }

    public void Shoot()
    {
        if (!PlayerCanShoot())
            return;

        AudioManager.PlaySound(audio, AudioManager.Instance.pistol);

        currentAmmo--;
        GameUi.Instance.GetAmmoText(currentAmmo);

        GameObject bullet = GetPooledObject(bulletPrefab, bullets);
        if (bullet != null)
        {
            bullet.transform.position = bulletPoint.position;
            bullet.SetActive(true);
            StartCoroutine(DisableBulletAfterDelay(bullet));
        }

        GameObject muzzle = GetPooledObject(muzzleFlashPrefab, muzzleFlashes);
        if (muzzle != null)
        {
            muzzle.transform.position = bulletPoint.position;
            muzzle.SetActive(true);
            DisableMuzzleAfterDelay(muzzle);
        }

        timeForShooting = 0f;
    }

    private IEnumerator DisableBulletAfterDelay(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifeTime);
        bullet.SetActive(false);
    }

    public void PlayShooting()
    {
        IsShoot = true;
    }

    public void StopShooting()
    {
        IsShoot = false;
    }

    private bool PlayerCanShoot()
    {
        return LevelManager.Instance.GameIsFinished()
               && timeForShooting >= shootingTime
               && currentAmmo > 0
               && !PlayerController.Instance.playerIsHit;
    }

    private void CreateObjects(GameObject prefabObject, List<GameObject> objectList)
    {
        for (int i = 0; i < spawnBulletCount; i++)
        {
            GameObject obj = Instantiate(
                prefabObject,
                bulletPoint.position,
                Quaternion.identity,
                bulletParent.transform);

            obj.SetActive(false);
            objectList.Add(obj);
        }
    }

    private void DisableMuzzleAfterDelay(GameObject muzzle)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(muzzleLifeTime);
        sequence.AppendCallback(() => muzzle.SetActive(false));
    }

    private GameObject GetPooledObject(GameObject prefabObject, List<GameObject> objectList)
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            if (!objectList[i].activeInHierarchy)
            {
                return objectList[i];
            }
        }

        if (!canGrowPool)
            return null;

        GameObject obj = Instantiate(
            prefabObject,
            bulletPoint.position,
            Quaternion.identity,
            bulletParent.transform);

        objectList.Add(obj);

        return obj;
    }
}