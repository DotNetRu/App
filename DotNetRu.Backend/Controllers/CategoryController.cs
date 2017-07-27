﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using XamarinEvolve.DataObjects;
using XamarinEvolve.Backend.Models;
using XamarinEvolve.Backend.Identity;

namespace XamarinEvolve.Backend.Controllers
{
    public class CategoryController : TableController<Category>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            XamarinEvolveContext context = new XamarinEvolveContext();
            DomainManager = new EntityDomainManager<Category>(context, Request, true);
        }

        public IQueryable<Category> GetAllCategory()
        {
            return Query(); 
        }

        public SingleResult<Category> GetCategory(string id)
        {
            return Lookup(id);
        }

        [EmployeeAuthorize]
        public Task<Category> PatchCategory(string id, Delta<Category> patch)
        {
             return UpdateAsync(id, patch);
        }

        [EmployeeAuthorize]
        public async Task<IHttpActionResult> PostCategory(Category item)
        {
            Category current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        [EmployeeAuthorize]
        public Task DeleteCategory(string id)
        {
             return DeleteAsync(id);
        }

    }
}