using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickUpCard : MonoBehaviour
{
    public float pickupRange = 1f;
    public Camera playerCamera;
    public bool isFull = false;
    public CardUIManager cardUIManager;
    public SoloPlayerMovement soloPlayerMovement;
    public SoloCreateDoor soloCreateDoor;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            cardUIManager.DropLeft();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            cardUIManager.DropRight();
        }
    }

    void TryPickupItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Card"))
            {
                if (cardUIManager.CheckBotEmpty() != -1)
                {
                    string cardName = hit.collider.name;
                    int cardNumber = 0;

                    // Check which suit it is and calculate the card number based on the suit name length
                    if (cardName.Contains("Heart"))
                    {
                        cardNumber = int.Parse(cardName.Substring("Heart_".Length, 2));

                        // Specific logic for Heart cards
                        if (cardNumber == 1)
                        {
                            soloPlayerMovement.SpeedUp();
                            cardUIManager.UpdateUtilLogo(1);
                        }
                        else if (cardNumber == 2)
                        {
                            soloCreateDoor.enabled = true;
                        }

                        cardUIManager.ReplaceCard("Heart", cardNumber);
                    }
                    else if (cardName.Contains("Spade"))
                    {
                        cardNumber = int.Parse(cardName.Substring("Spade_".Length, 2));

                        // Specific logic for Spade cards
                        if (cardNumber == 1)
                        {
                            soloPlayerMovement.SpeedUp();
                            cardUIManager.UpdateUtilLogo(2);
                        }

                        cardUIManager.ReplaceCard("Spade", cardNumber);
                    }
                    else if (cardName.Contains("Clubs"))
                    {
                        cardNumber = int.Parse(cardName.Substring("Clubs_".Length, 2));

                        // Specific logic for Club cards
                        if (cardNumber == 1)
                        {
                            soloPlayerMovement.SpeedUp();
                            cardUIManager.UpdateUtilLogo(3);
                        }

                        cardUIManager.ReplaceCard("Clubs", cardNumber);
                    }
                    else if (cardName.Contains("Diamond"))
                    {
                        cardNumber = int.Parse(cardName.Substring("Diamond_".Length, 2));

                        // Specific logic for Diamond cards
                        if (cardNumber == 1)
                        {
                            soloPlayerMovement.SpeedUp();
                            cardUIManager.UpdateUtilLogo(4);
                        }

                        cardUIManager.ReplaceCard("Diamond", cardNumber);
                    }

                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }



    void PickupItem(GameObject item)
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exchange"))
        {
            //isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Exchange"))
        {
            //isColliding = false;
        }
    }
}
