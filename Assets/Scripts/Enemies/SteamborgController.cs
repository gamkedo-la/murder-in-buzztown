using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamborgController : MonoBehaviour
{
    // private float _skin = 0.05f;
    private float _skin = 5f;
    private float _internalFaceDirection = 1;
    private Vector2 _boundsBottomLeft;
    private Vector2 _boundsBottomRight;
    private Vector2 _boundsBottomMiddle;
    private Vector2 _boundsTopLeft;
    private Vector2 _boundsTopRight;
    private Vector2 _boundsTopMiddle;
    private Vector2 _force;
    private Vector2 _movePosition;
    private float _boundsHeight;
    private float _boundsWidth;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] private int horizontalRayAmount = 8;

    [Header("Collisions")]
    [SerializeField] private LayerMask collideWith;

    // Start is called before the first frame update
    void Start() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.left) * 2;
        Debug.DrawRay(transform.position, forward, Color.green);
        RaycastCheckForCollisionHorizontally(0);
    }

    private void RaycastCheckForCollisionHorizontally(int direction) {
        // Determine ray length based on movement in the horizontal + size of the object + skin
        // float rayLength = Mathf.Abs(_force.x * Time.deltaTime) + _boundsWidth / 2f + _skin * 2f;
        float rayLength = 5f;

        Vector2 rayHorizontalBottom = (_boundsBottomLeft + _boundsBottomRight) / 2f;
        Vector2 rayHorizontalTop = (_boundsTopLeft + _boundsTopRight) / 2f;
        rayHorizontalBottom += (Vector2)transform.up * _skin; // Don't collide with above you
        rayHorizontalTop -= (Vector2)transform.up * _skin; // Don't collide with below you


        // For the number of horizontal rays, calculate the distance from the midpoint of the object to the horizontal edges
        for (int i = 0; i < horizontalRayAmount; i++) {
            Vector2 rayOrigin = Vector2.Lerp(rayHorizontalBottom, rayHorizontalTop, (float)i / (float)(horizontalRayAmount - 1));
            // Direction will be 1 or -1, right or left, multiple by the transform.right to get full dist
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction * transform.right, rayLength, collideWith);
            Debug.DrawRay(rayOrigin, transform.right * rayLength * 1, Color.cyan);

            if (hit) {
                if (direction >= 0) // If we collide on the right
                {
                    // Set the move position to be the distance to contact - everything we added to ray length. This makes you kiss the wall (at the hit.distance) and then takes in the extra fluff from ray.
                    _movePosition.x = hit.distance - _boundsWidth / 2f - _skin * 2f;
                } else // If we collide on the left
                  {
                    // Set the move position to be the negative distance (cuz left) to contact + everything we added to ray length. This makes you kiss the wall (at the hit.distance) and then takes in the extra fluff from ray.
                    _movePosition.x = -hit.distance + _boundsWidth / 2f + _skin * 2f;
                }

                _force.x = 0; // security check
            }
        }
        // Project the rays
        // Check if the rays plus a skin register a hit
        // if the rays hit, prevent the movement from proceeding
    }

    #region RayOrigins
    private void SetRayOrigins() {
        Bounds playerBounds = _boxCollider2D.bounds;

        _boundsBottomLeft = new Vector2(playerBounds.min.x, playerBounds.min.y);
        _boundsBottomRight = new Vector2(playerBounds.max.x, playerBounds.min.y);
        _boundsBottomMiddle = new Vector2(playerBounds.center.x, playerBounds.min.y);

        _boundsTopLeft = new Vector2(playerBounds.min.x, playerBounds.max.y);
        _boundsTopRight = new Vector2(playerBounds.max.x, playerBounds.max.y);
        _boundsTopMiddle = new Vector2(playerBounds.center.x, playerBounds.max.y);


        _boundsHeight = Vector2.Distance(_boundsBottomLeft, _boundsTopLeft);
        _boundsWidth = Vector2.Distance(_boundsBottomLeft, _boundsBottomRight);
    }
    #endregion

}
