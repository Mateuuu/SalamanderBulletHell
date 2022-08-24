using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesSaveInterface : MonoBehaviour
{
    // * Movement Incrementers
    [SerializeField] private UnlockableIncrementer moveSpeedIncrementer;
    [SerializeField] private UnlockableIncrementer invincibilityDashIncrementer;
    [SerializeField] private UnlockableIncrementer bulletTrailIncrementer;
    // * Recoil Incrementers
    [SerializeField] private UnlockableIncrementer recoilAmountIncrementer;
    [SerializeField] private UnlockableIncrementer recoilShieldSizeIncrementer;
    [SerializeField] private UnlockableIncrementer shotgunIncrementer;
    // * Shield Incrementers
    [SerializeField] private UnlockableIncrementer shieldRadiusIncrementer;
    [SerializeField] private UnlockableIncrementer shieldSlowdownIncrementer;
    [SerializeField] private UnlockableIncrementer shieldBounceIncrementer;
    [SerializeField] private UnlockableIncrementer shieldDecoyIncrementer;
    // * Bullet Incrementers
    [SerializeField] private UnlockableIncrementer explodingBulletIncrementer;
    [SerializeField] private UnlockableIncrementer bulletSizeIncrementer;
    [SerializeField] private UnlockableIncrementer bulletSpeedIncrementer;
    [SerializeField] private UnlockableIncrementer bulletTrajectoryIncrementer;

    [HideInInspector] public GenericUpgradesData genericUpgradesData;




    
    void Awake()
    {
        genericUpgradesData = SaveSystem.ReadGenericUpgradesData();
        if(genericUpgradesData == null)
        {
            SaveSystem.SaveGenericUpgradesData(new GenericUpgradesData());
            genericUpgradesData = SaveSystem.ReadGenericUpgradesData();
        }
    }
    IEnumerator Start()
    {
        yield return null;
        moveSpeedIncrementer.SetValue(genericUpgradesData.movementSpeed, genericUpgradesData.movementLevel);
        bulletTrailIncrementer.SetValue(genericUpgradesData.bulletTrail, genericUpgradesData.bulletTrailLevel);
        // invincibilityDashIncrementer.SetValue(genericUpgradesData.invincibilityDash, genericUpgradesData.invincibilityDashLevel);

        recoilAmountIncrementer.SetValue(genericUpgradesData.recoilAmount, genericUpgradesData.recoilAmountLevel);
        recoilShieldSizeIncrementer.SetValue(genericUpgradesData.recoilShieldSize, genericUpgradesData.recoilShieldSizeLevel);
        shotgunIncrementer.SetValue(genericUpgradesData.shotgun, genericUpgradesData.shotgunLevel);

        shieldRadiusIncrementer.SetValue(genericUpgradesData.shieldRadius, genericUpgradesData.shieldRadiusLevel);
        shieldSlowdownIncrementer.SetValue(genericUpgradesData.shieldSlowdown, genericUpgradesData.shieldSlowdownLevel);
        shieldBounceIncrementer.SetValue(genericUpgradesData.shieldBounce, genericUpgradesData.shieldBounceLevel);
        // shieldDecoyIncrementer.SetValue(genericUpgradesData.shieldDecoy, genericUpgradesData.shieldDecoyLevel);

        explodingBulletIncrementer.SetValue(genericUpgradesData.explodingBullet, genericUpgradesData.explodingBulletLevel);
        bulletSizeIncrementer.SetValue(genericUpgradesData.bulletSize, genericUpgradesData.bulletSizeLevel);
        bulletSpeedIncrementer.SetValue(genericUpgradesData.bulletSpeed, genericUpgradesData.bulletSpeedLevel);
        bulletTrajectoryIncrementer.SetValue(genericUpgradesData.bulletTrajectory, genericUpgradesData.bulletTrajectoryLevel);
    }


    public void SetMovementSpeed(float percentage)
    {
        genericUpgradesData.movementSpeed = percentage;
    }
    public void SetInvincibilityDash(float percentage)
    {
        genericUpgradesData.invincibilityDash = percentage;
    }
    public void SetBulletTrail(float percentage)
    {
        genericUpgradesData.bulletTrail = percentage;
    }
    public void SetRecoil(float percentage)
    {
        genericUpgradesData.recoilAmount = percentage;
    }
    public void SetRecoilShieldSize(float percentage)
    {
        genericUpgradesData.recoilShieldSize = percentage;
    }
    public void SetShotgun(float bulletAmount)
    {
        genericUpgradesData.shotgun = bulletAmount;
    }
    public void SetShieldRadius(float percentage)
    {
        genericUpgradesData.shieldRadius = percentage;
    }
    public void SetShieldSlowdown(float percentage)
    {
        genericUpgradesData.shieldSlowdown = percentage;
    }
    public void SetShieldBounce(float percentage)
    {
        genericUpgradesData.shieldBounce = percentage;
    }
    public void SetShieldDecoy(float percentage)
    {
        genericUpgradesData.shieldDecoy = percentage;
    }
    public void SetExplodingBullet(float percentage)
    {
        genericUpgradesData.explodingBullet = percentage;
    }
    public void SetBulletSize(float percentage)
    {
        genericUpgradesData.bulletSize = percentage;
    }
    public void SetBulletSpeed(float percentage)
    {
        genericUpgradesData.bulletSpeed = percentage;
    }
    public void SetBulletTrajectory(float percentage)
    {
        genericUpgradesData.bulletTrajectory = percentage;
    }



    // * Movement Upgrades Level
    public void SetMovementSpeedLevel(int level)
    {
        genericUpgradesData.movementLevel = level;
    }
    public void SetInvincibilityDashLevel(int level)
    {
        genericUpgradesData.invincibilityDashLevel = level;
    }
    public void SetBulletTrailLevel(int level)
    {
        genericUpgradesData.bulletTrailLevel = level;
    }
    // * Recoil Upgrades Level
    public void SetRecoilLevel(int level)
    {
        genericUpgradesData.recoilAmountLevel = level;
    }
    public void SetRecoilShieldSizeLevel(int level)
    {
        genericUpgradesData.recoilShieldSizeLevel = level;
    }
    public void SetShotgunLevel(int level)
    {
        genericUpgradesData.shotgunLevel = level;
    }
    // * Shield Upgrades Level
    public void SetShieldRadiusLevel(int level)
    {
        genericUpgradesData.shieldRadiusLevel = level;
    }
    public void SetShieldSlowdownLevel(int level)
    {
        genericUpgradesData.shieldSlowdownLevel = level;
    }
    public void SetShieldBounceLevel(int level)
    {
        genericUpgradesData.shieldBounceLevel = level;
    }
    public void SetShieldDecoyLevel(int level)
    {
        genericUpgradesData.shieldDecoyLevel = level;
    }
    // * Bullet Upgrades Level
    public void SetExplodingBulletLevel(int level)
    {
        genericUpgradesData.explodingBulletLevel = level;
    }
    public void SetBulletSizeLevel(int level)
    {
        genericUpgradesData.bulletSizeLevel = level;
    }
    public void SetBulletSpeedLevel(int level)
    {
        genericUpgradesData.bulletSpeedLevel = level;
    }
    public void SetBulletTrajectoryLevel(int level)
    {
        genericUpgradesData.bulletTrajectoryLevel = level;
    }
}
