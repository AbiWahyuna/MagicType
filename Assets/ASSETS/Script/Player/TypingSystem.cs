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
    public MagicCircle comboCircle; // UNGU



    public bool typingMode = false;

    private bool iceMode = false;

    // COMBO
    private bool comboMode = false;
    private bool firstWasIce = false;

    public System.Action<string> OnSubmit;

    [Header("Combo Skill")]
    public GameObject comboSkillPrefab;
    public Transform castPoint;

    [Header("References")]
    public SpellManager spellManager;





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
            if (spellManager != null && spellManager.IsStunned)
                return;

            StartTyping();
        }

        else if (typingMode && Input.GetKeyDown(KeyCode.Return))
        {
            SubmitTyping();
        }

        if (typingMode && Input.GetKeyDown(KeyCode.Tab))
        {
            StartCombo();
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

        ShowSingleCircle();
    }

    void OnTypingChanged(string value)
    {
        if (!typingMode) return;

        string typed = value.ToLower().Trim();

        // ===== SINGLE MODE =====
        if (!comboMode)
        {
            if (typed == "fireball")
            {
                iceMode = false;
                ShowFire();
            }
            else if (typed == "glaciafall")
            {
                iceMode = true;
                ShowIce();
            }

            return;
        }

        // ===== COMBO MODE =====
        // hanya jika spell ke-2 adalah KEBALIKAN
        if (typed == "fireball" && firstWasIce)
        {
            ShowCombo();
        }
        else if (typed == "glaciafall" && !firstWasIce)
        {
            ShowCombo();
        }
    }

    void StartCombo()
    {
        string typed = inputField.text.ToLower().Trim();

        if (typed != "fireball" && typed != "glaciafall")
            return;

        // cast spell pertama
        OnSubmit?.Invoke(typed);

        comboMode = true;
        firstWasIce = (typed == "glaciafall");

        inputField.text = "";
        inputField.ActivateInputField();   // penting
        moveController.canMove = false;
    }



    void SubmitTyping()
    {
        string typed = inputField.text.ToLower().Trim();

        typingMode = false;
        inputField.gameObject.SetActive(false);

        // ❌ cancel total (ESC logic)
        if (string.IsNullOrEmpty(typed))
        {
            comboMode = false;
            inputField.text = "";
            renderer.PlayIdleDown();
            HideAll();
            moveController.canMove = true;
            return;
        }

        // 🔮 COMBO
        if (comboMode)
        {
            // ❌ spell ke-2 SALAH → biar SpellManager yang stun
            if (!IsValidComboSecondSpell(typed))
            {
                OnSubmit?.Invoke(typed); // ini bakal masuk TryCast → STUN
                ResetAll();
                return;
            }

            // ✅ spell ke-2 BENAR → cast combo
            CastCombo();
            ResetAll();
            moveController.canMove = true;
            return;
        }


        // 🔥 NORMAL / TYPO → biar SpellManager yang tentukan
        OnSubmit?.Invoke(typed);

        ResetAll();
        // ❗ JANGAN set canMove di sini
    }




    void CastCombo()
    {
        HideAll();

        if (comboSkillPrefab != null && castPoint != null)
        {
            Instantiate(
                comboSkillPrefab,
                castPoint.position,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogWarning("Combo prefab / castPoint belum di-set!");
        }
    }

    bool IsValidComboSecondSpell(string typed)
    {
        if (firstWasIce && typed == "fireball")
            return true;

        if (!firstWasIce && typed == "glaciafall")
            return true;

        return false;
    }



    // ===== VISUAL HELPERS =====

    void ShowSingleCircle()
    {
        if (iceMode)
            ShowIce();
        else
            ShowFire();
    }

    void ShowFire()
    {
        HideAll();
        fireCircle.gameObject.SetActive(true);
        fireCircle.Show();
    }

    void ShowIce()
    {
        HideAll();
        iceCircle.gameObject.SetActive(true);
        iceCircle.Show();
    }

    void ShowCombo()
    {
        HideAll();
        comboCircle.gameObject.SetActive(true);
        comboCircle.Show();
    }

    void HideAll()
    {
        if (fireCircle.gameObject.activeInHierarchy)
            fireCircle.Hide();

        if (iceCircle.gameObject.activeInHierarchy)
            iceCircle.Hide();

        if (comboCircle.gameObject.activeInHierarchy)
            comboCircle.Hide();
    }

    void ResetAll()
    {
        comboMode = false;
        HideAll();

        
        renderer.PlayIdleDown();
    }
}
