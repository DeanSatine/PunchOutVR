using UnityEngine;
using FMODUnity;
using FMOD;
public class AudioManager : MonoBehaviour
{
    [SerializeField] EventReference exampleEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayOneShot()
    {
        RuntimeManager.PlayOneShot(exampleEvent);
    }
   
}
