using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] Button btn;
    [SerializeField] int thisIndex;
    private bool pointerOnBtn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuButtonController.index = thisIndex;
        pointerOnBtn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerOnBtn = false;
    }

    // Update is called once per frame
    void Update () {

        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);

            if (Input.GetAxis("Submit") == 1)
                animator.SetBool("pressed", true);
            else if (pointerOnBtn && (Input.GetAxis("Left Mouse") == 1))
                animator.SetBool("pressed", true);
            else if (animator.GetBool("pressed"))
                animator.SetBool("pressed", false);
        }
        else
        {
            animator.SetBool("selected", false);
        }


        if (Input.GetButtonDown("Cancel"))
        {
            if (btn != null)
                btn.onClick.Invoke();
        }

	}
}
