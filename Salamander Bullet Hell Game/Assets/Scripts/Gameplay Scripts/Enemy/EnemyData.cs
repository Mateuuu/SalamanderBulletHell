using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Assets/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Generic Upgrades")]
    [Space(10)]
    [Range(0, 1)] public float movementSpeed;
    [Range(0, 1)] public float recoilAmount;
    [Range(0, 1)] public float fireRate;
    [Range(0, 1)] public float shieldRadius;
    [Range(.005f, 1)] public float shieldSlowdown;
    [Range(0, 10)] public float fireRange;
    [Space(10)]

    [Header("Class Upgrades")]
    [Space(10)]
    public bool shieldBounceEnabled;
    public bool frontShieldEnabled;
    public bool explodingBulletsEnabled;
    public bool shotgunEnabled;
    [Space(10)]
    [Range(0, 1)] public float explosionSize;
    [Range(0, 5)] public int shotgunBulletAmount;
    [Range(0, 1)] public float shieldBounceProbability;
    [Range(0, 1)] public float frontShieldSize;
    [Space(10)]
    [Header("Textures and Images")]
    [Space(10)]
    public Color enemyColor;
    public Animator enemyAnimator;
    public Texture2D enemyWeapon;
    [Header("Textures and Images")]
    [Space(10)]
    public int moneyDropMin;
    public int moneyDropMax;
    [Range(0, 1)] public float runeDropChance;
}
