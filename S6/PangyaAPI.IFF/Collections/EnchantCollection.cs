﻿using PangyaAPI.IFF.BinaryModels;
using PangyaAPI.IFF.Common;
using PangyaAPI.IFF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace PangyaAPI.IFF.Collections
{
    public class EnchantCollection : List<Enchant>
    {
        #region Fields
        IFFHeader IFF_FILE_HEADER;
        public bool Update { get; set; }
        #endregion

        public bool Load(MemoryStream data)
        {
            Enchant Enchant;

            if (data == null || data.Length == 0)
            {
                MessageBox.Show(" data\\Enchant.iff is not loaded", "Pangya.IFF");
                return false;
            }

            try
            {
                using (var Reader = new PangyaBinaryReader(data))
                {
                    if (new string(Reader.ReadChars(2)) == "PK")
                    {
                        throw new Exception("The given IFF file is a ZIP file, please unpack it before attempting to parse it");
                    }
                    Reader.Seek(0, 0);

                    IFF_FILE_HEADER = (IFFHeader)Reader.Read(new IFFHeader());

                    long recordLength = (Reader.GetSize() - 8L) / IFF_FILE_HEADER.RecordCount;

                    var datacount = Tools.IFFTools.SizeStruct(new Enchant());
                    if (datacount != recordLength)
                    {
                        throw new Exception($"Enchant.iff the structure size is incorrect, Real: {recordLength} Enchant.cs: {datacount} ");
                    }

                    for (int i = 0; i < IFF_FILE_HEADER.RecordCount; i++)
                    {
                        Enchant = (Enchant)Reader.Read(new Enchant());

                        this.Add(Enchant);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error Struct ", "Pangya.IFF.Model.Enchant");
                return false;
            }
        }

        //Destructor
        ~EnchantCollection()
        {
            this.Clear();
        }

    }
}
