using UnityEngine;
using TMPro;
public class Settings_UI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI masterVolumeText;
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

    public void SetMasterVolume(float value)
    {
        Settings.audioSettings.MasterVolume = value;
        masterVolumeText.text =  Mathf.Round(value * 100).ToString();
    }
}
