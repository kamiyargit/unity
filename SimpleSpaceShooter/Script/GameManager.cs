using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Enemies Settings:")]
    [SerializeField]
    Transform _enemiesHolder;
    const float _enemiesHolderTopOffsetPos = .4f;
    [SerializeField]
    float _enemiesHolderMoveDownDelay = 1;
    float _enemiesHolderMoveDownDelayHolder = 0;
    [SerializeField]
    float _enemiesHolderMoveDownAmount = .3f;
    [SerializeField]
    GameObject _enemyPrefab;
    [SerializeField]
    [Range(0, 10)] int row = 2;
    [SerializeField]
    [Range(0, 7)] int col = 2;

    Vector2 _enemyScale = new Vector2(.4f, .4f);
    Vector2 _enemyScaleOffset = new Vector2(.15f, .15f);

    [SerializeField]
    List<EnemyMovement> enemyMovements;
    [SerializeField]
    float _changingEnemiesDirectionTime = 1;

    Vector4 TopRightDownleftSideOfScreen;

    bool canPlay = true;



    private void Awake()
    {
        I = this;
        Init();
    }
    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        EnemiesHolderMovingSystem();
    }

    void Init()
    {
        StopAllCoroutines();
        CalcualteScreenSidesPos();
        SetEnemiesHolderPos();
    }

    /// <summary>
    /// x = top
    /// y = right
    /// z = down
    /// w = left
    /// </summary>
    void CalcualteScreenSidesPos()
    {
        float left = Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height;
        float down = Camera.main.transform.position.y - Camera.main.orthographicSize;

        TopRightDownleftSideOfScreen.x = -down; //TOP
        TopRightDownleftSideOfScreen.y = -left; // RIGHT
        TopRightDownleftSideOfScreen.z = down; // DOWN
        TopRightDownleftSideOfScreen.w = left; // LEFT

        print(TopRightDownleftSideOfScreen);
    }

    public Vector4 GetScreenSidesPos()
    {
        return TopRightDownleftSideOfScreen;
    }

    void SetEnemiesHolderPos()
    {
        _enemiesHolder.position = new Vector3(0, TopRightDownleftSideOfScreen.x - _enemiesHolderTopOffsetPos, 0);
        _enemiesHolderMoveDownDelayHolder = _enemiesHolderMoveDownDelay;
    }

    void EnemiesHolderMovingSystem()
    {
        _enemiesHolderMoveDownDelayHolder -= Time.deltaTime;
        if(_enemiesHolderMoveDownDelayHolder <= 0)
        {
            _enemiesHolder.position = new Vector3(_enemiesHolder.position.x, _enemiesHolder.position.y - _enemiesHolderMoveDownAmount);
            _enemiesHolderMoveDownDelayHolder = _enemiesHolderMoveDownDelay;
        }
    }

    void SpawnEnemies()
    {
        float holdingWidth = 0;
        for (int c = 0; c < col; c++)
        {
            for (int r = 0; r < row; r++)
            {
                GameObject tEnemy = Instantiate(_enemyPrefab, _enemiesHolder.position, Quaternion.identity, _enemiesHolder);
                float x = (_enemyScale.x + _enemyScaleOffset.x) * c;
                holdingWidth = x;
                tEnemy.transform.localPosition = new Vector3(x, - (_enemyScale.y + _enemyScaleOffset.y) * r );
                tEnemy.name += (c + " " + r);

            }
        }

        // set enemy holder to center
        _enemiesHolder.transform.position = new Vector3(-holdingWidth /2, 4.6f, 0);
    }

    public void SpawnHitEffect(Vector2 pos, GameObject go)
    {
        GameObject tGo = Instantiate(go, pos, Quaternion.identity);
        Destroy(tGo, 1.5f);
    }

    public List<EnemyMovement> GetEnemyMovements()
    {
        return enemyMovements;
    }

    public float GetChangingEnemiesDirectionTime()
    {
        return _changingEnemiesDirectionTime;
    }

    public void PlayerKilledOneEnemy()
    {
        if(_enemiesHolder.childCount - 1 <= 0)
        {
            PlayerWin();
        }
    }

    public bool CanPlay()
    {
        return canPlay;
    }

    public void PlayerLose()
    {
        Time.timeScale = 0;
        StartCoroutine(GameOver(false));
    }

    IEnumerator GameOver(bool isWin)
    {
        yield return new WaitForSeconds(3);
        if (isWin)
        {
            print("I can't believe it! You WIN Bro!");
        }
        else
        {
            print("Player Die!!!!!");
        }
        Time.timeScale = 0;
    }

    void PlayerWin()
    {
        StartCoroutine(GameOver(true));
    }


}

[Serializable]
public class EnemyMovement{
    public EnemyMovementDirections direction;
    public float amount = .1f;
}


public enum EnemyMovementDirections
{
    up,
    down,
    right,
    left
}
