using System.Threading.Tasks;
using UnityEngine;


namespace GameLogic
{
	public class GameSystem : MonoBehaviour
	{
		[SerializeField]
		private CardsLogic _logic;

		[SerializeField, Range(1, 10)]
		private int _minCardCount = 4;

		[SerializeField, Range(1, 10)]
		private int _maxCardCount = 6;


		private void Start()
		{
			InitGame(UnityEngine.Random.Range(_minCardCount, _maxCardCount + 1));
		}

		private async void InitGame(int count)
		{
			for (int i = 0; i < count; i++)
			{
				_logic.SpawnCard(SharedCode.ProjectUtils.GetRandomName(),
					$"<size=18>Info: </size>Card Description {count * i * i + i}");

				await Task.Delay(500);
			}
		}
	}
}