using UnityEngine;

public class GameManagerConector : MonoBehaviour
{
    public TypingSystem typing;
    public SpellManager spellManager;

    void Start()
    {
        typing.OnSubmit += spellManager.TryCast;
    }
}
