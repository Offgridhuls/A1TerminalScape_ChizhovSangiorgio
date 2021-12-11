using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(playAudio());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator playAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();

        audio.volume = 0f;

        while (audio.volume < 1f)
        {
            audio.volume = Mathf.Lerp(audio.volume, 0f, 1f * Time.deltaTime);
            yield return 1f;
        }
    }
}
