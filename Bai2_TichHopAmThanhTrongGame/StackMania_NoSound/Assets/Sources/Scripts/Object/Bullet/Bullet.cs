using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace KienChi
{
	public class Bullet : PooledObject
	{
		private ShooterManager _shooterManager;
		private float _speed = 30f;
		private float _shrinkSpeed = 5f;
		private float _arriveThreshold;
		private bool _isShoot;
		private Vector3 _target;
		private Block _block;
		private Shooter _shooter;
		private GameObject[] _blockPrefabs;
		private List<GameObject> _blockPrefabsInstance = new List<GameObject>();
		private GameObject _goldPrefabs;
		private int valueBlockInx;
		private bool _isGold;

		void Awake()
		{
			// _blockPrefabs = BlockManager.Instance.BlockPrefabs;
			// _goldPrefabs = Instantiate(BlockManager.Instance.GoldBlockTemplate.gameObject, Vector3.zero, Quaternion.identity, transform);
			// _goldPrefabs.SetActive(false);

			// foreach (GameObject block in _blockPrefabs)
			// {
			// 	GameObject blockInstance = Instantiate(block, Vector3.zero, Quaternion.identity, transform);
			// 	blockInstance.SetActive(false);

			// 	_blockPrefabsInstance.Add(blockInstance);
			// }

			_shooterManager = ShooterManager.Instance;
		}
		void Start()
		{
		}
		void Update()
		{
			if (_isShoot)
			{
				this._target = this._block.GetBlock().GetCertralPos().position;

				// transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, _shrinkSpeed * Time.deltaTime);

				// if (transform.localScale.magnitude < _arriveThreshold)
				// {
				// 	transform.localScale = Vector3.zero;
				// }

				transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
				transform.LookAt(_target);

				if (Vector3.Distance(transform.position, _target) < _arriveThreshold)
				{
					_isShoot = false;
					int xAxis = _block.XAxis;
					int yAxis = _block.YAxis;
					_shooterManager.NumBulletArrived--;

					_block.DestroyBlock();

					// _shooter.PendingDestroyShooter(_block);

					// if (!_isGold)
					// 	_blockPrefabsInstance[valueBlockInx].SetActive(false);
					// else
					// {
					// 	_goldPrefabs.SetActive(false);
					// 	_isGold = false;
					// }

					Release();
				}
			}
		}
		public void Shot(Block block, Transform shooterDis, Shooter shooter)
		{
			transform.localScale = Vector3.one;

			this._block = block;
			this._shooter = shooter;
			// this.valueBlockInx = block.ValueBlock;

			// if (block.TypeBlock != Consts.GOLD_BLOCK)
			// 	_blockPrefabsInstance[valueBlockInx].SetActive(true);
			// else
			// {
			// 	_goldPrefabs.SetActive(true);
			// 	_isGold = true;
			// }

			// this._target = shooterDis.position;
			this._arriveThreshold = Consts.BLOCK_SIZE * _block.XLen / 2;
			_isShoot = true;

			// _block.DestroyBlock();
		}
	}
}
