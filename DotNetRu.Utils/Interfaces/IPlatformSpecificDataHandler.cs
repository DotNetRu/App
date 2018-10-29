using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetRu.Utils.Interfaces
{
	public interface IPlatformSpecificDataHandler<T>
	{
		Task UpdateMultipleEntities(IEnumerable<T> data);
		Task UpdateSingleEntity(T data);
	}
}
