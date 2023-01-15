namespace CarFleetSystemServer.Models;

public class PermissionSet
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

    public bool ViewUserList{
        get => Root || _implicitViewUserList;
        set
        {
            if (!Root) _implicitViewUserList = value;
        }
    }

    public bool AddUser
    {
        get => Root || _implicitAddUser;
        set
        {
            if(!Root) _implicitAddUser = value;
        }
    }
    
    public bool DeleteUser
    {
        get => Root || _implicitDeleteUser;
        set
        {
            if(!Root) _implicitDeleteUser = value;
        }
    }
    
    public bool EditUser
    {
        get => Root || _implicitEditUser;
        set
        {
            if(!Root) _implicitEditUser = value;
        }
    }

    public bool SetPermissions
    {
        get => Root || _implicitSetPermissions;
        set
        {
            if(!Root) _implicitSetPermissions = value;
        }
    }

    public bool ViewCarList
    {
        get => Root || _implicitViewCarList;
        set
        {
            if (!Root) _implicitViewCarList = value;
        }
    }

    public bool AddCar 
    {
        get => Root || _implicitAddCar;
        set
        {
            if (!Root) _implicitAddCar = value;
        }
    }
    
    public bool EditCar 
    {
        get => Root || _implicitEditCar;
        set
        {
            if (!Root) _implicitEditCar = value;
        }
    }
    
    public bool DeleteCar 
    {
        get => Root || _implicitDeleteCar;
        set
        {
            if (!Root) _implicitDeleteCar = value;
        }
    }
    
    public bool ViewCarDetails
    {
        get => Root || _implicitViewCarDetails;
        set
        {
            if (!Root) _implicitViewCarDetails = value;
        }
    }
    
    public bool UpdateCarDetails
    {
        get => Root || _implicitUpdateCarDetails;
        set
        {
            if (!Root) _implicitUpdateCarDetails = value;
        }
    }
}