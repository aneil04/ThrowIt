using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUpgrades : MonoBehaviour
{
    public static float maxUpgradeValue;
    public Slider strengthSlider;
    public Slider maxHealthSlider;
    public Slider movementSpeedSlider;
    public Slider healthRegenerationSlider;
    public Slider bodyDamageSlider;
    class Upgrade
    {
        private float upgradeValue = 0;
        private Slider upgradeSlider;
        public Slider UpgradeSlider
        {
            get { return upgradeSlider; }
            set
            {
                upgradeSlider = value;

                upgradeSlider.maxValue = maxUpgradeValue;
                upgradeSlider.minValue = 0;
                upgradeSlider.value = upgradeValue;
            }
        }
        public float UpgradeValue
        {
            get { return this.upgradeValue; }
            set
            {
                this.upgradeValue = value;
                this.upgradeSlider.value = value;
            }
        }
    }

    Dictionary<string, Upgrade> upgrades = new Dictionary<string, Upgrade>()
    {
        ["strength"] = new Upgrade(),
        ["maxHealth"] = new Upgrade(),
        ["movementSpeed"] = new Upgrade(),
        ["healthRegeneration"] = new Upgrade(),
        ["bodyDamage"] = new Upgrade()
    };
    // public int availableUpgrades = 0; 

    void Start()
    {
        upgrades["strength"].UpgradeSlider = strengthSlider;
        upgrades["maxHealth"].UpgradeSlider = maxHealthSlider;
        upgrades["movementSpeed"].UpgradeSlider = movementSpeedSlider;
        upgrades["healthRegeneration"].UpgradeSlider = healthRegenerationSlider;
        upgrades["bodyDamage"].UpgradeSlider = bodyDamageSlider;
    }

    public void increaseProperty(string prop)
    {
        // if (availableUpgrades <= 0) {return;}
        // availableUpgrades--;

        upgrades[prop].UpgradeValue += 1;
    }

}
