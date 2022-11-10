using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityStatus {
    public int maxHP;
    public int atk;
    public int def;
    public int dodge;
    public int aim;

    public EntityStatus(int hp, int atk, int def, int dodge, int aim)
    {
        this.maxHP = hp;
        this.atk = atk;
        this.def = def;
        this.dodge = dodge;
        this.aim = aim;
    }
}

public class BattleEntity : MonoBehaviour {
    [SerializeField] private int _entityID = 0;
    public int EntityID {
        get { return _entityID; }
    }
    [SerializeField] private EntityStatus _entityStatus;
    public int AttackPower {
        get { return _entityStatus.atk; }
    }

    private int _maxHP;
    private int _curHP;
    private WeaponBase _currentWeapon = null;
    public WeaponBase CurrentWeapon {
        get { return _currentWeapon; }
    }

    public int CurHP {
        get { return _curHP; }
    }

    public virtual void Initialize() {
        _curHP = _entityStatus.maxHP;
        _currentWeapon = ResourceManager.GetInstance().GetWeapon(WeaponType.Default);
    }

    public float GetHPPercent() {
        return (float)_curHP / _entityStatus.maxHP;
    }

    public void DoAttack(EnemyControl receiverController) {
        CurrentWeapon.Attack(this, receiverController);
    }

    public void ReceiveAttack(int damage) {
        DecreaseHP(damage);
    }

    protected void DecreaseHP(int amount) {
        _curHP = Mathf.Max(_curHP - amount, 0);
    }
}