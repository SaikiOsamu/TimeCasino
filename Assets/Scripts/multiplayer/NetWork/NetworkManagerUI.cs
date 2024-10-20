using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private GameObject networkManagerUI;
    [SerializeField] private Button QuitBtn;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject gameWinUI;

    public PlayerCount haven;

    public int playerNumber = 2;

    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private Button confirmIPButton;

    public GameObject Timer;
    public Timer timer;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            networkManagerUI.SetActive(false);
            timer.RestoreTime();
        });
        clientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            networkManagerUI.SetActive(false);
            timer.RestoreTime();
        });

        confirmIPButton.onClick.AddListener(() => {
            UpdateNetworkAddress();
        });
    }

    public void Update()
    {
        if (haven.count >= 2)
        {
            GameOverUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        CheckForSpheres();
    }

    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void MinusPlayer()
    {
        playerNumber--;
        if (playerNumber <= 0)
        {
            GameOverUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        Debug.Log(playerNumber);
    }

    private void UpdateNetworkAddress()
    {
        string ipAddress = ipInputField.text;
        if (!string.IsNullOrEmpty(ipAddress))
        {
            NetworkManager networkManager = GameObject.FindObjectOfType<NetworkManager>();
            if (networkManager != null)
            {
                UnityTransport transport = networkManager.GetComponent<UnityTransport>();
                if (transport != null)
                {
                    transport.ConnectionData.Address = ipAddress;
                    Debug.Log("Updated IP Address: " + ipAddress);
                }
                else
                {
                    Debug.LogError("UnityTransport component not found on NetworkManager.");
                }
            }
            else
            {
                Debug.LogError("NetworkManager not found in the scene.");
            }
        }
    }

    void CheckForSpheres()
    {
        GameObject[] spheres = GameObject.FindGameObjectsWithTag("PickupItem");
        bool anySpheresLeft = false;

        foreach (GameObject sphere in spheres)
        {
            if (sphere.name == "Sphere")
            {
                anySpheresLeft = true;
                break;
            }
        }

        if (!anySpheresLeft)
        {
            GameWin();
        }
    }

    void GameWin()
    {
        gameWinUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    /*public int GetPlayerCount()
    {
        if (NetworkManager.Singleton != null)
        {
            return NetworkManager.Singleton.ConnectedClientsList.Count;
         
        }
        else
        {
            Debug.LogError("NetworkManager is not initialized.");
            return 0;
        }
    }*/

}
