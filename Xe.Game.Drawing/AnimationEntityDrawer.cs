using System;
using Xe.Drawing;
using Xe.Game.Animations;

namespace Xe.Game.Drawing
{
	public sealed class AnimationEntityDrawer
	{
		#region public properties

		public AnimationDrawer Drawer { get; }

		/// <summary>
		/// Get or set the name of current animation
		/// </summary>
		public string Animation
		{
			get => _currentAnimation?.Name;
			set
			{
				// Validate the name
				if (!string.IsNullOrWhiteSpace(value))
				{
					// Get the animation object from its name
					if (Drawer.DictionaryAnimations.TryGetValue(value, out _currentAnimation))
					{
						// pre-load the texture
						ISurface texture = null;
						var textureId = _currentAnimation.Texture;
						foreach (var item in Drawer.AnimationData.Textures)
						{
							if (textureId == item.Id)
							{
								texture = Drawer.Resources[item.Name];
								break;
							}
						}
						_spriteSheet = texture;

						// Reset the frame index
						_frameIndex = -1;
						FrameIndex = 0;
					}
				}
			}
		}

		/// <summary>
		/// Get the current animation object loaded
		/// </summary>
		public Animation CurrentAnimation => _currentAnimation;

		/// <summary>
		/// Get the current frame reference
		/// </summary>
		public FrameRef CurrentFrameReference
		{
			get
			{
				if (FrameIndex >= 0 && _currentAnimation != null &&
					FrameIndex < _currentAnimation.Frames.Count)
					return _currentAnimation.Frames[FrameIndex];
				return null;
			}
		}

		/// <summary>
		/// Get the current frame from frame reference
		/// </summary>
		public Frame CurrentFrame
		{
			get
			{
				var currentFrameReference = CurrentFrameReference;
				if (currentFrameReference == null ||
					currentFrameReference.Frame == null)
					return null;
				Drawer.DictionaryFrames.TryGetValue(currentFrameReference.Frame, out Frame frame);
				return frame;
			}
		}

		/// <summary>
		/// Get or set the current frame index
		/// </summary>
		public int FrameIndex
		{
			get => _frameIndex;
			set
			{
				if (_frameIndex != value)
				{
					_frameIndex = value;
					Drawer.NotifyFrameChanged(this);
				}
			}
		}

		public double Timer
		{
			get => _timer;
			set
			{
				_timer = value;
				UpdateTimer();
			}
		}

		#endregion

		#region public methods

		internal AnimationEntityDrawer(AnimationDrawer drawer)
		{
			Drawer = drawer;
		}

		public bool SetAnimation(string animationDefinitionName, Direction direction)
		{
            if (string.IsNullOrEmpty(animationDefinitionName))
                return false;

			if (Drawer.DictionaryAnimationDefinitions.TryGetValue(animationDefinitionName, out var animationDefinition))
			{
				string animation;
				switch (direction)
				{
					case Direction.Undefined:
						animation = animationDefinition.Default.Name;
						break;
					case Direction.Up:
						animation = animationDefinition.DirectionUp.Name;
						break;
					case Direction.Right:
						animation = animationDefinition.DirectionRight.Name;
						break;
					case Direction.Down:
						animation = animationDefinition.DirectionDown.Name;
						break;
					case Direction.Left:
						animation = animationDefinition.DirectionLeft.Name;
						break;
					default:
						return false;
				}
				
				if (string.IsNullOrEmpty(animation))
				{
					if (direction != Direction.Undefined)
						return SetAnimation(animationDefinitionName, Direction.Undefined);
					return false;
				}
				else
				{
					Animation = animation;
				}
			}
			return false;
		}

		public void Draw(double x, double y)
		{
			var frame = CurrentFrame;
			if (frame != null)
			{
				var rect = new System.Drawing.Rectangle()
				{
					X = frame.Left,
					Y = frame.Top,
					Width = frame.Right - frame.Left,
					Height = frame.Bottom - frame.Top
				};
				Drawer.Drawing.DrawSurface(_spriteSheet, rect,
					(int)x - frame.CenterX, (int)y - frame.CenterY);
			}
		}

		#endregion

		#region private

		private static readonly double TIMESTEP = 21600.0; // 5^2 + 3^3 + 2^5

		private Animation _currentAnimation;

		private ISurface _spriteSheet;
		
		private int _frameIndex;

		private double _timer;
		
		private void UpdateTimer()
		{
			var curAnim = CurrentAnimation;
			if (curAnim != null)
			{
				int framesCount = curAnim.Frames.Count;
				if (curAnim.Frames.Count > 0)
				{
					double freq = 1.0 / (curAnim.Speed / TIMESTEP);
					var index = (int)(Timer * freq);
					if (index >= 0)
					{
						if (index >= framesCount)
						{
							var loop = curAnim.Loop;
							if (loop == 0)
							{
								FrameIndex = index % framesCount;
							}
							else if (loop < framesCount)
							{
								FrameIndex = index % framesCount;
								//FrameIndex = index - loop + ((index - loop) % (framesCount - loop));
							}
						}
						else
						{
							FrameIndex = index;
						}
					}
				}
			}
		}

		#endregion
	}
}
