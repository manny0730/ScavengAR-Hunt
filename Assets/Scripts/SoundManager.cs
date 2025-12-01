using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Components")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip scanSuccessSound;
    [SerializeField] private AudioClip keyCardPickupSound;
    [SerializeField] private AudioClip uiClickSound;

    public static SoundManager Instance;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayScanSuccess()
    {
        PlayClip(scanSuccessSound);
    }

    public void PlayKeyPickup()
    {
        PlayClip(keyCardPickupSound);
    }
    public void PlayUIClick()
    {
        PlayClip(uiClickSound);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {            
            audioSource.PlayOneShot(clip);
        }
    }
}
