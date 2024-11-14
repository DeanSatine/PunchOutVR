using UnityEngine;
using FMODUnity;
using FMOD;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] EventReference exampleEvent;
    Bus masterBus;
    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RuntimeManager.StudioSystem.getBus("bus:/", out masterBus);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayOneShot(EventReference fmodEvent)
    {
        RuntimeManager.PlayOneShot(fmodEvent);
    }

    public void UpdateHealthFmodParam(int health)
    {
        RuntimeManager.StudioSystem.setParameterByName("Health", health);
    }

    public void AssignMasterVolume(float volume)
    {
        masterBus.setVolume(volume);
    }
}
