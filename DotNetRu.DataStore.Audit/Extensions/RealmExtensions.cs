namespace DotNetRu.DataStore.Audit.Extensions
{
    using System;

    using Realms;

    public static class RealmExtensions
    {
        public static TApp ToModel<TRealm, TApp>(this TRealm realmObject) where TRealm : RealmObject
        {            
            throw new NotImplementedException();
        }
    }
}
