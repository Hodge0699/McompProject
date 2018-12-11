using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GunController : MonoBehaviour {

    private PlayerController player;

    private enum EMOTION { HAPPY, ANGRY };
    private GameObject smile;
    private GameObject angry;

    private float gunTimer = 0.0f;

    private GameObject bulletContainer; // Container GameObject to hide all bullets in heirarchy


    public Transform firePoint;
    public bool mainGun; //shoot with leftClick or left bumper if true
                         //shoot with rightClick or right bumper if false
    public Gun.AbstractGun currentGun;

    public bool debugging = false;

    private void Awake ()
    {
        // Only search through parent's children, quicker than searching through whole scene - Jake
        smile = transform.parent.Find("SmileFace").gameObject;
        angry = transform.parent.Find("AngryFace").gameObject;

        player = GetComponentInParent<PlayerController>();
        setGun(typeof(Gun.Handgun));

        bulletContainer = new GameObject();
        bulletContainer.name = "Active Bullets";
        bulletContainer.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainGun)
        {
            Vector3 ? target = null;

            if (Input.GetMouseButton(0))
            {
                Vector3 mouseTarget = player.getMousePos();
                mouseTarget.y = firePoint.transform.position.y;
                target = mouseTarget;
            }
            else if (Input.GetKey(KeyCode.Joystick1Button4))
                target = player.transform.position + (player.transform.forward * 100.0f);


            if (target != null)
            {
                setFace(EMOTION.ANGRY);

                GameObject bullet = currentGun.shoot(firePoint.position, target.Value);

                if (bullet != null)
                    bullet.transform.parent = bulletContainer.transform;
            }
            else
                setFace(EMOTION.HAPPY);
        }

        if (!mainGun)
        {
            if (Input.GetMouseButton(1))
            {
                setFace(EMOTION.ANGRY);
                Vector3 target = player.getMousePos();
                target.y = firePoint.transform.position.y;

                currentGun.shoot(firePoint.position, target);
            }
            else
                setFace(EMOTION.HAPPY);

            if ((Input.GetKey(KeyCode.Joystick1Button5)) && (player.useController))
            {
                setFace(EMOTION.ANGRY);

                Transform target;
                target = firePoint;

                target.position.Set(player.getMousePos().x, firePoint.transform.position.y, player.getMousePos().z);
                currentGun.shoot(firePoint.position, target.position);
            }
            else
                setFace(EMOTION.HAPPY);

        }

        if (currentGun.GetType() != typeof(Gun.Handgun))
        {
            gunTimer -= Time.deltaTime;

            if (gunTimer <= 0.0f)
                setGun(typeof(Gun.Handgun));
        }


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

        // Can't destroy old gun immediately on collision so find the right gun from all attached guns
        Gun.AbstractGun[] attachedGuns = gameObject.GetComponents<Gun.AbstractGun>();

        int i = attachedGuns.Length -1; // Loop backwards, more efficient since gun is most probably latest
        while (!currentGun || (currentGun.GetType() != gun && i >= 0))
        {
            currentGun = attachedGuns[i];
            i--;
        }

        gunTimer = duration;

        if (debugging)
            Debug.Log("Switching to " + currentGun.ToString());
    }


    /// <summary>
    /// Sets Darren's face to an emotion
    /// </summary>
    /// <param name="e">Happy or angry.</param>
    private void setFace(EMOTION e)
    {
        smile.SetActive(e == EMOTION.HAPPY);
        angry.SetActive(e == EMOTION.ANGRY);
    }
}
