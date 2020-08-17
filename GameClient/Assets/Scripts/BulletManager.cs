using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public int id;

    public void Initialize(int _id)
    {
        id = _id;
        Destroy(gameObject, 3f);
    }

    public void Delete()
    {
        GameManager.bullets.Remove(id);
        Destroy(gameObject);
    }
}
