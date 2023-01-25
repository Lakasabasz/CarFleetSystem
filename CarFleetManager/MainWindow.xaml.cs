using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        public MainWindow()
        {
            InitializeComponent();
            MapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            MapControl.Map?.Layers.Add(CreatePointLayer());
            var ath = new MPoint(19.05885, 49.78364);
            MapControl.Navigator?.NavigateTo(SphericalMercator.FromLonLat(ath.X, ath.Y).ToMPoint(), MapControl.Map.Resolutions[16]);
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
    }
}