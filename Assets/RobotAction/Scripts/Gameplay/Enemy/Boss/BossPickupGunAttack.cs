using RobotAction.Gameplay.Parts.Weapons.Guns;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    public class BossPickupGunAttack : BossAttackBase
    {
        public override float AttackRange => _data.AttackRange;

        [SerializeField] private Transform[] _gunEquipPoints;
        [Header("e‚ª“®“I‚É•Ï‚í‚é‚½‚ßƒCƒ“ƒXƒyƒNƒ^[ã‚Å‚ ‚ç‚©‚¶‚ßUŒ‚”ÍˆÍ‚ğİ’è")]
        [SerializeField] private BossPickupGunAttackData _data;

        private Collider[] _detectedColliders;
        private List<GunBase> _currentGuns;

        private void Awake()
        {
            _currentGuns = new(_data.MaxPickupCount);
            _detectedColliders = new Collider[_data.MaxSearchCount];

            if(_data.MaxPickupCount > _gunEquipPoints.Length)
            {
                Debug.LogWarning("MaxPickupCount‚ğMaxPickupCountˆÈ‰º‚É‚µ‚Ä‚­‚¾‚³‚¢");
            }
        }

        public override void Execute()
        {
            if(_currentGuns.Count <= 0)
            {
                TryPickupGunProcess();
            }

            for (int i = 0; i < _currentGuns.Count; i++)
            {
                _currentGuns[i].SetTarget(_player.position);
                _currentGuns[i].Attack();
            }
        }

        public override bool CanAttack(float distanceSqr)
        {
            //UŒ‚”ÍˆÍ“à‚Å‚È‚¨‚©‚ÂE‚¦‚ée‚ª‚ ‚é‚È‚çtrue‚ğ•Ô‚·
            if (!base.CanAttack(distanceSqr)) return false;

            return _currentGuns.Count > 0 || HasPickableGunAround();
        }

        private bool HasPickableGunAround()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _data.PickupRange,
                _detectedColliders,
                _data.SearchLayer
            );

            for (int i = 0; i < hitCount; i++)
            {
                if (_detectedColliders[i].TryGetComponent(out GunBase _))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryPickupGunProcess()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _data.PickupRange,
                _detectedColliders,
                _data.SearchLayer
            );

            if (hitCount <= 0) return false;

            //‹ß‚¢‚à‚Ì‚©‚ç‡‚ÉE‚¤‚½‚ß‚É•À‚×‘Ö‚¦
            SortDetectedCollidersByProximity(hitCount);

            //e‚ª‚ ‚ê‚Î‘S‚ÄE‚¤
            for (int i = 0; i < hitCount; i++)
            {
                if (_detectedColliders[i].TryGetComponent(out GunBase gun))
                {
                    gun.Equip(_gunEquipPoints[i]);
                    gun.SetAttackTargetLayer(_data.AttackTargetLayer);
                    _currentGuns.Add(gun);

                    //ŒŸ’m”‚ÆE‚¦‚é”‚Í•Ê‚È‚½‚ßãŒÀ‚É‚È‚Á‚½‚çƒŠƒ^[ƒ“‚·‚éB
                    if (_currentGuns.Count == _data.MaxPickupCount) return true;
                }
            }

            return _currentGuns.Count > 0;
        }

        private void SortDetectedCollidersByProximity(int hitCount)
        {
            for (int i = 0; i < hitCount - 1; i++)
            {
                for (int j = i + 1; j < hitCount; j++)
                {
                    float distanceSqrI = (_detectedColliders[i].transform.position - transform.position).sqrMagnitude;
                    float distanceSqrJ = (_detectedColliders[j].transform.position - transform.position).sqrMagnitude;

                    //J‚Ì•û‚ªI‚æ‚è‹ß‚¢ˆÊ’u‚É‚¢‚é‚È‚ç“ü‚ê‘Ö‚¦
                    if (distanceSqrJ < distanceSqrI)
                    {
                        var temp = _detectedColliders[i];
                        _detectedColliders[i] = _detectedColliders[j];
                        _detectedColliders[j] = temp;
                    }
                }
            }
        }

    }
}
