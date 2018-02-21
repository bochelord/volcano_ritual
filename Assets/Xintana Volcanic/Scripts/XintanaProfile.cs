﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XintanaProfile : MonoBehaviour {

    #region Variables

    public string gameVersion = "";
    public int profileID;
    public int highscore;
    public int gems;
    public int shells;
    public int timesKilledByZazuc;
    public int timesKilledByMakula;
    public int timesKilledByKogi;
    public int timesKilledByBlackKnight; //TODO add variable for any type of enemy
    public int level;
    public float experience;
    public WeaponType weaponEquipped;
    public int adsViewed;
    public int adsSkipped;
    public bool audioEnabled;
    public bool extraLife;
    public bool noAds;
    public bool doubleScore;
    public bool firstTimePlayed;
    public System.DateTime timeStamp;
    public int sharedScoreTimes;
    //TODO creat purchases done and equipped_items;
    #endregion

    #region contructor

    public XintanaProfile GenerateXintanaProfile(int id)
    {
        XintanaProfile profile = new XintanaProfile();
#if !UNITY_EDITOR
        profile.gameVersion = Application.version;
#else
        profile.gameVersion = "Debugversion";
#endif
        profile.profileID = id;
        profile.highscore = 0;
        profile.gems = 1;
        profile.shells = 10;
        profile.adsViewed = 0;
        profile.adsSkipped = 0;
        profile.level = 1;
        profile.experience = 0;
        profile.weaponEquipped = WeaponType.red; // TODO check if this one will be the default weapon
        profile.audioEnabled = true;
        profile.extraLife = false;
        profile.noAds = false;
        profile.doubleScore = false;
        profile.firstTimePlayed = true;
        profile.timesKilledByBlackKnight = 0;
        profile.timesKilledByKogi = 0;
        profile.timesKilledByMakula = 0;
        profile.timesKilledByZazuc = 0;
        profile.timeStamp = System.DateTime.Now;
        profile.sharedScoreTimes = 0;
        return profile;
    }
    #endregion
}
