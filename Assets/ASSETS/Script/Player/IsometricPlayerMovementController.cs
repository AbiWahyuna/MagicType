using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 1f;
    IsometricCharacterRenderer isoRenderer;
    Rigidbody2D rbody;

    public bool canMove = true;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponent<IsometricCharacterRenderer>();
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rbody.velocity = Vector2.zero;
            // HAPUS pemanggilan SetDirection di sini supaya animcasting nggak ketimpa
            // isoRenderer.SetDirection(Vector2.zero);
            return;
        }

        Vector2 currentPos = rbody.position;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 inputVector = new Vector2(h, v);
        inputVector = Vector2.ClampMagnitude(inputVector, 1f);

        Vector2 movement = inputVector * moveSpeed;
        isoRenderer.SetDirection(movement);

        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        rbody.MovePosition(newPos);
    }

}
