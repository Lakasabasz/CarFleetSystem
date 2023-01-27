using System.Windows;

namespace CarFleetManager;

public partial class MainWindowCtx
{
    
    private bool _carTabAccess;
    private bool CarTabAccess
    {
        get => _carTabAccess;
        set
        {
            _carTabAccess = value;
            OnPropertyChanged(nameof(CarTabVisibility));
        }
    }
    public Visibility CarTabVisibility => CarTabAccess ? Visibility.Visible : Visibility.Collapsed;
}