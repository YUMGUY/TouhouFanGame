using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet_Null", menuName = "Custom Bullet Pattern")]
public class SpecialBulletData : ScriptableObject
{
    public GameObject bulletResourceS;
    public string bulletPoolTag;
    public bool hasRandomAngles;

    [Header("spawning vars")]
    public int numScols;
    public float fireRateSC;
    public float minAngle;
    public float maxAngle;
    public float lifeTimeSC;
    public bool willAccelSC;
    

    [Header("Bullet Properties")]
    public float bulletSpeedSC;
    public Vector2 dirSC;
    public AnimationCurve curveSC;


}
