#pragma once
#include "afxwin.h"

#include "afxmt.h"//CEvent

#include "WriteIMEIDlg.h"

#include "PackingPrint.h"

#include "md5.h"

#include <vector>      
#include <map>
 //图片显示
#include "ThumbnailBoxDlg.h"

#include <deque>

// IMEIWrite_MulAT 对话框
#define THREAD_NUM 16
#define PORTS_NUM 48

#define WM_SEFOCS  WM_USER+111

static CRITICAL_SECTION GETPORT;

static CRITICAL_SECTION CPUCOMMUNICATE;

static CRITICAL_SECTION UseScanResult;

static CRITICAL_SECTION SPEAKVOICE;

static CRITICAL_SECTION SUCCFAILCOUNT;

static CRITICAL_SECTION UPDATEDB[THREAD_NUM/4];

static CRITICAL_SECTION WIFICOMMUNICATE[THREAD_NUM/2];

static CRITICAL_SECTION SHOWONEPIC;

#define STOPDELAY 200
#define ITEMMAX   100

#define PICDATAMAX 150000
typedef struct
{
	LPVOID WinHandle;
	int HandleNum;
	CEdit* m_Result;
	CEdit* Final_Result_Control;
	CThumbnailBoxDlg* picdlg;
	CString ChipRfIDBG;	//后台要上传的数据
	BOOL DayTimeStatep; //白天黑夜状态
	CString RecString;	//串口接收到的图片数据
}
PARA_Handle;

//任务管理器
struct wininfo
{
	char winbuf[100];
	char filename[100];
	CString winname;
	unsigned long pid;
	HICON winicon;
	HWND hwnd;
	short sign;
};

//改变窗口大小
#include ".\ResizableLib\ResizableDialog.h"

//语音播报
#include <sapi.h>

//图片合成
#include ".\JPGUnion\BasicUse.h"
#include ".\JPGUnion\CombineImages.h"
#include ".\JPGUnion\BjfProcess.h"
#include ".\JPGUnion\PictureProcess.h"


/*三合一新增功能全局函数*/
//变量
struct CopyDataMSG
{
	char Port[1024];//串口号
	INT MessageNum;//自定消息
	char MessageChar[1024];//传递字符串
};

//函数
LONG CallBackCrashHandler(EXCEPTION_POINTERS *pException);

/*三合一新增功能到这里结束*/

class IMEIWrite_MulAT : public CResizableDialog
{
	DECLARE_DYNAMIC(IMEIWrite_MulAT)

public:
	IMEIWrite_MulAT(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~IMEIWrite_MulAT();

// 对话框数据
	enum { IDD = IDD_MULTCOMTEST };

protected:
	HICON m_hIcon;

	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();

	DECLARE_MESSAGE_MAP()
protected:
	afx_msg HBRUSH IMEIWrite_MulAT::OnCtlColor(CDC* pDC,CWnd* pWnd,UINT nCtlColor); 
	virtual BOOL PreTranslateMessage(MSG* pMsg);

	void OnCbnSelchangeCombo2_Fresh();					//刷新串口
	void OnGetWebSetting();
	BOOL GetSettingFromDB(CAdoInterface& myado,CString m_server,CString m_db,CString m_user,CString m_pwd,CString SoftwareVer,CEdit* m_Result,CEdit* Final_Result_Control);
	BOOL GetATSetting(CString SettingChar);

	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnContextMenu(CWnd*, CPoint point);

	virtual LRESULT DefWindowProc(UINT message, WPARAM wParam, LPARAM lParam); 

	afx_msg void OnTimer(UINT nIDEvent);
public:
	CString ATCommandList_CSDBCompare;
public:
	BOOL SIMisOK[THREAD_NUM];
	PARAMETER_MAIN paraArray[MAXPARANUM];

	CFont* font;
	CFont* fontsp;
public:
	CEvent RunCommandList1;
	BOOL StopAutoStart;
	BOOL StopSign[THREAD_NUM];
public:
	void UI_LoadConfig();
	void AT_LoadConfig();
	void Delay_Clock(UINT mSecond);

	BOOL OnGetport();
	void InitCOM(CComboBox* m_Port,CComboBox* m_Baud,int num);//初始化串口
	BOOL OPen_Serial_Port(CComboBox* m_Port,CComboBox* m_Baud,int HandleNum,BOOL CPUChoose=FALSE);
	BOOL OPen_Serial_PortReadConstant(CComboBox* m_Port,CComboBox* m_Baud,int HandleNum,BOOL CPUChoose=FALSE);
	BOOL CheckConnect_Thread(CComboBox* m_Port,CComboBox* m_Baud,int HandleNum,CEdit* m_Result,CEdit* Final_Result_Control);
	char*  Send_Serial_Order(CString* Vaule_Return,CString strCommand_Vaule,int HandleNum,char* EndSign,char* StartSign,int WaitTime=3);

	void LogShow_exchange(CEdit* m_Result,CEdit* Final_Result_Control,int State,CString Msg_Log,int HandleNum,CString Category="-1",CString ChipRfIDbg=""); //重点看
	bool IMEI_Function_Judge(int i,CString IMEI_FT_Item,char* Serial_Order_Return,int HandleNum,CEdit* m_Result,CEdit* Final_Result_Control);

	CString GetData(char* Serial_Order_Return,CString Start,CString End,int Count=1,int HandleNum=0);//获取有效数据

public:	
	BOOL WriteIMEIFunction_Thread(CComboBox* m_Port,CComboBox* m_Baud,int HandleNum,CEdit* m_Result,CEdit* Final_Result_Control,CEdit* Data_Input_Control);
	__int64 ComputeCD(__int64 nImei);
	BOOL CheckIMEI(CString	m_IMEI);

	void EnableWindow_ALL(BOOL Choose);

	void EnableWindow_StartALL(BOOL Choose);
	void EnableWindow_Start(int HandleNum);
	void EnableWindow_Choose(int HandleNum);
	void SetFocus_Choose(int HandleNum);
	//void SetFocus_ChooseThread(int HandleNum);

	void Get_App_Running();//获取正在运行的应用 
	void OnWndFocus(BOOL ShowWin,BOOL Print,CString IMEI_Print[THREAD_NUM]);
public:
	CArray<wininfo,wininfo&> m_wininfoarray;
	wininfo m_currentwin;
	CEvent        Save_Wait;
public:
	BOOL EnableMenu;

	BOOL COM_IniDone;
	CString COM_Number[PORTS_NUM];

	BOOL COM_State[THREAD_NUM+1];//串口状态 
	BOOL Thread_State[THREAD_NUM];
public:
	COLORREF gColor[THREAD_NUM];
	
	CBT_Addr_Setting IMEI_Setdlg;
	//CBT_Addr_Setting IMEI_Setdlg2;
	//CBT_Addr_Setting IMEI_SetDB[THREAD_NUM];
	HANDLE hPort[THREAD_NUM+1];						//+1-->单片机控制端口
	CString Vaule_Return_Count_CS[THREAD_NUM+1];	//接收到的参数
	CWinThread* Thread_Handle[THREAD_NUM];

	CString IMEI_Input[THREAD_NUM];

	////////////////////////////////////////
	////////////////////////////////////////
public:
	// 写号线程1结果
	CEdit Final_Result_Control1;
	// LOG信息
	CEdit m_Result1;

	CComboBox m_Port1;
	CComboBox m_Baud1;

public:
	afx_msg void OnEnChangeImeia();
	CString IMEI_InputA;
	CEdit IMEI_InputA_Control;
	afx_msg void OnBnClickedButton1();
	afx_msg void OnBnClickedButton2();




	// 线程2
	CEdit Final_Result_Control2;
	CEdit m_Result2;
	CComboBox m_Port2;
	CComboBox m_Baud2;

	afx_msg void OnEnChangeImeia2();
	CString IMEI_InputB;
	CEdit IMEI_InputB_Control;
	afx_msg void OnBnClickedButton14();
	afx_msg void OnBnClickedButton15();


	// 线程3
	CEdit Final_Result_Control3;
	CEdit m_Result3;
	CComboBox m_Port3;
	CComboBox m_Baud3;
	CString IMEI_InputC;
	CEdit IMEI_InputC_Control;
	afx_msg void OnEnChangeImeia3();
	afx_msg void OnBnClickedButton16();
	afx_msg void OnBnClickedButton17();
	// 线程4
	CEdit Final_Result_Control4;
	CEdit m_Result4;
	CComboBox m_Port4;
	CComboBox m_Baud4;
	CString IMEI_InputD;
	CEdit IMEI_InputD_Control;
	afx_msg void OnEnChangeImeia4();
	afx_msg void OnBnClickedButton18();
	afx_msg void OnBnClickedButton19();
	// 线程5
	CEdit Final_Result_Control5;
	CEdit m_Result5;
	CComboBox m_Port5;
	CComboBox m_Baud5;
	CString IMEI_InputE;
	CEdit IMEI_InputE_Control;
	afx_msg void OnEnChangeImeia5();
	afx_msg void OnBnClickedButton20();
	afx_msg void OnBnClickedButton21();
	// 线程6
	CEdit Final_Result_Control6;
	CEdit m_Result6;
	CComboBox m_Port6;
	CComboBox m_Baud6;
	CString IMEI_InputF;
	CEdit IMEI_InputF_Control;
	afx_msg void OnEnChangeImeia6();
	afx_msg void OnBnClickedButton22();
	afx_msg void OnBnClickedButton23();
	afx_msg void OnBnClickedButton6();
	afx_msg void OnBnClickedButton7();
	// 线程一
	CButton StartA_Control;
	// 线程二
	CButton StartB_Control;
	// 线程3
	CButton StartC_Control;
	// 线程四
	CButton StartD_Control;
	// 线程五
	CButton StartE_Control;
	// 线程六
	CButton StartF_Control;


	// 扫描枪端口
	CComboBox ScanGunPort1;
	CComboBox ScanGunPort2;
	CComboBox ScanGunPort3;
	CComboBox ScanGunPort4;
	CComboBox ScanGunPort5;
	CComboBox ScanGunPort6;
	// 扫描枪波特率
	CComboBox ScanGunBaud1;
	CComboBox ScanGunBaud2;
	CComboBox ScanGunBaud3;
	CComboBox ScanGunBaud4;
	CComboBox ScanGunBaud5;
	CComboBox ScanGunBaud6;
	// 扫描枪类型
	CComboBox ScanGunTpye;

	HANDLE hScanGun[THREAD_NUM];
	CString Vaule_ScanGun[THREAD_NUM];												//扫描枪数据
	CString OtherData_Input[THREAD_NUM];
public:
	BOOL TestCheck;																	//公用扫描枪
	CString PublicIMEI;
	BOOL CompareIMEI;																//比对IMEI
public:
	BOOL OPen_ScanGun_Port(CComboBox* m_Port,CComboBox* m_Baud,int HandleNum);		//打开扫描枪端口
	BOOL Get_ScanGun_Data(CEdit* m_Result,CEdit* Final_Result_Control,int HandleNum,char* EndSign);								//获取扫描枪端口数据
	BOOL Check_ScanGun_Data(CEdit* m_Result,CEdit* Final_Result_Control,int HandleNum,CEdit* Data_Input_Control,BOOL ImeiChoose,int LengthLimit);
	afx_msg void OnBnClickedButton3();
	afx_msg void OnBnClickedButton25();
	afx_msg void OnBnClickedButton26();
	afx_msg void OnBnClickedButton27();
	afx_msg void OnBnClickedButton28();
	afx_msg void OnBnClickedButton29();
	afx_msg void OnBnClickedButton30();
	afx_msg void OnBnClickedButton31();
	afx_msg void OnBnClickedButton32();
	afx_msg void OnBnClickedButton33();
	afx_msg void OnBnClickedButton34();
	afx_msg void OnBnClickedButton35();
public:
	CPackingPrint PrintInt;
	BOOL PrintOn;//打印做了初始化
	CString Data_FromMachine[THREAD_NUM];
	CString Data1_FromDB[THREAD_NUM];
	CString Data2_FromDB[THREAD_NUM];
	CString Data3_FromDB[THREAD_NUM];
public:
	afx_msg void OnBnClickedButton10();
	// 选择是否打印
	CComboBox PrintSelect;
	afx_msg void OnBnClickedButton37();
	afx_msg void OnBnClickedButton38();
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedCancel();
	afx_msg void OnBnClickedButton40();
	// 串口是否为稳定端口
	CComboBox PortType;
	afx_msg void OnBnClickedButton42();

public:
	CString MachineType_CS;				//机型名称
	CString ZhiDanCS;					//制单
	CString ReturnCodeCS;				//返工编码
public:
	long TestTimeStart[THREAD_NUM];			//开始接通测试时间
	CFile cReport[THREAD_NUM];
	CString IMEIRecord[THREAD_NUM];			//IMEI
	CString ChipRfID[THREAD_NUM];
	CString Software_Version[THREAD_NUM];	//软件版本
	CString Software_VersionFull;
	CString BTMac[THREAD_NUM];				//蓝牙地址
	CString BTTestResult[THREAD_NUM];		//蓝牙测试结果
	CString SimNumber[THREAD_NUM];			//SIM卡号
	CString NteTemperature[THREAD_NUM];		//温度
	CString GPSSnrAve[THREAD_NUM];			//GPS平均SNR
	CString InchargeI[THREAD_NUM];			//充电电流
public:
	void WriteLogIni(int LogNum,CString ChipRfIDbg="");//写LOG信息
	void WriteLog(CString Text,int	  LogNum);
	void WriteLogEnd(int LogNum);
public:
	// 最终结果
	CEdit Final_Result_Control7;
	CEdit Final_Result_Control8;
	CEdit Final_Result_Control9;
	CEdit Final_Result_Control10;
	CEdit Final_Result_Control11;
	CEdit Final_Result_Control12;
	CEdit Final_Result_Control13;
	CEdit Final_Result_Control14;
	CEdit Final_Result_Control15;
	CEdit Final_Result_Control16;
	CEdit m_Result7;
	CEdit m_Result8;
	CEdit m_Result9;
	CEdit m_Result10;
	CEdit m_Result11;
	CEdit m_Result12;
	CEdit m_Result13;
	CEdit m_Result14;
	CEdit m_Result15;
	CEdit m_Result16;
	CComboBox m_Port7;
	CComboBox m_Port8;
	CComboBox m_Port9;
	CComboBox m_Port10;
	CComboBox m_Port11;
	CComboBox m_Port12;
	CComboBox m_Port13;
	CComboBox m_Port14;
	CComboBox m_Port15;
	CComboBox m_Port16;
	CComboBox m_Baud7;
	CComboBox m_Baud8;
	CComboBox m_Baud9;
	CComboBox m_Baud10;
	CComboBox m_Baud11;
	CComboBox m_Baud12;
	CComboBox m_Baud13;
	CComboBox m_Baud14;
	CComboBox m_Baud15;
	CComboBox m_Baud16;
	CButton Start7_Control;
	CButton Start8_Control;
	CButton Start9_Control;
	CButton Start10_Control;
	CButton Start11_Control;
	CButton Start12_Control;
	CButton Start13_Control;
	CButton Start14_Control;
	CButton Start15_Control;
	CButton Start16_Control;
	CButton StartALL_Control;
	CButton StopALL_Control;
	afx_msg void OnBnClickedButtonstart7();
	afx_msg void OnBnClickedButtonstop7();
	afx_msg void OnBnClickedButtonstart8();
	afx_msg void OnBnClickedButtonstart9();
	afx_msg void OnBnClickedButtonstart10();
	afx_msg void OnBnClickedButtonstart11();
	afx_msg void OnBnClickedButtonstart12();
	afx_msg void OnBnClickedButtonstart13();
	afx_msg void OnBnClickedButtonstart14();
	afx_msg void OnBnClickedButtonstart15();
	afx_msg void OnBnClickedButtonstart16();
	afx_msg void OnBnClickedButtonstop8();
	afx_msg void OnBnClickedButtonstop9();
	afx_msg void OnBnClickedButtonstop10();
	afx_msg void OnBnClickedButtonstop11();
	afx_msg void OnBnClickedButtonstop12();
	afx_msg void OnBnClickedButtonstop13();
	afx_msg void OnBnClickedButtonstop14();
	afx_msg void OnBnClickedButtonstop15();
	afx_msg void OnBnClickedButtonstop16();
public:
	//语音播报
	HRESULT hr;
	ISpVoice*  pVoice;
	void Voice_Ini();
	void Voice_Speak(CString Text);
public:

public:
	BOOL GetExistReturnCode(CAdoInterface& myado,int DataUpNum,CEdit* m_Result,CEdit* Final_Result_Control,CString *ExistMessage,CString ChipRfIDbg="");
	BOOL Data_UpdatePara(CAdoInterface& myado,int DataUpNum,CEdit* m_Result,CEdit* Final_Result_Control,BOOL ErrorUpEnable=TRUE,CString ChipRfIDbg="");
	BOOL Data_UpdatePara2(CAdoInterface& myado,int DataUpNum,CEdit* m_Result,CEdit* Final_Result_Control,BOOL ErrorUpEnable=TRUE,CString ChipRfIDbg="");			//SMT测试
	BOOL Data_UpdateParaPre(CAdoInterface& myado,int DataUpNum,CEdit* m_Result,CEdit* Final_Result_Control,BOOL* smt_Check);
	BOOL Data_UpdateError(CAdoInterface& myado,int DataUpNum,CEdit* m_Result,CEdit* Final_Result_Control,CString Message,CString Category,CString ChipRfIDbg="");
	BOOL GetExistError(CAdoInterface& myado,int DataUpNum,CEdit* m_Result,CEdit* Final_Result_Control,CString *ExistMessage,CString ChipRfIDbg="");
	// 单片机控制
public:
	BOOL CPUCommunication(CString CpuOrderName,int HandleNum,CString EndSign,CString StartSign,BOOL Enable,CEdit* m_Result=NULL,CEdit* Final_Result_Control=NULL);
public:
	HINSTANCE password_Handle;
public:
	void  SaveConfig_Change();
	void  LoadConfig_Change();

	unsigned short (WINAPI* GetNEWTTL_encrypt_16)(const unsigned char* pData, int nLength);
	bool  LoadDll();

	char HexToASCII(unsigned char  data_hex);
	void HexGroupToString(char *OutStrBuffer, unsigned char *InHexBuffer, unsigned int HexLength);//用于显示串口返回值

	BOOL StringToHexGroup(unsigned char *OutHexBuffer, char *InStrBuffer, unsigned int strLength);//string转HEX "12AB"--->0x12 0xab
	unsigned int SeperateToHex(const char *src, unsigned char *des);							  //"12 AB"--->0x120xAB--->兼容空格
	CString  ShowPassWord(CString InputID,CEdit* m_Result,CEdit* Final_Result_Control,int HandleNum);

	char* AscToHexString(char* pInData, char* pOutData);											//"ab01234" -> "61 62 30 31 32 33 34 "
public:
	CComboBox CPUControlPort;
	CComboBox CPUControlBaud;
	afx_msg void OnBnClickedButtonconncpu();
	afx_msg void OnBnClickedButtondisconncpu();
	// 订单号
	CComboBox OrderNumbersControl;
	CComboBox GPS_SNRLimit;
	CString GPS_SNRLimitCS;
	// 成功失败计数
	int SuccessCount;
	int FailCount;
	// 返工编码
	CComboBox ReturnCodeControl;
	// 电压电流表
	CString SimulationCurrent[THREAD_NUM];
	CComboBox VIControlPort;
	CComboBox VIControlBaud;
	afx_msg void OnBnClickedButtonconncpu2();
	afx_msg void OnBnClickedButtondisconncpu2();
	// 工站选择
	CComboBox WorkStationControl;
	CString WorkStationCS;
public:
	CString	Hostname;
	CString Ipaddress;
	BOOL GetIp_Name(CString& m_hostname,CString& m_ipaddress);
	// 检查SMT是否漏测
	BOOL CheckSMTChoose;
	afx_msg void OnBnClickedCheck39();
	BOOL RTestChoose;
	afx_msg void OnBnClickedCheck40();
	afx_msg void OnCbnSelchangeCombo53();
public:
	void OnBnClickedBtnCopy();
	void FileCopy();
	DWORD CopyFileExLBC(CString strSrcPath, CString strDestPath, CString strExt);
public:
	HANDLE hPicPort[THREAD_NUM];
	CString Vaule_Pic[THREAD_NUM];												//图像数据

	BOOL DayTimeState[THREAD_NUM];												//FALSE夜视,TRUE白天
	CString PicPath[2][THREAD_NUM];												//0夜视,1白天

	CEvent DoorWait[THREAD_NUM];
	BOOL ResetButtonWIFI[THREAD_NUM];											//按钮WIFI信号
	BOOL PicUnionOK[THREAD_NUM];												//图片合成结果

	CEvent SerialPic[THREAD_NUM];
	BOOL SerialPicOver[THREAD_NUM];

	int TimerHandleNum;//定时器显示图片的线程Number
	CThumbnailBoxDlg Picdlg[THREAD_NUM];
	CString UnionPicPath[THREAD_NUM];
public:
	//图片数据
	BOOL GetPicFunction_Thread(int HandleNum,CEdit* m_Result,CEdit* Final_Result_Control,CThumbnailBoxDlg* picdlg,CString ChipRfIDbg,BOOL DayTimeStatep);
	void StartGetPic(int HandleNum,CEdit* m_Result,CEdit* Final_Result_Control);

	BOOL ShowPicFunction_Thread(CString RecString,int HandleNum,CEdit* m_Result,CEdit* Final_Result_Control,CThumbnailBoxDlg* picdlg,CString ChipRfIDbg,BOOL DayTimeStatep);
	void StartShowPic(int HandleNum,CEdit* m_Result,CEdit* Final_Result_Control,CString RecString);
	BOOL Show_Pic_Data(CString RecString,CEdit* m_Result,CEdit* Final_Result_Control,CThumbnailBoxDlg* picdlg,int HandleNum,CString StartSign,CString EndSign,CString ChipRfIDbg,BOOL DayTimeStatep);		//获取图像端口数据

	BOOL OPen_Pic_Port(CComboBox* m_Port,CComboBox* m_Baud,int HandleNum);		//打开图像端口
	BOOL Get_Pic_Data(CEdit* m_Result,CEdit* Final_Result_Control,CThumbnailBoxDlg* picdlg,int HandleNum,CString StartSign,CString EndSign,CString ChipRfIDbg,BOOL DayTimeStatep);		//获取图像端口数据

	BOOL Get_Wifi_Data(CEdit* m_Result,CEdit* Final_Result_Control,int HandleNum,CString StartSign,CString EndSign,BOOL InThread=FALSE);	//获取按钮WIFI数据

	BOOL HEX2JPG();
	BOOL HEX2JPG2();

	BYTE ConvertHexChar(BYTE ch);
	BYTE * StrDecToCHex(int &length, CString str);

	void JPG2HEX();
	BOOL HEX2JPG3(CString str,CString& m_sPath,CString NightDay,int HandleNum,CString ChipRfIDbg="");
	BOOL GetPicChoose;
	//图片合成
	BOOL Jpgunionjpg(CString szPathName1,CString szPathName2,CString strInfo,CString PicName,CString& strDestJpgName);
public:
	ULONG_PTR m_GdiplusToken;
	GdiplusStartupInput m_GdiplusStartupInput;
public:	
	afx_msg void OnBnClickedCheck41();
	// 后台确认
	BOOL BGConfirmChoose;
	CString BGShowPicTime[THREAD_NUM];//图片后台显示时间
	int BGShowPicCount[THREAD_NUM];//图片后台显示次数

	BOOL TestResult[THREAD_NUM];//测试线程的结果
	afx_msg void OnBnClickedCheck42();
	BOOL GPSDataTypeChoose;
	CComboBox WIFI_RSSILimit;
	CString WIFI_RSSILimitCS;
	afx_msg void OnBnClickedCheck43();
	afx_msg void OnCbnSelchangeCombo56();
public:
	BOOL RecoverVoltage;
	// 图片静态显示
	BOOL PicStaticChoose;




	//三合一新增功能
	

	/*进程通讯相关的*/
	protected:
		afx_msg LRESULT MSG_GetSimpleMessage(WPARAM wParam, LPARAM lParam);//获取自定义消息用的系统消息函数
		afx_msg BOOL OnCopyData(CWnd* pWnd, COPYDATASTRUCT* pCopyDataStruct);//用来发送结构体的
	public:
		CString m_SoftVersion;//软件版本，在网络获取函数中使用的
		int m_bVar;//消息传送时回复的值，在网络获取函数中使用的

		static map<CString, int>PortStatusMap;//此map容器与StopSign数组联动，键放的是串口号，值放的是StopSign数组对应的下标，利用此函数便可以快速通过串口号来判断端口是否处于占用

		void MSG_SendSimpleMessage();//自定义消息发送函数,发送句柄给总控程序
		void MSG_SendCopyDataMessage(CopyDataMSG *MessageStruct,int replyInt);//利用CopyData来发送结构体

		void ADCTInquire(CString PortNo,INT CommandNo);//ADCT进行查询时
		void ADCTSetup(CString PortNo, INT CommandNo,CString message);//ADCT进行设置时
		void SendADCTTestResult(CString PortNo, CString message);//给ADCT主控程序发送测试结果
		void SendADCTHint(CString PortNo, CString message,int MessageType);//给ADCT主控程序发送提示信息,MessageType,0是提示信息，1是正常信息日志，2是错误信息，3是弹出框
		afx_msg void OnClose();;//程序正常退出
		//LONG WINAPI FreeEIM_UnhandledExceptionFilter(LPEXCEPTION_POINTERS ExceptionInfo); //程序异常退出
		void AfxMessageBoxHint(CString str);//因为三合一的关系，所以要用它来代替程序原本的弹出框

		int InitSetting(CString Zhidan, CString Version);//实现自动获取配置
		int StartPortTest(CString PortNo);//实现串口开始
		int StopPortTest(CString PortNo);//实现串口关闭
		


	/*整机测试新增功能*/

	public:
		int OrderNumber;//蓝牙指令条数
		int MacIntercept;//MAC地址取后几位
		int OrderCount;//蓝牙指令发送次数
		int ExitOrderNumber;//蓝牙断开指令条数
		CString BleOrder[10][2];//存放蓝牙指令
		CString ExitOrder[10][2];//存放蓝牙断开指令
		int BleOrderCount[10];//存放蓝牙指令延迟时间
		int BleGetSettingFlag[16];//为了让串口被关掉时连接函数也被关掉，特设此标志位
		CString BleMacAddress[16];//存放蓝牙Mac地址的
		deque<int> ComFreeDeq;//存放空闲的串口
		deque<int> ComoOccupancydDeq;//存放被占用的串口
		CWinThread* BLESetting_Thread_Handle;//蓝牙线程

		int BLEGetSettingFlag;//这个是用来卡获取配置用的标志位，当串口扫描枪扫入MAC地址，并建立起蓝牙连接后才可以继续获取配置，-1表示还没获取配置，0代表正在获取配置，2表示获得MAC但是连接失败，然后就会直接获取配置失败，1表示连接成功可以继续获取配置
		BOOL MacScanEnbleFlag;//Mac地址是否允许扫描标志位，如果为FALSE，表示此时正在进行蓝牙连接，不允许扫入，反之，则允许扫入并连接此蓝牙地址
		BOOL BLEGetSettingEndFlag;//这个是配置是否已经获取完成的标志位

		BOOL GetBluetoothCheckValue;//蓝牙复选框变量


		afx_msg void OnBnClickedBluetoothCheck();//蓝牙复选框点击函数
		void GetBleOrder();//获取蓝牙配置文件里的指令同时初始化整机测试的变量
		void BleStartPortTest(CString MACStr);//根据ComFreeDeq队列的第一位来开始某个端口
		void BleStopPortTest(int PortNo);//因为是通过扫描枪扫描，然后分配MAC给串口，所以无论成功还是失败之后都要停止该串口
		void DeleteComoOccupancydDeq(int PortNo);//从队列中删除已经测试完的端口号
		void BluetoothCoreFunction(CString MACStr);//蓝牙整机测试核心函数，里面包括获取配置时的蓝牙连接和开始测试时的蓝牙连接
		int BluetoothConnect(int PortNo, CString MACStr, CComboBox* m_Port, CComboBox* m_Baud);//蓝牙连接函数
		void BluetoothHint(int PortNo, CString str);//整机测试串口提示用的函数
		void BluetoothDisconnect(int PortNo);//蓝牙断开函数

};
