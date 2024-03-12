using UnityEngine;

public class HowToPopupController : MonoBehaviour
{
    public GameObject howToPopup;

    // Function to be called to toggle the visibility of the how-to popup
    public void TogglePopup()
    {
        // Toggle the active state of the popup
        howToPopup.SetActive(!howToPopup.activeSelf);
    }
}
