using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletEnemy : MonoBehaviour
{
    Rigidbody2D _rg;

    [SerializeField]
    float _speed = 2;
    [SerializeField]
    int _health = 2;
    [SerializeField]
    GameObject _hitEffectPrefab;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        _rg = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 20f);
    }

    private void FixedUpdate()
    {
        if (!GameManager.I.CanPlay()) return;
        _rg.velocity = Vector2.down * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerHome"))
        {
            Destroying();
        } 
        else if (collision.CompareTag("PlayerBullet"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        _health -= 1;

        if (_health <= 0)
        {
            Destroying();        
        }
    }

    void Destroying()
    {
        GameManager.I.SpawnHitEffect(transform.position, _hitEffectPrefab);
        Destroy(gameObject);
    }


}
