using UnityEngine;

public class SignInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactIcon;
    public GameObject signPanel;

    private bool playerInRange;
    private bool panelOpen;

    void Start()
    {
        interactIcon.SetActive(false);
        signPanel.SetActive(false);
    }

    void Update()
    {
        // INPUT hanya saat panel terbuka
        if (panelOpen && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            ClosePanel();
            return;
        }

        // Buka panel hanya kalau player di range
        if (playerInRange && !panelOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenPanel();
        }
    }

    void OpenPanel()
    {
        signPanel.SetActive(true);
        interactIcon.SetActive(false);
        panelOpen = true;
    }

    void ClosePanel()
    {
        signPanel.SetActive(false);
        panelOpen = false;

        // icon hanya muncul kalau player masih di range
        interactIcon.SetActive(playerInRange);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (!panelOpen)
            interactIcon.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        interactIcon.SetActive(false);
        signPanel.SetActive(false);
        panelOpen = false;
    }
}
