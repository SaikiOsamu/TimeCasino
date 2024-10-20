using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MarkSign : NetworkBehaviour
{
    [SerializeField] int costTime = 5;

    public Camera playerCamera;
    public float markRange = 1.2f;
    public GameObject signPrefab;

    private GameObject timer;
    private Timer timerScript;

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
            TryMarkSign();
        }
    }

    void TryMarkSign()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, markRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Vector3 position = hit.point;
                Quaternion rotation = (hit.collider.transform.rotation.y == 0f || hit.collider.transform.rotation.y == 1f)
                                        ? Quaternion.identity
                                        : Quaternion.Euler(0f, 90f, 0f);
                playerAudio.SpraySound();
                RequestMarkSignServerRpc(position, rotation);
                timerScript.RemoveTime(costTime);
            }
            else
            {
                Debug.Log("This is not a Wall");
            }
        }
    }

    [ServerRpc]
    void RequestMarkSignServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject newSign = Instantiate(signPrefab, position, rotation);
        newSign.GetComponent<NetworkObject>().Spawn();
    }
}