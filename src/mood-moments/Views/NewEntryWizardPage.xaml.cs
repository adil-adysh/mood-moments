using mood_moments.Models;
using mood_moments.ViewModels;
using mood_moments.Views.MoodEntryWizard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace mood_moments.Views
{
    // This partial class represents the code-behind for the NewEntryWizardPage XAML.
    // It handles page lifecycle events, data binding, and dynamic step content.
    public partial class NewEntryWizardPage : ContentPage
    {
        // Strongly-typed ViewModel property for easier access to the BindingContext.
        public NewEntryWizardViewModel ViewModel { get; } = new NewEntryWizardViewModel();

        public NewEntryWizardPage()
        {
            InitializeComponent();
            BindingContext = this;
            // Hide Shell's default back button
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsVisible = false,
                IsEnabled = false
            });
        }

        // Called when the page appears on screen.
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            ViewModel.WizardFinished += OnWizardFinished;
            SetStepContent(ViewModel.CurrentStep);
        }

        // Called when the page is about to disappear.
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.WizardFinished -= OnWizardFinished;
        }

        // Called when the BindingContext (ViewModel) changes.
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            SetStepContent(ViewModel.CurrentStep);
        }

        // Handler for when the wizard is finished.
        private void OnWizardFinished()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Navigation.NavigationStack.Count > 0)
                    await Navigation.PopAsync();
            });
        }

        // Handles property changes in the ViewModel.
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewEntryWizardViewModel.CurrentStep) ||
                e.PropertyName == nameof(NewEntryWizardViewModel.EmotionSelection))
            {
                SetStepContent(ViewModel.CurrentStep);
            }
        }

        // Dynamically sets the content of the wizard step based on the current step index.
        private void SetStepContent(int step)
        {
            View? content = step switch
            {
                0 => new EmotionStep(),
                1 => new MidEmotionStep(),
                2 => new NuanceEmotionStep(),
                3 => new IntensityStep(),
                4 => new NoteStep(),
                5 => new ContextStep(),
                6 => new TriggerStep(),
                _ => null
            };
            if (content != null)
                content.BindingContext = ViewModel;
            StepContent = content;
        }

        // Property for ContentPresenter binding
        private View? _stepContent;
        public View? StepContent
        {
            get => _stepContent;
            set
            {
                if (_stepContent != value)
                {
                    _stepContent = value;
                    OnPropertyChanged(nameof(StepContent));
                }
            }
        }
    }
}
