using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Drawing;
using Xe.Game.Drawing.SequenceOperations;
using Xe.Game.Sequences;
using Xe.Game.Tilemaps;
using Xe.Tools.Projects;
using Xe.Tools.Services;

namespace Xe.Game.Drawing
{
	public class SequenceDrawer : MapDrawer
	{
		private Sequence _sequence;
		private List<ISequenceOperation> _asyncOperations = new List<ISequenceOperation>();
		private ISequenceOperation _currentOperation;
		private int _entryIndex;
		private bool _isAborted;

		public delegate void ChangeSequenceIndexDelegate(int index);
		public delegate void SequenceFinished();

		public Sequence Sequence
		{
			get => _sequence;
			set
			{
				_sequence = value;
				Reset();
			}
		}

		public int EntryIndex
		{
			get => _entryIndex;
			set
			{
				_entryIndex = value;
				OnChangeSequenceIndex?.Invoke(value);
			}
		}

		public bool Paused { get; set; } = true;

		public int ExecutingOperationsCount => _asyncOperations.Count + (_currentOperation == null ? 0 : 1);

		public event ChangeSequenceIndexDelegate OnChangeSequenceIndex;

		public event SequenceFinished OnSequenceFinished;

		#region Operation variables

		public bool IsAborted
		{
			get => _isAborted;
			set
			{
				_isAborted = value;
				EntryIndex = int.MaxValue - 1;
				OnSequenceFinished?.Invoke();
			}
		}

		public double Timer { get; set; }

		public double TimeMultiplier { get; private set; }

		public System.Drawing.Color ForegroundColor { get; set; }

		public double GamepadRumbleX { get; private set; }

		public double GamepadRumbleY { get; private set; }

		public PointF Camera { get; set; }

		#endregion

		public SequenceDrawer(ProjectService projService, IDrawing drawing) :
			base(projService, drawing)
		{ }

		public override void Render(RectangleF rect, bool drawInvisibleObjects = false)
		{
			const float PRECISION = 1.0f;

			float x = Math.Max(0.0f, Camera.X - rect.Width / 2.0f);
			float y = Math.Max(0.0f, Camera.Y - rect.Height / 2.0f);

			var camRect = new RectangleF()
			{
				X = x - (x % PRECISION),
				Y = y - (y % PRECISION),
				Width = rect.Width,
				Height = rect.Height
			};

			base.Render(camRect, drawInvisibleObjects);
			if (ForegroundColor.A > 0)
			{
				Drawing.FillRectangle(new RectangleF()
				{
					X = 0.0f,
					Y = 0.0f,
					Width = rect.Width,
					Height = rect.Height
				}, ForegroundColor);
			}
		}

		private Sequence.Entry PeekEntry()
		{
			return _sequence.Entries[EntryIndex];
		}

		private ISequenceOperation PopEntry()
		{
			if (EntryIndex >= _sequence.Entries.Count)
			{
				IsAborted = true;
				return null;
			}
			var entry = _sequence.Entries[EntryIndex++];
			return Execute(entry);
		}

		public override void Update(double deltaTime)
		{
			if (Paused)
				return;

			var currentDeltaTime = deltaTime * TimeMultiplier;
			while (!IsAborted && currentDeltaTime > 0.0)
			{
				Timer += currentDeltaTime;

				if (_currentOperation != null)
					_currentOperation.Update(currentDeltaTime);
				foreach (var op in _asyncOperations)
					op.Update(deltaTime);
				base.Update(currentDeltaTime);

				if (_currentOperation?.IsFinished ?? true)
				{
					currentDeltaTime = _currentOperation?.TimeDiscarded ?? 0.0;
					while (!IsAborted)
					{
						var seqOp = PopEntry();
						if (seqOp != null)
						{
							if (seqOp.IsAsynchronous)
							{
								_asyncOperations.Add(seqOp);
							}
							else
							{
								_currentOperation = seqOp;
								break;
							}
						}
					}
				}
				else
				{
					currentDeltaTime = 0.0;
				}
			}
			if (IsAborted)
			{
				base.Update(currentDeltaTime);
			}
		}
		
		private ISequenceOperation Execute(Sequence.Entry entry)
		{
			Entity entity;
			IEnumerable<Entity> entities;

			switch (entry.Operation)
			{
				case Operation.None:
					return null;
				case Operation.Abort:
					IsAborted = true;
					return null;
				case Operation.Sleep:
					return new Sleep(entry);
				case Operation.SetTimeMuliplier:
					TimeMultiplier = (double)entry.GetValue(0);
					return null;
				case Operation.RemoveAsynchronousOperations:
					_asyncOperations.Clear();
					return null;
				case Operation.PlaySequence:
					break;
				case Operation.ChangeMap:
					ChangeMap(entry);
					return null;
				case Operation.GamepadRumble:
					GamepadRumbleX = (double)entry.GetValue(0);
					GamepadRumbleY = (double)entry.GetValue(1);
					return null;
				case Operation.GamepadColor:
					break;
				case Operation.FadeInBlack:
					ForegroundColor = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
					return new FadeIn(this, entry);
				case Operation.FadeInWhite:
					ForegroundColor = System.Drawing.Color.FromArgb(255, System.Drawing.Color.White);
					return new FadeIn(this, entry);
				case Operation.FadeOutBlack:
					ForegroundColor = System.Drawing.Color.FromArgb(0, System.Drawing.Color.Black);
					return new FadeOut(this, entry);
				case Operation.FadeOutWhite:
					ForegroundColor = System.Drawing.Color.FromArgb(255, System.Drawing.Color.White);
					return new FadeOut(this, entry);
				case Operation.CameraLock:
					break;
				case Operation.CameraShake:
					return new CameraShake(this, entry);
				case Operation.CameraSet:
					Camera = new PointF()
					{
						X = (int)entry.GetValue(0),
						Y = (int)entry.GetValue(1)
					};
					return null;
				case Operation.CameraMove:
					return new CameraMove(this, entry);
				case Operation.PlayBgm:
					break;
				case Operation.StopBgm:
					break;
				case Operation.PauseBgm:
					break;
				case Operation.SetBgmVolume:
					break;
				case Operation.PlaySe:
					break;
				case Operation.StopAllSe:
					break;
				case Operation.PlayerInputLock:
					break;
				case Operation.BattleFlag:
					break;
				case Operation.AddItem:
					break;
				case Operation.AddEquip:
					break;
				case Operation.AddSkill:
					break;
				case Operation.AddAbility:
					break;
				case Operation.CameraFollow:
					break;
				case Operation.EntityPosition:
					entity = GetEntityByName((string)entry.GetValue(0));
					if (entity != null)
					{
						int x = (int)entry.GetValue(1);
						int y = (int)entry.GetValue(2);
						entity.Position = new PointF(x, y);
					}
					else
					{
						// TODO log not found
					}
					return null;
				case Operation.EntityMove:
					entity = GetEntityByName((string)entry.GetValue(0));
					if (entity == null)
					{
						// TODO log not found
						return null;
					}
					return new EntityMove(entity, entry);
				case Operation.EntityAnimation:
					entity = GetEntityByName((string)entry.GetValue(0));
					if (entity != null)
					{
						var animName = (string)entry.GetValue(1);
						var direction = (Direction)(int)entry.GetValue(2);
						entity.SetAnimation(animName, direction);
					}
					else
					{
						// TODO log not found
					}
					return null;
				case Operation.EntityDirection:
					entity = GetEntityByName((string)entry.GetValue(0));
					if (entity != null)
					{
						var direction = (Direction)(int)entry.GetValue(1);
						entity.SetAnimation(entity.Animation, direction);
					}
					else
					{
						// TODO log not found
					}
					return null;
				case Operation.DialogFaceset:
					break;
				case Operation.TextColor:
					break;
			}
			IsAborted = true;
			throw new NotImplementedException($"Operation {entry.Operation} not implemented yet.");
		}

		private void ChangeMap(Sequence.Entry entry)
		{
			var mapName = GetMapName((int)entry.GetValue(0), (int)entry.GetValue(1));

			var file = ProjectService.Project.GetFilesByFormat("tilemap")
				.FirstOrDefault(x => x.Name == $"{mapName}.tmx");
			var tiledMap = new Tiled.Map(file.FullPath);
			Map = new TilemapTiled().Open(tiledMap, null);
			_asyncOperations.Clear();
		}

		private static readonly string[] ZONES = new string[]
		{
			"debug", "island", "forest", "cave", "mountain", "depuration", "powerplant"
		};
		private string GetMapName(int zone, int map)
		{
			return $"{ZONES[zone]}_{map.ToString("D02")}";
		}

		public void Reset()
		{
			IsAborted = false;
			TimeMultiplier = 1.0;
			ForegroundColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
			EntryIndex = 0;
			_currentOperation = null;
			_asyncOperations.Clear();
		}

	}
}