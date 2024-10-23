namespace api.Utilities
{
    public class Helpers
    {
        /// <summary>
        /// Extract public ID from Cloudinary image URL path
        /// </summary>
        /// <param name="url">Cloudinary image URL</param>
        /// <returns>Image's public ID</returns>
        public static string GetPublicIdFromUrl(string url)
        {
            Uri uri = new Uri(url);
            string[] segments = uri.Segments;
            string publicIdWithExtension = segments[^1];
            string publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
            return publicId;
        }
    }
}
