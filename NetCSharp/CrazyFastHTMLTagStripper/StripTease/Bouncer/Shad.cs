using System.Collections.Generic;

namespace StripTease.Bouncer
{
    public class Shad
    {
      
        private long _length;
        private long _currentPosition;
        /// <summary>
        /// Bounces the specified tags.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <param name="files">The files.</param>
        public void Bounce(List<string> tags, List<string> files)
        {
            foreach (var file in files)
            {

                using ( var fileReader = new FileReader(file))
                {
                    _length = fileReader.Length();
                    var output = string.Empty;
                    var trigger = string.Empty;
                    var bounce = false;
                    var close = false;
                    _currentPosition = 0;
                    while (_length > _currentPosition)
                    {
                        var s = fileReader.ReadString(1);
                        _currentPosition++;
                        if (s == ">")
                            close = true;
                        if (s == "<")
                        {

                            trigger = string.Empty;
                            bounce = false;
                            close = false;

                        }
                        if (!close)
                        {
                            trigger = trigger + s;

                        }
                        if (close)
                        {
                            if (s == ">")
                            {
                                trigger = trigger + s;
                                foreach (var tag in tags)
                                {
                                    if (trigger.Contains(tag))
                                        bounce = true;
                                }
                                if (!bounce)
                                {
                                    output = output + trigger;
                                }
                            }
                            else
                            {
                                output = output + s;
                            }

                        }
                    }
                    fileReader.Close();

                    using (var fileWriter = new FileWriter())
                    {
                        fileWriter.WriteFile(output, file);
                    }


                }
            }
           
            

        }

    }
}
