using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamDogController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _stoppedTime;
    private Animator _anim;
    private Rigidbody2D _rb;
    private bool _hasCrashed;
    private Vector2[] positions;
    bool isStopped;
    bool _isLeaping;
    int posIndex;
    // Start is called before the first frame update
    void Start()
    {
        // Set wander positions
        positions = new Vector2[2];
        positions[0] = transform.GetChild(0).position;
        positions[1] = transform.GetChild(1).position;
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _hasCrashed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isLeaping && _rb.velocity.x < 0.05f)
        {
            _isLeaping = false;
        }
        if (isStopped || _isLeaping) return;
        if (Vector2.Distance(transform.position, positions[posIndex]) < 0.3f || _hasCrashed)
        {
            _hasCrashed = false;
            isStopped = true;
            _anim.CrossFade("SteamDog", 0, 0);
            posIndex = posIndex == 1 ? 0 : 1; // since there are only 2 posible positions
            Vector3 rot = new Vector3(transform.rotation.x, posIndex == 1 ? 180f : 0f, transform.rotation.z); // if going to posIndex 0 that means its going left
            transform.rotation = Quaternion.Euler(rot);
            Invoke("EnableMovement", _stoppedTime);
        }
        transform.position = Vector2.MoveTowards(transform.position, positions[posIndex], _movementSpeed * Time.deltaTime);
    }

    void EnableMovement()
    {
        isStopped = false;
        _anim.CrossFade("Walk", 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isLeaping)
        {
            _isLeaping = true;
            _anim.CrossFade("Leap", 0, 0);
            _rb.AddForce(new Vector2(10 * (posIndex == 0 ? -1 : 1), 0f), ForceMode2D.Impulse);
            Debug.Log("Leap");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            _hasCrashed = true;
            _rb.velocity = Vector2.zero;
            GameManager.Instance.DecreaseLives();
            Vector2 direction = new Vector2(transform.position.x < other.transform.position.x ? 1 : -1, 0);
            other.transform.GetComponent<PlayerController>().ApplyPushBack(direction, true);
            // Hurt player
        }
        else
        {
            _hasCrashed = true;
        }
    }
}
