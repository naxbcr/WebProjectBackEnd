using Microsoft.AspNetCore.Http;
using Microsoft.Owin;
using Newtonsoft.Json;
using project1.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace project1.Controllers
{
    public class FilesController : ApiController
    {

        [Route("api/Upload")]
        public async Task<IHttpActionResult> Upload()
        {   // Входные данные лучше не указывать иначе опять прилетит 415 ошибка.

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/UploadedFiles");
            var streamProvider = new CustomMultipartFormDataStreamProvider(root); // Используем наш иначе имена файлов буду упоротые
            //var streamProvider = new MultipartFormDataStreamProvider(root);
            
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(streamProvider); // на данном этапе он уже сохраняется файл, в папку указанную в руте. 
                //Все что ниже так на будущее.

                //// This illustrates how to get the file names.
                foreach (MultipartFileData file in streamProvider.FileData)
                {
                    Console.WriteLine(file.Headers.ContentDisposition.FileName);
                    Console.WriteLine("Server file path: " + file.LocalFileName);
                    
                }

                //foreach (var file in streamProvider.Contents)
                //{

                //var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                //var buffer = await file.ReadAsByteArrayAsync();
                //Do whatever you want with filename and its binaray data.

                //File.WriteAllBytes(filename,buffer);
                //}

                //Stream requestStream = await Request.Content.ReadAsStreamAsync();
                //using (FileStream fileStream = File.Create(@"C:\UploadedFiles\" + fileName))
                //{
                // await requestStream.CopyToAsync(fileStream);
                //}

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpGet]
        [Route("api/Download/{key}")]
        public IHttpActionResult Download(string key)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/UploadedFiles/"+key);

            if (File.Exists(path)) {

                return new FileActionResult(key);
            }
            else
            {
                return NotFound();
            }
        }
    }

    public class FileActionResult : IHttpActionResult
    {
        public FileActionResult(string fileId)
        {
            this.FileId = fileId;
        }

        public string FileId { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/UploadedFiles/" + FileId);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StreamContent(File.OpenRead(@"<base path>" + FileId));
            var stream = new FileStream(path, FileMode.Open);
            response.Content = new StreamContent(stream);
            //response.Content = new StreamContent(new FileStream(path, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition.FileName = FileId;
            response.Content.Headers.ContentLength = stream.Length;

            return Task.FromResult(response);
        }
    }
}
