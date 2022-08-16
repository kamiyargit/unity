using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement Setting:")]
    [SerializeField]
    float _speed;
    PlayerInputAction _playerInputAction;
    Rigidbody2D _rg;
    Vector2 _moveInput;

    [Header("Shooting Setting:")]
    [SerializeField]
    float _shootingDelay = .3f;
    float _shootingDelayHolder;
    [SerializeField]
    float _shootingSpeed = 3f;
    [SerializeField]
    Transform _shootingBarrel;
    [SerializeField]
    GameObject _shootingBullet;

    [Header("Shooting Setting:")]
    [SerializeField]
    int _health = 5;
    float _healthMinesForBar = 0;
    [SerializeField]
    Transform _healthBar;

    [SerializeField]
    GameObject _dieEffectPrefab;



    // Start is called before the first frame update
    void Awake()
    {
        _playerInputAction = new PlayerInputAction();
        _rg = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Init();
    }
    void Init()
    {
        CalcualteHealthForHealthBar();
    }

    void CalcualteHealthForHealthBar()
    {
        _healthMinesForBar = _healthBar.localScale.x / _health;
    }

    private void OnEnable()
    {
        _playerInputAction.Player_Map.Enable();
    }

    private void OnDisable()
    {
        _playerInputAction.Player_Map.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.I.CanPlay()) return;
        Movement();
    }

    private void Update()
    {
        if (!GameManager.I.CanPlay()) return;
        Shooting();
    }

    void Movement()
    {
        _moveInput = _playerInputAction.Player_Map.Movement.ReadValue<Vector2>();
        _rg.velocity = _moveInput * _speed;
    }

    void Shooting()
    {
        _shootingDelayHolder -= Time.deltaTime;
        if (_shootingDelayHolder <= 0)
        {
            GameObject tBullet = Instantiate(_shootingBullet, _shootingBarrel.position, Quaternion.identity);
            tBullet.GetComponent<Bullet>().Init(_shootingSpeed);
            _shootingDelayHolder = _shootingDelay;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        _health -= 1;
        _healthBar.localScale = new Vector3(_healthBar.localScale.x - _healthMinesForBar, _healthBar.localScale.y);

        if(_health <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        _playerInputAction.Player_Map.Disable();
        GameManager.I.SpawnHitEffect(transform.position, _dieEffectPrefab);
        GameManager.I.PlayerLose();
    }
}
