using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneTemporaryInventory : MonoBehaviour
{

    public bool collectedKey = false;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void CollectKey()
    {
        collectedKey = true;
        Debug.Log("Clone Picked Up a key!");

    }

    public void TransferToPlayer()
    {
        if(collectedKey && player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if(inventory != null)
            {
                inventory.GrantKey();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
