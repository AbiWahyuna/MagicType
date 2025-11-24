using UnityEngine;
using TMPro;

public class TypingSystem : MonoBehaviour
{
    public TMP_InputField inputField;
    public bool typingMode;

    public System.Action<string> OnSubmit;

    public IsometricPlayerMovementController moveController; // drag via inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // masuk mode mengetik
            if (!typingMode)
            {
                typingMode = true;
                inputField.text = "";
                inputField.gameObject.SetActive(true);
                inputField.ActivateInputField();

                moveController.canMove = false; // ⛔ STOP GERAK
            }
            else
            {
                // selesai ngetik (benar/salah nanti dicek di SpellManager)
                typingMode = false;
                string typed = inputField.text;
                inputField.gameObject.SetActive(false);

                moveController.canMove = true; // ✔️ BOLEH GERAK LAGI

                OnSubmit?.Invoke(typed);
            }
        }
    }

}
