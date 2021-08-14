using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcStoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RayonsController : ControllerBase
    {
        private readonly AppDbContext context;

        public RayonsController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        //restful get(liste), post(yeni kayıt), put(güncelleme), delete(silme)

        //stateless 

        [HttpGet]
        public async Task<IEnumerable<dynamic>> Get()
        {
            return await context.Rayons.OrderBy(p => p.SortOrder).Select(p=> new { p.Id, p.DateCreated, p.Enabled,p.Name, UserName = p.User.Name }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Rayon> Get(int id)
        {
            return await context.Rayons.FindAsync(id);
        }


    }
}
