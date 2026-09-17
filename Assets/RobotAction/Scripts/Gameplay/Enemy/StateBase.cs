namespace RobotAction.Gameplay.Enemy
{
    public abstract class StateBase
    {
        protected readonly StateMachine _ownerMachine;
        protected readonly EnemyBase _ownerEnemy;
        protected readonly BlackBoard _blackBoard;

        protected StateBase(StateMachine ownerMachine,
                            EnemyBase ownerEnemy,
                            BlackBoard blackBoard)
        {
            _ownerMachine = ownerMachine;
            _ownerEnemy = ownerEnemy;
            _blackBoard = blackBoard;
        }

        /// <summary>
        /// 状態遷移の判定ロジック
        /// <br/>各Stateで適した処理を実装する
        /// </summary>
        public abstract void Think();

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
