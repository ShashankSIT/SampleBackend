using Microsoft.Extensions.Options;
using SampleBackend.Model.Model;

namespace SampleBackend.Service
{
    public abstract class BaseRepository
    {
        public readonly IOptions<DataConfig> _dataConfig;

        public BaseRepository(IOptions<DataConfig> dataConfig)
        {
            _dataConfig = dataConfig;
        }
    }
}
