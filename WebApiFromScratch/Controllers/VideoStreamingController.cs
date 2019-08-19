using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApiFromScratch.Controllers
{
    public class VideoStreamingController : ApiController
    {
        public async Task<HttpResponseMessage> GetVideoContent()
        {
            var httpResponce = Request.CreateResponse();
            httpResponce.Content = new PushStreamContent(WriteContentToStream);
            return httpResponce;
        }

        public async void WriteContentToStream(Stream outputStream, HttpContent content, TransportContext transportContext)
        {
            //path of file which we have to read//  
            var filePath = HttpContext.Current.Server.MapPath("~/Videos/Ejemplo_de_video.mp4");
            //here set the size of buffer, you can set any size  
            int bufferSize = 1000;
            byte[] buffer = new byte[bufferSize];
            //here we re using FileStream to read file from server//  
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int totalSize = (int)fileStream.Length;
                /*here we are saying read bytes from file as long as total size of file 

                is greater then 0*/
                while (totalSize > 0)
                {
                    // Simulate thortling
                    Thread.Sleep(100);

                    int count = totalSize > bufferSize ? bufferSize : totalSize;
                    //here we are reading the buffer from orginal file  
                    int sizeOfReadedBuffer = fileStream.Read(buffer, 0, count);
                    //here we are writing the readed buffer to output//  
                    await outputStream.WriteAsync(buffer, 0, sizeOfReadedBuffer);
                    //and finally after writing to output stream decrementing it to total size of file.  
                    totalSize -= sizeOfReadedBuffer;
                }
            }
        }
    }
}