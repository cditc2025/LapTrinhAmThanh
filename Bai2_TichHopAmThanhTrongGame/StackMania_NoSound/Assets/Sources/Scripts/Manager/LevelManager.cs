using System;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;


namespace KienChi
{

	public class LevelManager : Singleton<LevelManager>
	{
		private BlockManager _blockManager;
		private SlotManager _slotManager;
		private ShooterManager _shooterManager;
		private UIManager _uIManager;
		private int _currentLevel = 1;
		public Action<Difficulty> OnLoadLevelData;
		[SerializeField] private bool _isHome = true;
		private void Awake()
		{
			_blockManager = BlockManager.Instance;
			_slotManager = SlotManager.Instance;
			_shooterManager = ShooterManager.Instance;
			_uIManager = UIManager.Instance;

			if (!PlayerPrefs.HasKey("level"))
			{
				PlayerPrefs.SetInt("level", 1);
				PlayerPrefs.Save();
				_currentLevel = 1;
			}
			else
			{
				_currentLevel = PlayerPrefs.GetInt("level");
			}
		}
		private void Start()
		{
			if (!_isHome)
				InitDataLevel();
		}
		public Difficulty DifficultConvert(int difficult)
		{
			switch (difficult)
			{
				case Consts.DIFFICULT_NORMAL:
					return Difficulty.NORMAL;
				case Consts.DIFFICULT_DIFFICULT:
					return Difficulty.DIFFICULT;
				case Consts.DIFFICULT_BRUTAL:
					return Difficulty.BRUTAL;
				default:
					return Difficulty.NORMAL;
			}
		}
		private void InitDataLevel()
		{
			// string path = Path.Combine(Application.streamingAssetsPath, $"{Consts.FILE_PREFIX}{_currentLevel}{Consts.FILE_SUFFIX}"); 
			TextAsset[] allTextAsset = Resources.LoadAll<TextAsset>($"Data");
            // TextAsset textAsset = Resources.Load<TextAsset>($"Data/{Consts.FILE_PREFIX}{_currentLevel}");
            // Debug.Log($"Data/{Consts.FILE_PREFIX}{_currentLevel}{Consts.FILE_SUFFIX}");
            // string encryptedText = textAsset.text;
            // Debug.Log(textAsset);
            string encryptedBase64 = allTextAsset[GameLevelManager.instance.CalculateLevelIndex(_currentLevel, allTextAsset.Length)].text;
            //string encryptedBase64 = allTextAsset[_currentLevel - 1].text;
            string decrypted = AESCrypto.Decrypt(encryptedBase64);
			DataLevel dataLevel = JsonUtility.FromJson<DataLevel>(decrypted);
			// DataLevel dataLevel = JSONReader.ReadFromJson<DataLevel>(textAsset.text);

			_blockManager.SpawnBlock(PreprocessDataBlock(dataLevel), dataLevel.block.totalBlocks);
			_shooterManager.SpawnShooter(PreprocessDataShooter(dataLevel));
			_slotManager.SpawnSlot(PreprocessDataSlot(dataLevel));

			//
			OnLoadLevelData?.Invoke(DifficultConvert(dataLevel.level.difficulty));
		}
		private bool CheckBlockGridEmpty(ValueBlock[,] blockRawDataGrid, int x, int y, int rLen)
		{
			for (int r = 0; r < rLen; r++)
			{
				if (blockRawDataGrid[y, x + r] != null)
				{
					return false;
				}
			}
			return true;
		}
		private ValueBlock[,] PreprocessDataBlock(DataLevel dataLevel)
		{
			BlockData blockData = dataLevel.block;

			ValueBlock[,] blockRawDataGrid = new ValueBlock[blockData.size.y * 2, blockData.size.x];

			int rowBlock = 0;
			int colBlock = 0;
			ValueBlock valueBlock = null;

			for (int i = 0; i < blockData.size.y * blockData.size.x; i++)
			{
				if (i >= blockData.valueBlocks.Length) break;

				int cLen = blockData.valueBlocks[i].size.y;
				int rLen = blockData.valueBlocks[i].size.x;

				while (true)
				{
					if (blockRawDataGrid[colBlock, rowBlock] == null
						&& rLen + rowBlock <= blockData.size.x
						&& CheckBlockGridEmpty(blockRawDataGrid, rowBlock, colBlock, rLen)
						)
					{
						for (int y = 0; y < Mathf.Abs(cLen); y++)
						{
							if (y + colBlock >= blockData.size.y) break;

							for (int x = 0; x < Mathf.Abs(rLen); x++)
							{
								if (x + rowBlock >= blockData.size.x) break;

								if (x == 0 && y == 0)
								{
									// Debug.Log(blockData.valueBlocks[i].typeBlock);
									valueBlock = blockData.valueBlocks[i];
								}
								else
								{
									valueBlock = new ValueBlock
									{
										color = -1,
										typeBlock = -1,
										numOfBlocks = -1,
										size = blockData.valueBlocks[i].size
									};
								}

								blockRawDataGrid[colBlock + y, rowBlock + x] = valueBlock;
							}
						}
						break;
					}
					rowBlock++;

					if (rowBlock >= blockData.size.x)
					{
						rowBlock = 0;
						colBlock++;
					}
				}
			}

			//*Gắn -1 cho các ô còn lại trong mảng cuối cùng trong mảng 2 chiều

			for (int c = colBlock; c < blockData.size.y; c++)
				for (int r = rowBlock + 1; r < blockData.size.x; r++)
				{
					Size2D sizeBlock = new Size2D
					{
						x = -1,
						y = -1
					};
					blockRawDataGrid[c, r] = new ValueBlock
					{
						color = -1,
						typeBlock = -1,
						numOfBlocks = -1,
						size = sizeBlock
					};
				}

			return blockRawDataGrid;
		}
		private ValueShooter[,] PreprocessDataShooter(DataLevel dataLevel)
		{
			ShooterData shooterData = dataLevel.shooter;
			uint totalShooters = dataLevel.shooter.totalShooters;
			int shootersInRow = dataLevel.shooter.size.x;

			ValueShooter[,] shooterRawDataGrid = new ValueShooter[shooterData.size.y * 2, shooterData.size.x];

			int rowShooter = 0;
			int colShooter = 0;

			for (int i = 0; i < shooterData.size.y * shooterData.size.x; i++)
			{
				shooterRawDataGrid[colShooter, rowShooter] = dataLevel.shooter.valueShooters[i];

				rowShooter++;

				if (rowShooter >= shootersInRow)
				{
					rowShooter = 0;
					colShooter++;
				}
			}
			// Debug.Log(rowShooter + " " + shootersInRow);

			//*Gắn -1 cho các ô còn lại trong mảng cuối cùng trong mảng 2 chiều
			for (int r = rowShooter; r < shootersInRow; r++)
			{

				shooterRawDataGrid[colShooter, r] = new ValueShooter
				{
					color = -1,
					typeShooter = -1,
					numOfBullet = -1,
				};
			}

			return shooterRawDataGrid;
		}
		private int PreprocessDataSlot(DataLevel dataLevel)
		{
			return (int)dataLevel.slot.totalSlot;
		}
	}
}
