using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickUpItem : NetworkBehaviour
{

    public float pickupRange = 0.5f;
    public Transform handPosition;

    //private Transform itemPosition = Vector3(handPosition.x + )
    public Camera playerCamera;
    public bool heldItem = false;

    //private Outline thisOutline;

    //public bool hasSelection = false;
    //private GameObject currentHighlightedItem = null;

    [SerializeField] GameObject sphereOnHand;
    [SerializeField] GameObject spherePrefab;

    public Timer timerScript;
    public GameObject timer;

    public bool isColliding = false;

    public PlayerAudio playerAudio;

    private NumberPickedup numberPickedup;

    void Start()
    {
        timer = GameObject.Find("Timer");
        timerScript = timer.GetComponent<Timer>();

        numberPickedup = timer.GetComponent<NumberPickedup>();
    }

    void Update()
    {

        //HighlightItem();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!heldItem)
            {
                TryPickupItem();
            }
            else
            {
                Debug.Log(heldItem);
                DropItem();
            }
        }
    }


    /*void HighlightItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("PickupItem"))
            {
                if (currentHighlightedItem != hit.collider.gameObject)
                {
                    if (currentHighlightedItem != null)
                    {
                        DisableOutline(currentHighlightedItem);
                    }
                    EnableOutline(hit.collider.gameObject);
                }
            }
            else
            {
                if (currentHighlightedItem != null)
                {
                    DisableOutline(currentHighlightedItem);
                    currentHighlightedItem = null;
                }
            }
        }
        else
        {
            if (currentHighlightedItem != null)
            {
                DisableOutline(currentHighlightedItem);
                currentHighlightedItem = null;
            }
        }
    }*/

    void TryPickupItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("PickupItem"))
            {
                PickupItem(hit.collider.gameObject);
                heldItem = true;
            }
        }
    }

/*    void EnableOutline(GameObject item)
    {
        thisOutline = item.GetComponent<Outline>();
        if (thisOutline != null)
        {
            thisOutline.enabled = true;
            hasSelection = true;
            currentHighlightedItem = item; // Update the currently highlighted item
        }
    }*/

/*    void DisableOutline(GameObject item)
    {
        Outline outline = item.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
            hasSelection = false;
        }
    }*/


    void PickupItem(GameObject item)
    {
        if (item.name == "Sphere")
        {
            playerAudio.PlayPickupSound(); // SFX
            sphereOnHand.SetActive(true);
        }
        numberPickedup.IncrementPickedUpCountServerRpc();

        RequestDestroyItemServerRpc(item.GetComponent<NetworkObject>().NetworkObjectId);
        
        //heldItem = item;
        //heldItem.transform.position = handPosition.position; // Move the item to the hand position
        //heldItem.transform.parent = handPosition; // Parent it to the hand so it moves with the player
        //heldItem.GetComponent<Rigidbody>().isKinematic = true; // Disable physics while holding the item
        //heldItem.GetComponent<BoxCollider>().enabled = false;
    }

    [ServerRpc]
    void RequestDestroyItemServerRpc(ulong networkObjectId)
    {
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        if (networkObject != null)
        {
            DestroyItemClientRpc(networkObjectId);
            networkObject.Despawn();
        }
    }

    [ClientRpc]
    void DestroyItemClientRpc(ulong networkObjectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject networkObject))
        {
            Destroy(networkObject.gameObject);
        }
    }


    /*void DropItem()
    {
        //heldItem.GetComponent<BoxCollider>().enabled = true;
        //heldItem.GetComponent<Rigidbody>().isKinematic = false; // Re-enable physics
        //heldItem.transform.parent = null; // Unparent the item
        if (heldItem.CompareTag("Exchange")) // Check if the dropped item collides with an object tagged 'Exchange'
        {
            Debug.Log("Add 30 seconds");
            //playerTimer.AddTime(30); // Add 30 seconds to the timer
            Destroy(heldItem);
        }
        else
        {
            heldItem = null; // Clear the reference
        }
    }*/


    void DropItem()
    {
        if (isColliding)
        {
            sphereOnHand.SetActive(false);
            timerScript.AddTime(30);
            heldItem = false;
            playerAudio.PlayDropitemSound(); // SFX
            Debug.Log(heldItem);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exchange"))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Exchange"))
        {
            isColliding = false;
        }
    }
}