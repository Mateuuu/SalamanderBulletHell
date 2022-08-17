using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeMenuButtons : MonoBehaviour
{
    [SerializeField] private UpgradesSaveInterface upgradesSaveInterface;
    [SerializeField] private ShopMoneyManager shopMoneyManager;

    public void PlayGame()
    {
        SaveSystem.SaveGenericUpgradesData(upgradesSaveInterface.genericUpgradesData);
        SaveSystem.SaveClassUpgradesData(upgradesSaveInterface.classUpgradesData);
        SaveSystem.SaveCurrencyData(shopMoneyManager.currencyData);

        SceneManager.LoadScene("Game");
    }
}
