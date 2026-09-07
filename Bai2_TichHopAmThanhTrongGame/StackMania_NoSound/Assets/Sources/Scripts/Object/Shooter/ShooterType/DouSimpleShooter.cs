using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
	[RequireComponent(typeof(ShooterDisplay))]
	public class DouSimpleShooter : Shooter
	{
		protected override void Awake()
		{
			base.Awake();

			shooterDisplay = GetComponent<ShooterDisplay>();
		}
		public override void CheckShooterSelf()
		{
			base.CheckShooterSelf();

			if (!boosterManager.IsPickSelect)
			{
				if (this.yAxis != 0)
					return;
			}
			else
			{
				boosterManager.UsePickBooster();
			}

			List<Slot> slotEmpty = slotManager.CheckSlotEmpty();

			if (slotEmpty.Count > 0)
			{
				HandleBlockQueueSelection(slotEmpty[0]);
				shooterDisplay.EnableOutLine(false);
				shooterManager.ClearSortInGrid(yAxis, xAxis);
			}
		}
		public override void DestroyShooter()
		{
			slot.SetShooterInSlot(null);

			base.DestroyShooter();

			Destroy(gameObject);
		}
		protected override void SetUpShooter(ValueShooter valueShooter)
		{
			shooterDisplay.SetMatSelectedShooter(colorManager.MatColorShooterList[this.valueShooter], this);
		}
		public override void OnPositionChanged()
		{
			if (this.yAxis == 0)
			{
				shooterDisplay.SetProjectileCount(pendingBullets, new Color32(255, 255, 255, 255));
				shooterDisplay.EnableOutLine(true);
			}
			else
			{
				shooterDisplay.SetProjectileCount(pendingBullets, new Color32(255, 255, 255, 180));
				shooterDisplay.EnableOutLine(false);
			}
		}

	}
}
