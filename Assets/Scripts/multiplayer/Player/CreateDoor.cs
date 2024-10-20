using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class CreateDoor : NetworkBehaviour
{
    [SerializeField] int costTime = 100;

    public Camera playerCamera;
    public float doorRange = 1.2f;
    public GameObject doorPrefab;

    private GameObject timer;
    private Timer timerScript;

    // Reference to the current door
    private GameObject currentDoor;

    public PlayerAudio playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.Find("Timer");
        timerScript = timer.GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.Q) && !timerScript.CheckIfDead())
        {
            TryCreateDoor();
        }
    }

    void TryCreateDoor()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, doorRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Vector3 position = hit.point;
                float wallYRotation = hit.collider.transform.eulerAngles.y;

                // Set the door's rotation based on the wall's rotation
                Quaternion rotation;
                if (Mathf.Approximately(wallYRotation, 90f) || Mathf.Approximately(wallYRotation, -90f))
                {
                    rotation = Quaternion.Euler(0f, 0f, 0f); // Door with y rotation 0
                }
                else if (Mathf.Approximately(wallYRotation, 0f) || Mathf.Approximately(wallYRotation, 180f))
                {
                    rotation = Quaternion.Euler(0f, 90f, 0f); // Door with y rotation 90
                }
                else
                {
                    rotation = Quaternion.identity; // Default rotation if needed
                }
                playerAudio.CreatePortalSound();
                RequestDoorServerRpc(position, rotation);
                timerScript.RemoveTime(costTime);
            }
            else
            {
                Debug.Log("This is not a Wall");
            }
        }
    }

    [ServerRpc]
    void RequestDoorServerRpc(Vector3 position, Quaternion rotation)
    {
        if (currentDoor != null)
        {
            Destroy(currentDoor);
        }

        GameObject newDoor = Instantiate(doorPrefab, position, rotation);
        newDoor.GetComponent<NetworkObject>().Spawn();
        currentDoor = newDoor;
    }
}
