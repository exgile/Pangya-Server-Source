using PangyaAPI.IFF.BinaryModels;
using PangyaAPI.IFF.Common;
using PangyaAPI.IFF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace PangyaAPI.IFF.Collections
{
    public class TikiSpecialTableCollection : List<TikiSpecialTable>
    {
        #region Fields
        IFFHeader IFF_FILE_HEADER;
        #endregion

        public bool Load(MemoryStream data)
        {
            TikiSpecialTable TikiSpecialTable;

            if (data == null || data.Length == 0)
            {
                MessageBox.Show(" data\\TikiSpecialTable.iff is not loaded", "Pangya.IFF");
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

                    var datacount = Tools.IFFTools.SizeStruct(new TikiSpecialTable());
                    if (datacount != recordLength)
                    {
                        throw new Exception($"TikiSpecialTable.iff the structure size is incorrect, Real: {recordLength} TikiSpecialTable.cs: {datacount} ");
                    }

                    for (int i = 0; i < IFF_FILE_HEADER.RecordCount; i++)
                    {
                        TikiSpecialTable = (TikiSpecialTable)Reader.Read(new TikiSpecialTable());

                        this.Add(TikiSpecialTable);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error Struct ", "Pangya.IFF.Model.TikiSpecialTable");
                return false;
            }
        }

        //Destructor
        ~TikiSpecialTableCollection()
        {
            this.Clear();
        }
    }
}
