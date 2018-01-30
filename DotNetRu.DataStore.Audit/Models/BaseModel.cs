namespace DotNetRu.DataStore.Audit.Models
{
    using MvvmHelpers;

    public class BaseModel : ObservableObject, IModel
    {
        public string Id { get; set; }
    }
}
