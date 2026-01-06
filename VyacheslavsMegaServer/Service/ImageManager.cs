namespace VyacheslavsMegaServer.Service
{
    public class ImageManager
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageManager(IWebHostEnvironment hostEnvironment) 
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> SaveUniqueFileName(string pathInImg, IFormFile image)
        {
            string uploadedFileName = image.FileName.ToLower();
            string uploadedFileType = uploadedFileName.Substring(uploadedFileName.LastIndexOf('.'));
            string uniqueFileName = string.Concat(Guid.NewGuid(), uploadedFileType);
            string directoryPath = Path.Combine(_hostEnvironment.WebRootPath, "uploaded-resources", "img", pathInImg);

            if (!new DirectoryInfo(directoryPath).Exists) 
                Directory.CreateDirectory(directoryPath);

            using (FileStream imageStream = new FileStream(
                Path.Combine(directoryPath, uniqueFileName),
                FileMode.Create))
            {
                await image.CopyToAsync(imageStream);
            }

            return uniqueFileName;
        }

        public void RemoveImage(string pathInImg, string fileName)
        {
            FileInfo image = new FileInfo(Path.Combine(_hostEnvironment.WebRootPath, "uploaded-resources", "img", pathInImg, fileName));
            if (image.Exists) image.Delete();
        }
    }
}
