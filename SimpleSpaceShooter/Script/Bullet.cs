using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    GameObject _hitEffectPrefab;
    
    float _speed;
    Rigidbody2D _rg;


    public void Init(float speed)
    {
        _speed = speed;
        _rg = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 7f);
    }

    private void FixedUpdate()
    {
        if (!GameManager.I.CanPlay()) return;
        _rg.velocity = Vector2.up * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
        {
            GameManager.I.SpawnHitEffect(transform.position, _hitEffectPrefab);
            Destroy(gameObject);        
        }
    }


}
