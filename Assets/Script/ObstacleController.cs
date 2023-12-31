using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController playerController;
    GameObject playerGO;
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("PlayerSpawner");
        playerController = playerGO.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            playerController.CopGotKilled(other.gameObject);
        }
    }
}
