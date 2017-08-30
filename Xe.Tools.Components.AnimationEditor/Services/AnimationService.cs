using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Xe.Game.Animations;

namespace Xe.Tools.Components.AnimationEditor.Services
{
    public class AnimationService
    {
        private static readonly int TIMESTEP = 21600; // 5^2 + 3^3 + 2^5

        private string _animationName;
        private int _frameIndex;
        private Animation _currentAnimation;
        private Dictionary<string, Frame> _dicFrames;
        private Dictionary<string, Animation> _dicAnimations;

        public event Action<AnimationService> OnFrameChanged;

        public AnimationData AnimationData { get; private set; }

        public Timer Timer { get; private set; }
        public Stopwatch Stopwatch { get; private set; }

        /// <summary>
        /// Get or set if the animation system is running
        /// </summary>
        public bool IsRunning
        {
            get => Timer.Enabled;
            set
            {
                if (Timer.Enabled != value)
                {
                    if (value)
                    {
                        Stopwatch.Start();
                    }
                    else
                    {
                        Stopwatch.Stop();
                    }
                    Timer.Enabled = value;
                }
            }
        }

        /// <summary>
        /// Get the number of frames per second of selected animation
        /// </summary>
        public int FramesPerSecond
        {
            get
            {
                var speed = CurrentAnimation?.Speed ?? 0;
                return speed > 0 ? TIMESTEP / speed : 0;
            }
        }

        /// <summary>
        /// Get or set the name of current animation
        /// </summary>
        public string Animation
        {
            get => _animationName;
            set
            {
                _animationName = value;
                // Validate the name
                if (!string.IsNullOrWhiteSpace(value))
                {
                    // Get the animation object from its name
                    if (_dicAnimations.TryGetValue(value, out _currentAnimation))
                    {
                        // Reset the timer
                        if (IsRunning)
                        {
                            Stopwatch.Restart();
                        }
                        else
                        {
                            Stopwatch.Reset();
                        }
                        // Reset the frame index
                        _frameIndex = -1;
                        FrameIndex = 0;
                    }
                }
                else
                {
                    Stopwatch.Reset();
                }
            }
        }

        /// <summary>
        /// Get or set the current frame index
        /// </summary>
        public int FrameIndex
        {
            get => _frameIndex;
            set
            {
                if (_frameIndex != value)
                {
                    _frameIndex = value;
                    OnFrameChanged?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Get the current animation object loaded
        /// </summary>
        public Animation CurrentAnimation => _currentAnimation;

        /// <summary>
        /// Get the current frame reference
        /// </summary>
        public FrameRef CurrentFrameReference
        {
            get
            {
                if (FrameIndex >= 0 && _currentAnimation != null &&
                    FrameIndex < _currentAnimation.Frames.Count)
                    return _currentAnimation.Frames[FrameIndex];
                return null;
            }
        }

        /// <summary>
        /// Get the current frame from frame reference
        /// </summary>
        public Frame CurrentFrame
        {
            get
            {
                var currentFrameReference = CurrentFrameReference;
                if (currentFrameReference == null ||
                    currentFrameReference.Frame == null) 
                    return null;
                _dicFrames.TryGetValue(currentFrameReference.Frame, out Frame frame);
                return frame;
            }
        }

        /// <summary>
        /// Initialize a new instance of an animation service
        /// </summary>
        /// <param name="animData">Animation data where information are loaded</param>
        public AnimationService(AnimationData animData)
        {
            AnimationData = animData;
            Timer = new Timer(1);
            Timer.Elapsed += Timer_Elapsed;
            Stopwatch = new Stopwatch();

            _dicFrames = AnimationData.Frames
                .ToDictionary(x => x.Name, x => x);
            _dicAnimations = AnimationData.Animations
                .ToDictionary(x => x.Name, x => x);

            Timer.Enabled = true;
        }

        /// <summary>
        /// Change the frame's reference from the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="frame"></param>
        public void SetFrame(int index, string frameName)
        {
            var framesList = CurrentAnimation?.Frames;
            if (framesList != null)
            {
                if (index >= 0 && index < framesList.Count)
                {
                    framesList[index].Frame = frameName;
                    if (FrameIndex == index)
                    {
                        OnFrameChanged?.Invoke(this);
                    }
                }
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (CurrentAnimation == null) return;
            if (IsRunning == false) return;
            if (FramesPerSecond <= 0) return;

            double freq = 1.0 / (CurrentAnimation.Speed / 21600.0);
            double timer = Stopwatch.ElapsedMilliseconds;
            var index = Math.Floor(timer / (1000.0 / freq));
            if (index >= 0)
            {
                var curAnim = CurrentAnimation;
                if (index >= curAnim.Frames.Count)
                {
                    if (curAnim.Loop == 0)
                    {
                        FrameIndex = index % curAnim.Frames.Count;
                    }
                    else if (curAnim.Loop < curAnim.Frames.Count)
                    {
                        FrameIndex = index - (curAnim.Frames.Count - curAnim.Loop) % curAnim.Loop;
                    }
                }
                else
                {
                    FrameIndex = index;
                }
            }
        }
    }
}
