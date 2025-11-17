using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScavengARManager : MonoBehaviour
{
    [Header("Trigger & Virtual Object References")]
    [SerializeField] private List<Collider> triggerVolumes;
    [SerializeField] private List<GameObject> hiddenObjects;

    [Header("Events")]
    [SerializeField] private UnityEvent OnAllObjectsFound;

    //Private Variables
    private HashSet<Collider> foundTriggers = new HashSet<Collider>();
    void Start()
    {
        //Hide all virtual objects in the list
        foreach (GameObject obj in hiddenObjects)
        {
            obj.SetActive(false);
        }

        //Make sure all trigger volumes are set to true
        foreach (Collider trigger in triggerVolumes)
        {
            if (!trigger.isTrigger)
            {
                trigger.isTrigger = true;
            }
        }
    }

    public void TriggerWasEntered(Collider trigger)
    {
        //Check if this trigger has been found
        if (foundTriggers.Contains(trigger))
        {
            return;
        }

        foundTriggers.Add(trigger);

        int index = triggerVolumes.IndexOf(trigger);
        if (index != -1 && index < hiddenObjects.Count)
        {
            hiddenObjects[index].SetActive(true);
        }

        if(foundTriggers.Count == triggerVolumes.Count)
        {
            AllObjectsHaveBeenFound();
        }
    }
    private void AllObjectsHaveBeenFound()
    {
        if(OnAllObjectsFound != null)
        {
            OnAllObjectsFound.Invoke();
        }
    }
}
