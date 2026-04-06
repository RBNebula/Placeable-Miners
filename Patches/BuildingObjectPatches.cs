using HarmonyLib;

namespace PlaceableMiners;

[HarmonyPatch(typeof(BuildingObject), nameof(BuildingObject.Pack))]
internal static class BuildingObjectPackPatch
{
    private static bool Prefix(BuildingObject __instance)
    {
        return !PlaceableMinerPlacementService.ShouldBlockPack(__instance);
    }
}
