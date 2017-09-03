using System.Linq;
using System.Threading.Tasks;
using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Azure.Abstractions;

namespace XamarinEvolve.DataStore.Azure.Stores
{
	public class ConferenceFeedbackStore : BaseStore<ConferenceFeedback>, IConferenceFeedbackStore
	{
		public async Task<bool> LeftFeedback()
		{
			await InitializeStore();
			var items = await Table.ReadAsync().ConfigureAwait(false);
			return items.Any();
		}

		public Task DropConferenceFeedback()
		{
			return Task.FromResult(true);
		}

		public override string Identifier => "ConferenceFeedback";

	}
}
