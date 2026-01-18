using UnityEngine;
using UnityEngine.UI;

public class IsometricPlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 1f;

    [Header("Dash")]
    public float dashSpeed = 5f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 2f;
    public Slider dashCooldownSlider;
    public AudioSource audioSource;
    public AudioClip dashSfx;

    [Header("Dash Trail")]
    public TrailRenderer dashTrail;


    IsometricCharacterRenderer isoRenderer;
    Rigidbody2D rbody;

    public bool canMove = true;

    private bool isDashing = false;
    private float lastDashTime = -999f;
    private Vector2 lastMoveDir = Vector2.down;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponent<IsometricCharacterRenderer>();

        if (dashTrail != null)
            dashTrail.emitting = false;

        if (dashCooldownSlider != null)
            dashCooldownSlider.value = 1f;
    }   


    void Update()
    {
        // Update slider cooldown
        if (dashCooldownSlider != null)
        {
            float t = Mathf.Clamp01((Time.time - lastDashTime) / dashCooldown);
            dashCooldownSlider.value = t;
        }

        // Dash pakai Shift
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isDashing && Time.time >= lastDashTime + dashCooldown)
            {
                StartCoroutine(Dash());
            }
        }
    }

    void FixedUpdate()
    {
        if (!canMove || isDashing)
        {
            rbody.velocity = Vector2.zero;
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 inputVector = new Vector2(h, v);
        inputVector = Vector2.ClampMagnitude(inputVector, 1f);

        if (inputVector != Vector2.zero)
            lastMoveDir = inputVector;

        Vector2 movement = inputVector * moveSpeed;
        isoRenderer.SetDirection(movement);

        rbody.MovePosition(rbody.position + movement * Time.fixedDeltaTime);
    }

    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        // 🔊 PLAY DASH SFX
        if (audioSource != null && dashSfx != null)
            audioSource.PlayOneShot(dashSfx);

        float timer = 0f;

        isoRenderer.SetDirection(lastMoveDir);

        if (dashTrail != null)
        {
            dashTrail.Clear();
            dashTrail.emitting = true;
        }

        while (timer < dashDuration)
        {
            rbody.MovePosition(
                rbody.position + lastMoveDir.normalized * dashSpeed * Time.fixedDeltaTime
            );

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (dashTrail != null)
            dashTrail.emitting = false;

        isDashing = false;
    }

}
