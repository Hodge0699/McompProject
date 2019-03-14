using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUISwitch : MonoBehaviour {

    [SerializeField] private Image handgun;
    [SerializeField] private Image shotgun;
    [SerializeField] private Image machinegun;
    //[SerializeField] private Image EXDhandgun;
    //[SerializeField] private Image nonTimeEffectingGun;

    List<Image> weaponUI = new List<Image>();
    int currentWeapon;

    private void initWeaponUI()
    {
        weaponUI.Add(handgun);
        weaponUI.Add(shotgun);
        weaponUI.Add(machinegun);
        //weaponUI.Add(EXDhandgun);
        //weaponUI.Add(nonTimeEffectingGun);
    }

    // Use this for initialization
    void Start () {
        weaponUI.Add(handgun);
        weaponUI.Add(shotgun);
        weaponUI.Add(machinegun);
        //weaponUI.Add(EXDhandgun);
        //weaponUI.Add(nonTimeEffectingGun);

        // Show starting pistol - Jake
        switchWeaponUI(0);
    }

    // Update is called once per frame
    void Update () {
		
	}


    public void switchWeaponUI(int i)
    {
        weaponUI[currentWeapon].enabled = false;
        weaponUI[i].enabled = true;

        currentWeapon = i;
    }
}
