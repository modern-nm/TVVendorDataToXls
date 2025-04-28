namespace TvVendorDataToXls
{
    public interface IProcessable
    {
        bool ShouldProcess(string propertyName);
        void SelectFields(IEnumerable<string> fields);
    }

    public class Processable : IProcessable
    {
        private HashSet<string> _selectedFields = new();

        public bool ShouldProcess(string propertyName) => _selectedFields.Contains(propertyName);
        public void SelectFields(IEnumerable<string> fields) => _selectedFields = new HashSet<string>(fields);
    }
}
