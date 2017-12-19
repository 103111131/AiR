using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/* 若Example.cs已存在，就不需要宣告這部份
public enum BroadcastMode {
	send	= 0,
	receive	= 1,
	unknown = 2
}
public enum BroadcastState {
	inactive = 0,
	active	 = 1
}*/

//internal class BReceiver_four : MonoBehaviour {
public class BReceiver_four : MonoBehaviour {
	//--------------------------------------------------
	private string ck_Region = "com.Cowabunga.Test"; //區域名（共通）
	private string ck_UUID = "2B0798A3-E752-423B-BBFD-A5513823DEAA"; //主要ID（共通）
	private int ck_Major = 10; //大分類（共通）
	private int ck_Minor1 = 0; //小分類（Beacon的主要識別號）
	private int ck_Minor2 = 0;
	private int ck_Minor3 = 0;
	private int ck_Minor4 = 0;
	private int myStr1 = -59; // 1m時的強度（此為無輸入時的預設值）
	private int myStr2 = -59;
	private int myStr3 = -59;
	private int myStr4 = -59;
	private double envfac; // 環境因子
	private double sheight; // 設置高度
	private double lheight; // 最低高度
	private int Qlimit = 15;
	private double pos12; // Beacon之間相對擺放距離(m)
	private double pos23;
	//private double pos34;
	//private double pos14;
	private double MB1; // Beacon與手機的距離(m)
	private double MB2;
	private double MB3;
	private double MB4;
	private double MB1b; // 前一次計算結果
	private double MB2b;
	private double MB3b;
	private double MB4b;

	private double CDbx;
	private double CDby;
	private float CDN_x;
	private float CDN_y;
	private float CDN_z;

	private float t_stamp;
	private float tps;

	private Queue<int> BCarr1rssi = new Queue<int>();
	private Queue<int> BCarr2rssi = new Queue<int>();
	private Queue<int> BCarr3rssi = new Queue<int>();
	private Queue<int> BCarr4rssi = new Queue<int>();
	private Queue<double> BCarr1meter = new Queue<double>();
	private Queue<double> BCarr2meter = new Queue<double>();
	private Queue<double> BCarr3meter = new Queue<double>();
	private Queue<double> BCarr4meter = new Queue<double>();

	private double BCave1rssi;
	private double BCave1meter;
	private double BCave2rssi;
	private double BCave2meter;
	private double BCave3rssi;
	private double BCave3meter;
	private double BCave4rssi;
	private double BCave4meter;

	private Text _txtBC1rssi;
	private Text _txtBC2rssi;
	private Text _txtBC3rssi;
	private Text _txtBC4rssi;
	private Text _txtBC1meter;
	private Text _txtBC2meter;
	private Text _txtBC3meter;
	private Text _txtBC4meter;
	private Text _POSx;
	private Text _POSy;
	private Text _POSz;

	//--------------------
	private Text _DEBUG1;
	private Text _DEBUG2;
	private Text _DEBUG3;
	//--------------------

	private Button _ssbutton;

	private InputField _B1name;
	private InputField _B2name;
	private InputField _B3name;
	private InputField _B4name;
	private InputField _B1minor;
	private InputField _B2minor;
	private InputField _B3minor;
	private InputField _B4minor;

	private InputField _POS12;
	private InputField _POS23;
	//private InputField _POS34;
	//private InputField _POS14;

	private InputField _IFB1str;
	private InputField _IFB2str;
	private InputField _IFB3str;
	private InputField _IFB4str;
	private InputField _IFEV;
	private InputField _IFHT;
	//--------------------------------------------------

	/*** Beacon Properties ***/
	private string s_Region;
	private string s_UUID;
	private string s_Major;
	private string s_Minor;

	/** Input **/
	private BeaconType bt_Type;
	private BroadcastMode bm_Mode;
	private BroadcastState bs_State;
	// Receive
	//private List<Beacon> mybeacons = new List<Beacon>();

	// 無介面純啟動版不需要Start
	/*private void Start() {
		//--------------------------------------------------
		_txtBC1rssi = GameObject.Find("Canvas/Panel_4Beacon/B1_rssi").GetComponent<Text>();
		_txtBC2rssi = GameObject.Find("Canvas/Panel_4Beacon/B2_rssi").GetComponent<Text>();
		_txtBC3rssi = GameObject.Find("Canvas/Panel_4Beacon/B3_rssi").GetComponent<Text>();
		_txtBC4rssi = GameObject.Find("Canvas/Panel_4Beacon/B4_rssi").GetComponent<Text>();
		_txtBC1meter = GameObject.Find("Canvas/Panel_4Beacon/B1_meter").GetComponent<Text>();
		_txtBC2meter = GameObject.Find("Canvas/Panel_4Beacon/B2_meter").GetComponent<Text>();
		_txtBC3meter = GameObject.Find("Canvas/Panel_4Beacon/B3_meter").GetComponent<Text>();
		_txtBC4meter = GameObject.Find("Canvas/Panel_4Beacon/B4_meter").GetComponent<Text>();
		_POSx = GameObject.Find("Canvas/Panel_4Beacon/POS_X").GetComponent<Text>();
		_POSy = GameObject.Find("Canvas/Panel_4Beacon/POS_Y").GetComponent<Text>();
		_POSz = GameObject.Find("Canvas/Panel_4Beacon/POS_Z").GetComponent<Text>();

		//--------------------
		_DEBUG1 = GameObject.Find("Canvas/Panel_4Beacon/DEBUG1").GetComponent<Text>();
		_DEBUG2 = GameObject.Find("Canvas/Panel_4Beacon/DEBUG2").GetComponent<Text>();
		_DEBUG3 = GameObject.Find("Canvas/Panel_4Beacon/DEBUG3").GetComponent<Text>();
		//--------------------

		_ssbutton = GameObject.Find("Canvas/Panel_4Beacon/Button").GetComponent<Button>();

		_B1name = GameObject.Find("Canvas/Panel_4Beacon/B1_name").GetComponent<InputField>();
		_B2name = GameObject.Find("Canvas/Panel_4Beacon/B2_name").GetComponent<InputField>();
		_B3name = GameObject.Find("Canvas/Panel_4Beacon/B3_name").GetComponent<InputField>();
		_B4name = GameObject.Find("Canvas/Panel_4Beacon/B4_name").GetComponent<InputField>();
		_B1minor = GameObject.Find("Canvas/Panel_4Beacon/B1_minor").GetComponent<InputField>();
		_B2minor = GameObject.Find("Canvas/Panel_4Beacon/B2_minor").GetComponent<InputField>();
		_B3minor = GameObject.Find("Canvas/Panel_4Beacon/B3_minor").GetComponent<InputField>();
		_B4minor = GameObject.Find("Canvas/Panel_4Beacon/B4_minor").GetComponent<InputField>();

		_POS12 = GameObject.Find("Canvas/Panel_4Beacon/POS_B1B2").GetComponent<InputField>();
		_POS23 = GameObject.Find("Canvas/Panel_4Beacon/POS_B2B3").GetComponent<InputField>();
		//_POS34 = GameObject.Find("Canvas/Panel_4Beacon/POS_B3B4").GetComponent<InputField>();
		//_POS14 = GameObject.Find("Canvas/Panel_4Beacon/POS_B1B4").GetComponent<InputField>();

		_IFB1str = GameObject.Find("Canvas/Panel_4Beacon/IF_B1str").GetComponent<InputField>();
		_IFB2str = GameObject.Find("Canvas/Panel_4Beacon/IF_B2str").GetComponent<InputField>();
		_IFB3str = GameObject.Find("Canvas/Panel_4Beacon/IF_B3str").GetComponent<InputField>();
		_IFB4str = GameObject.Find("Canvas/Panel_4Beacon/IF_B4str").GetComponent<InputField>();
		_IFEV = GameObject.Find("Canvas/Panel_4Beacon/IF_EV").GetComponent<InputField>();
		_IFHT = GameObject.Find("Canvas/Panel_4Beacon/IF_HT").GetComponent<InputField>();
		//--------------------------------------------------
		setBeaconPropertiesAtStart(); // 初始設定

		// 藍芽狀態偵測（目前未使用）
		BluetoothState.BluetoothStateChangedEvent += delegate(BluetoothLowEnergyState state) {
			switch (state) {
			case BluetoothLowEnergyState.TURNING_OFF:
			case BluetoothLowEnergyState.TURNING_ON:
				break;
			case BluetoothLowEnergyState.UNKNOWN:
			case BluetoothLowEnergyState.RESETTING:
				//SwitchToStatus();
				//_statusText.text = "Checking Device…";
				break;
			case BluetoothLowEnergyState.UNAUTHORIZED:
				//SwitchToStatus();
				//_statusText.text = "You don't have the permission to use beacons.";
				break;
			case BluetoothLowEnergyState.UNSUPPORTED:
				//SwitchToStatus();
				//_statusText.text = "Your device doesn't support beacons.";
				break;
			case BluetoothLowEnergyState.POWERED_OFF:
				//SwitchToMenu();
				//_bluetoothButton.interactable = true;
				//_bluetoothText.text = "Enable Bluetooth";
				break;
			case BluetoothLowEnergyState.POWERED_ON:
				//SwitchToMenu();
				//_bluetoothButton.interactable = false;
				//_bluetoothText.text = "Bluetooth already enabled";
				break;
			default:
				//SwitchToStatus();
				//_statusText.text = "Unknown Error";
				break;
			}
		};
		BluetoothState.Init();
	}*/

	private void setBeaconPropertiesAtStart() {
		//RestorePlayerPrefs();				//載入前次紀錄
		// 若iBeaconServer或Receiver有設定，就優先讀取（目前未使用）
		/*if (iBeaconServer.region.regionName != "") {
			Debug.Log("check iBeaconServer-inspector");
			s_Region = iBeaconServer.region.regionName;
			bt_Type 	= iBeaconServer.region.beacon.type;
			if (bt_Type == BeaconType.EddystoneURL) {
				s_UUID = iBeaconServer.region.beacon.UUID;
			} else if (bt_Type == BeaconType.EddystoneUID) {
				s_UUID = iBeaconServer.region.beacon.UUID;
				s_Major = iBeaconServer.region.beacon.instance;
			} else if (bt_Type == BeaconType.iBeacon) {
				s_UUID = iBeaconServer.region.beacon.UUID;
				s_Major = iBeaconServer.region.beacon.major.ToString();
				s_Minor = iBeaconServer.region.beacon.minor.ToString();
			}
		} else if (iBeaconReceiver.regions.Length != 0) {
			Debug.Log("check iBeaconReceiver-inspector");
			s_Region	= iBeaconReceiver.regions[0].regionName;
			bt_Type 	= iBeaconReceiver.regions[0].beacon.type;
			if (bt_Type == BeaconType.EddystoneURL) {
				s_UUID = iBeaconReceiver.regions[0].beacon.UUID;
			} else if (bt_Type == BeaconType.EddystoneUID) {
				s_UUID = iBeaconReceiver.regions[0].beacon.UUID;
				s_Major = iBeaconReceiver.regions[0].beacon.instance;
			} else if (bt_Type == BeaconType.iBeacon) {
				s_UUID = iBeaconReceiver.regions[0].beacon.UUID;
				s_Major = iBeaconReceiver.regions[0].beacon.major.ToString();
				s_Minor = iBeaconReceiver.regions[0].beacon.minor.ToString();
			} 
		}*/
	}

	// 啟動訊號掃描
	public void btn_StartStop() {
		//Debug.Log ("Button Pushed");
		//createobj.set_bt_position (10,15,20);
		StartStates(); //開始掃描時讀入輸入的參數
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.regions = new iBeaconRegion[]{new iBeaconRegion(s_Region, new Beacon())};
		iBeaconReceiver.Scan();
		//bs_State = BroadcastState.active;
	}

	private void StartStates() {
		s_Region = ck_Region;				//讀入設定的區域名
		ck_UUID = ck_UUID.ToLower();
		s_UUID = ck_UUID;					//讀入設定的UUID
		bm_Mode = BroadcastMode.receive;	//廣播模式設定為接收
		bt_Type = BeaconType.Any;			//Beacon模式設定為任意模式
		bs_State = BroadcastState.inactive;	//將廣播狀態設為未啟動

		ck_Minor1 = 1001;
		ck_Minor2 = 1002;
		ck_Minor3 = 1003;
		ck_Minor4 = 1004;
		/*ck_Minor1 = 22720;
		ck_Minor2 = 31168;
		ck_Minor3 = 64703;
		ck_Minor4 = 59583;*/
		myStr1 = -65;
		myStr2 = -62;
		myStr3 = -57;
		myStr4 = -59;
		envfac = 2.0;
		sheight = 2.0;
		lheight = 0.5;
		pos12 = 4.4;
		pos23 = 9.6;
		tps = 1.1f;

		/*if(_B1minor.text != "")
			ck_Minor1 = Convert.ToInt32(_B1minor.text);
		if(_B2minor.text != "")
			ck_Minor2 = Convert.ToInt32(_B2minor.text);
		if(_B3minor.text != "")
			ck_Minor3 = Convert.ToInt32(_B3minor.text);
		if(_B4minor.text != "")
			ck_Minor4 = Convert.ToInt32(_B4minor.text);
		if(_IFB1str.text != "")
			myStr1 = Convert.ToInt32(_IFB1str.text);
		if(_IFB2str.text != "")
			myStr2 = Convert.ToInt32(_IFB2str.text);
		if(_IFB3str.text != "")
			myStr3 = Convert.ToInt32(_IFB3str.text);
		if(_IFB4str.text != "")
			myStr4 = Convert.ToInt32(_IFB4str.text);
		if(_IFEV.text != "")
			envfac = Convert.ToDouble(_IFEV.text);
		if(_IFHT.text != "")
			sheight = Convert.ToDouble(_IFHT.text);
		if(_POS12.text != "")
			pos12 = Convert.ToDouble(_POS12.text);
		else
			pos12 = 0.0;
		if(_POS23.text != "")
			pos23 = Convert.ToDouble(_POS23.text);
		else
			pos23 = 0.0;*/
		/*if(_POS34.text != "")
		pos34 = Convert.ToDouble(_POS34.text);
		else
			pos34 = 0.0;
		if(_POS14.text != "")
			pos14 = Convert.ToDouble(_POS14.text);
		else
			pos14 = 0.0;*/
		//四顆版面積
		//HF_area = pos12 * pos23;
	}

	private double RssiToMeter(double rssi, int mySTR) {
		double iRSSI = Math.Abs(rssi);
		double power = (iRSSI + mySTR)/(10 * envfac);
		return Math.Pow(10, power);
	}

	private void OnBeaconRangeChanged(Beacon[] beacons) { 
		foreach (Beacon b in beacons) {
			if (createobj.get_Accelerometer_bool ()) {
				BCarr1rssi.Clear();
				BCarr2rssi.Clear();
				BCarr3rssi.Clear();
				BCarr4rssi.Clear();
			}
			//--------------------------------------------------
			if(b.UUID == ck_UUID && b.major == ck_Major) {
				if(b.minor == ck_Minor1) {
					if(BCarr1rssi.Count < Qlimit) {
						BCarr1rssi.Enqueue(b.rssi);
					}
					else {
						BCarr1rssi.Dequeue();
						BCarr1rssi.Enqueue(b.rssi);
					}
					double BC1ct = BCarr1rssi.Count;
					double AveTemp1 = 0.0;
					foreach (double queue in BCarr1rssi)
					{
						AveTemp1 += queue;
					}
					BCave1rssi = AveTemp1 / BC1ct;
					MB1 = RssiToMeter(BCave1rssi, myStr1);
				}
				else if(b.minor == ck_Minor2) {
					if(BCarr2rssi.Count < Qlimit) {
						BCarr2rssi.Enqueue(b.rssi);
					}
					else {
						BCarr2rssi.Dequeue();
						BCarr2rssi.Enqueue(b.rssi);
					}
					double BC2ct = BCarr2rssi.Count;
					double AveTemp2 = 0.0;
					foreach (double queue in BCarr2rssi)
					{
						AveTemp2 += queue;
					}
					BCave2rssi = AveTemp2 / BC2ct;
					MB2 = RssiToMeter(BCave2rssi, myStr2);
				}
				else if(b.minor == ck_Minor3) {
					if(BCarr3rssi.Count < Qlimit) {
						BCarr3rssi.Enqueue(b.rssi);
					}
					else {
						BCarr3rssi.Dequeue();
						BCarr3rssi.Enqueue(b.rssi);
					}
					double BC3ct = BCarr3rssi.Count;
					double AveTemp3 = 0.0;
					foreach (double queue in BCarr3rssi)
					{
						AveTemp3 += queue;
					}
					BCave3rssi = AveTemp3 / BC3ct;
					MB3 = RssiToMeter(BCave3rssi, myStr3);
				}
				else if(b.minor == ck_Minor4) {
					if(BCarr4rssi.Count < Qlimit) {
						BCarr4rssi.Enqueue(b.rssi);
					}
					else {
						BCarr4rssi.Dequeue();
						BCarr4rssi.Enqueue(b.rssi);
					}
					double BC4ct = BCarr4rssi.Count;
					double AveTemp4 = 0.0;
					foreach (double queue in BCarr4rssi)
					{
						AveTemp4 += queue;
					}
					BCave4rssi = AveTemp4 / BC4ct;
					MB4 = RssiToMeter(BCave4rssi, myStr4);
				}
				//Beacon內部其他可用參數
				//b.range.ToString();
				//b.strength.ToString(); //(db)
				//b.rssi.ToString(); //(db)
				//b.accuracy.ToString(); //(m)
			}
		}
		// 如果四值的結果至少其一跟前次不同
		/*if(MB1 != MB1b || MB2 != MB2b || MB3 != MB3b || MB4 != MB4b) {
			MB1b = MB1;
			MB2b = MB2;
			MB3b = MB3;
			MB4b = MB4;
			if(MB1 > 0 && MB2 > 0 && MB3 > 0 && MB4 > 0 && pos12 > 0 && pos23 > 0) {
			BeaconToCoordinate(MB1, MB2, MB3, MB4);
			//----------輸出給createobj----------
			createobj.set_bt_position (CDN_x,CDN_z,CDN_y);
			//------------------------------
			}
		}
		else {
			//----------輸出給createobj----------
			createobj.set_bt_position (CDN_x,CDN_z,CDN_y);
			//------------------------------
		}*/
		// Beacon距離都不為0且有輸入值
		if(MB1 > 0 && MB2 > 0 && MB3 > 0 && MB4 > 0 && pos12 > 0 && pos23 > 0) {
			BeaconToCoordinate(MB1, MB2, MB3, MB4);
			//----------輸出給createobj----------
			createobj.set_bt_position (CDN_x,CDN_z,CDN_y);
			t_stamp = float.Parse(DateTime.Now.ToString ("ss.fff"));
			createobj.set_time_stamp (t_stamp, tps);
			//------------------------------
		}
		/**/
	}
	/*void Start(){
	}
	void Update(){
		
	}
	public void TimeStamp(){
		float test_x = float.Parse(DateTime.Now.ToString ("ss.fff"));
		if (t_stamp != 0) {
			createobj.set_bt_position (test_x - t_stamp, 1, 1);
		}
		t_stamp = test_x;
	}*/
	// 由三角形三邊求出比例距離（參數一：底面、參數二：作為距離基準的斜邊）
	private double heron_h(double t1, double t2, double t3) {
		double hfs = (t1 + t2 + t3) / 2;
		if (hfs <= t1 || hfs <= t2 || hfs <= t3) {
			if (t2 >= t1 && t3 >= t1) {
				while (hfs <= t1 || hfs <= t2 || hfs <= t3) {
					t2 -= 0.1;
					t3 -= 0.1;
					hfs = (t1 + t2 + t3) / 2;
				}
			} 
			else if (t2 < t1 && t3 < t1) {
				while (hfs <= t1 || hfs <= t2 || hfs <= t3) {
					t2 += 0.1;
					t3 += 0.1;
					hfs = (t1 + t2 + t3) / 2;
				}
			} 
			else {
				return 0;
				/*if (t2 > t3) {
					while (hfs <= t1 || hfs <= t2 || hfs <= t3) {
						//t2 -= 0.1;
						t3 += 0.1;
						hfs = (t1 + t2 + t3) / 2;
					}
				} 
				else {
					while (hfs <= t1 || hfs <= t2 || hfs <= t3) {
						t2 += 0.1;
						//t3 -= 0.1;
						hfs = (t1 + t2 + t3) / 2;
					}
				}*/
			}
		}
		double hfa = Math.Sqrt(hfs * (hfs - t1) * (hfs - t2) * (hfs - t3));
		double ht = hfa * 2 / t1;
		//double chk = Math.Pow(t2,2) - Math.Pow(ht,2);
		//if(chk < 0) chk = 0;
		//return Math.Sqrt(chk);
		return Math.Sqrt(Math.Pow(t2,2) - Math.Pow(ht,2));
	}

	// pos:邊長 t1:基本邊 t2:比例邊
	private double prop(double pos, double t1, double t2) {
		double pl = t1 / (t1 + t2);
		return pl * pos;
	}

	// 兩座標求距離 + Beacon距離求高
	private double co_range(double c1x, double c1y, double c2x, double c2y, double BC) {
		double x_pow = Math.Pow(c2x - c1x, 2);
		double y_pow = Math.Pow(c2y - c1y, 2);
		double chk = Math.Pow(BC, 2) - (x_pow + y_pow);
		if(chk < 0) chk = 0;
		return Math.Sqrt(chk);
	}

	private void BeaconToCoordinate(double B1, double B2, double B3, double B4) {
		//四顆版的預設座標位置：B2為原點（0,0）、B1為（0,y）、B3為（x,0）、B4為（x,y）
		CDbx = pos23;
		CDby = pos12;
		//先求角錐頂點投影點（各求兩邊取平均）

		//CDN_x = (float)((heron_h(CDbx, B2, B3) + heron_h(CDbx, B1, B4)) / 2);
		//CDN_y = (float)((heron_h(CDby, B2, B1) + heron_h(CDby, B3, B4)) / 2);
		/*float CDNx11 = (float)heron_h(CDbx, B2, B3);
		float CDNx12 = (float)heron_h(CDbx, B1, B4);
		float CDNy11 = (float)heron_h(CDbx, B2, B1);
		float CDNy12 = (float)heron_h(CDbx, B3, B4);
		float CDNx1;
		float CDNy1;

		if (CDNx11 == 0 && CDNx12 == 0) {
			CDNx1 = 0;
		} 
		else if (CDNx11 == 0) {
			CDNx1 = CDNx12;
		} 
		else if (CDNx12 == 0) {
			CDNx1 = CDNx11;
		} 
		else {
			CDNx1 = (CDNx11 + CDNx12) / 2;
		}

		if (CDNy11 == 0 && CDNy12 == 0) {
			CDNy1 = 0;
		} 
		else if (CDNy11 == 0) {
			CDNy1 = CDNy12;
		} 
		else if (CDNy12 == 0) {
			CDNy1 = CDNy11;
		} 
		else {
			CDNy1 = (CDNy11 + CDNy12) / 2;
		}*/

		//float CDNx2 = (float)Math.Round((prop(CDbx, B2, B3) + prop(CDbx, B1, B4)) / 2, 2);
		//float CDNy2 = (float)Math.Round((prop(CDby, B2, B1) + prop(CDby, B3, B4)) / 2, 2);
		float CDNx2 = (float)((prop(CDbx, B2, B3) + prop(CDbx, B1, B4)) / 2);
		float CDNy2 = (float)((prop(CDby, B2, B1) + prop(CDby, B3, B4)) / 2);
		CDN_x = CDNx2;
		CDN_y = CDNy2;

		/*if (CDNx1 == 0) {
			CDN_x = CDNx2;
		} 
		else {
			CDN_x = (CDNx1 + CDNx2) / 2;
		}

		if (CDNy1 == 0) {
			CDN_y = CDNy2;
		} 
		else {
			CDN_y = (CDNy1 + CDNy2) / 2;
		}*/

		CDN_x = (float)Math.Round(CDN_x, 2);
		CDN_y = (float)Math.Round(CDN_y, 2);
		//取四點算出之高，再求平均
		//暫時版
		//double verB1 = 0.75 + co_range(0, CDby, CDN_x, CDN_y, B1);
		//CDN_z = (float)Math.Round(verB1, 2);
		//
		double verB1 = sheight - co_range(0, CDby, CDN_x, CDN_y, B1);
		//if(double.IsNaN (verB1)) verB1 = 1.5;
		//else if(verB1 > sheight) verB1 = sheight;
		if(verB1 > sheight) verB1 = sheight;
		else if(verB1 < lheight) verB1 = lheight;

		double verB2 = sheight - co_range(0, 0, CDN_x, CDN_y, B2);
		//if(double.IsNaN (verB2)) verB2 = 1.5;
		//else if(verB2 > sheight) verB2 = sheight;
		if(verB2 > sheight) verB2 = sheight;
		else if(verB2 < lheight) verB2 = lheight;

		double verB3 = sheight - co_range(CDbx, 0, CDN_x, CDN_y, B3);
		//if(double.IsNaN (verB3)) verB3 = 1.5;
		//else if(verB3 > sheight) verB3 = sheight;
		if(verB3 > sheight) verB3 = sheight;
		else if(verB3 < lheight) verB3 = lheight;

		double verB4 = sheight - co_range(CDbx, CDby, CDN_x, CDN_y, B4);
		//if(double.IsNaN (verB4)) verB4 = 1.5;
		//else if(verB4 > sheight) verB4 = sheight;
		if(verB4 > sheight) verB4 = sheight;
		else if(verB4 < lheight) verB4 = lheight;

		CDN_z = (float)Math.Round((verB1 + verB2 + verB3 + verB4) / 4, 2);

		//_DEBUG1.text = Convert.ToString(Math.Round(verB1,3)) + " / " + Convert.ToString(Math.Round(verB2,3)) + " / " + Convert.ToString(Math.Round(verB3,3));
		//_DEBUG2.text = Convert.ToString(Math.Round(verB4,3)) + " /  / " + Convert.ToString(Math.Round(B1,3));
		//_DEBUG3.text = Convert.ToString(Math.Round(B2,3)) + " / " + Convert.ToString(Math.Round(B3,3)) + " / " + Convert.ToString(Math.Round(B4,3));
	}
}
