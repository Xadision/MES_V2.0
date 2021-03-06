﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Print_Message;

namespace Print.Message.Dal
{
    class PrintMessageDAL
    {
        private static readonly string conStr = ConfigurationManager.ConnectionStrings["conn1"].ConnectionString;



        //插入打印数据到ManuPrintParam表
        public int InsertPrintMessageDAL(List<PrintMessage> list) {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    string sim, vip, bat;
                    int i = list.Count;
                    if (i > 0)
                    {
                        if (list[i - 1].SIM == "0") { sim = ""; } else { sim = list[i - 1].SIM; }
                        if (list[i - 1].VIP == "0") { vip = ""; } else { vip = list[i - 1].VIP; }
                        if (list[i - 1].BAT == "0") { bat = ""; } else { bat = list[i - 1].BAT; }
                        string CH_PrintTime = list[i - 1].CH_PrintTime == "" ? "NULL" : "'" + list[i - 1].CH_PrintTime + "'";
                        string JS_PrintTime = list[i - 1].JS_PrintTime == "" ? "NULL" : "'" + list[i - 1].JS_PrintTime + "'";
                        command.CommandText = "INSERT INTO dbo.Gps_ManuPrintParam(ZhiDan,IMEI,IMEIStart,IMEIEnd,SN,IMEIRel,SIM,VIP,BAT,SoftModel,Version,Remark,JS_PrintTime,JS_TemplatePath,JS_ReprintNum,JS_ReFirstPrintTime,JS_ReEndPrintTime,UserName,CH_PrintTime,CH_TemplatePath1,CH_TemplatePath2,CH_ReprintNum,CH_ReFirstPrintTime,CH_ReEndPrintTime,ICCID,MAC,Equipment) VALUES('" + list[i - 1].Zhidan + "','" + list[i - 1].IMEI + "','" + list[i - 1].IMEIStart + "','" + list[i - 1].IMEIEnd + "','" + list[i - 1].SN + "','" + list[i - 1].IMEIRel + "','" + sim + "','" + vip + "','" + bat + "','" + list[i - 1].SoftModel + "','" + list[i - 1].Version + "','" + list[i - 1].Remark + "'," + JS_PrintTime + ",'" + list[i - 1].JS_TemplatePath + "','0',NULL,NULL,''," + CH_PrintTime + ",'" + list[i - 1].CH_TemplatePath1 + "','" + list[i - 1].CH_TemplatePath2 + "','0',NULL,NULL,'" + list[i - 1].ICCID + "','" + list[i - 1].MAC + "','" + list[i - 1].Equipment + "')";
                    }
                    int httpstr = command.ExecuteNonQuery();
                    return httpstr;
                }
            }
        }

        //更新彩盒关联打印信息(SIM+ICCID+SN)
        public int UpdateSN_SIM_ICCIDDAL(string IMEI, string CHPrintTime, string lj1, string lj2, string SIM, string ICCID, string SN)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    string CH_PrintTime = CHPrintTime == "" ? "NULL" : "'" + CHPrintTime + "'";
                    command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',SIM='" + SIM + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新彩盒关联打印信息(VIP+SN)
        public int UpdateSN_VIPDAL(string IMEI, string CHPrintTime, string lj1, string lj2, string VIP, string SN)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr)) {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    string CH_PrintTime = CHPrintTime == "" ? "NULL" : "'" + CHPrintTime + "'";
                    command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',VIP='" + VIP + "' WHERE IMEI='" + IMEI + "'";
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新彩盒关联打印信息(VIP(SIM/ICCID)+SN)
        public int UpdateSN_SIM_VIP_ICCIDDAL(string IMEI, string CHPrintTime, string lj1, string lj2, string SIM, string VIP, string ICCID, string SN)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    string CH_PrintTime = CHPrintTime == "" ? "NULL" : "'" + CHPrintTime + "'";
                    if (SIM == "")
                    {
                        command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',VIP='" + VIP + "' WHERE IMEI='" + IMEI + "'";
                    }
                    else
                    {
                        command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',SIM='" + SIM + "',VIP='" + VIP + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新彩盒关联打印信息(BAT(VIP/SIM/ICCID)+SN)
        public int UpdateSN_SIM_VIP_BAT_ICCIDDAL(string IMEI, string CHPrintTime, string lj1, string lj2, string SIM, string VIP, String BAT, string ICCID, string SN)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    string CH_PrintTime = CHPrintTime == "" ? "NULL" : "'" + CHPrintTime + "'";
                    if (SIM == "")
                    {
                        if (VIP == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',BAT='" + BAT + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',VIP='" + VIP + "',BAT='" + BAT + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    else
                    {
                        if (VIP == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',SIM='" + SIM + "',BAT='" + BAT + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',SIM='" + SIM + "',VIP='" + VIP + "',BAT='" + BAT + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                        }

                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新彩盒关联打印信息(BAT/VIP/ICCID)+SN)
        public int UpdateSN_VIP_BAT_ICCIDDAL(string IMEI, string CHPrintTime, string lj1, string lj2, string VIP, String BAT, string ICCID, string SN)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    string CH_PrintTime = CHPrintTime == "" ? "NULL" : "'" + CHPrintTime + "'";
                    if (VIP == "")
                    {
                        if (BAT == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',BAT='" + BAT + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    else
                    {
                        if (BAT == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',VIP='" + VIP + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',VIP='" + VIP + "',BAT='" + BAT + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新彩盒关联打印信息(MAC(VIP/BAT/SIM/ICCID)+SN)
        public int UpdateSN_SIM_VIP_BAT_ICCID_MACDAL(string IMEI, string CHPrintTime, string lj1, string lj2, string SIM, string VIP, String BAT, string ICCID, String MAC, string SN)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    string CH_PrintTime = CHPrintTime == "" ? "NULL" : "'" + CHPrintTime + "'";
                    if (SIM == "")
                    {
                        if (VIP != "" && BAT != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', VIP ='" + VIP + "', BAT ='" + BAT + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', VIP ='" + VIP + "', MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',  BAT ='" + BAT + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    else
                    {
                        if (VIP != "" && BAT != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "',  VIP ='" + VIP + "', BAT ='" + BAT + "',ICCID='" + ICCID + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "',  VIP ='" + VIP + "', ICCID='" + ICCID + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',  BAT ='" + BAT + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "', MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新彩盒关联打印信息(MAC(VIP/BAT/SIM/ICCID)+SN)
        public int UpdateSN_SIM_VIP_BAT_ICCID_MAC_EquipmentDAL(string IMEI, string CHPrintTime, string lj1, string lj2, string SIM, string VIP, String BAT, string ICCID, String MAC, string Equipment, string SN)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    string CH_PrintTime = CHPrintTime == "" ? "NULL" : "'" + CHPrintTime + "'";
                    if (SIM == "")
                    {
                        if (VIP != "" && BAT != "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', VIP ='" + VIP + "', BAT ='" + BAT + "',MAC='" + MAC + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT != "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', VIP ='" + VIP + "', BAT ='" + BAT + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', VIP ='" + VIP + "', MAC='" + MAC + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', VIP ='" + VIP + "', Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',  BAT ='" + BAT + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "',  BAT ='" + BAT + "',MAC='" + MAC + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT == "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', MAC='" + MAC + "', Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    else
                    {
                        if (VIP != "" && BAT != "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "', VIP ='" + VIP + "', BAT ='" + BAT + "',ICCID='" + ICCID + "',MAC='" + MAC + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT != "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "', VIP ='" + VIP + "', BAT ='" + BAT + "',ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "', VIP ='" + VIP + "', MAC='" + MAC + "',ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "', VIP ='" + VIP + "', ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "',  BAT ='" + BAT + "',ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "',  BAT ='" + BAT + "',MAC='" + MAC + "',ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT == "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "',ICCID='" + ICCID + "', Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  SN='" + SN + "', CH_PrintTime=" + CH_PrintTime + ", CH_TemplatePath1 ='" + lj1 + "', CH_TemplatePath2 ='" + lj2 + "', SIM ='" + SIM + "', MAC='" + MAC + "', ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新Sim、Iccid字段
        public int UpdateSimIccidDAL(string IMEI, string SIM, string ICCID)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "', ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新VIP字段
        public int UpdateVIPDAL(string IMEI, string VIP)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  VIP ='" + VIP + "' WHERE IMEI='" + IMEI + "'";
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新Sim、Vip、Iccid字段
        public int UpdateSimVipIccidDAL(string IMEI, string SIM, string VIP, string ICCID)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "',  VIP ='" + VIP + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新Sim、Vip、Bat、Iccid字段
        public int UpdateVipAndBatDAL(string IMEI, string SIM, string VIP, string BAT, string ICCID)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    if (SIM == "")
                    {
                        if (VIP != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET VIP ='" + VIP + "', BAT ='" + BAT + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET BAT ='" + BAT + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    else
                    {
                        if (VIP != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "',  VIP ='" + VIP + "', BAT ='" + BAT + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "', BAT ='" + BAT + "',ICCID='" + ICCID + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新Sim、Vip、Bat、Iccid,MAC字段
        public int UpdateVipAndBatAndMacDAL(string IMEI, string SIM, string VIP, string BAT, string ICCID, string MAC)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    if (SIM == "")
                    {
                        if (VIP != "" && BAT != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET VIP ='" + VIP + "', BAT ='" + BAT + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET VIP ='" + VIP + "', MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  BAT ='" + BAT + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    else
                    {
                        if (VIP != "" && BAT != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "',  VIP ='" + VIP + "', BAT ='" + BAT + "',ICCID='" + ICCID + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "',  VIP ='" + VIP + "', ICCID='" + ICCID + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  BAT ='" + BAT + "',MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "', MAC='" + MAC + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        //更新Sim、Vip、Bat、Iccid,MAC,Equipment字段
        public int UpdateVipAndBatAndMacAndEquDAL(string IMEI, string SIM, string VIP, string BAT, string ICCID, string MAC, string Equipment)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    if (SIM == "")
                    {
                        if (VIP != "" && BAT != "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET VIP ='" + VIP + "', BAT ='" + BAT + "',MAC='" + MAC + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT != "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET VIP ='" + VIP + "', BAT ='" + BAT + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET VIP ='" + VIP + "', MAC='" + MAC + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET VIP ='" + VIP + "', Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  BAT ='" + BAT + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET  BAT ='" + BAT + "',MAC='" + MAC + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT == "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET MAC='" + MAC + "', Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }
                    else
                    {
                        if (VIP != "" && BAT != "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "', VIP ='" + VIP + "', BAT ='" + BAT + "',ICCID='" + ICCID + "',MAC='" + MAC + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT != "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "', VIP ='" + VIP + "', BAT ='" + BAT + "',ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "', VIP ='" + VIP + "', MAC='" + MAC + "',ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP != "" && BAT == "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "', VIP ='" + VIP + "', ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "',  BAT ='" + BAT + "',ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT != "" && MAC != "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "',  BAT ='" + BAT + "',MAC='" + MAC + "',ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else if (VIP == "" && BAT == "" && MAC == "")
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "',ICCID='" + ICCID + "', Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                        else
                        {
                            command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET SIM ='" + SIM + "', MAC='" + MAC + "', ICCID='" + ICCID + "',Equipment='" + Equipment + "' WHERE IMEI='" + IMEI + "'";
                        }
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }

        //检查IMEI号是否存在，存在返回1，否则返回0
        public int CheckCHOrJSIMEIDAL(string IMEInumber, int PrintType) {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    if (PrintType == 1)
                    {
                        command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + IMEInumber + "' AND JS_PrintTime is NULL)";
                    }
                    else
                    {
                        command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + IMEInumber + "' AND (CH_PrintTime is NULL OR CH_PrintTime=''))";
                    }
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //检查IMEI号是否存在，存在返回1，否则返回0
        public int CheckReCHOrJSIMEIDAL(string IMEInumber, int PrintType)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    if (PrintType == 1)
                    {
                        command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + IMEInumber + "' AND JS_PrintTime != '')";
                    }
                    else
                    {
                        command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + IMEInumber + "' AND CH_PrintTime != '')";
                    }
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //范围检查机身贴IMEI号数量是否与输入的起始位到终止位之间的数量相等，相等返回1，否则返回0
        public int CheckReJSRangeIMEIDAL(string StarIMEI, string EndIMEI)
        {
            SqlConnection conn1 = new SqlConnection(conStr);
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM dbo.Gps_ManuPrintParam WHERE (IMEI>='" + StarIMEI + "' AND IMEI<='" + EndIMEI + "')";
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        //检查IMEI号是否存在，存在返回1，否则返回0
        public int CheckIMEIDAL(string IMEInumber)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE IMEI='" + IMEInumber + "'";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //范围检查IMEI号是否存在，存在返回IMEI，否则返回0
        public List<PrintMessage> CheckRangeIMEIDAL(string StarIMEI, string EndIMEI)
        {
            List<PrintMessage> pm = new List<PrintMessage>();
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI>='" + StarIMEI + "' AND IMEI<='" + EndIMEI + "')";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        pm.Add(new PrintMessage()
                        {
                            IMEI = dr.GetString(2)
                        });
                    }
                    return pm;
                }
            }
        }

        //检查SN号是否存在，存在返回1，否则返回0
        public int CheckSNDAL(string SNnumber)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE SN='" + SNnumber + "'";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //检查SIM号是否存在，存在返回1，否则返回0
        public int CheckSIMDAL(string SIM)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE SIM='" + SIM + "'";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //检查VIP号是否存在，存在返回1，否则返回0
        public int CheckVIPDAL(string VIP)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE VIP='" + VIP + "'";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //检查BAT号是否存在，存在返回1，否则返回0
        public int CheckBATDAL(string BAT)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr)) {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE BAT='" + BAT + "'";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //检查ICCID号是否存在，存在返回1，否则返回0
        public int CheckICCIDDAL(string ICCID)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr)) {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE ICCID='" + ICCID + "'";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //检查MAC号是否存在，存在返回1，否则返回0
        public int CheckMACDAL(string MAC)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr)) {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE MAC='" + MAC + "'";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //检查Equipment号是否存在，存在返回1，否则返回0
        public int CheckEquipmentDAL(string Equipment)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE Equipment='" + Equipment + "'";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        //根据IMEI号获取sn号进行重打
        public List<PrintMessage> SelectSnByIMEIDAL(string IMEInumber)
        {
            List<PrintMessage> pm = new List<PrintMessage>();
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE IMEI='" + IMEInumber + "'";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        pm.Add(new PrintMessage()
                        {
                            SN = dr.IsDBNull(5) ? "" : dr.GetString(5),
                            SIM = dr.IsDBNull(7) ? "" : dr.GetString(7),
                            VIP = dr.IsDBNull(8) ? "" : dr.GetString(8),
                            BAT = dr.IsDBNull(9) ? "" : dr.GetString(9),
                            SoftModel = dr.GetString(10),
                            Remark = dr.IsDBNull(12) ? "" : dr.GetString(12),
                            ICCID = dr.IsDBNull(25) ? "" : dr.GetString(25),
                            MAC = dr.IsDBNull(26) ? "" : dr.GetString(26),
                            Equipment = dr.IsDBNull(27) ? "" : dr.GetString(27)
                        });
                    }
                    return pm;
                }
            }
        }

        //根据IMEI号只获取sn号进行重打
        public string SelectOnlySnByIMEIDAL(string IMEInumber)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT SN FROM dbo.Gps_ManuPrintParam WHERE IMEI='" + IMEInumber + "'";
                    return Convert.ToString(command.ExecuteScalar());
                }
            }
        }


        //根据IMEI号获取机身贴重打次数
        public int SelectJS_RePrintNumByIMEIDAL(string IMEInumber)
        {
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "select  * FROM dbo.Gps_ManuPrintParam WHERE IMEI='" + IMEInumber + "'";
                SqlDataReader dr = command.ExecuteReader();
                int RePrintNum = 0;
                while (dr.Read())
                {
                    RePrintNum = dr.GetInt32(15);
                }
                return RePrintNum;
            }
            }
        }

        //根据IMEI号获取彩盒贴重打次数
        public int SelectCH_RePrintNumByIMEIDAL(string IMEInumber)
        {
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "select  * FROM dbo.Gps_ManuPrintParam WHERE IMEI='" + IMEInumber + "'";
                SqlDataReader dr = command.ExecuteReader();
                int RePrintNum = 0;
                while (dr.Read())
                {
                    RePrintNum = dr.GetInt32(22);
                }
                return RePrintNum;
            }
            }
        }

        //更新机身首次重打数据
        public int UpdateRePrintDAL(string IMEInumber, string RePrintTime,string lj)
        {
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET JS_ReFirstPrintTime ='" + RePrintTime + "',JS_TemplatePath = '" + lj + "',JS_RePrintNum = JS_RePrintNum+1 WHERE IMEI='" + IMEInumber + "'";
                return command.ExecuteNonQuery();
            }
            }
        }

        //更新机身末次重打数据
        public int UpdateReEndPrintDAL(string IMEInumber, string RePrintTime, string lj)
        {
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET JS_ReEndPrintTime ='" + RePrintTime + "', JS_TemplatePath = '" + lj + "',JS_RePrintNum = JS_RePrintNum+1 WHERE IMEI='" + IMEInumber + "' ";
                return command.ExecuteNonQuery();
            }
            }
        }

        //更新彩盒首次重打数据
        public int UpdateCHRePrintDAL(string IMEInumber, string RePrintTime, string lj,string lj1)
        {
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET CH_ReFirstPrintTime ='" + RePrintTime + "',CH_TemplatePath1 = '" + lj + "',CH_TemplatePath2 = '" + lj1 + "', CH_RePrintNum = CH_RePrintNum+1 WHERE IMEI='" + IMEInumber + "'";
                return command.ExecuteNonQuery();
            }
            }
        }

        //更新彩盒末次重打数据
        public int UpdateCHReEndPrintDAL(string IMEInumber, string RePrintTime, string lj,string lj1)
        {
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET CH_ReEndPrintTime ='" + RePrintTime + "', CH_TemplatePath1 = '" + lj + "',CH_TemplatePath2 = '" + lj1 + "',CH_RePrintNum = CH_RePrintNum+1 WHERE IMEI='" + IMEInumber + "' ";
                return command.ExecuteNonQuery();
            }
            }
        }

        //获取机身贴重打记录
        public List<PrintMessage> SelectAllReJSTDAL()
        {
            List<PrintMessage> pm = new List<PrintMessage>();
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE JS_RePrintNum !=0";
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    pm.Add(new PrintMessage()
                    {
                        Zhidan = dr.GetString(1),
                        IMEI = dr.GetString(2),
                        SN = dr.GetString(5),
                        SoftModel = dr.IsDBNull(10) ? "" : dr.GetString(10),
                        JS_PrintTime = dr.GetString(13),
                        JS_TemplatePath = dr.GetString(14),
                        JS_RePrintNum = dr.GetInt32(15),
                        JS_ReFirstPrintTime = dr.IsDBNull(16) ? "" : dr.GetDateTime(16).ToString(),
                        JS_ReEndPrintTime = dr.IsDBNull(17) ? "" : dr.GetDateTime(17).ToString()
                    });
                }
                return pm;
            }
            }
        }

        //获取机身贴重打记录
        public List<PrintMessage> SelectAllReCHTDAL()
        {
            List<PrintMessage> pm = new List<PrintMessage>();
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE CH_RePrintNum !=0";
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    pm.Add(new PrintMessage()
                    {
                        Zhidan = dr.GetString(1),
                        IMEI = dr.GetString(2),
                        SN = dr.GetString(5),
                        SoftModel = dr.IsDBNull(10) ? "" : dr.GetString(10),
                        CH_PrintTime = dr.IsDBNull(19) ? "" : dr.GetString(19),
                        CH_TemplatePath1 = dr.IsDBNull(20) ? "" : dr.GetString(20),
                        CH_TemplatePath2 = dr.IsDBNull(21) ? "" : dr.GetString(21),
                        CH_RePrintNum = dr.GetInt32(22).ToString(),
                        CH_ReFirstPrintTime = dr.IsDBNull(23) ? "" : dr.GetDateTime(23).ToString(),
                        CH_ReEndPrintTime = dr.IsDBNull(24) ? "" : dr.GetDateTime(24).ToString()
                    });
                }
                return pm;
            }
            }
        }

        //根据制单号或IMEI号获取重打记录
        public List<PrintMessage> SelectReMesByZhiDanOrIMEIDAL(string ToCheck)
        {
            List<PrintMessage> pm = new List<PrintMessage>();
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE ((ZhiDan='" + ToCheck + "' OR IMEI='" + ToCheck + "') AND (CH_RePrintNum!=0 OR JS_RePrintNum!=0))";
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    pm.Add(new PrintMessage()
                    {
                        Zhidan = dr.GetString(1),
                        IMEI = dr.GetString(2),
                        SN = dr.IsDBNull(5) ? "" : dr.GetString(5),
                        SoftModel = dr.IsDBNull(10) ? "" : dr.GetString(10),
                        JS_PrintTime = dr.IsDBNull(13) ? "" : dr.GetString(13),
                        JS_TemplatePath = dr.IsDBNull(14) ? "" : dr.GetString(14),
                        JS_RePrintNum = dr.GetInt32(15),
                        JS_ReFirstPrintTime = dr.IsDBNull(16) ? "" : dr.GetDateTime(16).ToString(),
                        JS_ReEndPrintTime = dr.IsDBNull(17) ? "" : dr.GetDateTime(17).ToString(),
                        CH_PrintTime = dr.IsDBNull(19) ? "" : dr.GetString(19),
                        CH_TemplatePath1 = dr.IsDBNull(20) ? "" : dr.GetString(20),
                        CH_TemplatePath2 = dr.IsDBNull(21) ? "" : dr.GetString(21),
                        CH_RePrintNum = dr.GetInt32(22).ToString(),
                        CH_ReFirstPrintTime = dr.IsDBNull(23) ? "" : dr.GetDateTime(23).ToString(),
                        CH_ReEndPrintTime = dr.IsDBNull(24) ? "" : dr.GetDateTime(24).ToString()
                    });
                }
                return pm;
            }
            }
        }

        //根据SN号或IMEI号获取打印记录
        public List<PrintMessage> SelectPrintMesBySNOrIMEIDAL(string conditions)
        {
            List<PrintMessage> pm = new List<PrintMessage>();
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "SELECT * FROM dbo.Gps_ManuPrintParam WHERE (IMEI='" + conditions + "' OR SN='" + conditions + "')";
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    pm.Add(new PrintMessage()
                    {
                        ID = dr.GetInt32(0),
                        Zhidan = dr.IsDBNull(1) ? "" : dr.GetString(1),
                        IMEI = dr.IsDBNull(2) ? "" : dr.GetString(2),
                        SN = dr.IsDBNull(5) ? "" : dr.GetString(5),
                        IMEIRel = dr.IsDBNull(6) ? "" : dr.GetString(6),
                        SIM = dr.IsDBNull(7) ? "" : dr.GetString(7),
                        VIP = dr.IsDBNull(8) ? "" : dr.GetString(8),
                        BAT = dr.IsDBNull(9) ? "" : dr.GetString(9),
                        SoftModel = dr.IsDBNull(10) ? "" : dr.GetString(10),
                        JS_PrintTime = dr.IsDBNull(13) ? "" : dr.GetString(13),
                        JS_TemplatePath = dr.IsDBNull(14) ? "" : dr.GetString(14),
                        ICCID = dr.IsDBNull(25) ? "" : dr.GetString(25),
                        MAC = dr.IsDBNull(26) ? "" : dr.GetString(26),
                        Equipment = dr.IsDBNull(27) ? "" : dr.GetString(27)
                    });
                }
                return pm;
            }
            }
        }

        //根据ID删除打印记录
        public int DeletePrintMessageDAL(int id,string field)
        {
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "UPDATE dbo.Gps_ManuPrintParam SET " + field + " ='' WHERE ID='" + id + "'";
                int httpstr = command.ExecuteNonQuery();
                return httpstr;
            }
            }
        }


        //根据制单号获取首次选择信息
        public List<PrintMessage> SelectPrintMesByZhiDanDAL(string ZhiDan)
        {
            List<PrintMessage> pm = new List<PrintMessage>();
             using (SqlConnection conn1 = new SqlConnection(conStr)){
            conn1.Open();
            using (SqlCommand command = conn1.CreateCommand())
            {
                command.CommandText = "SELECT TOP 1 * FROM dbo.Gps_ManuPrintParam WHERE ZhiDan='" + ZhiDan + "'";
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    pm.Add(new PrintMessage()
                    {
                        SN = dr.IsDBNull(5) ? "" : dr.GetString(5),
                        SIM = dr.IsDBNull(7) ? "" : dr.GetString(7),
                        VIP = dr.IsDBNull(8) ? "" : dr.GetString(8),
                        BAT = dr.IsDBNull(9) ? "" : dr.GetString(9),
                        ICCID = dr.IsDBNull(25) ? "" : dr.GetString(25),
                        MAC = dr.IsDBNull(26) ? "" : dr.GetString(26),
                        Equipment = dr.IsDBNull(27) ? "" : dr.GetString(27)
                    });
                }
                return pm;
            }
            }
        }

        //根据制单号获取imei实现断电保护IMEI当前号
        public string SelectPresentImeiByZhidanDAL(string ZhiDan)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                string PresentImei = "";
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT top 1 *FROM [GPSTest].[dbo].[Gps_ManuPrintParam]  WHERE ZhiDan ='" + ZhiDan + "' ORDER BY IMEI  DESC";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        PresentImei = dr.GetString(2);
                    }
                    return PresentImei;
                }
            }
        }

        //根据制单号获取sn实现断电保护sn当前号
        public string SelectPresentSNByZhidanDAL(string ZhiDan)
        {
            using (SqlConnection conn1 = new SqlConnection(conStr))
            {
                conn1.Open();
                string Presentsn = "";
                using (SqlCommand command = conn1.CreateCommand())
                {
                    command.CommandText = "SELECT top 1 *FROM [GPSTest].[dbo].[Gps_ManuPrintParam]  WHERE ZhiDan ='" + ZhiDan + "' ORDER BY IMEI  DESC";
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        Presentsn = dr.GetString(5);
                    }
                    return Presentsn;
                }
            }
        }

    }
}
