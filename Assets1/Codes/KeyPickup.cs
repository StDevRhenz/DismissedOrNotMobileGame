using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public KeyCollectionManager manager;
    public GameObject pressEUI;

    private bool isPlayerInside = false;

    void Start()
    {
        if (pressEUI != null)
            pressEUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (pressEUI != null)
                pressEUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            if (pressEUI != null)
                pressEUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            manager.CollectKey();
            Destroy(gameObject);

            if (pressEUI != null)
                pressEUI.SetActive(false);
        }
    }
}
