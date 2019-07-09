using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
                return NotFound();
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
                return NotFound();
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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new CelestialObject { Id = celestialObject.Id }, celestialObject);
            
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var item = _context.CelestialObjects.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = celestialObject.Name;
            item.OrbitalPeriod = celestialObject.OrbitalPeriod;
            item.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(item);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var item = _context.CelestialObjects.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = name;
            _context.CelestialObjects.Update(item);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();

            if (item == null)
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(item);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
