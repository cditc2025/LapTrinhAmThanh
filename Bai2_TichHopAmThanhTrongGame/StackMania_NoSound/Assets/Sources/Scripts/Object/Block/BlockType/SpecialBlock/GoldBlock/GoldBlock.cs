using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KienChi
{
	[RequireComponent(typeof(GoldBlockDisplay))]
	public class GoldBlock : SpecialBlock
	{
		[SerializeField] private SubGoldBlock _subGoldBlock;
		private GoldBlockDisplay _goldBlockDisplay;
		Stack<SubGoldBlock> _subGoldBlocks = new Stack<SubGoldBlock>();
		private int _numOfGoldBlock;
		protected override void Awake()
		{
			base.Awake();
			_goldBlockDisplay = GetComponent<GoldBlockDisplay>();

			blockDisplay = _goldBlockDisplay as BlockDisplay;
		}
		public override void DestroyBlock()
		{
			SubGoldBlock currSubGoldBlock;

			if (_subGoldBlocks.Count > 0)
			{
				currSubGoldBlock = _subGoldBlocks.Peek();

				currSubGoldBlock.DestroyBlock();

				if (currSubGoldBlock.CurrentBlockCount == 0)
					_subGoldBlocks.Pop();
			}
			else
			{
				currentBlockCount--;
				if (currentBlockCount == 0)
				{
					blockManager.ClearBlockInGrid(yAxis, xAxis);
					_goldBlockDisplay.ChangeValueChest();
					_goldBlockDisplay.DisplayDestroyBlock();
					blockDisplay.EnableHitBlockVFX(this);
					PlayerResourceController.instance.GainGold(Consts.GAIN_GOLD_DESTROY_BLOCK);
				}
				_goldBlockDisplay.DisplayDestroyGoldBlock(currentBlockCount, numOfBlocks);
			}
		}
		public override Block GetBlock()
		{
			foreach (SubGoldBlock subGoldBlock in _subGoldBlocks)
			{
				if (!_subGoldBlock.BlockToBeDestroyed)
					return subGoldBlock;
			}
			return this;
		}
		protected override void SetUpBlock(ValueBlock valueData)
		{
			_numOfGoldBlock = valueData.valueGoldBlock.numOfGoldBlocks;

			for (int i = 1; i < _numOfGoldBlock; i++)
			{
				Size2D sizeBlock = new Size2D
				{
					x = xLen,
					y = yLen
				};
				ValueBlock valueBlock = new ValueBlock
				{
					color = this.valueBlock,
					typeBlock = this.typeBlock,
					numOfBlocks = this.numOfBlocks,
					size = sizeBlock
				};

				SubGoldBlock subGoldBlock = Instantiate(_subGoldBlock, transform);
				subGoldBlock.InitBlock(
					valueBlock,
					yAxis,
					xAxis);

				Vector3 posSpawn = new Vector3(0, i * Consts.BLOCK_Z_GAP, 0);
				subGoldBlock.transform.localPosition = posSpawn;

				_subGoldBlocks.Push(subGoldBlock);
			}
		}
		public override void ShotToDestroyBlock()
		{
			Block block = GetBlock();
			if (block != this)
				block.ShotToDestroyBlock();
			else
				base.ShotToDestroyBlock();
		}
	}
}
