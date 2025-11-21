using UnityEngine;

public class ChecklistToggler : MonoBehaviour
{
    
    private Animator animator;

    private bool isVisible = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsVisible", isVisible);
    }

    public void ToggleChecklist()
    {
        isVisible = !isVisible;
        animator.SetBool("IsVisible", isVisible);
    }
}
