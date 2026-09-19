namespace RobotAction.Gameplay.Enemy
{
    public class PatrolState : StateBase
    {
        public PatrolState(StateMachine ownerMachine,
                           EnemyBase ownerEnemy,
                           Blackboard blackboard) 
            
            : base(ownerMachine,
                  ownerEnemy,
                  blackboard)
        {

        }

        public override void OnPeriodicTick()
        {
            _ownerEnemy.SearchNearlyTarget();
        }

        public override void OnTick()
        {
            if (_blackboard.Target != null)
            {
                _ownerMachine.ChangeState(_ownerEnemy.ChaseState);
            }
        }
    }
}
