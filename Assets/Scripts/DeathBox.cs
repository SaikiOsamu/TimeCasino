using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private GameObject haven;
    private Transform havenTransform;

    // Start is called before the first frame update
    void Start()
    {
        haven = GameObject.Find("Haven");
        if (haven != null)
        {
            havenTransform = haven.transform;
        }
        else
        {
            Debug.LogError("Haven GameObject not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (havenTransform != null)
            {
                other.transform.position = havenTransform.position;
                Debug.Log("Player moved to Haven.");
            }
            else
            {
                Debug.LogError("Haven Transform is null.");
            }
        }
    }
}
