using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", 2f);
    }

    public void SetDirection(bool isRight)
    {
        direction = isRight ? 1 : -1;
        GetComponent<SpriteRenderer>().flipX = !isRight;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * direction * _bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.DecreaseLives();
            Vector2 direction = new Vector2(transform.position.x < other.transform.position.x ? 1 : -1, 0);
            other.GetComponent<PlayerController>().ApplyPushBack(direction, true);
        }
        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
