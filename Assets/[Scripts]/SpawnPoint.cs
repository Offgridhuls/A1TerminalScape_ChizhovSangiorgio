using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject playerPrefab;

    static PlayerController playerReference = null;
    // Start is called before the first frame update
    void Start()
    {
        if (playerReference == null)
        {
            GameObject playerObject = Instantiate(playerPrefab, transform.position, transform.rotation);
            playerReference = playerObject.GetComponent<PlayerController>();
        }
    }
}
