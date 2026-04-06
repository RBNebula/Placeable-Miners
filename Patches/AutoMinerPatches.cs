using HarmonyLib;
using System.Collections;

namespace PlaceableMiners;

[HarmonyPatch(typeof(AutoMiner), "OnEnable")]
internal static class AutoMinerOnEnablePatch
{
    private static void Prefix(AutoMiner __instance)
    {
        PlaceableMinerPlacementService.RememberDesiredState(__instance);
        if (PlaceableMinerPlacementService.TryAssignNearestResourceDefinition(__instance))
        {
            PlaceableMinerPlacementService.RestoreDesiredStateIfNeeded(__instance);
        }
    }
}

[HarmonyPatch(typeof(AutoMiner), "Update")]
internal static class AutoMinerUpdatePatch
{
    private static bool Prefix(AutoMiner __instance)
    {
        if (!PlaceableMinerPlacementService.IsSupportedMiner(__instance))
        {
            return true;
        }

        if (PlaceableMinerPlacementService.TryAssignNearestResourceDefinition(__instance))
        {
            PlaceableMinerPlacementService.RestoreDesiredStateIfNeeded(__instance);
            return true;
        }

        return __instance.ResourceDefinition != null;
    }
}

[HarmonyPatch(typeof(AutoMiner), "WaitThenCheckIfValid")]
internal static class AutoMinerWaitThenCheckIfValidPatch
{
    private static bool Prefix(AutoMiner __instance, ref IEnumerator __result)
    {
        if (!PlaceableMinerPlacementService.IsSupportedMiner(__instance))
        {
            return true;
        }

        __result = PlaceableMinerPlacementService.GetEmptyEnumerator();
        return false;
    }
}
