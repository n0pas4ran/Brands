using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;



namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Brands : Controller
    {
        static List<Entry> entries = new List<Entry>();

        private readonly ILogger<Brands> _logger;

        public Brands(ILogger<Brands> logger)
        {
            _logger = logger;
        }
        // GET all: api/Brands
        
        [HttpGet]
        [Authorize]
        public string Read()
        {
            string output = "Nothing has been added";
            if(entries.Count > 0)
            {
                output = "";
                foreach(var ent in entries)
                {
                    output += ent.ToString() + "\n";
                }
            }
            return output;
        }

        // GET api/Brands/{id}
        
        [HttpGet("{id}")]
        [Authorize]
        public string Read(int id)
        {
            string outPut = "Not Found";
            if((id < entries.Count) && (id >= 0))
            {
                outPut = JsonConvert.SerializeObject(entries[id]);
            }
            Console.WriteLine(String.Format("Запрошена запись № {0} : {1}", id, outPut));
            return outPut;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Entry ent)
        {
            if (ent == null)
            {
                return BadRequest();
            }

            entries.Add(ent);
            Console.WriteLine(String.Format("Всего записей {0}, послано: {1}", entries.Count, ent.ToString()));

            return new OkResult();

        }

        //Change /api/Brands/{id}
        [HttpPut("{id}")]
        public IActionResult Change(int id, [FromBody] Entry ent){
            if((id<0)||(id>entries.Count)||(ent == null)){
                return BadRequest();
            }
            entries.RemoveAt(id);
            entries.Insert(id, ent);
            Console.WriteLine(String.Format("Запись {0} успешно изменена на {1}", id, ent.ToString()));
            return new OkResult();

        }
        
        // DELETE api/Brands/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if((id<0) || (id>entries.Count)){
                return BadRequest();
            }
            entries.RemoveAt(id);
            Console.WriteLine("Запись "+id+" успешно удалена.");
            return new OkResult();
        }
    }
}
