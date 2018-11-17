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
using UnityEngine;
using UnityEngine.UI;

namespace Skillster.Animation
{
	public class TweenProperty<TComponent, TData> : TweenBase where TComponent : Component
	{
		protected TComponent m_target;
		protected TData m_start;
		protected TData m_end;

		private float m_length = 0f;
		private float m_time = 0f;
		private TweenDir m_dir = TweenDir.In;
		private EasingType m_easing = EasingType.Linear;


		public override bool IsDestroyed { get { return !m_target; } }


		public TweenProperty<TComponent, TData> Init(TComponent target, TData end, float length, TweenDir dir, EasingType easing)
		{
			m_target = target;
			m_end = end;
			m_length = length;
			m_time = 0f;
			m_dir = dir;
			m_easing = easing;

			return this;
		}

		public override bool Update(float dt)
		{
			bool alive = true;

			m_time += dt;
			if (m_time > m_length)
			{
				m_time = m_length;
				alive = false;
				m_keepWaiting = false;
			}

			switch (m_dir)
			{
				case TweenDir.In:
					Set(Easing.EaseIn(m_time / m_length, m_easing));
					break;

				case TweenDir.Out:
					Set(Easing.EaseOut(m_time / m_length, m_easing));
					break;

				case TweenDir.InOut:
					Set(Easing.EaseInOut(m_time / m_length, m_easing));
					break;
			}

			return alive;
		}

		public override void Start() { }
		protected virtual void Set(float t) { }
	}


	public class TweenTransformTransform: TweenProperty<Transform, Transform>
	{
		private Vector3 m_startPosition;
		private Quaternion m_startRotation;

		public override void Start()
		{
			m_startPosition = m_target.position;
			m_startRotation = m_target.rotation;
		}

		protected override void Set(float t)
		{
			m_target.SetPositionAndRotation(Vector3.Lerp(m_startPosition, m_end.position, t), Quaternion.Slerp(m_startRotation, m_end.rotation, t));
		}
	}

	public class TweenTransformPosition : TweenProperty<Transform, Vector3>
	{
		public override void Start()
		{
			m_start = m_target.position;
		}

		protected override void Set(float t)
		{
			m_target.position = Vector3.Lerp(m_start, m_end, t);
		}
	}

	public class TweenTransformLocalPosition : TweenProperty<Transform, Vector3>
	{
		public override void Start()
		{
			m_start = m_target.localPosition;
		}

		protected override void Set(float t)
		{
			m_target.localPosition = Vector3.Lerp(m_start, m_end, t);
		}
	}

	public class TweenTransformRotation : TweenProperty<Transform, Quaternion>
	{
		public override void Start()
		{
			m_start = m_target.rotation;
		}

		protected override void Set(float t)
		{
			m_target.rotation = Quaternion.Slerp(m_start, m_end, t);
		}
	}

	public class TweenTransformLocalRotation : TweenProperty<Transform, Quaternion>
	{
		public override void Start()
		{
			m_start = m_target.localRotation;
		}

		protected override void Set(float t)
		{
			m_target.localRotation = Quaternion.Slerp(m_start, m_end, t);
		}
	}

	public class TweenTransformLocalScale : TweenProperty<Transform, Vector3>
	{
		public override void Start()
		{
			m_start = m_target.localScale;
		}

		protected override void Set(float t)
		{
			m_target.localScale = Vector3.Lerp(m_start, m_end, t);
		}
	}

	public class TweenRectTransformAnchoredPosition : TweenProperty<RectTransform, Vector2>
	{
		public override void Start()
		{
			m_start = m_target.anchoredPosition;
		}

		protected override void Set(float t)
		{
			m_target.anchoredPosition = Vector2.Lerp(m_start, m_end, t);
		}
	}

	public class TweenRectTransformSizeDelta : TweenProperty<RectTransform, Vector2>
	{
		public override void Start()
		{
			m_start = m_target.sizeDelta;
		}

		protected override void Set(float t)
		{
			m_target.sizeDelta = Vector2.Lerp(m_start, m_end, t);
		}
	}

	public class TweenRectTransformPivot : TweenProperty<RectTransform, Vector2>
	{
		public override void Start()
		{
			m_start = m_target.pivot;
		}

		protected override void Set(float t)
		{
			m_target.pivot = Vector2.Lerp(m_start, m_end, t);
		}
	}

	public class TweenCanvasGroupAlpha : TweenProperty<CanvasGroup, float>
	{
		public override void Start()
		{
			m_start = m_target.alpha;
		}

		protected override void Set(float t)
		{
			m_target.alpha = Mathf.Lerp(m_start, m_end, t);
		}
	}

	public class TweenUIColor : TweenProperty<UnityEngine.UI.MaskableGraphic, Color>
	{
		public override void Start()
		{
			m_start = m_target.color;
		}

		protected override void Set(float t)
		{
			m_target.color = Color.Lerp(m_start, m_end, t);
		}
	}

	public class TweenLayoutElementPreferredSize : TweenProperty<UnityEngine.UI.LayoutElement, Vector2>
	{
		public override void Start()
		{
			m_start = new Vector2(m_target.preferredWidth, m_target.preferredHeight);
		}

		protected override void Set(float t)
		{
			m_target.preferredWidth = Mathf.Lerp(m_start.x, m_end.x, t);
			m_target.preferredHeight = Mathf.Lerp(m_start.y, m_end.y, t);
		}
	}

    public class TweenCallback : TweenProperty<Component, float>
    {
        private Action<float> m_action;

        public override bool IsDestroyed { get { return false; } }

        public TweenCallback(Action<float> action, float start)
        {
            m_action = action;
            m_start = start;
        }

        protected override void Set(float t)
        {
            m_action.Invoke(Mathf.Lerp(m_start, m_end, t));
        }
    }

    public class TweenContinous : TweenBase
	{
		private enum State
		{
			ToMin,
			ToMax,
			AtTarget,
			Stop,
		}

		public delegate void SetValueDelegate(float t);
		public delegate void AtStartOrEndDelegate(bool atEnd);

		private float m_minValue = 0f;
		private float m_maxValue = 0f;
		private float m_length = 0f;
		private float m_time = 0f;
		private TweenDir m_dir = TweenDir.In;
		private EasingType m_easing = EasingType.Linear;
		private State m_state = State.AtTarget;

		public event SetValueDelegate OnSetValue = null;
		public event AtStartOrEndDelegate AtStartOrEnd = null;

		public bool Running { get { return m_state != State.AtTarget; } }
		public override bool IsDestroyed { get { return false; } }

		public TweenContinous(float minValue, float maxValue, float length, TweenDir dir, EasingType easing)
		{
			m_minValue = minValue;
			m_maxValue = maxValue;
			m_length = length;
			m_dir = dir;
			m_easing = easing;
		}

		public void ToMin(bool instantaneously = false)
		{
			if (instantaneously)
			{
				m_state = State.AtTarget;
				m_time = 0;
				m_keepWaiting = false;

				if (OnSetValue != null)
					OnSetValue.Invoke(m_minValue);

				if (AtStartOrEnd != null)
					AtStartOrEnd.Invoke(false);
			}
			else
			{
				m_state = State.ToMin;
				m_keepWaiting = true;
			}
		}

		public void ToMax(bool instantaneously = false)
		{
			if (instantaneously)
			{
				m_state = State.AtTarget;
				m_time = m_length;
				m_keepWaiting = false;

				if (OnSetValue != null)
					OnSetValue.Invoke(m_maxValue);
					
				if (AtStartOrEnd != null)
					AtStartOrEnd.Invoke(true);
			}
			else
			{
				m_state = State.ToMax;
				m_keepWaiting = true;
			}
		}

		public void Stop()
		{
			m_state = State.Stop;
		}

		public override void Start()
		{
		}

		public override bool Update(float dt)
		{
			switch (m_state)
			{
				case State.ToMin:
					m_keepWaiting = true;
					m_time -= dt;
					break;

				case State.ToMax:
					m_keepWaiting = true;
					m_time += dt;
					break;

				case State.AtTarget:
					m_keepWaiting = false;
					return true;

				case State.Stop:
					return false;
			}

			if (m_time >= m_length)
			{
				m_time = m_length;
				m_state = State.AtTarget;

				if (AtStartOrEnd != null)
					AtStartOrEnd.Invoke(true);
			}

			if (m_time <= 0f)
			{
				m_time = 0f;
				m_state = State.AtTarget;

				if (AtStartOrEnd != null)
					AtStartOrEnd.Invoke(false);
			}

			float t = 0f;
			switch (m_dir)
			{
				case TweenDir.In:
					t = Easing.EaseIn(m_time / m_length, m_easing);
					break;

				case TweenDir.Out:
					t = Easing.EaseOut(m_time / m_length, m_easing);
					break;

				case TweenDir.InOut:
					t = Easing.EaseInOut(m_time / m_length, m_easing);
					break;
			}

			float value = Mathf.Lerp(m_minValue, m_maxValue, t);
			if (OnSetValue != null)
				OnSetValue.Invoke(value);

			return true;
		}
	}
}
