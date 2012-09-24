using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using YouScribe.Rest.Models;

namespace YouScribe.Rest.Helpers
{
    public static class FileUrlModelHelpers
    {
        public static IEnumerable<FileModel> ToFileModel(this IEnumerable<FileUrlModel> filesUri)
        {
            if (filesUri == null || filesUri.Any() == false)
                throw new ArgumentNullException("filesUri", "You need to select file(s) Uri to upload");
            if (filesUri.Any(f => f.IsValid == false))
                return null;

            var files = new List<FileModel>();

            using (var webClient = new WebClient())
            {
                foreach (var fileUrl in filesUri)
                {
                    var file = new FileModel
                    {
                        ContentType = fileUrl.ContentType,
                        FileName = fileUrl.FileName
                    };
                    file.Content = webClient.OpenRead(fileUrl.Uri);

                    files.Add(file);
                }
            }

            return files;
        }
    }
}
