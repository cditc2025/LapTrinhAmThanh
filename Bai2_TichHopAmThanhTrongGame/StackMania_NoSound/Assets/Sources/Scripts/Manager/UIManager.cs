using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KienChi
{
	public class UIManager : Singleton<UIManager>
	{
		[SerializeField] private Transform _rateOfProgress;
		[SerializeField] private LayoutGroup _progressParent;
		[SerializeField] private GameObject _chestGroup;
		[SerializeField] private TMP_Text _chestCountDisplay;
		[SerializeField] private LevelIntroController _levelIntroController;
		private BlockManager _blockManager;
		private GameManager _gameManager;
		private LevelManager _levelManager;
		private int _chestCount;


		public LevelIntroController LevelIntroController { get => _levelIntroController; set => _levelIntroController = value; }

		void Awake()
		{
			_blockManager = BlockManager.Instance;
			_gameManager = GameManager.Instance;
			_levelManager = LevelManager.Instance;

			_blockManager.OnDestroyBlock += HandleProressSlide;
		}
		void Start()
		{
			if (_rateOfProgress)
			{
                _rateOfProgress.localScale = new Vector3(0, 1, 1);
				_progressParent.transform.localScale = new Vector3(0, 1, 1);
            }
				

			// _levelIntroController
		}
		public void HandleProressSlide()
		{
			uint totalBlock = _blockManager.TotalBlock;
			uint currDetroyBlock = _blockManager.NumOfDestroyBlock;
			_rateOfProgress.localScale = new Vector3((float)currDetroyBlock / totalBlock, 1, 1);
			_progressParent.transform.localScale = new Vector3(Mathf.Clamp01(_rateOfProgress.localScale.x * 100f), 1, 1);
            _progressParent.SetLayoutHorizontal();
		}
		public void SettingChest(int chest)
		{
			if (chest > 0)
			{
				_chestGroup.SetActive(true);
				_chestCount = chest;
				OnChestChanged(0);
			}
			else
			{
				_chestGroup.SetActive(false);
			}
		}
		public void OnChestChanged(int chest = 1)
		{
			_chestCount -= chest;
			_chestCountDisplay.text = _chestCount.ToString();
		}
	}
}
