
using UnityEngine;

public class PointInTime {

    public Vector3 position;
    public Quaternion rotation;
    public float health;

    /// <summary>
    /// Allows for RewindTimeManager to hold both values in single list
    /// </summary>
    /// <param name="_position"></param>
    /// <param name="_rotation"></param>
    public PointInTime (Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }
    public PointInTime(Vector3 _position, Quaternion _rotation, float _health)
    {
        position = _position;
        rotation = _rotation;
        health = _health;
    }

}
