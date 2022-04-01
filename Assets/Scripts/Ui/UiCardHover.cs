using System;
using UnityEngine.EventSystems;
using UnityEngine;


namespace GameUi
{
	public class UiCardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		private int subId;

		public Action onHoverEnter;
		public Action onHoverLeave;


		public void OnPointerEnter(PointerEventData eventData)
		{
			subId = transform.GetSiblingIndex();
			transform.SetAsLastSibling();
			onHoverEnter();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			transform.SetSiblingIndex(subId);
			onHoverLeave();
		}
	}
}
