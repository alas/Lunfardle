namespace Lunfardle.Game;

using System.IO.Compression;
using System.Text;

internal class WordsLists
{
    private readonly string[] SpanishFull;

    private readonly string[] Lunfardo;

    public WordsLists()
    {
#if DEBUG
        var start = DateTime.UtcNow;
#endif

        #region Decompress Dictionaries

        var spanishFull = FromGzipAsync(WordListsCompressed.SpanishFull).Result;
        var lunfardo = FromGzipAsync(WordListsCompressed.Lunfardo).Result;

        #endregion

#if DEBUG
        var dt = DateTime.UtcNow.Subtract(start).TotalSeconds;
        System.Diagnostics.Debug.WriteLine($"Decompressing dictionaries took {dt} seconds");
        start = DateTime.UtcNow;
#endif

        SpanishFull = spanishFull;
        Lunfardo = lunfardo;

#if DEBUG
        dt = DateTime.UtcNow.Subtract(start).TotalSeconds;
        System.Diagnostics.Debug.WriteLine($"Processing dictionaries took {dt} seconds");
#endif
    }

    public static async Task<string[]> FromGzipAsync(string value)
    {
        var bytes = Convert.FromBase64String(value);
        await using var input = new MemoryStream(bytes);
        await using var output = new MemoryStream();
        await using var stream = new GZipStream(input, CompressionMode.Decompress);

        await stream.CopyToAsync(output);
        await stream.FlushAsync();

        return Encoding.Unicode.GetString(output.ToArray())
            .Split("\n");
    }

    public string GetWordOfTheDay()
    {
        var start = new DateTime(2022, 4, 1);
        var totalDays = Convert.ToInt32(
            DateTime.Today.Subtract(start).TotalDays);
        var index = totalDays % Lunfardo.Length;
        return Lunfardo[index];
    }

    public bool WordExists(string word)
    {
        return SpanishFull.Contains(word) || Lunfardo.Contains(word);
    }
}
