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
        private Frame _currentFrame;
        private Dictionary<string, Frame> _dicFrames;
        private Dictionary<string, Animation> _dicAnimations;

        public event Action<AnimationService> OnFrameChanged;

        public AnimationData AnimationData { get; private set; }

        public Timer Timer { get; private set; }
        public Stopwatch Stopwatch { get; private set; }

        public bool IsRunning
        {
            get => Timer.Enabled;
            set
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

        public int FramesPerSecond
        {
            get
            {
                var speed = CurrentAnimation?.Speed ?? 0;
                return speed > 0 ? TIMESTEP / speed : 0;
            }
        }

        public string Animation
        {
            get => _animationName;
            set
            {
                if (value != null)
                {
                    if (_dicAnimations.TryGetValue(_animationName = value, out _currentAnimation))
                    {
                        if (IsRunning)
                        {
                            Stopwatch.Restart();
                        }
                        else
                        {
                            Stopwatch.Reset();
                        }
                        var frameRef = CurrentAnimation.Frames[0];
                        if (frameRef != null)
                            _dicFrames.TryGetValue(frameRef.Frame, out _currentFrame);
                        OnFrameChanged?.Invoke(this);
                    }
                }
                else
                {
                    Stopwatch.Reset();
                }
            }
        }

        public int FrameIndex
        {
            get => _frameIndex;
            set
            {
                if (_frameIndex != value)
                {
                    _frameIndex = value;
                    var frameRef = CurrentAnimation.Frames[_frameIndex];
                    if (frameRef != null)
                        _dicFrames.TryGetValue(frameRef.Frame, out _currentFrame);
                    OnFrameChanged?.Invoke(this);
                }
            }
        }

        public Animation CurrentAnimation => _currentAnimation;

        public Frame CurrentFrame => _currentFrame;

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

        public void SetFrame(int index, Frame frame)
        {
            var framesList = CurrentAnimation?.Frames;
            if (framesList != null)
            {
                if (index >= 0 && index < framesList.Count)
                {
                    framesList[index].Frame = frame.Name;
                    if (FrameIndex == index)
                    {
                        _currentFrame = frame;
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
