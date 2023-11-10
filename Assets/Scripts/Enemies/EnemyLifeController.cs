using UnityEngine;

public class EnemyLifeController : MonoBehaviour
{

    const int SWORD_DAMAGE = 5;
    const int BULLET_DAMAGE = 2;
    public int max_health;
    public int _health;


    private void Start()
    {
        _health = max_health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("PlayerSword"))
        {
            _health -= SWORD_DAMAGE;
        }
        else if (other.CompareTag("PlayerBullet"))
        {
            _health -= BULLET_DAMAGE;
        }

        if (_health <= 0)
        {
            Destroy(gameObject); // TODO: Replace with death animation and disable
        }
    }
}
