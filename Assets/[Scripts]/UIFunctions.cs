using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panel;
    public void OpenPanel()
    {
        if(panel != null)
        {
            panel.SetActive(true);
        }   
    }
}
