using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {

    public static GameController control;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    HealthManager.PlayerHealthManager playerHealth;
    public float health;
    string sceneName;
    Scene scene;
    [SerializeField]
    private float test = 5;
    private bool saved = false;
    [SerializeField]
    private int firstData = 1;
    private float loadedHealth;


    private void Awake()
    {
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }
    private void Update()
    {
        if (player == null && scene.name != "MainMenu")
        {
            player = GameObject.Find("Player(Clone)");
            playerHealth = player.GetComponent<HealthManager.PlayerHealthManager>();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Save();
        }
        if(firstData == 0)
        {
            dataLoad();
            firstData = 1;
        }
    }

    private void dataUpdate()
    {

        health = playerHealth.getHealth();
        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;

    }

    private void dataLoad()
    {
        playerHealth.setHealth(health, true);
        
        
    }

    public void Save()
    {
        Debug.Log("Saving File");
        if (playerHealth.isAlive)
        {
            dataUpdate();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

            PlayerData data = new PlayerData(health, sceneName);
            bf.Serialize(file, data);
            file.Close();
        }
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            health = data.health;
            sceneName = data.sceneName;
            SceneManager.LoadScene(sceneName);
            firstData = 0;
        }
    }
}

[Serializable]
class PlayerData
{
    public float health;
    public string sceneName;
    public PlayerData(float phealth, String pscene)
    {
        health = phealth;
        sceneName = pscene;
    }
}