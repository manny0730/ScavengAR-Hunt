using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ObjectHuntManager : MonoBehaviour
{
    
    [Header("Game Settings")]
    [Tooltip("Time limit in seconds")]
    [SerializeField] private float timeLimit = 300f;

    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private List<TMP_Text> clueChecklistTexts;
    [SerializeField] private Color foundColor = Color.green;

    [Header("Events")]
    public UnityEvent<bool> onPhaseEnded;

    //Private variables
    private float timeRemaining;
    private bool isTimeRunning = false;

    //Tracking each object individually
    private bool[] objectsFoundStatus;
    private int totalObjectsFoundCount = 0;
    private const int TOTAL_OBJECTS_TO_FIND = 5;


    void Start()
    {
        timeRemaining = timeLimit;
        objectsFoundStatus = new bool[TOTAL_OBJECTS_TO_FIND];
        isTimeRunning = true;

        UpdateTimerUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                isTimeRunning = false;
                UpdateTimerUI();

                EndPhase(false);
                
            }
        }
    }
    
    public void MarkObjectAsFound(int objectIndex)
    {
        if (objectIndex < 0 || objectIndex >= TOTAL_OBJECTS_TO_FIND)
        {
            return;
        }

        if (objectsFoundStatus[objectIndex])
        {
            return;
        }

        objectsFoundStatus[objectIndex] = true;
        totalObjectsFoundCount++;

        //Update UI Checklist
        if(objectIndex < clueChecklistTexts.Count && clueChecklistTexts[objectIndex] != null)
        {
            clueChecklistTexts[objectIndex].fontStyle = FontStyles.Strikethrough;
            clueChecklistTexts[objectIndex].color = foundColor;
        }

        //Check win condition
        if(totalObjectsFoundCount == TOTAL_OBJECTS_TO_FIND)
        {
            isTimeRunning = false;
            EndPhase(true);
        }
    }

    private void UpdateTimerUI()
    {
        if(timerText != null)
        {
            //Timer formatted as MM:SS
            int minutes = Mathf.FloorToInt(timeRemaining / 60F);
            int seconds = Mathf.FloorToInt(timeRemaining % 60F);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void EndPhase(bool playerWon)
    {
        onPhaseEnded?.Invoke(playerWon);
    }
}
