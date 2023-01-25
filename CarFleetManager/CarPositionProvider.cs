using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;

namespace CarFleetManager;

public class CarPositionProvider : MemoryProvider, IDynamic
{
    private readonly Timer _timer;
    public CarPositionProvider()
    {
        _timer = new Timer(_ => DataHasChanged(), this, 0, 1 * 1000);
    }
    public override Task<IEnumerable<IFeature>> GetFeaturesAsync(FetchInfo fetchInfo)
    {
        var features = new List<PointFeature>();
        var position = SphericalMercator.FromLonLat(19.05885, 49.78364).ToMPoint();
        features.Add(new PointFeature(position)
        {
            ["ID"] = "0"
        });
        return Task.FromResult((IEnumerable<IFeature>) features);
    }

    public void DataHasChanged()
    {
        DataChanged?.Invoke(this, new DataChangedEventArgs(null, false, null));
    }

    public event DataChangedEventHandler? DataChanged;
}