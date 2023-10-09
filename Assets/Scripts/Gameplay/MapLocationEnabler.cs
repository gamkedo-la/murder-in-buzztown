using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocationEnabler : MonoBehaviour
{
    [SerializeField] private GameObject _location;
    [SerializeField] private GameObject _playerMarker;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _location.SetActive(true);
            _playerMarker.transform.position = _location.transform.position - new Vector3(0, 40, 0);
        }
    }
}
