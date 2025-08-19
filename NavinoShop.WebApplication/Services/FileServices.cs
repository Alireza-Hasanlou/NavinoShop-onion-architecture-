using Shared.Application.Service;

namespace NavinoShop.WebApplication.Services
{
    internal class FileServices : IFileService
    {
        public bool DeleteImage(string route)
        {
            throw new NotImplementedException();
        }

        public bool ResizeImage(string imageName, string Folder, int newSize)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadImage(IFormFile file, string folder)
        {
            throw new NotImplementedException();
        }
    }
}
