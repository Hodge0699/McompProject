using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorFunctions : MonoBehaviour {

    [SerializeField] MenuButtonController menuButtonController;
    public Button btn;

    void OnPressed()
    {
        btn.onClick.Invoke();
    }
}
