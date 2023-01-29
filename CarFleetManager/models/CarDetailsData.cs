using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarFleetManager.models;

public class CarDetailsData: INotifyPropertyChanged
{
    private Color _carColor;
    private string _description;
    private DateOnly _nextCarReview;
    private DateOnly _insuranceEnding;

    public Color CarColor
    {
        get => _carColor;
        set => SetField(ref _carColor, value);
    }

    public string Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public DateOnly NextCarReview
    {
        get => _nextCarReview;
        set => SetField(ref _nextCarReview, value);
    }

    public DateOnly InsuranceEnding
    {
        get => _insuranceEnding;
        set => SetField(ref _insuranceEnding, value);
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