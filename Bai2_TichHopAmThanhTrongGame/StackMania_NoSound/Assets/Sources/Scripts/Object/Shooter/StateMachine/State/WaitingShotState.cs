using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
    public class WaitingShotState : IState
    {
        public void OnEnter(Shooter shooter)
        {
            shooter.Waiting();
        }

        public void OnExecute(Shooter shooter)
        {
            if (shooter.IsBoosterBreak() && !shooter.IsAbleToShoot)
            {
                shooter.IsAbleToShoot = true;
            }

            if (shooter.IsAbleToShoot && !shooter.IsBoosterBreak())
            {
                shooter.TransitionTo(new ShootState());
                return;
            }

        }

        public void OnExit(Shooter shooter)
        {
        }
    }
}
