using DG.Tweening;
using UnityEngine;

namespace KienChi
{
	public class ShooterMovement : MonoBehaviour
	{
		private float _speed = 10f;
		private float _arriveThreshold = 0.01f;
		private bool _isMoving = true;
		private Vector3 _target;
		private Vector3 _directtion;
		private float _rotationSpeed = 10f;
		private bool _isInSlot;
		Tween _moveTween;
		public void InitMove(Vector3 target)
		{
			this._target = target;

			_isMoving = true;
		}
		public void Idle()
		{
			_moveTween?.Kill();

			// if (!_isInSlot)
			// transform.rotation = Quaternion.Euler(0, 0, 0);
			// else
			_moveTween = transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast);
		}
		public void Shooting(Vector3 target)
		{
			_moveTween?.Kill();

			Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
		}
		public void Moving()
		{
			_moveTween?.Kill();

			if (!_isMoving) return;

			_directtion = _target - transform.position;
			Quaternion targetRotation = Quaternion.LookRotation(_directtion);
			transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

			if (Vector3.Distance(transform.position, _target) < _arriveThreshold)
			{
				_isMoving = false;

				transform.position = _target;
				_moveTween = transform.DORotate(new Vector3(0, -30, 0), 0.5f, RotateMode.Fast);
			}
		}
		public void SetWaitingShot()
		{
			_isInSlot = true;
		}
		public bool CheckMoving()
		{
			return _isMoving;
		}
	}
}