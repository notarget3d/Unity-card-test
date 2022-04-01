using System.Threading.Tasks;
using UnityEngine;


namespace GameLogic
{
	public class CardsEffects : MonoBehaviour
	{
		private enum RandomValueType
		{
			HEALTH, MANA, ATTACK, MAX
		}

		[SerializeField]
		private CardsLogic _logicInstance;

		[SerializeField, Range(1, 15)]
		private int _maxSpawnCard = 9;

		private bool _isSeqRunning;		// Avoid button press 2 time while seuence is running
		private int _cardCount => _logicInstance.cardCount;
		private int _randomCardIndex = -1;  // Used for random button sequence


		private void Awake()
		{
			GameUi.UiMain.instance.onRandomPress = OnPressRandonButton;
			GameUi.UiMain.instance.onAddPress = OnPressAdd;
			GameUi.UiMain.instance.onSequencePress = OnPressSequence;

			_logicInstance.onCardRemove += OnCardRemove;
		}

		private void OnCardRemove(Card card, int remIndex)
		{
			// To make sure random index pointing to valid card array index
			if (_randomCardIndex >= remIndex)
			{
				_randomCardIndex--;
			}

			if (_cardCount == 0)
			{
				// We removed last card, set to -1 to avoid errors
				_randomCardIndex = -1;
			}
		}

		private void OnPressAdd()
		{
			// Dont spawn too much
			if (_cardCount < _maxSpawnCard)
			{
				_logicInstance.SpawnCard(SharedCode.ProjectUtils.GetRandomName(),
					$"<size=18>Info: </size>New Card Description {_cardCount * _cardCount + 1}");
			}
		}

		private async void OnPressSequence()
		{
			if (!_isSeqRunning)
			{
				_isSeqRunning = true;

				for (int i = 0; i < _cardCount; i++)
				{
					Card card = _logicInstance.cardsInHand[i];

					if (card != null)
					{
						ApplyRandomEffect(card);
						if (card.dead)
						{
							i--;
						}

						await Task.Delay(400);
					}
				}

				_isSeqRunning = false;
			}
		}

		private void OnPressRandonButton()
		{
			if (!_isSeqRunning)
			{
				_randomCardIndex++;

				if (_randomCardIndex >= _cardCount)
				{
					_randomCardIndex = _cardCount == 0 ? -1 : 0;
				}

				if (_randomCardIndex != -1)
				{
					Card card = _logicInstance.cardsInHand[_randomCardIndex];

					ApplyRandomEffect(card);
				}
			}
		}

		private void ApplyRandomEffect(Card card)
		{
			// randomly change one randomly selected value
			int value = Random.Range(-2, 10);

			switch ((RandomValueType)Random.Range(0, (int)RandomValueType.MAX))
			{
				case RandomValueType.HEALTH:
					card.health = value;
					break;
				case RandomValueType.MANA:
					card.mana = value;
					break;
				case RandomValueType.ATTACK:
					card.attack = value;
					break;
			}
		}
	}
}