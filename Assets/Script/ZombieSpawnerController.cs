
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerController : MonoBehaviour
{
    public GameObject zombie;
    public List<GameObject> zombies = new List<GameObject>();
    PlayerController PlayerSpawner;
    GameObject playerSpawnerGO;
    public bool isZombieAttack;
    public int zombieCount;

    private void Awake()
    {
        playerSpawnerGO = GameObject.FindGameObjectWithTag("PlayerSpawner");
        PlayerSpawner = playerSpawnerGO.GetComponent<PlayerController>();
        isZombieAttack = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnZombie(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnZombie(int zombieCount)
    {
        for (int i = 0; i < zombieCount; i++)
        {
            Quaternion zombieRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            GameObject zombieGO = Instantiate(zombie, GetZombiePosition(), zombieRotation, transform);
            ZombieControllee zombieScript = zombieGO.GetComponent<ZombieControllee>();
            zombieScript.playerSpawnerGO = playerSpawnerGO;
            zombieScript.zombieSpawner = this;
            zombies.Add(zombieGO);
        }  
    }

    public Vector3 GetZombiePosition()
    {
        Vector3 pos = Random.insideUnitSphere * 0.1f;
        Vector3 newPos = transform.position + pos;
        return newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false;
            PlayerSpawner.EnemyDetected(gameObject);
            LookAtPLayer(other.gameObject);
            isZombieAttack = true;
        }
    }

    void LookAtPLayer(GameObject target)
    {
        Vector3 dir = transform.position - target.transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.x = 0;
        lookRot.z = 0;

        transform.rotation = lookRot;
    }

    public void ZombieAttackThisCop(GameObject cop, GameObject zombie)
    {
        zombies.Remove(zombie);
        CheckZombieCount();
        PlayerSpawner.CopGotKilled(cop);
    }

    public void ZombieGotShoot(GameObject zom)
    {
        zombies.Remove(zom);
        Destroy(zom);
        CheckZombieCount();
    }

    void CheckZombieCount()
    {
        if(zombies.Count <= 0)
        {
            PlayerSpawner.AllZombiesKilled();
        }
    }
}
