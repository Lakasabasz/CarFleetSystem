using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using CarFleetManager.models;

namespace CarFleetManager;

public partial class MainWindowCtx: INotifyPropertyChanged
{
    private bool _userTabAccess;
    private UserData? _currentUser;
    private string? _originalUsername;
    public Visibility LoginVisible => _loginMode ? Visibility.Visible : Visibility.Collapsed;
    public Visibility ManagerVisible => _loginMode ? Visibility.Collapsed : Visibility.Visible;


    public bool IsLoginButtonEnabled => !string.IsNullOrEmpty(LoginText) && !string.IsNullOrEmpty(PasswordText);

    private bool UserTabAccess
    {
        get => _userTabAccess;
        set
        {
            _userTabAccess = value;
            OnPropertyChanged(nameof(UserTabVisibility));
        }
    }
    public Visibility UserTabVisibility => UserTabAccess ? Visibility.Visible : Visibility.Collapsed;

    public ObservableCollection<UserData> Users { get; set; } = new ();

    private int _currentUserIndex;
    private bool _userTabActive;

    public UserData? CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            _originalUsername = _currentUser?.Username;
            if (_currentUser is not null) _currentUser.PropertyChanged += (sender, args) => OnPropertyChanged();
            OnPropertyChanged();
        }
    }

    public int CurrentUserIndex
    {
        get => _currentUserIndex;
        set
        {
            if (_currentUser is not null && _currentUser.Username != _originalUsername)
            {
                OnPropertyChanged();
                return;
            }
            if (_currentUser is not null && _originalUsername is null)
            {
                OnPropertyChanged();
                return;
            }
            _currentUserIndex = value;
            OnPropertyChanged();
        }
    }
    
    public Visibility UserTabActiveVisibility => UserTabActive ? Visibility.Visible : Visibility.Collapsed;

    public bool UserTabActive
    {
        get => _userTabActive;
        set
        {
            if (value == _userTabActive) return;
            _userTabActive = value;
            OnPropertyChanged(nameof(UserTabActiveVisibility));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void AddNewUser()
    {
        var newUser = new UserData();
        Users.Add(newUser);
        CurrentUser = newUser;
    }
}