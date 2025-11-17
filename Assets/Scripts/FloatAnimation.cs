using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotateSpeed = 50f;

    [Header("Floating")]
    [SerializeField] private float floatAmplitude = 0.1f;
    [SerializeField] private float floarFrequency = 1f;

    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        float tempYOffset = Mathf.Sin(Time.time * floarFrequency) * floatAmplitude;

        transform.position = new Vector3(startPosition.x, startPosition.y + tempYOffset, startPosition.z);
    }
}
