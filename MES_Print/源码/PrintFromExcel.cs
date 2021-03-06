﻿using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using Print.Message.Bll;
using System.Drawing.Printing;
using Seagull.BarTender.Print;
using System.Text.RegularExpressions;
using ExcelPrint.Param.Bll;
using System.IO;
using Print_Message;
using System.Media;

namespace WindowsForms_print
{
    public partial class PrintFromExcel : Form
    {
        InputExcelBLL IEB = new InputExcelBLL();
        ManuExcelPrintParamBLL MEPPB = new ManuExcelPrintParamBLL();
        List<ManuExcelPrintParam> mepp = new List<ManuExcelPrintParam>();
        Engine btEngine = new Engine();
        LabelFormatDocument btFormat;

        SoundPlayer player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "失败.wav");

        //记录模板刷新次数
        int RefreshNum = 0;

        //打印参数
        int TN =1;
        string lj = "";

        public PrintFromExcel()
        {
            InitializeComponent();
            int wid = Screen.PrimaryScreen.WorkingArea.Width;
            this.Width = wid;
        }

        static bool IsNumeric(string s)
        {
            double v;
            if (double.TryParse(s, out v))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PrintFromExcel_Load(object sender, EventArgs e)
        {
            PrintDocument print = new PrintDocument();
            string sDefault = print.PrinterSettings.PrinterName;//默认打印机名
            this.Printer.Text = sDefault;
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                Printer.Items.Add(sPrint);
            }
            btEngine.Start();
        }

        //选择模板
        private void Open_Template_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string path = ofd.FileName;
            string strExtension = "";
            int nIndex = path.LastIndexOf('.');
            if (nIndex > 0)
            {
                strExtension = path.Substring(nIndex);
                if (strExtension != ".btw")
                {
                    player.Play();
                    this.remined.AppendText("请选择正确的btw文件！\r\n");
                }
                else
                {
                    this.Select_Template.Text = path;
                    lj = path;
                }
            }
        }

        //导入Excel文件
        private void Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            string path = dialog.FileName;
            if (path != "")
            {
                string strExtension = path.Substring(path.LastIndexOf('.'));
                if (strExtension != ".xls" && strExtension != ".xlsx")
                {
                    player.Play();
                    this.remined.AppendText("请选择xls文件!\r\n");
                }
                else
                {
                    this.dataGridView1.DataSource = "";
                    this.ImportPath.Text = path;
                    DataTable dt = IEB.GetExcelDatatable(path,strExtension);
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].Width = 200;
                    dataGridView1.Columns[1].Width = 200;
                    dataGridView1.Columns[2].Width = 200;
                    dataGridView1.Columns[3].Width = 200;
                    dataGridView1.Columns[4].Width = 200;
                }
            }
        }

        //判断行数的输入格式是否为“数字-数字”
        public static bool IsTrue(string inString)
        {
            Regex regex = new Regex("^\\d+\\-+\\d");
            return regex.IsMatch(inString.Trim());
        }

        //鼠标离开模板打印份数框时触发的函数
        private void TemplateNum_Leave(object sender, EventArgs e)
        {
            if (this.TemplateNum.Text == "")
            {
                this.TemplateNum.Text = 1.ToString();
            }
            else
            {
                if (IsNumeric(this.TemplateNum.Text))
                {
                    TN = int.Parse(this.TemplateNum.Text);
                }
                else
                {
                    player.Play();
                    this.remined.AppendText("请输入数字\r\n");
                    this.TemplateNum.Text = 1.ToString();
                    this.TemplateNum.Focus();
                }
            }
        }

        //输入行数后按下Ecter键触发的函数
        private void RowNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if(this.ImportPath.Text == "")
                {
                    player.Play();
                    this.remined.AppendText("请先导入Excel\r\n");
                    this.RowNumber.Clear();
                    this.RowNumber.Focus();
                    return;
                }
                if (this.RowNumber.Text != "")
                {
                    int i = 1;
                    this.dataGridView1.DataSource = "";
                    string strExtension = this.ImportPath.Text.Substring(this.ImportPath.Text.LastIndexOf('.'));
                    DataTable dt = IEB.GetExcelDatatable(this.ImportPath.Text,strExtension);
                    DataRow dr2 = dt.Rows[0];
                    DataTable dd = new DataTable();
                    dd.Columns.Add(" ");
                    dd.Columns.Add(new DataColumn(dr2[0].ToString(), typeof(string)));
                    dd.Columns.Add(new DataColumn(dr2[1].ToString(), typeof(string)));
                    dd.Columns.Add(new DataColumn(dr2[2].ToString(), typeof(string)));
                    dd.Columns.Add(new DataColumn(dr2[3].ToString(), typeof(string)));
                    dd.Columns.Add(new DataColumn(dr2[4].ToString(), typeof(string)));
                    if (IsNumeric(this.RowNumber.Text))
                    {
                        DataRow dr3 = dt.Rows[int.Parse(this.RowNumber.Text)];
                        if (dr3[0].ToString() == "")
                        {
                            player.Play();
                            this.remined.AppendText("您输入的行数大于Excel行数\r\n");
                            this.RowNumber.Clear();
                            this.RowNumber.Focus();
                            return;
                        }
                        dd.Rows.Add(1, dr3[0].ToString(), dr3[1].ToString(), dr3[2].ToString(), dr3[3].ToString(), dr3[4].ToString());
                        dataGridView1.DataSource = dd;
                        dataGridView1.Columns[0].Width = 50;
                        dataGridView1.Columns[1].Width = 200;
                        dataGridView1.Columns[2].Width = 200;
                        dataGridView1.Columns[3].Width = 200;
                        dataGridView1.Columns[4].Width = 200;
                        dataGridView1.Columns[5].Width = 200;
                    }
                    else
                    {
                        if (IsTrue(this.RowNumber.Text))
                        {
                            string[] range = this.RowNumber.Text.Split('-');
                            if(int.Parse(range[0])> int.Parse(range[1]))
                            {
                                player.Play();
                                this.remined.AppendText("起始位不能大于终止位\r\n");
                                this.RowNumber.Clear();
                                this.RowNumber.Focus();
                                return;
                            }
                            for (int j = int.Parse(range[0]); j <= int.Parse(range[1]); j++)
                            {
                                DataRow dr3 = dt.Rows[j];
                                dd.Rows.Add(i, dr3[0].ToString(), dr3[1].ToString(), dr3[2].ToString(), dr3[3].ToString(), dr3[4].ToString());
                                dataGridView1.DataSource = dd;
                                i++;
                            }
                            dataGridView1.Columns[0].Width = 50;
                            dataGridView1.Columns[1].Width = 200;
                            dataGridView1.Columns[2].Width = 200;
                            dataGridView1.Columns[3].Width = 200;
                            dataGridView1.Columns[4].Width = 200;
                            dataGridView1.Columns[5].Width = 200;
                        }
                        else
                        {
                            player.Play();
                            this.remined.AppendText("请输入'数字-数字'格式\r\n");
                            this.RowNumber.Clear();
                            this.RowNumber.Focus();
                        }
                    }
                }
            }
        }

        //打印按钮函数
        private void ExcelToPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ImportPath.Text == "")
                {
                    player.Play();
                    this.remined.AppendText("请先导入Excel\r\n");
                    return;
                }
                if (this.Select_Template.Text != "")
                {
                    if (IsNumeric(this.RowNumber.Text))
                    {
                        string strExtension = this.ImportPath.Text.Substring(this.ImportPath.Text.LastIndexOf('.'));
                        DataTable dt = IEB.GetExcelDatatable(this.ImportPath.Text, strExtension);
                        DataRow dr3 = dt.Rows[int.Parse(this.RowNumber.Text)];
                        if (!MEPPB.CheckIMEIBLL(dr3[0].ToString(), dr3[1].ToString()))
                        {
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer.Text;
                            //对模板相应字段进行赋值
                            btFormat.SubStrings["IMEI1"].Value = dr3[0].ToString();
                            btFormat.SubStrings["IMEI2"].Value = dr3[1].ToString();
                            btFormat.SubStrings["IMEI3"].Value = dr3[2].ToString();
                            btFormat.SubStrings["IMEI4"].Value = dr3[3].ToString();
                            btFormat.SubStrings["IMEI5"].Value = dr3[4].ToString();
                            btFormat.SubStrings["ProductDate"].Value = DateTime.Now.ToString("yyyy.MM.dd");
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = TN;
                            //记录数据
                            mepp.Add(new ManuExcelPrintParam()
                            {
                                IMEI1 = dr3[0].ToString(),
                                IMEI2 = dr3[1].ToString(),
                                IMEI3 = dr3[2].ToString(),
                                IMEI4 = dr3[3].ToString(),
                                IMEI5 = dr3[4].ToString(),
                                PrintTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff"),
                                Template = this.Select_Template.Text
                            });
                            if (MEPPB.InsertManuExcelPrintBLL(mepp))
                            {
                                btFormat.Print();
                                Form1.Log("Excel打印了机身贴IMEI号为" + dr3[1].ToString() + "的制单", null);
                            }
                        }
                        else
                        {
                            player.Play();
                            this.remined.AppendText(dr3[0].ToString() + "或" + dr3[1].ToString() + "重号\r\n");
                        }
                    }
                    else if (IsTrue(this.RowNumber.Text))
                    {
                        LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                        //指定打印机名称
                        btFormat.PrintSetup.PrinterName = this.Printer.Text;
                        string[] range = this.RowNumber.Text.Split('-');
                        string strExtension = this.ImportPath.Text.Substring(this.ImportPath.Text.LastIndexOf('.'));
                        DataTable dt = IEB.GetExcelDatatable(this.ImportPath.Text, strExtension);
                        for (int i = int.Parse(range[0]); i <= int.Parse(range[1]); i++)
                        {
                            DataRow dr3 = dt.Rows[i];
                            if (!MEPPB.CheckIMEIBLL(dr3[0].ToString(), dr3[1].ToString()))
                            {
                                //对模板相应字段进行赋值
                                btFormat.SubStrings["IMEI1"].Value = dr3[0].ToString();
                                btFormat.SubStrings["IMEI2"].Value = dr3[1].ToString();
                                btFormat.SubStrings["IMEI3"].Value = dr3[2].ToString();
                                btFormat.SubStrings["IMEI4"].Value = dr3[3].ToString();
                                btFormat.SubStrings["IMEI5"].Value = dr3[4].ToString();
                                btFormat.SubStrings["ProductDate"].Value = DateTime.Now.ToString("yyyy.MM.dd");
                                //打印份数,同序列打印的份数
                                btFormat.PrintSetup.IdenticalCopiesOfLabel = TN;
                                //记录数据
                                mepp.Add(new ManuExcelPrintParam()
                                {
                                    IMEI1 = dr3[0].ToString(),
                                    IMEI2 = dr3[1].ToString(),
                                    IMEI3 = dr3[2].ToString(),
                                    IMEI4 = dr3[3].ToString(),
                                    IMEI5 = dr3[4].ToString(),
                                    PrintTime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff"),
                                    Template = this.Select_Template.Text
                                });
                                if (MEPPB.InsertManuExcelPrintBLL(mepp))
                                {
                                    btFormat.Print();
                                    Form1.Log("Excel打印了机身贴IMEI号为" + dr3[1].ToString() + "的制单", null);
                                }
                            }
                            else
                            {
                                player.Play();
                                this.remined.AppendText(dr3[0].ToString() + "或" + dr3[1].ToString() + "重号\r\n");
                            }
                        }
                    }
                    else
                    {
                        player.Play();
                        this.remined.AppendText("请输入正确的行数格式：数字-数字\r\n");
                        this.RowNumber.Clear();
                        this.RowNumber.Focus();
                        this.dataGridView1.DataSource = "";
                    }
                }
                else
                {
                    player.Play();
                    this.remined.AppendText("请先选择模板\r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
            }
        }

        //导入Excel按钮函数
        private void OpenExcel_Click(object sender, EventArgs e)
        {
            if (this.ImportPath.Text == "")
            {
                player.Play();
               this.remined.AppendText("请先导入Excel\r\n");
            }
            else
            {
                string path = this.ImportPath.Text;
                if (File.Exists(path))
                {
                    System.Diagnostics.Process.Start(path);
                }
                else
                {
                    player.Play();
                    this.remined.AppendText("Excel不存在\r\n");
                }
            }
        }

        //查询函数
        private void E_Check_Click(object sender, EventArgs e)
        {
            if (this.WhatToCheck.Text != "")
            {
                this.dataGridView1.DataSource = "";
                int co=1;
                mepp = MEPPB.SelectByImei1to5BLL(this.WhatToCheck.Text);
                if (mepp.Count == 0)
                {
                    player.Play();
                    this.remined.AppendText("查找不到关于" + this.WhatToCheck.Text + "的记录\r\n");
                    this.WhatToCheck.Clear();
                }
                else
                {
                    DataTable dd = new DataTable();
                    dd.Columns.Add(" ");
                    dd.Columns.Add("IMEI1");
                    dd.Columns.Add("IMEI2");
                    dd.Columns.Add("IMEI3");
                    dd.Columns.Add("IMEI4");
                    dd.Columns.Add("IMEI5");
                    dd.Columns.Add("打印时间");
                    dd.Columns.Add("打印模板");
                    dd.Columns.Add("重打次数");
                    dd.Columns.Add("首次重打时间");
                    dd.Columns.Add("末次重打时间");
                    foreach (ManuExcelPrintParam a in mepp)
                    {
                        dd.Rows.Add(co, a.IMEI1, a.IMEI2, a.IMEI3, a.IMEI4, a.IMEI5,a.PrintTime,a.Template,a.RePrintNum,a.ReFirstPrintTime,a.ReEndPrintTime);
                        co++;
                    }
                    dataGridView1.DataSource = dd;
                    dataGridView1.Columns[0].Width = 20;
                    dataGridView1.Columns[1].Width = 120;
                    dataGridView1.Columns[2].Width = 120;
                    dataGridView1.Columns[3].Width = 150;
                    dataGridView1.Columns[4].Width = 150;
                    dataGridView1.Columns[5].Width = 150;
                    dataGridView1.Columns[6].Width = 150;
                    dataGridView1.Columns[7].Width = 150;
                    dataGridView1.Columns[8].Width = 100;
                    dataGridView1.Columns[9].Width = 150;
                    dataGridView1.Columns[10].Width = 150;
                }
            }
            else
            {
                this.dataGridView1.DataSource = "";
            }
        }

        //打开模板按钮
        private void OpenTemplate_Click(object sender, EventArgs e)
        {
            if (this.Select_Template.Text == "")
            {
                player.Play();
            }
            else
            {
                string path = this.Select_Template.Text;
                if (File.Exists(path))
                {
                    System.Diagnostics.Process.Start(path);
                }
                else
                {
                    player.Play();
                }
            }
        }

        //调试打印
        private void Debug_Print_Click(object sender, EventArgs e)
        {
            //判断导入Excel没
            if (this.ImportPath.Text == "")
            {
                player.Play();
                this.remined.AppendText("请先导入Excel\r\n");
                return;
            }
            //判断模板是否为空
            if (this.Select_Template.Text == "")
            {
                player.Play();
                this.remined.AppendText("请先选择模板\r\n");
                return;
            }
            string strExtension = this.ImportPath.Text.Substring(this.ImportPath.Text.LastIndexOf('.'));
            DataTable dt = IEB.GetExcelDatatable(this.ImportPath.Text, strExtension);
            DataRow dr3 = dt.Rows[1];
            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
            //指定打印机名称
            btFormat.PrintSetup.PrinterName = this.Printer.Text;
            //对模板相应字段进行赋值
            btFormat.SubStrings["IMEI1"].Value = dr3[0].ToString();
            btFormat.SubStrings["IMEI2"].Value = dr3[1].ToString();
            btFormat.SubStrings["IMEI3"].Value = dr3[2].ToString();
            btFormat.SubStrings["IMEI4"].Value = dr3[3].ToString();
            btFormat.SubStrings["IMEI5"].Value = dr3[4].ToString();
            btFormat.SubStrings["ProductDate"].Value = DateTime.Now.ToString("yyyy.MM.dd");
            //打印份数,同序列打印的份数
            btFormat.PrintSetup.IdenticalCopiesOfLabel = TN;
            btFormat.Print();
            Form1.Log("Excel调试打印了机身贴IMEI号为" + dr3[1].ToString() + "的制单", null);
        }

        //刷新模板
        private void RefeshTemplate_Click(object sender, EventArgs e)
        {
            if (this.Select_Template.Text != "")
            {
                string path = this.Select_Template.Text;
                string filename = Path.GetFileName(path);
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Excel模板"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Excel模板");
                }
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Excel模板\\" + RefreshNum))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Excel模板\\" + RefreshNum);
                }
                File.Copy(path, AppDomain.CurrentDomain.BaseDirectory + "\\Excel模板\\" + RefreshNum + "\\" + filename, true);
                lj = AppDomain.CurrentDomain.BaseDirectory + "\\Excel模板\\" + RefreshNum + "\\" + filename;
                this.remined.AppendText("刷新模板成功\r\n");
                RefreshNum++;
            }
        }

        //重打查询按钮函数
        private void RePrintCheck_Click(object sender, EventArgs e)
        {
            mepp = MEPPB.SelectAllRePrintBLL();
            int co = 1;
            if (mepp.Count == 0)
            {
                player.Play();
                this.remined.AppendText("无重打记录\r\n");
            }
            else
            {
                DataTable dd = new DataTable();
                dd.Columns.Add(" ");
                dd.Columns.Add("IMEI1");
                dd.Columns.Add("IMEI2");
                dd.Columns.Add("IMEI3");
                dd.Columns.Add("IMEI4");
                dd.Columns.Add("IMEI5");
                dd.Columns.Add("打印时间");
                dd.Columns.Add("打印模板");
                dd.Columns.Add("重打次数");
                dd.Columns.Add("首次重打时间");
                dd.Columns.Add("末次重打时间");
                foreach (ManuExcelPrintParam a in mepp)
                {
                    dd.Rows.Add(co, a.IMEI1, a.IMEI2, a.IMEI3, a.IMEI4, a.IMEI5, a.PrintTime, a.Template, a.RePrintNum, a.ReFirstPrintTime, a.ReEndPrintTime);
                    co++;
                }
                dataGridView1.DataSource = dd;
                dataGridView1.Columns[0].Width = 20;
                dataGridView1.Columns[1].Width = 120;
                dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[3].Width = 150;
                dataGridView1.Columns[4].Width = 150;
                dataGridView1.Columns[5].Width = 150;
                dataGridView1.Columns[6].Width = 150;
                dataGridView1.Columns[7].Width = 150;
                dataGridView1.Columns[8].Width = 100;
                dataGridView1.Columns[9].Width = 150;
                dataGridView1.Columns[10].Width = 150;
            }
        }

        //重打按钮函数
        private void RePrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ImportPath.Text == "")
                {
                    player.Play();
                    this.remined.AppendText("请先导入Excel\r\n");
                    return;
                }
                if (this.Select_Template.Text != "")
                {
                    if (IsNumeric(this.RowNumber.Text))
                    {
                        string strExtension = this.ImportPath.Text.Substring(this.ImportPath.Text.LastIndexOf('.'));
                        DataTable dt = IEB.GetExcelDatatable(this.ImportPath.Text, strExtension);
                        DataRow dr3 = dt.Rows[int.Parse(this.RowNumber.Text)];
                        if (MEPPB.CheckIMEIBLL(dr3[0].ToString(), dr3[1].ToString()))
                        {
                            LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                            //指定打印机名称
                            btFormat.PrintSetup.PrinterName = this.Printer.Text;
                            //对模板相应字段进行赋值
                            btFormat.SubStrings["IMEI1"].Value = dr3[0].ToString();
                            btFormat.SubStrings["IMEI2"].Value = dr3[1].ToString();
                            btFormat.SubStrings["IMEI3"].Value = dr3[2].ToString();
                            btFormat.SubStrings["IMEI4"].Value = dr3[3].ToString();
                            btFormat.SubStrings["IMEI5"].Value = dr3[4].ToString();
                            btFormat.SubStrings["ProductDate"].Value = DateTime.Now.ToString("yyyy.MM.dd");
                            //打印份数,同序列打印的份数
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = TN;
                            if (MEPPB.UpdateRePrintTimeBLL(dr3[0].ToString(), DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff")))
                            {
                                btFormat.Print();
                                Form1.Log("Excel打印了机身贴IMEI号为" + dr3[1].ToString() + "的制单", null);
                            }
                        }
                        else
                        {
                            player.Play();
                            this.remined.AppendText(dr3[0].ToString() + "这条数据无需重打\r\n");
                        }
                    }
                    else if (IsTrue(this.RowNumber.Text))
                    {
                        LabelFormatDocument btFormat = btEngine.Documents.Open(lj);
                        //指定打印机名称
                        btFormat.PrintSetup.PrinterName = this.Printer.Text;
                        string[] range = this.RowNumber.Text.Split('-');
                        string strExtension = this.ImportPath.Text.Substring(this.ImportPath.Text.LastIndexOf('.'));
                        DataTable dt = IEB.GetExcelDatatable(this.ImportPath.Text, strExtension);
                        for (int i = int.Parse(range[0]); i <= int.Parse(range[1]); i++)
                        {
                            DataRow dr3 = dt.Rows[i];
                            if (MEPPB.CheckIMEIBLL(dr3[0].ToString(), dr3[1].ToString()))
                            {
                                //对模板相应字段进行赋值
                                btFormat.SubStrings["IMEI1"].Value = dr3[0].ToString();
                                btFormat.SubStrings["IMEI2"].Value = dr3[1].ToString();
                                btFormat.SubStrings["IMEI3"].Value = dr3[2].ToString();
                                btFormat.SubStrings["IMEI4"].Value = dr3[3].ToString();
                                btFormat.SubStrings["IMEI5"].Value = dr3[4].ToString();
                                btFormat.SubStrings["ProductDate"].Value = DateTime.Now.ToString("yyyy.MM.dd");
                                //打印份数,同序列打印的份数
                                btFormat.PrintSetup.IdenticalCopiesOfLabel = TN;
                                if (MEPPB.UpdateRePrintTimeBLL(dr3[0].ToString(), DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff")))
                                {
                                    btFormat.Print();
                                    Form1.Log("Excel打印了机身贴IMEI号为" + dr3[1].ToString() + "的制单", null);
                                }
                            }
                            else
                            {
                                player.Play();
                                this.remined.AppendText(dr3[0].ToString() + "这条数据无需重打\r\n");
                            }
                        }
                    }
                    else
                    {
                        player.Play();
                        this.remined.AppendText("请输入正确的行数格式：数字-数字\r\n");
                        this.RowNumber.Clear();
                        this.RowNumber.Focus();
                        this.dataGridView1.DataSource = "";
                    }
                }
                else
                {
                    player.Play();
                    this.remined.AppendText("请先选择模板\r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
            }
        }

    }
}
