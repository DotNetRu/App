namespace DotNetRu.DataStore.Audit.Services
{
    using System;
    using System.IO;

    using Realms;

    public class RealmService
    {
        private static Realm auditRealm;

        public static Realm AuditRealm => auditRealm ?? (auditRealm = Realm.GetInstance("Audit.realm"));

        public static void InitializeRealm()
        {          
            var realmDB = "Audit.realm";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            File.WriteAllBytes(Path.Combine(documentsPath, realmDB), ExtractRealmBytes());
        }

        private static byte[] ExtractRealmBytes()
        {
            var assembly = typeof(RealmService).Assembly;
            using (Stream resFilestream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Audit.realm"))
            {
                if (resFilestream == null)
                {
                    return null;
                }

                byte[] resultBytes = new byte[resFilestream.Length];
                resFilestream.Read(resultBytes, 0, resultBytes.Length);
                return resultBytes;
            }
        }
    }
}
