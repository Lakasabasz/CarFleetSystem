using System.Threading.Tasks;
using System.Windows;
using CarFleetManager.models;

namespace CarFleetManager;

public partial class MainWindowCtx
{
    private bool _carTabAccess;
    private bool _carTabActive;
    private int _currentCarIndex;
    private int? _originalCarId;
    private CarData? _currentCar;

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

    private Task? _fetchDetailsTask;
    private CarDetailsData? _currentCarDetails;

    public CarData? CurrentCar
    {
        get => _currentCar;
        set
        {
            if (Equals(value, _currentCar)) return;
            _currentCar = value;
            _originalCarId = _currentCar?.Id;
            if (_currentCar?.Id is not null)
            {
                if (_fetchDetailsTask is null || _fetchDetailsTask.IsCompleted)
                    _fetchDetailsTask = FetchCurrentCarDetails();
            }
            OnPropertyChanged();
        }
    }

    public CarDetailsData? CurrentCarDetails
    {
        get => _currentCarDetails;
        set
        {
            if (Equals(value, _currentCarDetails)) return;
            _currentCarDetails = value;
            if (_currentCarDetails != null) _currentCarDetails.PropertyChanged += (sender, args) => OnPropertyChanged();
            OnPropertyChanged();
        }
    }

    public void AddNewCar()
    {
        var newCar = new CarData();
        Cars.Add(newCar);
        CurrentCar = newCar;
    }
}