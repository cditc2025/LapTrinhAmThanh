using DG.Tweening;
using UnityEngine;

namespace KienChi
{
	public class BlockDisplay : MonoBehaviour
	{
		[SerializeField] protected Transform model3D;
		[SerializeField] private ParticleSystem _hitBlock;
		private Tween _tweenDestroy;
		private float _durationScale = 0.4f;
		protected virtual void Start()
		{

		}

		public void DisplayDestroyBlock()
		{
			_tweenDestroy?.Kill();
			model3D.transform.localScale = new Vector3(1, 1, 1);
			_tweenDestroy = model3D.transform.DOScale(0f, _durationScale)
				.SetEase(Ease.InOutBounce)
				.OnComplete(() =>
				{
					this.gameObject.SetActive(false);
					model3D.transform.localScale = new Vector3(1, 1, 1);
				});
		}
		public void EnableHitBlockVFX(Block block)
		{
			// Debug.Log(_hitBlock.GetComponent<ParticleSystemRenderer>());
			if (block.TypeBlock != Consts.GOLD_BLOCK)
			{
				var renderer = _hitBlock.GetComponent<ParticleSystemRenderer>();
				Material clonedMaterial = new Material(renderer.sharedMaterial);
				renderer.material = clonedMaterial;
				// Debug.Log(block.InxBlock);
				Color newColor = ColorManager.Instance.MatColorBlockList[block.ValueBlock].colorBlock.color;
				if (clonedMaterial.HasProperty("_Color"))
				{
					clonedMaterial.color = newColor;
				}
			}

			_hitBlock?.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			_hitBlock?.Play();
		}
		public Transform Model3D { get => model3D; set => model3D = value; }
	}
}
