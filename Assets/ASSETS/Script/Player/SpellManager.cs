using System.Collections;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [Header("Spells")]
    public Spell[] spells;

    [Header("References")]
    public Transform castPoint;
    public IsometricPlayerMovementController moveController;
    public Animator playerAnimator;

    private bool isCasting = false;
    private float spellCooldownTimer = 0f;

    void Update()
    {
        if (isCasting)
        {
            spellCooldownTimer -= Time.deltaTime;
            if (spellCooldownTimer <= 0)
                isCasting = false;
        }
    }

    public void TryCast(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Debug.Log("Input kosong!");
            return;
        }

        if (isCasting)
        {
            Debug.Log("Cooldown, sabar...");
            return;
        }

        input = input.ToLower().Trim();

        foreach (Spell s in spells)
        {
            if (input == s.keyword.ToLower().Trim())
            {
                Cast(s);
                return;
            }
        }

        Debug.Log("Typo detected → STUN!");
        Stun(1f);
    }

    void Cast(Spell spell)
    {
        Debug.Log($"Cast spell: {spell.spellName}");

        if (playerAnimator != null)
            playerAnimator.SetTrigger("Cast");

        if (castPoint == null)
        {
            Debug.LogError("⚠️ castPoint belum diassign!");
            return;
        }

        if (spell.prefab == null)
        {
            Debug.LogError($"⚠️ Prefab untuk spell `{spell.spellName}` belum dimasukkan!");
            return;
        }

        Instantiate(spell.prefab, castPoint.position, Quaternion.identity);

        isCasting = true;
        spellCooldownTimer = spell.cooldown;
    }

    public void Stun(float duration)
    {
        moveController.canMove = false;
        StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        moveController.canMove = true;
    }
}
