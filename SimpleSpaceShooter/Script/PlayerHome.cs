using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHome : MonoBehaviour
{
    [SerializeField]
    Transform _home;
    [SerializeField]
    BoxCollider2D _homeCollider;
    [SerializeField]
    Transform _homeHealthBarBody;
    [SerializeField]
    Transform _homeHealthBar;
    [SerializeField]
    int _homeHealth = 10;


    void Start()
    {
        Vector4 screenSides = GameManager.I.GetScreenSidesPos();

        // set home Position to right place
        _home.position = new Vector3(screenSides.w, screenSides.z,0);

        // set Home Base to right Scale and Position Place
        _homeCollider.size = new Vector2(screenSides.y*2, _home.localScale.y);
        _homeCollider.offset = new Vector3(screenSides.y, (_home.localScale.y / 2));

        // set HomeHealth Position Place and Scale
        _homeHealthBarBody.localScale = new Vector3(screenSides.y * 2, _home.localScale.y / (10 * _home.localScale.y), 1);
        _homeHealthBarBody.localPosition = new Vector3(screenSides.y, 1, 0);

        // set _homehealthBar Scale to normal
        _homeHealthBar.localScale = Vector3.one;

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
        _homeHealth -= 1;

        _homeHealthBar.localScale = new Vector3(_homeHealthBar.localScale.x - .1f, _homeHealthBar.localScale.y);

        if (_homeHealth <= 0)
        {
            YouDie();
        }
    }

    void YouDie()
    {
        GameManager.I.PlayerLose();
    }
}
