using System;
using UnityEngine;

using GameUi;


namespace GameLogic
{
	public class Card : MonoBehaviour
	{
		[SerializeField, Range(2, 25)]
		private int _minRandStat = 2;

		[SerializeField, Range(2, 25)]
		private int _maxRandStat = 25;

		private UiCard _cardUi;
		private UiCardAnimation _anim;
		private RectTransform _place;

		private Vector2 _dragOffset;

		private int _health;
		private int _attack;
		private int _mana;

		public bool isHover { get; private set; }
		public bool isDrag { get; private set; }
		public bool isOnTable { get; private set; }

		public Action<Card> onDestroy;
		public Action<Card> onStartDrag;
		public Action<Card> onEndDrag;

		public RectTransform rect => _cardUi.rect;
		public Vector2 pos2d => _cardUi.rect.anchoredPosition;
		public Vector2 pos => _cardUi.transform.position;

		public string description
		{
			get => _cardUi.description;
			set => _cardUi.description = value;
		}

		public string title
		{
			get => _cardUi.title;
			set => _cardUi.title = value;
		}

		public bool dead
		{
			get => _health <= 0;
		}

		public int health
		{
			get => _health;
			set
			{
				_health = value;
				_cardUi.SetHPValue(value);

				if (health <= 0)
				{
					DestroyCard();
				}
				else
				{
					_anim.Flash(Color.red);
				}
			}
		}

		public int attack
		{
			get => _attack;
			set
			{
				_attack = value;
				_cardUi.SetAttackValue(value);
				_anim.Flash(Color.yellow);
			}
		}

		public int mana
		{
			get => _mana;
			set
			{
				_mana = value;
				_cardUi.SetManaValue(value);
				_anim.Flash(Color.cyan);
			}
		}


		private void Awake()
		{
			_cardUi = Instantiate(UiMain.instance.cardPrefab, UiMain.instance.handsRect);
			_anim = _cardUi.GetComponent<UiCardAnimation>();
			UiCardHover hover = _cardUi.GetComponent<UiCardHover>();

			health = UnityEngine.Random.Range(_minRandStat, _maxRandStat + 1);
			attack = UnityEngine.Random.Range(_minRandStat, _maxRandStat + 1);
			mana = UnityEngine.Random.Range(_minRandStat, _maxRandStat + 1);

			hover.onHoverEnter = OnCardHoverEnter;
			hover.onHoverLeave = OnCardHoverLeave;
		}

		private void DestroyCard()
		{
			onDestroy?.Invoke(this);
			_cardUi.DestroyUiCard();

			Destroy(gameObject);
		}

		private void Update()
		{
			RectTransform hands = UiMain.instance.handsRect;
			Vector3 mousePos = Input.mousePosition;
			bool mouse = Input.GetMouseButton(0);

			if (mouse && isHover)
			{
				if (!isDrag)
				{
					OnCardDrag();
				}

				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hands, mousePos, null, out Vector2 pos))
				{
					_cardUi.rect.anchoredPosition = pos - _dragOffset;
				}
			}
			else if (isDrag)
			{
				// End Drag, set to true if desk rect contains mouse position
				OnCardDragEnd(RectTransformUtility.RectangleContainsScreenPoint(UiMain.instance.desk, mousePos, null));
			}
		}

		private void OnCardHoverEnter()
		{
			isHover = true;
			_anim.StartGlowing();

			if (!isDrag && !isOnTable)
			{
				_cardUi.SetOffset(new Vector2(0.0f, 80.0f));
			}
		}

		private void OnCardHoverLeave()
		{
			isHover = false;
			_anim.StopGlowing();

			if (!isDrag && !isOnTable)
			{
				_cardUi.SetOffset(Vector2.zero);
			}
		}

		private void OnCardDrag()
		{
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_cardUi.rect,
				Input.mousePosition, null, out Vector2 pos))
			{
				_dragOffset = pos;
			}

			_cardUi.SetOffset(Vector2.zero);
			_anim.StartFloating();
			isDrag = true;

			onStartDrag?.Invoke(this);
		}

		private void OnCardDragEnd(bool onDesk)
		{
			isOnTable = onDesk;
			_cardUi.SetActive(!onDesk);

			_anim.StopFloating();
			isDrag = false;

			onEndDrag?.Invoke(this);
		}

		public void SetPlaceholder(RectTransform rt)
		{
			_place = rt;
		}

		public void Return()
		{
			SetPositionFromPlace(_place);
		}

		public void SetRotation(float ang, LeanTweenType tweenType = LeanTweenType.easeOutBounce)
		{
			LeanTween.rotateZ(_cardUi.gameObject, ang, 0.4f).setEase(tweenType);
		}

		public void SetPositionAndRotation(Vector2 pos, float ang, LeanTweenType tweenType = LeanTweenType.easeOutBounce)
		{
			LeanTween.moveLocal(_cardUi.gameObject, pos, 0.5f).setEase(tweenType);
			LeanTween.rotateZ(_cardUi.gameObject, ang, 0.4f).setEase(tweenType);
		}

		public void SetPositionFromPlace(RectTransform rt, LeanTweenType tweenType = LeanTweenType.easeOutBounce)
		{
			LeanTween.moveLocal(_cardUi.gameObject, rt.anchoredPosition, 0.5f).setEase(tweenType);
			LeanTween.rotateZ(_cardUi.gameObject, rt.eulerAngles.z, 0.4f).setEase(tweenType);
		}

		public void SetAvatarImage(Texture2D tex) => _cardUi.SetAvatarImage(tex);

		public static Vector2Int GetImageSize() => UiMain.instance.cardPrefab.GetAvatarImageSize();
	}
}