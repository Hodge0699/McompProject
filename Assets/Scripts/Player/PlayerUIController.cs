using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject pUI;
    [SerializeField]
    private Image angryFace;
    [SerializeField]
    private Image happyFace;

    // Use this for initialization
    void Start()
    {
        pUI = GameObject.Find("PlayerUI(Clone)");
        angryFace = pUI.transform.Find("Elite").transform.Find("ImageWrap").transform.Find("AngryFace").GetComponent<Image>();
        happyFace = pUI.transform.Find("Elite").transform.Find("ImageWrap").transform.Find("HappyFace").GetComponent<Image>();
    }

    private void Update()
    {
        //if(angryFace = null)
        //    angryFace = pUI.transform.Find("Elite").transform.Find("ImageWrap").transform.Find("AngryFace").GetComponent<Image>();
        //if(happyFace = null)
        //    happyFace = pUI.transform.Find("Elite").transform.Find("ImageWrap").transform.Find("HappyFace").GetComponent<Image>();
    }

    public void changeToHappy()
    {
        if (angryFace != null && happyFace != null)
        {
            angryFace.enabled = false;
            happyFace.enabled = true;
        }
    }
    public void changeToSad()
    {
        happyFace.enabled = false;
        angryFace.enabled = true;
    }
}
