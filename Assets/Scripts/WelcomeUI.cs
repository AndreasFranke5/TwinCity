using UnityEngine;

public class WelcomeUI : MonoBehaviour
{
    public GameObject uiPanel;

    public void StartExperience()
    {
        uiPanel.SetActive(false); // Hides the welcome screen
    }
}

