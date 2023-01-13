namespace CarFleetSystemServer.Models;

public class PermissionSet
{
    public bool Root { get; set; }
    
    private bool _implicitViewUserList;
    private bool _implicitAddUser;
    private bool _implicitDeleteUser;
    private bool _implicitEditUser;
    private bool _implicitSetPermissions;

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
}