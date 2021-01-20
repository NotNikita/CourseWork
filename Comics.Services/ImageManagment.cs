using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Comics.Services
{
    public class ImageManagment
    {
        public static Cloudinary cloudinary;

        public const string CLOUD_NAME = "dynastycomic";
        public const string API_KEY = "574426444463172";
        public const string API_SECRET = "6Jjjakv_7HXd1oGpIIP3FKm_2DI";

        public ImageManagment()
        {
            Account account = new Account(CLOUD_NAME, API_KEY, API_SECRET);
            cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(string imagePath, Stream imageStream) //+ public_id
        {
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imagePath, imageStream)

                    //PublicId = "sample_id" по которому будет уже реализована загрузка из сервера картинок, можно организовывать в папки: user/public_id
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult.Url.AbsoluteUri;
                //Log "upload successfully"
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"ImageManagment Service. NULL REF in upload async. {e.Message}");
                return null;
                //Log error occured e.Message
            }
        }

        public void GetImagesByIds(string user_name, List<string> ids)
        {
            var publicIds = ids;
            var some = cloudinary.ListResourcesByPublicIds(publicIds);
            //unparse json to get 'url' from each
        }

        public void GetImageById(string user_name, string id)
        {
            var some = cloudinary.GetResource(id);
            //unparse json to get 'url'
        }
    }
}
