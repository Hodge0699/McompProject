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
        // checks to see if the file already exists on the scene
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
            if(player != null)
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
    /// <summary>
    /// updates the file variables with the data from real time to be saved
    /// </summary>
    private void dataUpdate()
    {

        health = playerHealth.getHealth();
        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;

    }
    /// <summary>
    /// updates character data from the stored file data
    /// </summary>
    private void dataLoad()
    {
        playerHealth.setHealth(health, true);
    }
    /// <summary>
    /// Save function to save all the data into a file
    /// </summary>
    public void Save()
    {
        Debug.Log("Saving File");
        if (playerHealth.isAlive)
        {
            dataUpdate();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat"); // creates player file

            PlayerData data = new PlayerData(health, sceneName); // creates a class that holds all the data to be sabed
            bf.Serialize(file, data);// encrypts the file by making it a binrary
            file.Close(); 
        }
    }
    /// <summary>
    /// loads the saved player file and all its data stored
    /// </summary>
    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open); // opens the file
            PlayerData data = (PlayerData)bf.Deserialize(file); // decrypt the file to get the data
            file.Close();
            health = data.health; // updates this files variable with the saved file data
            sceneName = data.sceneName; // updates this files variable with the saved file data
            SceneManager.LoadScene(sceneName); // change scenes
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