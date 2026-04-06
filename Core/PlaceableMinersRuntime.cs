using BepInEx.Logging;
using HarmonyLib;
using System;

namespace PlaceableMiners;

internal sealed class PlaceableMinersRuntime : IDisposable
{
    private readonly ManualLogSource _logger;
    private Harmony? _harmony;

    internal PlaceableMinersRuntime(ManualLogSource logger)
    {
        _logger = logger;
    }

    internal void Initialize()
    {
        _harmony ??= new Harmony(ModInfo.HARMONY_ID);
        _harmony.PatchAll();
    }

    public void Dispose()
    {
        if (_harmony == null)
        {
            return;
        }

        _harmony.UnpatchSelf();
        _harmony = null;
        _logger.LogInfo($"{ModInfo.LOG_PREFIX} Shutdown");
    }
}
