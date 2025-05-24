using UnityEngine;

public class WelcomeUI : MonoBehaviour
{
    public GameObject uiPanel;   // 👈 поле для Welcome Canvas
    public GameObject mainUI;    // 👈 поле для Main Canvas

    public void StartExperience()
    {
        Debug.Log("Start clicked!");   // опционально
        uiPanel.SetActive(false);      // спрятать welcome
        mainUI.SetActive(true);        // показать main
    }
}
