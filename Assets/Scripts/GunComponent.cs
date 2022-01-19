using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent : MonoBehaviour
{

    public float bulletSpeed = 60f;
    public float fireRate = 12f; // How many rounds fired per second average fire rate = 700/min
    public GameObject bullet;
    public Transform barrel;
    public AudioSource audioSource;
    
    public AudioClip gunFireAudio;
    public AudioClip gunEmptyAudio;
    public AudioClip gunMagInAudio;
    public AudioClip gunMagOutAudio;
    
    public GameObject magazineSocket;

    private MagazineComponent _magazineComponent;
    private bool _isActive = false;
    public void Fire()
    {
        _magazineComponent = magazineSocket.GetComponentInChildren<MagazineComponent>();

        if (_magazineComponent != null)
        {
            Debug.Log(_magazineComponent.currentAmmoCount);

            if (_magazineComponent.currentAmmoCount > 0)
            {
                GameObject spawnedBullet = Instantiate(bullet, barrel.position, barrel.rotation);
                spawnedBullet.GetComponent<Rigidbody>().velocity = bulletSpeed * barrel.forward;
                audioSource.volume = 50f;
                audioSource.PlayOneShot(gunFireAudio);
                Destroy(spawnedBullet, 5);
                _magazineComponent.RemoveBullet();
            }
            else audioSource.PlayOneShot(gunEmptyAudio);
        }
        else audioSource.PlayOneShot(gunEmptyAudio);
    }

    private float timeBeforeShooting;

    private void Start()
    {
        timeBeforeShooting = 1 / fireRate;
    }

    private void Update()
    {
        if (_isActive)
        {
            if (timeBeforeShooting <= 0f)
            {
                Fire();
                timeBeforeShooting = 1 / fireRate;
            }
            else
            {
                timeBeforeShooting -= Time.deltaTime;
            }
        }
    }

    public void WeaponState()
    {
        _isActive = !_isActive;
    }

    public void PlayMagIn()
    {
        audioSource.PlayOneShot(gunMagInAudio);
    }
    
    public void PlayMagOut()
    {
        audioSource.PlayOneShot(gunMagOutAudio);
    }
}
