using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropertiesInterface : MonoBehaviour
{
    GenericUpgradesData genericUpgradesData;
    ClassUpgradesData classUpgradesData;
    PlayerController playerController;
    PlayerShield playerShield;
    PlayerRecoilAttack playerRecoilAttack;
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

        playerController = GetComponent<PlayerController>();
        playerShield = GetComponentInChildren<PlayerShield>();
        playerRecoilAttack = GetComponentInChildren<PlayerRecoilAttack>();

        SetMovementSpeed(genericUpgradesData.movementSpeed);
        SetRecoil(genericUpgradesData.recoil);
        SetShieldRadius(genericUpgradesData.shieldRadius);
        SetShieldSlowdown(genericUpgradesData.shieldSlowdown);

        ToggleRecoilAttack(classUpgradesData.recoilAttackEnabled);
        ToggleShieldDeflection(classUpgradesData.shieldAttackEnabled);
        ToggleExplodingBullet(classUpgradesData.explodingBulletEnabled);
        ToggleShotGun(classUpgradesData.shotgunEnabled);
    }
    private void ToggleRecoilAttack(bool isDisabled)
    {
        playerController.recoilAttackEnabled = isDisabled;
    }
    private void ToggleShieldDeflection(bool isDisabled)
    {
        playerShield.shieldDeflectEnabled = isDisabled;
    }
    private void ToggleExplodingBullet(bool isDisabled)
    {
        playerController.explodingBulletEnabled = isDisabled;
    }
    private void ToggleShotGun(bool isDisabled)
    {
        playerController.shotgunEnabled = isDisabled;
    }
    private void SetMovementSpeed(float percentage)
    {
        playerController.movementSpeed = 500 * percentage;
    }
    private void SetRecoil(float percentage)
    {
        playerController.shootForce = 1500 * (1 - percentage);
    }
    private void SetShieldRadius(float percentage)
    {
        playerShield.SetShieldRadius(15 * percentage);
    }
    private void SetShieldSlowdown(float percentage)
    {
        playerShield.shieldSlowdown = (1 - percentage);
    }
}
