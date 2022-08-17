using System.IO;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Load.Stub.Rest.dotNet.Configuration
{
    internal class JsonFileMonitor<T> : ICurrentConfig<T>, IDisposable where
        T : class
    {
        private readonly Serilog.ILogger logger;
        private T? _value;
        private readonly FileInfo fileInfo;
        private readonly FileSystemWatcher? watcher;

        public JsonFileMonitor(Serilog.ILogger logger, string path)
        {
            this.logger = logger;
            fileInfo = new FileInfo(path);

            if (fileInfo.DirectoryName == null)
            {
                logger.Information("No path provided for file monitor for {type}. Value will be null.", typeof(T).Name);
                return;
            }

            TryLoadValue();

            watcher = new FileSystemWatcher(fileInfo.DirectoryName, fileInfo.Name);
            watcher.Changed += (e, o) =>
            {
                TryLoadValue();
            };
        }
        public void SetConfig(T value)
        {
            _value = value;
        }

        private void TryLoadValue()
        {
            if (!fileInfo.Exists)
            {
                logger.Warning("File monitor for {type} file not found.", typeof(T).Name);
            }

            try
            {
                var json = File.ReadAllText(fileInfo.FullName);
                _value = System.Text.Json.JsonSerializer.Deserialize<T>(json, new System.Text.Json.JsonSerializerOptions()
                {
                    Converters = { new JsonStringEnumConverter() }

                });
                logger.Information("File monitor for {type} file loaded hash {fileHash}.", typeof(T).Name, json?.GetHashCode());
            }
            catch (Exception exception)
            {
                logger.Error(exception, "File monitor for {type} file failed with exception.", typeof(T).Name);
            }
        }

        public T? Value => _value;

        public void Dispose()
        {
            watcher?.Dispose();
        }

    }
}