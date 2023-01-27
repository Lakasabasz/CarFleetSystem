using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarFleetManager.models;

public class UserData: INotifyPropertyChanged
{
    private PermissionSet _permission;
    private string _username;
    private string _password;

    public UserData()
    {
        _permission = new();
        _username = string.Empty;
        _password = string.Empty;
    }

    public string Username
    {
        get => _username;
        set
        {
            if (value == _username) return;
            _username = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (value == _password) return;
            _password = value;
            OnPropertyChanged();
        }
    }

    public PermissionSet Permission
    {
        get => _permission;
        set
        {
            if (Equals(value, _permission)) return;
            _permission = value;
            _permission.PropertyChanged += (sender, args) => OnPropertyChanged();
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}