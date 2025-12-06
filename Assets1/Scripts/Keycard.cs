using UnityEngine;

public class Keycard : MonoBehaviour
{
    [Header("Visual Settings")]
    public float rotationSpeed = 100f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.3f;

    private Vector3 startPosition;

    [Header("Audio (Optional)")]
    public AudioClip collectSound;

    [Header("First Keycard Dialogue")]
    private static bool firstKeycardCollected = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<CharacterController>() != null)
        {
            if (!firstKeycardCollected)
            {
                firstKeycardCollected = true;
                if (DialogueManager.Instance != null)
                {
                    string[] dialogue = new string[]
                    {
                        "What the...",
                        "Collect keycards... does that mean I have to collect all these to get out?"
                    };
                    DialogueManager.Instance.ShowMultipleLines(dialogue, 1.5f);
                }
            }

            CollectionManager manager = FindObjectOfType<CollectionManager>();
            if (manager != null)
            {
                manager.CollectKeycard();
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}