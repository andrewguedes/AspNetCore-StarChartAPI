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
            var celestialObjects = _context.CelestialObjects.Find(id);

            if (celestialObjects == null)
            {
                NotFound();
            }

            celestialObjects.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();

            return Ok(celestialObjects);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList();

            if (!celestialObjects.Any())
            {
                NotFound();
            }

            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == item.Id).ToList();

            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();


            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(celestialObjects);
        }
    }
}
