using System.Collections;
using UnityEngine;

public class CutsceneMoveTrigger : MonoBehaviour
{
    [Header("Target")]
    public Transform moveTarget;

    [Header("Object To Activate")]
    public GameObject objectToEnable; // 🔥 INI YANG AWALNYA FALSE
    public GameObject objectToEnable2; // 🔥 INI YANG AWALNYA FALSE

    [Header("Movement")]
    public float autoMoveSpeed = 1f;

    private bool triggered = false;
    void Start()
    {
        if (objectToEnable2 != null)
            objectToEnable2.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(Cutscene(other.gameObject));
        }
    }

    IEnumerator Cutscene(GameObject player)
    {
        var moveCtrl = player.GetComponent<IsometricPlayerMovementController>();
        var isoRenderer = player.GetComponent<IsometricCharacterRenderer>();
        var rb = player.GetComponent<Rigidbody2D>();

        // 1️⃣ Matikan kontrol player
        moveCtrl.canMove = false;
        rb.velocity = Vector2.zero;

        // 2️⃣ AKTIFKAN GAMEOBJECT
        if (objectToEnable != null)
            objectToEnable.SetActive(true);

        if (objectToEnable2 != null)
            objectToEnable2.SetActive(false);



        yield return null;

        // 3️⃣ Paksa animasi Move_UpRight
        Vector2 forcedDir = new Vector2(1, 1);

        // 4️⃣ Auto jalan
        while (Vector2.Distance(player.transform.position, moveTarget.position) > 0.05f)
        {
            isoRenderer.SetDirection(forcedDir);

            player.transform.position = Vector2.MoveTowards(
                player.transform.position,
                moveTarget.position,
                autoMoveSpeed * Time.deltaTime
            );

            yield return null;
        }

        // 5️⃣ Idle
        isoRenderer.SetDirection(Vector2.zero);

        // 6️⃣ Balikin kontrol
        moveCtrl.canMove = true;
    }
}
