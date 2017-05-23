using System.Threading.Tasks;
using XamarinEvolve.DataObjects;
using XamarinEvolve.DataStore.Abstractions;

namespace XamarinEvolve.DataStore.Mock
{
	public class ConferenceFeedbackStore : BaseStore<ConferenceFeedback>, IConferenceFeedbackStore
	{
		public Task<bool> LeftFeedback()
		{
			return Task.FromResult(Settings.LeftConferenceFeedback());
		}

		public async Task DropConferenceFeedback()
		{
			await Settings.ClearFeedback();
		}

		public override Task<bool> InsertAsync(ConferenceFeedback item)
		{
			Settings.LeaveConferenceFeedback(true);
			return Task.FromResult(true);
		}

		public override Task<bool> RemoveAsync(ConferenceFeedback item)
		{
			Settings.LeaveConferenceFeedback(false);
			return Task.FromResult(true);
		}
	}
}
