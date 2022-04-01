using UnityEngine;


namespace GameLogic
{
	public class CardsPlaceholder : MonoBehaviour
	{
		[SerializeField]
		private Transform[] _pathCurve;

		[SerializeField]
		private RectTransform _parent;

		[SerializeField]
		private float _anglesOffset = 4.0f;

		private LTSpline _curve;

		private RectTransform[] _placeholders = new RectTransform[0];


		private void Awake()
		{
			// Create curve
			Vector3[] path = new Vector3[_pathCurve.Length];

			for (int i = 0; i < _pathCurve.Length; i++)
			{
				path[i] = _pathCurve[i].position;
			}

			_curve = new LTSpline(path);
		}

		public RectTransform[] InitPlaceholder(int count)
		{
			for (int i = 0; i < _placeholders.Length; i++)
			{
				// Clear previous rectangles
				Destroy(_placeholders[i].gameObject);
			}

			float ratioInc = 1.0f / (float)count;	// curve ration increment
			float ratio = ratioInc * 0.5f;			// ratio starts at small offset
			_placeholders = new RectTransform[count];

			// Place transforms along curve path
			for (int i = 0; i < count; i++)
			{
				GameObject place = new GameObject($"Placeholder_{i}");
				RectTransform rt = place.AddComponent<RectTransform>();
				rt.SetParent(_parent);
				_curve.place2d(rt, ratio);
				rt.localScale = Vector3.one;
				rt.sizeDelta = new Vector2(300.0f, 450.0f);

				// Add angle offset
				float curAng = rt.eulerAngles.z;
				rt.eulerAngles = new Vector3(0.0f, 0.0f, curAng + _anglesOffset);
				_placeholders[i] = rt;
				ratio += ratioInc;
			}

			return _placeholders;
		}
	}
}