using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PlaceableMiners;

internal static class BuildingPlacementNodeGhostUtility
{
    private static readonly MethodInfo? ShowGhostMethod = typeof(BuildingPlacementNode)
        .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        .FirstOrDefault(method => method.Name == "ShowGhost");

    internal static void ShowGhost(BuildingPlacementNode node, bool show, PlacementNodeRequirement requirement)
    {
        if (node == null)
        {
            return;
        }

        if (!IsUsableGameObject(node.GhostPrefab))
        {
            node.GhostPrefab = null;
        }

        if (ShowGhostMethod != null)
        {
            try
            {
                ParameterInfo[] parameters = ShowGhostMethod.GetParameters();
                if (parameters.Length >= 2)
                {
                    ShowGhostMethod.Invoke(node, new object[] { show, requirement });
                    return;
                }

                if (parameters.Length == 1)
                {
                    ShowGhostMethod.Invoke(node, new object[] { show });
                    return;
                }

                if (parameters.Length == 0)
                {
                    ShowGhostMethod.Invoke(node, System.Array.Empty<object>());
                    if (!show && IsUsableGameObject(node.GhostPrefab))
                    {
                        node.GhostPrefab!.SetActive(false);
                    }

                    return;
                }
            }
            catch (TargetInvocationException)
            {
                if (IsUsableGameObject(node.GhostPrefab))
                {
                    try
                    {
                        node.GhostPrefab!.SetActive(show && node.AttachedBuildingObject == null);
                    }
                    catch
                    {
                        node.GhostPrefab = null;
                    }
                }

                return;
            }
            catch
            {
                return;
            }
        }

        if (IsUsableGameObject(node.GhostPrefab))
        {
            try
            {
                node.GhostPrefab!.SetActive(show && node.AttachedBuildingObject == null);
            }
            catch
            {
                node.GhostPrefab = null;
            }
        }
    }

    private static bool IsUsableGameObject(GameObject? gameObject)
    {
        if (gameObject == null)
        {
            return false;
        }

        try
        {
            return gameObject.transform != null;
        }
        catch
        {
            return false;
        }
    }
}
