﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject bulletPrefab;
    public GameObject projectilePrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(50, 26959);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(-32f, 1f, -3f), Quaternion.identity).GetComponent<Player>();
    }

    public void InstantiateEnemy(Vector3 _position)
    {
        Instantiate(enemyPrefab, _position, Quaternion.identity);
    }

    public Bullet InstantiateBullet(Transform _shootOrigin, Quaternion _bulletRotation)
    {
        return Instantiate(bulletPrefab, _shootOrigin.position + _shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Bullet>();
    }

    public Projectile InstantiateProjectile(Transform _shootOrigin)
    {
        return Instantiate(projectilePrefab, _shootOrigin.position + _shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Projectile>();
    }
}
