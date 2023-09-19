public record Cheep(string Author, string Message, long Timestamp){

    public string FormattedCheep(){

        long seconds = Timestamp;
        DateTime convertedTime = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
        convertedTime = convertedTime.AddSeconds(seconds);
        convertedTime = convertedTime.ToLocalTime();
        string formattedTime = convertedTime.ToString("HH:mm");

        string Output = $"{Author} @ {formattedTime}: {Message}";
        return Output;
    }
}

