using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon.Gun.AbstractGun), true)]
public class AbstractGun_GUI : Editor {

    public override void OnInspectorGUI()
    {
        Weapon.Gun.AbstractGun scr = target as Weapon.Gun.AbstractGun;

        scr.customValues = EditorGUILayout.Toggle("Custom Values", scr.customValues);

        // Only show if custom values enabled
        if (scr.customValues)
        {
            scr.damage = EditorGUILayout.FloatField("Damage", scr.damage);
            scr.speed = EditorGUILayout.FloatField("Bullet Speed", scr.speed);
            scr.fireRate = EditorGUILayout.FloatField("Rounds per second", scr.fireRate);
            scr.maxAmmo = EditorGUILayout.IntField("Max Ammo", scr.maxAmmo);
        }
    }
}
