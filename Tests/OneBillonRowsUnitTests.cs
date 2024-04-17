using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Tests;

public class OneBillonRowsUnitTests
{
    [Theory]
    [InlineData("measurements-1")]
    // [InlineData("measurements-3")]
    // [InlineData("measurements-short")]
    // [InlineData("measurements-10")]
    // [InlineData("measurements-boundaries")]
    // [InlineData("measurements-shortest")]
    // [InlineData("measurements-10000-unique-keys")]
    // [InlineData("measurements-complex-utf8")]
    // [InlineData("measurements-2")]
    // [InlineData("measurements-dot")]
    // [InlineData("measurements-20")]
    // [InlineData("measurements-rounding")]
    public async Task ValidateConsoleOutput(string file)
    {
        var input = Path.Combine("Data/Input", $"{file}.txt");
        var expectedOutput = Path.Combine("Data/Output", $"{file}.out.csv");
        var processPath = "../../OneBillonRows/debug/OneBillonRows";
        if (!File.Exists(input)) throw new InvalidOperationException($"The input test data: `{input}` does not exist.");
        if (!File.Exists(expectedOutput)) throw new InvalidOperationException($"The expected output test data: `{expectedOutput}` does not exist.");
        if(!File.Exists(processPath)) throw new InvalidOperationException($"OneBillonRows binary do not exist on location: `{processPath}`");
        var startInfo = new ProcessStartInfo
        {
            FileName = processPath,
            Arguments = input,
            RedirectStandardOutput = true,  // Redirect stdout
            UseShellExecute = false,         // Needed to redirect stdout
            CreateNoWindow = true            // Do not create a window
        };
        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Cannot start the OneBillonRows process. The process start method returned null.");
        await process.WaitForExitAsync();
        var consoleOutput = await process.StandardOutput.ReadToEndAsync();

        var csvConsoleOutput = Regex.Replace(
            consoleOutput
            .Replace("{", string.Empty)
            .Replace("}", string.Empty)
            .Replace("=", ";")
            .Replace("/", ";")
            , @"(\d), ", m => m.Groups[1].Value + "\n");

        var expectedOutputContent = await File.ReadAllTextAsync(expectedOutput);
        Assert.Equal(expectedOutputContent, csvConsoleOutput);
    }
}