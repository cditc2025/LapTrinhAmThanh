using UnityEngine;

namespace KienChi
{
	public class ShooterAnimator : MonoBehaviour
	{
		[SerializeField] private Animator anim;
		private const string MOVING_ANIM = "isMoving";
		private const string SHOOTING_ANIM = "isShooting";
		public void ChangeAnim(string animName)
		{
			switch (animName)
			{
				case Consts.MOVING_ANIM:
					MovingAnim();
					break;
				case Consts.IDLE_ANIM:
					IdleAnim();
					break;
				case Consts.SHOOTING_ANIM:
					ShootAnim();
					break;
				default:
					Debug.Log("KC_ERROR: No Anim");
					break;
			}
		}
		private void MovingAnim()
		{
			anim?.SetBool(MOVING_ANIM, true);
		}
		private void IdleAnim()
		{
			anim?.SetBool(MOVING_ANIM, false);
		}
		private void ShootAnim()
		{
			anim?.SetTrigger(SHOOTING_ANIM);
		}
	}
}
