using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CarFleetManager.models;

namespace CarFleetManager;

public partial class MainWindowCtx
{
    public async Task Login()
    {
        bool result = await ApiClient.Login(LoginText, PasswordText);
        if (result) LoginMode = false;
        else
        {
            MessageBox.Show("Nie udało się zalogować", "Błąd logowania",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (_updateUserListTask is null || _updateUserListTask.IsCompleted) _updateUserListTask = UpdateUserList();
    }

    public async Task UpdateUserList()
    {
        var (result, listUsers) = await ApiClient.ListUsers();
        if (result)
        {
            UserTabAccess = true;
            Dictionary<UserData, UserData?> compareMap = new();
            foreach (var newUser in listUsers)
            {
                var user = Users.FirstOrDefault(x => x.Username == newUser.Username);
                compareMap.Add(newUser, user);
            }

            foreach (var user in Users)
            {
                if (!compareMap.Any(x => x.Value == user)) Users.Remove(user);
                var match = compareMap.FirstOrDefault(x => x.Value == user);
                int index = Users.IndexOf(match.Value!);
                Users[index].Password = match.Key.Password;
                Users[index].Permission = match.Key.Permission;
            }

            foreach (var (key, value) in compareMap)
                if (value is null)
                    Users.Add(key);
            CurrentLoggedInUser = Users.First(x => x.Username == ApiClient.LoggedInUsername);
        }
    }

    public async Task EditOrAddCurrentUser()
    {
        if (string.IsNullOrEmpty(_originalUsername))
            await _addCurrentUser();
        else
            await _editCurrentUser();
        await UpdateUserList();
    }

    private async Task _editCurrentUser()
    {
        bool result = CurrentUser != null && await ApiClient.EditUser(_originalUsername, CurrentUser);
        if (!result)
            MessageBox.Show(
                "Nie udało się zapisać edytowanego użytkownika. Kliknij przycisk odśwież aby cofnąć zmiany",
                "Błąd zapisu użytkownika", MessageBoxButton.OK, MessageBoxImage.Error);
        else MessageBox.Show("Zapisano zmiany", "Sukces zapisu", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async Task _addCurrentUser()
    {
        bool result = CurrentUser != null && await ApiClient.AddUser(CurrentUser);
        if (!result)
            MessageBox.Show(
                "Nie udało się dodać użytkownika. Kliknij przycisk odśwież aby cofnąć zmiany",
                "Błąd zapisu użytkownika", MessageBoxButton.OK, MessageBoxImage.Error);
        else MessageBox.Show("Zapisano zmiany", "Sukces zapisu", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public async Task DeleteCurrentUser()
    {
        bool result = CurrentUser != null && await ApiClient.DeleteUser(CurrentUser);
        if (!result)
            MessageBox.Show("Nie udało się usunąć użytkownika", "Błąd usuwanie użytkownika",
                MessageBoxButton.OK, MessageBoxImage.Error);
        else
            MessageBox.Show("Pomyślnie usunięto użytkownika", "Sukces usuwania", MessageBoxButton.OK,
                MessageBoxImage.Information);
        await UpdateUserList();
    }
}