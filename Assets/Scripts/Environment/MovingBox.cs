using UnityEngine;

public class MovingBox : MonoBehaviour
{

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _stoppedTime;

    private Vector2[] positions;
    bool isStopped;
    int posIndex;
    // Start is called before the first frame update
    void Start()
    {
        // Set wander positions
        positions = new Vector2[2];
        positions[0] = transform.GetChild(0).position;
        positions[1] = transform.GetChild(1).position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopped) return;
        transform.position = Vector2.MoveTowards(transform.position, positions[posIndex], _movementSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, positions[posIndex]) < 0.3f)
        {
            isStopped = true;
            posIndex = posIndex == 1 ? 0 : 1; // since there are only 2 posible positions
            Invoke("EnableMovement", _stoppedTime);
        }
    }

    void EnableMovement()
    {
        isStopped = false;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        other.transform.SetParent(null);
    }
}
