namespace api.Interfaces
{
    public interface ICloudinaryRepository
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task DeleteImageAsync(String publicId);
    }
}
