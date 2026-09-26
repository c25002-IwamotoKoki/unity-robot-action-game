using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class PatrolState : StateBase
    {
        private Vector3 _moveDirection;
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

            _moveDirection = _ownerEnemy.transform.position;

            //ƒ‰ƒ“ƒ_ƒ€‚È•ûŒü‚Ö‚ÌˆÚ“®‚É—p‚¢‚é‚½‚ß-1ˆÈã2–¢–‚ÅŒˆ‚ß‚Ä‚¢‚Ü‚·
            //‚±‚±‚Å-1~1‚Ì”ÍˆÍ‚É‚µ‚Ä‚¢‚é‚Ì‚ÍMoveSpeed‚Í_ownerEnemy“à‚Éˆê”C‚µ‚Ä‚¢‚é‚½‚ß
            //ˆÚ“®•ûŒü‚¾‚¯‚ğƒ‰ƒ“ƒ_ƒ€‚ÅŒˆ’è‚µ‚½‚¢‚Æ‚¢‚¤ˆÓ}‚ª‚ ‚é‚©‚ç‚Å‚·
            _moveDirection.x = Random.Range(-1, 2);
            _moveDirection.y = 0;
            _moveDirection.z = Random.Range(-1, 2);
        }

        public override void OnTick()
        {
            if (_blackboard.Target != null)
            {
                _ownerMachine.ChangeState(_ownerEnemy.ChaseState);
            }

            _ownerEnemy.RotateTowards(_moveDirection);
            _ownerEnemy.Move(_moveDirection);
        }
    }
}
