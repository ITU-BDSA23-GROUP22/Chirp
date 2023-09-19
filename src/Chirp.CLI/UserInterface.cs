class UserInterface{
    public static void printCheeps(IEnumerable<Cheep> cheeps){
        foreach(Cheep result in cheeps)
        {
            Console.WriteLine(result.FormattedCheep());
        }
    }
}