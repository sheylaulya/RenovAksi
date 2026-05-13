using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    public void ClosePanel(GameObject panel)
    {
        AudioManager.instance.PlaySFX("Click");
        panel.SetActive(false);
    }

    public void OpenPanel(GameObject panel)
    {
        AudioManager.instance.PlaySFX("Click");
        panel.SetActive(true);
    }

    public void PlayClickSound()
    {
        AudioManager.instance.PlaySFX("Click");
    }

}
