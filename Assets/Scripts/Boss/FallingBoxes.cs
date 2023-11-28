using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoxes : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Vector3 _targetPosition;
    private bool finished = false;

    private void Update()
    {
        if (!finished)
        {
            if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            {
                finished = true;
                _rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}
