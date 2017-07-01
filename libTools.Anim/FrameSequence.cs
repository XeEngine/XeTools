using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe;
using static Xe.Math;

namespace libTools.Anim
{
    public class FrameSequence : IDeepCloneable
    {
        private static readonly int TIMESTEP = 21600; // 5^2 + 3^3 + 2^5

        private short _left, _top, _right, _bottom;
        private ushort _speed;
		private byte _loop, _event, _texture;
        public List<string> Frames = new List<string>();

		public FrameSequence()
		{
			_left = -8;
			_top = -8;
			_right = +8;
			_bottom = +8;
			_speed = 00;
			_loop = 255;
			_event = 255;
			_texture = 0;
		}

        public int HitboxLeft
        {
            get { return _left; }
            set { _left = (short)Range(value, short.MinValue, short.MaxValue); }
        }
        public int HitboxTop
        {
            get { return _top; }
            set { _top = (short)Range(value, short.MinValue, short.MaxValue); }
        }
        public int HitboxRight
        {
            get { return _right; }
            set { _right = (short)Range(value, short.MinValue, short.MaxValue); }
        }
        public int HitboxBottom
        {
            get { return _bottom; }
            set { _bottom = (short)Range(value, short.MinValue, short.MaxValue); }
		}
		public int Loop
		{
			get { return _loop; }
			set { _loop = (byte)Range(value, byte.MinValue, byte.MaxValue); }
		}
		public int Event
		{
			get { return _event; }
			set { _event = (byte)Range(value, byte.MinValue, byte.MaxValue); }
		}
		public int Texture
        {
            get { return _texture; }
            set { _texture = (byte)Range(value, byte.MinValue, byte.MaxValue); }
        }
        public int Speed
        {
            get { return _speed; }
            set { _speed = (ushort)Range(value, ushort.MinValue, ushort.MaxValue); }
        }
        [JsonIgnore]
        public float FramesPerSecond
        {
            get { return _speed > 0 ? TIMESTEP / _speed : 0; }
            set { _speed = (ushort)(value > 0 ? System.Math.Round(TIMESTEP / value) : 0); }
        }
        [JsonIgnore]
        public Rectangle Hitbox
        {
            get { return new Rectangle(_left, _top, _right - _left, _bottom - _top); }
        }

        public FrameSequence MergeWidth(FrameSequence sequence)
        {
            /*foreach (var name in sequence.Frames)
            {
                bool found = false;
                foreach (var curframe in Frames)
                {
                    if (string.Compare(curframe.Name, name.Name, true) == 0)
                    {
                        found = true;
                        curframe.Rectangle = name.Rectangle;
                    }
                }
                if (!found)
                {
                    var size = name.Size;
                    name.CenterX = size.Width / 2;
                    name.CenterY = size.Height / 2;
                    Frames.Add(name);
                }
            }*/
            return this;
        }

        public override int GetHashCode()
        {
            int r = unchecked((int)(_loop ^
				(((uint)_texture << 9) | ((uint)_texture << 30)) ^
				(((uint)_event << 13) | ((uint)_event << 21)) ^
				(((uint)_speed << 2) | ((uint)_speed >> 26)) ^
                (((uint)_left << 13) | ((uint)_left >> 6)) ^
                (((uint)_top << 17) | ((uint)_top >> 8)) ^
                (((uint)_right << 21) | ((uint)_right >> 10)) ^
                (((uint)_bottom << 25) | ((uint)_bottom >> 9))));
            foreach (var item in Frames)
                r ^= item.GetHashCode();
            return r;
        }

        public object DeepClone()
        {
			var item = new FrameSequence()
			{
				_left = _left,
				_top = _top,
				_right = _right,
				_bottom = _bottom,
				_speed = _speed,
				_loop = _loop,
				_event = _event,
				_texture = _texture,
				Frames = new List<string>()
			};
			foreach (var e in Frames) item.Frames.Add(e.Clone() as string);
            return item;
        }
    }
}
