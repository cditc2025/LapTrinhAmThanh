using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace KienChi
{
	public class SlotManager : Singleton<SlotManager>
	{
		[SerializeField] private Transform _slotStorage;
		[SerializeField] private Slot _slotTemplate;
		private BlockManager _blockManager;
		private ShooterManager _shooterManager;
		private GameManager _gameManager;
		private int _valueGoldInx;
		private List<Slot> _slots = new List<Slot>();
		private Slot _slotAds;
		void Awake()
		{
			_blockManager = BlockManager.Instance;
			_shooterManager = ShooterManager.Instance;
			_gameManager = GameManager.Instance;

			_blockManager.OnDestroyBlock += CanShoot;
		}
		public void SpawnSlot(int slotsInRow)
		{
			float rowLength = Consts.SLOTS_X_GAP * slotsInRow;

			float xOrigin = -rowLength / 2;

			for (int s = 0; s < slotsInRow; s++)
			{
				float xPos = xOrigin + rowLength / (slotsInRow - 1) * s;

				Slot slot = Instantiate(_slotTemplate, _slotStorage);
				slot.transform.localPosition = new Vector3(xPos, 0, 0);

				if (s != (slotsInRow - 1))
				{
					slot.SetStateSlotAds(false);

					_slots.Add(slot);
					slot.InxSLot = s;
				}
				else
				{
					slot.gameObject.layer = LayerMask.NameToLayer(Consts.LAYER_SLOT_ADS);

					slot.SetStateSlotAds(true);

					_slotAds = slot;
				}
			}
		}
		float _speedMoveSlot = 0.5f;
		public void AddSlot(Slot slot)
		{
			_slots.Add(slot);

			int slotsInRow = _slots.Count + 1;

			List<Vector3> slotsPos = new List<Vector3>();

			if (slotsInRow <= Consts.MAX_SLOTS_PER_ROW)
			{
				float rowLength = Consts.SLOTS_X_GAP * slotsInRow;

				float xOrigin = -rowLength / 2;

				for (int s = 0; s <= slotsInRow; s++)
				{
					float xPos = xOrigin + rowLength / (slotsInRow - 1) * s;

					slotsPos.Add(new Vector3(xPos, 0, 0));
				}
				for (int s = 0; s < slotsInRow - 1; s++)
				{
					_slots[s].transform.DOLocalMoveX(slotsPos[s].x, _speedMoveSlot);
				}
				Slot slotAds = Instantiate(_slotTemplate, _slotStorage);
				slotAds.transform.localPosition = new Vector3(slotsPos[slotsInRow - 1].x, 0, 0);
				slotAds.gameObject.layer = LayerMask.NameToLayer(Consts.LAYER_SLOT_ADS);
				slotAds.SetStateSlotAds(true);
				_slotAds = slotAds;
			}
		}
		public List<Slot> CheckSlotEmpty(int neededSlots = 1)
		{
			List<Slot> slotEmpty = new List<Slot>();
			int maxSlotEmpty = 0;

			for (int i = 0; i < _slots.Count; i++)
			{
				if (!_slots[i].ShooterInSlot)
				{
					slotEmpty.Add(_slots[i]);
					maxSlotEmpty++;
				}
				else
					slotEmpty.Clear();

				if (slotEmpty.Count == neededSlots)
					return slotEmpty;
			}
			if (maxSlotEmpty >= neededSlots)
			{
				Queue<int> inxSlotQueue = new Queue<int>();
				for (int i = 0; i < _slots.Count; i++)
				{
					if (!_slots[i].ShooterInSlot)
					{
						inxSlotQueue.Enqueue(i);
					}
					else
					{
						if (inxSlotQueue.Count > 0)
						{
							int inx = inxSlotQueue.Dequeue();

							_slots[inx].ShooterInSlot = _slots[i].ShooterInSlot;

							_slots[inx].ShooterInSlot.Slot = _slots[inx];

							_slots[inx].ShooterInSlot.transform.parent = _slots[inx].transform;

							_slots[i].ShooterInSlot.ChangePosition(_slots[inx].GetPosSlot());

							_slots[i].ShooterInSlot = null;

							inxSlotQueue.Enqueue(i);
						}
					}
				}

				slotEmpty.Clear();
				for (int i = 0; i < _slots.Count; i++)
				{
					if (!_slots[i].ShooterInSlot)
					{
						slotEmpty.Add(_slots[i]);
						maxSlotEmpty++;
					}
					else
						slotEmpty.Clear();

					if (slotEmpty.Count == neededSlots)
						return slotEmpty;
				}
			}

			return slotEmpty;
		}

		public void CanShoot()
		{
			Dictionary<int, Shooter> valueShooterAny = new Dictionary<int, Shooter>(); //* Danh sách giá trị đã tồn tại
			Dictionary<Block, List<Shooter>> valueShooterPriority = new Dictionary<Block, List<Shooter>>(); //* Danh sách giá trị ưu tiên

			for (int i = 0; i < _slots.Count; i++)
			{
				if (_slots[i].ShooterInSlot)
				{
					Shooter shooter = _slots[i].ShooterInSlot;
					int valueShooter = shooter.ValueShooter;

					//*Gắn trả lại giá trị ban đầu để bắt đầu xét
					shooter.IsAbleToShoot = false;

					//*Gắn biến cho các shooter đang bắn kiểm tra lại trạng thái thông tin block đang bắn hoặc sắp bắn
					shooter.IsReCheck = true;
					//* Kiểm tra số lượng đạn còn lại
					if (shooter.NumberOfBullets > 0)
					{
						// Debug.Log(shooter.XAxis + " " + shooter.YAxis +" "+shooter.NumberOfBullets);

						Queue<Block> target = _blockManager.FindBlockInGrid(shooter, valueShooter);
						// Debug.Log(target.Count);
						//*Kiểm tra có block phù hợp để bắn phá không?
						if (target.Count > 0)
						{
							Block targetPriority = target.Peek();
							int typeBlock = targetPriority.TypeBlock;

							//*Kiểm tra độ ưu tiên
							if (typeBlock == Consts.SIMPLE_BLOCK)
							{
								//* Chưa tồn tại giá trị và có mục tiêu thì thêm vào danh sách
								if (!valueShooterAny.ContainsKey(valueShooter))
									valueShooterAny[valueShooter] = shooter;
								//* Đã tồn tại thì kiểm tra số lượng đạn
								else
									valueShooterAny[valueShooter] =
										shooter.NumberOfBullets < valueShooterAny[valueShooter].NumberOfBullets
										? shooter : valueShooterAny[valueShooter];
							}
							else
							{
								//*Thêm phần tử vào danh sách ưu tiên
								if (!valueShooterPriority.ContainsKey(targetPriority))
									valueShooterPriority[targetPriority] = new List<Shooter>();
								valueShooterPriority[targetPriority].Add(shooter);
							}
						}
					}
				}
			}
			//*Gắn giá trị trong danh sách ưu tiên
			foreach (KeyValuePair<Block, List<Shooter>> shooterList in valueShooterPriority)
			{
				// Debug.Log("Numofprio" + valueShooterPriority.Count);
				int needBullet = 0;
				//*Trường hợp khối đặc biệt thì chia số lượng cần phải bắn cho các shooter đang target
				SpecialBlock specialBlock = shooterList.Key as SpecialBlock;
				List<Shooter> shooters = shooterList.Value;

				specialBlock.NumOfDividedBlocks.Clear();

				needBullet = specialBlock.GetBlock().FutureBlockCount;

				int quotient = needBullet / shooters.Count;
				int remainder = needBullet % shooters.Count;

				int rad = Random.Range(0, shooters.Count);
				for (int i = 0; i < shooters.Count; i++)
				{
					if (i == rad)
						needBullet = quotient + remainder;
					else
						needBullet = quotient;

					if (needBullet > 0)
					{
						specialBlock.NumOfDividedBlocks.Enqueue(needBullet);
					}
				}
				// Debug.Log(shooters.Count);
				for (int i = 0; i < shooters.Count; i++)
				{
					// Debug.Log(shooters[i].XAxis + " " + shooters[i].YAxis + " " + shooters[i].NumberOfBullets);
					shooters[i].IsAbleToShoot = true;
				}

				// break;
			}


			//*Gắn giá trị trong danh sách
			foreach (KeyValuePair<int, Shooter> shooter in valueShooterAny)
			{
				shooter.Value.IsAbleToShoot = true;
			}

			if (valueShooterAny.Count == 0)
				_gameManager.CheckLoseGame();
		}
		public bool CheckFullShooterInShot()
		{
			int numShooterNeed = _shooterManager.GetNumShooterCanSelect();
			int numSlotHave = 0;

			if (_shooterManager.NumBulletArrived > 0) return false;

			for (int i = 0; i < _slots.Count; i++)
			{
				Shooter shooter = _slots[i].ShooterInSlot;
				Block[,] blockGrids = _blockManager.BlockGrid;

				if (shooter is LinkShooter linkShooter)
				{
					bool isCheck = false;
					for (int s = 0; s < linkShooter.ShooterLinks.Count; s++)
					{
						if (linkShooter.ShooterLinks[s].NumberOfBullets > 0)
						{
							isCheck = true;
							break;
						}
					}
					if (!isCheck)
					{
						numSlotHave++;
						continue;
					}
				}
				else
				{
					if (shooter == null)
					{
						numSlotHave++;
						continue;
					}
				}

				List<Block> blockCanCheck = new List<Block>();

				for (int j = 0; j < blockGrids.GetLength(1); j++)
				{
					if (blockGrids[0, j] != null)
					{
						blockCanCheck.Add(blockGrids[0, j]);
						break;
					}
				}


				for (int k = 0; k < blockCanCheck.Count; k++)
				{
					if (blockCanCheck[k].ValueBlock == shooter.ValueShooter && shooter.NumberOfBullets > 0)
					{
						return false;
					}
				}

				if (shooter != null && shooter.IsAbleToShoot)
				{
					return false;
				}
				if (shooter.TypeShooter != Consts.LINK_SHOOTER && shooter.NumberOfBullets <= 0)
				{
					numSlotHave++;
				}

				if (numSlotHave >= numShooterNeed)
				{
					return false;
				}
			}
			if (numSlotHave >= numShooterNeed)
			{
				return false;
			}
			Debug.Log(numSlotHave + " " + numShooterNeed + " LoseGame");
			return true;
		}
		public bool CheckAnyShooterInShot()
		{
			for (int i = 0; i < _slots.Count; i++)
			{
				if (_slots[i].ShooterInSlot != null && !_slots[i].ShooterInSlot.IsBulletsNone)
					return false;
			}

			return true;
		}
		public List<Slot> Slots { get => _slots; set => _slots = value; }
		public int ValueGoldInx { get => _valueGoldInx; set => _valueGoldInx = value; }
		public Slot SlotAds { get => _slotAds; set => _slotAds = value; }
	}
}
