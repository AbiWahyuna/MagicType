using UnityEngine;
using System.Collections;

public class MagicCircle : MonoBehaviour
{
    public ParticleSystem[] particles;
    public Transform particleRoot;
    public float rotateSpeed = 30f;
    public float scaleDuration = 0.3f;
    public Transform rotatePivot; // ini RotatePivot
    private void Awake()
    {
        if (particles == null || particles.Length == 0)
            particles = GetComponentsInChildren<ParticleSystem>(true);

        transform.localScale = Vector3.zero;
    }

    

    void Update()
    {
        if (rotatePivot != null)
            rotatePivot.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
    }






    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleRoutine(0f, 1f));

        foreach (var ps in particles) ps.Play();
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleRoutine(1f, 0f));

        foreach (var ps in particles) ps.Stop();
    }

    private IEnumerator ScaleRoutine(float from, float to)
    {
        float t = 0;
        Vector3 start = Vector3.one * from;
        Vector3 end = Vector3.one * to;

        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, end, t / scaleDuration);
            yield return null;
        }

        transform.localScale = end;

        if (to == 0f)
            gameObject.SetActive(false);
    }
}
