using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    int _health = 2;

    List<EnemyMovement> _enemyMovements;
    int _enemyCurrentMovementIndex = 0;
    float _changingDirectionTime;
    float _changingDirectionTimeHolder;

    [SerializeField]
    Transform _barrel;
    [SerializeField]
    GameObject _bulletPrefab;
    [SerializeField]
    Vector2 _shootingDelayRange = new Vector2(3,50);
    float _shootingDelayHolder;

    [SerializeField]
    GameObject _dieEffectPrefab;



    private void Awake()
    {
        Init();
    }

    void Init()
    {
        _enemyMovements = GameManager.I.GetEnemyMovements();
        _changingDirectionTime = GameManager.I.GetChangingEnemiesDirectionTime();
        _changingDirectionTimeHolder = _changingDirectionTime;

        _shootingDelayHolder = Random.Range(_shootingDelayRange.x, _shootingDelayRange.y);
    }

    private void Update()
    {
        if (!GameManager.I.CanPlay()) return;
        Movement();
        Shooting();
    }

    void Movement()
    {
        _changingDirectionTimeHolder -= Time.deltaTime;
        if(_changingDirectionTimeHolder <= 0)
        {
            EnemyMovement tEM = _enemyMovements[_enemyCurrentMovementIndex];

            Vector3 direction;
            switch (tEM.direction)
            {
                case EnemyMovementDirections.up:
                    direction = new Vector3(0, tEM.amount);
                    break;
                case EnemyMovementDirections.down:
                    direction = new Vector3(0, -tEM.amount);
                    break;
                case EnemyMovementDirections.right:
                    direction = new Vector3(tEM.amount, 0);
                    break;
                case EnemyMovementDirections.left:
                    direction = new Vector3(-tEM.amount, 0);
                    break;
                default:
                    direction = new Vector3(0, 0, 0);
                    break;

            }

            transform.localPosition += direction;

            _changingDirectionTimeHolder = _changingDirectionTime;

            if (_enemyCurrentMovementIndex < _enemyMovements.Count - 1)
                _enemyCurrentMovementIndex += 1;
            else
                _enemyCurrentMovementIndex = 0;
        }
    }

    void Shooting()
    {
        _shootingDelayHolder -= Time.deltaTime;
        if (_shootingDelayHolder <= 0)
        {
            Instantiate(_bulletPrefab, _barrel.position, Quaternion.identity);
            _shootingDelayHolder = Random.Range(_shootingDelayRange.x, _shootingDelayRange.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            HitByPlayerBullet();
        }

        if(collision.CompareTag("Player") || collision.CompareTag("PlayerHome"))
        {
            Destroy(gameObject);
        }
    }

    void HitByPlayerBullet()
    {
        _health -= 1;

        if(_health <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        GameManager.I.PlayerKilledOneEnemy();
        GameManager.I.SpawnHitEffect(transform.position, _dieEffectPrefab);
        Destroy(gameObject);
    }
}

