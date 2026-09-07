using System.Collections.Generic;
using System.Linq;
using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;

namespace KienChi
{
    [RequireComponent(typeof(LinkShooterDisplay))]
    public class LinkShooter : Shooter
    {
        private int _index;
        private int _order;
        private List<LinkShooter> _linkShooters = new List<LinkShooter>();
        private LinkShooterDisplay _linkShooterDisplay;

        protected override void Awake()
        {
            base.Awake();

            shooterDisplay = GetComponent<ShooterDisplay>();

            _linkShooterDisplay = shooterDisplay as LinkShooterDisplay;
        }
        public override void CheckShooterSelf()
        {
            base.CheckShooterSelf();

            List<Slot> slotEmpty = slotManager.CheckSlotEmpty(_linkShooters.Count);

            if (!boosterManager.IsPickSelect)
            {
                if (!CheckShooterLink())
                    return;
            }
            else
            {
                boosterManager.UsePickBooster();
            }

            if (slotEmpty.Count >= _linkShooters.Count)
            {
                for (int i = 0; i < _linkShooters.Count; i++)
                {
                    _linkShooters[i].HandleBlockQueueSelection(slotEmpty[i]);
                    _linkShooters[i].ShooterDisplay.EnableOutLine(false);
                    shooterManager.ClearSortInGrid(_linkShooters[i].YAxis, _linkShooters[i].XAxis);
                }
            }
        }
        private bool CheckAllDestroyShooter()
        {
            foreach (LinkShooter linkShooter in _linkShooters)
            {
                if (linkShooter.NumberOfBullets > 0) return false;
            }
            return true;
        }
        public override void DestroyShooter()
        {
            if (CheckAllDestroyShooter())
            {
                for (int i = 0; i < _linkShooters.Count; i++)
                {
                    _linkShooters[i].Slot.SetShooterInSlot(null);
                }
            }

            base.DestroyShooter();

            if (CheckAllDestroyShooter())
            {
                for (int i = 0; i < _linkShooters.Count; i++)
                {
                    Destroy(_linkShooters[i].gameObject);
                }
            }
        }
        public override void OnPositionChanged()
        {
            if (CheckShooterLink())
            {
                foreach (Shooter shooter in _linkShooters)
                {
                    shooter.ShooterDisplay.SetProjectileCount(shooter.NumberOfBullets, new Color32(255, 255, 255, 255));
                    if (!shooter.ShooterDisplay.IsSelect) //* Ngăn việc ghi đè quá nhiều lần bị xung đột khi select
                    {
                        shooter.ShooterDisplay.EnableOutLine(true);
                    }
                }
            }
            else
            {
                // foreach (Shooter shooter in _linkShooters)
                // {
                //     if (shooter.YAxis == 0)
                //         return;
                // }
                foreach (Shooter shooter in _linkShooters)
                {
                    shooter.ShooterDisplay.EnableOutLine(false);
                }
                shooterDisplay.SetProjectileCount(pendingBullets, new Color32(255, 255, 255, 180));
            }
        }
        private bool CheckShooterLink() //* Hàm kiểm tra có thể lựa chọn shooter link hay không
        {
            List<LinkShooter> linkShooterInit = _linkShooters.ToList();
            List<LinkShooter> itemsWithIndexZero = _linkShooters.Where(x => x.YAxis == 0).ToList();

            if (itemsWithIndexZero.Count == 0)
                return false;

            for (int i = 0; i < itemsWithIndexZero.Count; i++)
            {
                linkShooterInit.Remove(itemsWithIndexZero[i]);
            }

            for (int i = 0; i < itemsWithIndexZero.Count; i++) //*Kiểm tra từng cột có Y ban đầu = 0 đến 1,2,3,...
            {
                int yCheck = 1;
                while (true)
                {
                    LinkShooter first = linkShooterInit.FirstOrDefault(x => x.YAxis == yCheck && x.XAxis == itemsWithIndexZero[i].xAxis);

                    if (first)
                    {
                        linkShooterInit.Remove(first);
                        yCheck++;
                    }
                    else
                        break;
                }
            }

            if (linkShooterInit.Count == 0)
                return true;
            else
                return false;
        }

        protected override void SetUpShooter(ValueShooter valueShooter)
        {
            shooterDisplay.SetMatSelectedShooter(colorManager.MatColorShooterList[this.valueShooter], this);

            this._index = valueShooter.valueShooterLink.index;
            this._order = valueShooter.valueShooterLink.order;
            // Debug.Log(_order);
        }
        public int Index { get => _index; set => _index = value; }
        public int Order { get => _order; set => _order = value; }
        public List<LinkShooter> ShooterLinks
        {
            get => _linkShooters;
            set
            {
                _linkShooters = value;

                if (_order != value.Count - 1)
                    _linkShooterDisplay.Wiring(value[_order + 1].GetCertralPos());
                else
                    _linkShooterDisplay.RopeDisable();
            }
        }
    }
}
