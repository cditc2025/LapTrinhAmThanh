using UnityEngine;
using UnityEngine.EventSystems;

namespace KienChi
{
	public class InputManager : Singleton<InputManager>
	{
		private BoosterManager _boosterManager;
		private GameManager _gameManager;
		void Awake()
		{
			_boosterManager = BoosterManager.Instance;
            _gameManager = GameManager.Instance;

        }
		void Update()
		{
			if (Input.GetMouseButtonDown(0) && (!UIEvent.instance.IsPointerOverUIObject() || !EventSystem.current.IsPointerOverGameObject()) && !_gameManager.isFinishLevel)
			{
				SelectedShooter();
				SelectedBlock();
				SelectedSlot();
			}
		}
		private void SelectedShooter()
		{
			GameObject col = RaycastFromMouse(LayerMask.GetMask(Consts.LAYER_SHOOTER));
			if (col && col.TryGetComponent<Shooter>(out Shooter shooter))
			{
				shooter.CheckShooterSelf();
			}
		}
		private void SelectedBlock()
		{
			if (_boosterManager.IsBreakBlockBooster)
			{
				GameObject col = RaycastFromMouse(LayerMask.GetMask(Consts.LAYER_BLOCK));
				if (col && col.TryGetComponent<Block>(out Block block))
				{
					block.SelectBlockBooster();
				}
			}
		}
		private void SelectedSlot()
		{
			GameObject col = RaycastFromMouse(LayerMask.GetMask(Consts.LAYER_SLOT_ADS));
			if (col && col.TryGetComponent<Slot>(out Slot slot))
			{
				slot.AddSlot();

			}
		}
		public GameObject RaycastFromMouse(LayerMask layerMask)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			// Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			{
				return hit.collider.gameObject;
			}
			return null;
		}
	}
}
