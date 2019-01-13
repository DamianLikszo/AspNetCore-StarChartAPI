using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestailObject = _context.CelestialObjects.Find(id);

            if (celestailObject == null)
                return NotFound();

            celestailObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();

            return Ok(celestailObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestailObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();

            if(!celestailObjects.Any())
            {
                return NotFound();
            }

            foreach (var celestailObject in celestailObjects)
            {
                celestailObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestailObject.Id).ToList();
            }

            return Ok(celestailObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestailObjects = _context.CelestialObjects.ToList();

            foreach (var celestailObject in celestailObjects)
            {
                celestailObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestailObject.Id).ToList();
            }

            return Ok(celestailObjects);
        }
    }
}
