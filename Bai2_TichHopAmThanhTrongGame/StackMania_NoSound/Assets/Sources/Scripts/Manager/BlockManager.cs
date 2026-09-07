using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace KienChi
{
	public class BlockManager : Singleton<BlockManager>
	{
		[SerializeField] private Transform _blockStorage;
		[SerializeField] private SimpleBlock _simpleBlockTemplate;
		[SerializeField] private TowerBlock _towerBlockTemplate;
		[SerializeField] private GoldBlock _goldBlockTemplate;
		[SerializeField] private GameObject[] _blockPrefabs;
		private Block[,] _blockGrid;
		private Vector3[,] _blockGridPos;
		private ColorManager _colorManager;
		private SlotManager _slotManager;
		private UIManager _uIManager;
		private BoosterManager _boosterManager;
		private uint _totalBlock;
		private uint _numOfDestroyBlock;
		public Action OnDestroyBlock;
		public Block[,] BlockGrid { get => _blockGrid; set => _blockGrid = value; }
		public uint TotalBlock { get => _totalBlock; set => _totalBlock = value; }
		public uint NumOfDestroyBlock { get => _numOfDestroyBlock; set => _numOfDestroyBlock = value; }
		public GameObject[] BlockPrefabs { get => _blockPrefabs; set => _blockPrefabs = value; }
		public GoldBlock GoldBlockTemplate { get => _goldBlockTemplate; set => _goldBlockTemplate = value; }

		private Dictionary<int, List<Block>> _valueBlockDictionary = new Dictionary<int, List<Block>>();

		private void Awake()
		{
			_colorManager = ColorManager.Instance;
			_slotManager = SlotManager.Instance;
			_uIManager = UIManager.Instance;
			_boosterManager = BoosterManager.Instance;
		}
		public void SpawnBlock(ValueBlock[,] blockRawDataGrid, uint totalBlocks)
		{
			int blockChest = 0;
			this._totalBlock = totalBlocks;
			int colMax = blockRawDataGrid.GetLength(0);
			int rowMax = blockRawDataGrid.GetLength(1);
			int xAxis = 0;
			int yAxis = 0;
			Vector3 posSpawn;

			ValueBlock valueBlock = null;

			_blockGrid = new Block[colMax, rowMax];
			_blockGridPos = new Vector3[colMax, rowMax];

			int inxBlock = 0;
			for (int c = 0; c < colMax; c++)
			{
				for (int r = 0; r < rowMax; r++)
				{
					valueBlock = blockRawDataGrid[c, r];

					if (valueBlock != null && valueBlock.typeBlock >= 0 && _blockGrid[c, r] == null)
					{
						Block block = Instantiate(BlockTemplate(valueBlock.typeBlock), _blockStorage);

						if (valueBlock.typeBlock == Consts.GOLD_BLOCK)
						{
							blockChest += valueBlock.valueGoldBlock.numOfGoldBlocks;
						}

						xAxis = valueBlock.size.x >= 0 ? r : r - valueBlock.size.x - 1;
						yAxis = c;

						block.InxBlock = inxBlock++;

						posSpawn = new Vector3(r * Consts.BLOCK_X_GAP, 0, c * Consts.BLOCK_Y_GAP);

						block.transform.localPosition = posSpawn;

						block.InitBlock(valueBlock, yAxis, xAxis);

						for (int i = c; i < c + Math.Abs(valueBlock.size.x); i++)
						{
							for (int j = r; j < r + Math.Abs(valueBlock.size.y); j++)
							{
								_blockGrid[i, j] = block;
							}
						}
					}
					posSpawn = new Vector3(r * Consts.BLOCK_X_GAP, 0, c * Consts.BLOCK_Y_GAP);
					if (_blockStorage != null)
						_blockGridPos[c, r] = _blockStorage.TransformPoint(posSpawn);
				}
			}

			_uIManager.SettingChest(blockChest);
		}
		public Block BlockTemplate(int blockType)
		{
			switch (blockType)
			{
				case Consts.SIMPLE_BLOCK:
					return _simpleBlockTemplate;
				case Consts.TOWER_BLOCK:
					return _towerBlockTemplate;
				case Consts.GOLD_BLOCK:
					return _goldBlockTemplate;
				default:
					return _simpleBlockTemplate;
			}
		}
		public Queue<Block> FindBlockInGrid(Shooter shooter, int value, int currectTarget = -1)
		{
			Queue<Block> blocks = new Queue<Block>();

			Block block = null;

			Block highestPriorityBlock = null;

			int num = 0;

			//* Tìm kiếm khối ưu tiên cùng màu hoặc khối vàng
			for (int r = 0; r < _blockGrid.GetLength(1); r++)
			{
				block = _blockGrid[0, r];
				if (block != null
					&& block.BlockToBeDestroyed == false
					&& block.TypeBlock > Consts.SIMPLE_BLOCK
					&& (block.ValueBlock == value || block.TypeBlock == Consts.GOLD_BLOCK)
					)
				{
					highestPriorityBlock =
						!highestPriorityBlock || highestPriorityBlock.TypeBlock < block.TypeBlock ?
						block : highestPriorityBlock;
				}
			}

			if (highestPriorityBlock != null)
			{
				blocks.Enqueue(highestPriorityBlock);
				return blocks;
			}

			//* Tìm khối cùng màu
			for (int r = shooter.ShootPriority; r < _blockGrid.GetLength(1) + shooter.ShootPriority; r++)
			{
				int x = r % _blockGrid.GetLength(1);
				block = _blockGrid[0, x];
				if (num < Consts.MAX_SHOOTING_BLOCKS
					&& block != null
					&& block.ValueBlock == value
					)
				{
					if (block.BlockToBeDestroyed == false)
					{
						blocks.Enqueue(block);
						num++;
					}
					else
					{
						block = _blockGrid[1, x];
						if (num < Consts.MAX_SHOOTING_BLOCKS
							&& block != null
							&& block.ValueBlock == value)
							blocks.Enqueue(block);
						num++;
					}
				}
			}

			//* Tìm khối lên hàng 2
			if (blocks.Count == 0)
			{
				for (int r = 0; r < _blockGrid.GetLength(1); r++)
				{
					//* Tìm kiếm tại các vị có hàng 1 rỗng
					if (_blockGrid[0, r] == null)
					{
						for (int c = 1; c < _blockGrid.GetLength(0); c++)
						{
							block = _blockGrid[c, r];
							if (c >= Consts.MIN_BLOCKS_PER_COL) break;

							//* Tìm kiếm khối cùng màu và có thể bắn tới check theo Raycast
							if (block != null
								&& block.BlockToBeDestroyed == false
								&& block.ValueBlock == value
								&& Physics.Linecast(shooter.transform.position, block.GetBlock().GetCertralPos().position, out RaycastHit hit, LayerMask.GetMask(Consts.LAYER_BLOCK))
								&& hit.transform.GetComponent<Block>() == block //* Cần tối ưu lại đoạn này
								)
							{
								blocks.Enqueue(block);
							}
							else
								break;
						}
					}
				}
			}
			return blocks;
		}
		//*Kiểm tra các vị trị trống ở trong cột
		private bool CheckSpaceBlockInGrid(int y, int x, int xLen)
		{
			if (y < 0) return false;

			for (int r = 0; r < Math.Abs(xLen); r++)
			{
				if (_blockGrid[y, x + r * xLen / Math.Abs(xLen)] != null)
				{
					return false;
				}
			}
			return true;
		}
		public void SortBlockInGrid()
		{
			int xAxis = 0;
			int yAxis = 0;
			int xLen = 0;
			int yLen = 0;

			for (int c = 1; c < _blockGrid.GetLength(0); c++)
			{
				for (int r = 0; r < _blockGrid.GetLength(1); r++)
				{
					if (_blockGrid[c, r] == null) continue;

					Block block = _blockGrid[c, r];
					xAxis = block.XAxis;
					yAxis = block.YAxis;
					xLen = block.XLen;
					yLen = block.YLen;

					int yNext = 0;
					while (CheckSpaceBlockInGrid(yAxis - yNext - 1, xAxis, xLen)) yNext++;
					if (yNext == 0) continue;

					// Vector3 posMove = new Vector3(0, 0, Consts.BLOCK_Y_GAP * yNext);
					_blockGrid[yAxis, xAxis].ChangePosition(_blockGridPos[yAxis - yNext, xAxis]);

					block.YAxis = yAxis - yNext;

					for (int i = 0; i < Mathf.Abs(yLen); i++)
					{
						for (int j = 0; j < Mathf.Abs(xLen); j++)
						{
							_blockGrid[yAxis - yNext + i, xAxis + j * xLen / Math.Abs(xLen)] = block;

							_blockGrid[yAxis + i, xAxis + j * xLen / Math.Abs(xLen)] = null;
						}
					}
				}
			}
		}
		public void ClearBlockInGrid(int c, int r)
		{
			if (_blockGrid[c, r] != null)
			{
				Block block = _blockGrid[c, r];
				int y = _blockGrid[c, r].YLen;
				int x = _blockGrid[c, r].XLen;


				for (int i = 0; i < Math.Abs(y); i++)
				{
					for (int j = 0; j < Math.Abs(x); j++)
					{
						_blockGrid[c + i, r + j * x / Math.Abs(x)] = null;
					}
				}
				_numOfDestroyBlock++;

				// Destroy(block.gameObject);
				// block.gameObject.SetActive(false);

				SortBlockInGrid();

				if (!_boosterManager.IsBreakBlockBooster)
					OnDestroyBlock.Invoke();
			}
		}
		public void HandlerOnceBlockInGrid(Block block, int c, int r)
		{
			block.XAxis = r;
			block.YAxis = c;
			block.transform.position = _blockGridPos[c, r];

			_blockGrid[c, r] = block;
			_blockGrid[c, r].ChangePosition(_blockGridPos[c, r]);

			// SortBlockInGrid();
		}
		public void ClearAllBlockSameColor(int value)
		{
			_isSequenceLoop = false;
			foreach (var ele in _valueBlockDictionary)
			{
				Color baseColor = ColorManager.Instance.MatColorBlockList[ele.Key].colorBlock.color;
				ColorManager.Instance.MatColorBlockList[ele.Key].colorBlock.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1); ;
			}

			if (_valueBlockDictionary[value].Count > 0)
			{
				Block block = null;
				for (int i = 0; i < _valueBlockDictionary[value].Count; i++) //* Phá khối
				{
					block = _valueBlockDictionary[value][i];
					ClearBlockInGrid(block.YAxis, block.XAxis);
				}

				int inx = 0;

				//* Trả lại khối từ vị trí thú 10 trở lên

				for (int c = Consts.MIN_BLOCKS_PER_COL; c < _blockGrid.GetLength(0); c++)
				{
					for (int r = 0; r < _blockGrid.GetLength(1); r++)
					{
						if (inx >= _valueBlockDictionary[value].Count) break;

						block = _valueBlockDictionary[value][inx];

						if (_blockGrid[c, r] == null)
						{
							HandlerOnceBlockInGrid(block, c, r);

							inx++;
						}
					}

					if (inx >= _valueBlockDictionary[value].Count) break;
				}

				//* Xét các khối còn lại
				if (inx < _valueBlockDictionary[value].Count)
					for (int c = _blockGrid.GetLength(0) - 1; c >= 0; c--)
					{
						if (inx >= _valueBlockDictionary[value].Count) break;

						for (int r = 0; r < _blockGrid.GetLength(1); r++)
						{
							if (inx >= _valueBlockDictionary[value].Count) break;

							block = _valueBlockDictionary[value][inx];

							if (_blockGrid[c, r] == null)
							{
								HandlerOnceBlockInGrid(block, c, r);

								inx++;
							}
						}

						if (inx >= _valueBlockDictionary[value].Count) break;
					}

			}
			_numOfDestroyBlock -= (uint)_valueBlockDictionary[value].Count;
			SortBlockInGrid();
			_boosterManager.UsePickBreak();
		}
		private Dictionary<int, Material> _initColor = new Dictionary<int, Material>();
		private bool _isSequenceLoop;
		public void BreakBlockClassify()
		{
			_isSequenceLoop = true;
			_valueBlockDictionary?.Clear();
			int valueBlock = 0;

			Block block = null;

			for (int c = 0; c < Consts.MIN_BLOCKS_PER_COL; c++)
			{
				if (c >= _blockGrid.GetLength(0)) break;

				for (int r = 0; r < Consts.MAX_BLOCKS_PER_ROW; r++)
				{

					block = _blockGrid[c, r];
					if (block != null && block.TypeBlock == Consts.SIMPLE_BLOCK)
					{
						valueBlock = block.ValueBlock;
						// Debug.Log(c + " " + r + " " + block.InxBlock);

						if (!_valueBlockDictionary.ContainsKey(valueBlock))
							_valueBlockDictionary[valueBlock] = new List<Block>();
						_valueBlockDictionary[valueBlock].Add(block);
					}
				}
			}

			foreach (var ele in _valueBlockDictionary)
			{
				if (ele.Value.Count > 0)
				{
					int colorInx = ele.Key;
					_initColor[colorInx] = ColorManager.Instance.MatColorBlockList[colorInx].colorBlock;
				}
			}

			// StartCoroutine(StartSequence());
		}
		IEnumerator StartSequence()
		{

			foreach (var ele in _valueBlockDictionary)
			{
				if (!_isSequenceLoop) yield break;

				ColorManager.Instance.MatColorBlockList[ele.Key].colorBlock.DOFade(0.2f, 1.5f)
					.SetEase(Ease.InOutSine)
					.OnComplete(() =>
					{
						ColorManager.Instance.MatColorBlockList[ele.Key].colorBlock.DOFade(1f, 1.5f);
					});

				yield return new WaitForSeconds(3);
			}
			StartCoroutine(StartSequence());
		}
	}
}
