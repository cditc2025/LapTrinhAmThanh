using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KienChi
{
	public class GameManager : Singleton<GameManager>
	{
		private SlotManager _slotManager;
		private ShooterManager _shooterManager;
		private BlockManager _blockManager;
		private GameLevelManager _gameLevelManager;
		public bool isFinishLevel;
		public bool isWinLevel;
		void Awake()
		{
			_slotManager = SlotManager.Instance;
			_shooterManager = ShooterManager.Instance;
			_blockManager = BlockManager.Instance;
			_gameLevelManager = GameLevelManager.instance;

			_blockManager.OnDestroyBlock += CheckWinGame;
		}
		//
		private void Start()
		{
			isFinishLevel = false;
			isWinLevel = false;

            _gameLevelManager.onGameLevelFail += (result) =>
			{
				isFinishLevel = true;
			};
			_gameLevelManager.onGameLevelSuccess += (result) =>
			{
				isFinishLevel = true;
				isWinLevel = true;

            };
		}
		public void LoseGame()
		{
			_gameLevelManager.onGameLevelFail?.Invoke(new LevelResult());
		}
		public void WinGame()
		{
			LevelResult levelResult = new LevelResult
			{
				level = 1,
				retry_count = 1,
				booster_count = 1,
				slot_add = 1,
				pointer = 1,
				shuffle = 1,
			};

			_gameLevelManager.onGameLevelSuccess?.Invoke(new LevelResult());

			int currentLevel = PlayerPrefs.GetInt("level");
			TextAsset[] allTextAsset = Resources.LoadAll<TextAsset>($"Data");
			// string path = Path.Combine(Application.streamingAssetsPath, $"{Consts.FILE_PREFIX}{currentLevel}{Consts.FILE_SUFFIX}");
			// if (File.Exists(path) || Directory.Exists(path))
			// Debug.Log(allTextAsset.Length);
			//if (currentLevel < allTextAsset.Length)
			_gameLevelManager.IncreaseCurrentLevel();
		}
		public void CheckLoseGame()
		{
			// Debug.Log("Alooooo");
			if (_slotManager.CheckFullShooterInShot())
			{
				// Time.timeScale = 0;
				StartCoroutine(WaitingLosePanelGame());
			}
		}
		IEnumerator WaitingLosePanelGame()
		{
			isFinishLevel = true;
			yield return new WaitForSeconds(0.5f);
			LoseGame();
		}
		public void CheckWinGame()
		{
			// Debug.Log(_blockManager.NumOfDestroyBlock + " " + _blockManager.TotalBlock);
			if (_blockManager.NumOfDestroyBlock >= _blockManager.TotalBlock)
			{
				WinGame();
			}
		}
	}
}
