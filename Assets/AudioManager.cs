using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public EventReference UI_OnHover;
    public EventReference level_Music;

    EventInstance musicInstance;
    Bus masterBus;
    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMusic()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(level_Music);
        musicInstance.start();

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

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }
}
