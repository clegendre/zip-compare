using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO.Compression;
using System.IO;
using Ionic.Zip;

namespace ZipTests
{
    [TestFixture]
    public class ZipTests
    {
        [Test]
        public void ApiDiff()
        {

            using (FileStream zipArchiveOutputStream = new FileStream(@"C:\Users\cedri_000\Pictures\Popims\Extract\Zip.zip", FileMode.Create, FileAccess.Write))
            {
                using (ZipArchive z = new ZipArchive(zipArchiveOutputStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    foreach (var s in Directory.GetFiles(@"C:\Users\cedri_000\Pictures\Popims\Images"))
                    {
                        FileInfo f = new FileInfo(s);

                        var e = z.CreateEntry(f.Name, CompressionLevel.Optimal);

                        using (var fileStream = f.OpenRead())
                        {
                            using (var entryStream = e.Open())
                            {
                                // Read the source file into a byte array.
                                byte[] bytes = new byte[fileStream.Length];
                                int numBytesToRead = (int)fileStream.Length;
                                int numBytesRead = 0;
                                while (numBytesToRead > 0)
                                {
                                    // Read may return anything from 0 to numBytesToRead.
                                    int n = fileStream.Read(bytes, numBytesRead, numBytesToRead);

                                    // Break when the end of the file is reached.
                                    if (n == 0)
                                        break;

                                    numBytesRead += n;
                                    numBytesToRead -= n;
                                }
                                numBytesToRead = bytes.Length;

                                // Write the byte array to the other FileStream.
                                entryStream.Write(bytes, 0, numBytesToRead);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void ZipIonicDirectory()
        {

            using (Ionic.Zip.ZipFile z = new ZipFile(@"C:\Users\cedri_000\Pictures\Popims\Extract\Zip2.zip"))
            {
                z.AddDirectory(@"C:\Users\cedri_000\Pictures\Popims\Images");
                z.Save();
            }


        }

         [Test]
        public void ZipIonic()
        {
            using (Ionic.Zip.ZipFile z = new ZipFile(@"C:\Users\cedri_000\Pictures\Popims\Extract\Zip3.zip"))
            {
                foreach (var s in Directory.GetFiles(@"C:\Users\cedri_000\Pictures\Popims\Images"))
                {
                    z.AddEntry(Path.GetFileName(s), File.ReadAllBytes(s));
                    z.Save();
                }
            }
    }
    }
}
