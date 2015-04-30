using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Drawing;

namespace Lab4_Server
{
    public class DataBaseObject : MarshalByRefObject, IDatabaseObject
    {
        public string QueryResult(string lastName)
        {
            const string connectingString = @"provider=Microsoft.Jet.OLEDB.4.0;data source=..\mydb.mdb";
            var myConn = new OleDbConnection(connectingString);
            const string selectString = "SELECT stud1.Group, stud1.Estimate FROM stud1 WHERE stud1.LastName = @Lastname;";
            OleDbCommand myCommand = myConn.CreateCommand();
            myCommand.CommandText = selectString;
            myCommand.Parameters.AddWithValue("@Lastname", lastName);
            var oda = new OleDbDataAdapter { SelectCommand = myCommand };
            var myDataset = new DataSet();
            if (myConn.State != ConnectionState.Open)
            {
                myConn.Open();
            }
            const string dataTableName = "studA";
            oda.Fill(myDataset, dataTableName);
            var myDataTable = myDataset.Tables[dataTableName];
            var str = "";
            foreach (DataRow dr in myDataTable.Rows)
            {
                var str1 = String.Format("\nGroup: {0}, Estimate: {1}", dr["group"], dr["estimate"]);
                str += str1;
            }
            return str;
        }
    }

    public class ImageViewer : MarshalByRefObject, IImageViewer
    {
        public byte[] GetFile(string name)
        {
            string[] filePaths = Directory.GetFiles(@"..\Images");
            return (from filePath in filePaths where Path.GetFileNameWithoutExtension(filePath).ToLower() == name.ToLower() select File.ReadAllBytes(filePath)).FirstOrDefault();
        }
    }

    class Program
    {
        //public byte[] imageToByteArray(Image imageIn)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    imageIn.Save(ms, Imaging.ImageFormat.Gif);
        //    return ms.ToArray();
        //}
        //public Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;
        //}
        static void Main(string[] args)
        {
            var channel = new HttpChannel(54321);
            ChannelServices.RegisterChannel(channel);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(DataBaseObject),
                "SOEndPoint",
                WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(ImageViewer),
                "ImageViewer",
                WellKnownObjectMode.Singleton);
            Console.WriteLine("Press To Stop Server");
            Console.ReadLine();
        }

    }
}
