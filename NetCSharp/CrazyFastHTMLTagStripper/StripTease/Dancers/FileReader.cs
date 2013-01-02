using System;
using System.Text;

// ReSharper disable CheckNamespace
namespace StripTease
// ReSharper restore CheckNamespace
{
   public class FileReader : IDisposable
   {
        private System.IO.BinaryReader _br;
        private System.IO.FileStream _temp;
        private readonly string _mFilename = "";
        /// <summary>
        /// Initializes a new instance of the <see cref="FileReader"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public FileReader(string filename)
        {
            _mFilename = filename;
            _temp = System.IO.File.Open(_mFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            _br = new System.IO.BinaryReader(_temp, Encoding.Default);
        }
        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            _br.Close();
            _br = null;
            _temp.Close();
            _temp = null;
        }
        /// <summary>
        /// Lengthes this instance.
        /// </summary>
        /// <returns></returns>
        public long Length()
        {
            return _br.BaseStream.Length;
        }
        /// <summary>
        /// Rewinds the specified number.
        /// </summary>
        /// <param name="number">The number.</param>
        public void Rewind(int number)
        {
            _br.BaseStream.Position -= number;
        }
        /// <summary>
        /// Positions this instance.
        /// </summary>
        /// <returns></returns>
        public long Position()
        {
            return _br.BaseStream.Position;
        }
        /// <summary>
        /// Byteses the left.
        /// </summary>
        /// <returns></returns>
        public long BytesLeft()
        {
            return (_br.BaseStream.Length - _br.BaseStream.Position) + 1; //length starts at 1, position starts at 0
        }

       /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <returns></returns>
        public string ReadString(int num)
        {
            var chars = _br.ReadChars(num);
            return new string(chars);
        }

       /// <summary>
        /// Reads the single.
        /// </summary>
        /// <returns></returns>
        public Single ReadSingle()
        { // 4 bytes
            return _br.ReadSingle();
        }
        /// <summary>
        /// Advances the specified num.
        /// </summary>
        /// <param name="num">The num.</param>
        public void Advance(int num)
        { //advance stream X bytes
            _br.ReadBytes(num);
        }
        /// <summary>
        /// Reads the byte.
        /// </summary>
        /// <returns></returns>
        public Byte ReadByte()
        { // 1byte
            return _br.ReadByte();
        }
        /// <summary>
        /// Reads the int.
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        { // 2 bytes
            return _br.ReadInt16();
        }
        /// <summary>
        /// Reads the double.
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            return _br.ReadDouble();
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
