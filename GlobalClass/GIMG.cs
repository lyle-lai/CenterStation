using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//-----------------------------------------------------------------------
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
//-----------------------------------------------------------------------
namespace GlobalClass
{
    //-----------------------------------------------------------------------
    public class GIMG
    {
        //public static GImage GIMG;
        private struct TP_IMG
        {
            public String Name;
            public Bitmap Img;
        }
        //-----------------------------------------------------------------------
        private static TP_IMG[] Items;
        //-----------------------------------------------------------------------
        public static void Init()
        {
            if (GLB.IsEmpty(Config.ImgPath))
                LoadFromDB();
            else
                LoadFromFile();

        }
        //-----------------------------------------------------------------------
        private static void LoadFromFile()
        {
            String[] fList = Directory.GetFiles(GLB.EXE_DIR + Config.ImgPath, "*.Png");
            Items = new TP_IMG[fList.Length];
            for (int i = 0; i < fList.Length; i++)
            {
                Items[i].Name = Path.GetFileName(fList[i]);
                //Items[i].Img = Bitmap.FromFile(fList[i]);
                Items[i].Img = new Bitmap(fList[i]);
            }
        }
        //-----------------------------------------------------------------------
        private static void LoadFromDB()
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Image");

            Items = new TP_IMG[dr.Count];
            int i = 0;
            while (dr.Read())
            {
                Items[i].Name = dr.GetStr("Name");
                //Items[i].Img = dr.GetImg("Img");
                Items[i].Img = dr.GetBitImg("Img");
                i++;
            }
        }
        //-----------------------------------------------------------------------
        public static Bitmap Get(String ImgName)
        {
            //if (GLB.Same(ImgName, "NurLev2.png")) ImgName = "Gender1.png";
            //if (GLB.Same(ImgName, "NurLev3.png")) ImgName = "Gender1.png";
            //if (GLB.Same(ImgName, "NurLev4.png")) ImgName = "Gender1.png";
            //if (GLB.Same(ImgName, "WorkArea.png")) ImgName = "DosageList.png";
            //if (GLB.Same(ImgName, "PatList.png")) ImgName = "DosageList.png";

            for (int i = 0; i < Items.Length; i++)
            {
                if (!GLB.Same(Items[i].Name, ImgName)) continue;
                return Items[i].Img;
            }
            return null;
        }
        //-----------------------------------------------------------------------
        public static Bitmap GetBitmap(String ImgName)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (!GLB.Same(Items[i].Name, ImgName)) continue;
                return Items[i].Img;
            }
            return null;
        }
        //-----------------------------------------------------------------------
        public static void SaveImgToDB_ALL()
        {
            String[] fList = Directory.GetFiles(GLB.IMG_DIR, "*.Png");
            DBConnect Conn = DBConnect.SYS;
            Conn.BeginTransaction();
            Conn.ExecSQL("Delete From Image");
            for (int i = 0; i < fList.Length; i++)
            {
                //*****************
                String FileName = fList[i];
                String fn = Path.GetFileName(FileName);
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] content = new byte[fs.Length];
                fs.Read(content, 0, (int)fs.Length);
                fs.Close();
                //*****************
                Conn.AddPara("@Name", fn);
                Conn.AddPara("@Img", content);
                Conn.ExecSQL("Insert Into Image (Name, Img) Values (@Name, @Img)");//添加
            }
            //SaveImgToDB(Conn, fList[i]);
            Conn.Commit();
        }
        //-----------------------------------------------------------------------
        private static void SaveImgToDB(DBConnect Conn, String FileName)
        {
            //*****************
            String fn = Path.GetFileName(FileName);
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] content = new byte[fs.Length];
            fs.Read(content, 0, (int)fs.Length);
            fs.Close();
            //*****************
            Conn.AddPara("@Name", fn);
            Conn.AddPara("@Img", content);
            Conn.ExecSQL("Insert Into Image (Name, Img) Values (@Name, @Img)");//添加
        }
        //-----------------------------------------------------------------------

        //-----------------------------------------------------------------------

    }//End of CGImage
    //-----------------------------------------------------------------------
}
