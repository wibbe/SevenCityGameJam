/**
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Skillster AB
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skillster.Animation
{
	public enum TweenDir
	{
		In,
		Out,
		InOut
	}


	public abstract class TweenBase : CustomYieldInstruction
	{
		protected bool m_keepWaiting = true;

		public override bool keepWaiting { get { return m_keepWaiting; } }

		public abstract bool IsDestroyed { get; }

		public abstract void Start();
		public abstract bool Update(float dt);
	}

	public class NullTween : TweenBase
	{
		public override bool IsDestroyed { get { return true; } }

		public override void Start() { }
		public override bool Update(float dt) { return false; }
	}



	public class Tween : MonoBehaviour
	{
		private static Tween m_instance = null;
		private static bool m_isQuitting = false;

		public static Tween Instance
		{
			get {
				if (m_instance == null)
				{
					if (m_isQuitting)
					{
						Debug.LogError("Accessing Tween.Instance while application is quitting");
					}
					else
					{
						GameObject go = new GameObject("_Tween");
						m_instance = go.AddComponent<Tween>();
					}
				}
				return m_instance;
			}
		}

		private uint m_currentList = 0;
		private List<TweenBase>[] m_tweens = new List<TweenBase>[2] {
			new List<TweenBase>(),
			new List<TweenBase>()
		};


		public static TweenBase Transform(Transform obj, Transform target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenTransformTransform().Init(obj, target, time, dir, type));
		}

		public static TweenBase Position(Transform obj, Vector3 target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenTransformPosition().Init(obj, target, time, dir, type));
		}

		public static TweenBase LocalPosition(Transform obj, Vector3 target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenTransformLocalPosition().Init(obj, target, time, dir, type));
		}

		public static TweenBase Rotation(Transform obj, Quaternion target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenTransformRotation().Init(obj, target, time, dir, type));
		}

		public static TweenBase LocalRotation(Transform obj, Quaternion target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenTransformLocalRotation().Init(obj, target, time, dir, type));
		}

		public static TweenBase LocalScale(Transform obj, Vector3 target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenTransformLocalScale().Init(obj, target, time, dir, type));
		}

		public static TweenBase AnchoredPosition(RectTransform obj, Vector2 target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenRectTransformAnchoredPosition().Init(obj, target, time, dir, type));
		}

		public static TweenBase SizeDelta(RectTransform obj, Vector2 target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenRectTransformSizeDelta().Init(obj, target, time, dir, type));
		}

		public static TweenBase Pivot(RectTransform obj, Vector2 target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenRectTransformPivot().Init(obj, target, time, dir, type));
		}

		public static TweenBase Alpha(CanvasGroup obj, float target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenCanvasGroupAlpha().Init(obj, target, time, dir, type));
		}

		public static TweenBase UIColor(UnityEngine.UI.MaskableGraphic obj, Color end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenUIColor().Init(obj, end, time, dir, type));
		}

		public static TweenBase PreferredSize(UnityEngine.UI.LayoutElement obj, Vector2 end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenLayoutElementPreferredSize().Init(obj, end, time, dir, type));
		}

		public static TweenBase ActionAfter(float time, Action action)
		{
			if (m_isQuitting)
				return new NullTween();

			return Instance.AddTween(new TweenActionAfter(time, action));
		}

        public static TweenBase Callback(float startValue, float targetValue, float time, Action<float> action, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
        {
            if (m_isQuitting)
                return new NullTween();

            return Instance.AddTween(new TweenCallback(action, startValue).Init(null, targetValue, time, dir, type));
        }

		public static TweenContinous Continous(float minValue, float maxValue, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			TweenContinous tween = new TweenContinous(minValue, maxValue, time, dir, type);
			if (!m_isQuitting)
				Instance.AddTween(tween);
			return tween;
		}

		public static TweenTrack Track()
		{
			var track = new TweenTrack();
			if (!m_isQuitting)
				Instance.AddTween(track);
			return track;
		}

		public static void RemoveTween(TweenBase tween)
		{
			if (m_isQuitting && m_instance == null)
				return;

			foreach (var tweenList in Instance.m_tweens)
				tweenList.Remove(tween);
		}

		private TweenBase AddTween(TweenBase tween)
		{
			tween.Start();
			m_tweens[m_currentList].Add(tween);
			return tween;
		}

		private void Awake()
		{
			m_isQuitting = false;
		}

		private void OnDestroy()
		{
			if (m_instance == this)
				m_instance = null;
		}

		private void OnApplicationQuit()
		{
			m_isQuitting = true;
		}

		private void Update()
		{
			if (m_tweens[m_currentList].Count > 0)
			{
				var nextList = (m_currentList + 1) % 2;
				m_tweens[nextList].Clear();

				foreach (var tween in m_tweens[m_currentList])
				{
					if (!tween.IsDestroyed)
						if (tween.Update(Time.unscaledDeltaTime))
							m_tweens[nextList].Add(tween);
				}

				m_currentList = nextList;
			}
		}
	}

}
