using Egnyte.Api;
using Egnyte.Api.Files;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MichalBialecki.com.Egnyte.Examples.Files
{
    [TestFixture]
    class CopyBigFiles
    {
        private const string Domain = "yourdomain";

        private const string Token = "<private token>";

        [Test]
        public async Task UploadBigFile()
        {
            var client = new EgnyteClient(Token, Domain);

            var fileStream = new MemoryStream(File.ReadAllBytes("C:/test/big-file.zip"));
            var response = await ChunkUploadFile(client, "Shared/MikTests/Blog/big-file.zip", fileStream);

            Assert.Pass();
        }

        [Test]
        public async Task DownloadBigFile()
        {
            var client = new EgnyteClient(Token, Domain);

            var responseStream = await client.Files.DownloadFileAsStream("Shared/MikTests/Blog/big-file.zip");

            using (FileStream file = new FileStream("C:/test/big-file01.zip", FileMode.OpenOrCreate, FileAccess.Write))
            {
                CopyStream(responseStream.Data, file);
            }

            Assert.Pass();
        }

    private async Task<UploadedFileMetadata> ChunkUploadFile(
        EgnyteClient client,
        string serverFilePath,
        MemoryStream fileStream)
    {
        // first chunk
        var defaultChunkLength = 10485760;
        var firstChunkLength = defaultChunkLength;
        if (fileStream.Length < firstChunkLength)
        {
            firstChunkLength = (int)fileStream.Length;
        }

        var bytesRead = firstChunkLength;
        var buffer = new byte[firstChunkLength];
        fileStream.Read(buffer, 0, firstChunkLength);

        var response = await client.Files.ChunkedUploadFirstChunk(serverFilePath, new MemoryStream(buffer))
            .ConfigureAwait(false);
        int number = 2;

        while (bytesRead < fileStream.Length)
        {
            var nextChunkLength = defaultChunkLength;
            bool isLastChunk = false;
            if (bytesRead + nextChunkLength >= fileStream.Length)
            {
                nextChunkLength = (int)fileStream.Length - bytesRead;
                isLastChunk = true;
            }

            buffer = new byte[nextChunkLength];
            fileStream.Read(buffer, 0, nextChunkLength);

            if (!isLastChunk)
            {
                await client.Files.ChunkedUploadNextChunk(
                    serverFilePath,
                    number,
                    response.UploadId,
                    new MemoryStream(buffer)).ConfigureAwait(false);
            }
            else
            {
                return await client.Files.ChunkedUploadLastChunk(
                    serverFilePath,
                    number,
                    response.UploadId,
                    new MemoryStream(buffer)).ConfigureAwait(false);
            }
            number++;
            bytesRead += nextChunkLength;
        }

        throw new Exception("Something went wrong - unable to enumerate to next chunk.");
    }

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
