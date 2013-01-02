using System.Collections.Generic;



// ReSharper disable CheckNamespace
namespace StripTease.Dancers
// ReSharper restore CheckNamespace
{
    public class TagFinder
    {
        private FileReader _fileReader;
        private static long _length;
        private readonly List<string> _tags;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagFinder"/> class.
        /// </summary>
        public TagFinder()
        {    
            _tags = new List<string>();
        }

        /// <summary>
        /// Tagses the specified presious.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        public List<string> Tags(List<string> files )
        {
            foreach (var file in files)
            {
                _fileReader = new FileReader(file);
                _length = _fileReader.Length();
                long currentPosition = 0;
                var trigger = string.Empty;
                var skip = false;
                var inTrigger = false;
                while (_length > currentPosition)
                {
                    var s = _fileReader.ReadString(1);
                    currentPosition++;
                    if (s == "<")
                    {

                        trigger = string.Empty;
                        skip = false;
                        inTrigger = true;
                    }
                    trigger = trigger + s;
                    if (s == " ")
                    {
                        if (inTrigger)
                        {
                            if (!_tags.Contains(trigger))
                            {
                                _tags.Add(trigger);
                            }
                            trigger = string.Empty;
                            skip = true;
                            inTrigger = false;
                        }
                    }
                    if (s == ">")
                    {
                        if (!skip)
                        {
                            if (inTrigger)
                            {
                                if (!_tags.Contains(trigger))
                                {
                                    _tags.Add(trigger);
                                }
                                trigger = string.Empty;
                                inTrigger = false;
                            }
                        }
                    }
                }

            }

            return _tags;
        }





    }
}
