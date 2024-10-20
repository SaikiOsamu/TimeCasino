using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class NumberPickedup : NetworkBehaviour
{
    [SerializeField] private TMP_Text pickedUpCountText;

    private NetworkVariable<int> pickedUpCount = new NetworkVariable<int>(0);

    void Start()
    {
        // Ensure the UI is updated initially
        UpdateUI(pickedUpCount.Value);

        // Listen for changes to the network variable
        pickedUpCount.OnValueChanged += (oldValue, newValue) =>
        {
            UpdateUI(newValue);
        };
    }

    void UpdateUI(int count)
    {
        pickedUpCountText.text = $"Items Picked Up: {count}/12";
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncrementPickedUpCountServerRpc()
    {
        pickedUpCount.Value++;
    }
}