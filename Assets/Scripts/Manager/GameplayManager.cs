using Dod;
using Roguelike;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayManager : Singleton<GameplayManager>
{
    public uint level;
    public Item item_Exp;
    public uint exp;
    public uint expCap;
    private float expProgress;

    public uint initialCap = 100;
    public float capMultipier = 1.1f;
    public float capMulIncreament = 0.1f;
    public uint enemyCount;
    public uint killCount;

    public Item item_Cash;
    public int money;

    private float playTime;

    public UnityEvent<uint> OnSpawn;
    public UnityEvent<uint> OnKilled;
    public UnityEvent<uint> OnLevelUp;
    public UnityEvent<uint> OnExp;
    public UnityEvent<uint> OnExpCapacityChange;
    public UnityEvent<float> OnExpPrg;
    public UnityEvent<float> OnPlaytime;
    public Skill[] skills;

    protected override void Awake()
    {
        base.Awake();
        exp = 0;
        expCap = initialCap;
    }
    private void Start()
    {
        OnExp?.Invoke(exp);
        OnExpCapacityChange?.Invoke(expCap);
    }

    private void LateUpdate()
    {
        playTime += Time.deltaTime;
        OnPlaytime?.Invoke(playTime);
    }

    public void Recall()
    {
        OnExp?.Invoke(exp);
        OnExpCapacityChange?.Invoke(expCap);
        OnPlaytime?.Invoke(playTime);
        OnSpawn?.Invoke(enemyCount);
        OnKilled?.Invoke(killCount);
        OnLevelUp?.Invoke(level);
        OnExpPrg?.Invoke(expProgress);
    }

    public void EnemySpawn()
    {
        enemyCount++;
        OnSpawn?.Invoke(enemyCount);
    }

    public void Killed()
    {
        killCount++;
        enemyCount--;
        OnKilled?.Invoke(killCount);
    }

    public void AddItem(Item item, int amount)
    {
        int tempId = item.ItemId;
        if (tempId == item_Exp.ItemId)
        {
            exp += (uint)amount;
            OnExp?.Invoke(exp);
            if (exp >= expCap)
            {
                exp = 0;
                level++;
                expCap = (uint)Mathf.RoundToInt((float)expCap * capMultipier);
                OnExpCapacityChange?.Invoke(expCap);
                OnLevelUp?.Invoke(level);
            }
            expProgress = (float)exp / (float)expCap;
            OnExpPrg?.Invoke(expProgress);

        }
        else if (tempId == item_Cash.ItemId)
        {
            money += amount;
        }
    }
}