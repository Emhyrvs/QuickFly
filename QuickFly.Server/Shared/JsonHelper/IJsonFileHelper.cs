

namespace QuickFly.Server.Shared.JsonHelper
{
    public interface IJsonFileHelper
    {
        List<T> ReadFromJsonFile<T>();
        void WriteToJsonFile<T>(List<T> data);
    }
}
