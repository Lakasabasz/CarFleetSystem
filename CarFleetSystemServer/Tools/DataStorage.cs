namespace CarFleetSystemServer.Tools;

public class DataStorage
{
    private static IDataStorage? _instance = null;
    public static IDataStorage Instance => _instance ??= new RamDataStorage();
    public static void Init(IDataStorage dataStorage)
    {
        _instance = dataStorage;
    }
}