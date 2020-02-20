namespace IdServer.Services
{
    public interface INameGenerator
    {
        int NumberOfPossibleNames { get; set; }
        string GenerateName();
        string FilterForView(string name);
        string FilterForDb(string name);
    }
}
