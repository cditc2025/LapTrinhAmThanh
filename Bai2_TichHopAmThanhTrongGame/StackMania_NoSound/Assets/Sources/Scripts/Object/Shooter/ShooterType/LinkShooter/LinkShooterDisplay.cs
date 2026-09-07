using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;

namespace KienChi
{
	public class LinkShooterDisplay : ShooterDisplay
	{
		[SerializeField] private Rope _rope;
		[SerializeField] private RopeMesh _ropeMesh;
		private Transform _linkTarget;
		protected override void Start()
		{
			base.Start();

		}
		public void Wiring(Transform target)
		{
			this._linkTarget = target;
			_rope.EndPoint = target;

			_ropeMesh.InitializeComponents();
			_ropeMesh.SubscribeToRopeEvents(); 
		}
		public void RopeDisable()
		{
			_rope.enabled = false;
		}
	}
}
