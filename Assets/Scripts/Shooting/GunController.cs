using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

using Weapon.Gun;

public class GunController : MonoBehaviour {

    private GameObject bulletContainer; // Container GameObject to hide all bullets in heirarchy

    public Transform firePoint;

    public AbstractGun currentGun;

    public List<string> ignoreTags = new List<string>();

    private float gunTimer = 0.0f;

    public bool debugging = false;

    private void Awake ()
    {
        setGun(typeof(Handgun));

        bulletContainer = new GameObject();
        bulletContainer.name = "Active Bullets";
        bulletContainer.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGun.GetType() != typeof(Handgun))
        {
            gunTimer -= Time.deltaTime;

            if (gunTimer <= 0.0f)
                setGun(typeof(Handgun));
        }
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
    }

    /// <summary>
    /// Sets the current left-click gun
    /// </summary>
    /// <param name="gun">The type of gun to use.</param>
    /// <param name="duration">The amount of time this gun will be active for (0 for infinite).</param>
    public void setGun(System.Type gun, float duration = 0)
    {
        if (currentGun != null && currentGun.GetType() == gun)
        {
            gunTimer = duration;
            return;
        }

        if (currentGun)
            Destroy(currentGun);

        gameObject.AddComponent(gun);
        currentGun = gameObject.GetComponent(gun) as AbstractGun;

        gunTimer = duration;

        if (debugging)
            Debug.Log("Switching to " + currentGun.ToString());
    }

    private void OnDestroy()
    {
        Destroy(bulletContainer);
    }
}
