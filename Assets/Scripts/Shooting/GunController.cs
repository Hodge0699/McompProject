using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

using Weapon.Gun;

public class GunController : MonoBehaviour {

    private GameObject bulletContainer; // Container GameObject to hide all bullets in heirarchy

    public Transform firePoint;

    public List<string> ignoreTags = new List<string>();

    public bool autoSwitchOnEmpty = true; // Automatically switch weapon when empty?

    public bool debugging = false;

    private AbstractGun currentGun;
    private List<AbstractGun> guns = new List<AbstractGun>();

    private void Awake ()
    {
        bulletContainer = new GameObject();
        bulletContainer.name = "Active Bullets";
        bulletContainer.transform.position = Vector3.zero;

        guns.Add(gameObject.AddComponent<Handgun>());
        guns.Add(gameObject.AddComponent<Shotgun>());
        guns.Add(gameObject.AddComponent<MachineGun>());
        guns.Add(gameObject.AddComponent<EXDHandgun>());
        guns.Add(gameObject.AddComponent<NonTimeEffectingGun>());

        setGun(typeof(Handgun));
    }

    /// <summary>
    /// Shoots the current gun
    /// </summary>
    public void shoot()
    {
        GameObject bullet = currentGun.shoot(firePoint.position);

        if (bullet == null)
            return;

        if (bullet.GetComponent<BulletController>())
            bullet.GetComponent<BulletController>().ignoreTags = ignoreTags;
        else
        {
            BulletController[] bulletControllers = bullet.GetComponentsInChildren<BulletController>();

            for (int i = 0; i < bulletControllers.Length; i++)
                bulletControllers[i].ignoreTags = ignoreTags;
        }

        bullet.transform.parent = bulletContainer.transform;

        if (currentGun.getCurrentAmmo() <= 0)
            setGun(typeof(Handgun));
    }

    /// <summary>
    /// Sets the current left-click gun.
    /// </summary>
    /// <param name="gun">The type of gun to use.</param>
    public void setGun(System.Type gun, bool infiniteAmmo = false)
    {
        int index = getIndex(gun);

        if (index == -1)
            return;
        else
            currentGun = guns[index];

        if (infiniteAmmo)
            currentGun.giveInfiniteAmmo();

        if (debugging)
            Debug.Log("Switching to " + currentGun.ToString());
    }

    /// <summary>
    /// Switches to the best weapon available with ammo
    /// </summary>
    public void switchToBest()
    {
        for (int i = guns.Count - 1; i > 0; i--)
        {
            if (guns[i].getCurrentAmmo() > 0)
            {
                setGun(guns[i].GetType());
                return;
            }
        }
    }

    /// <summary>
    /// Gets the current gun
    /// </summary>
    public AbstractGun getGun()
    {
        return currentGun;
    }

    /// <summary>
    /// Adds ammo to a certain gun
    /// </summary>
    /// <param name="gun">Gun to replenish.</param>
    /// <param name="ammo">Amount of ammo to add.</param>
    public void addAmmo(System.Type gun, int ammo)
    {
        int index = getIndex(gun);

        if (index == -1) // Gun not found
            return;
        else
            guns[index].addAmmo(ammo);
    }

    /// <summary>
    /// Gets the index of a type of gun in guns array.
    /// </summary>
    /// <param name="gun">Gun to search for.</param>
    /// <returns>Index of gun or -1 if not in array.</returns>
    private int getIndex(System.Type gun)
    {
        for (int i = 0; i < guns.Count; i++)
        {
            if (guns[i].GetType() == gun)
                return i;
        }

        return -1;
    }

    private void OnDestroy()
    {
        Destroy(bulletContainer);
    }
}
