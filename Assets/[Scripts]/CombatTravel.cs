using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class CombatTravel : MonoBehaviour
{

    public Light2D light;
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.gameObject.tag == "Player")
        {
            PlayerController.canOpenDoor = true;
            light.color = Color.green;
            SceneManager.LoadScene("CombatScene");
        }
    }
}
