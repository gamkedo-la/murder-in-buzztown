using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private float _colorChangeRate;
    private bool _isFlashing;
    const int SWORD_DAMAGE = 5;
    const int BULLET_DAMAGE = 2;
    public const int MAX_HEALTH = 30;
    public int _health;

    private void Start()
    {
        _health = MAX_HEALTH;
    }

    private void Update()
    {
        // Flash when bellow 30% life
        if (gameObject.CompareTag("Boss") && IsBelowPercentage(.3f) && !_isFlashing)
        {
            _isFlashing = true;
            InvokeRepeating("AlterColor", 0, _colorChangeRate);
        }
    }

    public bool IsBelowPercentage(float percentage)
    {
        return _health < MAX_HEALTH * percentage;
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
