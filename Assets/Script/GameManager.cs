using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject failPanel;
    public GameObject congratPanel;
    public static GameManager instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if(instance!=null && instance!= this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonTapped()
    {
        MainPanel.SetActive(false);
        GameObject playerSpawnerGO = GameObject.FindGameObjectWithTag("PlayerSpawner");
        PlayerController playerController = playerSpawnerGO.GetComponent<PlayerController>();
        playerController.MovePlayer();
    }

    public void ShowFailPanel()
    {
        failPanel.SetActive(true);
    }

    public void RestartButtonTapped()
    {
        LevelLoader.instance.GetLevel();
    }

    public void ShowCongratsPanel()
    {
        congratPanel.SetActive(true);
    }

    public void NextLevelButtonTapped()
    {
        LevelLoader.instance.NextLevel();
    }
}
