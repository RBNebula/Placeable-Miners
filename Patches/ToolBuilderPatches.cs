using HarmonyLib;

namespace PlaceableMiners;

[HarmonyPatch(typeof(ToolBuilder), "Update")]
internal static class ToolBuilderUpdatePatch
{
    private static void Postfix(ToolBuilder __instance)
    {
        if (__instance.Definition == null ||
            __instance.CurrentPrefabIndex < 0 ||
            __instance.CurrentPrefabIndex >= __instance.Definition.BuildingPrefabs.Count)
        {
            return;
        }

        BuildingObject selectedPrefab = __instance.Definition.BuildingPrefabs[__instance.CurrentPrefabIndex];
        PlaceableMinerPlacementService.RefreshPlacementNodeGhosts(selectedPrefab);
    }
}
