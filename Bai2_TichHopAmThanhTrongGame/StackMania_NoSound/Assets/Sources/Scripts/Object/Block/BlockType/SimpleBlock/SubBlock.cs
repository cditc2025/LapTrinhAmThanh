using UnityEngine;

namespace KienChi
{
	[RequireComponent(typeof(BlockDisplay))]
	public class SubBlock : Block
	{
		protected override void Awake()
		{
			base.Awake();
			blockDisplay = GetComponent<BlockDisplay>();
		}
		public override void DestroyBlock()
		{
			// Destroy(gameObject);

			// gameObject.SetActive(false);
			blockDisplay.DisplayDestroyBlock();
			blockDisplay.EnableHitBlockVFX(this);
		}

		public override Block GetBlock()
		{
			return this;
		}


		protected override void SetUpBlock(ValueBlock valueData)
		{
			// blockDisplay.SetMatSelectedBlock(colorManager.MatColorList[this.valueBlock]);

			GameObject block = Instantiate(BlockManager.Instance.BlockPrefabs[this.valueBlock], blockDisplay.Model3D);
			block.transform.localPosition = Vector3.zero;
		}
	}
}
