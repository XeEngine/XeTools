using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xe.Game.Animations;

namespace Xe.Tools.Modules
{
    public partial class Animation
    {
        private struct Header
        {
            public uint MagicCode;
            public byte Reserved;
            public byte Version;
            public ushort Flags;

            public ushort SpriteSheetsLength;
            public ushort SpriteSheetsCount;
            public ushort FramesLength;
            public ushort FramesCount;
            public ushort FrameExLength;
            public ushort FrameExCount;
            public ushort AnimationsLength;
            public ushort AnimationsCount;
            public ushort AnimationReferencesLength;
            public ushort AnimationReferencesCount;
        }

        [Flags]
        private enum HeaderFlags
        {
            SpriteSheet = 1 << 0,
            Frames = 1 << 1,
            FramesEx = 1 << 2,
            Animations = 1 << 3,
            AnimationReferences = 1 << 4,
        }

        private void Export(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
                Export(writer);
        }

        private void Export(BinaryWriter writer)
        {
            #region Pick only the used data
            var listAnimationRefs = new List<AnimationRef>();
            var listAnimations = new Dictionary<string, Tuple<int, Game.Animations.Animation>>();
            /*foreach (var animRef in AnimationsGroup.AnimationGroups)
            {
                if (listAnimations.ContainsKey(animRef.Animation))
                    continue;
                if (!AnimationsGroup.Animations.TryGetValue(animRef.Name, out var anim))
                {
                    Log.Warning($"Animation {animRef.Animation} not found in {animRef.Name}.");
                    continue;
                }
                listAnimationRefs.Add(animRef);
                listAnimations.Add(animRef.Animation, new Tuple<int, Game.Animations.Animation>(
                    listAnimations.Keys.Count, anim));
            }*/

            var listFrames = new Dictionary<string, Tuple<int, Frame>>();
            foreach (var tuple in listAnimations.Values)
            {
                var anim = tuple.Item2;
                foreach (var frameRef in anim.Frames)
                {
                    if (!AnimationsGroup.Frames.TryGetValue(frameRef.Frame, out var frame))
                    {
                        Log.Warning($"Frame {frameRef.Frame} not found in {anim.Name}.");
                        continue;
                    }
                }
            }
            #endregion

            #region calculate header
            var spriteSheets = new List<byte[]>();
            int spriteSheetSectionLength = 0;
            foreach (var texture in AnimationsGroup.Textures)
            {
                var data = Encoding.ASCII.GetBytes(texture.Name);
                spriteSheets.Add(data);
                spriteSheetSectionLength += data.Length + 1;
            }
            var spriteSheetPaddingData = 4 - (spriteSheetSectionLength % 4);
            
            var header = new Header
            {
                MagicCode = 0x4D494E41U,
                Reserved = 0,
                Version = 2,
                Flags = (ushort)(HeaderFlags.SpriteSheet |
                    HeaderFlags.Frames |
                    HeaderFlags.FramesEx |
                    HeaderFlags.Animations |
                    HeaderFlags.AnimationReferences),

                SpriteSheetsLength = (ushort)(spriteSheetSectionLength + spriteSheetPaddingData),
                SpriteSheetsCount = (ushort)AnimationsGroup.Textures.Count,
                FramesLength = 12,
                FramesCount = (ushort)listFrames.Count,
                FrameExLength = 12,
                FrameExCount = 0,
                AnimationsLength = 16,
                AnimationsCount = (ushort)listAnimations.Count,
                AnimationReferencesLength = 0,
                AnimationReferencesCount = (ushort)listAnimationRefs.Count
            };
            #endregion

            #region write data
            // Write header
            writer.Write(header.MagicCode);
            writer.Write(header.Reserved);
            writer.Write(header.Version);
            writer.Write(header.Flags);
            writer.Write(header.SpriteSheetsLength);
            writer.Write(header.SpriteSheetsCount);
            writer.Write(header.FramesLength);
            writer.Write(header.FramesCount);
            writer.Write(header.FrameExLength);
            writer.Write(header.FrameExCount);
            writer.Write(header.AnimationsLength);
            writer.Write(header.AnimationsCount);
            writer.Write(header.AnimationReferencesLength);
            writer.Write(header.AnimationReferencesCount);

            // Write spritesheets
            foreach (var item in spriteSheets)
                writer.Write(item);
            for (int i = 0; i < spriteSheetPaddingData; i++)
                writer.Write((byte)0);

            // Write frames
            foreach (var item in listFrames.Values)
            {
                var frame = item.Item2;
                writer.Write((ushort)frame.Left);
                writer.Write((ushort)frame.Top);
                writer.Write((ushort)frame.Right);
                writer.Write((ushort)frame.Bottom);
                writer.Write((short)frame.CenterX);
                writer.Write((short)frame.CenterY);
            }

            // Write animations
            foreach (var item in listAnimations.Values)
            {
                var anim = item.Item2;
                writer.Write((ushort)anim.Frames.Count);
                writer.Write((ushort)anim.Speed);
                writer.Write((byte)anim.Loop);
                writer.Write((byte)0);
                writer.Write((ushort)0);
                writer.Write((ushort)anim.FieldHitbox.Left);
                writer.Write((ushort)anim.FieldHitbox.Top);
                writer.Write((ushort)anim.FieldHitbox.Right);
                writer.Write((ushort)anim.FieldHitbox.Bottom);
                foreach (var frame in anim.Frames)
                {
                    int index;
                    if (listFrames.TryGetValue(frame.Frame, out var tuple))
                        index = tuple.Item1;
                    else
                        index = 0;

                    int flags = 0;
                    if (frame.FlipX) flags |= 1;
                    if (frame.FlipY) flags |= 2;
                    if (frame.Trigger) flags |= 4;

                    writer.Write((ushort)index);
                    writer.Write((ushort)flags);
                    writer.Write((ushort)frame.Hitbox.Left);
                    writer.Write((ushort)frame.Hitbox.Top);
                    writer.Write((ushort)frame.Hitbox.Right);
                    writer.Write((ushort)frame.Hitbox.Bottom);
                }
            }

            // Write animation reference names
            foreach (var item in listAnimationRefs)
            {
                /*var data = Encoding.ASCII.GetBytes(item.Name);
                var hash = Security.Crc32.CalculateDigest(data, 0, (uint)data.Length);
                writer.Write(hash);*/
            }

            // Write animation reference data
            foreach (var item in listAnimationRefs)
            {
                var animIndex = listAnimations[item.Animation].Item1;

                int direction = (int)item.Direction;
                if (item.IsDiagonal)
                    direction |= 0x80;
                int flags = 0;
                if (item.FlipX) flags |= 1;
                if (item.FlipY) flags |= 2;

                writer.Write((ushort)animIndex);
                writer.Write((byte)direction);
                writer.Write((byte)flags);
            }
            #endregion
        }
    }
}
