using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.IO;


namespace Lesson1_Practice
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        public static string mainAddress = "https://jsonplaceholder.typicode.com/posts";
        public static string fname ="result.txt";

        private static int magicNumber1=4;
        private static int magicNumber2=13;

        static async Task Main(string[] args)
        {
              try
              {
                var tasks = new List<Task<Post>>() ;
                for (int i=magicNumber1 ;i<=magicNumber2;i++)  tasks.AddRange(new [] {GetPost(i) });                
                await Task.WhenAll(tasks);
                try
                    {
                    // ************************
                            FileStream fs = new FileStream(fname, FileMode.Create);
                            TextWriter tmp = Console.Out;
                            StreamWriter ecrivain_streaming  = new StreamWriter(fs);
                            Console.SetOut( ecrivain_streaming );
                    //+++++++++++++++++++++++++++++++
                            foreach ( var variable  in tasks)
                            {
                              Console.WriteLine(variable.Result.UserId);
                              Console.WriteLine(variable.Result.Id);
                              Console.WriteLine(variable.Result.Title);
                              Console.WriteLine(variable.Result.Body);
                              Console.WriteLine();
                            }
                    //+++++++++++++++++++++++++++++
                            Console.SetOut(tmp);
                             ecrivain_streaming.Close();
                    // ************************
                     }
                catch(IOException e)
                {
                    TextWriter errorWriter = Console.Error;
                    errorWriter.WriteLine(e.Message);
                }
              }
              catch (Exception ex)
              {
                    Console.WriteLine(ex.Message);
              }
          // The End.
                Console.WriteLine($"All result stored in {fname} file!");
        }

       static async Task <Post> GetPost(int id)
       {
            var response = await client.GetAsync(mainAddress+"/"+ id.ToString());
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error while getting an http reponse!");
                return default;
            }
                var content =await response.Content.ReadAsStringAsync();
                var post =  JsonConvert.DeserializeObject<Post>(content);
            return post;
       }
    }
}
