using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace PlaceableMiners;

[HarmonyPatch]
internal static class BuildingManagerCanPlaceObjectPatch
{
    private static MethodBase? TargetMethod()
    {
        return AccessTools.Method(typeof(BuildingManager), nameof(BuildingManager.CanPlaceObject), new[]
        {
            typeof(Vector3Int),
            typeof(BuildingObject),
            typeof(Quaternion),
            typeof(bool),
            typeof(bool),
            typeof(PlacementNodeRequirement),
            typeof(BuildingPlacementNode).MakeByRefType(),
            typeof(ToolBuilder)
        });
    }

    private static void Prefix(Vector3Int position, BuildingObject objPrefab, ref PlacementNodeRequirement placementNodeRequirement)
    {
        if (!PlaceableMinerPlacementService.IsSupportedMiner(objPrefab))
        {
            return;
        }

        if (PlaceableMinerPlacementService.HasNearbyCompatiblePlacementNode(position, placementNodeRequirement))
        {
            return;
        }

        placementNodeRequirement = PlacementNodeRequirement.None;
    }
}
