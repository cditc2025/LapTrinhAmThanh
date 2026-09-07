using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KienChi
{
	public class ShooterManager : Singleton<ShooterManager>
	{
		[SerializeField] private Transform _shooterStorage;
		[SerializeField] private List<Transform> _shooterStopPoint;
		[SerializeField] private SimpleShooter _simpleShooterTemplate;
		[SerializeField] private DouSimpleShooter _douSimpleShooterTemplate;
		[SerializeField] private LinkShooter _linkShooterTemplate;
		[SerializeField] private SecretShooter _secretShooterTemplate;
		[SerializeField] private BulletSpawn _bulletSpawn;
		[SerializeField] private GameObject _slotWaiting;
		private int numBulletArrived = 0;
		private Shooter[,] _shooterGrid;
		private Vector3[,] _shooterGridPos;
		private List<LinkShooter> _linkShooter = new List<LinkShooter>();
		private ColorManager _colorManager;
		private BlockManager _blockManager;

		private void Awake()
		{
			_bulletSpawn = GetComponent<BulletSpawn>();

			_colorManager = ColorManager.Instance;
			_blockManager = BlockManager.Instance;
		}
		public Vector3 GetTutoriorShooterPos()
		{
			return _shooterGridPos[0, 0];
		}
		public void SpawnShooter(ValueShooter[,] shooterRawDataGrid)
		{
			int colMax = shooterRawDataGrid.GetLength(0);
			int rowMax = shooterRawDataGrid.GetLength(1);

			_shooterGrid = new Shooter[colMax, rowMax];
			_shooterGridPos = new Vector3[colMax, rowMax];

			float rowLength = Consts.SHOOTER_X_GAP * rowMax;


			float xOrigin = -rowLength / 2;

			for (int c = 0; c < colMax; c++)
			{
				for (int r = 0; r < rowMax; r++)
				{
					if (shooterRawDataGrid[c, r] == null) continue;
					int valueShooter = shooterRawDataGrid[c, r].typeShooter;

					if (valueShooter < 0) continue;

					float xPos = rowMax != 1 ? xOrigin + rowLength / (rowMax - 1) * r : 0;

					Shooter shooter = Instantiate(ShooterTemplate(valueShooter), _shooterStorage);
					shooter.InitShooter(
						shooterRawDataGrid[c, r],
						c,
						r
						);

					Vector3 posSpawn = new Vector3(xPos, 0, -c * Consts.SHOOTER_Y_GAP);
					shooter.transform.localPosition = posSpawn;

					if (c == 0)
					{
						GameObject slot = Instantiate(_slotWaiting, _shooterStorage);
						slot.transform.localPosition = new Vector3(xPos, 0.01f, -c * Consts.SHOOTER_Y_GAP);
					}

					if (valueShooter == Consts.LINK_SHOOTER)
						_linkShooter.Add((LinkShooter)shooter);

					_shooterGrid[c, r] = shooter;
					_shooterGridPos[c, r] = _shooterStorage.TransformPoint(posSpawn);
				}
			}
			LinkShooterHandler();

			for (int c = 0; c < colMax; c++)
			{
				for (int r = 0; r < rowMax; r++)
				{
					if (_shooterGrid[c, r] == null) return;
					_shooterGrid[c, r].OnPositionChanged();
				}
			}
		}
		public void LinkShooterHandler()
		{
			// List<List<LinkShooter>> groups = new List<List<LinkShooter>>();
			// var groups = _linkShooter
			// 				.GroupBy(x => x.Index)
			// 				.ToList();

			Dictionary<int, List<LinkShooter>> groups = new Dictionary<int, List<LinkShooter>>();

			foreach (LinkShooter linkShooter in _linkShooter)
			{
				int index = linkShooter.Index;

				if (!groups.ContainsKey(index))
				{
					groups[index] = new List<LinkShooter>();
				}

				groups[index].Add(linkShooter);
			}
			;



			foreach (List<LinkShooter> group in groups.Values)
			{
				List<LinkShooter> itemsWithSameIndex = group.OrderBy(x => x.Order).ToList();

				for (int i = 0; i < itemsWithSameIndex.Count; i++)
				{
					itemsWithSameIndex[i].ShooterLinks = itemsWithSameIndex;
				}
			}
		}
		public Shooter ShooterTemplate(int shooterType)
		{
			switch (shooterType)
			{
				case Consts.SIMPLE_SHOOTER:
					return _simpleShooterTemplate;
				case Consts.DOU_SIMPLE_SHOOTER:
					return _douSimpleShooterTemplate;
				case Consts.LINK_SHOOTER:
					return _linkShooterTemplate;
				case Consts.SECRET_SHOOTER:
					return _secretShooterTemplate;
				default:
					return null;
			}
		}
		public void SortShooterList()
		{
			int xAxis = 0;
			int yAxis = 0;

			for (int c = 0; c < _shooterGrid.GetLength(0); c++)
			{
				for (int r = 0; r < _shooterGrid.GetLength(1); r++)
				{
					if (_shooterGrid[c, r] == null) continue;

					Shooter shooter = _shooterGrid[c, r];
					xAxis = shooter.XAxis;
					yAxis = shooter.YAxis;

					int yNext = 0;
					while (CheckSpaceBlockInGrid(yAxis - yNext - 1, xAxis)) yNext++;
					if (yNext == 0) continue;

					_shooterGrid[c, r].ChangePosition(_shooterGridPos[c - yNext, r]);

					shooter.YAxis = yAxis - yNext;

					_shooterGrid[c - yNext, r] = shooter;

					_shooterGrid[c, r] = null;
				}
			}
		}
		private bool CheckSpaceBlockInGrid(int y, int x)
		{
			if (y < 0 || _shooterGrid[y, x] != null) return false;

			return true;
		}
		public void PickShooter()
		{
			int cols = _shooterGrid.GetLength(0);
			int rows = _shooterGrid.GetLength(1);

			for (int c = 0; c < cols; c++)
				for (int r = 0; r < rows; r++)
				{
					if (_shooterGrid[c, r])
					{
						_shooterGrid[c, r].ShooterDisplay.EnableOutLine(true);
						if (_shooterGrid[c, r].TypeShooter != Consts.SECRET_SHOOTER)
						{
							_shooterGrid[c, r].ShooterDisplay.SetProjectileCount(_shooterGrid[c, r].NumberOfBullets, new Color32(255, 255, 255, 255));
						}
					}
				}
		}
		public void ResetPickShooter()
		{
			int cols = _shooterGrid.GetLength(0);
			int rows = _shooterGrid.GetLength(1);

			for (int c = 0; c < cols; c++)
				for (int r = 0; r < rows; r++)
				{
					if (_shooterGrid[c, r])
					{
						_shooterGrid[c, r].OnPositionChanged();
					}
				}
		}
		public void ShuffleShooter()
		{
			int cols = _shooterGrid.GetLength(0);
			int rows = _shooterGrid.GetLength(1);

			List<Shooter> tempList = new List<Shooter>();

			for (int c = 0; c < cols; c++)
				for (int r = 0; r < rows; r++)
					if (_shooterGrid[c, r] &&
					_shooterGrid[c, r].TypeShooter <= Consts.DOU_SIMPLE_SHOOTER)
					{
						tempList.Add(_shooterGrid[c, r]);
					}

			int size = tempList.Count;
			if (size < 2) return;
			List<int> indices = Enumerable.Range(0, size).ToList();
			System.Random rng = new System.Random();

			//* Derangement shuffle
			bool isDeranged = false;
			while (!isDeranged)
			{
				//* Shuffle
				for (int i = size - 1; i > 0; i--)
				{
					int k = rng.Next(i + 1);
					(indices[i], indices[k]) = (indices[k], indices[i]);
				}

				//* Kiểm tra có giữ nguyên vị trí nào không
				isDeranged = true;
				for (int i = 0; i < size; i++)
				{
					if (indices[i] == i)
					{
						isDeranged = false;
						break;
					}
				}
			}

			//* Tạo danh sách hoán đổi
			List<Shooter> shuffled = new List<Shooter>(size);
			for (int i = 0; i < size; i++)
				shuffled.Add(tempList[indices[i]]);

			int index = 0;
			for (int c = 0; c < cols; c++)
				for (int r = 0; r < rows; r++)
					if (_shooterGrid[c, r] &&
					_shooterGrid[c, r].TypeShooter <= Consts.DOU_SIMPLE_SHOOTER)
					{
						_shooterGrid[c, r] = shuffled[index];

						_shooterGrid[c, r].ChangePosition(_shooterGridPos[c, r]);

						_shooterGrid[c, r].XAxis = r;
						_shooterGrid[c, r].YAxis = c;

						index++;
					}


			Block[,] blockGrids = _blockManager.BlockGrid;
			bool hasSameValue = false;
			for (int c = 0; c < blockGrids.GetLength(1); c++)
			{
				for (int i = 0; i < _shooterGrid.GetLength(1); i++)
				{
					if (_shooterGrid[0, i] && blockGrids[0, c] && _shooterGrid[0, i].ValueShooter == blockGrids[0, c].ValueBlock)
					{
						hasSameValue = true;
						break;
					}
				}
			}

			if (!hasSameValue)
			{
				// Debug.Log("alooooo");
				int valueColor = -1;
				for (int c = 0; c < blockGrids.GetLength(1); c++)
				{
					if (blockGrids[0, c] != null)
					{
						valueColor = blockGrids[0, c].ValueBlock;
						break;
					}
				}
				// Debug.Log("ValueColor +" + valueColor);
				Shooter shooterTem = null;
				for (int c = 0; c < cols; c++)
					for (int r = 0; r < rows; r++)
					{
						// if (_shooterGrid[c, r] != null)
						// {
						// 	Debug.Log(c + " " + r + " " + _shooterGrid[c, r].ValueShooter);
						// }
						if (_shooterGrid[c, r] &&
							_shooterGrid[c, r].TypeShooter <= Consts.DOU_SIMPLE_SHOOTER &&
							_shooterGrid[c, r].ValueShooter == valueColor)
						{
							_shooterGrid[0, 0].ChangePosition(_shooterGridPos[c, r]);
							_shooterGrid[0, 0].XAxis = r;
							_shooterGrid[0, 0].YAxis = c;

							_shooterGrid[c, r].ChangePosition(_shooterGridPos[0, 0]);
							_shooterGrid[c, r].XAxis = 0;
							_shooterGrid[c, r].YAxis = 0;

							shooterTem = _shooterGrid[c, r];
							_shooterGrid[c, r] = _shooterGrid[0, 0];
							_shooterGrid[0, 0] = shooterTem;

							return;
						}
					}
			}
		}
		public bool CheckAnyShooterInGrid()
		{
			int cols = _shooterGrid.GetLength(0);
			int rows = _shooterGrid.GetLength(1);

			for (int c = 0; c < cols; c++)
				for (int r = 0; r < rows; r++)
					if (_shooterGrid[c, r] != null)
						return false;

			return true;
		}
		public int GetNumShooterCanSelect()
		{
			int numShooter = 0;
			int rows = _shooterGrid.GetLength(1);
			for (int r = 0; r < rows; r++)
				if (_shooterGrid[0, r] != null)
				{
					if (_shooterGrid[0, r] is LinkShooter linkShooter)
					{
						numShooter = (numShooter == 0 || numShooter > linkShooter.ShooterLinks.Count) ? linkShooter.ShooterLinks.Count : numShooter;
					}
					else
					{
						numShooter = 1;
						break;
					}
				}
			return numShooter;
		}
		public void ClearSortInGrid(int c, int r)
		{
			if (_shooterGrid[c, r] != null)
			{
				_shooterGrid[c, r] = null;
			}
			SortShooterList();
		}
		public BulletSpawn BulletSpawn { get => _bulletSpawn; set => _bulletSpawn = value; }
		public int NumBulletArrived { get => numBulletArrived; set => numBulletArrived = value; }
	}
}
