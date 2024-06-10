namespace webapi.util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

public class UpLoadFileStreamHelper
{
    const int WRITE_FILE_SIZE = 1024 * 1024 * 2;
    
    public static async Task<double> UploadWriteFileAsync(Stream stream, string path)
    {
        try
        {
            double writeCount = 0;
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                       FileShare.ReadWrite, WRITE_FILE_SIZE))
            {
                Byte[] by = new byte[WRITE_FILE_SIZE];

                int readCount = 0;
                while ((readCount = await stream.ReadAsync(by, 0, by.Length)) > 0)
                {
                    await fileStream.WriteAsync(by, 0, readCount);
                    writeCount += readCount;
                }
            }

            return Math.Round((writeCount * 1.0 / (1024 * 1024)), 2);
        }
        catch (Exception ex)
        {
            throw new Exception("发生异常：" + ex.Message);
        }
    }
    
}
