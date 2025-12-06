using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitDoor3 : MonoBehaviour
{
    [Header("Door Settings")]
    public bool isLocked = true;
    public Material lockedMaterial;
    public Material unlockedMaterial;
    public Light doorLight;

    [Header("UI References")]
    public Image fadeImage;
    public GameObject completionPanel;

    [Header("Fade Settings")]
    public float fadeAndWaitDuration = 2f; // Mas mabilis na fade

    private Renderer doorRenderer;
    private bool isTransitioning = false;

    void Start()
    {
        doorRenderer = GetComponent<Renderer>();

        if (isLocked) SetLockedAppearance();
        else SetUnlockedAppearance();

        if (completionPanel != null)
            completionPanel.SetActive(false);
    }

    void Update()
    {
        if (!isLocked && doorLight != null)
        {
            float pulse = Mathf.PingPong(Time.time * 2f, 1f);
            doorLight.intensity = 3f + pulse * 2f;
        }
    }

    public void UnlockDoor()
    {
        if (isLocked)
        {
            isLocked = false;
            SetUnlockedAppearance();

            if (DialogueManager.Instance != null)
                DialogueManager.Instance.ShowDialogue("Finally! Get me out of this place!");
        }
    }

    void SetLockedAppearance()
    {
        if (doorRenderer != null && lockedMaterial != null)
            doorRenderer.material = lockedMaterial;

        if (doorLight != null)
        {
            doorLight.color = Color.red;
            doorLight.intensity = 1f;
        }
    }

    void SetUnlockedAppearance()
    {
        if (doorRenderer != null && unlockedMaterial != null)
            doorRenderer.material = unlockedMaterial;

        if (doorLight != null)
        {
            doorLight.color = Color.green;
            doorLight.intensity = 5f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning && !isLocked)
        {
            StartCoroutine(ExitToMainMenu(other.gameObject));
        }
    }

    IEnumerator ExitToMainMenu(GameObject player)
    {
        isTransitioning = true;

        // Disable player movement
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.enabled = false;

        // Show completion panel
        if (completionPanel != null)
            completionPanel.SetActive(true);

        // Fade to white
        yield return StartCoroutine(FadeToWhite());

        // Load Main Menu
        SceneManager.LoadScene("Main_Menu");
    }

    IEnumerator FadeToWhite()
    {
        if (fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(1f, 1f, 1f, 0f);

        float elapsed = 0f;
        while (elapsed < fadeAndWaitDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeAndWaitDuration);
            fadeImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
    }
}
