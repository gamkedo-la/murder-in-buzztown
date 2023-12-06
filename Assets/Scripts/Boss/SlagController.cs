using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlagController : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody2D _rb;
    [SerializeField] AudioClip _slagClip;
    [SerializeField] CircleCollider2D _radiationCollider;



    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.DecreaseLives();
            Vector2 direction = new Vector2(transform.position.x < other.transform.position.x ? 1 : -1, 0);
            other.transform.GetComponent<PlayerController>().ApplyPushBack(direction, true);
            DestroySlag();
        }
        if (other.CompareTag("Ground"))
        {
            _rb.bodyType = RigidbodyType2D.Static;
            _anim.CrossFade("SlagGrounding", 0, 0);
            _radiationCollider.enabled = true;
            AudioManager.Instance.PlayEffect(_slagClip);
            Invoke("StopRadiating", 2f);
        }
    }

    private void StopRadiating()
    {
        _anim.CrossFade("SlagRadiateEnd", 0, 0);
    }

    public void DestroySlag()
    {
        Destroy(gameObject);
    }
}
