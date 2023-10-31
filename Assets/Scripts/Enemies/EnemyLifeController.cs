using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeController : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _healthToFlash;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] float _colorChangeRate;
    private bool _isFlashing;

    const int SWORD_DAMAGE = 2;
    const int BULLET_DAMAGE = 1;

    private void Update()
    {
        if (gameObject.CompareTag("Boss") && _health < _healthToFlash && !_isFlashing)
        {
            _isFlashing = true;
            InvokeRepeating("AlterColor", 0, _colorChangeRate);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSword"))
        {
            _health -= SWORD_DAMAGE;
        }
        else if (other.CompareTag("PlayerBullet"))
        {
            _health -= BULLET_DAMAGE;
        }
    }

    private void AlterColor()
    {
        _sr.color = _sr.color == Color.white ? Color.red : Color.white;
    }
}
