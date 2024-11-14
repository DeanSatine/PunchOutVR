using System;
using System.IO;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[Serializable]
public static class Settings
{
    static string settingsFilePath = Path.Combine(Application.persistentDataPath, "Settings.VRPO"); // path for settings file.
    public static AudioSettings audioSettings = new();
    public static PlayerSettings playerSettings = new();
    public static Action<bool> OnHandednessChange = (value) => { }; // given empty lambda expression to avoid crashing if invoked with no subscribed methods.

    static Settings()
    {
       // Load(); // load previous settings on game start.
    }

    public static void Save()
    {
        try
        {
            // create container class for settings
            var settingsData = new SettingsData(audioSettings, playerSettings);

            // settings to JSON
            string jsonData = JsonUtility.ToJson(settingsData, true);

            // JSON to file
            File.WriteAllText(settingsFilePath, jsonData);
            Debug.Log("Settings saved to " + settingsFilePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save settings: " + e.Message);
        }
    }


    public static void Load()
    {
        if (!File.Exists(settingsFilePath)) // if file doesnt exist, use default settings.
        {
            Debug.LogWarning("Settings file not found, creating new default settings.");
            audioSettings = new ();
            playerSettings = new ();
            return;
        }

        try
        {
            // read JSON from file
            string jsonData = File.ReadAllText(settingsFilePath);

            //  JSON to SettingsData
            SettingsData settingsData = JsonUtility.FromJson<SettingsData>(jsonData);

            // assign loaded values
            audioSettings = settingsData.audioSettings;
            playerSettings = settingsData.playerSettings;

            // notify listeners if handedness changed
            OnHandednessChange?.Invoke(playerSettings.IsRightHanded);

            Debug.Log("Settings loaded from " + settingsFilePath);
        }
        catch (Exception e)
        {
            playerSettings = new();
            audioSettings = new();
            Debug.LogError("Failed to load settings: " + e.Message);
        }
    }

    // Container class for saving and loading both settings
    [Serializable]
    private class SettingsData
    {
        public AudioSettings audioSettings;
        public PlayerSettings playerSettings;

        public SettingsData(AudioSettings audioSettings, PlayerSettings playerSettings)
        {
            this.audioSettings = audioSettings;
            this.playerSettings = playerSettings;
        }
    }
}


[Serializable]
public class AudioSettings
{
    Bus masterBus;
    Bus crowdBus;
    Bus heartbeatBus;
    Bus punchBus;
    Bus uiBus;


    public AudioSettings()
    {
        RuntimeManager.StudioSystem.getBus("bus:/", out masterBus);
        RuntimeManager.StudioSystem.getBus("bus:/Crowd_Cheering", out crowdBus);
        RuntimeManager.StudioSystem.getBus("bus:/Heartbeat", out heartbeatBus);
        RuntimeManager.StudioSystem.getBus("bus:/Punch_Sounds", out punchBus);
        RuntimeManager.StudioSystem.getBus("bus:/UI_Sounds", out uiBus);
        //RuntimeManager.StudioSystem.getBus("bus:/", out masterBus);
    }


    float master_Volume = 1;
    public float MasterVolume
    {
        get { return master_Volume; }
        set { master_Volume = value; masterBus.setVolume(Mathf.Round(value * 100) / 100); }
    }
    
    float crowd_Volume = 1;
    public float CrowdVolume
    {
        get { return crowd_Volume; }
        set { crowd_Volume = value; crowdBus.setVolume(Mathf.Round(value * 100) / 100); }
    }
   
    float heartbeat_Volume = 1;
    public float HeartbeatVolume
    {
        get { return heartbeat_Volume; }
        set { heartbeat_Volume = value; heartbeatBus.setVolume(Mathf.Round(value * 100) / 100); }
    }
   
    float punch_Volume = 1;
    public float PunchVolume
    {
        get { return PunchVolume; }
        set { punch_Volume = value; punchBus.setVolume(Mathf.Round(value * 100) / 100); }
    }
    
    float ui_Volume = 1;
    public float UIVolume
    {
        get { return ui_Volume; }
        set { ui_Volume = value; uiBus.setVolume(Mathf.Round(value * 100) / 100); }
    }


}

[Serializable]
public class PlayerSettings
{
    bool isRightHanded = true;
    public bool IsRightHanded {  
        get { return isRightHanded; }
        set { 
            isRightHanded = value; 
            Settings.OnHandednessChange(value);
        }
    }

}