using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
    public class ShootState : IState
    {
        private float _shootCooldown = 0.1f;
        private float _lastShootTime = 0f;
        private int _numOfBlocks;
        private Block _blockTarget;
        public void OnEnter(Shooter shooter)
        {
        }

        public void OnExecute(Shooter shooter)
        {
            if (shooter.NumberOfBullets <= 0)
            {
                shooter.TransitionTo(new IdleState());
                return;
            }

            if (shooter.IsReCheck)
            {
                shooter.IsReCheck = false;
                shooter.TargetBlock.Clear();
                _numOfBlocks = 0;
            }

            if (!shooter.IsAbleToShoot || shooter.IsBoosterBreak())
            {
                shooter.TransitionTo(new WaitingShotState());
                return;
            }

            if (Time.time - _lastShootTime >= _shootCooldown)
            {
                if (_numOfBlocks == 0)
                {
                    if (shooter.TargetBlock.Count == 0)
                    {
                        shooter.FindBlock();
                    }

                    if (shooter.TargetBlock.Count == 0)
                    {
                        shooter.TransitionTo(new WaitingShotState());
                        return;
                    }
                    else
                    {
                        _blockTarget = shooter.TargetBlock.Dequeue();

                        if (_blockTarget.TypeBlock == Consts.SIMPLE_BLOCK)
                        {
                            _numOfBlocks = _blockTarget.FutureBlockCount;
                        }
                        else
                        {
                            SpecialBlock specialBlock = _blockTarget as SpecialBlock;
                            if (specialBlock.NumOfDividedBlocks.Count > 0)
                            {
                                _numOfBlocks = specialBlock.NumOfDividedBlocks.Dequeue();
                            }
                            else
                            {
                                shooter.TransitionTo(new WaitingShotState());
                                return;
                            }
                        }
                    }
                }

                if (!_blockTarget.BlockToBeDestroyed)
                {
                    shooter.Shoot(_blockTarget);
                }

                _numOfBlocks--;
                _lastShootTime = Time.time;
            }
            shooter.Shooting(_blockTarget);
        }

        public void OnExit(Shooter shooter)
        {
            // GameManager.Instance.CheckLoseGame();
        }
    }
}
