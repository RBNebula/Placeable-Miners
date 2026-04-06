using BepInEx;

namespace PlaceableMiners;

[BepInPlugin(ModInfo.PLUGIN_GUID, ModInfo.PLUGIN_NAME, ModInfo.PLUGIN_VERSION)]
public sealed class PlaceableMiners : BaseUnityPlugin
{
    private PlaceableMinersRuntime? _runtime;

    private void Awake()
    {
        _runtime = new PlaceableMinersRuntime(Logger);
        _runtime.Initialize();
        Logger.LogInfo($"{ModInfo.LOG_PREFIX} Initialized");
    }

    private void OnDestroy()
    {
        _runtime?.Dispose();
        _runtime = null;
    }
}
