using System;
using UnityEngine.UI;
using UnityEngine;


namespace GameUi
{
	public class UiMain : MonoBehaviour
	{
		[SerializeField]
		private UiCard _cardPrefab;
		public UiCard cardPrefab => _cardPrefab;

		[SerializeField]
		private RectTransform _hands;
		public RectTransform handsRect => _hands;

		[SerializeField]
		private RectTransform _desk;
		public RectTransform desk => _desk;

		[SerializeField]
		private RectTransform _tableDropTarget;		// Move cards on center of table when droppped
		public RectTransform tableDropTarget => _tableDropTarget;

		[SerializeField]
		private Button _randomButton;

		[SerializeField]
		private Button _sequenceButton;

		[SerializeField]
		private Button _addButton;

		public Action onSequencePress;
		public Action onRandomPress;
		public Action onAddPress;

		public static UiMain instance;


		private void Awake()
		{
			if (instance != null)
			{
				Destroy(this);
				return;
			}

			instance = this;
			DontDestroyOnLoad(this);

			_sequenceButton.onClick.AddListener(() =>
			{
				onSequencePress?.Invoke();
			});
			_randomButton.onClick.AddListener(() =>
			{
				onRandomPress?.Invoke();
			});
			_addButton.onClick.AddListener(() =>
			{
				onAddPress?.Invoke();
			});
		}
	}
}