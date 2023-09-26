namespace SimpleDB.test;

using SimpleDB;

public class DatabaseUnitTest
{

    CsvDatabase<Message> database;
    [Fact]
    public void TestReadCheep()
    {
        database = CsvDatabase<Message>.getInstance("../../../test_csv_db.csv");

        IEnumerable<Message> results = database.Read(1);
        foreach (Message result in results)
        {
            bool doesResultMatch = result.GetContent().Equals("This is a test message");
            Assert.True(doesResultMatch, "Result should be 'This is a test message' yes, not '" + result.GetContent() + "'");
        }
    }

    [Fact]
    public void TestWriteCheep()
    {
        database = CsvDatabase<Message>.getInstance("../../../test_csv_db.csv");

        IEnumerable<Message> results = database.Read(10);
        int originalRecordsCount = 0;
        foreach (Message result in results)
        {
            originalRecordsCount++;
        }

        DateTimeOffset convertedTime = DateTimeOffset.UtcNow;
        long unixTime = convertedTime.ToUnixTimeSeconds();
        database.Store(new Message(unixTime.ToString()));

        results = database.Read(10);
        int recordsCount = 0;
        string lastMessageRead = "";
        foreach (Message result in results)
        {
            recordsCount++;
            lastMessageRead = result.GetContent();
        }

        int goalCount = originalRecordsCount + 1;
        Assert.True(recordsCount == goalCount, $"Amount of records should be {goalCount}, not {recordsCount}");
        Assert.True(lastMessageRead.Equals(unixTime.ToString()), $"Result should be {unixTime.ToString()}, not {lastMessageRead}");
    }
}