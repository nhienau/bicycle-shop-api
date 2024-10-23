using api.Interfaces;
using api.Utilities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace api.Repositories
{
    public class CloudinaryRepository : ICloudinaryRepository
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryRepository(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task DeleteImageAsync(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string publicId = Helpers.GetPublicIdFromUrl(url);
                DeletionResult result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
                if (result.Error != null)
                {
                    throw new Exception(result.Error.Message);
                }
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using Stream stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                AssetFolder = Constants.CLOUDINARY_ASSET_FOLDER,
            };

            ImageUploadResult result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }

            return result.SecureUrl.ToString();
        }
    }
}
