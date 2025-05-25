using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace mood_moments.Services
{
    public class MauiAppFileProvider : IAppFileProvider
    {
        public Task<Stream> OpenAppPackageFileAsync(string filename)
        {
            return FileSystem.OpenAppPackageFileAsync(filename);
        }
    }
}
