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
using System.Collections.Generic;
using UnityEngine;


namespace Skillster.Animation
{
	public class TweenDelay : TweenBase
	{
		private float m_delay = 0f;

		public TweenDelay(float delay)
		{
			m_delay = delay;
		}

		public override bool IsDestroyed { get { return false; } }

		public override void Start() { }

		public override bool Update(float dt)
		{
			m_delay -= dt;
			m_keepWaiting = m_delay > 0f;
			return m_keepWaiting;
		}
	}

	public class TweenAction : TweenBase
	{
		private Action m_action;

		public TweenAction(Action action)
		{
			m_action = action;
		}

		public override bool IsDestroyed { get { return false; } }

		public override void Start() { }

		public override bool Update(float dt)
		{
			m_action.Invoke();
			m_keepWaiting = false;
			return false;
		}
	}

	public class TweenActionAfter : TweenBase
	{
		private float m_delay = 0f;
		private Action m_action;


		public TweenActionAfter(float delay, Action action)
		{
			m_delay = delay;
			m_action = action;
		}

		public override bool IsDestroyed { get { return false; } }

		public override void Start() { }

		public override bool Update(float dt)
		{
			m_delay -= dt;
			m_keepWaiting = m_delay > 0f;

			if (!m_keepWaiting)
				m_action.Invoke();

			return m_keepWaiting;
		}
	}

	public class TweenSetActive : TweenBase
	{
		private GameObject m_target = null;
		private bool m_active = false;

		public TweenSetActive(GameObject target, bool active)
		{
			m_target = target;
			m_active = active;
		}

		public override bool IsDestroyed { get { return !m_target; } }

		public override void Start() { }

		public override bool Update(float dt)
		{
			m_target.SetActive(m_active);
			m_keepWaiting = false;
			return false;
		}
	}


	public class TweenTrack : TweenBase
	{
		private List<TweenBase> m_track = new List<TweenBase>();
		private int m_position = 0;
		private bool m_start = false;


		public override bool IsDestroyed { get { return false; } }

		public TweenTrack Delay(float delay)
		{
			Add(new TweenDelay(delay));
			return this;
		}

		public TweenTrack Action(Action action)
		{
			Add(new TweenAction(action));
			return this;
		}

		public TweenTrack Active(GameObject obj, bool active)
		{
			Add(new TweenSetActive(obj, active));
			return this;
		}

		public TweenTrack Position(Transform obj, Vector3 end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenTransformPosition().Init(obj, end, time, dir, type));
			return this;
		}

		public TweenTrack LocalPosition(Transform obj, Vector3 end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenTransformLocalPosition().Init(obj, end, time, dir, type));
			return this;
		}

		public TweenTrack Rotation(Transform obj, Quaternion end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenTransformRotation().Init(obj, end, time, dir, type));
			return this;
		}

		public TweenTrack LocalRotation(Transform obj, Quaternion end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenTransformLocalRotation().Init(obj, end, time, dir, type));
			return this;
		}

		public TweenTrack LocalScale(Transform obj, Vector3 target, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenTransformLocalScale().Init(obj, target, time, dir, type));
			return this;
		}

		public TweenTrack AnchoredPosition(RectTransform obj, Vector2 end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenRectTransformAnchoredPosition().Init(obj, end, time, dir, type));
			return this;
		}

		public TweenTrack SizeDelta(RectTransform obj, Vector2 end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenRectTransformSizeDelta().Init(obj, end, time, dir, type));
			return this;
		}

		public TweenTrack Pivot(RectTransform obj, Vector2 end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenRectTransformPivot().Init(obj, end, time, dir, type));
			return this;
		}

		public TweenTrack Alpha(CanvasGroup obj, float end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenCanvasGroupAlpha().Init(obj, end, time, dir, type));
			return this;
		}

		public TweenTrack UIColor(UnityEngine.UI.MaskableGraphic obj, Color end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenUIColor().Init(obj, end, time, dir, type));
			return this;
		}

		public TweenTrack PreferredSize(UnityEngine.UI.LayoutElement obj, Vector2 end, float time, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
		{
			Add(new TweenLayoutElementPreferredSize().Init(obj, end, time, dir, type));
			return this;
		}

        public TweenTrack Callback(float startValue, float targetValue, float time, Action<float> action, TweenDir dir = TweenDir.InOut, EasingType type = EasingType.Quadratic)
        {
            Add(new TweenCallback(action, startValue).Init(null, targetValue, time, dir, type));
            return this;
        }

        public override void Start()
		{
		}

		public override bool Update(float dt)
		{
			if (m_start)
			{
				if (!m_track[m_position].IsDestroyed)
					m_track[m_position].Start();
				m_start = false;
			}

			if (m_position >= m_track.Count)
			{
				m_keepWaiting = false;
				return false;
			}

			if (m_track[m_position].IsDestroyed || !m_track[m_position].Update(dt))
			{
				m_position++;

				if (m_position >= m_track.Count)
				{
					m_keepWaiting = false;
					return false;
				}
				else
					m_start = true;
			}

			return true;
		}

		private void Add(TweenBase tween)
		{
			m_track.Add(tween);
			m_start = true;
		}
	}

}
