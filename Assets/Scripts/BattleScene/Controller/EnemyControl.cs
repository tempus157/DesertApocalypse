using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyControl : EntityControlBase {
    [SerializeField] private Transform[] _enemySpawnPositions = null;
    private List<BattleEntity> _currentEnemies;

    private float[] _fillAmounts;

    public override void Initialize() {
        var resourceManager = ResourceManager.GetInstance();
        SOLevelInfo levelInfo = resourceManager.GetLevel(1);
        RoomInfo room = levelInfo.roomInformation[Random.Range(0, levelInfo.roomInformation.Length)];
        _currentEnemies = new List<BattleEntity>();

        for (int i = 0; i < room.enemyID.Length; ++i) {
            int eid = room.enemyID[i];
            var prefab = resourceManager.GetEnemyPrefab(eid);
            var enemy = Instantiate(prefab, _enemySpawnPositions[i].position, Quaternion.identity);

            int randomType = Random.Range((int)WeaponType.WeaponTypeStarts + 2, (int)WeaponType.WeaponTypeEnds);
            enemy.Initialize();
            enemy.SetupWeapon((WeaponType)randomType);

            _currentEnemies.Add(enemy);
        }

        _fillAmounts = new float[_currentEnemies.Count];
    }

    public override void Progress() {
        for (int i = 0; i < _currentEnemies.Count; ++i) {
            BattleEntity entity = _currentEnemies[i];
            if (entity == null) continue;
            bool isDead = entity.GetHPPercent() < Mathf.Epsilon;

            if (isDead) {
                entity.StartDeadAnimation();
                _currentEnemies[i] = null;
            }
        }
    }

    public override bool IsDefeated() {
        int numOfEntities = 0;
        foreach (BattleEntity entity in _currentEnemies) {
            if (entity != null) {
                ++numOfEntities;
            }
        }
        return numOfEntities == 0;
    }

    public float[] GetFillAmounts() {
        for (int i = 0; i < _currentEnemies.Count; ++i) {
            if (_currentEnemies[i] == null)
                _fillAmounts[i] = 0f;
            else
                _fillAmounts[i] = _currentEnemies[i].GetHPPercent();
        }
        return _fillAmounts;
    }

    public override IEnumerator DoAttack(BattleMain battleControl, System.Action uiSetupCallback) {
        Debug.Log("적 공격");

        foreach (BattleEntity enemy in _currentEnemies) {
            if (enemy == null) continue;
            enemy.StartAttackAnimation();

            var player = battleControl.PlayerCtrl.Player;
            player.ReceiveAttack(enemy.AttackPower);
            uiSetupCallback.Invoke();

            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }

    public BattleEntity GetRandomTarget() {
        BattleEntity entity = null;
        while (entity == null) {
            int idx = Random.Range(0, _currentEnemies.Count);
            entity = _currentEnemies[idx];
        }
        return entity;
    }

    public List<BattleEntity> GetRandomTargets(int targetCounts) {
        var entityList = _currentEnemies.ToList();
        for (int i = 0; i < entityList.Count; ++i) {
            if (entityList[i] == null) {
                entityList.RemoveAt(i--);
            }
        }

        int numOfEntities = entityList.Count;
        targetCounts = Mathf.Min(targetCounts, numOfEntities);

        int diff = numOfEntities - targetCounts;
        for (int i = 0; i < diff; ++i) {
            int removeIndex = Random.Range(0, entityList.Count);
            entityList.RemoveAt(removeIndex);
        }


        return entityList;
    }
}
