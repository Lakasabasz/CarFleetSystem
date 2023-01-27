using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Layers.AnimatedLayers;
using Mapsui.Projections;
using Mapsui.Styles;
using Brush = Mapsui.Styles.Brush;
using Color = Mapsui.Styles.Color;

namespace CarFleetManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _lockPasswordFallback = false;
        public MainWindow()
        {
            InitializeComponent();
            MapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            MapControl.Map?.Layers.Add(CreatePointLayer());
            var ath = new MPoint(19.05885, 49.78364);
            MapControl.Navigator?.NavigateTo(SphericalMercator.FromLonLat(ath.X, ath.Y).ToMPoint(), MapControl.Map.Resolutions[16]);
            DataContext = new MainWindowCtx();
            ((MainWindowCtx)DataContext).PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CurrentUser") return;
            if (_lockPasswordFallback)
            {
                _lockPasswordFallback = false;
                return;
            }
            UserPassword.Password = ((MainWindowCtx)DataContext).CurrentUser?.Password;
        }

        private ILayer CreatePointLayer()
        {
            return new AnimatedPointLayer(new CarPositionProvider())
            {
                Name = "Car position",
                Style = new SymbolStyle()
                    { Fill = new Brush(Color.Red), SymbolScale = 0.5, SymbolType = SymbolType.Ellipse },
            };
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((MainWindowCtx)DataContext).PasswordText = ((PasswordBox)sender).Password;
        }

        private Task? _loginTask;
        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(_loginTask is null || _loginTask.IsCompleted)
                _loginTask = ((MainWindowCtx)DataContext).Login();
        }

        private void UserPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var currentUser = ((MainWindowCtx)DataContext).CurrentUser;
            if (currentUser == null || currentUser.Password == ((PasswordBox)sender).Password) return;
            _lockPasswordFallback = true;
            currentUser.Password = ((PasswordBox)sender).Password;
        }

        private void AddUser_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowCtx)DataContext).AddNewUser();
        }

        private Task? _deleteUserTask;
        private void DeleteUser_OnClick(object sender, RoutedEventArgs e)
        {
            if (_deleteUserTask is null || _deleteUserTask.IsCompleted)
                _deleteUserTask = ((MainWindowCtx)DataContext).DeleteCurrentUser();
        }

        private Task? _refreshUsersTask;
        private void RefreshUserList_OnClick(object sender, RoutedEventArgs e)
        {
            if (_refreshUsersTask is null || _refreshUsersTask.IsCompleted)
                _refreshUsersTask = ((MainWindowCtx)DataContext).UpdateUserList();
        }

        private Task? _editUserTask;
        private void EditUser_OnClick(object sender, RoutedEventArgs e)
        {
            if (_editUserTask is null || _editUserTask.IsCompleted)
                _editUserTask = ((MainWindowCtx)DataContext).EditOrAddCurrentUser();
        }
    }
}