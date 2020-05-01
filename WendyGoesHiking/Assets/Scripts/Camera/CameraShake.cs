using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get => instance; }
    private static CameraShake instance;
    [SerializeField]
    [Tooltip("Used to smooth shake - higher number is faster")]
    private float lerpSpeed = 2f;
    [Tooltip("The duration of the camera shake")]
    public float duration = 0.5f;
    [Tooltip("The magnitude of the camera shake - higher is more shake")]
    public float magnitude = 1f;
    [Button(ButtonSizes.Small)]
    private void TestCameraShake()
    {
        StartShake();
    }

    // CameraShake Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("A CameraShake already exists in the scene!");
            Destroy(this.gameObject);
        }
        else
            instance = this;
    }

    public void StartShake()
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake(float _duration, float _magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0f;

        while(elapsed < _duration)
        {
            float x = Random.Range(-1f, 1) * _magnitude;
            float y = Random.Range(-1f, 1f) * _magnitude;

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(x, y, originalPosition.z), Time.deltaTime * lerpSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
