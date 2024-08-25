using UnityEngine;
using UnityEngine.UI;

public class ChargeBarUIManager : MonoBehaviour
{
    public Image[] chargeSlots; // Array of images for the charge slots

    // Method to update the charge bar UI
    public void UpdateChargeBar(int chargeCount)
    {
        // Loop through all slots
        for (int i = 0; i < chargeSlots.Length; i++)
        {
            // Activate or deactivate based on the current charge count
            chargeSlots[i].gameObject.SetActive(i < chargeCount);
        }
    }
}
