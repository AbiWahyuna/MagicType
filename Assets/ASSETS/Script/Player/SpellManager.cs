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

    [Header("Visual")]
    public SpriteRenderer playerSprite;
    public Color stunColor = new Color(1f, 0.66f, 0f); // FFA900
    public bool IsStunned { get; private set; }



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
        GameObject go = Instantiate(spell.prefab, castPoint.position, Quaternion.identity);

        //Fireball fb = go.GetComponent<Fireball>();
        //Vector3 dir = playerAnimator.transform.right;
        //fb.Init(dir);


        isCasting = true;
        spellCooldownTimer = spell.cooldown;

        moveController.canMove = true;
    }



    public void Stun(float duration)
    {
        if (IsStunned) return;

        IsStunned = true;
        moveController.canMove = false;
        StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        Color originalColor = playerSprite.color;
        playerSprite.color = stunColor;

        yield return new WaitForSeconds(duration);

        playerSprite.color = originalColor;
        IsStunned = false;
        moveController.canMove = true;
    }


}
