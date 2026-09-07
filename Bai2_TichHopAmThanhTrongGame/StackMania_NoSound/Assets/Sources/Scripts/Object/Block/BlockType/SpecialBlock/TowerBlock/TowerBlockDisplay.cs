using DG.Tweening;
using TMPro;
using UnityEngine;

namespace KienChi
{
	public class TowerBlockDisplay : BlockDisplay
	{
		[SerializeField] private TMP_Text _textBlock;
		[SerializeField] private Renderer _renderBlock;
		[SerializeField] private Renderer[] _renderBlockInSide;
		private float _bounceDuration = 0.3f;
		public void SetTextBlock(string text)
		{
			if (_textBlock != null)
			{
				_textBlock.text = text;
			}
		}
		public void SetMatSelectedBlock(DataColorBlock color)
		{
			Material[] newBlockMaterials = new Material[_renderBlock.materials.Length];
			Material[] newBlockInSideMaterials = new Material[_renderBlockInSide[0].materials.Length];

			newBlockMaterials[0] = color.colorBlock;
			newBlockInSideMaterials[0] = color.colorBlock;
			newBlockInSideMaterials[1] = color.colorBlock;

			_renderBlock.materials = newBlockMaterials;

			for (int i = 0; i < _renderBlockInSide.Length; i++)
			{
				_renderBlockInSide[i].materials = newBlockInSideMaterials;
			}
		}
		public void DisplayDestroyBlockInSide(float percent)
		{
			for (int i = 1; i <= _renderBlockInSide.Length; i++)
			{
				// Debug.Log("Percent: " + percent + " i: " + ((float)i / (float)_renderBlockInSide.Length));
				if (percent >= ((float)i / (float)_renderBlockInSide.Length))
				{
					GameObject objectBlockInSide = _renderBlockInSide[i - 1].gameObject;
					Vector3 originalPos = objectBlockInSide.transform.localPosition;

					objectBlockInSide.transform.DOScale(Vector3.zero, _bounceDuration)
						.SetEase(Ease.OutBounce)
						.OnComplete(() =>
						{
							objectBlockInSide.SetActive(false);
						});
				}
			}
		}
	}
}
