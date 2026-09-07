using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
    public class IdleState : IState
    {
        public void OnEnter(Shooter shooter)
        {
            shooter.Idle();
        }

        public void OnExecute(Shooter shooter)
        {
            if (!shooter.IsBulletsNone && shooter.NumberOfBullets <= 0)
            {
                shooter.DestroyShooter();
                return;
            }
        }

        public void OnExit(Shooter shooter)
        {
        }
    }
}
