using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JSON_Web_API_Consumption
{
	class Program
	{
        const string PictaculousPath = "http://localhost/pruebaColoresApi.php";

		static void Main(string[] args)
		{
			var PictaculousUri = new Uri(PictaculousPath);
            var json = ApiRequest.GetJson(PictaculousUri);
            Console.WriteLine(json.ToString());
            Console.ReadKey();
            //var jsonObject = JsonConvert.DeserializeObject<List<GitHubRepo>>(json);
		}
	}

    //public class Owner
    //{
    //    [JsonProperty("avatar_url")]
    //    public string AvatarUrl { get; set; }
    //    public string login { get; set; }
    //    public string url { get; set; }
    //    public string gravatar_id { get; set; }
    //    public int id { get; set; }
    //}

    //public class GitHubRepo
    //{
    //    public string description { get; set; }
    //    public bool has_wiki { get; set; }
    //    public string svn_url { get; set; }
    //    public int open_issues { get; set; }
    //    public string language { get; set; }
    //    public int watchers { get; set; }
    //    public bool fork { get; set; }
    //    public string homepage { get; set; }
    //    public string git_url { get; set; }
    //    public string clone_url { get; set; }
    //    public string pushed_at { get; set; }
    //    public int size { get; set; }
    //    public bool @private { get; set; }
    //    public string created_at { get; set; }
    //    public string html_url { get; set; }
    //    public Owner owner { get; set; }
    //    public string name { get; set; }
    //    public string url { get; set; }
    //    public object mirror_url { get; set; }
    //    public bool has_downloads { get; set; }
    //    public bool has_issues { get; set; }
    //    public string ssh_url { get; set; }
    //    public string updated_at { get; set; }
    //    public int id { get; set; }
    //    public int forks { get; set; }
    //}
}
