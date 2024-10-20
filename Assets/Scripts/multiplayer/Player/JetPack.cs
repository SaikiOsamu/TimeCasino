using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JetPack : NetworkBehaviour
{
    [SerializeField] int costTime = 30;
    public Camera playerCamera;
    public float jetPackHeight = 20f;
    public float jetPackDuration = 5f;
    public float flyUpSpeed = 2f;
    public float dropSpeed = 10f;
    private PlayerMovement playerMovement;
    private bool isUsingJetPack = false;

    private GameObject timer;
    private Timer timerScript;

    public PlayerAudio playerAudio;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        timer = GameObject.Find("Timer");
        timerScript = timer.GetComponent<Timer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isUsingJetPack && !timerScript.CheckIfDead())
        {
            StartCoroutine(UseJetPack());
            timerScript.RemoveTime(costTime);
        }

    }

    IEnumerator UseJetPack()
    {
        isUsingJetPack = true;

        // Disable WASD movement
        playerMovement.enabled = false;

        playerAudio.JetpackSound();
        // Move the player up gradually
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, jetPackHeight, 0);
        float elapsedTime = 0f;

        while (elapsedTime < jetPackDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, (elapsedTime / jetPackDuration) * flyUpSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for the remaining duration while player is at the top
        yield return new WaitForSeconds(jetPackDuration - elapsedTime);

        // Drop the player down quickly
        elapsedTime = 0f;
        while (transform.position.y > initialPosition.y)
        {
            transform.position = Vector3.Lerp(targetPosition, initialPosition, elapsedTime * dropSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player is back to the initial position
        transform.position = initialPosition;

        // Enable WASD movement
        playerMovement.enabled = true;

        isUsingJetPack = false;
    }
}