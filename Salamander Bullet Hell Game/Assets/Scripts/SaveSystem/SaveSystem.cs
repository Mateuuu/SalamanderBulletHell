using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
public static class SaveSystem
{
    public static void SaveGenericUpgradesData(GenericUpgradesData genericUpgradesData)
    {
        string path = Application.persistentDataPath + "/GenericUpgradesData.json";
        string data = JsonUtility.ToJson(genericUpgradesData);
        System.IO.File.WriteAllText(path, data);

    }
    public static GenericUpgradesData ReadGenericUpgradesData()
    {
        string path = Application.persistentDataPath + "/GenericUpgradesData.json";
        if(File.Exists(path))
        {
            string data = System.IO.File.ReadAllText(path);
            return JsonUtility.FromJson<GenericUpgradesData>(data);
        }
        else
        {
            Debug.LogWarningFormat("Path {0} does not exist", path);
            return null;
        }
    }
    // public static void SaveClassUpgradesData(ClassUpgradesData classUpgradesData)
    // {
    //     string path = Application.persistentDataPath + "/ClassUpgradesData.json";
    //     string data = JsonUtility.ToJson(classUpgradesData);
    //     System.IO.File.WriteAllText(path, data);

    // }
    // public static ClassUpgradesData ReadClassUpgradesData()
    // {
    //     string path = Application.persistentDataPath + "/ClassUpgradesData.json";
    //     if(File.Exists(path))
    //     {
    //         string data = System.IO.File.ReadAllText(path);
    //         return JsonUtility.FromJson<ClassUpgradesData>(data);
    //     }
    //     else
    //     {
    //         Debug.LogWarningFormat("Path {0} does not exist", path);
    //         return null;
    //     }
    // }
    public static void SaveCurrencyData(CurrencyData currencyData)
    {
        string path = Application.persistentDataPath + "/CurrencyData.json";
        string data = JsonUtility.ToJson(currencyData);
        System.IO.File.WriteAllText(path, data);

    }
    public static CurrencyData ReadCurrencyData()
    {
        string path = Application.persistentDataPath + "/CurrencyData.json";
        if(File.Exists(path))
        {
            string data = System.IO.File.ReadAllText(path);
            return JsonUtility.FromJson<CurrencyData>(data);
        }
        else
        {
            Debug.LogWarningFormat("Path {0} does not exist", path);
            return null;
        }
    }
}

[System.Serializable]
public class GenericUpgradesData
{
    // * Movement Upgrades
    public float movementSpeed = 0f;
    public float invincibilityDash = 0f;
    public float bulletTrail = 0f;
    // * Recoil Upgrades
    public float recoilAmount = 0f;
    public float shotgun = 0f;
    public float recoilAttackSize = 0f;

    // * Shield Upgrades
    public float shieldRadius = 0f;
    public float shieldSlowdown = 0f;
    public float shieldBounce = 0f;
    public float shieldDecoy = 0f;
    // * Bullet Upgrades
    public float explodingBullet = 0f;
    public float bulletSize = 0f;
    public float bulletSpeed = 0f;
    public float bulletTrajectory = 0f;



    // the max level the player has unlocked
    // * Movement Upgrades Level
    public int movementLevel = 0;
    public int invincibilityDashLevel = 0;
    public int bulletTrailLevel;
    // * Recoil Upgrades Level
    public int recoilAmountLevel = 0;
    public int shotgunLevel = 0;
    public int recoilAttackSizeLevel = 0;
    // * Shield Upgrades Level

    public int shieldRadiusLevel = 0;
    public int shieldSlowdownLevel = 0;
    public int shieldBounceLevel = 0;
    public int shieldDecoyLevel = 0;
    // * Bullet Upgrades Level
    public int explodingBulletLevel = 0;
    public int bulletSizeLevel = 0;
    public int bulletSpeedLevel = 0;
    public int bulletTrajectoryLevel = 0;
}
// [System.Serializable]
// public class ClassUpgradesData
// {
//     public bool recoilAttackEnabled = false;
//     public bool shieldAttackEnabled = false;
//     public bool explodingBulletEnabled = false;
//     public bool shotgunEnabled = false;
// }
[System.Serializable]
public class CurrencyData
{
    public int money = 0;
    public int runes = 0;
}