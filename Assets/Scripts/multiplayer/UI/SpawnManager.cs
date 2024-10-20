using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : NetworkBehaviour
{
    public GameObject spawnUI;
    public GameObject abilityUI;
    public Button spawnButton1;
    public Button spawnButton2;
    public Button spawnButton3;
    public Button spawnButton4;
    public Button abilityButton1;
    public Button abilityButton2;
    public Button abilityButton3;
    private GameObject spawnLocation1;
    private GameObject spawnLocation2;
    private GameObject spawnLocation3;
    private GameObject spawnLocation4;
    public JetPack jetPack;
    public MarkSign marker;
    public CreateDoor createDoor;
    private PlayerMovement playerMovement;

    public GameObject gun;
    public GameObject spray;
    public GameObject jetpack;


    void Start()
    {
        Debug.Log("SpawnManager Start");
        playerMovement = GetComponent<PlayerMovement>();
        if (!IsOwner) return;
        spawnLocation1 = GameObject.Find("SpawnLocation1");
        spawnLocation2 = GameObject.Find("SpawnLocation2");
        spawnLocation3 = GameObject.Find("SpawnLocation3");
        spawnLocation4 = GameObject.Find("SpawnLocation4");
        DisablePlayerMovement();
        ShowSpawnUI();

        // Setup button listeners
        spawnButton1.onClick.AddListener(() => ChooseSpawnLocation(spawnLocation1));
        spawnButton2.onClick.AddListener(() => ChooseSpawnLocation(spawnLocation2));
        spawnButton3.onClick.AddListener(() => ChooseSpawnLocation(spawnLocation3));
        spawnButton4.onClick.AddListener(() => ChooseSpawnLocation(spawnLocation4));
        abilityButton1.onClick.AddListener(() => ChooseJetPack(jetPack));
        abilityButton2.onClick.AddListener(() => ChooseMarker(marker));
        abilityButton3.onClick.AddListener(() => ChooseCreateDoor(createDoor));
    }

    void DisablePlayerMovement()
    {
        Debug.Log("Disabling Player Movement");
        playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void EnablePlayerMovement()
    {
        Debug.Log("Enabling Player Movement");
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ShowSpawnUI()
    {
        spawnUI.SetActive(true);
        abilityUI.SetActive(false);
    }

    void ShowAbilityUI()
    {
        spawnUI.SetActive(false);
        abilityUI.SetActive(true);
    }

    public void ChooseSpawnLocation(GameObject spawnLocation)
    {
        transform.position = spawnLocation.transform.position;
        ShowAbilityUI();
    }

    public void ChooseJetPack(JetPack ability)
    {
        ability.enabled = true;
        jetpack.SetActive(true);
        FinishSetup();
    }

    public void ChooseMarker(MarkSign ability)
    {
        ability.enabled = true;
        spray.SetActive(true);
        FinishSetup();
    }

    public void ChooseCreateDoor(CreateDoor ability) 
    {
        ability.enabled = true;
        gun.SetActive(true);
        FinishSetup();
    }

    void FinishSetup()
    {
        abilityUI.SetActive(false);
        EnablePlayerMovement();
    }
}
