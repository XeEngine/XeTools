using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Sequences;

namespace Xe.Game.Drawing.SequenceOperations
{
	public class Sleep : ISequenceOperation
	{
		private double _dstTimer;

		public bool IsFinished => Timer > _dstTimer;

		public double TimeDiscarded => Timer - _dstTimer;

		public bool IsAsynchronous { get; }

		public double Timer { get; private set; }


		public Sleep(Sequence.Entry entry)
		{
			_dstTimer = (double)entry.GetValue(0);
			IsAsynchronous = entry.IsAsynchronous;
		}

		public void Update(double deltaTime)
		{
			Timer += deltaTime;
		}
	}

	public class FadeIn : ISequenceOperation
	{
		private SequenceDrawer _seq;
		private double _dstTimer;

		public bool IsFinished => Timer > _dstTimer;

		public double TimeDiscarded => Timer - _dstTimer;

		public bool IsAsynchronous { get; }

		public double Timer { get; private set; }


		public FadeIn(SequenceDrawer seq, Sequence.Entry entry)
		{
			_seq = seq;
			_dstTimer = (double)entry.GetValue(0);
			IsAsynchronous = entry.IsAsynchronous;
		}

		public void Update(double deltaTime)
		{
			Timer += deltaTime;
			int opacity = (int)((Timer / _dstTimer) * 255.0);
			opacity = Math.Min(Math.Max(opacity, 0), 255);
			_seq.ForegroundColor = Color.FromArgb(255 - opacity, _seq.ForegroundColor);
		}
	}

	public class FadeOut : ISequenceOperation
	{
		private SequenceDrawer _seq;
		private double _dstTimer;

		public bool IsFinished => Timer > _dstTimer;

		public double TimeDiscarded => Timer - _dstTimer;

		public bool IsAsynchronous { get; }

		public double Timer { get; private set; }


		public FadeOut(SequenceDrawer seq, Sequence.Entry entry)
		{
			_seq = seq;
			_dstTimer = (double)entry.GetValue(0);
			IsAsynchronous = entry.IsAsynchronous;
		}

		public void Update(double deltaTime)
		{
			Timer += deltaTime;
			int opacity = (int)((Timer / _dstTimer) * 255.0);
			opacity = Math.Min(Math.Max(opacity, 0), 255);
			_seq.ForegroundColor = Color.FromArgb(opacity, _seq.ForegroundColor);
		}
	}

	public class CameraShake : ISequenceOperation
	{
		private SequenceDrawer _seq;
		private double _camMulX, _camMulY;
		private double _camPrevX, _camPrevY;
		private double _dstTimer;

		public bool IsFinished => Timer >= _dstTimer;

		public double TimeDiscarded => Timer - _dstTimer;

		public bool IsAsynchronous { get; }

		public double Timer { get; private set; }


		public CameraShake(SequenceDrawer seq, Sequence.Entry entry)
		{
			_seq = seq;
			_camMulX = (double)entry.GetValue(0);
			_camMulY = (double)entry.GetValue(1);
			_dstTimer = (double)entry.GetValue(2);
			IsAsynchronous = entry.IsAsynchronous;
		}

		public void Update(double deltaTime)
		{
			var x = _seq.Camera.X - _camPrevX;
			var y = _seq.Camera.Y - _camPrevY;
			Timer += deltaTime;
			if (!IsFinished)
			{
				_camPrevX = _camMulX = _camMulX * -1.0f;
				_camPrevY = _camMulY = _camMulY * -1.0f;
				x += _camMulX;
				y += _camMulY;
			}
			_seq.Camera = new PointF((float)x, (float)y);

		}
	}

	public class CameraMove : ISequenceOperation
	{
		private SequenceDrawer _seq;
		private int _dstX, _dstY;
		private double _speed;

		public bool IsFinished => _seq.Camera.X == _dstX && _seq.Camera.Y == _dstY;

		public double TimeDiscarded => 0.0;

		public bool IsAsynchronous { get; }

		public double Timer { get; private set; }

		public CameraMove(SequenceDrawer seq, Sequence.Entry entry)
		{
			_seq = seq;
			_dstX = (int)entry.GetValue(0);
			_dstY = (int)entry.GetValue(1);
			_speed = (double)entry.GetValue(2);
			IsAsynchronous = entry.IsAsynchronous;
		}

		public void Update(double deltaTime)
		{
			var diffX = _dstX - _seq.Camera.X;
			var diffY = _dstY - _seq.Camera.Y;
			double rad = Math.Atan2(diffY, diffX);
			var x = (float)(Math.Cos(rad) * _speed * deltaTime);
			var y = (float)(Math.Sin(rad) * _speed * deltaTime);
			_seq.Camera = new PointF(_seq.Camera.X + x, _seq.Camera.Y + y);
			if (Math.Sign(diffX) != Math.Sign(_dstX - _seq.Camera.X))
			{
				_seq.Camera = new PointF(_dstX, _dstY);
			}
		}
	}
}
