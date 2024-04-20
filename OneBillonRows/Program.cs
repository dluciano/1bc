using System.Text.Unicode;
using Microsoft.Win32.SafeHandles;
#if DEBUG
var file = "/Users/dawlin/Developer/SSD/1bc/measurements_10K copy.txt";
int numberOfSegments = 2;
#else
var file = args[0];
int numberOfSegments = Environment.ProcessorCount;
#endif

using var stream = new StreamReader(file);
string fileName = file;
long fileLength = stream.BaseStream.Length;
var fileHandle = File.OpenHandle(fileName);
var fileSegments = SegmentFile(fileName, fileHandle, fileLength, numberOfSegments);

await Parallel.ForEachAsync(fileSegments, async (fileSegment, cancellationToken) =>
{
    await fileSegment.FileRead(cancellationToken).ConfigureAwait(false);
});

Console.WriteLine("Done");

static FileSegment[] SegmentFile(string FileName, SafeFileHandle fileHandle, long fileLength, int numberOfSegments)
{
    var fileSegments = new FileSegment[numberOfSegments];
    var bufferSize = fileLength / numberOfSegments;
    for (var i = 0; i < numberOfSegments; ++i)
    {
        var offset = i * bufferSize;
        fileSegments[i] = new FileSegment(FileName, fileHandle, offset, bufferSize);
    }
    return fileSegments;
}

sealed record FileSegment(string FileName, SafeFileHandle FileHandle, long Offset, long BufferSize)
{
    public async Task RandomAccessRead(CancellationToken cancellationToken)
    {
        var buffer = new byte[BufferSize];
        var memory = new Memory<byte>(buffer);
        var readSize = await RandomAccess.ReadAsync(FileHandle, memory, Offset, cancellationToken).ConfigureAwait(false);
        ProcessContent();
        void ProcessContent()
        {
            var span = memory.Span;
            for (int i = 0, inc = 0; i < readSize; i += inc)
            {
                var cur = span[i];
                if ((cur & 0b1100_0000) == 0b1100_0000)
                    inc = 2;
                else if ((cur & 0b1110_0000) == 0b1110_0000)
                    inc = 3;
                else if ((cur & 0b1111_0000) == 0b1111_0000)
                    inc = 4;
                else
                    inc = 1;

                if (i + inc > readSize)
                {
                    // Incomplete first or last line
                    // Needs to be completed with other segments incomplete lines
                    break;
                }

                if (inc == 2)
                {
                    var next = span[i + 1];
                }
                else if (inc == 3)
                {
                    var next = span[i + 1];
                    var nextNext = span[i + 2];
                }
                else if (inc == 4)
                {
                    var next = span[i + 1];
                    var nextNext = span[i + 2];
                    var nextNextNext = span[i + 3];
                }
                else
                {

                }
            }
        }
    }
    public async Task FileRead(CancellationToken cancellationToken)
    {
        using var stream = File.OpenRead(FileName);
        var buffer = new byte[BufferSize];
        var memory = new Memory<byte>(buffer);
        stream.Position = Offset;
        var readSize = await stream.ReadAsync(memory, cancellationToken).ConfigureAwait(false);
        Process();
        void Process()
        {
            var span = memory.Span;
            for (int i = 0, inc = 0; i < readSize; i += inc)
            {
                var cur = span[i];
                if ((cur & 0b1100_0000) == 0b1100_0000)
                    inc = 2;
                else if ((cur & 0b1110_0000) == 0b1110_0000)
                    inc = 3;
                else if ((cur & 0b1111_0000) == 0b1111_0000)
                    inc = 4;
                else
                    inc = 1;

                if (i + inc > readSize)
                {
                    // Incomplete first or last line
                    // Needs to be completed with other segments incomplete lines
                    break;
                }

                if (inc == 2)
                {
                    var next = span[i + 1];
                }
                else if (inc == 3)
                {
                    var next = span[i + 1];
                    var nextNext = span[i + 2];
                }
                else if (inc == 4)
                {
                    var next = span[i + 1];
                    var nextNext = span[i + 2];
                    var nextNextNext = span[i + 3];
                }
                else
                {

                }
            }
        }
    }
}
