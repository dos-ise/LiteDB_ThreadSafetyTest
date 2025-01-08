using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using Newtonsoft.Json;

namespace LiteDBLib
{
    public class DB
    {

        // NOTE: this code was part of a more complex implementation and can for sure even more simplified but it shows the problem fine
        private class FileData
        {
            public string Path { get; set; }
            public string Key { get; set; }
        }

        private class _fileDataDic
        {
            public string id { get; set; }
            public Dictionary<string, FileData> dic { get; set; }
        }
        public static string Read(string file)
        {
            var db = new LiteDatabase("Async=true;Mode=Read;Filename=" + Path.GetFileNameWithoutExtension(file) + ".dat");
            var col = db.GetCollection<_fileDataDic>("Files");

            // the bug will show up here: 
            // when this functions are accessed the FIRST TIME for a db
            // - it will fail to init the FileData dic proberly
            // - in more or less 50% of all cases it will return NULL instead of the real value
            // - every access beyond the first will return the true value

            var dic = col.FindAll().First(); // in RL we search by ID but FindAll() will trigger the bug too

            string json = JsonConvert.SerializeObject(dic);
            Console.WriteLine(json);

            db.Dispose();

            // this will give visual response 
            return dic.dic.Values.First().Path;
        }

        // used to create the example dbs
        public static void Write(string file)
        {
            var db = new LiteDatabase("Async=true;Mode=Shared;Filename=" + file + ".dat");
            var col = db.GetCollection<_fileDataDic>("Files");

            col.Insert(file, new _fileDataDic
            {
                id = file,
                dic = new Dictionary<string, FileData>
                {
                    { file, new FileData() { Path = file + ".png", Key = file } }
                }
            });

            db.Dispose();
        }
    }
}
