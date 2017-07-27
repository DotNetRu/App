using System;
using System.Data.Entity;

namespace XamarinEvolve.Backend.Models
{
    //DropCreateDatabaseIfModelChanges
    public class XamarinEvolveContextInitializer : CreateDatabaseIfNotExists<XamarinEvolveContext>
    {
        protected override void Seed(XamarinEvolveContext context)
        {
            //Seed Data Here
        }
    }
}