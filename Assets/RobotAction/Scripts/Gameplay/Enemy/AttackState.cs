namespace RobotAction.Gameplay.Enemy
{
    public class AttackState : StateBase
    {
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
            if (_blackboard.Target == null)
            {
                _ownerMachine.ChangeState(_ownerEnemy.PatrolState);
            }
        }

        public override void OnTick()
        {
            _ownerEnemy.Attack();
        }
    }
}
