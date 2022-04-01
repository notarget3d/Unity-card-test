using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic
{
	public class CardsLogic : MonoBehaviour
	{
		[SerializeField]
		private CardsPlaceholder _placesInstance;

		[SerializeField]
		private Card _cardPrefab;

		private List<Card> _cardsInHand = new List<Card>();

		public List<Card> cardsInHand => _cardsInHand;
		public int cardCount => _cardsInHand.Count;

		public Action<Card> onCardAdd;
		public Action<Card, int> onCardRemove;	// Add removed Card index


		private void OnDestroy()
		{
			// Remove callbacks to avoid errors
			foreach (Card card in _cardsInHand)
			{
				card.onDestroy = null;
				card.onEndDrag = null;
				card.onStartDrag = null;
			}
		}

		// Do this every time when card count changed
		// to recalculate hand arc positions
		private void OnCardCountChange()
		{
			RectTransform[] places = _placesInstance.InitPlaceholder(cardCount);

			for (int i = 0; i < _cardsInHand.Count; i++)
			{
				Card card = _cardsInHand[i];
				RectTransform place = places[i];

				card.SetPlaceholder(place);
				card.SetPositionFromPlace(place);
			}
		}

		public void SpawnCard(string title, string description)
		{
			Card card = Instantiate(_cardPrefab);
			card.title = title;
			card.description = description;
			card.SetAvatarImage(null);

			card.onDestroy = OnCardRemoved;
			card.onStartDrag = OnCardDragStart;
			card.onEndDrag = OnCardDragEnd;

			_cardsInHand.Add(card);

			// Since loading images takes some time i don't
			// want to wait until all of them are loaded
			LoadCardAvatar(card);
			OnCardCountChange();

			onCardAdd?.Invoke(card);
		}

		private async void LoadCardAvatar(Card card)
		{
			Vector2Int size = Card.GetImageSize();
			Texture2D tex = null;
			int retry = 2;

			// Try 2 times in case of network error
			while (retry > 0)
			{
				tex = await SharedCode.ProjectUtils.GetRandomTexture(size);

				if (tex != null)
				{
					break;
				}
			}

			// Make sure card if still 'alive'
			if (card != null)
			{
				card.SetAvatarImage(tex);
			}
		}

		private void OnCardDragStart(Card card)
		{
		}

		private void OnCardDragEnd(Card card)
		{
			if (!card.isOnTable)
			{
				// Return to hands
				card.Return();
			}
			else
			{
				// The card was placed on the table, remove from hands
				// Add logic for when the card is on table
				OnCardRemoved(card);

				// For now move the card to the center of table and destroy after few seconds
				Vector2 table = card.pos2d;
				Vector2 screen = RectTransformUtility.WorldToScreenPoint(null, GameUi.UiMain.instance.tableDropTarget.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(GameUi.UiMain.instance.handsRect, screen, null, out table);

				card.SetPositionAndRotation(new Vector2(card.pos2d.x, table.y), 0.0f, LeanTweenType.easeInOutExpo);
				StartCoroutine(DelayCardDestroy(card, 5.0f));
			}
		}

		private void OnCardRemoved(Card card)
		{
			int remIndex = _cardsInHand.FindIndex(x => x == card);

			if (remIndex != -1)
			{
				_cardsInHand.RemoveAt(remIndex);
				OnCardCountChange();

				onCardRemove?.Invoke(card, remIndex);
			}
		}

		private IEnumerator DelayCardDestroy(Card card, float delay = 5.0f)
		{
			yield return new WaitForSeconds(delay);
			card.health = 0;
		}
	}
}