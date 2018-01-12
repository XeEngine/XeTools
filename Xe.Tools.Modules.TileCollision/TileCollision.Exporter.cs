using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Collisions;

namespace Xe.Tools.Modules
{
	public partial class TileCollision
    {
        private void Export(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
                Export(writer);
        }

        private void Export(BinaryWriter w)
		{
			const uint MagicCode = 0x014C4F43U;
			const int TileWidth = 16;
			const int TileHeight = 16;

			#region Header
			w.Write(MagicCode);
			w.Write((byte)TileWidth);
			w.Write((byte)TileHeight);
			w.Write((byte)0); // RESERVED
			w.Write((byte)0); // RESERVED
			#endregion

			w.WriteChunk(CollisionSystem, WriteCollisionShapesChunk);
			w.WriteChunk(CollisionSystem, WriteCollisionTypesMapChunk);
			w.WriteChunk(CollisionSystem, WriteCollsionTypesChunk);
			w.WriteChunkEnd();

			w.Flush();
		}

		private void WriteChunk(CollisionSystem collisionSystem, BinaryWriter writer, Func<CollisionSystem, BinaryWriter, string> action)
		{
			using (var memoryStream = new MemoryStream(0x8000))
			{
				using (var memoryWriter = new BinaryWriter(memoryStream))
				{
					var strChunk = action(collisionSystem, memoryWriter);
					if (strChunk != null && memoryStream.Length > 0)
					{
						var head = System.Text.Encoding.ASCII.GetBytes(strChunk);
						writer.Write(head, 0, 4);
						writer.Write((uint)memoryStream.Length);
						writer.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
					}
				}
			}
		}

		private string WriteCollisionShapesChunk(CollisionSystem collisionSystem, BinaryWriter w)
		{
			return "SHP\x01";
		}

		private string WriteCollisionTypesMapChunk(CollisionSystem collisionSystem, BinaryWriter w)
		{
			var count = collisionSystem.Collisions.Count;
			if (count == 0)
				return null;
			w.Write((ushort)count);
			w.Write((ushort)0); // RESERVED
			w.Write((uint)0); // RESERVED

			byte counter = 1;
			var collisionTypes = new Dictionary<Guid, byte>(collisionSystem.CollisionTypes.Count)
			{
				{ Guid.Empty, 0 }
			};
			foreach (var item in collisionSystem.CollisionTypes)
			{
				if (item.Id != Guid.Empty)
					collisionTypes.Add(item.Id, counter++);
			}

			foreach (var item in collisionSystem.Collisions)
			{
				if (!collisionTypes.TryGetValue(item.TypeId, out byte index))
				{
					Log.Warning($"Collision type {item.TypeId} not found; id will be set to 0.");
				}
				w.Write(index);
			}

			return "MAP\x01";
		}

		private string WriteCollsionTypesChunk(CollisionSystem collisionSystem, BinaryWriter w)
		{
			var count = collisionSystem.CollisionTypes.Count(x => x.Id != Guid.Empty);
			if (count == 0)
				return null;
			w.Write((ushort)(count + 1));
			w.Write((ushort)0); // RESERVED
			w.Write((uint)0); // RESERVED

			// Write empty collision type
			w.Write(0);

			foreach (var item in collisionSystem.CollisionTypes)
			{
				if (item.Id == Guid.Empty) continue;

				int value;
				Guid valueId = item.EffectParameterValueId ?? Guid.Empty;
				if (!EffectsDictionary.TryGetValue(item.Effect, out byte effectId))
					Log.Warning($"Effect {item.Effect} not found; {nameof(effectId)} will be set to 0.");

				if (item.Effect == Effects.WalkEffect)
				{
					if (!WalksDictionary.TryGetValue(valueId, out var data))
						Log.Warning($"Effect {valueId} not found; {nameof(data)} will be set to 0.");
					value = data;
				}
				else if (item.Effect == Effects.WalkEffect)
				{
					if (!BehaviorsDictionary.TryGetValue(valueId, out var data))
						Log.Warning($"Effect {valueId} not found; {nameof(data)} will be set to 0.");
					value = data;
				}
				else
				{
					value = item.EffectParameterValue ?? 0;
				}

				w.Write(effectId);
				w.Write((byte)0);
				w.Write((ushort)value);
			}

			return "TYP\x01";
		}

	}
}
