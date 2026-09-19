using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class ChaseState : StateBase
    {
        private Vector3 _toTargetDirection;

        public ChaseState(StateMachine ownerMachine,
                         EnemyBase ownerEnemy,
                         Blackboard blackBoard)

            : base(ownerMachine,
                  ownerEnemy,
                  blackBoard)
        {

        }

        public override void OnPeriodicTick()
        {
            if (_blackboard.Target != null)
            {
                _toTargetDirection = (_blackboard.Target.position -
                                      _ownerEnemy.transform.position).normalized;
            }
        }

        public override void Enter()
        {
            if (_blackboard.Target == null)
            {
                _ownerMachine.ChangeState(_ownerEnemy.PatrolState);
            }
        }

        public override void OnTick()
        {
            float distanceSqr = (_blackboard.Target.position- 
                                 _ownerEnemy.transform.position).sqrMagnitude;

            if (distanceSqr <= _blackboard.AttackRangeSqr)
            {
                _ownerMachine.ChangeState(_ownerEnemy.AttackState);
            }

            _ownerEnemy.RotateTowards(_toTargetDirection);
            _ownerEnemy.Move(_ownerEnemy.transform.forward);
        }
    }
}
