using UnityEngine;
using TMPro;
using UnityEngine.XR.Management;
using UnityEngine.XR;

public class Settings_UI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI masterVolumeText;
    [SerializeField] TextMeshProUGUI musicVolumeText;
    [SerializeField] TextMeshProUGUI crowdVolumeText;
    [SerializeField] TextMeshProUGUI heartbeatVolumeText;
    [SerializeField] TextMeshProUGUI punchVolumeText;
    [SerializeField] TextMeshProUGUI uiVolumeText;
    public void ChangeHandedness(bool isRightHanded)
    {
        Settings.playerSettings.IsRightHanded = isRightHanded;

    } 


    public void SaveSettings()
    {
        Settings.Save();
    }

    public void LoadSettings()
    {
        Settings.Load();
    }

    public void TryRecenter()
    {
        XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRInputSubsystem>().TryRecenter();
    }
    public void SetMasterVolume(float value)
    {
        Settings.audioSettings.MasterVolume = value;
        masterVolumeText.text =  Mathf.Round(value * 100).ToString();
    }
    public void SetMusicVolume(float value)
    {
        Settings.audioSettings.MusicVolume = value;
        musicVolumeText.text = Mathf.Round(value * 100).ToString();
    }
    public void SetCrowdVolume(float value)
    {
        Settings.audioSettings.CrowdVolume = value;
        crowdVolumeText.text = Mathf.Round(value * 100).ToString();
    }
    public void SetHeartbeatVolume(float value)
    {
        Settings.audioSettings.HeartbeatVolume = value;
        heartbeatVolumeText.text = Mathf.Round(value * 100).ToString();
    }
    public void SetPunchVolume(float value)
    {
        Settings.audioSettings.PunchVolume = value;
        punchVolumeText.text = Mathf.Round(value * 100).ToString();
    }
    public void SetUIVolume(float value)
    {
        Settings.audioSettings.UIVolume = value;
        uiVolumeText.text = Mathf.Round(value * 100).ToString();
    }
}
