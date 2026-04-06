using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlaceableMiners;

internal static class PlaceableMinerPlacementService
{
    private const string UpdatePopupHeader = "New Update!";
    private const string MovedSpotPopupSnippet = "spots have been moved";

    private static readonly HashSet<SavableObjectID> SupportedMinerIds = new()
    {
        SavableObjectID.AutoMinerMk1,
        SavableObjectID.RapidAutoMiner,
        SavableObjectID.HeavyAutoMiner
    };

    private static readonly Dictionary<int, bool> PendingEnabledStates = new();
    private static readonly IEnumerator EmptyEnumerator = EmptyRoutine();

    internal static bool IsSupportedMiner(BuildingObject? buildingObject)
    {
        return buildingObject != null && SupportedMinerIds.Contains(buildingObject.SavableObjectID);
    }

    internal static bool IsSupportedMiner(AutoMiner? miner)
    {
        return TryGetBuildingObject(miner, out BuildingObject? buildingObject) &&
               IsSupportedMiner(buildingObject);
    }

    internal static bool HasNearbyCompatiblePlacementNode(Vector3 position, PlacementNodeRequirement requirement)
    {
        if (requirement == PlacementNodeRequirement.None)
        {
            return false;
        }

        foreach (BuildingPlacementNode node in BuildingPlacementNode.All)
        {
            if (node == null || node.RequirementType != requirement || node.AttachedBuildingObject != null)
            {
                continue;
            }

            if (Vector3.Distance(position, node.transform.position) < 4f)
            {
                return true;
            }
        }

        return false;
    }

    internal static bool TryAssignNearestResourceDefinition(AutoMiner miner)
    {
        if (miner.ResourceDefinition != null)
        {
            return true;
        }

        if (!TryGetBuildingObject(miner, out BuildingObject? buildingObject) || !IsSupportedMiner(buildingObject))
        {
            return false;
        }

        PlacementNodeRequirement requirement = buildingObject!.PlacementNodeRequirement;
        if (requirement == PlacementNodeRequirement.None)
        {
            return false;
        }

        BuildingPlacementNode? closestNode = null;
        float closestDistance = float.MaxValue;

        foreach (BuildingPlacementNode node in BuildingPlacementNode.All)
        {
            if (node == null || node.RequirementType != requirement || node.AutoMinerResourceDefinition == null)
            {
                continue;
            }

            float distance = Vector3.Distance(node.transform.position, buildingObject.transform.position);
            if (distance >= closestDistance)
            {
                continue;
            }

            closestDistance = distance;
            closestNode = node;
        }

        if (closestNode == null)
        {
            return false;
        }

        miner.ResourceDefinition = closestNode.AutoMinerResourceDefinition;
        miner.ConfigureFromDefinition();
        return true;
    }

    internal static bool TryGetBuildingObject(AutoMiner? miner, out BuildingObject? buildingObject)
    {
        buildingObject = miner?.GetComponentInParent<BuildingObject>();
        return buildingObject != null;
    }

    internal static bool TryGetManagedMiner(BuildingObject? buildingObject, out AutoMiner? miner)
    {
        miner = buildingObject?.GetComponentInChildren<AutoMiner>();
        return miner != null && IsSupportedMiner(buildingObject);
    }

    internal static void RememberDesiredState(AutoMiner miner)
    {
        PendingEnabledStates[miner.GetInstanceID()] = miner.Enabled;
    }

    internal static void RestoreDesiredStateIfNeeded(AutoMiner miner)
    {
        int instanceId = miner.GetInstanceID();
        if (!PendingEnabledStates.TryGetValue(instanceId, out bool desiredState))
        {
            return;
        }

        PendingEnabledStates.Remove(instanceId);
        miner.Toggle(desiredState);
    }

    internal static bool IsMovedSpotPopup(string? header, string? message)
    {
        return header == UpdatePopupHeader &&
               message is string text &&
               text.Contains(MovedSpotPopupSnippet);
    }

    internal static bool ShouldBlockPack(BuildingObject buildingObject)
    {
        return TryGetManagedMiner(buildingObject, out AutoMiner? miner) &&
               miner!.ResourceDefinition == null;
    }

    internal static void RefreshPlacementNodeGhosts(BuildingObject buildingObject)
    {
        if (!IsSupportedMiner(buildingObject))
        {
            return;
        }

        PlacementNodeRequirement requirement = buildingObject.PlacementNodeRequirement;
        foreach (BuildingPlacementNode node in BuildingPlacementNode.All)
        {
            if (node == null)
            {
                continue;
            }

            bool shouldShow = node.AttachedBuildingObject == null && node.RequirementType == requirement;
            BuildingPlacementNodeGhostUtility.ShowGhost(node, shouldShow, requirement);
        }
    }

    internal static IEnumerator GetEmptyEnumerator()
    {
        return EmptyEnumerator;
    }

    private static IEnumerator EmptyRoutine()
    {
        yield break;
    }
}
