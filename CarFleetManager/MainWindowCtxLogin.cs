using System.Threading.Tasks;
using System.Windows;
using CarFleetManager.models;

namespace CarFleetManager;

public partial class MainWindowCtx
{
    private string _loginText = null!;
    private string _passwordText = null!;
    private bool _loginMode = true;
    private UserData _currentLoggedInUser = null!;
    public bool LoginMode
    {
        get => _loginMode;
        set
        {
            _loginMode = value;
            OnPropertyChanged(nameof(LoginVisible));
            OnPropertyChanged(nameof(ManagerVisible));
        }
    }
    
    public UserData CurrentLoggedInUser
    {
        get => _currentLoggedInUser;
        set
        {
            if (Equals(value, _currentLoggedInUser)) return;
            _currentLoggedInUser = value;
            OnPropertyChanged();
        }
    }
    
    public string LoginText
    {
        get => _loginText;
        set
        {
            _loginText = value;
            OnPropertyChanged(nameof(IsLoginButtonEnabled));
        }
    }
    
    public string PasswordText
    {
        get => _passwordText;
        set
        {
            _passwordText = value;
            OnPropertyChanged(nameof(IsLoginButtonEnabled));
        }
    }
}