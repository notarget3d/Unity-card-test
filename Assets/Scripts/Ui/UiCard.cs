using UnityEngine.UI;
using UnityEngine;


namespace GameUi
{
	public class UiCard : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _rtCard;	// Root rect of card ui

		[SerializeField]
		private RectTransform _offsetRect;

		[SerializeField]
		private RectTransform _dragRect;

		[SerializeField]
		private UiTextValueAnimate _txtHP;

		[SerializeField]
		private UiTextValueAnimate _txtMana;

		[SerializeField]
		private UiTextValueAnimate _txtAttack;

		[SerializeField]
		private Text _txtTitle;

		[SerializeField]
		private Text _txtDescription;

		[SerializeField]
		private RawImage _imgAvatar;

		[SerializeField]
		private CanvasGroup _group;

		public RectTransform rect => _rtCard;
		public RectTransform dragRect => _dragRect;


		public string title
		{
			get => _txtTitle.text;
			set => _txtTitle.text = value;
		}

		public string description
		{
			get => _txtDescription.text;
			set => _txtDescription.text = value;
		}

		public void SetHPValue(int val)
		{
			_txtHP.value = val;
		}

		public void SetManaValue(int val)
		{
			_txtMana.value = val;
		}

		public void SetAttackValue(int val)
		{
			_txtAttack.value = val;
		}

		public void SetAvatarImage(Texture2D tex)
		{
			_imgAvatar.texture = tex;
		}

		public void SetActive(bool active)
		{
			_group.interactable = active;
			_group.blocksRaycasts = active;
		}

		public void SetOffset(Vector2 offset)
		{
			_offsetRect.LeanCancel();
			_offsetRect.LeanMoveLocal(offset, 0.3f).setEase(LeanTweenType.easeInOutCubic);
		}

		public void DestroyUiCard()
		{
			gameObject.LeanScale(Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(() =>
			{
				Destroy(gameObject);
			});
		}

		public Vector2Int GetAvatarImageSize()
		{
			Vector2 size = _imgAvatar.rectTransform.sizeDelta;
			return new Vector2Int((int)size.x, (int)size.y);
		}
	}
}