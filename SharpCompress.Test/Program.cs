using System;
using System.IO;
using SharpCompress.Archive;
using SharpCompress.Archive.Zip;
using SharpCompress.Common;
using SharpCompress.Reader;
using SharpCompress.Reader.Zip;

namespace SharpCompress.Test
{
    static class Program
    {
        private const string TEST_BASE_PATH = @"C:\code\sharpcompress";
        private static readonly string TESTARCHIVES_PATH = Path.Combine(TEST_BASE_PATH, "TestArchives");

        static void Main()
        {
            //new RewindableStreamTest().Test();
            //TestGenericZip();
            //TestEncryptedZip();
            //TestZipping();
            //TestZip();
            //TestArchiveZipping();
            //TestRar();
            //TestRar2();
            //TestRar3();
            //TestArchiveZipping();
            //TestGenericBZip2();
            //TestGenericGZip();

            new RarArchiveTests().Rar_Solid_StreamRead_Extract_All();
        }

        public static void TestEncryptedZip()
        {
            TestEncryptedZip("SharpCompress.AES.zip");
            TestEncryptedZip("SharpCompress.Encrypted.zip");
            TestEncryptedZip("SharpCompress.Encrypted2.zip");
        }


        public static void TestEncryptedZip(string zip)
        {
            using (Stream stream = File.OpenRead(Path.Combine(TESTARCHIVES_PATH, zip)))
            {
                var reader = ZipReader.Open(stream, "test");
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        Console.WriteLine(reader.Entry.FilePath);
                        reader.WriteEntryToDirectory(@"C:\temp",
                                                     ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }
            var archive = ZipArchive.Open(Path.Combine(TESTARCHIVES_PATH, zip), "test");
            foreach (var entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    Console.WriteLine(entry.FilePath);
                    entry.WriteToDirectory(@"C:\temp",
                                                 ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                }
            }
        }
    }
}
