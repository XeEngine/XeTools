using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Xe.Drawing;
using Xe.Game;
using Xe.Game.Animations;
using Xe.Tools.Projects;
using Xe.Tools.Services;

namespace Xe.Tools.Components.MapEditor.Models
{
    public class AnimationDataEntry
    {
        private IProjectFile _file;
        private AnimationData _animationData;
        private IDrawing _drawing;
        private string _directory;
        private ResourceService<Guid, ISurface> _textures;

        public string Name { get; }

        public IEnumerable<string> Animations => _animationData.AnimationDefinitions
            .Select(x => x.Name);

        public AnimationDataEntry(AnimationService animService, IDrawing drawing,
            string name, IProjectFile file)
        {
            Name = name;
            _drawing = drawing;
            _file = file;
            _animationData = animService.GetAnimationData(file);
            _directory = Path.GetDirectoryName(file.FullPath);
            _textures = new ResourceService<Guid, ISurface>(OnLoad, OnDispose);
            foreach (var k in _animationData.Textures)
            {
                _textures.Add(k.Id);
            }
        }

        public FramesGroup GetAnimation(string animationName, Direction direction = Direction.Undefined)
        {
            var animationRef = _animationData.AnimationDefinitions
                .FirstOrDefault(x => x.Name == animationName);
            if (animationRef == null)
                animationRef = _animationData.AnimationDefinitions.FirstOrDefault();
            if (animationRef == null)
                return null;
            return GetAnimation(animationRef, direction);
        }

        public FramesGroup GetAnimation(AnimationDefinition animationRef, Direction direction = Direction.Undefined)
        {
            AnimationReference animDirectionRef;
            switch (direction)
            {
                case Direction.Undefined:
                    animDirectionRef = animationRef.Default;
                    break;
                case Direction.Up:
                    animDirectionRef = animationRef.DirectionUp;
                    break;
                case Direction.Right:
                    animDirectionRef = animationRef.DirectionRight;
                    break;
                case Direction.Down:
                    animDirectionRef = animationRef.DirectionDown;
                    break;
                case Direction.Left:
                    animDirectionRef = animationRef.DirectionLeft;
                    break;
                default:
                    animDirectionRef = null;
                    break;
            }

            if (animDirectionRef == null || animDirectionRef.Name == null)
            {
                animDirectionRef = animationRef.Default;
                if (animDirectionRef == null || animDirectionRef.Name == null)
                    animDirectionRef = animationRef.DirectionUp;
                if (animDirectionRef == null || animDirectionRef.Name == null)
                    animDirectionRef = animationRef.DirectionRight;
                if (animDirectionRef == null || animDirectionRef.Name == null)
                    animDirectionRef = animationRef.DirectionDown;
                if (animDirectionRef == null || animDirectionRef.Name == null)
                    animDirectionRef = animationRef.DirectionLeft;
            }

            var animation = _animationData.Animations
                .SingleOrDefault(x => x.Name == animDirectionRef?.Name);
            if (animation != null)
            {
                return new FramesGroup()
                {
                    Definition = animationRef,
                    Reference = animDirectionRef,
                    Animation = animation,
                    Texture = _textures[animation.Texture],
                    Frames = animation.Frames.Join(_animationData.Frames,
                        x => x.Frame,
                        x => x.Name,
                        (frameRef, frames) => new Frame()
                        {
                            Source = new System.Windows.Rect()
                            {
                                X = frames.Left,
                                Y = frames.Top,
                                Width = frames.Right - frames.Left,
                                Height = frames.Bottom - frames.Top
                            },
                            Pivot = new System.Windows.Point()
                            {
                                X = frames.CenterX,
                                Y = frames.CenterY
                            }
                        })
                };
            }
            return null;
        }

        private bool OnLoad(Guid key, out ISurface surface)
        {
            var texture = _animationData.Textures.SingleOrDefault(x => x.Id == key);
            if (texture != null)
            {
                var fileName = Path.Combine(_directory, texture.Name);
                if (File.Exists(fileName))
                {
                    var colorFilter = texture.Transparencies.Select(x =>
                            System.Drawing.Color.FromArgb(
                                (byte)(x >> 0),
                                (byte)(x >> 24),
                                (byte)(x >> 16),
                                (byte)(x >> 8)))
                                .ToArray();
                    surface = _drawing.CreateSurface(fileName, colorFilter);
                    return true;
                }
            }
            surface = null;
            return false;
        }

        private void OnDispose(Guid key, ISurface surface)
        {
            surface?.Dispose();
        }

        public static AnimationDataEntry Create(AnimationService animService, IDrawing drawing, string filePath)
        {
            var file = animService.ProjectFiles.SingleOrDefault(x => x.Path == filePath);
            return file != null ? new AnimationDataEntry(animService, drawing, filePath, file) : null;
        }
    }
}
