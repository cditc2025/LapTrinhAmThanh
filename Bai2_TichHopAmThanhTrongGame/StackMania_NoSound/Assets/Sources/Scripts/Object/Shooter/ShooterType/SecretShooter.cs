using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
	[RequireComponent(typeof(ShooterDisplay))]
	public class SecretShooter : Shooter
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

				shooterManager.ClearSortInGrid(yAxis, xAxis);

				yAxis = 0;
				OnPositionChanged();
				shooterDisplay.EnableOutLine(false);
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
		}

		public override void OnPositionChanged()
		{
			if (!boosterManager.IsPickSelect)
			{
				if (this.yAxis != 0)
				{
					shooterDisplay.SetProjectileCount(pendingBullets, new Color32(255, 255, 255, 180));
					shooterDisplay.SetProjectTileActive(false);
					shooterDisplay.EnableOutLine(false);
				}
				else
				{
					shooterDisplay.SetProjectileCount(pendingBullets, new Color32(255, 255, 255, 255));
					shooterDisplay.EnableOutLine(true);
					shooterDisplay.SetProjectTileActive(true);
				}
			}

			shooterDisplay.SetMatSelectedShooter(colorManager.MatColorShooterList[this.valueShooter], this);
		}

	}
}
