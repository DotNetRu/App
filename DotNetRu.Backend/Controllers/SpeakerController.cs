﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using XamarinEvolve.DataObjects;
using XamarinEvolve.Backend.Models;
using System;
using XamarinEvolve.Backend.Identity;

namespace XamarinEvolve.Backend.Controllers
{
    public class SpeakerController : TableController<Speaker>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            XamarinEvolveContext context = new XamarinEvolveContext();
            DomainManager = new EntityDomainManager<Speaker>(context, Request, true);
        }

        [EnableQuery(MaxTop = 500, PageSize =200)]
        public IQueryable<Speaker> GetAllSpeaker()
        {
            return Query();
        }

        public SingleResult<Speaker> GetSpeaker(string id)
        {
            return Lookup(id);
        }

        [EmployeeAuthorize]
        public Task<Speaker> PatchSpeaker(string id, Delta<Speaker> patch)
        {
             return UpdateAsync(id, patch);
        }

        [EmployeeAuthorize]
        public async Task<IHttpActionResult> PostSpeaker(Speaker item)
        {
            Speaker current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        [EmployeeAuthorize]
        public Task DeleteSpeaker(string id)
        {
             return DeleteAsync(id);
        }

    }
}