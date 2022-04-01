using UnityEngine.UI;
using UnityEngine;


namespace GameUi
{
	public class UiTextValueAnimate : MonoBehaviour
	{
		[SerializeField]
		private float _animTime = 2.0f;

		[SerializeField]
		private float _animScale = 1.4f;

		private Text _txt;

		private int _valueTarget;
		private float _value;
		int _animId;

		public int value
		{
			get => _valueTarget;
			set
			{
				_valueTarget = value;
				_animId = LeanTween.value(_value,
					_valueTarget, _animTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate(OnUpdateValue).id;
				_txt.transform.localScale = Vector3.one;
				_txt.gameObject.LeanScale(new Vector3(_animScale, _animScale, 1.0f), 0.2f).setLoopPingPong(1);
			}
		}


		private void Awake()
		{
			_txt = GetComponent<Text>();
		}

		private void OnDestroy()
		{
			LeanTween.cancel(_animId);
		}

		private void OnUpdateValue(float val)
		{
			_value = val;
			_txt.text = Mathf.CeilToInt(_value).ToString();
		}
	}
}
