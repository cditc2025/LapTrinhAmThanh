using DG.Tweening;
using UnityEngine;

namespace KienChi
{
	public class GoldBlockDisplay : BlockDisplay
	{
		private float _xYScale;
		private Tween _tweenGoldBlockScale;
		private float _durationGoldBlockScale = 0.02f;
		private UIManager _uIManager;
		protected void Awake()
		{
			_uIManager = UIManager.Instance;
		}
		protected override void Start()
		{
			base.Start();

			_xYScale = model3D.localScale.x;
		}
		public void DisplayDestroyGoldBlock(int currentBlock, int totalBlock)
		{
			model3D.transform.localScale = new Vector3(_xYScale, 1, 1);
			_tweenGoldBlockScale?.Kill();
			_xYScale = 1 + (float)(totalBlock - currentBlock) / totalBlock * 0.3f;
			_tweenGoldBlockScale = model3D.transform.DOScale(new Vector3(_xYScale, _xYScale, 1), _durationGoldBlockScale)
				.SetEase(Ease.InOutBounce);

		}
		public void ChangeValueChest()
		{
			_uIManager.OnChestChanged();
		}
	}
}
