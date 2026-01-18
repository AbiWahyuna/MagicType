using UnityEngine;
using TMPro;

public class TypingSystem : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;

    [Header("References")]
    public IsometricCharacterRenderer renderer;
    public IsometricPlayerMovementController moveController;

    [Header("Magic Circles")]
    public MagicCircle fireCircle;
    public MagicCircle iceCircle;

    public bool typingMode = false;

    // 🔑 mode terakhir yang VALID
    private bool iceMode = false;

    public System.Action<string> OnSubmit;

    void Awake()
    {
        inputField.onValueChanged.AddListener(OnTypingChanged);
    }

    void OnDestroy()
    {
        inputField.onValueChanged.RemoveListener(OnTypingChanged);
    }

    void Update()
    {
        if (!typingMode && Input.GetKeyDown(KeyCode.Return))
        {
            StartTyping();
        }
        else if (typingMode && Input.GetKeyDown(KeyCode.Return))
        {
            SubmitTyping();
        }
    }

    void StartTyping()
    {
        typingMode = true;

        inputField.text = "";
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();

        moveController.canMove = false;
        renderer.PlayCasting();

        // 🔥❄️ AKTIFKAN MODE TERAKHIR
        if (iceMode)
        {
            if (fireCircle.gameObject.activeInHierarchy)
                fireCircle.Hide();

            iceCircle.gameObject.SetActive(true);
            iceCircle.Show();
        }
        else
        {
            if (iceCircle.gameObject.activeInHierarchy)
                iceCircle.Hide();

            fireCircle.gameObject.SetActive(true);
            fireCircle.Show();
        }
    }

    // REAL-TIME SWITCH
    void OnTypingChanged(string value)
    {
        if (!typingMode) return;

        string typed = value.ToLower().Trim();

        if (typed == "glaciafall")
        {
            if (!iceMode)
            {
                iceMode = true;

                if (fireCircle.gameObject.activeInHierarchy)
                    fireCircle.Hide();

                iceCircle.gameObject.SetActive(true);
                iceCircle.Show();
            }
        }
        else if (typed == "fireball")
        {
            if (iceMode)
            {
                iceMode = false;

                if (iceCircle.gameObject.activeInHierarchy)
                    iceCircle.Hide();

                fireCircle.gameObject.SetActive(true);
                fireCircle.Show();
            }
        }
    }

    void SubmitTyping()
    {
        typingMode = false;

        string typed = inputField.text.ToLower().Trim();
        inputField.gameObject.SetActive(false);

        // 🔑 KUNCI MODE TERAKHIR DARI INPUT
        if (typed == "glaciafall")
            iceMode = true;
        else if (typed == "fireball")
            iceMode = false;

        if (fireCircle.gameObject.activeInHierarchy)
            fireCircle.Hide();

        if (iceCircle.gameObject.activeInHierarchy)
            iceCircle.Hide();

        moveController.canMove = true;
        renderer.PlayIdleDown();

        OnSubmit?.Invoke(typed);
    }
}
