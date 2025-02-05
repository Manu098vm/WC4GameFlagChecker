namespace WC4GameFlagChecker;

public class GamesName
{
    protected string FileName { get; init; }
    protected string GameNames { get; init; }

    public bool Diamond => GameNames.Contains('D', StringComparison.InvariantCulture);
    public bool Pearl => GameNames.Contains('P', StringComparison.InvariantCulture) && (GameNames.Contains("PPt", StringComparison.InvariantCulture) || !Platinum);
    public bool Platinum => GameNames.Contains("Pt", StringComparison.InvariantCulture);
    public bool HeartGold => GameNames.Contains("HG", StringComparison.InvariantCulture);
    public bool SoulSilver => GameNames.Contains("SS", StringComparison.InvariantCulture);

    public GamesName(string filepath)
    {
        FileName = Path.GetFileNameWithoutExtension(filepath);
        foreach (var str in FileName.Split('-'))
        {
            if (str.Length > 0)
            {
                GameNames = str;
                return;
            }
        }
        GameNames = "-";
    }

    public bool NameCardMatch(PCD card, int depth, out string result)
    {
        result = $"{StringHelper.GetDepthString(depth, true)}{FileName}:{Environment.NewLine}";

        if (Diamond == card.Diamond && Pearl == card.Pearl && Platinum == card.Platinum && HeartGold == card.HeartGold && SoulSilver == card.SoulSilver)
        {
            result += $"{StringHelper.GetDepthString(depth)}File name matches the Wondercard Game Flags.{Environment.NewLine}";
            return true;
        }

        if (Diamond != card.Diamond)
            result += GetMismatchString(Games.Diamond, Diamond, depth);
        if (Pearl != card.Pearl)
            result += GetMismatchString(Games.Pearl, Pearl, depth);
        if (Platinum != card.Platinum)
            result += GetMismatchString(Games.Platinum, Platinum, depth);
        if (HeartGold != card.HeartGold)
            result += GetMismatchString(Games.HeartGold, HeartGold, depth);
        if (SoulSilver != card.SoulSilver)
            result += GetMismatchString(Games.SoulSilver, SoulSilver, depth);

        return false;
    }

    private static string GetMismatchString(Games game, bool isInName, int depth) => $"{StringHelper.GetDepthString(depth)}{(isInName ? $"{game} present in the file name, " +
        $"but missing in the wondercard flags." : $"{game} missing in the file name, but present in the wondercard flags.")}{Environment.NewLine}";

    public string GetFixedFilename(PCD card)
    {
        var games = "";
        if (card.Diamond)
            games += "D";
        if (card.Pearl)
            games += "P";
        if (card.Platinum)
            games += "Pt";
        if (card.HeartGold)
            games += "HG";
        if (card.SoulSilver)
            games += "SS";

        return FileName.Replace(GameNames, $"{games} ");
    }
}