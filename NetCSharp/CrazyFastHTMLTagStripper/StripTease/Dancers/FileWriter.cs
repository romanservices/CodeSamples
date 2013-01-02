using System;
using System.Globalization;
using System.IO;
using System.Linq;


// ReSharper disable CheckNamespace
namespace StripTease
// ReSharper restore CheckNamespace
{
   public class FileWriter : IDisposable
   {
       /// <summary>
       /// Writes the file.
       /// </summary>
       /// <param name="output">The output.</param>
       /// <param name="file">The file.</param>
       public void WriteFile(string output, string file)
       {
           var backFile = file + ".StripBack";
           if (File.Exists(backFile))
           {
               var i = Directory.GetFiles(Path.GetDirectoryName(file)).Count(countedFile => countedFile.Contains(backFile));
               i++;
               backFile = string.Format("{0}({1})", backFile, i.ToString(CultureInfo.InvariantCulture));
           }

           File.Move(file, backFile);
           using (var outfile =
           new StreamWriter(file))
           {
               outfile.Write(output);
               outfile.Flush();
               outfile.Close();
           }
       }

       /// <summary>
       /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
       /// </summary>
       public void Dispose()
       {
           //throw new NotImplementedException();
       }
   }
}
