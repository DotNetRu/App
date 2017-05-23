using System.Reflection;
using Plugin.EmbeddedResource;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
	public class CodeOfConductViewModel : ViewModelBase
	{
		public static string CodeOfConductContent = ResourceLoader.GetEmbeddedResourceString(Assembly.Load(new AssemblyName("XamarinEvolve.Clients.Portable")), "codeofconduct.txt");

		public string CodeOfConduct => CodeOfConductContent;

		public string PageTitle => AboutThisApp.CodeOfConductPageTitle;
	}
}

