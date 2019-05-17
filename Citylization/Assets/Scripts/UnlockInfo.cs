using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnlockStatus { Locked, Unlocked, AtMax, CantAfford}

public class UnlockInfo
{
    public UnlockStatus unlockStatus;
    public int currentAmount;
    public float discount;

    public UnlockInfo(UnlockStatus _unlockStatus, int _currentAmount, float _discount)
    {
        unlockStatus = _unlockStatus;
        currentAmount = _currentAmount;
        discount = _discount;

    }
}
