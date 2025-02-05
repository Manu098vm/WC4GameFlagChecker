namespace WC4GameFlagChecker;

public class Program
{
    public static void Main()
    {
        var countMismatch = 0;

        if (Environment.GetCommandLineArgs().Length < 2)
        {
            Console.WriteLine($"Please drag & drop a valid directory containing WC4 files. Press any key to exit.");
            Console.ReadKey();
            return;
        }

        var path = Environment.GetCommandLineArgs()[1];

        if (!Directory.Exists(path))
        {
            Console.WriteLine($"{path} is not a valid directory. Press any key to exit.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"Processing, please wait...{Environment.NewLine}");
        var result = $"List of cards with game flags mismatching the card filenames:{Environment.NewLine}";
        ScanDirctories(path, ref result, ref countMismatch, fixFileNames: false);

        var exportPath = Path.Combine(Environment.CurrentDirectory, "log.txt");
        Console.WriteLine($"{result}{Environment.NewLine}Mismatch count: {countMismatch}{Environment.NewLine}Log exported to: {exportPath}");
        File.WriteAllText(exportPath, $"Mismatch Count: {countMismatch}{Environment.NewLine}{result}");
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    public static void ScanDirctories(string path, ref string result, ref int countMismatch, bool fixFileNames, int depth = 1)
    {
        try
        {
            result += $"{StringHelper.GetDepthString(depth, true)}{path}{Environment.NewLine}";
            CheckAllCardsInDirectory(path, ++depth, ref result, ref countMismatch, fixFileNames);

            foreach (var directory in Directory.EnumerateDirectories(path))
                ScanDirctories(directory, ref result, ref countMismatch, fixFileNames, depth);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public static void CheckAllCardsInDirectory(string directory, int depth, ref string result, ref int countMismatch, bool fixFileNames)
    {
        foreach (var file in Directory.EnumerateFiles(directory))
        {
            var ext = Path.GetExtension(file);
            if (ext.Equals(".pcd", StringComparison.OrdinalIgnoreCase) || ext.Equals(".wc4", StringComparison.OrdinalIgnoreCase))
            {
                var gamesInName = new GamesName(file);
                var wondercard = new PCD(File.ReadAllBytes(file));
                if (!gamesInName.NameCardMatch(wondercard, depth, out var match))
                {
                    result += match;
                    countMismatch++;

                    if (fixFileNames)
                    {
                        var path = Path.Combine(directory, $"{gamesInName.GetFixedFilename(wondercard)}{ext}");
                        File.WriteAllBytes(path, wondercard.Data);
                        File.Delete(file);
                    }
                }
            }
        }
    }
}

    