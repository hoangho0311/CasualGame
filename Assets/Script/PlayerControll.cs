using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    Animator playerAnimator;
    public GameObject bulletGO;
    public Transform bulletSpawnTransform;
    float bulletSpeed = 13f;
    bool isShootingOn = false;
    Transform playerSpawnerCenter;
    float goToCenterSpeed = 4f;

    public AudioSource playerSpawnerAudio;
    public AudioClip shootClip;
    // Start is called before the first frame update
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerSpawnerCenter = transform.parent.gameObject.transform;
        PlayerController playerSpawner = transform.parent.gameObject.GetComponent<PlayerController>();
        playerSpawnerAudio = playerSpawner.playerSpawnerAudioSource;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerSpawnerCenter.position, Time.fixedDeltaTime * goToCenterSpeed);
    }

    public void StartShooting()
    {
        StartShootingAnim();
        isShootingOn = true;
        StartCoroutine(Shooting());
    }

    public void StopShotting()
    {
        isShootingOn = false;
        StartRunningAnim();
    }

    private void StartShootingAnim()
    {
        playerAnimator.SetBool("IsShooting", true);
        playerAnimator.SetBool("IsRunning", false);
    }

    private void StartRunningAnim()
    {
        playerAnimator.SetBool("IsRunning", true);
        playerAnimator.SetBool("IsShooting", false);
    }

    public void StartIdleAnim()
    {
        playerAnimator.SetBool("IsLevelFinished", true);
        playerAnimator.SetBool("IsRunning", false);
    }

    IEnumerator Shooting()
    {
        while (isShootingOn)
        {
            yield return new WaitForSeconds(0.5f);
            Shoot();
            yield return new WaitForSeconds(2f);

        }
    }

    void Shoot()
    {
        PlayAudio();
        GameObject bullet = Instantiate(bulletGO, bulletSpawnTransform.position, Quaternion.identity);
        Rigidbody bulletRigibody = bullet.GetComponent<Rigidbody>();
        bulletRigibody.velocity = transform.forward * bulletSpeed;

    }

    void PlayAudio()
    {
        if(playerSpawnerAudio != null)
        {
            playerSpawnerAudio.PlayOneShot(shootClip, 0.3f);
        }
    }
}
