using System.Windows;
using CarFleetManager.models;

namespace CarFleetManager;

public partial class MainWindowCtx
{
    private bool _carTabAccess;
    private bool _carTabActive;
    private int _currentCarIndex;

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

    public bool CarTabActive
    {
        get => _carTabActive;
        set
        {
            if (value == _carTabActive) return;
            _carTabActive = value;
            OnPropertyChanged(nameof(CarTabActiveVisibility));
        }
    }

    public int CurrentCarIndex
    {
        get => _currentCarIndex;
        set
        {
            if (value == _currentCarIndex) return;
            _currentCarIndex = value;
            OnPropertyChanged();
        }
    }

    public Visibility CarTabActiveVisibility => CarTabActive ? Visibility.Visible : Visibility.Collapsed;
    public CarData? CurrentCar { get; set; }
}