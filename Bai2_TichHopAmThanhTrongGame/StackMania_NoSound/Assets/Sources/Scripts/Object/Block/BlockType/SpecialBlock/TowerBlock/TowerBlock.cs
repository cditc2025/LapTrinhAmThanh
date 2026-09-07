using UnityEngine;

namespace KienChi
{
	[RequireComponent(typeof(TowerBlockDisplay))]
	public class TowerBlock : SpecialBlock
	{
		private TowerBlockDisplay _towerBlockDisplay;
		protected override void Awake()
		{
			base.Awake();
			blockDisplay = GetComponent<TowerBlockDisplay>();

			_towerBlockDisplay = blockDisplay as TowerBlockDisplay;
		}
		protected override void Start()
		{
			base.Start();

			_towerBlockDisplay.SetTextBlock(numOfBlocks.ToString());
		}
		public override void DestroyBlock()
		{
			currentBlockCount--;
			_towerBlockDisplay.SetTextBlock(currentBlockCount.ToString());
			_towerBlockDisplay.DisplayDestroyBlockInSide((float)(numOfBlocks - currentBlockCount) / (float)numOfBlocks);
			if (currentBlockCount <= 0)
			{
				blockManager.ClearBlockInGrid(yAxis, xAxis);
				blockDisplay.DisplayDestroyBlock();
			}
			blockDisplay.EnableHitBlockVFX(this);
		}
		public override Block GetBlock()
		{
			return this;
		}
		protected override void SetUpBlock(ValueBlock valueBlock)
		{
			_towerBlockDisplay.SetMatSelectedBlock(colorManager.MatColorBlockList[this.valueBlock]);

		}
	}
}
