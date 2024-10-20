using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    public int count;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           count++; 
        }
        
    }
}
