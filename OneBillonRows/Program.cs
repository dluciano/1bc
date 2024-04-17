using System.Text;

static string ReadStation(StreamReader stream){
    var cur = -1;
    var sb = new StringBuilder();
    while(true){
        cur = stream.Read();
        if(cur == -1) return string.Empty;
        if(cur == ';') return sb.ToString();
        sb.Append((char)cur);
    }
}
static decimal ReadMeasurement(StreamReader stream){
    var cur = -1;
    var sb = new StringBuilder();
    while(true){
        cur = stream.Read();
        if(cur == -1){
            if(sb.Length == 0) throw new InvalidProgramException("Incomplete measurement");
            return decimal.Parse(sb.ToString());
        }
        if(cur == '\n') return decimal.Parse(sb.ToString());
        sb.Append((char)cur);
    }
}
static void UpdateStatistics(
    Dictionary<string, decimal[]> db,
    string station,
    decimal measurement
) {
    if (!db.ContainsKey(station))
        db[station] = [decimal.MaxValue, 0M, 0M, decimal.MinValue];
    db[station][0] = Math.Min(db[station][0], measurement);
    db[station][1] = Math.Round(measurement + db[station][1], 1);
    db[station][2]++;
    db[station][3] = Math.Max(db[station][3], measurement);
}

var file = args[0];

var statsDb = new Dictionary<string, decimal[]>();
using var stream = new StreamReader(file);

while(true) {
    var station = ReadStation(stream);
    if(station == string.Empty) break;
    var measurement = ReadMeasurement(stream);
    UpdateStatistics(statsDb, station, measurement);
}

Console.Write("{");
var sortedResults = statsDb.OrderBy(kv => kv.Key, new NaturalSorting()).ToArray();
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