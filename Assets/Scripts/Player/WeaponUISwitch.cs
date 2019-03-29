using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Weapon.Gun;

public class WeaponUISwitch : MonoBehaviour {

    [SerializeField] private Image handgun;
    [SerializeField] private Image shotgun;
    [SerializeField] private Image machinegun;
    [SerializeField] private Image EXDhandgun;
    //[SerializeField] private Image nonTimeEffectingGun;

    [SerializeField] private Transform AmmoUI;

    private int currentAmmo = 125;

    List<Image> weaponUI = new List<Image>();
    int currentWeapon;

    public Handgun handgunClass;
    public Shotgun shotgunClass;
    public MachineGun machinegunClass;
    public EXDHandgun EXDHandgunClass;

    private void initWeaponUI()
    {
        weaponUI.Add(handgun);
        weaponUI.Add(shotgun);
        weaponUI.Add(machinegun);
        weaponUI.Add(EXDhandgun);
        //weaponUI.Add(nonTimeEffectingGun);
    }

    //private void Awake()
    //{
    //    handgunClass = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPrimary").GetComponent<Handgun>();
    //    shotgunClass = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPrimary").GetComponent<Shotgun>();
    //    machinegunClass = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPrimary").GetComponent<MachineGun>();
    //    EXDHandgunClass = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPrimary").GetComponent<EXDHandgun>();
    //
    //}

    // Use this for initialization
    void Start () {
        weaponUI.Add(handgun);
        weaponUI.Add(shotgun);
        weaponUI.Add(machinegun);
        weaponUI.Add(EXDhandgun);
        //weaponUI.Add(nonTimeEffectingGun);

        // Show starting pistol - Jake
        switchWeaponUI(0);
    }

    // Update is called once per frame
    void Update () {

        if (handgunClass == null)
            handgunClass = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPrimary").GetComponent<Handgun>();
        
        if (shotgunClass == null)
            shotgunClass = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPrimary").GetComponent<Shotgun>();
        
        if (machinegunClass == null)
            machinegunClass = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPrimary").GetComponent<MachineGun>();
        
        if (EXDHandgunClass == null)
            EXDHandgunClass = GameObject.FindGameObjectWithTag("Player").transform.Find("GunPrimary").GetComponent<EXDHandgun>();

        if (weaponUI[0].isActiveAndEnabled)
            currentAmmo = handgunClass.getCurrentAmmo();
        if (weaponUI[1].isActiveAndEnabled)
            currentAmmo = shotgunClass.getCurrentAmmo();
        if (weaponUI[2].isActiveAndEnabled)
            currentAmmo = machinegunClass.getCurrentAmmo();
        if (weaponUI[3].isActiveAndEnabled)
            currentAmmo = EXDHandgunClass.getCurrentAmmo();

        updateAmmo(currentAmmo);

    }

    public void updateAmmo(int ammo)
    {
        AmmoUI.GetComponent<Text>().text = ammo.ToString();
    }


    public void switchWeaponUI(int i)
    {
        weaponUI[currentWeapon].enabled = false;
        weaponUI[i].enabled = true;

        currentWeapon = i;
    }
}
