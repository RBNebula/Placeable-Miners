using HarmonyLib;

namespace PlaceableMiners;

[HarmonyPatch(typeof(UIManager), nameof(UIManager.ShowInfoMessagePopup))]
internal static class UIManagerShowInfoMessagePopupPatch
{
    private static bool Prefix(string header, string message)
    {
        return !PlaceableMinerPlacementService.IsMovedSpotPopup(header, message);
    }
}
