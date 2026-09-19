
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class AttackState : StateBase
    {
        private Vector3 _toTargetDirection;

        public AttackState(StateMachine ownerMachine,
                           EnemyBase ownerEnemy,
                           Blackboard blackBoard) 
            
            : base(ownerMachine,
                   ownerEnemy,
                   blackBoard)
        {

        }

        public override void Enter()
        {
            if (_blackboard.Target == null)
            {
                _ownerMachine.ChangeState(_ownerEnemy.PatrolState);
            }
        }

        public override void OnPeriodicTick()
        {
            if (_blackboard.Target != null)
            {
                float distanceSqr = (_blackboard.Target.position-
                                     _ownerEnemy.transform.position).sqrMagnitude;

                if(distanceSqr >= _blackboard.AttackRangeSqr)
                {
                    _ownerMachine.ChangeState(_ownerEnemy.ChaseState);
                }

                _toTargetDirection = (_blackboard.Target.position-
                                      _ownerEnemy.transform.position).normalized;

            }
        }

        public override void OnTick()
        {
            _ownerEnemy.Attack();

            if (_blackboard.Target == null)
            {
                _ownerMachine.ChangeState(_ownerEnemy.PatrolState);
            }

            _ownerEnemy.RotateTowards(_toTargetDirection);
        }
    }
}
