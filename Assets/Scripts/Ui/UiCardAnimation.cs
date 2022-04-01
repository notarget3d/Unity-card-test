using UnityEngine.UI;
using UnityEngine;


namespace GameUi
{
	public class UiCardAnimation : MonoBehaviour
	{
		[SerializeField]
		private RawImage _imgShadow;

		[SerializeField]
		private RectTransform _rtCard;

		[SerializeField]
		private Color _glowColor;

		private int _glowId;
		private int _floatRotateId;
		private int _floatScaleId;

		private bool _isGlowing = false;
		private bool _isFloating = false;
		private Color _defaultColor;


		private void Awake()
		{
			_defaultColor = _imgShadow.color;
		}

		public void Flash(Color color)
		{
			if (!_isGlowing)
			{
				LeanTween.color(_imgShadow.rectTransform, color,
					0.3f).setEase(LeanTweenType.easeInOutCubic).setLoopPingPong(1);
			}
		}

		public void StartFloating()
		{
			if (!_isFloating)
			{
				_isFloating = true;

				float ang = _rtCard.eulerAngles.z - 8.0f;

				_rtCard.gameObject.LeanCancel(_floatScaleId);

				_floatScaleId = _rtCard.gameObject.LeanScale(new Vector3(1.1f, 1.1f, 1.0f), 0.2f).setEase(LeanTweenType.easeInCubic).id;
				_floatRotateId = _rtCard.gameObject.LeanRotateZ(ang, 2.0f).setEaseInOutSine().setLoopPingPong().id;
			}
		}

		public void StopFloating()
		{
			if (_isFloating)
			{
				_isFloating = false;

				_rtCard.gameObject.LeanCancel(_floatRotateId);
				_rtCard.gameObject.LeanCancel(_floatScaleId);

				_floatScaleId = _rtCard.gameObject.LeanScale(Vector3.one, 0.3f).setEase(LeanTweenType.easeOutCubic).id;
			}
		}

		public void StartGlowing()
		{
			if (!_isGlowing)
			{
				_imgShadow.gameObject.LeanCancel();

				Color initColor = _glowColor * 0.75f;
				initColor.a = _glowColor.a;

				LeanTween.color(_imgShadow.rectTransform, initColor, 0.5f).setEase(LeanTweenType.easeInCubic);
				LeanTween.scale(_imgShadow.gameObject, new Vector3(1.08f, 1.08f, 1.0f), 0.5f).setEase(LeanTweenType.easeInCubic).setOnComplete(() =>
				{
					LeanTween.scale(_imgShadow.gameObject, new Vector3(1.11f, 1.11f, 1.0f), 0.80f).setEase(LeanTweenType.easeInCubic).setLoopPingPong();
					LeanTween.color(_imgShadow.rectTransform, _glowColor, 0.80f).setEase(LeanTweenType.easeInCubic).setLoopPingPong();
				});

				_isGlowing = true;
			}
		}

		public void StopGlowing()
		{
			if (_isGlowing)
			{
				_imgShadow.gameObject.LeanCancel();
				LeanTween.scale(_imgShadow.gameObject, Vector3.one, 1.0f).setEase(LeanTweenType.easeOutCubic);
				LeanTween.color(_imgShadow.rectTransform, _defaultColor, 1.0f).setEase(LeanTweenType.easeOutCubic);

				_isGlowing = false;
			}
		}
	}
}