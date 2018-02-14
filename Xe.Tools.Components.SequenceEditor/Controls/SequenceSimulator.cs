using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Drawing;
using Xe.Game.Drawing;
using Xe.Game.Sequences;
using Xe.Game.Tilemaps;
using Xe.Tools.Projects;
using Xe.Tools.Services;
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.SequenceEditor.Controls
{
	public class SequenceSimulator : DrawingControl
	{
		private SequenceDrawer _drawer;
		private ISurface _backBuffer;
		private IController _controller;
		private Sequence _sequence;

		public IController Controller
		{
			get => _controller;
			set
			{
				_controller = value;
				_sequence = _controller.Sequence;
				if (_drawer != null)
					_drawer.Sequence = _controller.Sequence;
			}
		}

		public Sequence Sequence
		{
			get => _sequence;
			set
			{
				_sequence = value;
				if (_drawer != null)
					_drawer.Sequence = value;
			}
		}

		public int CameraWidth { get; set; } = 427;

		public int CameraHeight { get; set; } = 240;

		public bool Paused
		{
			get => _drawer.Paused;
			set => _drawer.Paused = value;
		}

		public void Reset()
		{
			_drawer.Reset();
		}

		protected override void OnDrawingCreated()
		{
			base.OnDrawingCreated();
			_drawer = new SequenceDrawer(_controller.ProjectService, Drawing);
			_drawer.OnChangeSequenceIndex += Drawer_OnChangeSequenceIndex;
			_drawer.OnSequenceFinished += Drawer_OnSequenceFinished;
			_backBuffer = Drawing.CreateSurface(CameraWidth, CameraHeight, PixelFormat.Format32bppArgb, SurfaceType.InputOutput);
			_drawer.Sequence = Sequence;
		}

		private void Drawer_OnSequenceFinished()
		{
			Controller.IsSequenceFinished = true;
		}

		private void Drawer_OnChangeSequenceIndex(int index)
		{
			Controller.CurrentOperationIndex = index;
		}

		protected override void OnDrawRequired()
		{
#if DEBUG
			_drawer.Update(1.0 / 60.0);
#else
			_drawer.Update(DeltaTime);
#endif

			bool useZoom = true;

			lock (Drawing)
			{
				if (useZoom)
				{
					var frontBuffer = Drawing.Surface;
					Drawing.Surface = _backBuffer;

					_drawer.Render(new RectangleF()
					{
						X = 0,
						Y = 0,
						Width = CameraWidth,
						Height = CameraHeight
					}, true);

					Drawing.Surface = frontBuffer;
					var src = new Rectangle(0, 0, CameraWidth, CameraHeight);
					RectangleF dst;
					if (ActualWidth / ActualHeight > (float)CameraWidth / CameraHeight)
					{
						float zoom = (float)(ActualHeight / CameraHeight);
						float width = CameraWidth * zoom;
						float x = (float)(ActualWidth - width) / 2.0f;
						dst = new RectangleF()
						{
							X = x,
							Y = 0,
							Width = width,
							Height = (float)ActualHeight
						};
					}
					else
					{
						float zoom = (float)(ActualWidth / CameraWidth);
						float height = CameraHeight * zoom;
						float y = (float)(ActualHeight - height) / 2.0f;
						dst = new RectangleF()
						{
							X = 0,
							Y = y,
							Width = (float)ActualWidth,
							Height = height
						};
					}
					Drawing.Clear(System.Drawing.Color.Black);
					Drawing.DrawSurface(_backBuffer, src, dst);
				}
				else
				{
					_drawer.Render(new RectangleF()
					{
						X = 0,
						Y = 0,
						Width = CameraWidth,
						Height = CameraHeight
					}, true);
				}
			}
			base.OnDrawRequired();

			Controller.RenderTime = LastDrawAndPresentTime;
			Controller.ExecutionTime = _drawer.Timer;
			Controller.TimeMultiplier = _drawer.TimeMultiplier;
		}

		protected override void OnDrawCompleted()
		{
			base.OnDrawCompleted();
		}
	}
}
