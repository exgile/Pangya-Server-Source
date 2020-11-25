using PangyaAPI.IFF.BinaryModels;
using PangyaAPI.IFF.Common;
using PangyaAPI.IFF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace PangyaAPI.IFF.Collections
{
    public class PartCollection : List<Part>
    {
        #region Fields
        IFFHeader IFF_FILE_HEADER;
        public bool Update { get; set; }
        #endregion
        public bool Load(MemoryStream data)
        {
            Part Part;

            if (data == null || data.Length == 0)
            {
                MessageBox.Show(" data\\Part.iff is not loaded", "Pangya.IFF");
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

                    var datacount = Tools.IFFTools.SizeStruct(new Part());
                    if (datacount != recordLength)
                    {
                        throw new Exception($"Part.iff the structure size is incorrect, Real: {recordLength} Part.cs: {datacount} ");
                    }

                    for (int i = 0; i < IFF_FILE_HEADER.RecordCount; i++)
                    {
                        Part = (Part)Reader.Read(new Part());

                        this.Add(Part);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error Struct ", "Pangya.IFF.Model.Part");
                return false;
            }
        }

        //Destructor
        ~PartCollection()
        {
            this.Clear();
        }

        internal Part GetItem(uint TypeID)
        {
            foreach (var part in this)
            {
                if (part.Base.TypeID == TypeID)
                {
                    return part;
                }
            }
            return new Part();
        }

        public string GetItemName(UInt32 ID)
        {
            foreach (var item in this)
            {
                if (item.Base.TypeID == ID)
                {
                    return item.Base.Name;
                }
            }
            return "";
        }
    }
}
