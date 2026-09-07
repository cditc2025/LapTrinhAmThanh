using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
	[RequireComponent(typeof(ShooterAnimator), typeof(ShooterMovement), typeof(AudioSource))]
	public abstract class Shooter : MonoBehaviour
	{
		IState _currentState;
		protected ShooterAnimator shooterAnimator;
		protected ShooterDisplay shooterDisplay;
		protected ShooterMovement shooterMovement;
		protected BlockManager blockManager;
		protected ColorManager colorManager;
		protected ShooterManager shooterManager;
		protected SlotManager slotManager;
		protected BoosterManager boosterManager;
		protected GameManager gameManager;
		protected Slot slot;
		protected Queue<Block> targetBlock = new Queue<Block>();
		protected int valueShooter;
		protected int typeShooter;
		protected int numberOfBullets;
		protected int pendingBullets;
		protected int xAxis;
		protected int yAxis;
		//* Biến cho phép bắn
		protected bool isAbleToShoot;
		//*Biến cho các shooter đang bắn kiểm tra lại trạng thái thông tin block đang bắn hoặc sắp bắn
		protected bool isReCheck;
		protected bool isBulletsNone;
		protected int shootPriority;
		protected abstract void SetUpShooter(ValueShooter valueShooter);
		public abstract void OnPositionChanged();
		protected virtual void Awake()
		{
			//
			shooterAnimator = GetComponent<ShooterAnimator>();
			shooterMovement = GetComponent<ShooterMovement>();

			blockManager = BlockManager.Instance;
			shooterManager = ShooterManager.Instance;
			slotManager = SlotManager.Instance;
			colorManager = ColorManager.Instance;
			boosterManager = BoosterManager.Instance;
			gameManager = GameManager.Instance;

			this.gameObject.layer = LayerMask.NameToLayer(Consts.LAYER_SHOOTER);
		}

		public void PlaySoundFX()
		{

			//play sound

		}

		void Start()
		{
			TransitionTo(new IdleState());
		}
		public virtual void CheckShooterSelf()
		{
		}
		public void InitShooter(ValueShooter valueShooter, int yAxis, int xAxis)
		{
			this.xAxis = xAxis;
			this.yAxis = yAxis;

			this.valueShooter = valueShooter.color;
			this.typeShooter = valueShooter.typeShooter;
			this.numberOfBullets = valueShooter.numOfBullet;
			this.pendingBullets = valueShooter.numOfBullet;

			SetUpShooter(valueShooter);
		}

		void Update()
		{
			_currentState?.OnExecute(this);
		}
		public void Idle()
		{
			shooterMovement.Idle();
			shooterAnimator.ChangeAnim(Consts.IDLE_ANIM);
		}
		public void Move(Vector3 target)
		{
			shooterMovement.InitMove(target);
			shooterAnimator.ChangeAnim(Consts.MOVING_ANIM);
			shooterDisplay.SetStateSmokeEffect(true);
		}
		public void Moving()
		{
			shooterMovement.Moving();
		}
		public void StopMoving()
		{
			shooterDisplay.SetStateSmokeEffect(false);
			slotManager.CanShoot();
		}
		public void Waiting()
		{
			Idle();
			// slotManager.CanShoot();
		}
		public void ChangePosition(Vector3 target)
		{
			TransitionTo(new MoveState(target));
		}
		public void Shoot(Block block)
		{
			if (numberOfBullets > 0)
			{
				shooterManager.NumBulletArrived++;
				//PlaySoundFX();
				shooterDisplay.EnableShotVFX();
				shooterAnimator.ChangeAnim(Consts.SHOOTING_ANIM);
				shootPriority = (block.XAxis + 1) % Consts.MAX_BLOCKS_PER_ROW;

				block.ShotToDestroyBlock();

				if (block.TypeBlock != Consts.GOLD_BLOCK)
					NumberOfBullets--;

				shooterManager.BulletSpawn.SpawnBullet(shooterDisplay.BulletTranform, block, this);
			}
		}
		public void PendingDestroyShooter(Block block)
		{
			PlaySoundFX();
			if (block.TypeBlock != Consts.GOLD_BLOCK)
				PendingBullets--;
		}
		public void Shooting(Block block)
		{
			if (block == null) return;
			shooterMovement.Shooting(block.GetBlock().GetCertralPos().position);
		}
		public void FindBlock()
		{
			targetBlock = blockManager.FindBlockInGrid(this, valueShooter);
		}
		public void HandleBlockQueueSelection(Slot slot)
		{
			this.gameObject.layer = LayerMask.NameToLayer(Consts.LAYER_DEFAULT);

			this.slot = slot;

			slot.SetShooterInSlot(this);

			transform.parent = slot.transform;

			TransitionTo(new MoveState(slot.GetPosSlot()));
			shooterMovement.SetWaitingShot();
		}

		public void TransitionTo(IState newState)
		{
			_currentState?.OnExit(this);

			_currentState = newState;

			_currentState?.OnEnter(this);
		}
		public virtual void DestroyShooter()
		{
			isBulletsNone = true;

			slotManager.CanShoot();
		}
		public bool HasArrived()
		{
			return !shooterMovement.CheckMoving();
		}
		public bool IsBoosterBreak()
		{
			return boosterManager.IsBreakBlockBooster;
		}
		void OnDisable()
		{
			// slotManager.ClearShooterInSlot(this);
			// slotManager.CanShoot();
		}
		public Transform GetCertralPos()
		{
			return shooterDisplay.BodyObj;
		}
		public int ValueShooter { get => valueShooter; set => valueShooter = value; }
		public int NumberOfBullets
		{
			get => numberOfBullets;
			set
			{
				numberOfBullets = value;
				shooterDisplay.SetProjectileCount(value, new Color32(255, 255, 255, 255));
			}
		}
		public bool IsAbleToShoot
		{
			get => isAbleToShoot;
			set
			{
				// Debug.Log(xAxis + " " + yAxis + " " + value);
				isAbleToShoot = value;
			}
		}
		public int XAxis
		{
			get => xAxis;
			set
			{
				xAxis = value;

				OnPositionChanged();
			}
		}
		public int YAxis
		{
			get => yAxis;
			set
			{
				yAxis = value;

				OnPositionChanged();
			}
		}

		public int PendingBullets
		{
			get => pendingBullets;
			set
			{
				pendingBullets = value;
				// shooterDisplay.SetProjectileCount(value, new Color32(255, 255, 255, 255));
			}
		}
		public int ShootPriority { get => shootPriority; set => shootPriority = value; }
		public int TypeShooter { get => typeShooter; set => typeShooter = value; }
		public bool IsBulletsNone { get => isBulletsNone; set => isBulletsNone = value; }
		public bool IsReCheck { get => isReCheck; set => isReCheck = value; }
		public Slot Slot { get => slot; set => slot = value; }
		public Queue<Block> TargetBlock { get => targetBlock; set => targetBlock = value; }
		public ShooterDisplay ShooterDisplay { get => shooterDisplay; set => shooterDisplay = value; }
	}
}
