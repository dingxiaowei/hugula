﻿// Copyright (c) 2015 hugula
// direct https://github.com/tenvick/hugula
//
using UnityEngine;
using System.Collections;
using System.IO;
using System;
using SLua;
using System.Text;
using Hugula.Update;

namespace Hugula.Utils
{
    /// <summary>
    /// 文件读取等操作
    /// </summary>
    [SLua.CustomLuaClass]
    public class FileHelper
    {

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="context">Context.</param>
        /// <param name="fileName">File name.</param>
        public static void SavePersistentFile(Array context, string fileName)
        {
            string path = CUtils.GetRealPersistentDataPath() + "/";
            path = path + fileName;
			FileInfo finfo = new FileInfo(path);
			if(!finfo.Directory.Exists)finfo.Directory.Create();

            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.BaseStream.Write((byte[])context, 0, context.Length);
                sw.Flush();
            }

#if UNITY_EDITOR
            Debug.LogFormat("save file path={0},len={1}", path, context.Length);
#endif
        }

        /// <summary>
        /// Saves the persistent file.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="fileName">File name.</param>
        public static void SavePersistentFile(string context, string fileName)
        {
            byte[] cont = LuaHelper.GetBytes(context);
            SavePersistentFile(cont, fileName);
        }

        /// <summary>
        /// Reads the persistent file.
        /// </summary>
        /// <returns>The persistent file.</returns>
        /// <param name="fileName">File name.</param>
        public static byte[] ReadPersistentFile(string fileName)
        {
            string path = CUtils.GetRealPersistentDataPath() + "/" + fileName;
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        /// <summary>
        /// Changes the name of the persistent file.
        /// </summary>
        /// <returns><c>true</c>, if persistent file name was changed, <c>false</c> otherwise.</returns>
        /// <param name="oldName">Old name.</param>
        /// <param name="newname">Newname.</param>
        public static bool ChangePersistentFileName(string oldName, string newname)
        {
            string path = CUtils.GetRealPersistentDataPath() + "/";
            string oldPath = path + oldName;
            string newPath = path + newname;
            if (File.Exists(oldPath))
            {
				FileInfo newFile = new FileInfo(newPath); //如果新的存在需要删除
				if(newFile.Exists) newFile.Delete();

                FileInfo finfo = new FileInfo(oldPath);
                finfo.MoveTo(newPath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes the persistent file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public static void DeletePersistentFile(string fileName)
        {
			string path = Path.Combine (CUtils.GetRealPersistentDataPath (), fileName);
            if (File.Exists(path))
                File.Delete(path);
        }

        /// <summary>
        /// Delete the persistent Directory
        /// </summary>
		public static void DeletePersistentDirectory(string relative=null)
        {
            string path = CUtils.GetRealPersistentDataPath();
			if (!string.IsNullOrEmpty (relative))
				path = Path.Combine (path, relative);
            DirectoryInfo dinfo = new DirectoryInfo(path);
            if (dinfo.Exists)
            {
                var allFiles = dinfo.GetFiles("*", SearchOption.AllDirectories);
                FileInfo fino;
                for (int i = 0; i < allFiles.Length; i++)
                {
                    fino = allFiles[i];
                    fino.Delete();
                };
                dinfo.Delete(true);
            }
        }

		/// <summary>
		/// the Persistents  file is Exists.
		/// </summary>
		/// <returns><c>true</c>, if file was persistented, <c>false</c> otherwise.</returns>
		/// <param name="abpath">Abpath.</param>
		public static bool PersistentFileExists(string abpath)
		{
			string path = Path.Combine(CUtils.GetRealPersistentDataPath(),abpath);
			return File.Exists (path); 
		}

        /// <summary>
        /// Computes the crc32.
        /// </summary>
        /// <returns>The crc32.</returns>
        /// <param name="path">Path.</param>
        public static uint ComputeCrc32(string path)
        {
            if (File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                uint crc = Crc32.Compute(bytes);
                return crc;
            }
            else
                return 0;
        }

        private static LuaFunction callBackFn;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ab"></param>
        /// <param name="luaFn"></param>
        public static void UnpackConfigAssetBundleFn(AssetBundle ab, LuaFunction luaFn)
        {
            callBackFn = luaFn;
#if UNITY_5
            UnityEngine.Object[] all = ab.LoadAllAssets();
#else
        UnityEngine.Object[] all = ab.LoadAll();
#endif
            foreach (UnityEngine.Object i in all)
            {
                if (i is TextAsset)
                {
                    TextAsset a = (TextAsset)i;
                    if (callBackFn != null)
                        callBackFn.call(a.name, a.text);
                }
            }
        }

        /// <summary>
        /// 检测文件路径文件夹是否存在
        /// </summary>
        /// <param name="filePath"></param>
        public static void CheckCreateFilePathDirectory(string filePath)
        {
            FileInfo finfo = new FileInfo(filePath);
            if (!finfo.Directory.Exists) finfo.Directory.Create();
        }
    }
}