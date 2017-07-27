using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamarinEvolve.Utils
{
	public interface IPlatformSpecificDataHandler<T>
	{
		Task UpdateMultipleEntities(IEnumerable<T> data);
		Task UpdateSingleEntity(T data);
	}
}
