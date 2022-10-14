using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuffController
{
    public event Action<int> OnBuffDamage;
    public event Action OnUnbuffDamage;

    private IDictionary<string, BuffKeyValue> _activeBuffs;
    private ISet<string> _inactiveBuffKeys;

    public TowerBuffController()
    {
        _activeBuffs = new Dictionary<string, BuffKeyValue>();
        _inactiveBuffKeys = new HashSet<string>();
    }

    public void Update()
    {
        foreach (KeyValuePair<string, BuffKeyValue> buff in _activeBuffs)
        {
            buff.Value.DecreaseDuration(Time.deltaTime);
            if (!buff.Value.IsActive)
            {
                _inactiveBuffKeys.Add(buff.Key);
                UnapplyBuff(buff.Key);
            }
        }

        RemoveInactiveBuffs();
    }

    private void RemoveInactiveBuffs()
    {
        foreach (string inactiveBuffKey in _inactiveBuffKeys)
        {
            RemoveBuff(inactiveBuffKey);
        }

        _inactiveBuffKeys.Clear();
    }

    private void RemoveBuff(string buffKey)
    {
        if(!_activeBuffs.ContainsKey(buffKey))
        {
#if UNITY_EDITOR
            Debug.LogWarning($"There is no buff called {buffKey} to remove. Aborting remove operation...");
#endif
            return;
        }

        _activeBuffs.Remove(buffKey);
    }

    public void AddBuffs(BuffKeyValue[] buffs)
    {
        foreach (BuffKeyValue buff in buffs)
        {
            if(_activeBuffs.ContainsKey(buff.Key))
            {
                ReplaceBuff(buff);
            }
            else
            {
                _activeBuffs.Add(buff.Key, buff);
            }

            ApplyBuff(buff);
        }
    }

    private void ReplaceBuff(BuffKeyValue buffToAdd)
    {
        _activeBuffs[buffToAdd.Key] = buffToAdd;
    }

    private void ApplyBuff(BuffKeyValue buff)
    {
        switch (buff.Key)
        {
            case "FireRate":
                break;
            case "Damage":
                OnBuffDamage?.Invoke((int)buff.Value);
                break;
            case "Range":
                break;
        }
    }

    private void UnapplyBuff(string buffKey)
    {
        switch (buffKey)
        {
            case "FireRate":
                break;
            case "Damage":
                OnUnbuffDamage?.Invoke();
                break;
            case "Range":
                break;
        }
    }
}
