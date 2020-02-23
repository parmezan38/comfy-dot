using System.Threading.Tasks;

namespace IdServer.Services
{
    public interface INameGeneratorService
    {
        Task<string> GetName();
        string FilterForView(string name);
        string FilterForDb(string name);
    }
}
