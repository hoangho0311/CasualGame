using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerGO;
    public List<GameObject> playerList = new List<GameObject>();

    float playerSpeed = 5;
    float xSpeed;
    float maxPosition = 4.1f;
    bool isPlayerMoving;
    // Start is called before the first frame update

    public AudioSource playerSpawnerAudioSource;
    public AudioClip gateClip, congratsClip, failClip;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerMoving == false)
        {
            return;
        }
        float touchX = 0;
        float newXValue = 0;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            xSpeed = 100;
            touchX = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }

        if (Input.GetMouseButton(0))
        {
            xSpeed = 100;
            touchX = Input.GetAxis("Mouse X");
        }
        newXValue = transform.position.x + xSpeed * touchX * Time.deltaTime;
        newXValue = Mathf.Clamp(newXValue, -maxPosition, maxPosition);
        Vector3 playerPosition = new Vector3(newXValue, transform.position.y, transform.position.z + Time.deltaTime * playerSpeed);
        transform.position = playerPosition;
    }
    public void SpawnPlayer(int gateValue, GateType gateType)
    {
        PlayAudio(gateClip);

        if (gateType == GateType.additionType)
        {
            for (int i = 0; i < gateValue; i++)
            {
                GameObject newPlayerGo = Instantiate(playerGO, GetPlayerPosition(), Quaternion.identity, transform);
                playerList.Add(newPlayerGo);
            }
        }
        else if (gateType == GateType.multiplyType)
        {
            int newPlayerCount = (playerList.Count * gateValue) - playerList.Count;
            for (int i = 0; i < newPlayerCount; i++)
            {
                GameObject newPlayerGo = Instantiate(playerGO, GetPlayerPosition(), Quaternion.identity, transform);
                playerList.Add(newPlayerGo);
            }
        }

    }

    void StopPlayer()
    {
        isPlayerMoving = false;
    }

    public Vector3 GetPlayerPosition()
    {
        Vector3 position = Random.insideUnitSphere * 0.1f;
        Vector3 newPos = transform.position + position;
        return newPos;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Finish_line")
        {
            StopPlayer();
            ChangeAllCopToIdleAnim();
            StopBGMusic();
            PlayAudio(congratsClip);
            GameManager.instance.ShowCongratsPanel();
        }
    }

    public void CopGotKilled(GameObject copGO)
    {
        playerList.Remove(copGO);
        Destroy(copGO);
        DetectCopCount();
    }

    void DetectCopCount()
    {
        if (playerList.Count <= 0)
        {
            StopPlayer();
            StopBGMusic();
            PlayAudio(failClip);
            GameManager.instance.ShowFailPanel();
        }
    }

    public void EnemyDetected(GameObject target)
    {
        isPlayerMoving = false;
        LookAtEnemy(target);
        StartAllCopsShooting();
    }

    void LookAtEnemy(GameObject target)
    {
        Vector3 dir = target.transform.position - transform.position;
        Quaternion LookRotation = Quaternion.LookRotation(dir);
        LookRotation.x = 0;
        LookRotation.z = 0;
        transform.rotation = LookRotation;
    }
    void LookAtForward()
    {
        transform.rotation = Quaternion.identity;
    }

    public void AllZombiesKilled()
    {
        LookAtForward();
        MovePlayer();
    }
    
    public void MovePlayer()
    {
        isPlayerMoving = true;
        ChangeAllCopRun();
    }

    void StartAllCopsShooting()
    {
        for(int i = 0; i < playerList.Count; i++)
        {
            PlayerControll cop = playerList[i].GetComponent<PlayerControll>();
            cop.StartShooting();
        }
    }

    void ChangeAllCopRun()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            PlayerControll cop = playerList[i].GetComponent<PlayerControll>();
            cop.StopShotting();
        }
    }

    void ChangeAllCopToIdleAnim()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            PlayerControll cop = playerList[i].GetComponent<PlayerControll>();
            cop.StartIdleAnim();
        }
    }

    void PlayAudio(AudioClip audio)
    {
        if (playerSpawnerAudioSource != null)
        {
            playerSpawnerAudioSource.PlayOneShot(audio, 0.5f);
        }
    }

    void StopBGMusic()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
    }
}
