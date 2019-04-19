using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzFuncDocker
{
    public static class FizzBuzz
    {
        [FunctionName("FizzBuzz")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string numberQueryString = req.Query["number"];                                    

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            numberQueryString = numberQueryString ?? data?.number;

            int number;
            // if its not a number
            if(!Int32.TryParse(numberQueryString, out number ))
            {
                return new BadRequestObjectResult("Please pass a number on the query string or in the request body");
            }

            
            return (ActionResult)new OkObjectResult($"Result of FizzBuzz: {GetFizzBuzz(number)}");
        }

        public static string GetFizzBuzz(int number)
        {
            if(number % 3 == 0 && number % 5 == 0 ){
                return "FizzBuzz!";
            }else if(number % 3 == 0){
                return "Fizz";
            }else if(number % 5 == 0){
                return "Buzz";
            }
            return number.ToString();
        }
    }
}
