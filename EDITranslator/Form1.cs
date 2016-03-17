using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
//using System.Threading;
using System.Text.RegularExpressions;
using System.Configuration;


namespace EDITranslator
{
    public partial class Form1 : Form
    {

        static string FldValue { get; set; }
        static string UniqID { get; set; }
        static string SSN { get; set; }
        static string LName { get; set; }
        static string FName { get; set; }
        static string MidIni { get; set; }
        static string HomePhone { get; set; }
        static string Addr1 { get; set; }
        static string Addr2 { get; set; }
        static string City { get; set; }
        static string State { get; set; }
        static string Zip { get; set; }
        static string Zip4 { get; set; }
        static string DOB { get; set; }
        static string Gender { get; set; }
        static string EffDate { get; set; }
        static string PaidThruDT { get; set; }
        static string TermDate { get; set; }
        static string SSNSub { get; set; }
        static string SSNSub1 { get; set; }
        static string SSNSub2 { get; set; }
        static bool HPS { get; set; }
        static bool PEK { get; set; }
        static bool BOON { get; set; }
        static string InputStream { get; set; }
        static string OutputStream { get; set; }
        static bool bProcessData { get; set; }
        static string Relation { get; set; }
        static string GroupCode { get; set; }
        static int StartingCount { get; set; }
        static int Sequence { get; set; }

        static string CompanyName { get; set; }
        static string PekinDate { get; set; }
        static string PekinMinutes { get; set; }
        static string PekinSeconds { get; set; }
        static string SequenceNo { get; set; }

        int TESTCOUNTER = 0;






        public Form1()
        {
            InitializeComponent();
            chbHPS.AutoCheck = false;
            chbHealthSprings.AutoCheck = false;
            this.lstbxFileToProcess.DragDrop += new
            System.Windows.Forms.DragEventHandler(this.lstbxFileToProcess_DragDrop);
            this.lstbxFileToProcess.DragEnter += new
             System.Windows.Forms.DragEventHandler(this.lstbxFileToProcess_DragEnter);
            //rbHPS.CheckedChanged +=
            //new System.EventHandler(rbHPS_CheckedChanged);
            //rbHealthSprings.CheckedChanged +=
            //new System.EventHandler(rbHealthSprings_CheckedChanged);

        }


        private void lstbxFileToProcess_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;

        }

        private void lstbxFileToProcess_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {

            lblStatus.ForeColor = System.Drawing.Color.Black;
            lblStatus.Text = "Reading file."; // runs on UI thread
            //Thread tDirectoryExists = new Thread(DirectoryExists);
            //tDirectoryExists.Start();
            DirectoryExists();
            //DirectoryExists();
            lstbxFileToProcess.AllowDrop = false;
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            for (i = 0; i < s.Length; i++)
            {


                if (Path.GetExtension(s[i]).ToString().ToUpper() == ".TXT")
                {
                    lstbxFileToProcess.Items.Add(Path.GetFileName(s[i]));
                    Refresh();
                    InputStream = s[i];
                    ReadFile();
                }
                else
                {
                    MessageBox.Show("You must select an EDI file!",
                    "Incorrect File Selection.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                    lstbxFileToProcess.AllowDrop = true;
                }

            }

        }// End of lstbxFileToProcess_DragDrop


        private void ReadFile()
        {
           // bool bProcessData = true;
            string message = "";
            string SegDelimiter = "";

            try
            {
                StreamReader reader;
                reader = new StreamReader(InputStream, Encoding.ASCII);
                //message = reader.ReadLine();
                message = reader.ReadToEnd();
                SegDelimiter = message[105].ToString();

                ProcessData(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        } // End of ReadFile

       
        
        private void ProcessData(string filestring)// bc of threading, parameter has to be object.
        {

           
            try
            {
              //  string message = Message.ToString();
                bProcessData = true;
                PEK = false;


                // Discover the delimiters used. They're always in the same positions 
                char SegDelimiter = filestring[105];
                char ElemDelimiter = filestring[103];

                var segments = from seg in filestring.Split(SegDelimiter).Select(x => x.Trim())
                               where !String.IsNullOrEmpty(seg)
                               select new
                               {
                                   SegID = seg.Substring(0, seg.IndexOf(ElemDelimiter)),
                                   Elements = seg.Split(ElemDelimiter).Skip(1).ToArray()

                               };


                var aList = segments.ToList();

                PekinDate = aList[3].Elements[2].ToString();
                PekinMinutes = aList[3].Elements[3].ToString();
                PekinSeconds = aList[3].Elements[4].ToString();



                CompanyName = aList[5].Elements[1].ToString().Substring(0, 2).ToString().ToUpper();
                if (CompanyName == "D8")
                    CompanyName = aList[1].Elements[1].ToString().Substring(0, 9);
                else
                    CompanyName = aList[5].Elements[1].ToString().Substring(0, 10);


                //   string CompanyName1 = aList[0].Elements[5].ToString().Substring(0, 5);





                if (CompanyName.ToUpper() == "HEALTHSPRI")
                {
                    HPS = false;
                    this.Invoke((MethodInvoker)delegate
                    {
                        chbHealthSprings.Checked = true;
                        lblStatus.ForeColor = System.Drawing.Color.Black;
                        lblStatus.Text = "Processing HealthSpring file."; // runs on UI thread
                    });

                }
                else if (CompanyName.ToUpper() == "HEALTHPLAN")
                {
                    HPS = true;
                    this.Invoke((MethodInvoker)delegate
                    {
                        chbHPS.Checked = true;
                        lblStatus.ForeColor = System.Drawing.Color.Black;
                        lblStatus.Text = "Processing HPS file."; // runs on UI thread
                    });
                }

                else if (CompanyName.ToUpper() == "PEKIN")
                {
                    PEK = true;
                    this.Invoke((MethodInvoker)delegate
                    {
                        chbPekin.Checked = true;
                        lblStatus.ForeColor = System.Drawing.Color.Black;
                        lblStatus.Text = "Processing Pekin file."; // runs on UI thread
                    });
                    CompanyName = "PEKIN";
                }

                else if (CompanyName.ToUpper() == "BOONGROUP")
                {
                    BOON = true;
                    this.Invoke((MethodInvoker)delegate
                    {
                        chbBoon.Checked = true;
                        lblStatus.ForeColor = System.Drawing.Color.Black;
                        lblStatus.Text = "Processing BoonGroup file."; // runs on UI thread
                    });
                    CompanyName = "BOON";
                }





                if (CompanyName.ToUpper() == "HEALTHPLAN")
                    OutputStream = ConfigurationManager.AppSettings["HPSFilePath"] + DateTime.Now.ToString("MMddyyyy") + ".TXT";
                else if (CompanyName.ToUpper() == "HEALTHSPRI")
                    OutputStream = ConfigurationManager.AppSettings["HealthSpringsFilePath"] + DateTime.Now.ToString("MMddyyyy") + ".TXT";
                else if (CompanyName.ToUpper() == "PEKIN")
                    OutputStream = ConfigurationManager.AppSettings["PekinFilePath"] + DateTime.Now.ToString("MMddyyyy") + ".TXT";
                else if (CompanyName.ToUpper() == "BOON")
                    OutputStream = ConfigurationManager.AppSettings["BoonFilePath"] + DateTime.Now.ToString("MMddyyyy") + ".TXT";

                if (File.Exists(OutputStream))
                    File.Delete(OutputStream);

                if (CompanyName == "PEKIN")// Create Pekin Header
                {
                    try
                    {
                        if (File.Exists(OutputStream))
                        {


                            using (StreamWriter sw = File.AppendText(OutputStream))
                            {
                                //string test = ConfigurationManager.recipients["wagner'];
                                sw.WriteLine("PEK" + PekinDate + PekinMinutes + PekinSeconds + "00M26 RPRXPEK/*ALL/*ALL              1 " + ConfigurationManager.AppSettings["bwagner"] + "    0000000000 ");
                                sw.Flush();

                            }

                        }
                        else
                            using (StreamWriter sw = File.CreateText(OutputStream))
                            {

                                sw.WriteLine("PEK" + PekinDate + PekinMinutes + PekinSeconds + "00M26 RPRXPEK/*ALL/*ALL              1 " + ConfigurationManager.AppSettings["bwagner"] + "    0000000000");
                                sw.Flush();


                            }
                    }// end try
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }


                if (CompanyName.ToUpper() == "HEALTHPLAN")
                    StartingCount = 6;
                else if (CompanyName.ToUpper() == "HEALTHSPRI")
                    StartingCount = 7;
                else if (CompanyName.ToUpper() == "PEKIN")
                    StartingCount = 7;
                else if (CompanyName.ToUpper() == "BOON")
                    StartingCount = 8;

                int ULength = 0;
                string sTest = "";
                if (bProcessData)
                {

                    for (int iRCnt = 0; iRCnt < aList.Count; iRCnt++)
                    {
                        TESTCOUNTER += 1;
                        //if (TESTCOUNTER == 2608)
                        //    TESTCOUNTER = 2608;
                        if (iRCnt > StartingCount)
                        {

                            FldValue = "";

                            sTest = aList[iRCnt].SegID.ToString();

                            if (aList[iRCnt].SegID == "REF" && aList[iRCnt].Elements[0] == "0F" && !HPS && !PEK)
                            {

                                ULength = aList[iRCnt].Elements[1].Length;
                                //MessageBox.Show("This is the length " + ULength.ToString());
                                UniqID = aList[iRCnt].Elements[1].Substring(0, ULength);
                                //MessageBox.Show("This is the length " + aList[iRCnt].Elements[1].ToString());
                            }

                            if (aList[iRCnt].SegID == "REF" && aList[iRCnt].Elements[0] == "1L" && PEK)
                            {
                                if (string.IsNullOrEmpty(GroupCode))
                                    GroupCode = aList[iRCnt].Elements[1].Substring(0, 6);
                                //Temporarily hardcode groupcode for testing 
                                // GroupCode = "H15000";
                            }

                            if (aList[iRCnt].SegID == "REF" && aList[iRCnt].Elements[0] == "0F" && PEK)
                            {

                                UniqID = aList[iRCnt].Elements[1].Substring(0, 11);
                            }

                            if (aList[iRCnt].SegID == "REF" && aList[iRCnt].Elements[0] == "1L" && (HPS || BOON))
                            {
                                if (HPS)
                                {
                                    UniqID = aList[iRCnt].Elements[1].Substring(15, 8);
                                    GroupCode = aList[iRCnt].Elements[1].Substring(0, 3);
                                    PrintData();
                                }
                                else// Boon only
                                {
                                    GroupCode = aList[iRCnt].Elements[1].Substring(0, 9);
                                }


                            }
                            if (aList[iRCnt].SegID == "REF" && aList[iRCnt].Elements[0] == "F6")
                            {

                                if (!HPS)
                                {

                                    if (aList[iRCnt].Elements.Length >= 1)
                                        SSN = aList[iRCnt].Elements[1];// Doesn't always have element[1]
                                    else
                                        SSN = "";
                                    SSNSub = SSN.Substring(0, 2);
                                    SSNSub1 = SSN.Substring(0, 1);
                                    SSNSub2 = SSN.Substring(0, 3);


                                    if (SSNSub == "MA" || SSNSub == "WD" || SSNSub == "CA")
                                        SSN = SSN.Substring(2, 9);

                                    if (SSNSub1 == "A")
                                        SSN = SSN.Substring(1, 9);

                                    if (SSNSub == "WA" || SSNSub2 == "WCA")
                                        SSN = "";

                                    if (SSNSub2 == "WCD")
                                        SSN = SSN.Substring(3, 9);

                                }

                            }
                            if (aList[iRCnt].SegID == "NM1" && aList[iRCnt].Elements[0] == "IL")
                            {
                                LName = aList[iRCnt].Elements[2];
                                if (aList[iRCnt].Elements.Length >= 4)
                                    FName = aList[iRCnt].Elements[3];
                                else
                                    FName = "";

                                if (aList[iRCnt].Elements.Length >= 5)
                                    MidIni = aList[iRCnt].Elements[4];// Doesn't always have element[4]
                                else
                                    MidIni = "";

                                if (aList[iRCnt].Elements.Length >= 9)
                                    SSN = aList[iRCnt].Elements[8];// Doesn't always have element[1]
                                else
                                    SSN = "";

                            }
                            if (aList[iRCnt].SegID == "PER" && aList[iRCnt].Elements[0] == "IP")
                            {

                                if (aList[iRCnt].Elements.Length >= 4)
                                    HomePhone = aList[iRCnt].Elements[3];// Doesn't always have element[3]
                                else
                                    HomePhone = "";

                            }
                            if (aList[iRCnt].SegID == "N3")
                            {
                                Addr1 = aList[iRCnt].Elements[0];

                                if (aList[iRCnt].Elements.Length >= 2)
                                    Addr2 = aList[iRCnt].Elements[1];// Doesn't always have element[1]
                                else
                                    Addr2 = "";


                            }

                            if (!HPS)
                            {
                                if (aList[iRCnt].SegID == "N4" && aList[iRCnt].Elements.Length == 6)
                                {
                                    City = aList[iRCnt].Elements[0];// Seg n4
                                    State = aList[iRCnt].Elements[1];// Seg n4
                                    Zip = aList[iRCnt].Elements[2];// Seg n4
                                    Zip4 = aList[iRCnt].Elements[3];// Seg n4'
                                }
                            }

                            if (PEK || BOON)
                            {
                                if (aList[iRCnt].SegID == "N4" && aList[iRCnt].Elements.Length == 3)
                                {
                                    City = aList[iRCnt].Elements[0];// Seg n4
                                    State = aList[iRCnt].Elements[1];// Seg n4
                                    Zip = aList[iRCnt].Elements[2];// Seg n4
                                    //  Zip4 = aList[iRCnt].Elements[3];// Seg n4'
                                }
                            }

                            if (HPS)
                            {
                                if (aList[iRCnt].SegID == "N4")
                                {
                                    City = aList[iRCnt].Elements[0];// Seg n4
                                    if (aList[iRCnt].Elements.Length >= 2)
                                        State = aList[iRCnt].Elements[1];// Seg n4// Doesn't always have element[2]
                                    else
                                        State = "";
                                    if (aList[iRCnt].Elements.Length >= 3)
                                        Zip = aList[iRCnt].Elements[2];// Seg n4// Doesn't always have element[2]
                                    else
                                        Zip = "     ";

                                }
                            }
                            if (aList[iRCnt].SegID == "DMG" && aList[iRCnt].Elements[0] == "D8")
                            {
                                DOB = aList[iRCnt].Elements[1];// Seg DMG
                                if (!PEK)
                                    DOB = FormatDate(DOB);
                                if (aList[iRCnt].Elements.Length >= 3)
                                    Gender = aList[iRCnt].Elements[2];// Doesn't always have element[2]
                                else
                                    Gender = "";
                                //  Gender = aList[iRCnt].Elements[2];// Seg DMG
                            }
                            if (aList[iRCnt].SegID == "DTP" && aList[iRCnt].Elements[0] == "348")
                            {
                                EffDate = aList[iRCnt].Elements[2];// Seg DTP
                                if (!PEK)
                                    EffDate = FormatDate(EffDate);
                                else
                                    EffDate = PekinFormatDate(EffDate);
                            }

                            if (aList[iRCnt].SegID == "DTP" && aList[iRCnt].Elements[0] == "543")
                            {
                                PaidThruDT = aList[iRCnt].Elements[2];// Seg DTP
                                PaidThruDT = FormatDate(PaidThruDT);
                            }

                            if (aList[iRCnt].SegID == "DTP" && aList[iRCnt].Elements[0] == "349")
                            {
                                TermDate = aList[iRCnt].Elements[2];// Seg DTP
                                if (!PEK)
                                    TermDate = FormatDate(TermDate);
                                else
                                    TermDate = PekinFormatDate(TermDate);

                                if (BOON)
                                    StandardPrintData();
                            }

                            if (aList[iRCnt].SegID == "INS")
                            {
                                if (BOON && !String.IsNullOrEmpty(EffDate))
                                    StandardPrintData();


                                Relation = aList[iRCnt].Elements[1];// Relation
                                if (HPS || BOON || CompanyName.ToUpper() == "HEALTHSPRI" || CompanyName.ToUpper() == "HEALTHSPRING")
                                {
                                    if (Relation == "18")
                                    {
                                        SequenceNo = "00";
                                        Sequence = 00;
                                        Relation = "M";
                                    }
                                    else if (Relation == "19")
                                    {
                                        Sequence += 1;
                                        SequenceNo = "0" + Sequence.ToString();
                                        Relation = "C";
                                    }
                                    else if (Relation == "01")
                                    {
                                        Sequence += 1;
                                        SequenceNo = "0" + Sequence.ToString();
                                        Relation = "S";
                                    }
                                    else
                                    {
                                        SequenceNo = "00";
                                        Relation = "O";
                                    }

                                }

                                if (PEK)
                                {
                                    if (Relation == "18")
                                        SequenceNo = "00";
                                    else
                                        SequenceNo = "01";

                                }


                                if (!HPS && !PEK && !BOON)
                                    PrintData();


                            }
                            if (aList[iRCnt].SegID == "N1" && aList[iRCnt].Elements[0] == "TV" && CompanyName == "PEKIN")
                            {
                                PekinPrintData();
                            }

                        }



                    }
                    if (PEK)
                        PekinPrintData();
                    else if (BOON)
                        StandardPrintData();
                    else
                        PrintData();

                    this.Invoke((MethodInvoker)delegate
                    {
                        lblStatus.ForeColor = System.Drawing.Color.DarkGreen;
                        lblStatus.Text = "Process Completed."; // runs on UI thread
                        lstbxFileToProcess.Items.RemoveAt(0);
                        chbHPS.Checked = false;
                        chbHealthSprings.Checked = false;
                        lstbxFileToProcess.AllowDrop = true;
                    });


                }// End of if (bProcessData) statement
            }// end try 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + TESTCOUNTER.ToString());
            }
        } // End of ProcessData

        static string FormatSpaces(string argStringVal, string argPadVal, int argTotalLength)
        {
            string sValue = " ";
            if (argStringVal != null)
                sValue = argStringVal;

            if (sValue.Length > argTotalLength)
            {
                sValue = sValue.Substring(0, sValue.Length - (sValue.Length - argTotalLength));
                return sValue;
            }
            else
            {
                if (argPadVal == "R")
                    sValue = sValue + Spaces(argTotalLength - sValue.Length);

                if (argPadVal == "L")
                    sValue = Spaces(argTotalLength - argStringVal.Length) + sValue;
                
                return sValue;
            }
        }//end of FormatSpaces



        static string FormatHPSZip(string argStringVal, string argPadVal, int argTotalLength)
        {
            //Some HPS zips are greater than five and less than 9 digits
            //This method will only print the 9's and if less than 9 will only print first 5 digits
            string tempZip = "";
            string tempZip4 = "";

            if (argStringVal.Length > 0)
            {

                    if (argStringVal.Length == 5)
                    {
                        return argStringVal + Spaces(4); ;
                    }

                    if (argStringVal.Length == argTotalLength)
                    {
                        tempZip = argStringVal.Substring(0, 5);
                        tempZip4 = argStringVal.Substring(5, 4);

                        if (tempZip4 == "0000")
                            return argStringVal = tempZip + Spaces(4);
                        else
                            return argStringVal = tempZip + tempZip4;
                    }
                    else
                       // return argStringVal.Substring(0, 5) + Spaces(4);
                    return argStringVal + Spaces(9 - argStringVal.Length);


            }
            else
                    return "     ";

        }//end of FormatHPSZip


        static string Spaces(int iNbrSpace)
        {
            string sSpaces = "";
            sSpaces = String.Format("{0," + iNbrSpace + "}", sSpaces);
            return sSpaces;
        }// end of Spaces

        static string FormatDate(string fDate)
        {

            if (!String.IsNullOrEmpty(fDate))
            {
                DateTime myDate = DateTime.ParseExact(fDate, "yyyyMMdd",
                                           System.Globalization.CultureInfo.InvariantCulture);
                return fDate = myDate.ToString("MMddyyyy");
            }
            else
                return "";



        }// end of FormatDate

        static string PekinFormatDate(string fDate)
        {

            if (!String.IsNullOrEmpty(fDate))
            {
            string Century = fDate.Substring(0, 2);
            fDate = fDate.Substring(2, 6);

            if (Century == "19")
                Century = "0";
            else
                Century = "1";
                return Century + fDate;
            }
            else
                return "";



        }// end of PekinFormatDate

        private static void StandardPrintData()
        {// This is the standard Careington File Layout
            //if (BOON)
            //    GroupCode = "BOON-DN11";
            if(String.IsNullOrEmpty(GroupCode))
                GroupCode = "            ";
            object locker = new object();
            FldValue = FormatSpaces("   ", "R", 3);//Title
            FldValue += FormatSpaces(FName.Replace("?", ""), "R", 15);//FName
            FldValue += FormatSpaces(MidIni.Replace("?", ""), "R", 1);// Middle Initial
            FldValue += FormatSpaces(LName.Replace("?", ""), "R", 20);//LName
            FldValue += FormatSpaces("    ", "R", 4);//PostName
            FldValue += FormatSpaces(UniqID.Replace("?", ""), "R", 12);//UniqID
            FldValue += FormatSpaces(SequenceNo, "R", 2);//Sequence Number
            FldValue += FormatSpaces(SSN, "R", 9);//SSN
            FldValue += FormatSpaces(Regex.Replace(Addr1, "[?]", ""), "R", 33);//Addr1  
            FldValue += FormatSpaces(Regex.Replace(Addr2, "[?]", ""), "R", 33);//Addr2
            FldValue += FormatSpaces(Regex.Replace(City, "[?]", ""), "R", 21);//City
            FldValue += FormatSpaces(Regex.Replace(State, "[?]", ""), "R", 2);//State
            FldValue += FormatSpaces(Zip.Replace("?", ""), "R", 9);//Zip
            FldValue += FormatSpaces(HomePhone, "R", 10);//HomePhone
            FldValue += FormatSpaces("          ", "R", 10);//WorkPhone
            FldValue += FormatSpaces("  ", "R", 2);//Coverage
            FldValue += FormatSpaces(GroupCode.Replace("?", ""), "R", 12);// GroupCode
            FldValue += FormatSpaces(TermDate, "R", 8);//TermDate
            FldValue += FormatSpaces(EffDate, "R", 8);//EffDate
            FldValue += FormatSpaces(DOB, "R", 8);//DOB
            FldValue += FormatSpaces(Relation, "R", 1);//Relation
            FldValue += FormatSpaces(" ", "R", 1);//StudentStatus
            FldValue += FormatSpaces("    ", "R", 4);//Plan
            FldValue += FormatSpaces(Gender.Replace("?", ""), "R", 1);//Gender


            try
            {
                if (File.Exists(OutputStream))
                {

                    using (StreamWriter sw = File.AppendText(OutputStream))
                    {
                        if (FName != "" && LName != "")
                            sw.WriteLine(FldValue);
                        sw.Flush();

                    }

                }
                else
                    using (StreamWriter sw = File.CreateText(OutputStream))
                    {
                        if (FName != "" && LName != "")
                            sw.WriteLine(FldValue);
                        sw.Flush();


                    }
            }// end try
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
            FName = "";
            LName = "";
            MidIni = "";
            UniqID = "";
            SSN = "";
            Addr1 = "";
            Addr2 = "";
            City = "";
            State = "";
            Zip = "";
            Zip4 = "";
            HomePhone = "";
            GroupCode = "";
            TermDate = "";
            EffDate = "";
            PaidThruDT = "";
            DOB = "";
            Relation = "";
            Gender = "";

        }// end of StandardPrintData


        private static void PekinPrintData()
        {// Pekin's data is not in the order it needs to print.
         // That's why it has it's own print method.
            string filler = "          000000000000000000000000000            00000000000000                    0000000      H000000100          00000000000000                                        0000000                                                                                         0000000000                                         ";

            int fillerlength = filler.Length;
            
            
            
            if(string.IsNullOrEmpty(TermDate))
            TermDate = "       ";
            object locker = new object();
            //FldValue = FormatSpaces("", "R", 3);//Title
            FldValue += FormatSpaces("PRXPEK", "R", 9);//?
            FldValue += FormatSpaces("006", "R", 15);//?
            FldValue += FormatSpaces(GroupCode.Replace("?", ""), "R", 15);// GroupCode
            FldValue += FormatSpaces(UniqID.Replace("?", ""), "R", 18);//UniqID
            FldValue += FormatSpaces(SequenceNo + " ", "R", 3);//Sequence Number
            FldValue += FormatSpaces("1" + LName.Replace("?", ""), "R", 26);//LName
            FldValue += FormatSpaces(FName.Replace("?", ""), "R", 15);//FName
            FldValue += FormatSpaces(MidIni.Replace("?", ""), "R", 1);// Middle Initial
            FldValue += FormatSpaces(Gender.Replace("?", ""), "R", 1);//Gender
            FldValue += FormatSpaces(DOB + "0", "R", 9);//DOB
            FldValue += FormatSpaces(" 1Y                  A00032773", "R", 30);// No idea what any of this is.
            FldValue += FormatSpaces(Regex.Replace(Addr1, "[?]", ""), "R", 25);//Addr1  
            FldValue += FormatSpaces(Regex.Replace(Addr2, "[?]", ""), "R", 15);//Addr2
            FldValue += FormatSpaces(Regex.Replace(City, "[?]", ""), "R", 20);//City
            FldValue += FormatSpaces(Regex.Replace(State, "[?]", ""), "R", 2);//State
            FldValue += FormatSpaces(Zip.Replace("?", ""), "R", 11);//Zip
            FldValue += FormatSpaces("USA ", "R", 4);//USA?
            FldValue += FormatSpaces("0000000000 ", "R", 11);//Supposed to be home phone.
            FldValue += FormatSpaces("0A00032773         ", "R", 19);//Don't know what this is.
            FldValue += FormatSpaces("00000000000000", "R", 14);//Don't know what this is.
            FldValue += FormatSpaces(EffDate, "R", 7);//EffDate
            FldValue += FormatSpaces(TermDate, "R", 7);//TermDate
            FldValue += FormatSpaces(filler, "R", 323);//TermDate



         //   FldValue += FormatSpaces(SSN.Replace("?", ""), "R", 9);//SSN

            //    FldValue += FormatHPSZip(Zip.Replace("?", ""), "R", 9);//Zip
            //else
            //    FldValue += FormatSpaces(Zip.Replace("?", ""), "R", 5);//Zip
            //if (!HPS && !PEK)
            //    FldValue += FormatSpaces(Zip4.Replace("?", ""), "R", 4);//Zip4
            //FldValue += FormatSpaces(HomePhone, "R", 10);//Homephone
            //FldValue += FormatSpaces("          ", "R", 10);// worksphone
            //FldValue += FormatSpaces("  ", "R", 2);// Coverage
            //if (HPS)
            //    FldValue += FormatSpaces(GroupCode.Replace("?", ""), "R", 12);// GroupCode
            //else
                //FldValue += FormatSpaces("HLTHSPRNG", "R", 12);// GroupCode

            //if (HPS)
            //    FldValue += FormatSpaces(Relation.Replace("?", ""), "R", 1);//Relation
            //else
            //    FldValue += FormatSpaces(" ", "R", 1);//Relation
            //FldValue += FormatSpaces(" ", "R", 1);//StudentStatus
            //if (HPS)
            //    FldValue += FormatSpaces(PaidThruDT, "R", 8);// PaidThruDate for HPS
            //else
            //    FldValue += FormatSpaces("    ", "R", 4);// Plan for Healthsprings


            try
            {
                if (File.Exists(OutputStream))
                {

                    using (StreamWriter sw = File.AppendText(OutputStream))
                    {
                        if (FName != "" && LName != "")
                            sw.WriteLine(FldValue);
                        sw.Flush();

                    }

                }
                else
                    using (StreamWriter sw = File.CreateText(OutputStream))
                    {
                        if (FName != "" && LName != "")
                            sw.WriteLine(FldValue);
                        sw.Flush();


                    }
            }// end try
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
            FName = "";
            LName = "";
            MidIni = "";
            UniqID = "";
            SSN = "";
            Addr1 = "";
            Addr2 = "";
            City = "";
            State = "";
            Zip = "";
            Zip4 = "";
            HomePhone = "";
            GroupCode = "";
            TermDate = "";
            EffDate = "";
            PaidThruDT = "";
            DOB = "";
            Relation = "";
            Gender = "";

        }// end of PekinPrintData

        private static void PrintData()
        {

            object locker = new object();
            FldValue = FormatSpaces("", "R", 3);//Title
            FldValue += FormatSpaces(FName.Replace("?", ""), "R", 15);//FName
            FldValue += FormatSpaces(MidIni.Replace("?", ""), "R", 1);// Middle Initial
            FldValue += FormatSpaces(LName.Replace("?", ""), "R", 20);//LName
            FldValue += FormatSpaces("", "R", 4);// Post Name
            FldValue += FormatSpaces(UniqID.Replace("?", ""), "R", 12);//UniqID
            //FldValue += FormatSpaces("  ", "R", 2);//Sequence Number
            FldValue += FormatSpaces(SequenceNo.Replace("?", "00"), "R", 2);//Sequence Number Healthsprings doesn't have deps.
            FldValue += FormatSpaces(SSN.Replace("?", ""), "R", 9);//SSN
            FldValue += FormatSpaces(Regex.Replace(Addr1, "[?]", ""), "R", 33);//Addr1  I can add more special chars, no commas between.
            FldValue += FormatSpaces(Regex.Replace(Addr2, "[?]",  ""), "R", 33);//Addr2
            FldValue += FormatSpaces(Regex.Replace(City, "[?]", ""), "R", 21);//City
            FldValue += FormatSpaces(Regex.Replace(State, "[?]", ""), "R", 2);//State
            if (HPS)
                FldValue += FormatHPSZip(Zip.Replace("?", ""), "R", 9);//Zip
            else
                FldValue += FormatSpaces(Zip.Replace("?", ""), "R", 5);//Zip
            if (!HPS && !PEK)
                FldValue += FormatSpaces(Zip4.Replace("?", ""), "R", 4);//Zip4
            FldValue += FormatSpaces(HomePhone, "R", 10);//Homephone
            FldValue += FormatSpaces("          ", "R", 10);// worksphone
            FldValue += FormatSpaces("  ", "R", 2);// Coverage
            if (HPS)
                FldValue += FormatSpaces(GroupCode.Replace("?", ""), "R", 12);// GroupCode
            else
                FldValue += FormatSpaces("HLTHSPRNG", "R", 12);// GroupCode
            FldValue += FormatSpaces(TermDate, "R", 8);//TermDate
            FldValue += FormatSpaces(EffDate, "R", 8);//EffDate
            FldValue += FormatSpaces(DOB, "R", 8);//DOB
            if (HPS)
                FldValue += FormatSpaces(Relation.Replace("?", ""), "R", 1);//Relation
            else
                FldValue += FormatSpaces(" ", "R", 1);//Relation
            FldValue += FormatSpaces(" ", "R", 1);//StudentStatus
            if (HPS)
                FldValue += FormatSpaces(PaidThruDT, "R", 8);// PaidThruDate for HPS
            else
                FldValue += FormatSpaces("    ", "R", 4);// Plan for Healthsprings
            FldValue += FormatSpaces(Gender.Replace("?", ""), "R", 1);//Gender

            try
            {
                if (File.Exists(OutputStream))
                {


                    using (StreamWriter sw = File.AppendText(OutputStream))
                    {
                        if (LName != "")
                            sw.WriteLine(FldValue);
                        sw.Flush();

                    }

                }
                else
                    using (StreamWriter sw = File.CreateText(OutputStream))
                    {
                        if (LName != "")
                            sw.WriteLine(FldValue);
                        sw.Flush();


                    }
            }// end try
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
            FName = "";
            LName = "";
            MidIni = "";
            UniqID = "";
            SSN = "";
            Addr1 = "";
            Addr2 = "";
            City = "";
            State = "";
            Zip = "     ";
            Zip4 = "    ";
            HomePhone = "";
            GroupCode = "";
            TermDate = "";
            EffDate = "";
            PaidThruDT = "";
            DOB = "";
            Relation = "";
            Gender = "";

        }// end of PrintData

        private static void DirectoryExists()
        {
            string MainPath = @"C:\Carelynx";
            string SubPathHealthSprings = "\\HEALTHSPRINGS";
            string SubPathHPS = "\\HPS";
            string SubPathPekin = "\\Pekin";



            if(!Directory.Exists(MainPath)) 
            {
                System.IO.Directory.CreateDirectory(MainPath);  
            }
            MainPath += "\\MbrEligIN";
            if (!Directory.Exists(MainPath))
            {
                System.IO.Directory.CreateDirectory(MainPath);
            }
            MainPath += "\\MbrEligINALL";
            if (!Directory.Exists(MainPath))
            {
                System.IO.Directory.CreateDirectory(MainPath);
            }
            if (!Directory.Exists(MainPath + SubPathHealthSprings))
            {
                System.IO.Directory.CreateDirectory(MainPath + SubPathHealthSprings);
            }
            if (!Directory.Exists(MainPath + SubPathHPS))
            {
                System.IO.Directory.CreateDirectory(MainPath + SubPathHPS);
            }
            if (!Directory.Exists(MainPath + SubPathPekin))
            {
                System.IO.Directory.CreateDirectory(MainPath + SubPathPekin);
            }


        }// End of DirectoryExists method


    }// end of class form1

}// end of namespace EDITranslator
