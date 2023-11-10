using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _spawnTime;
    [SerializeField] private bool _isRight;
    // Start is called before the first frame update
    void Start()
    {
        // InvokeRepeating("Shoot", 1f, _spawnTime);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _spawnPoint.position, Quaternion.identity);
        bullet.GetComponent<TurretBullet>().SetDirection(_isRight);

        PlayTurretShotSoundIfInViewport();
    }

    private void PlayTurretShotSoundIfInViewport()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
            viewportPosition.y >= 0 && viewportPosition.y <= 1)
        {
            AudioManager.Instance.PlayEffect(AudioManager.Instance.turretShotAudioClip);
        }
    }
}
