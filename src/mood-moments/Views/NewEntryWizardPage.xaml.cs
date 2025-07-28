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
        public NewEntryWizardViewModel ViewModel { get; } = new NewEntryWizardViewModel();
        private mood_moments.ViewModels.MoodEntryWizard.DomainSelectionViewModel? _domainVM;
        private mood_moments.ViewModels.MoodEntryWizard.ContextStepViewModel? _contextStepVM;
        private mood_moments.ViewModels.MoodEntryWizard.TriggerStepViewModel? _triggerStepVM;

        // Only one constructor should exist
        // ...existing code...

        public NewEntryWizardPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
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
        private readonly mood_moments.Services.ContextAndTriggersService _contextService = new();

        private void SetStepContent(int step)
        {
            View? content = null;
            switch (step)
            {
                case 0:
                    content = new EmotionStep { BindingContext = ViewModel };
                    break;
                case 1:
                    content = new MidEmotionStep { BindingContext = ViewModel };
                    break;
                case 2:
                    content = new NuanceEmotionStep { BindingContext = ViewModel };
                    break;
                case 3:
                    content = new IntensityStep { BindingContext = ViewModel };
                    break;
                case 4:
                    content = new NoteStep { BindingContext = ViewModel };
                    break;
                case 5:
                    if (_domainVM == null)
                    {
                        _domainVM = new mood_moments.ViewModels.MoodEntryWizard.DomainSelectionViewModel(_contextService);
                        _domainVM.PropertyChanged += (s, e) =>
                        {
                            if (e.PropertyName == nameof(mood_moments.ViewModels.MoodEntryWizard.DomainSelectionViewModel.SelectedDomain) && _domainVM.SelectedDomain != null)
                            {
                                ViewModel.SelectedDomainFile = _domainVM.SelectedDomain.FileName;
                                _contextStepVM = null;
                                _triggerStepVM = null;
                            }
                        };
                    }
                    content = new mood_moments.Views.MoodEntryWizard.DomainSelectionStep { BindingContext = _domainVM };
                    break;
                case 6:
                    if (_contextStepVM == null || _contextStepVM.DomainFileName != ViewModel.SelectedDomainFile)
                    {
                        _contextStepVM = new mood_moments.ViewModels.MoodEntryWizard.ContextStepViewModel(_contextService, ViewModel.SelectedDomainFile);
                        _contextStepVM.PropertyChanged += (s, e) =>
                        {
                            if (e.PropertyName == nameof(mood_moments.ViewModels.MoodEntryWizard.ContextStepViewModel.Context))
                            {
                                ViewModel.SelectedContextName = _contextStepVM.Context;
                                _triggerStepVM = null;
                            }
                        };
                    }
                    content = new ContextStep { BindingContext = _contextStepVM };
                    break;
                case 7:
                    if (_triggerStepVM == null || _triggerStepVM.DomainFileName != ViewModel.SelectedDomainFile || _triggerStepVM.SelectedContext != ViewModel.SelectedContextName)
                    {
                        _triggerStepVM = new mood_moments.ViewModels.MoodEntryWizard.TriggerStepViewModel(_contextService, ViewModel.SelectedDomainFile, ViewModel.SelectedContextName);
                    }
                    content = new TriggerStep { BindingContext = _triggerStepVM };
                    break;
                default:
                    content = null;
                    break;
            }
            StepContent = content;
        }

        private View? _stepContent;
        public View? StepContent
        {
            get => _stepContent;
            set
            {
                if (_stepContent != value)
                {
                    _stepContent = value;
                    // Notify the ContentPresenter in XAML, which now binds to the code-behind property via x:Reference
                    OnPropertyChanged(nameof(StepContent));
                }
            }
        }
    }
}
