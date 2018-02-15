﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum weapon { yellow,red,blue,green}
public class PlayerManager : MonoBehaviour {


    [Header("Stats")]
    private float maxLife = 9;
    public float life;
    public int level;
    public float experience;
    public float attack = 1.5f;
    public weapon weaponEquipped;
    [Tooltip("Every level will be increased by this amount")]
    public int pointsPerLevel = 1000;
    [Header("Particle")]
    public Transform particlePosition;

    private XintanaProfile profile;
    private Rad_GuiManager _guiManager;

    private float totalExpPerGame = 0;
    void Awake()
    {
        _guiManager = FindObjectOfType<Rad_GuiManager>();
        profile = Rad_SaveManager.profile;
        level = profile.level;
        experience = profile.experience;
        //TODO attack = level * something, or take damage from sheets
        //TODO maxLife = level * something , or take life from sheets
        life = GetMaxLife();
    }

    public float GetMaxLife()
    {
        return maxLife;
    }
    /// <summary>
    /// Callback from Animation Event 
    /// </summary>
    public void OnAttackFinished()
    {
        AnimationToIdle();
    }

    /// <summary>
    /// Callback for Animation Event Air Attack
    /// </summary>
    public void OnAirAttackFinished()
    {
        this.GetComponent<Animator>().SetBool("Attacking", false);
        this.GetComponent<Animator>().SetInteger("AnimState", 20);
    }

    /// <summary>
    /// CAllback for Animation Event Wing Jump
    /// </summary>
    public void OnAirWingJumpFinished()
    {
        AnimationToIdle();
    }

    /// <summary>
    /// Sets the animation parameter to Idle Animation
    /// </summary>
    private void AnimationToIdle()
    {
        this.GetComponent<Animator>().SetBool("Attacking", false);
        this.GetComponent<Animator>().SetInteger("AnimState", 0);
    }

    public void ReceiveDamage(float damage)
    {
        life -= damage;
    }

    public void AddExperience(int value)
    {
        totalExpPerGame += value;
        experience += value;
        _guiManager.AddExperienceToSlider(value);
        //if(experience >= GetMaxExperience())
        //{
        //    experience -= GetMaxExperience(); // Reset experience
        //    level++;
        //}
    }
    public int GetMaxExperience()
    {
        int _maxExperience = (level + 1) * pointsPerLevel;
        return _maxExperience;
    }

    public void SavePlayerStats()
    {
        profile.experience = experience;
        profile.level = level;
    }
    public float GetTotalExpPerGame()
    {
        return totalExpPerGame;
    }
    
    public void LevelUpAndUpdateExperience()
    {
        totalExpPerGame -= GetMaxExperience();
        level++;
    }
}
