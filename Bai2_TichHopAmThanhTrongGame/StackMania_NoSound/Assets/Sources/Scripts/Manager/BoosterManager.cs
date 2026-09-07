using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace KienChi
{
	public class BoosterManager : Singleton<BoosterManager>
	{
		private BlockManager _blockManager;
		private ShooterManager _shooterManager;
		private bool _isPickBooster;
		private bool _isBreakBlockBooster;
		private bool _isShuffleBooster;
		[SerializeField] private GameObject _boosterShuffle;
		[SerializeField] private GameObject _boosterShufflePopUp;
		[SerializeField] private GameObject _boosterPick;
		[SerializeField] private GameObject _boosterPickPopUp;
		[SerializeField] private GameObject _boosterBreak;
		[SerializeField] private GameObject _boosterBreakPopUp;
		public Action<bool> isUseBoosterAction;
		public bool IsPickSelect { get => _isPickBooster; set => _isPickBooster = value; }
		public bool IsBreakBlockBooster { get => _isBreakBlockBooster; set => _isBreakBlockBooster = value; }
        public bool IsShuffleBooster { get => _isShuffleBooster; set => _isShuffleBooster = value; }

        private Tween _tweenBooster;
		private Tween _tweenCamera;
		private Action _action;
		void Awake()
		{
			_blockManager = BlockManager.Instance;
			_shooterManager = ShooterManager.Instance;
		}
		void Start()
		{
		}
		public void ShuffleShooterBooster(Action action)
		{
			if (_isShuffleBooster || _isBreakBlockBooster || _isPickBooster) return;
			_tweenBooster?.Kill();
			_tweenCamera?.Kill();
			action();
			_isShuffleBooster = true;

			_shooterManager.ShuffleShooter();
			_boosterShuffle.SetActive(true);

			_boosterShufflePopUp.transform.localScale = Vector3.zero;
			_tweenBooster = _boosterShufflePopUp.transform.DOScale(Vector3.one, 0.2f)
										.SetEase(Ease.Linear);
			_tweenCamera = Camera.main.transform.DOMove(Camera.main.transform.position + new Vector3(0, 0, -10), 0.4f).SetEase(Ease.Linear);
			StartCoroutine(WaitForBoosterUI(1f));
		}
		IEnumerator WaitForBoosterUI(float time)
		{
			yield return new WaitForSeconds(time);
			_tweenBooster?.Kill();
			_tweenCamera?.Kill();
			_tweenBooster = _boosterShufflePopUp.transform.DOScale(Vector3.zero, 0.2f)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					_boosterShuffle.SetActive(false);
				});
			_tweenCamera = Camera.main.transform.DOMove(Camera.main.transform.position + new Vector3(0, 0, 10), 0.4f)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					_isShuffleBooster = false;
				});
		}
		public void PickShooterBooster(Action action)
		{
			if (_isPickBooster || _isBreakBlockBooster || _isShuffleBooster) return;
			_tweenBooster?.Kill();
			_tweenCamera?.Kill();

			_action = action;
			_isPickBooster = true;

			_shooterManager.PickShooter();

			_boosterPick.SetActive(true);
			_boosterPickPopUp.transform.localScale = Vector3.zero;
			_tweenBooster = _boosterPickPopUp.transform.DOScale(Vector3.one, 0.2f)
										.SetEase(Ease.Linear);
			_tweenCamera = Camera.main.transform.DOMove(Camera.main.transform.position + new Vector3(0, 0, -10), 0.4f).SetEase(Ease.Linear);
		}
		public void UsePickBooster()
		{
			if (_isPickBooster)
			{
				_action();
				ResetPickBooster();
			}
		}
		public void ResetPickBooster()
		{
			_tweenBooster?.Kill();
			_tweenCamera?.Kill();

			_isPickBooster = false;
			_shooterManager.ResetPickShooter();
			_tweenBooster = _boosterPickPopUp.transform.DOScale(Vector3.zero, 0.2f)
						.SetEase(Ease.Linear)
						.OnComplete(() =>
						{
							_boosterPick.SetActive(false);
						});
			_tweenCamera = Camera.main.transform.DOMove(Camera.main.transform.position + new Vector3(0, 0, 10), 0.4f).SetEase(Ease.Linear);
		}
		public void BreakBlockBooster(Action action)
		{
			if (_isBreakBlockBooster || _isShuffleBooster || _isPickBooster) return;

			_tweenBooster?.Kill();
			_tweenCamera?.Kill();
			_blockManager.BreakBlockClassify();

			_action = action;
			_isBreakBlockBooster = true;

			_boosterBreak.SetActive(true);
			_boosterBreakPopUp.transform.localScale = Vector3.zero;
			_tweenBooster = _boosterBreakPopUp.transform.DOScale(Vector3.one, 0.2f)
										.SetEase(Ease.Linear);
			_tweenCamera = Camera.main.transform.DOMove(Camera.main.transform.position + new Vector3(0, 0, 3), 0.4f).SetEase(Ease.Linear);
		}
		public void UsePickBreak()
		{
			if (_isBreakBlockBooster)
			{
				_action();
				ResetBreakBooster();
			}
		}
		public void ResetBreakBooster()
		{
			_tweenBooster?.Kill();
			_tweenCamera?.Kill();
			_isBreakBlockBooster = false;

			_tweenBooster = _boosterBreakPopUp.transform.DOScale(Vector3.zero, 0.2f)
						.SetEase(Ease.Linear)
						.OnComplete(() =>
						{
							_boosterBreak.SetActive(false);
						});
			_tweenCamera = Camera.main.transform.DOMove(Camera.main.transform.position + new Vector3(0, 0, -3), 0.4f).SetEase(Ease.Linear);
		}
	}
}
