var file = args[0];

var fileContent = await File.ReadAllTextAsync(file).ConfigureAwait(false);
var lines = fileContent.Split("\n");

var averages = new Dictionary<string, decimal[]>();
foreach (var line in lines)
{
    var content = line.Split(";");
    if (content.Length != 2) continue;
    var station = content[0];
    var measurement = decimal.Parse(content[1]);
    if (!averages.ContainsKey(station))
        averages[station] = [decimal.MaxValue, 0M, 0M, decimal.MinValue];
    averages[station][0] = Math.Min(averages[station][0], measurement);
    averages[station][1] = Math.Round(measurement + averages[station][1], 1);
    averages[station][2]++;
    averages[station][3] = Math.Max(averages[station][3], measurement);
}

Console.Write("{");
var sortedResults = averages.OrderBy(kv => kv.Key, new NaturalSorting()).ToArray();
for (var i = 0; i < sortedResults.Length; ++i)
{
    var kv = sortedResults[i];
    
    Console.Write($"{kv.Key}={kv.Value[0]}/{Math.Round(kv.Value[1] / kv.Value[2], 1, MidpointRounding.AwayFromZero)}/{kv.Value[3]}");
    if(i == sortedResults.Length - 1)continue;
    Console.Write(", ");
}
Console.WriteLine("}");

sealed class NaturalSorting : IComparer<string?>
{
    public int Compare(string? a, string? b)
    {
        if(a is null && b is null) return 0;
        if(a is null) return -1;
        if(b is null) return 1;
        if(ReferenceEquals(a, b)) return 0;
        var min = Math.Min(a.Length, b.Length);
        for(var i = 0; i < min; ++i)
            if(a[i] < b[i]) return -1;
            else if(a[i] > b[i]) return 1;
        return a.Length - b.Length;
    }
}