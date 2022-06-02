using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class DisplayAds : MonoBehaviour
{
#if UNITY_ANDROID
    string gameID = "4464653";
#else 
    string gameID = "4464652";
#endif

    public PlayerDamage playerDamage;

    void Start() {
        Advertisement.Initialize(gameID);
    }

    public void displayAd()
    {
        // if (Advertisement.IsReady())
        // {
        //     Advertisement.Show("Interstitial Android");
        //     //call revive method 
        // }
    }
}
