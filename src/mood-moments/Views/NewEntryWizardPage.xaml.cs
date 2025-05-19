using mood_moments.Models;
using mood_moments.ViewModels;
using mood_moments.Views.MoodEntryWizard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace mood_moments.Views
{
    public partial class NewEntryWizardPage : ContentPage
    {
        private NewEntryWizardViewModel? ViewModel => BindingContext as NewEntryWizardViewModel;

        public NewEntryWizardPage()
        {
            InitializeComponent();
            // Removed navigation bar back button code
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
                ViewModel.WizardFinished += OnWizardFinished;
                SetStepContent(ViewModel.CurrentStep);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                ViewModel.WizardFinished -= OnWizardFinished;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
                ViewModel.WizardFinished -= OnWizardFinished;
                ViewModel.WizardFinished += OnWizardFinished;
                SetStepContent(ViewModel.CurrentStep);
            }
        }

        private void OnWizardFinished()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Navigation.NavigationStack.Count > 0)
                    await Navigation.PopAsync();
            });
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewEntryWizardViewModel.CurrentStep) && ViewModel != null)
            {
                SetStepContent(ViewModel.CurrentStep);
            }
        }

        private void SetStepContent(int step)
        {
            if (StepHost == null || ViewModel == null)
                return;
            View? content = step switch
            {
                0 => new EmotionStep { BindingContext = ViewModel },
                1 => new MidEmotionStep { BindingContext = ViewModel },
                2 => new NuanceEmotionStep { BindingContext = ViewModel },
                3 => new IntensityStep { BindingContext = ViewModel },
                4 => new NoteStep { BindingContext = ViewModel },
                5 => new ContextStep { BindingContext = ViewModel },
                6 => new TriggerStep { BindingContext = ViewModel },
                _ => null
            };
            StepHost.Content = content;
        }
    }
}
