using DG.Tweening;
using UnityEngine;

namespace KienChi
{
	public class BlockMovement : MonoBehaviour
	{
		private float _speedMoveBlock = 2.5f;
		private Vector3 _targetPos;
		private bool _isMoving = false;
		private Tween _tween;
		void Start()
		{
			_targetPos = transform.position;
		}
		public void MoveTo(Vector3 addPos)
		{
			_tween.Kill();
			// transform.position = _targetPos;

			_targetPos = addPos;
			// _isMoving = true;
			float distance = Vector3.Distance(transform.position, addPos);
			distance = distance > 4 ? 4 : distance;
			float moveDuration = (distance) / _speedMoveBlock;
			// Debug.Log(moveDuration);
			_tween = transform.DOMove(_targetPos, moveDuration)
				.SetEase(Ease.OutBounce)
				.OnComplete(() => transform.position = _targetPos);
		}

		private void Update()
		{
			// if (_isMoving)
			// {
			// 	float distance = Vector3.Distance(transform.position, _targetPos);
			// 	transform.position = Vector3.MoveTowards(transform.position, _targetPos, distance / _speedMoveBlock * Time.deltaTime);
			// 	if (Vector3.Distance(transform.position, _targetPos) < 0.01f)
			// 	{
			// 		transform.position = _targetPos;
			// 		_isMoving = false;
			// 	}
			// }
		}
	}
}
