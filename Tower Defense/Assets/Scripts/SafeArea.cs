using System;
using UnityEditor;
using UnityEngine;

	[RequireComponent(typeof(RectTransform))]
	public class SafeArea : MonoBehaviour
	{
		public enum VerticalOptions
		{
			FullHeight,
			TopOnly,
			BottomOnly,
			None
		}

		public enum HorizontalOptions
		{
			FullWidth,
			LeftOnly,
			RightOnly,
			None
		}

		public enum BehaviourOptions
		{
			/// <summary>
			/// will lower height/width
			/// </summary>
			ScaleDown,

			/// <summary>
			/// will adjust position
			/// </summary>
			Move,

			/// <summary>
			/// will increase heigh/width
			/// </summary>
			ScaleUp
		}

		[Header("Vertical Setup")]
		[SerializeField]
		private VerticalOptions _verticalOption = VerticalOptions.None;

		internal VerticalOptions VerticalOption => _verticalOption;

		//[SerializeField, ShowIf(nameof(_verticalOption), ShowIfAttribute.CompareType.NotEqual, VerticalOptions.FullHeight, true)]
		private BehaviourOptions _verticalBehaviour = BehaviourOptions.ScaleDown;

		private BehaviourOptions VerticalBehaviour => _verticalBehaviour;

		[SerializeField, Range(0f, 1f)]
		private float _verticalInfluence = 1;

		private float VerticalInfluence => _verticalInfluence;

		[Header("Horizontal Setup")]
		[SerializeField]
		private HorizontalOptions _horizontalOption = HorizontalOptions.None;

		internal HorizontalOptions HorizontalOption => _horizontalOption;

		//[SerializeField, ShowIf(nameof(_horizontalOption), ShowIfAttribute.CompareType.NotEqual, HorizontalOptions.FullWidth, true)]
		private BehaviourOptions _horizontalBehaviour = BehaviourOptions.ScaleDown;

		private BehaviourOptions HorizontalBehaviour => _horizontalBehaviour;

		[SerializeField, Range(0f, 1f)]
		private float _horizontalInfluence = 1;

		private float HorizontalInfluence => _horizontalInfluence;

		internal RectTransform Rect { get; private set; }

		internal void Init()
		{
			Rect = GetComponent<RectTransform>();
		}

		private void Start()
		{
			ForceUpdate();
		}

		internal void FixRectTransform(bool verticalFull, bool horizontalFull)
		{
			Vector2 sDelta = Rect.sizeDelta;
			Vector2 aMin = Rect.anchorMin;
			Vector2 aMax = Rect.anchorMax;
			Vector2 pivot = Rect.pivot;
			Vector2 offMin = Rect.offsetMin;
			Vector2 offMax = Rect.offsetMax;
			Vector3 pos = Rect.position;

			Rect.pivot = new Vector2(horizontalFull ? 0 : pivot.x, verticalFull ? 0 : pivot.y);
			Rect.position = new Vector3(horizontalFull ? 0 : pos.x, verticalFull ? 0 : pos.y, pos.z);
			Rect.anchorMin = new Vector2(horizontalFull ? 0 : aMin.x, verticalFull ? 0 : aMin.y);
			Rect.anchorMax = new Vector2(horizontalFull ? 1 : aMax.x, verticalFull ? 1 : aMax.y);
			Rect.sizeDelta = new Vector2(horizontalFull ? 0 : sDelta.x, verticalFull ? 0 : sDelta.y);
			Rect.offsetMin = new Vector2(horizontalFull ? 0 : offMin.x, verticalFull ? 0 : offMin.y);
			Rect.offsetMax = new Vector2(horizontalFull ? 0 : offMax.x, verticalFull ? 0 : offMax.y);
		}

		private float GetRootCanvasScaleFactor()
		{
			Canvas canvas = GetComponentInParent<Canvas>();
			if (canvas == null)
				return 1;

			return canvas.rootCanvas.GetComponent<Canvas>().scaleFactor;
		}

		public void ForceUpdate()
		{
			if (Rect == null)
				Init();

			UpdateRect();
		}

		public void Config(
			VerticalOptions verticalOption,
			BehaviourOptions verticalBehaviour,
			float verticalInfluence,
			HorizontalOptions horizontalOption,
			BehaviourOptions horizontalBehaviour,
			float horizontalInfluence,
			bool update = false)
		{
			_verticalOption = verticalOption;
			_verticalBehaviour = verticalBehaviour;
			if (verticalInfluence >= 0 && verticalInfluence <= 1)
			{
				_verticalInfluence = verticalInfluence;
			}

			_horizontalOption = horizontalOption;
			_horizontalBehaviour = horizontalBehaviour;
			if (horizontalInfluence >= 0 && horizontalInfluence <= 1)
			{
				_horizontalInfluence = horizontalInfluence;
			}

			if (update)
			{
				ForceUpdate();
			}
		}

		private void UpdateRect()
		{
			Rect totalArea = new Rect(0, 0, Screen.width, Screen.height);
			Rect safeArea = Screen.safeArea;

			float bottomDistance = safeArea.y;
			float topDistance = (totalArea.height - (safeArea.y + safeArea.height));

			float leftDistance = safeArea.x;
			float rightDistance = (totalArea.width - (safeArea.x + safeArea.width));

			float csf = GetRootCanvasScaleFactor();
			bottomDistance /= csf;
			topDistance /= csf;
			leftDistance /= csf;
			rightDistance /= csf;

			Vector2 tempVector2;
			Vector3 tempVector3;

			switch (HorizontalOption)
			{
				case HorizontalOptions.FullWidth:
					Rect.offsetMin = new Vector2(leftDistance * HorizontalInfluence, Rect.offsetMin.y);
					Rect.offsetMax = new Vector2(-(rightDistance * HorizontalInfluence), Rect.offsetMax.y);
					break;
				case HorizontalOptions.LeftOnly:
					switch (HorizontalBehaviour)
					{
						case BehaviourOptions.Move:
							tempVector3 = Rect.localPosition;
							tempVector3 = new Vector3(
								tempVector3.x + leftDistance * HorizontalInfluence,
								tempVector3.y,
								tempVector3.z);
							Rect.localPosition = tempVector3;
							break;
						case BehaviourOptions.ScaleDown:
							tempVector2 = Rect.offsetMin;
							tempVector2.x += leftDistance * HorizontalInfluence;
							Rect.offsetMin = tempVector2;
							break;
						case BehaviourOptions.ScaleUp:
							Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Rect.rect.width + leftDistance * HorizontalInfluence);
							break;
					}

					break;
				case HorizontalOptions.RightOnly:
					switch (HorizontalBehaviour)
					{
						case BehaviourOptions.Move:
							tempVector3 = Rect.localPosition;
							tempVector3 = new Vector3(
								tempVector3.x - rightDistance * HorizontalInfluence,
								tempVector3.y,
								tempVector3.z);
							Rect.localPosition = tempVector3;
							break;
						case BehaviourOptions.ScaleDown:
							tempVector2 = Rect.offsetMax;
							tempVector2.x -= rightDistance * HorizontalInfluence;
							Rect.offsetMax = tempVector2;
							break;
						case BehaviourOptions.ScaleUp:
							Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Rect.rect.width + rightDistance * HorizontalInfluence);
							break;
					}

					break;
				case HorizontalOptions.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (VerticalOption)
			{
				case VerticalOptions.FullHeight:
					Rect.offsetMax = new Vector2(Rect.offsetMax.x, -(topDistance * VerticalInfluence));
					Rect.offsetMin = new Vector2(Rect.offsetMin.x, bottomDistance * VerticalInfluence);
					break;
				case VerticalOptions.TopOnly:
					switch (_verticalBehaviour)
					{
						case BehaviourOptions.Move:
							tempVector3 = Rect.localPosition;
							tempVector3 = new Vector3(
								tempVector3.x,
								tempVector3.y - topDistance * VerticalInfluence,
								tempVector3.z);
							Rect.localPosition = tempVector3;
							break;
						case BehaviourOptions.ScaleDown:
							tempVector2 = Rect.offsetMax;
							tempVector2.y -= topDistance * VerticalInfluence;
							Rect.offsetMax = tempVector2;
							break;
						case BehaviourOptions.ScaleUp:
							Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Rect.rect.height + topDistance * VerticalInfluence);
							break;
					}

					break;
				case VerticalOptions.BottomOnly:
					switch (_verticalBehaviour)
					{
						case BehaviourOptions.Move:
							tempVector3 = Rect.localPosition;
							tempVector3 = new Vector3(
								tempVector3.x,
								tempVector3.y + bottomDistance * VerticalInfluence,
								tempVector3.z);
							Rect.localPosition = tempVector3;
							break;
						case BehaviourOptions.ScaleDown:
							tempVector2 = Rect.offsetMin;
							tempVector2.y += bottomDistance * VerticalInfluence;
							Rect.offsetMin = tempVector2;
							break;
						case BehaviourOptions.ScaleUp:
							Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Rect.rect.height + bottomDistance * VerticalInfluence);
							break;
					}

					break;
				case VerticalOptions.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public Rect GetDistanceRaw()
		{
			var totalArea = new Rect(0, 0, Screen.width, Screen.height);

			var y0 = Screen.safeArea.y * VerticalInfluence;
			var y1 = (totalArea.height - (Screen.safeArea.y + Screen.safeArea.height)) * VerticalInfluence;

			var x0 = Screen.safeArea.y * HorizontalInfluence;
			var x1 = (totalArea.width - (Screen.safeArea.y + Screen.safeArea.width)) * HorizontalInfluence;

			return UnityEngine.Rect.MinMaxRect(
				VerticalOption == VerticalOptions.TopOnly ? 0 : y0,
				VerticalOption == VerticalOptions.BottomOnly ? 0 : y1,
				HorizontalOption == HorizontalOptions.RightOnly ? 0 : x0,
				HorizontalOption == HorizontalOptions.LeftOnly ? 0 : x1);
		}

		public Rect GetDistanceWithRatio()
		{
			var rect = GetDistanceRaw();
			var f = GetRootCanvasScaleFactor();
			return UnityEngine.Rect.MinMaxRect(
				rect.xMin / f,
				rect.xMax / f,
				rect.yMin / f,
				rect.yMax / f);
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(SafeArea))]
	public class SafeAreaEditor : UnityEditor.Editor
	{
		private SafeArea SafeArea { get; set; }

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (SafeArea == null)
			{
				SafeArea = (SafeArea) target;
				SafeArea.Init();
			}

			bool vertFull = SafeArea.VerticalOption == SafeArea.VerticalOptions.FullHeight;
			bool horizFull = SafeArea.HorizontalOption == SafeArea.HorizontalOptions.FullWidth;

			bool verticalRequired = false;
			bool horizontalRequired = false;

			// checking if it is really needed to apply changes
			if (vertFull)
			{
				if (Math.Abs(SafeArea.Rect.position.y) > 0.01 ||
				    Math.Abs(SafeArea.Rect.sizeDelta.y) > 0.01 ||
				    Math.Abs(SafeArea.Rect.anchorMin.y) > 0.01 ||
				    Math.Abs(SafeArea.Rect.anchorMax.y - 1) > 0.01)
				{
					verticalRequired = true;
				}
			}

			if (horizFull)
			{
				if (Math.Abs(SafeArea.Rect.position.x) > 0.01 ||
				    Math.Abs(SafeArea.Rect.sizeDelta.x) > 0.01 ||
				    Math.Abs(SafeArea.Rect.anchorMin.x) > 0.01 ||
				    Math.Abs(SafeArea.Rect.anchorMax.x - 1) > 0.01)
				{
					horizontalRequired = true;
				}
			}

			if (verticalRequired || horizontalRequired)
			{
				GUILayout.Space(15);
				if (GUILayout.Button("Adjust RectTransform"))
				{
					SafeArea.FixRectTransform(vertFull, horizFull);
				}
			}
		}
	}
#endif
