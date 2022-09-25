using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public static class RoomCodeGenerator
{
    /// <summary>
    /// A UTF-16 Letter is sized (max) 4 bytes
    /// </summary>
    private const uint SIZE_OF_LETTER = 4;
    /// <summary>
    /// A Room-Code is by default 4 UTF-16 Characters Long
    /// </summary>
    public static uint LENGTH_OF_CODE = 4;

    public static string GetRoomCode()
    {
        byte[] charBuff = null;
        using (MemoryStream ms = new MemoryStream(Convert.ToInt32(LENGTH_OF_CODE * SIZE_OF_LETTER)))
        {
            using (StreamWriter sw = new StreamWriter(ms, new System.Text.UTF8Encoding(false, false)))
            {
                for (byte i = 0; i < LENGTH_OF_CODE; ++i)
                {
                    int nominator = RandomNumberGenerator.GetInt32(0, 200);
                    ReadOnlySpan<byte> b = null;
                    if ((nominator % 5 == 0) || (nominator % 10 == 0))
                    {
                        int v = RandomNumberGenerator.GetInt32(48, 57);
                        b = new ReadOnlySpan<byte>(BitConverter.GetBytes(v));
                    }
                    else
                    {
                        int l = RandomNumberGenerator.GetInt32(97, 122);
                        b = new ReadOnlySpan<byte>(BitConverter.GetBytes(l));
                    }

                    sw.Write((char)BitConverter.ToChar(b));
                }
            }

            charBuff = ms.ToArray();
        }

        return System.Text.Encoding.UTF8.GetString(charBuff);
    }
}
