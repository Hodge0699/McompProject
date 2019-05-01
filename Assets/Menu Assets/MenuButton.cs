using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{

    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuButtonController.index = thisIndex;
    }

    // Update is called once per frame
    void Update () {

        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);

            if (Input.GetAxis("Submit") == 1)
                animator.SetBool("pressed", true);
            else if (animator.GetBool("pressed"))
                animator.SetBool("pressed", false);
        }
        else
        {
            animator.SetBool("selected", false);
        }

	}
}
