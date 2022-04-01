using UnityEngine.UI;
using UnityEngine;


namespace GameUi
{
	public class UiButtonAnimation : MonoBehaviour
	{
		private Button _btn;


		private void OnEnable()
		{
			_btn = GetComponent<Button>();

			if (_btn != null)
			{
				_btn.onClick.AddListener(PlayButtonAnim);
			}
		}

		private void OnDisable()
		{
			if (_btn != null)
			{
				_btn.onClick.RemoveListener(PlayButtonAnim);
			}
		}

		private void PlayButtonAnim()
		{
			_btn.gameObject.LeanCancel();
			_btn.gameObject.LeanScale(new Vector3(1.3f, 1.3f, 1.0f), 0.1f).setFrom(Vector3.one).setLoopPingPong(1);
		}
	}
}
