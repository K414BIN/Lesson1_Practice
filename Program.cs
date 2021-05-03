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

        static async Task Main(string[] args)
        {
              try
              {
                var value1 =   GetOnePost(4);
                var value2 =   GetOnePost(5);
                var value3 =   GetOnePost(6);
                var value4 =   GetOnePost(7);
                var value5 =   GetOnePost(8);
                var value6 =   GetOnePost(9);
                var value7 =   GetOnePost(10);
                var value8 =   GetOnePost(11);
                var value9 =   GetOnePost(12);
                var value10 =  GetOnePost(13);
                var tasks = new List<Task<Post>>() ;
                tasks.AddRange(new [] {value1,value2,value3,value4,value5,value6,value7,value8,value9,value10});
                await Task.WhenAll(tasks);
                // Круто - try внутри try !!!
                try
                    {
                    // Для того, чтобы просто вывести на экран - закомментируйте строки перед и после "плюсов"
                    // ************************
                            FileStream fs = new FileStream(fname, FileMode.Create);
                            TextWriter tmp = Console.Out;
                            StreamWriter sw = new StreamWriter(fs);
                            Console.SetOut(sw);
                    //+++++++++++++++++++++++++++++++
                            foreach ( var z in tasks)
                            {
                              Console.WriteLine(z.Result.UserId);
                              Console.WriteLine(z.Result.Id);
                              Console.WriteLine(z.Result.Title);
                              Console.WriteLine(z.Result.Body);
                              Console.WriteLine();
                            }
                    //+++++++++++++++++++++++++++++
                            Console.SetOut(tmp);
                            sw.Close();
                    // ************************
                     }
                // вспомогательный try
                catch(IOException e)
                {
                    TextWriter errorWriter = Console.Error;
                    errorWriter.WriteLine(e.Message);
                }
              }
            // главный try
               catch (Exception ex)
              {
                    Console.WriteLine(ex.Message);
              }
          // The End.
                Console.WriteLine($"All result stored in {fname} file!");
        }

       static async Task <Post> GetOnePost(int num)
       {
            var response = await client.GetAsync(mainAddress+"/"+ num.ToString());
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
