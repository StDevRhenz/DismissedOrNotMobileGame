using UnityEngine;
using TMPro;

public class KeyCollectionManager : MonoBehaviour
{
    [Header("Collection Settings")]
    public int totalKeys = 2;
    private int collectedKeys = 0;

    [Header("UI References")]
    public TextMeshProUGUI collectionText;

    [Header("Exit Door")]
    public GameObject exitDoor;
    private ExitDoor3 exitDoorScript;

    void Start()
    {
        UpdateUI();

        if (exitDoor != null)
        {
            exitDoorScript = exitDoor.GetComponent<ExitDoor3>();

            if (exitDoorScript == null)
                Debug.LogWarning("ExitDoor3 script not found on the assigned exit door object!");
        }
    }

    public void CollectKey()
    {
        collectedKeys++;
        UpdateUI();

        Debug.Log("Key collected! " + collectedKeys + "/" + totalKeys);

        if (collectedKeys >= totalKeys)
        {
            UnlockExit();
        }
    }

    void UpdateUI()
    {
        if (collectionText != null)
        {
            collectionText.text = string.Format("{0:D2} / {1:D2} Keys Collected",
                                                collectedKeys, totalKeys);
        }
    }

    void UnlockExit()
    {
        Debug.Log("🔓 All keys collected! Exit unlocked!");

        if (exitDoorScript != null)
        {
            exitDoorScript.UnlockDoor();
            Debug.Log("✅ Door unlocked successfully!");
        }
        else
        {
            Debug.LogError("❌ ExitDoor3 script missing!");
        }
    }

    public bool AreAllKeysCollected()
    {
        return collectedKeys >= totalKeys;
    }

    public int GetCollectedCount()
    {
        return collectedKeys;
    }
}
