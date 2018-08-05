using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Xe.Game;
using Xe.Game.Particles;
using Xe.Tools.Components.ParticleEditor.ViewModels;

namespace Xe.Tools.Components.ParticleEditor.Controls
{
	public enum ExecutionState
	{
		Ready, Running, Finished
	}

	/// <summary>
	/// Interaction logic for Effect.xaml
	/// </summary>
	public partial class Effect : UserControl, INotifyPropertyChanged
	{
		public EnumViewModel<Ease> AlgorithmTypes { get; private set; } = new EnumViewModel<Ease>();

		public EnumViewModel<ParameterType> ParameterTypes { get; private set; } = new EnumViewModel<ParameterType>();

		public static readonly DependencyProperty IsExecutionEnabledProperty =
			DependencyProperty.Register(
				"IsExecutionEnabled",
				typeof(bool),
				typeof(Effect),
				new PropertyMetadata(true, new PropertyChangedCallback(OnGenericPropertyChanged)),
				new ValidateValueCallback(ValidateBoolean));

		public static readonly DependencyProperty ExecutionStateProperty =
			DependencyProperty.Register(
				"ExecutionState",
				typeof(ExecutionState),
				typeof(Effect),
				new PropertyMetadata(ExecutionState.Ready, new PropertyChangedCallback(OnGenericPropertyChanged)),
				new ValidateValueCallback(ValidateExecutionState));

		public static readonly DependencyProperty ParameterProperty =
			DependencyProperty.Register(
				"ParameterType",
				typeof(ParameterType),
				typeof(Effect),
				new PropertyMetadata(ParameterType.X, new PropertyChangedCallback(OnGenericPropertyChanged)),
				new ValidateValueCallback(ValidateParameterType));

		public static readonly DependencyProperty AlgorithmProperty =
			DependencyProperty.Register(
				"Algorithm",
				typeof(Ease),
				typeof(Effect),
				new PropertyMetadata(Ease.Linear, new PropertyChangedCallback(OnGenericPropertyChanged)),
				new ValidateValueCallback(ValidateAlgorithm));

		public static readonly DependencyProperty FixStepProperty =
			DependencyProperty.Register(
				"FixStep",
				typeof(double),
				typeof(Effect),
				new PropertyMetadata(0.0, new PropertyChangedCallback(OnGenericPropertyChanged)),
				new ValidateValueCallback(ValidateNumeric));

		public static readonly DependencyProperty SpeedProperty =
			DependencyProperty.Register(
				"Speed",
				typeof(double),
				typeof(Effect),
				new PropertyMetadata(1.0, new PropertyChangedCallback(OnGenericPropertyChanged)),
				new ValidateValueCallback(ValidateNumeric));

		public static readonly DependencyProperty MultiplierProperty =
			DependencyProperty.Register(
				"Multiplier",
				typeof(double),
				typeof(Effect),
				new PropertyMetadata(1.0, new PropertyChangedCallback(OnGenericPropertyChanged)),
				new ValidateValueCallback(ValidateNumeric));

		public static readonly DependencyProperty DelayProperty =
			DependencyProperty.Register(
				"Delay",
				typeof(double),
				typeof(Effect),
				new PropertyMetadata(0.0, new PropertyChangedCallback(OnGenericPropertyChanged)),
				new ValidateValueCallback(ValidateNumeric));

		public static readonly DependencyProperty DurationProperty =
			DependencyProperty.Register(
				"Duration",
				typeof(double),
				typeof(Effect),
				new PropertyMetadata(10.0, new PropertyChangedCallback(OnGenericPropertyChanged)),
				new ValidateValueCallback(ValidateNumeric));

		public event PropertyChangedEventHandler PropertyChanged;

		public event Action<Effect> RequireRemoval;

		public Visibility StateReadyVisibility => ExecutionState == ExecutionState.Ready ? Visibility.Visible : Visibility.Collapsed;

		public Visibility StateRunningVisibility => ExecutionState == ExecutionState.Running ? Visibility.Visible : Visibility.Collapsed;

		public Visibility StateFinishedVisibility => ExecutionState == ExecutionState.Finished ? Visibility.Visible : Visibility.Collapsed;

		public bool IsExecutionEnabled
		{
			get => (bool)GetValue(IsExecutionEnabledProperty);
			set => SetValue(IsExecutionEnabledProperty, value);
		}

		public ExecutionState ExecutionState
		{
			get => (ExecutionState)GetValue(ExecutionStateProperty);
			set => SetValue(ExecutionStateProperty, value);
		}

		public ParameterType ParameterType
		{
			get => (ParameterType)GetValue(ParameterProperty);
			set
			{
				SetValue(ParameterProperty, value);
				OnPropertyChanged(nameof(StateReadyVisibility));
				OnPropertyChanged(nameof(StateRunningVisibility));
				OnPropertyChanged(nameof(StateFinishedVisibility));
			}
		}

		public Ease Algorithm
		{
			get => (Ease)GetValue(AlgorithmProperty);
			set => SetValue(AlgorithmProperty, value);
		}

		public double FixStep
		{
			get => (double)GetValue(FixStepProperty);
			set => SetValue(FixStepProperty, value);
		}

		public double Speed
		{
			get => (double)GetValue(SpeedProperty);
			set => SetValue(SpeedProperty, value);
		}

		public double Multiplier
		{
			get => (double)GetValue(MultiplierProperty);
			set => SetValue(MultiplierProperty, value);
		}

		public double Delay
		{
			get => (double)GetValue(DelayProperty);
			set => SetValue(DelayProperty, value);
		}

		public double Duration
		{
			get => (double)GetValue(DurationProperty);
			set => SetValue(DurationProperty, value);
		}

		public ParticleEffectsViewModel ViewModel => DataContext as ParticleEffectsViewModel;

		public Effect()
		{
			InitializeComponent();
			DataContext = this;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#region private methods

		private static void OnGenericPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Effect)?.OnPropertyChanged(nameof(e.Property.Name));
		}
		private static bool ValidateNumeric(object value)
		{
			return value is double;
		}
		private static bool ValidateBoolean(object value)
		{
			return value is bool;
		}
		private static bool ValidateParameterType(object value)
		{
			return value is ParameterType;
		}
		private static bool ValidateAlgorithm(object value)
		{
			return value is Ease;
		}
		private static bool ValidateExecutionState(object value)
		{
			return value is ExecutionState;
		}

		private void ButtonRemove_Click(object sender, RoutedEventArgs e)
		{
			RequireRemoval?.Invoke(this);
		}

		#endregion
	}
}
