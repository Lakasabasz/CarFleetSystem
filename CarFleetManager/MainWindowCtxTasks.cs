using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CarFleetManager.models;

namespace CarFleetManager;

public partial class MainWindowCtx
{
    private Task? _updateUserListTask;
    private Task? _updateCarListTask;

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
        if (_updateCarListTask is null || _updateCarListTask.IsCompleted) _updateCarListTask = UpdateCarListTask();
    }

    public ObservableCollection<CarData> Cars { get; set; } = new();

    public async Task UpdateCarListTask()
    {
        var (result, listCars) = await ApiClient.ListCars();
        if (result)
        {
            CarTabAccess = true;
            Dictionary<CarData, CarData?> compareMap = new();
            foreach (var newCar in listCars)
            {
                var user = Cars.FirstOrDefault(x => x.Id == newCar.Id);
                compareMap.Add(newCar, user);
            }

            foreach (var car in Cars)
            {
                if (!compareMap.Any(x => x.Value == car)) Cars.Remove(car);
                var match = compareMap.FirstOrDefault(x => x.Value == car);
                int index = Cars.IndexOf(match.Value!);
                Cars[index].Mark = match.Key.Mark;
                Cars[index].Model = match.Key.Model;
                Cars[index].Plates = match.Key.Plates;
                Cars[index].Vin = match.Key.Vin;
            }

            foreach (var (key, value) in compareMap)
                if (value is null)
                    Cars.Add(key);
        }
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

    public async Task EditOrAddCurrentCar()
    {
        if (_originalCarId.HasValue) await _editCurrentCar();
        else await _addCurrentCar();
        await UpdateCarListTask();
    }

    private async Task _addCurrentCar()
    {
        bool result = CurrentCar != null && await ApiClient.AddCar(CurrentCar);
        if (!result)
            MessageBox.Show(
                "Nie udało się dodać samochodu. Kliknij przycisk odśwież aby cofnąć zmiany",
                "Błąd zapisu samochodu", MessageBoxButton.OK, MessageBoxImage.Error);
        else MessageBox.Show("Zapisano zmiany", "Sukces zapisu", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async Task _editCurrentCar()
    {
        bool resultA = CurrentCar != null && _originalCarId != null && await ApiClient.EditCar(_originalCarId.Value, CurrentCar);
        bool resultB = _originalCarId != null && CurrentCar?.Id != null && CurrentCarDetails != null 
                       && await ApiClient.EditCarDetails(resultA ? CurrentCar.Id.Value : _originalCarId.Value, CurrentCarDetails);
        MessageBoxImage level = resultA && resultB ?
            MessageBoxImage.Information :
            (!resultA && resultB) || (resultA && !resultB) ?
                MessageBoxImage.Warning : MessageBoxImage.Error;
        string text;
        if (resultA && resultB) text = "Zapisano zmiany";
        else if (resultA && !resultB) text = "Nie udało się zapisać detali";
        else if (resultB && !resultA) text = "Nie udało się zapisać danych identyfikacyjnych";
        else text = "Nie udało się zapisać żadnych zmian. Kliknij przycisk odśwież aby cofnąć zmiany";
        MessageBox.Show(text, "Zapis zmian", MessageBoxButton.OK, level);
    }

    public async Task FetchCurrentCarDetails()
    {
        if (CurrentCar?.Id == null) return;
        var (result, report) = await ApiClient.GetCarDetails(CurrentCar.Id.Value);
        if (!result)
            MessageBox.Show("Nie udało się pobrać detali", "Błąd pobierania detali",
                MessageBoxButton.OK, MessageBoxImage.Error);
        CurrentCarDetails = report;
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

    public async Task DeleteCurrentCar()
    {
        bool result = CurrentCar != null && await ApiClient.DeleteCar(CurrentCar);
        if (!result)
            MessageBox.Show("Nie udało się usunąć samochodu", "Błąd usuwania samochodu",
                MessageBoxButton.OK, MessageBoxImage.Error);
        await UpdateCarListTask();
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