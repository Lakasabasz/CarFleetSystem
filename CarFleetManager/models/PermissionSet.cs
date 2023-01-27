using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarFleetManager.models;

public class PermissionSet: INotifyPropertyChanged
{
    public bool Root { get; set; }
    
    private bool _implicitViewUserList;
    private bool _implicitAddUser;
    private bool _implicitDeleteUser;
    private bool _implicitEditUser;
    private bool _implicitSetPermissions;
    private bool _implicitViewCarList;
    private bool _implicitAddCar;
    private bool _implicitEditCar;
    private bool _implicitDeleteCar;
    private bool _implicitViewCarDetails;
    private bool _implicitUpdateCarDetails;
    private bool _implicitViewAvailableCarList;
    private bool _implicitClaimCar;
    private bool _implicitReleaseCar;
    private bool _implicitUpdateCar;

    public bool ViewUserList{
        get => Root || _implicitViewUserList;
        set
        {
            if (Root) return;
            _implicitViewUserList = value;
            OnPropertyChanged();
        }
    }

    public bool AddUser
    {
        get => Root || _implicitAddUser;
        set
        {
            if (Root) return;
            _implicitAddUser = value;
            OnPropertyChanged();
        }
    }
    
    public bool DeleteUser
    {
        get => Root || _implicitDeleteUser;
        set
        {
            if (Root) return;
            _implicitDeleteUser = value;
            OnPropertyChanged();
        }
    }
    
    public bool EditUser
    {
        get => Root || _implicitEditUser;
        set
        {
            if (Root) return;
            _implicitEditUser = value;
            OnPropertyChanged();
        }
    }

    public bool SetPermissions
    {
        get => Root || _implicitSetPermissions;
        set
        {
            if (Root) return;
            _implicitSetPermissions = value;
            OnPropertyChanged();
        }
    }

    public bool ViewCarList
    {
        get => Root || _implicitViewCarList;
        set
        {
            if (Root) return;
            _implicitViewCarList = value;
            OnPropertyChanged();
        }
    }

    public bool AddCar 
    {
        get => Root || _implicitAddCar;
        set
        {
            if (Root) return;
            _implicitAddCar = value;
            OnPropertyChanged();
        }
    }
    
    public bool EditCar 
    {
        get => Root || _implicitEditCar;
        set
        {
            if (Root) return;
            _implicitEditCar = value;
            OnPropertyChanged();
        }
    }
    
    public bool DeleteCar 
    {
        get => Root || _implicitDeleteCar;
        set
        {
            if (Root) return;
            _implicitDeleteCar = value;
            OnPropertyChanged();
        }
    }
    
    public bool ViewCarDetails
    {
        get => Root || _implicitViewCarDetails;
        set
        {
            if (Root) return;
            _implicitViewCarDetails = value;
            OnPropertyChanged();
        }
    }
    
    public bool UpdateCarDetails
    {
        get => Root || _implicitUpdateCarDetails;
        set
        {
            if (Root) return;
            _implicitUpdateCarDetails = value;
            OnPropertyChanged();
        }
    }

    public bool ViewAvailableCarList { 
        get => Root || _implicitViewAvailableCarList;
        set
        {
            if (Root) return;
            _implicitViewAvailableCarList = value;
            OnPropertyChanged();
        }
    }

    public bool ClaimCar
    {
        get => Root || _implicitClaimCar;
        set
        {
            if (Root) return;
            _implicitClaimCar = value;
            OnPropertyChanged();
        }
    }
    
    public bool ReleaseCar
    {
        get => Root || _implicitReleaseCar;
        set
        {
            if (Root) return;
            _implicitReleaseCar = value;
            OnPropertyChanged();
        }
    }
    
    public bool UpdateCar
    {
        get => Root || _implicitUpdateCar;
        set
        {
            if (Root) return;
            _implicitUpdateCar = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}