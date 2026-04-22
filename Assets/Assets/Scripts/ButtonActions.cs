using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
