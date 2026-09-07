using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
	[RequireComponent(typeof(BlockDisplay))]
	public class SimpleBlock : Block
	{
		[SerializeField] private SubBlock _subBlock;
		private GameObject[] _blockPrefabs;
		Stack<SubBlock> _subBlocks = new Stack<SubBlock>();
		protected override void Awake()
		{
			base.Awake();
			blockDisplay = GetComponent<BlockDisplay>();
			_blockPrefabs = BlockManager.Instance.BlockPrefabs;
		}
		protected override void SetUpBlock(ValueBlock valueData)
		{
			// blockDisplay.SetMatSelectedBlock(colorManager.MatColorList[this.valueBlock]);
			
			GameObject block = Instantiate(_blockPrefabs[this.valueBlock], blockDisplay.Model3D);
			block.transform.localPosition = Vector3.zero;

			for (int i = 1; i < numOfBlocks; i++)
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
					numOfBlocks = 1,
					size = sizeBlock
				};

				SubBlock subBlock = Instantiate(_subBlock, transform);
				subBlock.InitBlock(
					valueBlock,
					yAxis,
					xAxis);

				Vector3 posSpawn = new Vector3(0, i * Consts.BLOCK_Z_GAP, 0);
				subBlock.transform.localPosition = posSpawn;

				_subBlocks.Push(subBlock);
			}
		}
		public override void ShotToDestroyBlock()
		{
			Block block = GetBlock();
			if (block != this)
				block.ShotToDestroyBlock();

			base.ShotToDestroyBlock();
			// Debug.Log("fur "+futureBlockCount);
		}
		public override void DestroyBlock()
		{
			if (_subBlocks.Count > 0)
			{
				SubBlock subBlock = _subBlocks.Pop();
				subBlock.DestroyBlock();
			}
			else
			{
				blockManager.ClearBlockInGrid(yAxis, xAxis);
				blockDisplay.EnableHitBlockVFX(this);
				blockDisplay.DisplayDestroyBlock();
			}
		}
		public override Block GetBlock()
		{
			foreach (SubBlock subBlock in _subBlocks)
			{
				if (!subBlock.BlockToBeDestroyed)
					return subBlock;
			}

			return this;
		}

	}
}
