using System;
using System.IO;

namespace SharpCompress.Compressor.Rar
{
    internal class ComprDataIO : Stream
    {
        Stream inner;
        int encryption;
        int decryption;

        Crypt crypt = new Crypt();
        Crypt decrypt = new Crypt();

        public ComprDataIO(Stream inner)
        {
            this.inner = inner;
        }

        public override bool CanRead
        {
            get { return inner.CanRead; }
        }

        public override bool CanSeek
        {
            get { return inner.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return inner.CanWrite; }
        }

        public override void Flush()
        {
            inner.Flush();
        }

        public override long Length
        {
            get { return inner.Length; }
        }

        public override long Position
        {
            get
            {
                return inner.Position;
            }
            set
            {
                inner.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int retCode = inner.Read(buffer, offset, count);

            if (retCode > 0)
            {
                if (decryption > 0)
                {
                    if (decryption < 20)
                        decrypt.DoCrypt(buffer, retCode, (decryption==15) ? Crypt.Version.NEW_CRYPT : Crypt.Version.OLD_DECODE);
                    else
                        if (decryption==20)
                            for (int i = 0; i < retCode; i += 16)
                                decrypt.DoDecryptBlock20(buffer, i);
                        else
                        {
                            int cryptSize = retCode < 16 ? retCode : (retCode / 16 + 1) * 16;
                            decrypt.DoDecryptBlock(buffer, cryptSize);
                        }
                }
            }

            return retCode;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            inner.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            inner.Write(buffer, offset, count);

            //        if (!testMode) {
            //            // DestFile->Write(Addr,Count);
            //            outputStream.write(addr, offset, count);
            //        }

            //        curUnpWrite += count;

            //        if (!skipUnpCRC) {
            //            if (archive.isOldFormat()) {
            //                unpFileCRC = RarCRC
            //                        .checkOldCrc((short) unpFileCRC, addr, count);
            //            } else {
            //                unpFileCRC = RarCRC.checkCrc((int) unpFileCRC, addr, offset,
            //                        count);
            //            }
            //        }
            //        // if (!skipArcCRC) {
            //        // archive.updateDataCRC(Addr, offset, ReadSize);
            //        // }
        }

        public void SetEncryption(int method, string password, byte[] salt, bool encrypt, bool handsOffHash)
        {
          if (encrypt)
          {
            encryption = password != null ? method : 0;
            crypt.SetCryptKeys(password, salt, encrypt, false, handsOffHash);
          }
          else
          {
            decryption = password != null ? method : 0;
            decrypt.SetCryptKeys(password, salt, encrypt, method<29, handsOffHash);
          }
        }


        void SetAV15Encryption()
        {
          decryption=15;
          decrypt.SetAV15Encryption();
        }

        void SetCmt13Encryption()
        {
          decryption=13;
          decrypt.SetCmt13Encryption();
        }
    }
}
