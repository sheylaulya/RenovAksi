using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OpenTab(0);
    }

    // Update is called once per frame
    public void OpenTab(int index)
    {
        for (int i = 0; i < tabImages.Length; i++)
        {
            if (i == index)
            {
                tabImages[i].color = Color.white; // Active tab color
                pages[i].SetActive(true);
            }
            else
            {
                tabImages[i].color = Color.gray; // Inactive tab color
                pages[i].SetActive(false);
            }
        }
    }
}
