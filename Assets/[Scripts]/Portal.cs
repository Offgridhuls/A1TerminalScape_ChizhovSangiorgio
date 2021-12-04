using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panel1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        WorldTraveler TravelerObject = collision.GetComponent<WorldTraveler>();
        if (TravelerObject != null)
        {
            if (panel1 != null)
            {
                panel1.SetActive(true);
                Debug.Log("ree");
            }
           // SceneManager.LoadScene(tag);
        }
    }
}
