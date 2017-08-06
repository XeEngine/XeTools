using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xe.Game;
using Xe.Game.Animations;

namespace Xe.Tools.Components.AnimationEditor
{
    public static class Utilities
    {
        private class TextureNameComparer : IEqualityComparer<Texture>
        {
            public bool Equals(Texture x, Texture y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode(Texture obj)
            {
                return obj.GetHashCode();
            }
        }
        private class AnimationRefComparer : IEqualityComparer<AnimationRef>
        {
            public bool Equals(AnimationRef x, AnimationRef y)
            {
                return x.Animation == y.Animation &&
                    x.Direction == y.Direction &&
                    x.IsDiagonal == y.IsDiagonal &&
                    x.FlipX == y.FlipX &&
                    x.FlipY == y.FlipY;
            }

            public int GetHashCode(AnimationRef obj)
            {
                return obj.Animation.GetHashCode() ^
                    ((int)obj.Direction << 1 | (int)obj.Direction << 21) |
                    (obj.IsDiagonal ? (1 << 4) : (1 << 25)) |
                    (obj.FlipX ? (1 << 5) : (1 << 27)) |
                    (obj.FlipY ? (1 << 6) : (1 << 29));
            }
        }

        public static void ImportOldAnimation(AnimationData dst, libTools.Anim.AnimationsGroup src)
        {
            ImportSpritesheets(dst, src.SpriteSheet);
            ImportFrames(dst, src.Frames);
            ImportAnimations(dst, src.Animations);
        }

        public static void ImportSpritesheets(AnimationData dst, IEnumerable<string> spriteSheetsFileNames)
        {
            var textures = spriteSheetsFileNames.Select(x => new Texture()
            {
                Id = Guid.NewGuid(),
                Name = Path.GetFileName(x),
                Transparencies = new uint[] { 0xFF00FFFF, 0xFF8000FF },
                MaintainPaletteOrder = true
            });
            dst.Textures = dst.Textures.Union(textures, new TextureNameComparer())
                .ToList();
        }

        public static void ImportFrames(AnimationData dst, Dictionary<string, libTools.Anim.Frame> frames)
        {
            dst.Frames = frames.Select(x => new Frame()
            {
                Name = x.Key,
                Left = x.Value.Left,
                Top = x.Value.Top,
                Right = x.Value.Right,
                Bottom = x.Value.Bottom,
                CenterX = x.Value.CenterX,
                CenterY = x.Value.CenterY
            }).ToList();
        }

        public static void ImportAnimations(AnimationData dst, Collection<libTools.Anim.Animation> animations)
        {
            dst.Animations = animations
                .Where(x => x.Sequence != null)
                .Select(x => new Animation()
            {
                Name = x.Name,
                FieldHitbox = new Hitbox()
                {
                    Left = x.Sequence.HitboxLeft,
                    Top = x.Sequence.HitboxTop,
                    Right = x.Sequence.HitboxRight,
                    Bottom = x.Sequence.HitboxBottom,
                },
                Frames = x.Sequence.Frames.Select(f => new FrameRef()
                {
                    Frame = f,
                    Hitbox = new Hitbox()
                    {
                        Left = x.Sequence.HitboxLeft,
                        Top = x.Sequence.HitboxTop,
                        Right = x.Sequence.HitboxRight,
                        Bottom = x.Sequence.HitboxBottom,
                    },
                    Trigger = false,
                    FlipX = false,
                    FlipY = false
                }).ToList(),
                Speed = x.Sequence.Speed,
                Loop = x.Sequence.Loop,
                Texture = dst.Textures.Select(t => t.Id).FirstOrDefault()
            }).ToList();

            var animGroups = dst.Animations.Select(x => new AnimationRef()
            {
                Name = x.Name.Split('_').First(),
                Animation = x.Name,
                Direction = GetDirectionFromAnimationName(x.Name),
                IsDiagonal = false,
                FlipX = false,
                FlipY = false
            }).ToList();

            dst.AnimationGroups = animGroups
                .Where(x => x.Direction == Direction.Left || x.Direction == Direction.Right)
                .Select(x => new AnimationRef()
                {
                    Name = x.Name,
                    Animation = x.Animation,
                    Direction = x.Direction == Direction.Left ? Direction.Right : Direction.Left,
                    IsDiagonal = false,
                    FlipX = true,
                    FlipY = false
                })
                .Union(animGroups, new AnimationRefComparer())
                .OrderBy(x => x.Animation)
                .ThenBy(x => x.Direction)
                .ToList();
        }

        private static Direction GetDirectionFromAnimationName(string name)
        {
            switch (name.Split('_').Skip(1).FirstOrDefault())
            {
                case "u": return Direction.Up;
                case "d": return Direction.Down;
                case "l": return Direction.Left;
                case "r": return Direction.Right;
                default: return Direction.Undefined;
            }
        }
    }
}
