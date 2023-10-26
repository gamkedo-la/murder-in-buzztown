using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    int direction = 1;

    private GameObject lifeManager;
    private LifeManager lifeManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        lifeManager = GameObject.FindGameObjectWithTag("Lives");
        lifeManagerScript = lifeManager.GetComponent<LifeManager>();
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
            lifeManagerScript.DecreaseLives();
        }
        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
