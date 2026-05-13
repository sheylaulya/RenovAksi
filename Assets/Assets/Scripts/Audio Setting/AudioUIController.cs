using UnityEngine;
using UnityEngine.UI;

public class AudioUIController : MonoBehaviour
{
    public Slider _musicVolumeSlider, _sfxVolumeSlider;
    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }

    public void SetMusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicVolumeSlider.value);
    }

    public void SetSFXVolume()
    {
        AudioManager.instance.SFXVolume(_sfxVolumeSlider.value);
    }
}
