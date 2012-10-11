using System;

namespace SharpCompress.Compressor.Rar
{
    internal class Crypt
    {
        public enum Version
        {
            OLD_DECODE = 0,
            OLD_ENCODE = 1,
            NEW_CRYPT = 2
        };

        public void SetCryptKeys(string password, byte[] salt, bool encrypt, bool oldOnly, bool handsOffHash)
        {
            /*
            if (password == null)
                return;
            
            if (oldOnly)
            {
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(password);
                SetOldKeys(buffer);
                key[0] = 0xD3A3B879;
                key[1] = 0x3F6D12F7;
                key[2] = 0x7515A235;
                key[3] = 0xA4E7F123;

                substTable = (byte[])InitSubstTable.Clone();
                for (int j=0;j<256;j++)
                    for (int i = 0; i < buffer.Length; i += 2)
                    {
                        uint n1 = (byte)RarCRC.CrcTab [((buffer[i]) - j) & 0xff];
                        uint n2 = (byte)RarCRC.CrcTab [((buffer[i + 1]) + j) & 0xff];
                        for (int k = 1; n1 != n2; n1 = (n1 + 1) & 0xff, k++)
                        {
                            byte by = substTable[n1];
                            substTable[n1] = substTable[(n1 + i + k) & 0xff];
                            substTable[(n1 + i + k) & 0xff] = by;
                        }
                    }
                for (int i = 0; i < buffer.Length; i+=16)
                    DoEncryptBlock20(buffer, i);
                return;
            }

            bool Cached=false;
            for (uint I=0;I<ASIZE(Cache);I++)
            if (Cache[I].Password==*Password &&
            (Salt==NULL && !Cache[I].SaltPresent || Salt!=NULL &&
            Cache[I].SaltPresent && memcmp(Cache[I].Salt,Salt,SALT_SIZE)==0) &&
            Cache[I].HandsOffHash==HandsOffHash)
            {
            memcpy(AESKey,Cache[I].AESKey,sizeof(AESKey));
            memcpy(AESInit,Cache[I].AESInit,sizeof(AESInit));
            Cached=true;
            break;
            }

            if (!Cached)
            {
            byte RawPsw[2*MAXPASSWORD+SALT_SIZE];
            WideToRaw(PlainPsw,RawPsw);
            size_t RawLength=2*wcslen(PlainPsw);
            if (Salt!=NULL)
            {
            memcpy(RawPsw+RawLength,Salt,SALT_SIZE);
            RawLength+=SALT_SIZE;
            }
            hash_context c;
            hash_initial(&c);

            const int HashRounds=0x40000;
            for (int I=0;I<HashRounds;I++)
            {
            hash_process( &c, RawPsw, RawLength, HandsOffHash);
            byte PswNum[3];
            PswNum[0]=(byte)I;
            PswNum[1]=(byte)(I>>8);
            PswNum[2]=(byte)(I>>16);
            hash_process( &c, PswNum, 3, HandsOffHash);
            if (I%(HashRounds/16)==0)
            {
            hash_context tempc=c;
            uint32 digest[5];
            hash_final( &tempc, digest, HandsOffHash);
            AESInit[I/(HashRounds/16)]=(byte)digest[4];
            }
            }
            uint32 digest[5];
            hash_final( &c, digest, HandsOffHash);
            for (int I=0;I<4;I++)
            for (int J=0;J<4;J++)
            AESKey[I*4+J]=(byte)(digest[I]>>(J*8));

            Cache[CachePos].Password=*Password;
            if ((Cache[CachePos].SaltPresent=(Salt!=NULL))==true)
            memcpy(Cache[CachePos].Salt,Salt,SALT_SIZE);
            Cache[CachePos].HandsOffHash=HandsOffHash;
            memcpy(Cache[CachePos].AESKey,AESKey,sizeof(AESKey));
            memcpy(Cache[CachePos].AESInit,AESInit,sizeof(AESInit));
            CachePos=(CachePos+1)%ASIZE(Cache);

            cleandata(RawPsw,sizeof(RawPsw));
            }
            rin.init(Encrypt ? Rijndael::Encrypt : Rijndael::Decrypt,AESKey,AESInit);
            cleandata(PlainPsw,sizeof(PlainPsw));
            */
        }

        public void SetAV15Encryption()
        {
        }

        public void SetCmt13Encryption()
        {
        }

        public void DoCrypt(byte[] buffer, int len, Crypt.Version version)
        {
            //throw new NotImplementedException();
        }

        public void DoDecryptBlock(byte[] buffer, int cryptSize)
        {
            //throw new NotImplementedException();
        }

        public void DoDecryptBlock20(byte[] buffer, int i)
        {
            //throw new NotImplementedException();
        }

        byte rol(byte x, int n, int xsize)
        {
            return (byte)(((x) << (n)) | ((x)>>(xsize-(n))));
        }

        byte ror(byte x, int n, int xsize)
        {
            return (byte)(((x) >> (n)) | ((x) << (xsize - (n))));
        }

        void SetOldKeys(byte[] passwordBytes)
        {
            uint passwordCrc = RarCRC.CheckCrc(0xffffffff, passwordBytes, 0, passwordBytes.Length);
            oldKey[0] = (ushort)(passwordCrc & 0xffff);
            oldKey[1] = (ushort)((passwordCrc >> 16) & 0xffff);
            oldKey[2] = oldKey[3] = 0;
            pn1 = pn2 = pn3 = 0;
            foreach (byte by in passwordBytes)
            {
                pn1 += by;
                pn2 ^= by;
                pn3 += by;
                pn3 = (byte) rol(pn3,1,8);
                oldKey[2] ^= (ushort)(by ^ RarCRC.CrcTab[by]);
                oldKey[3] += (ushort)(by + (RarCRC.CrcTab[by] >> 16));
            }
        }

        static byte[] InitSubstTable = new byte[256] {
              215, 19,149, 35, 73,197,192,205,249, 28, 16,119, 48,221,  2, 42,
              232,  1,177,233, 14, 88,219, 25,223,195,244, 90, 87,239,153,137,
              255,199,147, 70, 92, 66,246, 13,216, 40, 62, 29,217,230, 86,  6,
               71, 24,171,196,101,113,218,123, 93, 91,163,178,202, 67, 44,235,
              107,250, 75,234, 49,167,125,211, 83,114,157,144, 32,193,143, 36,
              158,124,247,187, 89,214,141, 47,121,228, 61,130,213,194,174,251,
               97,110, 54,229,115, 57,152, 94,105,243,212, 55,209,245, 63, 11,
              164,200, 31,156, 81,176,227, 21, 76, 99,139,188,127, 17,248, 51,
              207,120,189,210,  8,226, 41, 72,183,203,135,165,166, 60, 98,  7,
              122, 38,155,170, 69,172,252,238, 39,134, 59,128,236, 27,240, 80,
              131,  3, 85,206,145, 79,154,142,159,220,201,133, 74, 64, 20,129,
              224,185,138,103,173,182, 43, 34,254, 82,198,151,231,180, 58, 10,
              118, 26,102, 12, 50,132, 22,191,136,111,162,179, 45,  4,148,108,
              161, 56, 78,126,242,222, 15,175,146, 23, 33,241,181,190, 77,225,
                0, 46,169,186, 68, 95,237, 65, 53,208,253,168,  9, 18,100, 52,
              116,184,160, 96,109, 37, 30,106,140,104,150,  5,204,117,112, 84
            };

        //byte[] substTable;
        uint[] key = new uint[4];
        ushort[] oldKey = new ushort[4];
        byte pn1, pn2, pn3;
    }
//#ifndef _RAR_CRYPT_
//#define _RAR_CRYPT_

//enum { OLD_DECODE=0,OLD_ENCODE=1,NEW_CRYPT=2 };


//struct CryptKeyCacheItem
//{
//#ifndef _SFX_RTL_
//  CryptKeyCacheItem()
//  {
//    Password.Set(L"");
//  }

//  ~CryptKeyCacheItem()
//  {
//    memset(AESKey,0,sizeof(AESKey));
//    memset(AESInit,0,sizeof(AESInit));
//    memset(&Password,0,sizeof(Password));
//  }
//#endif
//  byte AESKey[16],AESInit[16];
//  SecPassword Password;
//  bool SaltPresent;
//  byte Salt[SALT_SIZE];
//  bool HandsOffHash;
//};

//class CryptData
//{
//  private:
//    void Encode13(byte *Data,uint Count);
//    void Decode13(byte *Data,uint Count);
//    void Crypt15(byte *Data,uint Count);
//    void UpdKeys(byte *Buf);
//    void Swap(byte *Ch1,byte *Ch2);
//    void SetOldKeys(const char *Password);

//    Rijndael rin;
    
//    byte SubstTable[256];
//    uint Key[4];
//    ushort OldKey[4];
//    byte PN1,PN2,PN3;

//    byte AESKey[16],AESInit[16];

//    static CryptKeyCacheItem Cache[4];
//    static int CachePos;
//  public:
//    void SetCryptKeys(SecPassword *Password,const byte *Salt,bool Encrypt,bool OldOnly,bool HandsOffHash);
//    void SetAV15Encryption();
//    void SetCmt13Encryption();
//    void EncryptBlock20(byte *Buf);
//    void DecryptBlock20(byte *Buf);
//    void EncryptBlock(byte *Buf,size_t Size);
//    void DecryptBlock(byte *Buf,size_t Size);
//    void Crypt(byte *Data,uint Count,int Method);
//    static void SetSalt(byte *Salt,int SaltSize);
//};

//#endif
}
