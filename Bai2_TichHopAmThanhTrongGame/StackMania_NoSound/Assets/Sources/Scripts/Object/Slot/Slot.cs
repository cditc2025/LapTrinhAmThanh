using TMPro;
using UnityEngine;

namespace KienChi
{
	public class Slot : MonoBehaviour
	{
		[SerializeField] private Transform _slotAds;
		[SerializeField] private Transform _idlePos;
		[SerializeField] private TMP_Text _textCost;
        [SerializeField] private SpriteRenderer _coinIcon;
        private SlotManager _slotManager;
		private Shooter _shooterInSlot;
		private int _inxSLot;
		private int _goldSlotCost = 50;
		void Awake()
		{
			_slotManager = SlotManager.Instance;
            PlayerResourceController.instance.OnGoldChanged += OnGoldChanged;

        }

        private void OnDestroy()
        {
			PlayerResourceController.instance.OnGoldChanged -= OnGoldChanged;
        }

		public void OnGoldChanged(int gold)
		{
			SetGoldSlotCost();

        }
        void Start()
		{
			_goldSlotCost = SlotManager.Instance.ValueGoldInx == 0 ? 600: 600;
			SetGoldSlotCost();

		}
		private void SetGoldSlotCost()
		{
			if (_textCost != null)
			{
				_textCost.text = _goldSlotCost.ToString();
			}

			if(PlayerResourceController.instance.gold < 600)
			{
                _coinIcon.color = new Color32(150, 150, 150, 255);
                _textCost.color = new Color32(30, 98, 10, 255);
            } else
			{
                _coinIcon.color = new Color32(255, 255, 255, 255);
                _textCost.color = new Color32(255, 192, 19, 255);
            }
			

        }
		public void SetStateSlotAds(bool isActive)
		{
			_slotAds.gameObject.SetActive(isActive);
		}
		public void SetShooterInSlot(Shooter shooter)
		{
			this._shooterInSlot = shooter;
		}
		public Vector3 GetPosSlot()
		{
			return _idlePos.position;
		}
		public void AddSlot(bool useGold = true)
		{
			// PlayerResourceController.instance.GainGold(1000);
			if (useGold)
            {
				if (!PlayerResourceController.instance.UseGold(_goldSlotCost)) return;

				GameLevelManager.instance.UpdateAddSlotCount();
			}
				
			this.gameObject.layer = LayerMask.NameToLayer(Consts.LAYER_DEFAULT);

			SlotManager.Instance.ValueGoldInx++;

			SetStateSlotAds(false);

			_slotManager.AddSlot(this);

		}
		public Shooter ShooterInSlot { get => _shooterInSlot; set => _shooterInSlot = value; }
		public int InxSLot { get => _inxSLot; set => _inxSLot = value; }
	}
}
