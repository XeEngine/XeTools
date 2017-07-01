using System.Collections.Generic;
using System.IO;
using Xe;

namespace libTools.Anim
{
    partial class AnimationsGroup
    {
        private class AnimHash
        {
            private uint mHash;
            private Animation mAnimation;

            public uint Hash { get { return mHash; } }
            public Animation Animation { get { return mAnimation; } }

            public AnimHash(Animation animation)
            {
                mHash = Xe.Security.Crc32.CalculateDigestAscii(animation.Name);
                mAnimation = animation;
            }
        }

        private class AnimHashComparer : IComparer<AnimHash>
        {
            public int Compare(AnimHash x, AnimHash y)
            {
                if (x.Hash > y.Hash) return 1;
                else if (x.Hash < y.Hash) return -1;
                else return 0;
            }
        }

        protected override void Export(BinaryWriter writer)
        {
            var index = 0;
            var listAnimations = new List<AnimHash>(Animations.Count);
            foreach (var item in Animations)
                listAnimations.Add(new AnimHash(item));
            listAnimations.Sort(new AnimHashComparer());

            const uint MAGIC_CODE = 0x4D494E41U;
            writer.Write(MAGIC_CODE);
            writer.Write((ushort)SpriteSheet.Count);
            writer.Write((ushort)Frames.Count);
            writer.Write((ushort)Animations.Count);

            int len = 0;
            foreach (var name in SpriteSheet)
                len += name.Length + 1;
            writer.Write((ushort)(len + 16));
            writer.Write((ushort)0); // RESERVED
            writer.Write((ushort)0); // RESERVED

            foreach (var name in SpriteSheet)
            {
                var data = System.Text.Encoding.ASCII.GetBytes(name);
                writer.Write(data);
                writer.Write((byte)0);
            }

            var dicFrames = new Dictionary<string, int>();
            foreach (var frame in Frames.Values)
            {
                writer.Write((ushort)frame.Left);
                writer.Write((ushort)frame.Top);
                writer.Write((ushort)frame.Right);
                writer.Write((ushort)frame.Bottom);
                writer.Write((short)frame.CenterX);
                writer.Write((short)frame.CenterY);
                dicFrames.Add(frame.Name, index++);
            }

            var sortedAnimsId = new SortedList<uint, Animation>();
            foreach (var anim in Animations)
                sortedAnimsId.Add(Xe.Security.Crc32.CalculateDigestAscii(anim.Name), anim);
            foreach (var animId in sortedAnimsId.Keys)
                writer.Write(animId);

            var pos = (int)writer.BaseStream.Position;
            writer.BaseStream.Position += Animations.Count * sizeof(uint);

            index = 0;
            int curPos = (int)writer.BaseStream.Position;
            var animsPos = new int[Animations.Count];
            foreach (var curanim in sortedAnimsId.Values)
            {
                var anim = curanim;
                if (anim.Link != null)
                {
                    while (anim.Link != null)
                    {
                        uint hash = Xe.Security.Crc32.CalculateDigestAscii(anim.Link);
                        if (!sortedAnimsId.TryGetValue(hash, out anim))
                        {
                            Log.Error(string.Format("Unable to link animation {0} from {1}.", anim.Link, curanim.Name));
                            return;
                        }
                    };
                }

				// Normalize values
				if (anim.Sequence.Loop > anim.Sequence.Frames.Count)
					anim.Sequence.Loop = byte.MaxValue;
				if (anim.Sequence.Event > anim.Sequence.Frames.Count)
					anim.Sequence.Event = byte.MaxValue;
				// Write values
				writer.Write((short)anim.Sequence.HitboxLeft);
                writer.Write((short)anim.Sequence.HitboxTop);
                writer.Write((short)anim.Sequence.HitboxRight);
                writer.Write((short)anim.Sequence.HitboxBottom);
                writer.Write((ushort)anim.Sequence.Speed);
                writer.Write((ushort)anim.Sequence.Frames.Count);
				writer.Write((byte)anim.Sequence.Loop);
				writer.Write((byte)anim.Sequence.Event);
				writer.Write((byte)anim.Sequence.Texture);
                writer.Write((byte)0); // RESERVED
                foreach (var frame in anim.Sequence.Frames)
                {
                    int frameIndex = 0;
                    if (!dicFrames.TryGetValue(frame, out frameIndex))
                    {
                        // ECCEZIONE NON GESTITA?
                    }
                    writer.Write((ushort)frameIndex);
                }
                animsPos[index++] = curPos;
                curPos += 16 + anim.Sequence.Frames.Count * sizeof(ushort);
            }

            writer.BaseStream.Position = pos;
            foreach (var animPos in animsPos)
                writer.Write((uint)animPos);
        }
    }
}
