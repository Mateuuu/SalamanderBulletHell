using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPropertiesInterface : MonoBehaviour
{
    [SerializeField] private UnlockableIncrementer moveSpeedIncrementer;
    [SerializeField] private UnlockableIncrementer recoilIncrementer;
    [SerializeField] private UnlockableIncrementer shieldRadiusIncrementer;
    [SerializeField] private UnlockableIncrementer shieldSlowdownIncrementer;
    GenericUpgradesData genericUpgradesData;
    // ClassUpgradesData classUpgradesData;
    PlayerController playerController;
    PlayerShield playerShield;
    PlayerRecoilAttack playerRecoilAttack;
    PlayerDeath playerDeath;
    PlayerBulletTrail playerBulletTrail;
    Dictionary<int, UnlockableIncrementer> incrementers = new Dictionary<int, UnlockableIncrementer>();
    private void OnEnable() => PlayerController.playerIsHit += TakeDamage;
    private void OnDisable() => PlayerController.playerIsHit -= TakeDamage;
    void Awake()
    {

        playerDeath = GetComponent<PlayerDeath>();

        genericUpgradesData = SaveSystem.ReadGenericUpgradesData();
        if(genericUpgradesData == null)
        {
            SaveSystem.SaveGenericUpgradesData(new GenericUpgradesData());
            genericUpgradesData = SaveSystem.ReadGenericUpgradesData();
        }
        // classUpgradesData = SaveSystem.ReadClassUpgradesData();
        // if(classUpgradesData == null)
        // {
        //     SaveSystem.SaveClassUpgradesData(new ClassUpgradesData());
        //     classUpgradesData = SaveSystem.ReadClassUpgradesData();
        // }

        playerController = GetComponent<PlayerController>();
        playerShield = GetComponentInChildren<PlayerShield>();
        playerBulletTrail = GetComponent<PlayerBulletTrail>();
        playerRecoilAttack = GetComponentInChildren(typeof(PlayerRecoilAttack), true) as PlayerRecoilAttack;

        SetMovementSpeed(genericUpgradesData.movementSpeed);
        SetInvincibilityDash(genericUpgradesData.invincibilityDash);
        SetBulletTrail(genericUpgradesData.bulletTrail);

        SetRecoil(genericUpgradesData.recoilAmount);
        SetRecoilAttackSize(genericUpgradesData.recoilAttackSize);
        SetShotgun(genericUpgradesData.shotgun);

        SetShieldRadius(genericUpgradesData.shieldRadius);
        SetShieldSlowdown(genericUpgradesData.shieldSlowdown);

        SetBulletSize(genericUpgradesData.bulletSize);
        SetBulletSpeed(genericUpgradesData.bulletSpeed);
        SetExplodingBullet(genericUpgradesData.explodingBullet);
        SetBulletTrajectory(genericUpgradesData.bulletTrajectory);
        // ToggleRecoilAttack(classUpgradesData.recoilAttackEnabled);
        // ToggleShieldDeflection(classUpgradesData.shieldAttackEnabled);
        // ToggleExplodingBullet(classUpgradesData.explodingBulletEnabled);
        // ToggleShotGun(classUpgradesData.shotgunEnabled);

        incrementers.Add(0, moveSpeedIncrementer);
        incrementers.Add(1, recoilIncrementer);
        incrementers.Add(2, shieldRadiusIncrementer);
        incrementers.Add(3, shieldSlowdownIncrementer);

    }
    IEnumerator Start()
    {
        yield return null;
        moveSpeedIncrementer.SetValue(genericUpgradesData.movementSpeed, genericUpgradesData.movementLevel);
        recoilIncrementer.SetValue(genericUpgradesData.recoilAmount, genericUpgradesData.recoilAmountLevel);
        shieldRadiusIncrementer.SetValue(genericUpgradesData.shieldRadius, genericUpgradesData.shieldRadiusLevel);
        shieldSlowdownIncrementer.SetValue(genericUpgradesData.shieldSlowdown, genericUpgradesData.shieldSlowdownLevel);

        if(incrementers[0].incrementStatus <= 0) incrementers.Remove(0);
        if(incrementers[1].incrementStatus <= 0) incrementers.Remove(1);
        if(incrementers[2].incrementStatus <= 0) incrementers.Remove(2);
        if(incrementers[3].incrementStatus <= 0) incrementers.Remove(3);
    }

    private void TakeDamage()
    {
        if(incrementers.Count == 0)
        {
            playerDeath.Die();
            return;
        }
        int rand = Random.Range(0, incrementers.Count);

        switch (incrementers.ElementAt(rand).Key)
        {
            case 0:
                SetMovementSpeed(moveSpeedIncrementer.SetIncrement(moveSpeedIncrementer.incrementStatus - 3));
                break;
            case 1:
                SetRecoil(recoilIncrementer.SetIncrement(recoilIncrementer.incrementStatus - 3));
                break;
            case 2:
                SetShieldRadius(shieldRadiusIncrementer.SetIncrement(shieldRadiusIncrementer.incrementStatus - 3));
                break;
            case 3:
                SetShieldSlowdown(shieldSlowdownIncrementer.SetIncrement(shieldSlowdownIncrementer.incrementStatus - 3));
                break;
        }
        if(incrementers[incrementers.ElementAt(rand).Key].incrementStatus <= 0) incrementers.Remove(incrementers.ElementAt(rand).Key);
    }
    // private void ToggleRecoilAttack(bool isDisabled)
    // {
    //     playerController.recoilAttackEnabled = isDisabled;
    // }
    // private void ToggleShieldDeflection(bool isDisabled)
    // {
    //     playerShield.shieldDeflectEnabled = isDisabled;
    // }
    // private void ToggleExplodingBullet(bool isDisabled)
    // {
    //     playerController.explodingBulletEnabled = isDisabled;
    // }
    // private void ToggleShotGun(bool isDisabled)
    // {
    //     playerController.shotgunEnabled = isDisabled;
    // }
    private void SetMovementSpeed(float percentage)
    {
        playerController.movementSpeed = 500 * percentage;
    }
    private void SetInvincibilityDash(float percentage)
    {
        playerController.invincibilityDash = percentage * 400;
    }
    private void SetBulletTrail(float percentage)
    {
        if(percentage > 0)
        {
            playerController.bulletTrailActivated = true;
        }
        else
        {
            playerController.bulletTrailActivated = false;
        }
        playerBulletTrail.SetTimeInterval((1 - percentage) * .5f);
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
    private void SetBulletSize(float percentage)
    {
        playerController.bulletSize = percentage;
    }
    private void SetBulletSpeed(float percentage)
    {
        playerController.bulletSpeed = percentage;
    }
    private void SetBulletTrajectory(float percentage)
    {
        playerController.bulletTrajectoryLength = percentage * 20f;
    }
    private void SetShotgun(float bulletCount)
    {
        playerController.shotgun = (int)bulletCount;
    }
    private void SetExplodingBullet(float percentage)
    {
        playerController.explodingBullet = percentage;
    }
    private void SetRecoilAttackSize(float percentage)
    {
        playerRecoilAttack.recoilAttackSize = percentage;
        if(playerRecoilAttack.recoilAttackSize != 0)
        {
            playerController.recoilAttackEnabled = true;
        }
        else
        {
            playerController.recoilAttackEnabled = false;
        }
    }
}
