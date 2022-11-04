using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
    WeaponStarts = 100,
    Default,
    Katana,
    Knife,
    Rifle,
    Count
}

public abstract class WeaponBase : ScriptableObject {
    public WeaponType WeaponID;
    public EntityStatus extraStatus;
    public abstract void Attack(BattleEntity caster, EnemyControl receiverController);
}
