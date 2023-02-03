using System.Windows.Controls;
using System.Windows;

namespace WpfEssentials.Controls
{
    public class ResetIsEnabled : ContentControl
    {
        static ResetIsEnabled()
        {
            IsEnabledProperty.OverrideMetadata(
                typeof(ResetIsEnabled),
                new UIPropertyMetadata(
                    defaultValue: true,
                    propertyChangedCallback: (_, __) => { },
                    coerceValueCallback: (_, x) => x));
        }
    }
}
