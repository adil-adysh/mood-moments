using System.IO;
using System.Threading.Tasks;

namespace mood_moments.Services
{
    public interface IAppFileProvider
    {
        Task<Stream> OpenAppPackageFileAsync(string filename);
    }
}
