using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL.Repositories
{
    public class ImageRepository
    {
        private static readonly string _path;

        static ImageRepository()
        {
            _path = AppDomain.CurrentDomain.BaseDirectory + "\\Images";
        }

        public FileStream GetSlotImage(int id) //CoreCompat.System.Drawing
        {
            string path = _path + $"\\Slots\\{id}";
            return File.Open(path, FileMode.Open);
        }

        public void CreateSlotImage(int id, byte[] image)
        {
            string path = _path + $"\\Slots\\{id}";
            using (FileStream dir = File.Create(path))
            {
                dir.
            }
        }
    }
}
