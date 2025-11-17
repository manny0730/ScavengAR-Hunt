using UnityEngine;

public class TriggerVolumeHelper : MonoBehaviour
{
    [Header("ScavengAR Manager Reference")]
    [SerializeField] private ScavengARManager manager;

    private Collider selfCollider;
    void Start()
    {
        selfCollider = GetComponent<Collider>();     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.TriggerWasEntered(selfCollider);
        }
    }
}
