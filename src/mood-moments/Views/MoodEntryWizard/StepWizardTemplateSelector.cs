using Microsoft.Maui.Controls;

namespace mood_moments.Views.MoodEntryWizard
{
    public class StepWizardTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? CoreEmotionTemplate { get; set; }
        public DataTemplate? MidEmotionTemplate { get; set; }
        public DataTemplate? NuancedEmotionTemplate { get; set; }
        public DataTemplate? IntensityTemplate { get; set; }
        public DataTemplate? NoteTemplate { get; set; }
        public DataTemplate? ContextTemplate { get; set; }
        public DataTemplate? TriggerTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is int step)
            {
                return step switch
                {
                    0 => CoreEmotionTemplate!,
                    1 => MidEmotionTemplate!,
                    2 => NuancedEmotionTemplate!,
                    3 => IntensityTemplate!,
                    4 => NoteTemplate!,
                    5 => ContextTemplate!,
                    6 => TriggerTemplate!,
                    _ => CoreEmotionTemplate!
                };
            }
            return CoreEmotionTemplate!;
        }
    }
}
