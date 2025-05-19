using Microsoft.Maui.Controls;

namespace mood_moments.Views.MoodEntryWizard
{
    public partial class MidEmotionStep : ContentView
    {
        public MidEmotionStep()
        {
            InitializeComponent();
        }

        public void OnMidEmotionCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value && sender is RadioButton rb && rb.Content is string emotion && BindingContext is mood_moments.ViewModels.NewEntryWizardViewModel vm && vm.SelectMidEmotionCommand.CanExecute(emotion))
            {
                vm.SelectMidEmotionCommand.Execute(emotion);
            }
        }
    }
}
