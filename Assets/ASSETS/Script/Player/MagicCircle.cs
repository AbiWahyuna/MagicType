using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class MagicCircle : MonoBehaviour
{
    public ParticleSystem[] particles;
    public Transform rotatePivot;
    public GameObject lightRoot;
    public Animator lightAnimator;

    // 🔥 GameObject khusus light
    public Light2D circleLight;

    public float rotateSpeed = 30f;
    public float scaleDuration = 0.3f;

    private float baseLightRadius;

    private void Awake()
    {
        if (particles == null || particles.Length == 0)
            particles = GetComponentsInChildren<ParticleSystem>(true);

        transform.localScale = Vector3.zero;

        if (circleLight != null)
            baseLightRadius = circleLight.pointLightOuterRadius;
    }

    void Update()
    {
        if (rotatePivot != null)
            rotatePivot.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (lightRoot != null)
        {
            lightRoot.SetActive(true);
            lightAnimator.Play("FadeLight");
        }

        StopAllCoroutines();
        StartCoroutine(ScaleRoutine(0f, 1f));

        foreach (var ps in particles)
            ps.Play();
    }


    public void Hide()
    {
        if (lightAnimator != null)
            lightAnimator.Play("Light_Close");
            lightAnimator.Play("SmallLightClose");

        StopAllCoroutines();
        StartCoroutine(ScaleRoutine(1f, 0f));

        foreach (var ps in particles)
            ps.Stop();
    }


    private IEnumerator ScaleRoutine(float from, float to)
    {
        float t = 0;

        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one * from, Vector3.one * to, t / scaleDuration);

            if (circleLight != null)
                circleLight.pointLightOuterRadius =
                    Mathf.Lerp(from, to, t / scaleDuration) * baseLightRadius;

            yield return null;
        }

        transform.localScale = Vector3.one * to;

        if (to == 0f)
        {
            if (lightRoot != null)
                lightRoot.SetActive(false);   // 💀 mati total

            gameObject.SetActive(false);
        }
    }
}
