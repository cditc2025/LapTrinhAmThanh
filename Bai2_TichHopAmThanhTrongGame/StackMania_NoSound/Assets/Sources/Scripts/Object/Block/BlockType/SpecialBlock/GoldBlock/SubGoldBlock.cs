using UnityEngine;

namespace KienChi
{
	[RequireComponent(typeof(GoldBlockDisplay))]
	public class SubGoldBlock : SpecialBlock
	{
		private GoldBlockDisplay _goldBlockDisplay;
		protected override void Awake()
		{
			base.Awake();
			_goldBlockDisplay = GetComponent<GoldBlockDisplay>();

			blockDisplay = _goldBlockDisplay as BlockDisplay;
		}
		public override void DestroyBlock()
		{
			currentBlockCount--;
			if (currentBlockCount == 0)
			{
				// Destroy(gameObject);
				// this.gameObject.SetActive(false);
				_goldBlockDisplay.ChangeValueChest();
				_goldBlockDisplay.DisplayDestroyBlock();

				blockDisplay.EnableHitBlockVFX(this);
				PlayerResourceController.instance.GainGold(Consts.GAIN_GOLD_DESTROY_BLOCK);
			}
			_goldBlockDisplay.DisplayDestroyGoldBlock(currentBlockCount, numOfBlocks);
		}
		public override Block GetBlock()
		{
			return this;
		}
		protected override void SetUpBlock(ValueBlock valueBlock)
		{
		}
	}
}
