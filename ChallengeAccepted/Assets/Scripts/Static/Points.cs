using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Points
{
    public static int NumberOfPoints { get; private set; }

    public static void ChangeNumberOfPoints(int offset)
    {
        NumberOfPoints += offset;
    }

    public static int GetCost(StoreItemType itemType)
    {
        switch (itemType)
        {
            default:
            case StoreItemType.Star: return 5;
            case StoreItemType.Delete: return 5;
            case StoreItemType.NewChallenge: return 10;
        }
    }

    public static bool TryBuy(StoreItemType itemType)
    {
        if (GetCost(itemType) > NumberOfPoints)
        {
            return false;
        }

        return true;
    }

}

public enum StoreItemType
{
    Star,
    Delete,
    NewChallenge
}
