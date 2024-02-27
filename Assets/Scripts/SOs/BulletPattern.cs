using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class BulletPattern : ScriptableObject {
    [Header("Projectile Settings")]
    public int numberOfProjectilesPerArray;  
    public int individualArraySpread = 180;  
    public int numOfArrays = 1;   
    public int totalArraySpread = 90;
    [Header("speed Settings")]
    public float projectileSpeed = 7f;               // Speed of the projectile.
    public float acceleration = .1f;
    public AnimationCurve curve;
    public bool useCurve = false;
    public float attackRotateSpeed = 15f;
    public float rotateSpeedChangeRate = .5f;
    public float fireRate = 4f;
    public float maxSpinSpeed = 30f;
}
