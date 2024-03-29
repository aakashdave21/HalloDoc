using System;
using System.IO;
using System.Text;
namespace HalloDocMVC.Services;
public class FileConverter
{
    public static string FileToBase64(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File '{filePath}' does not exist.");
        }

        // Read the file into a byte array
        byte[] fileBytes = File.ReadAllBytes(filePath);

        // Convert the byte array to a Base64 string
        string base64String = Convert.ToBase64String(fileBytes);

        return base64String;
    }
}