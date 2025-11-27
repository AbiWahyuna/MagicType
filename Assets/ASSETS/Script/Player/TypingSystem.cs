using UnityEngine;
using TMPro;

public class TypingSystem : MonoBehaviour
{
    public TMP_InputField inputField;

    public IsometricCharacterRenderer renderer; // drag di inspector

    public bool typingMode;

    public System.Action<string> OnSubmit;

    public IsometricPlayerMovementController moveController; // drag via inspector

    public MagicCircle magicCircle; // drag prefab/instance ke sini
    



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // MASUK MODE KETIK
            if (!typingMode)
            {
                typingMode = true;
                inputField.text = "";
                inputField.gameObject.SetActive(true);
                inputField.ActivateInputField();

                moveController.canMove = false;
                renderer.PlayCasting();  // 🔥 MULAI ANIM CASTING

                // magicCircle = reference ke MagicCircle di scene
                
                magicCircle.gameObject.SetActive(true);
                magicCircle.Show();

            }
            else
            {
                // KELUAR MODE KETIK + SUBMIT
                typingMode = false;
                string typed = inputField.text;
                inputField.gameObject.SetActive(false);

                magicCircle.Hide();
                


                moveController.canMove = true;
                renderer.PlayIdleDown();  // ➡️ BALIK KE IDLE_DOWN

                OnSubmit?.Invoke(typed);
            }
        }
    }


}
