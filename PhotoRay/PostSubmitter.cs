using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PhotoRay
{
    public class PostCompletedEventArgs : EventArgs
    {
        public bool Success { get; set; }

        public PostCompletedEventArgs(bool success)
        {
            Success = success;
        }
    }

    public class PostSubmitter
    {
        public string Url { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public event EventHandler<PostCompletedEventArgs> Completed;
        private readonly string _boundary = "----------" + DateTime.Now.Ticks;

        public void Submit()
        {
            // Prepare web request
            var myRequest = (HttpWebRequest) WebRequest.Create(new Uri(Url));
            myRequest.Method = "POST";
            myRequest.ContentType = string.Format("multipart/form-data; boundary={0}", _boundary);

            myRequest.BeginGetRequestStream(GetRequestStreamCallback, myRequest);
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest) asynchronousResult.AsyncState;
            var postStream = request.EndGetRequestStream(asynchronousResult);
            WriteMultipartObject(postStream, Parameters);
            postStream.Close();
            request.BeginGetResponse(GetResponseCallback, request);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var request = (HttpWebRequest) asynchronousResult.AsyncState;
                var response = (HttpWebResponse) request.EndGetResponse(asynchronousResult);
                var streamResponse = response.GetResponseStream();
                var streamRead = new StreamReader(streamResponse);
                streamRead.Close();
                // Release the HttpWebResponse
                response.Close();
            }
            catch(Exception)
            {
                OnCompleted(false);
            }
            OnCompleted(true);
        }


        public void WriteMultipartObject(Stream stream, object data)
        {
            var writer = new StreamWriter(stream);
            if (data != null)
            {
                var entries = data as Dictionary<string, object>;
                if (entries != null)
                {
                    foreach (var entry in entries)
                    {
                        WriteEntry(writer, entry.Key, entry.Value);
                    }
                }
            }
            writer.Write("--");
            writer.Write(_boundary);
            writer.WriteLine("--");
            writer.Flush();
        }

        private void WriteEntry(StreamWriter writer, string key, object value)
        {
            if (value != null)
            {
                writer.Write("--");
                writer.WriteLine(_boundary);
                if (value is byte[])
                {
                    var ba = value as byte[];
                    writer.WriteLine(
                        @"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, "sentPhoto.jpg");
                    writer.WriteLine(@"Content-Type: application/octet-stream");
                    writer.WriteLine(@"Content-Length: " + ba.Length);
                    writer.WriteLine();
                    writer.Flush();
                    var output = writer.BaseStream;
                    output.Write(ba, 0, ba.Length);
                    output.Flush();
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""", key);
                    writer.WriteLine();
                    writer.WriteLine(value.ToString());
                }
            }
        }

        private void OnCompleted(bool success)
        {
            var handler = Completed;
            if(handler != null)
            {
                handler(this, new PostCompletedEventArgs(success));
            }
        }
    }
}