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
        private NewEntryWizardViewModel? ViewModel => BindingContext as NewEntryWizardViewModel;

        public NewEntryWizardPage()
        {
            InitializeComponent();

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
            if (ViewModel != null)
            {
                // Subscribe to ViewModel property changes and wizard completion event.
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
                ViewModel.WizardFinished += OnWizardFinished;
                // Set the initial step content based on the current step.
                SetStepContent(ViewModel.CurrentStep);
            }
        }

        // Called when the page is about to disappear.
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (ViewModel != null)
            {
                // Unsubscribe from events to prevent memory leaks.
                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                ViewModel.WizardFinished -= OnWizardFinished;
            }
        }

        // Called when the BindingContext (ViewModel) changes.
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (ViewModel != null)
            {
                // Ensure event handlers are attached only once.
                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
                ViewModel.WizardFinished -= OnWizardFinished;
                ViewModel.WizardFinished += OnWizardFinished;
                // Update the step content for the new ViewModel.
                SetStepContent(ViewModel.CurrentStep);
            }
        }

        // Handler for when the wizard is finished.
        private void OnWizardFinished()
        {
            // Ensure navigation happens on the main UI thread.
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Navigation.NavigationStack.Count > 0)
                    await Navigation.PopAsync(); // Navigate back to the previous page.
            });
        }

        // Handles property changes in the ViewModel.
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // If the current step or emotion selection changes, update the step content.
            if ((e.PropertyName == nameof(NewEntryWizardViewModel.CurrentStep) ||
                 e.PropertyName == nameof(NewEntryWizardViewModel.EmotionSelection)) && ViewModel != null)
            {
                SetStepContent(ViewModel.CurrentStep);
            }
        }

        // Dynamically sets the content of the wizard step based on the current step index.
        private void SetStepContent(int step)
        {
            if (StepHost == null || ViewModel == null)
                return;
            // Switch statement selects the appropriate view for each step.
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
                content.BindingContext = ViewModel; // Bind the step view to the same ViewModel.
            StepHost.Content = content; // Display the selected step view.
        }
    }
}
