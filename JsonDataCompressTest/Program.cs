using JsonDataCompressTest;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

internal class Program
{
    private static void Main(string[] args)
    {
        //1.Create 20萬筆至檔案
        string original = InsertData();
        //2.壓縮
        string base64Str = Compress(original);
        //3.解壓縮
        Decompress(base64Str);
        Console.Read();
    }
    public static string InsertData()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        List<TestModel> msgList = new List<TestModel>();
        for (int i = 0; i < 200000; i++)
        {
            TestModel msg = new TestModel();
            msg.TestMessage01 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage02 = $"這只是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage03 = $"這還個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage04 = $"這一直是個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage05 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage06 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage07 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage08 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage09 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage10 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage11 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage12 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage13 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage14 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage15 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage16 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage17 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage18 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage19 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msg.TestMessage20 = $"這是一個小測試編號{i.ToString().PadLeft(6, '0')}";
            msgList.Add(msg);
        }
        string jsonStr = JsonConvert.SerializeObject(msgList);
        sw.Stop();
        string filePath = GetFilePath(1);
        WriteFile(filePath, jsonStr);
        Console.WriteLine($"產生檔案總比數{msgList.Count}共耗時 {sw.ElapsedMilliseconds} 毫秒");
        return jsonStr;
    }
    #region 壓縮字串BinaryFormmater
    /// <summary>
    /// 壓縮檔案 (BinaryFormatter)
    /// </summary>
    /// <param name="jsonStr"></param>
    /// <returns></returns>
    public static string Compress(string jsonStr)
    {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        MemoryStream zippedStream = new MemoryStream();
        using (GZipStream gzip = new GZipStream(zippedStream, CompressionMode.Compress))
        {
            BinaryFormatter bfmt = new BinaryFormatter();
            bfmt.Serialize(gzip, jsonStr);
            gzip.Flush();
        }

        byte[] zippedBuf = zippedStream.ToArray();
        string base64Str = Convert.ToBase64String(zippedBuf);
        sw.Stop();
        string filePath = GetFilePath(2);
        WriteFile(filePath, base64Str);
        Console.WriteLine($"Gzip壓縮共耗時 : {sw.ElapsedMilliseconds}毫秒");
        return base64Str;
    }
    #endregion
    #region 解壓縮字串(BinaryFormatter)
    /// <summary>
    /// 解壓縮字串
    /// </summary>
    /// <param name="base64Str"></param>
    public static void Decompress(string base64Str)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        string jsonStr;
        byte[] data = Convert.FromBase64String(base64Str);
        MemoryStream zippedStream = new MemoryStream(data);
        using (GZipStream gzip = new GZipStream(zippedStream, CompressionMode.Decompress))
        {
            BinaryFormatter bfmt = new BinaryFormatter();
            jsonStr = (string)bfmt.Deserialize(gzip);
        }
        sw.Stop();
        string filePath = GetFilePath(3);
        WriteFile(filePath, jsonStr);
        Console.WriteLine($"解壓縮共耗時{sw.ElapsedMilliseconds}毫秒");
        Console.ReadKey();
    }
    #endregion
    /// <summary>
    /// 寫入檔案
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileContent"></param>
    public static void WriteFile(string filePath, string fileContent)
    {
        FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
        using (StreamWriter sw = new StreamWriter(fs))
        {
            sw.WriteLine(fileContent);
        }
    }
    /// <summary>
    /// 產生檔案路徑
    /// </summary>
    /// <param name="type">
    /// 1.產出原始檔案路徑
    /// 2.產出壓縮檔案路徑
    /// 3.產出解壓縮檔案路徑
    /// </param>
    /// <returns></returns>
    public static string GetFilePath(int type)
    {
        string filePath = "";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            switch (type)
            {
                case 1:
                    filePath = @"C:\Users\user\Desktop\JsonTest.txt";
                    break;
                case 2:
                    filePath = @"C:\Users\user\Desktop\JsonTest(Compress).txt";
                    break;
                case 3:
                    filePath = @"C:\Users\user\Desktop\JsonTest(Decompress).txt";
                    break;
            }
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            switch (type)
            {
                case 1:
                    filePath = "/home/Test/TestSample/test2/JsonTest.txt";
                    break;
                case 2:
                    filePath = "/home/Test/TestSample/test2/JsonTest(Compress).txt";
                    break;
                case 3:
                    filePath = "/home/Test/TestSample/test2/JsonTest(Decompress).txt";
                    break;
            }
        }
        return filePath;
    }
}