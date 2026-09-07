using UnityEngine;

namespace KienChi
{
    public class MoveState : IState
    {
        private Vector3 target;
        public MoveState(Vector3 target)
        {
            this.target = target;
        }
        public void OnEnter(Shooter shooter)
        {
            shooter.Move(target);
        }

        public void OnExecute(Shooter shooter)
        {
            shooter.Moving();

            if (shooter.HasArrived())
            {
                if (shooter.Slot)
                {
                    shooter.TransitionTo(new WaitingShotState());
                    //move play bot pick
    
                }
                else
                    shooter.TransitionTo(new IdleState());
                return;
            }
        }

        public void OnExit(Shooter shooter)
        {
            shooter.StopMoving();
        }
    }
}
