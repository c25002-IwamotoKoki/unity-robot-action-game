namespace RobotAction.Gameplay.Enemy
{
    public abstract class StateBase
    {
        protected readonly StateMachine _ownerMachine;
        protected readonly EnemyBase _ownerEnemy;
        protected readonly Blackboard _blackboard;

        protected StateBase(StateMachine ownerMachine,
                            EnemyBase ownerEnemy,
                            Blackboard blackBoard)
        {
            _ownerMachine = ownerMachine;
            _ownerEnemy = ownerEnemy;
            _blackboard = blackBoard;
        }

        /// <summary>
        /// 毎フレーム実行すると重い処理を書くための場所
        /// </summary>
        public virtual void OnPeriodicTick()
        {

        }

        public virtual void Enter()
        {

        }

        public virtual void OnTick()
        {

        }

        public virtual void Exit()
        {

        }    
    }
}
