namespace WC4GameFlagChecker;

public static class UshortMethodExtension
{
    public static bool HasFlag(this ushort value, ushort constant) => (value & constant) == constant;
}

public static class StringHelper
{
    public static string GetDepthString(int depth, bool skipFirst = false)
    {
        var str = "";

        if (skipFirst)
            depth--;

        for (var i = 0; i < depth; i++)
        {
            str += "| ";
        }

        return str;
    }
}

public enum Games
{
    Diamond,
    Pearl,
    Platinum,
    HeartGold,
    SoulSilver,
}