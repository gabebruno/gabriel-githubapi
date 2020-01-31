using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using gabriel_githubapi.Model;

namespace gabriel_githubapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private HttpClient _client;
        public UsersController()
        {
            _client = new HttpClient();
        }

        //GET to return de user list and the link to next page
        public async Task<ResultUserList> Get(string since)
        {

            _client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            var response = await _client.GetAsync("https://api.github.com/users?since=" + since);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("não foi possível retornar resultados");
            }

            
            var content = await response.Content.ReadAsStringAsync();
            var usersList = JsonConvert.DeserializeObject<List<User>>(content);

            //Github API return always 30 results, so the last one id is the used lika param to the next page.
            var newSince = usersList[29].id;

            //TODO Improve this code, maybe with a dedicate class to mount URLs.
            var linkToNextPage = "http://" + HttpContext.Request.Host + HttpContext.Request.Path + "?since=" + newSince;
            
            return new ResultUserList
            {
                Users = usersList,
                NextPage = linkToNextPage
            };
        }

        //GET to return details for a specified user.
        [HttpGet("{username}/details")]
        public async Task<UserDetails> ForDetails(string username)
        {

            _client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            var response = await _client.GetAsync("https://api.github.com/users/"+username);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Não foi possível retornar resultados");
            }

            var content = await response.Content.ReadAsStringAsync();
            var details = JsonConvert.DeserializeObject<UserDetails>(content);
            
            return details;
        }

        //GET to return the public repos for a specified user.
        [HttpGet("{username}/repos")]
        public async Task<List<UserRepos>> ForRepos(string username)
        {

            _client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            var response = await _client.GetAsync("https://api.github.com/users/" + username + "/repos");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Não foi possível retornar resultados");
            }

            var content = await response.Content.ReadAsStringAsync();
            var details = JsonConvert.DeserializeObject<List<UserRepos>>(content);


            return details;
        }

    }
}