namespace SimpleDB;
using CsvHelper;
using System.Globalization;

public sealed class CsvDatabase<T> : IDatabaseRepository<T>
{
    string path;

    public CsvDatabase(string path){
        this.path = path;
    }
    public IEnumerable<T> Read(int? limit = null)
    {
    
        List<T> output = new List<T>();

        using (var sr = new StreamReader(path))
            using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture)){
                
                csv.Read();
                csv.ReadHeader();
                
                for(var i = 0; i < limit ; i++){
                    if(csv.Read()){
                        var record = csv.GetRecord<T>();
                        if(record != null){
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
        using(var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.NextRecord();
                csv.WriteRecord(record);
            }
    }
}
