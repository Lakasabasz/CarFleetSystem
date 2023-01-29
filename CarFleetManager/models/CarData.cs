using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarFleetManager.models;

public class CarData: INotifyPropertyChanged
{
    private string _vin;
    private string _plates;
    private string _mark;
    private string _model;
    public int? Id { get; set; }

    public string Vin
    {
        get => _vin;
        set => SetField(ref _vin, value);
    }

    public string Plates
    {
        get => _plates;
        set => SetField(ref _plates, value);
    }

    public string Mark
    {
        get => _mark;
        set => SetField(ref _mark, value);
    }

    public string Model
    {
        get => _model;
        set => SetField(ref _model, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}