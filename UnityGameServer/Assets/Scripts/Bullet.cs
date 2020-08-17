using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Dictionary<int, Bullet> Bullets = new Dictionary<int, Bullet>();
    private static int nextBulletId = 1;

    public int id;
    public int thrownByPlayer;
    public float damage = 10f;
    public float speed = 10f;
    
    private Vector3 oldPos;
    private Vector3 newPos;


    private void Start()
    {
        id = nextBulletId;
        nextBulletId++;
        Bullets.Add(id, this);

        oldPos = transform.position;
        newPos = transform.position;

        ServerSend.SpawnBullet(this, thrownByPlayer);

        StartCoroutine(DestroyAfterTime());
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        ServerSend.BulletPosition(this);
        CheckCollision();
    }

    private void CheckCollision()
    {
        newPos = transform.position;
        RaycastHit _hit;

        if (Physics.Linecast(oldPos, newPos, out _hit))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                _hit.collider.GetComponent<Player>().TakeDamage(damage);
                ServerSend.BulletDestroyed(this);
                Destroy(gameObject);
            }
            else if (_hit.collider.CompareTag("Enemy"))
            {
                _hit.collider.GetComponent<Enemy>().TakeDamage(damage);
                ServerSend.BulletDestroyed(this);
                Destroy(gameObject);
            }
            else
            {
                Vector3 _reflectedDirection = Vector3.Reflect(newPos - oldPos, _hit.normal);
                Debug.Log($"IN: {newPos - oldPos},  NORM: {_hit.normal},  OUT: {_reflectedDirection}");
                transform.LookAt(transform.position + _reflectedDirection);
            }
        }

        oldPos = newPos;
    }

    public void Initialize(Quaternion _initialMovementDirection, int _thrownByPlayer)
    {
        transform.rotation = _initialMovementDirection;
        thrownByPlayer = _thrownByPlayer;
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(3f);

        ServerSend.BulletDestroyed(this);
        Destroy(gameObject);
    }
}
