using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesSaveInterface : MonoBehaviour
{
    [SerializeField] private UnlockableIncrementer moveSpeedIncrementer;
    [SerializeField] private UnlockableIncrementer recoilIncrementer;
    [SerializeField] private UnlockableIncrementer shieldRadiusIncrementer;
    [SerializeField] private UnlockableIncrementer shieldSlowdownIncrementer;
    [SerializeField] private Toggle recoilAttackToggle;
    [SerializeField] private Toggle shieldAttackToggle;
    [SerializeField] private Toggle explodingBulletToggle;
    [SerializeField] private Toggle shotgunToggle;
    [HideInInspector] public GenericUpgradesData genericUpgradesData;
    [HideInInspector] public ClassUpgradesData classUpgradesData;




    
    void Awake()
    {
        genericUpgradesData = SaveSystem.ReadGenericUpgradesData();
        if(genericUpgradesData == null)
        {
            SaveSystem.SaveGenericUpgradesData(new GenericUpgradesData());
            genericUpgradesData = SaveSystem.ReadGenericUpgradesData();
        }
        classUpgradesData = SaveSystem.ReadClassUpgradesData();
        if(classUpgradesData == null)
        {
            SaveSystem.SaveClassUpgradesData(new ClassUpgradesData());
            classUpgradesData = SaveSystem.ReadClassUpgradesData();
        }
    }
    IEnumerator Start()
    {
        yield return null;
        moveSpeedIncrementer.SetValue(genericUpgradesData.movementSpeed, genericUpgradesData.movementLevel);
        recoilIncrementer.SetValue(genericUpgradesData.recoil, genericUpgradesData.recoilLevel);
        shieldRadiusIncrementer.SetValue(genericUpgradesData.shieldRadius, genericUpgradesData.shieldRadiusLevel);
        shieldSlowdownIncrementer.SetValue(genericUpgradesData.shieldSlowdown, genericUpgradesData.shieldSlowdownLevel);

        recoilAttackToggle.isOn = classUpgradesData.recoilAttackEnabled;
        shieldAttackToggle.isOn = classUpgradesData.shieldAttackEnabled;
        explodingBulletToggle.isOn = classUpgradesData.explodingBulletEnabled;
        shotgunToggle.isOn = classUpgradesData.shotgunEnabled;
    }


    public void SetMovementSpeed(float percentage)
    {
        genericUpgradesData.movementSpeed = percentage;
    }
    public void SetRecoil(float percentage)
    {
        genericUpgradesData.recoil = percentage;
    }
    public void SetShieldRadius(float percentage)
    {
        genericUpgradesData.shieldRadius = percentage;
    }
    public void SetShieldSlowdown(float percentage)
    {
        genericUpgradesData.shieldSlowdown = percentage;
    }




    public void SetMovementSpeedLevel(int level)
    {
        genericUpgradesData.movementLevel = level;
    }
    public void SetRecoilLevel(int level)
    {
        genericUpgradesData.recoilLevel = level;
    }
    public void SetShieldRadiusLevel(int level)
    {
        genericUpgradesData.shieldRadiusLevel = level;
    }
    public void SetShieldSlowdownLevel(int level)
    {
        genericUpgradesData.shieldSlowdownLevel = level;
    }




    public void ToggleRecoilAttack(bool isDisabled)
    {
        classUpgradesData.recoilAttackEnabled = isDisabled;
    }
    public void ToggleShieldAttack(bool isDisabled)
    {
        classUpgradesData.shieldAttackEnabled = isDisabled;
    }
    public void ToggleExplodingBullet(bool isDisabled)
    {
        classUpgradesData.explodingBulletEnabled = isDisabled;
    }
    public void ToggleShotgun(bool isDisabled)  
    {
        classUpgradesData.shotgunEnabled = isDisabled;
    }
}
