using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Animations;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class AnimationsViewModel
    {
        private AnimationData _animationData;
        private List<AnimationViewModel> _animations;

        public Project Project { get; private set; }
        public Project.Container Container { get; private set; }
        public Project.Item Item { get; private set; }

        public string Name => Item.Input;

        public List<AnimationViewModel> Animations
        {
            get
            {
                if (IsValid == null)
                {
                    LoadAnimations();
                }
                return _animations;
            }
        }

        public bool? IsValid { get; private set; }

        public AnimationsViewModel(Project project, Project.Container container, Project.Item item)
        {
            Project = project;
            Container = container;
            Item = item;
        }

        public void LoadAnimations()
        {
            var filePath = Path.Combine(Project.ProjectPath, Path.Combine(Container.Name, Item.Input));
            if (File.Exists(filePath))
            {
                try
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        _animationData = JsonConvert.DeserializeObject<AnimationData>(reader.ReadToEnd());
                        _animations = _animationData.Animations?
                            .Select(x => new AnimationViewModel(x))
                            .ToList();
                        IsValid = true;
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Unable to process item {Item.Input}: {e.Message}");
                }
            }
            else
            {
                Log.Warning($"File {filePath} does not exists.");
            }
            IsValid = false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
