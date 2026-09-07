using TMPro;
using UnityEngine;

namespace KienChi
{
	public class ShooterDisplay : MonoBehaviour
	{
		[SerializeField] private SkinnedMeshRenderer _renderShooter;
		[SerializeField] private ParticleSystem _smokeEffect;
		[SerializeField] private ParticleSystem _shooterMuzzle;
		[SerializeField] private Transform _bulletTranform;
		[SerializeField] private Transform _model3D;
		[SerializeField] private Transform _bodyObj;
		[SerializeField] private TMP_Text _projectileCountText;
		[SerializeField] private Outline _outline;
		private bool _isSelect;

		protected virtual void Start()
		{
		}
		public void SetProjectileCount(int count, Color32 color)
		{
			_projectileCountText.color = color;
			_projectileCountText.text = count.ToString();
		}
		public void SetProjectTileActive(bool isActive)
		{
			_projectileCountText.enabled = isActive;
		}
		public void SetMatSelectedShooter(DataColorShooter color, Shooter shooter)
		{
			// Debug.Log(shooter.TypeShooter+ " " + shooter.YAxis);
			Material[] newMaterials = new Material[_renderShooter.materials.Length];
			for (int i = 0; i < _renderShooter.materials.Length; i++)
			{
				newMaterials[i] = _renderShooter.materials[i];
			}

			if (shooter.TypeShooter == Consts.SECRET_SHOOTER && shooter.YAxis != 0)
			{
				newMaterials[1] = ColorManager.Instance.MatColorSecretShooter;
				newMaterials[2] = ColorManager.Instance.MatColorSecretShooter;
			}
			else if (shooter.TypeShooter == Consts.SIMPLE_BLOCK
					|| shooter.TypeShooter == Consts.LINK_SHOOTER
					|| (shooter.TypeShooter == Consts.SECRET_SHOOTER && shooter.YAxis == 0))
			{
				newMaterials[1] = color.colorTopShooter;
				newMaterials[2] = color.colorShooter;
			}
			else if (shooter.TypeShooter == Consts.DOU_SIMPLE_SHOOTER)
			{
				newMaterials[1] = color.colorTopShooter;
				newMaterials[0] = color.colorShooter;
			}
			_renderShooter.materials = newMaterials;
		}
		public void EnableShotVFX()
		{
			_shooterMuzzle?.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			_shooterMuzzle?.Play();
		}
		public void SetStateSmokeEffect(bool isActive)
		{
			if (isActive)
			{
				_smokeEffect.Play();
			}
			else
			{
				_smokeEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
		}
		public void EnableOutLine(bool state)
		{
			// Debug.Log(state);
			_outline.enabled = state;

			if (!state)
			{
				Material[] newMaterials = new Material[4];

				for (int i = 0; i < 4; i++)
				{
					newMaterials[i] = _renderShooter.materials[i];
				}
				_renderShooter.materials = newMaterials;
			}
			else
			{
				_isSelect = true;
				Material[] newMaterials = new Material[_renderShooter.materials.Length];

				for (int i = 0; i < _renderShooter.materials.Length; i++)
				{
					newMaterials[i] = _renderShooter.materials[i];
				}
				newMaterials[_renderShooter.materials.Length - 1].SetColor("_OutlineColor", Color.black);
				_renderShooter.materials = newMaterials;
			}
		}
		public Transform BulletTranform { get => _bulletTranform; set => _bulletTranform = value; }
		public Transform Model3D { get => _model3D; set => _model3D = value; }
		public Transform BodyObj { get => _bodyObj; set => _bodyObj = value; }
		public bool IsSelect { get => _isSelect; set => _isSelect = value; }
	}
}
