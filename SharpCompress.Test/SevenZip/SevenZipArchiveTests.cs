
using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpCompress.Common;
using SharpCompress.Archive;
using SharpCompress.Reader;

namespace SharpCompress.Test
{
    [TestClass]
    public class SevenZipArchiveTests : ArchiveTests
    {
        [TestMethod]
        public void SevenZipArchive_LZMA_StreamRead()
        {
            ArchiveStreamRead("7Zip.LZMA.7z");
        }

        [TestMethod]
        public void SevenZipArchive_LZMA_PathRead()
        {
            ArchiveFileRead("7Zip.LZMA.7z");
        }

        [TestMethod]
        public void SevenZipArchive_PPMd_StreamRead()
        {
            ArchiveStreamRead("7Zip.PPMd.7z");
        }

        [TestMethod]
        public void SevenZipArchive_PPMd_StreamRead_Extract_All()
        {
            ArchiveStreamReadExtractAll("7Zip.PPMd.7z", CompressionType.PPMd);
        }

        [TestMethod]
        public void SevenZipArchive_PPMd_PathRead()
        {
            ArchiveFileRead("7Zip.PPMd.7z");
        }
        [TestMethod]
        public void SevenZipArchive_LZMA2_StreamRead()
        {
            ArchiveStreamRead("7Zip.LZMA2.7z");
        }

        [TestMethod]
        public void SevenZipArchive_LZMA2_PathRead()
        {
            ArchiveFileRead("7Zip.LZMA2.7z");
        }
        [TestMethod]
        public void SevenZipArchive_BZip2_StreamRead()
        {
            ArchiveStreamRead("7Zip.BZip2.7z");
        }

        [TestMethod]
        public void SevenZipArchive_BZip2_PathRead()
        {
            ArchiveFileRead("7Zip.BZip2.7z");
        }


        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SevenZipArchive_BZip2_Split()
        {
            ArchiveStreamRead("Original.7z.001", "Original.7z.002",
                "Original.7z.003", "Original.7z.004", "Original.7z.005",
                "Original.7z.006", "Original.7z.007");
        }

        [TestMethod]
        public void SevenZipArchive_PartialRead()
        {
            ResetScratch();
            using (Stream stream = File.Open(Path.Combine(TEST_ARCHIVES_PATH, "7Zip.LZMA.7z"), FileMode.Open))
            using (var archive = ArchiveFactory.Open(stream))
            {
                using (IReader reader = archive.ExtractAllEntries())
                {
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            using (var entryStream = reader.OpenEntryStream())
                            {
                                entryStream.ReadByte();
                                entryStream.SkipEntry(true);
                                return;
                            }
                        }
                    }
                }
            }
            Assert.Fail();
        }
    }
}
