using Domain.Models.ApplicationModels;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;


namespace Infrastructure
{
    public class FileClient : IFileClient
    {
        private readonly string _bucketName;
        private readonly IMinioClient _client;
        public FileClient(IOptions<MinIoOptions> options, IMinioClient client)
        {
            _bucketName = options.Value.Bucket;
            _client = client;
        }
        public async Task<MemoryStream> GetAsync(string name, CancellationToken cancellationToken)
        {
            StatObjectArgs statObjectArgs = new StatObjectArgs().WithBucket(_bucketName).WithObject(name);
            var statArgs = await _client.StatObjectAsync(statObjectArgs);
            MemoryStream stream = new MemoryStream(Convert.ToInt32(statArgs.Size));
            GetObjectArgs args = new GetObjectArgs().WithBucket(_bucketName).WithObject(name).WithCallbackStream(async str =>
            {
                await str.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
            });
            await _client.GetObjectAsync(args, cancellationToken);
            if (stream.Length == 0)
                throw new DoesNotExistException(typeof(File));
            return stream;
        }

        public async Task DeleteAsync(string name, CancellationToken cancellationToken)
        {
            RemoveObjectArgs args = new RemoveObjectArgs().WithBucket(_bucketName).WithObject(name);
            await _client.RemoveObjectAsync(args, cancellationToken);
        }

        public async Task SaveAsync(Stream stream, string name, CancellationToken cancellationToken)
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
            if (!await _client.BucketExistsAsync(bucketExistsArgs))
                await _client.MakeBucketAsync(makeBucketArgs);
            PutObjectArgs args = new PutObjectArgs().WithBucket(_bucketName).WithObject(name).WithStreamData(stream).WithObjectSize(stream.Length).WithContentType("application/octet-stream");
            await _client.PutObjectAsync(args, cancellationToken);
        }
    }
}
