namespace SimpleDB;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public sealed class CsvDatabase<T> : IDatabaseRepository<T>
{
    private static CsvDatabase<T> instance;
    public static CsvDatabase<T> getInstance(string path)
    {
        if (instance == null)
            instance = new CsvDatabase<T>(path);

        return instance;
    }
    string path;

    public CsvDatabase(string path)
    {
        this.path = path;
    }
    public IEnumerable<T> Read(int? limit = null)
    {

        List<T> output = new List<T>();

        using (var sr = new StreamReader(path))
        using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();

            for (var i = 0; i < limit; i++)
            {
                if (csv.Read())
                {
                    var record = csv.GetRecord<T>();
                    if (record != null)
                    {
                        output.Add(record);
                    }
                }
            }
        }
        return output;
    }

    public void Store(T record)
    {
        using (var stream = File.Open(path, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.NextRecord();
            csv.WriteRecord(record);
        }
    }
    public void DeleteLastLine()
    {
        var lines = File.ReadAllLines(path).ToList();

        if (lines.Count == 0)
        {
            return;
        }

        // Create a new list of lines excluding the last line
        lines.RemoveAt(lines.Count - 1);

        // Overwrite the file with the updated lines
        File.WriteAllLines(path, lines);

    }
    public string GetLastItem()
    {
        string lastLine = null;
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (StreamReader reader = new StreamReader(fs))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    lastLine = line;
                }
            }
        }

        return lastLine;
    }

}

