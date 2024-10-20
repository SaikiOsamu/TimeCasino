using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenameSelf : MonoBehaviour
{
    public string name;

    void Start()
    {
        this.gameObject.name = name;
    }


}
