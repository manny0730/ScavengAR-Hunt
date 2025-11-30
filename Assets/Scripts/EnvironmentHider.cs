using UnityEngine;
using System.Collections;

public class EnvironmentHider : MonoBehaviour
{
    [SerializeField] private GameObject objectToHide;

    [SerializeField] private float delayInSeconds = 3f;
    
    public void StartHideProcess()
    {
        if (objectToHide != null)
        {
            StartCoroutine(HideRoutine());
        }
    }

    private IEnumerator HideRoutine()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(delayInSeconds);

        // Hide the object
        objectToHide.SetActive(false);
    }
}
