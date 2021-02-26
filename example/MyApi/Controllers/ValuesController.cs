using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Citolab.Persistence;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValuesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            var sampleEntity = _unitOfWork.GetCollection<SampleEntity>();
            if (sampleEntity.AsQueryable().Any()) return;
            for (var i = 0; i < 10; i++)
            {
                sampleEntity.AddAsync(new SampleEntity
                    { Id = new Guid(), Value = $"Value {i}" }).Wait();
            }
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<SampleEntity>> GetAll()
        {
            return Ok(_unitOfWork.GetCollection<SampleEntity>().AsQueryable().ToList());
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            return Ok(_unitOfWork.GetCollection<SampleEntity>()
                .FirstOrDefaultAsync(sample => sample.Id == id).Result);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] SampleEntity value)
        {
            _unitOfWork.GetCollection<SampleEntity>().AddAsync(value);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] SampleEntity value)
        {
            if (id != value.Id)
            {
                throw new Exception("Id should match ObjectId");
            }
            _unitOfWork.GetCollection<SampleEntity>().UpdateAsync(value).Wait();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _unitOfWork.GetCollection<SampleEntity>().DeleteAsync(id).Wait();
        }
    }
}
