using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var listAnimationRefs = new List<AnimationReference>();
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
            
            int index;

            var dicFrames = new Dictionary<string, Tuple<int, Frame>>();
            index = 0;
            foreach (var frame in AnimationData.Frames)
                dicFrames.Add(frame.Name, new Tuple<int, Frame>(index++, frame));

            var dicAnimations = new Dictionary<string, Tuple<int, Game.Animations.Animation>>();
            index = 0;
            foreach (var anim in AnimationData.Animations)
                dicAnimations.Add(anim.Name, new Tuple<int, Game.Animations.Animation>(index++, anim));

            foreach (var tuple in dicAnimations.Values)
            {
                var anim = tuple.Item2;
                foreach (var frameRef in anim.Frames)
                {
                    if (!dicFrames.TryGetValue(frameRef.Frame, out var frame))
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
            foreach (var texture in AnimationData.Textures)
            {
                var data = Encoding.ASCII.GetBytes(texture.Name);
                spriteSheets.Add(data);
                spriteSheetSectionLength += data.Length + 1;
            }
            var spriteSheetPaddingData = 4 - (spriteSheetSectionLength % 4);
            if (spriteSheetPaddingData == 4)
                spriteSheetPaddingData = 0; 

            var header = new Header
            {
                MagicCode = 0x4D494E41U,
                Reserved = 0,
                Version = 2,
                Flags = (ushort)(HeaderFlags.SpriteSheet |
                    HeaderFlags.Frames |
                    HeaderFlags.Animations |
                    HeaderFlags.AnimationReferences),

                SpriteSheetsLength = (ushort)(spriteSheetSectionLength + spriteSheetPaddingData),
                SpriteSheetsCount = (ushort)AnimationData.Textures.Count,
                FramesLength = 12,
                FramesCount = (ushort)dicFrames.Count,
                AnimationsLength = 16,
                AnimationsCount = (ushort)dicAnimations.Count,
                AnimationReferencesLength = 4 * 5,
                AnimationReferencesCount = (ushort)AnimationData.AnimationDefinitions.Count
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
            writer.Write(header.AnimationsLength);
            writer.Write(header.AnimationsCount);
            writer.Write(header.AnimationReferencesLength);
            writer.Write(header.AnimationReferencesCount);

            // Write spritesheets
            foreach (var item in spriteSheets)
            {
                writer.Write(item);
                writer.Write((byte)0x00);
            }
            for (int i = 0; i < spriteSheetPaddingData; i++)
                writer.Write((byte)0);

            // Write frames
            foreach (var item in dicFrames.Values)
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
            foreach (var item in dicAnimations.Values)
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
                    if (dicFrames.TryGetValue(frame.Frame, out var tuple))
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

            // Order animations by hash id
            var animationDefinitions = AnimationData.AnimationDefinitions
                .Select(x => new
                {
                    Key = x.Name.GetXeHash(),
                    Value = x
                })
                .OrderBy(x => x.Key);

            // Write animation reference names
            foreach (var item in animationDefinitions)
            {
                writer.Write(item.Key);
            }

            // Write animation reference data
            foreach (var item in animationDefinitions)
            {
                var value = item.Value;
                ExportAnimRef(writer, dicAnimations, value.Default);
                ExportAnimRef(writer, dicAnimations, value.DirectionUp);
                ExportAnimRef(writer, dicAnimations, value.DirectionRight);
                ExportAnimRef(writer, dicAnimations, value.DirectionDown);
                ExportAnimRef(writer, dicAnimations, value.DirectionLeft);
            }
            #endregion
        }

        private void ExportAnimRef(BinaryWriter w, Dictionary<string, Tuple<int, Xe.Game.Animations.Animation>> anims, AnimationReference animationReference)
        {
            if (animationReference != null && animationReference.Name != null &&
                anims.TryGetValue(animationReference.Name, out var tuple))
            {
                var animIndex = (short)tuple.Item1;
                short flags = (short)((animationReference.FlipX ? 1 : 0) |
                    (animationReference.FlipY ? 2 : 0));
                w.Write(animIndex);
                w.Write(flags);
            }
            else
            {
                w.Write((ushort)0xFFFF);
                w.Write((ushort)0xFFFF);
            }
        }
    }
}
