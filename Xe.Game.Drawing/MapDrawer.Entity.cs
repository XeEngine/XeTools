using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Tilemaps;

namespace Xe.Game.Drawing
{
	public partial class MapDrawer
	{
		public class Entity
		{
			private string _animation;
			private Direction _direction;

			public ObjectEntry Entry { get; }

			public AnimationDrawer Drawer
			{
				get => Animator?.Drawer;
				set
				{
					Animator = new AnimationEntityDrawer(value);
					SetAnimation(Animation, Direction);
				}
			}

			public AnimationEntityDrawer Animator { get; private set; }

			public string Name => Entry.Name;

			public string Group => Entry.Type;

			public PointF Position { get; set; }

			public float Z { get; set; }

			public string Animation => _animation;

			public Direction Direction => _direction;

			public bool IsVisible { get; set; }

			public Entity(ObjectEntry entry)
			{
				Entry = entry;
				Reset();
			}

			public void Reset()
			{
				Position = new PointF((float)Entry.X, (float)Entry.Y);
				Z = (float)Entry.Z;
				SetAnimation(Entry.AnimationName, Entry.Direction);
				IsVisible = Entry.Visible;
				if (Animator != null)
				{
					Animator.Timer = 0.0;
				}
			}

			public void Update(double deltaTime)
			{
				if (Animator != null)
					Animator.Timer += deltaTime;
			}

			public void Draw(float x, float y, float opacity)
			{
				Animator.Draw(Position.X - Entry.X + x, Position.Y - Entry.Y - Z + y, opacity);
			}

			public void SetAnimation(string animation, Direction direction)
			{
				_animation = animation;
				_direction = direction;
				Animator?.SetAnimation(animation, direction);
			}
		}
	}
}
