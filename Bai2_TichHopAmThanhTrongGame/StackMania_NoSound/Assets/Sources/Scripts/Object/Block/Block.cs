using UnityEngine;

namespace KienChi
{
	[RequireComponent(typeof(BlockAnimator), typeof(BlockMovement))]
	public abstract class Block : MonoBehaviour
	{
		protected BlockAnimator blockAnimator;
		protected BlockDisplay blockDisplay;
		protected BlockMovement blockMovement;
		protected ColorManager colorManager;
		protected BlockManager blockManager;
		protected int valueBlock; //*Giá trị của block (Màu sắc của block)
		protected int typeBlock; //*Loại block
		protected int numOfBlocks; //*Số lượng khối ban đầu
        protected int currentBlockCount; //*Số lượng khối hiện tại
        protected int futureBlockCount; //*Số lượng khối tương lai được (tính bằng số khối ban đầu - Số đạn đã được bắn và đang được bắn vào block này) 
		protected int inxBlock;
		protected int bulletHitCount;//*Số lượng đạn đã được bắn vào block này
		protected int xLen;
		protected int yLen;
		protected int xAxis;
		protected int yAxis;
		//*Biến kiểm tra block xác định SẼ bị phá hủy hay chưa (Bỏ qua tình trạng kiểm tra lại block khi đạn đang bay tới)
		protected bool blockToBeDestroyed;
		protected Vector3 certralPos;
		protected abstract void SetUpBlock(ValueBlock valueBlock);
		public abstract void DestroyBlock();
		public abstract Block GetBlock();
		protected virtual void Awake()
		{
			blockAnimator = GetComponent<BlockAnimator>();
			blockMovement = GetComponent<BlockMovement>();

			colorManager = ColorManager.Instance;
			blockManager = BlockManager.Instance;

			this.gameObject.layer = LayerMask.NameToLayer(Consts.LAYER_BLOCK);
		}
		protected virtual void Start()
		{
		}
		public void InitBlock(ValueBlock valueBlock, int yAxis, int xAxis)
		{
			this.xAxis = xAxis;
			this.yAxis = yAxis;

			this.xLen = valueBlock.size.x;
			this.yLen = valueBlock.size.y;

			this.valueBlock = valueBlock.color;
			this.typeBlock = valueBlock.typeBlock;
			this.numOfBlocks = valueBlock.numOfBlocks;
			this.currentBlockCount = numOfBlocks;
			this.futureBlockCount = numOfBlocks;

			this.certralPos = new Vector3(
				transform.localPosition.x - Consts.BLOCK_SIZE / 2 + xLen / 2,
				0,
				transform.localPosition.y - Consts.BLOCK_SIZE / 2 + yLen / 2
				);

			SetUpBlock(valueBlock);
		}
		public virtual void ShotToDestroyBlock()
		{
			bulletHitCount++;

			futureBlockCount = numOfBlocks - bulletHitCount;

			if (futureBlockCount <= 0)
			{
				blockToBeDestroyed = true;
			}
		}
		public void ChangePosition(Vector3 targetPos)
		{
			blockMovement.MoveTo(targetPos);
		}
		public Transform GetCertralPos()
		{
			return blockDisplay.Model3D;
		}
		public void SelectBlockBooster()
		{
			blockManager.ClearAllBlockSameColor(valueBlock);
		}
		public int TypeBlock { get => typeBlock; set => typeBlock = value; }
		public int ValueBlock { get => valueBlock; set => valueBlock = value; }
		public int XAxis { get => xAxis; set => xAxis = value; }
		public int YAxis { get => yAxis; set => yAxis = value; }
		public int NumOfBlocks { get => numOfBlocks; set => numOfBlocks = value; }
		public int BulletHitCount { get => bulletHitCount; set => bulletHitCount = value; }
		public int XLen { get => xLen; set => xLen = value; }
		public int YLen { get => yLen; set => yLen = value; }
		public int InxBlock { get => inxBlock; set => inxBlock = value; }
		public int FutureBlockCount { get => futureBlockCount; set => futureBlockCount = value; }
		public bool BlockToBeDestroyed { get => blockToBeDestroyed; set => blockToBeDestroyed = value; }
        public int CurrentBlockCount { get => currentBlockCount; set => currentBlockCount = value; }
    }
}
