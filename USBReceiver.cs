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

public class USBReceiver : MonoBehaviour {
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

	//private double HF_s; // 四顆版不用海龍公式
	//private double HF_area; // 底面積
	private double CDbx;
	private double CDby;
	private double CDN_x;
	private double CDN_y;
	private double CDN_z;

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
	private Text _txtBC1ave;
	private Text _txtBC2ave;
	private Text _txtBC3ave;
	private Text _txtBC4ave;
	/*private Text _POSx;
	private Text _POSy;
	private Text _POSz;*/

	//--------------------
	/*private Text _DEBUG1;
	private Text _DEBUG2;
	private Text _DEBUG3;*/
	//--------------------

	private Button _ssbutton;

	/*private InputField _B1name;
	private InputField _B2name;
	private InputField _B3name;
	private InputField _B4name;*/
	private InputField _B1minor;
	private InputField _B2minor;
	private InputField _B3minor;
	private InputField _B4minor;

	//private InputField _POS12;
	//private InputField _POS23;
	//private InputField _POS34;
	//private InputField _POS14;

	/*private InputField _IFB1str;
	private InputField _IFB2str;
	private InputField _IFB3str;
	private InputField _IFB4str;*/
	private InputField _IFEV;
	//private InputField _IFHT;
	//--------------------------------------------------

	private string s_Region;
	private string s_UUID;
	private string s_Major;
	private string s_Minor;

	//private BeaconType bt_PendingType;
	private BeaconType bt_Type;
	private BroadcastMode bm_Mode;
	private BroadcastState bs_State;

	private void Start() {
		//--------------------------------------------------
		_txtBC1rssi = GameObject.Find("Canvas/B1_rssi").GetComponent<Text>();
		_txtBC2rssi = GameObject.Find("Canvas/B2_rssi").GetComponent<Text>();
		_txtBC3rssi = GameObject.Find("Canvas/B3_rssi").GetComponent<Text>();
		_txtBC4rssi = GameObject.Find("Canvas/B4_rssi").GetComponent<Text>();
		_txtBC1meter = GameObject.Find("Canvas/B1_meter").GetComponent<Text>();
		_txtBC2meter = GameObject.Find("Canvas/B2_meter").GetComponent<Text>();
		_txtBC3meter = GameObject.Find("Canvas/B3_meter").GetComponent<Text>();
		_txtBC4meter = GameObject.Find("Canvas/B4_meter").GetComponent<Text>();
		_txtBC1ave = GameObject.Find("Canvas/B1_ave").GetComponent<Text>();
		_txtBC2ave = GameObject.Find("Canvas/B2_ave").GetComponent<Text>();
		_txtBC3ave = GameObject.Find("Canvas/B3_ave").GetComponent<Text>();
		_txtBC4ave = GameObject.Find("Canvas/B4_ave").GetComponent<Text>();
		/*_POSx = GameObject.Find("Canvas/POS_X").GetComponent<Text>();
		_POSy = GameObject.Find("Canvas/POS_Y").GetComponent<Text>();
		_POSz = GameObject.Find("Canvas/POS_Z").GetComponent<Text>();*/

		//--------------------
		/*_DEBUG1 = GameObject.Find("Canvas/DEBUG1").GetComponent<Text>();
		_DEBUG2 = GameObject.Find("Canvas/DEBUG2").GetComponent<Text>();
		_DEBUG3 = GameObject.Find("Canvas/DEBUG3").GetComponent<Text>();*/
		//--------------------

		_ssbutton = GameObject.Find("Canvas/Button").GetComponent<Button>();

		/*_B1name = GameObject.Find("Canvas/B1_name").GetComponent<InputField>();
		_B2name = GameObject.Find("Canvas/B2_name").GetComponent<InputField>();
		_B3name = GameObject.Find("Canvas/B3_name").GetComponent<InputField>();
		_B4name = GameObject.Find("Canvas/B4_name").GetComponent<InputField>();*/
		_B1minor = GameObject.Find("Canvas/B1_minor").GetComponent<InputField>();
		_B2minor = GameObject.Find("Canvas/B2_minor").GetComponent<InputField>();
		_B3minor = GameObject.Find("Canvas/B3_minor").GetComponent<InputField>();
		_B4minor = GameObject.Find("Canvas/B4_minor").GetComponent<InputField>();

		//_POS12 = GameObject.Find("Canvas/POS_B1B2").GetComponent<InputField>();
		//_POS23 = GameObject.Find("Canvas/POS_B2B3").GetComponent<InputField>();
		//_POS34 = GameObject.Find("Canvas/POS_B3B4").GetComponent<InputField>();
		//_POS14 = GameObject.Find("Canvas/POS_B1B4").GetComponent<InputField>();

		/*_IFB1str = GameObject.Find("Canvas/IF_B1str").GetComponent<InputField>();
		_IFB2str = GameObject.Find("Canvas/IF_B2str").GetComponent<InputField>();
		_IFB3str = GameObject.Find("Canvas/IF_B3str").GetComponent<InputField>();
		_IFB4str = GameObject.Find("Canvas/IF_B4str").GetComponent<InputField>();*/
		_IFEV = GameObject.Find("Canvas/IF_EV").GetComponent<InputField>();
		//_IFHT = GameObject.Find("Canvas/IF_HT").GetComponent<InputField>();
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
				//_statusText.text = "Checking Device…";
				break;
			case BluetoothLowEnergyState.UNAUTHORIZED:
				//_statusText.text = "You don't have the permission to use beacons.";
				break;
			case BluetoothLowEnergyState.UNSUPPORTED:
				//_statusText.text = "Your device doesn't support beacons.";
				break;
			case BluetoothLowEnergyState.POWERED_OFF:
				//_bluetoothButton.interactable = true;
				//_bluetoothText.text = "Enable Bluetooth";
				break;
			case BluetoothLowEnergyState.POWERED_ON:
				//_bluetoothButton.interactable = false;
				//_bluetoothText.text = "Bluetooth already enabled";
				break;
			default:
				//_statusText.text = "Unknown Error";
				break;
			}
		};
		BluetoothState.Init();
	}

	private void setBeaconPropertiesAtStart() {
		RestorePlayerPrefs();					//載入前次紀錄
		//----------------------------------------------------------------------------------------------------
		s_Region = ck_Region;					//讀入設定的區域名
		ck_UUID = ck_UUID.ToLower();
		s_UUID = ck_UUID;						//讀入設定的UUID
		bm_Mode = BroadcastMode.receive;		//廣播模式設定為接收
		bt_Type = BeaconType.Any;				//Beacon模式設定為所有類型
		bs_State = BroadcastState.inactive;		//將廣播狀態設為未啟動
		//----------------------------------------------------------------------------------------------------
		// 若iBeaconServer或Receiver有設定，就優先讀取（目前未使用）
		/*if (bm_Mode == BroadcastMode.unknown) { // first start
			bm_Mode = BroadcastMode.receive;
			bt_Type = BeaconType.iBeacon;
			if (iBeaconServer.region.regionName != "") {
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
			}
		}
		bs_State = BroadcastState.inactive;*/
	}

	// BroadcastState
	public void btn_StartStop() {
		//Debug.Log("Button Start / Stop pressed");
		/*** Beacon will start ***/
		if (bs_State == BroadcastState.inactive) {
			StartStates();			//開始掃描時讀入輸入的參數
			SavePlayerPrefs();		//儲存輸入的參數
			// ReceiveMode（接收模式）
			if (bm_Mode == BroadcastMode.receive) {
				iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
				// check if all mandatory propertis are filled
				if (s_Region == null || s_Region == "") {
					return;
				}
				if (bt_Type == BeaconType.Any) {
					iBeaconReceiver.regions = new iBeaconRegion[]{new iBeaconRegion(s_Region, new Beacon())};
				} else if (bt_Type == BeaconType.EddystoneEID) {
					iBeaconReceiver.regions = new iBeaconRegion[]{new iBeaconRegion(s_Region, new Beacon(BeaconType.EddystoneEID))};
				} else {
					if (s_UUID == null || s_UUID == "") {
						return;
					}
					if (bt_Type == BeaconType.iBeacon) {
						iBeaconReceiver.regions = new iBeaconRegion[]{new iBeaconRegion(s_Region, new Beacon(s_UUID, Convert.ToInt32(s_Major), Convert.ToInt32(s_Minor)))};
					} else if (bt_Type == BeaconType.EddystoneUID) {
						iBeaconReceiver.regions = new iBeaconRegion[]{new iBeaconRegion(s_Region, new Beacon(s_UUID, "")) };
					} else if (bt_Type == BeaconType.EddystoneURL) {
						iBeaconReceiver.regions = new iBeaconRegion[]{new iBeaconRegion(s_Region, new Beacon(s_UUID))};
					}
				}
				// !!! Bluetooth has to be turned on !!!
				iBeaconReceiver.Scan();
				_ssbutton.GetComponentInChildren<Text>().text = "Stop";
			}
			// SendMode（發送模式，目前不使用）
			else {
				// check if all mandatory propertis are filled
				if (s_Region == null || s_Region == "") {
					return;
				}
				if (bt_Type == BeaconType.EddystoneEID) {
				}
				if (bt_Type == BeaconType.Any) {
					iBeaconServer.region = new iBeaconRegion(s_Region, new Beacon());
				} else {
					if (s_UUID == null || s_UUID == "") {
						return;
					}
					if (bt_Type == BeaconType.EddystoneURL) {
						iBeaconServer.region = new iBeaconRegion(s_Region, new Beacon(s_UUID));
					} else {
						if (s_Major == null || s_Major == "") {
							return;
						}
						if (bt_Type == BeaconType.EddystoneUID) {
							iBeaconServer.region = new iBeaconRegion(s_Region, new Beacon(s_UUID, s_Major));
						} else if (bt_Type == BeaconType.iBeacon) {
							if (s_Minor == null || s_Minor == "") {
								return;
							}
							iBeaconServer.region = new iBeaconRegion(s_Region, new Beacon(s_UUID, Convert.ToInt32(s_Major), Convert.ToInt32(s_Minor)));
						}
					}
				}
				// !!! Bluetooth has to be turned on !!!
				iBeaconServer.Transmit();
			}
			bs_State = BroadcastState.active;
		} else {
			if (bm_Mode == BroadcastMode.receive) {// Stop for receive
				iBeaconReceiver.Stop();
				StopStates();			//停止掃描時將參數歸零
				iBeaconReceiver.BeaconRangeChangedEvent -= OnBeaconRangeChanged;
				_ssbutton.GetComponentInChildren<Text>().text = "Start";
			} else { // Stop for send
				iBeaconServer.StopTransmit();
			}
			bs_State = BroadcastState.inactive;
		}
	}

	private void StartStates() {
		if(_B1minor.text != "")
			ck_Minor1 = Convert.ToInt32(_B1minor.text);
		if(_B2minor.text != "")
			ck_Minor2 = Convert.ToInt32(_B2minor.text);
		if(_B3minor.text != "")
			ck_Minor3 = Convert.ToInt32(_B3minor.text);
		if(_B4minor.text != "")
			ck_Minor4 = Convert.ToInt32(_B4minor.text);
		/*if(_IFB1str.text != "")
			myStr1 = Convert.ToInt32(_IFB1str.text);
		if(_IFB2str.text != "")
			myStr2 = Convert.ToInt32(_IFB2str.text);
		if(_IFB3str.text != "")
			myStr3 = Convert.ToInt32(_IFB3str.text);
		if(_IFB4str.text != "")
			myStr4 = Convert.ToInt32(_IFB4str.text);*/
		if(_IFEV.text != "")
			envfac = Convert.ToDouble(_IFEV.text);
		/*if(_IFHT.text != "")
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

	private void StopStates() {
		_txtBC1rssi.text = "--";
		_txtBC2rssi.text = "--";
		_txtBC3rssi.text = "--";
		_txtBC4rssi.text = "--";
		_txtBC1meter.text = "--";
		_txtBC2meter.text = "--";
		_txtBC3meter.text = "--";
		_txtBC4meter.text = "--";
		_txtBC1ave.text = "--";
		_txtBC2ave.text = "--";
		_txtBC3ave.text = "--";
		_txtBC4ave.text = "--";
		/*_POSx.text = "--";
		_POSy.text = "--";
		_POSz.text = "--";*/
		BCarr1rssi.Clear();
		BCarr2rssi.Clear();
		BCarr3rssi.Clear();
		BCarr4rssi.Clear();
		BCarr1meter.Clear();
		BCarr2meter.Clear();
		BCarr3meter.Clear();
		BCarr4meter.Clear();
		//HF_area = 0;
	}

	private double RssiToMeter(double rssi, int mySTR) {
		double iRSSI = Math.Abs(rssi);
		double power = (iRSSI + mySTR)/(10 * envfac);
		return Math.Pow(10, power);
	}

	private void OnBeaconRangeChanged(Beacon[] beacons) { // 
		foreach (Beacon b in beacons) {
			if(b.UUID == ck_UUID && b.major == ck_Major) {
				if(b.minor == ck_Minor1) {
					if(BCarr1rssi.Count < Qlimit) {
						BCarr1rssi.Enqueue(b.rssi);
					}
					/*else {
						BCarr1rssi.Dequeue();
						BCarr1rssi.Enqueue(b.rssi);
					}*/
					double BC1ct = BCarr1rssi.Count;
					double AveTemp1 = 0.0;
					string optqueue1 = "";
					foreach (double queue in BCarr1rssi)
					{
						optqueue1 += Convert.ToString(queue) + ", ";
						AveTemp1 += queue;
					}
					_txtBC1rssi.text = optqueue1;
					BCave1rssi = AveTemp1 / BC1ct;
					_txtBC1ave.text = Convert.ToString(Math.Round(BCave1rssi, 2));
					MB1 = RssiToMeter(BCave1rssi, myStr1);
					_txtBC1meter.text = Convert.ToString(Math.Round(MB1, 3));
				}
				else if(b.minor == ck_Minor2) {
					if(BCarr2rssi.Count < Qlimit) {
						BCarr2rssi.Enqueue(b.rssi);
					}
					/*else {
						BCarr2rssi.Dequeue();
						BCarr2rssi.Enqueue(b.rssi);
					}*/
					double BC2ct = BCarr2rssi.Count;
					double AveTemp2 = 0.0;
					string optqueue2 = "";
					foreach (double queue in BCarr2rssi)
					{
						optqueue2 += Convert.ToString(queue) + ", ";
						AveTemp2 += queue;
					}
					_txtBC2rssi.text = optqueue2;
					BCave2rssi = AveTemp2 / BC2ct;
					_txtBC2ave.text = Convert.ToString(Math.Round(BCave2rssi, 2));
					MB2 = RssiToMeter(BCave2rssi, myStr2);
					_txtBC2meter.text = Convert.ToString(Math.Round(MB2, 3));
				}
				else if(b.minor == ck_Minor3) {
					if(BCarr3rssi.Count < Qlimit) {
						BCarr3rssi.Enqueue(b.rssi);
					}
					/*else {
						BCarr3rssi.Dequeue();
						BCarr3rssi.Enqueue(b.rssi);
					}*/
					double BC3ct = BCarr3rssi.Count;
					double AveTemp3 = 0.0;
					string optqueue3 = "";
					foreach (double queue in BCarr3rssi)
					{
						optqueue3 += Convert.ToString(queue) + ", ";
						AveTemp3 += queue;
					}
					_txtBC3rssi.text = optqueue3;
					BCave3rssi = AveTemp3 / BC3ct;
					_txtBC3ave.text = Convert.ToString(Math.Round(BCave3rssi, 2));
					MB3 = RssiToMeter(BCave3rssi, myStr3);
					_txtBC3meter.text = Convert.ToString(Math.Round(MB3, 3));
				}
				else if(b.minor == ck_Minor4) {
					if(BCarr4rssi.Count < Qlimit) {
						BCarr4rssi.Enqueue(b.rssi);
					}
					/*else {
						BCarr4rssi.Dequeue();
						BCarr4rssi.Enqueue(b.rssi);
					}*/
					double BC4ct = BCarr4rssi.Count;
					double AveTemp4 = 0.0;
					string optqueue4 = "";
					foreach (double queue in BCarr4rssi)
					{
						optqueue4 += Convert.ToString(queue) + ", ";
						AveTemp4 += queue;
					}
					_txtBC4rssi.text = optqueue4;
					BCave4rssi = AveTemp4 / BC4ct;
					_txtBC4ave.text = Convert.ToString(Math.Round(BCave4rssi, 2));
					MB4 = RssiToMeter(BCave4rssi, myStr4);
					_txtBC4meter.text = Convert.ToString(Math.Round(MB4, 3));
				}
				//Beacon內部其他可用參數
				//b.range.ToString();
				//b.strength.ToString(); //(db)
				//b.rssi.ToString(); //(db)
				//b.accuracy.ToString(); //(m)
			}
			/*_txtBC1rssi.text = b.type.ToString();
			_txtBC2rssi.text = b.UUID.ToString();
			_txtBC3rssi.text = b.major.ToString();
			_txtBC4rssi.text = b.minor.ToString();
			_txtBC1ave.text = b.rssi.ToString();*/
		}
	}

	private bool chklen(double B1, double B2, double B3, double B4) {
		// Beacon距離都不為0且有輸入值
		if(MB1 > 0 && MB2 > 0 && MB3 > 0 && MB4 > 0 && pos12 > 0 && pos23 > 0) {
			return true;
		}
		return false;
	}

	// 由三角形三邊求出比例距離（參數一：底面、參數二：作為距離基準的斜邊）
	private double heron_h(double t1, double t2, double t3) {
		double hfs = (t1 + t2 + t3) / 2;
		double hfa = Math.Sqrt(hfs * (hfs - t1) * (hfs - t2) * (hfs - t3));
		double ht = hfa * 2 / t1;
		return Math.Sqrt(Math.Pow(t2,2) - Math.Pow(ht,2));
	}

	// 兩座標求距離 + Beacon距離求高
	private double co_range(double c1x, double c1y, double c2x, double c2y, double BC) {
		double CR = Math.Pow(c2x - c1x, 2) + Math.Pow(c2y - c1y, 2); // 下面計算還要平方，所以這裡就省去開根號
		return Math.Sqrt(Math.Pow(BC, 2) - CR);
	}

	private void BeaconToCoordinate(double B1, double B2, double B3, double B4) {
		//四顆版的預設座標位置：B2為原點（0,0）、B1為（0,y）、B3為（x,0）、B4為（x,y）
		CDbx = pos23;
		CDby = pos12;
		//先求角錐頂點投影點（各求兩邊取平均）
		CDN_x = (heron_h(pos23, B2, B3) + heron_h(pos23, B1, B4)) / 2;
		CDN_y = (heron_h(pos12, B2, B1) + heron_h(pos12, B3, B4)) / 2;
		//取四點算出之高，再求平均
		double verB1 = co_range(0, CDby, CDN_x, CDN_y, B1);
		double verB2 = co_range(0, 0, CDN_x, CDN_y, B2);
		double verB3 = co_range(CDbx, 0, CDN_x, CDN_y, B3);
		double verB4 = co_range(CDbx, CDby, CDN_x, CDN_y, B4);
		CDN_z = sheight - ((verB1 + verB2 + verB3 + verB4) / 4);

		//_DEBUG1.text = Convert.ToString(Math.Round(HF_a,3)) + " / " + Convert.ToString(Math.Round(HF_b,3)) + " / " + Convert.ToString(Math.Round(HF_c,3));
		//_DEBUG2.text = Convert.ToString(Math.Round(HF_d,3));
		//_DEBUG3.text = Convert.ToString(Math.Round(HF_ft,3)) + " / " + Convert.ToString(Math.Round(HF_volume,3)) + " / " + Convert.ToString(Math.Round(HF_area,3));
	}

	// PlayerPrefs
	// Get與Set只接受String/Int/Float三種型態
	private void SavePlayerPrefs() {
		/*PlayerPrefs.SetString("Name1", _B1name.text);
		PlayerPrefs.SetString("Name2", _B2name.text);
		PlayerPrefs.SetString("Name3", _B3name.text);
		PlayerPrefs.SetString("Name4", _B4name.text);*/
		PlayerPrefs.SetString("Minor1", _B1minor.text);
		PlayerPrefs.SetString("Minor2", _B2minor.text);
		PlayerPrefs.SetString("Minor3", _B3minor.text);
		PlayerPrefs.SetString("Minor4", _B4minor.text);
		/*PlayerPrefs.SetString("myStr1", _IFB1str.text);
		PlayerPrefs.SetString("myStr2", _IFB2str.text);
		PlayerPrefs.SetString("myStr3", _IFB3str.text);
		PlayerPrefs.SetString("myStr4", _IFB4str.text);*/
		PlayerPrefs.SetString("envfac", _IFEV.text);
		/*PlayerPrefs.SetString("sheight", _IFHT.text);
		PlayerPrefs.SetString("POS12", _POS12.text);
		PlayerPrefs.SetString("POS23", _POS23.text);*/
		//PlayerPrefs.SetString("POS34", _POS34.text);
		//PlayerPrefs.SetString("POS14", _POS14.text);
		//PlayerPrefs.DeleteAll();
	}

	private void RestorePlayerPrefs() {
		/*if (PlayerPrefs.HasKey("Name1"))
			_B1name.text = PlayerPrefs.GetString("Name1");
		else
			_B1name.text = "Name1";
		if (PlayerPrefs.HasKey("Name2"))
			_B2name.text = PlayerPrefs.GetString("Name2");
		else
			_B2name.text = "Name2";
		if (PlayerPrefs.HasKey("Name3"))
			_B3name.text = PlayerPrefs.GetString("Name3");
		else
			_B3name.text = "Name3";
		if (PlayerPrefs.HasKey("Name4"))
			_B4name.text = PlayerPrefs.GetString("Name4");
		else
			_B4name.text = "Name4";*/
		if (PlayerPrefs.HasKey("Minor1"))
			_B1minor.text = PlayerPrefs.GetString("Minor1");
		if (PlayerPrefs.HasKey("Minor2"))
			_B2minor.text = PlayerPrefs.GetString("Minor2");
		if (PlayerPrefs.HasKey("Minor3"))
			_B3minor.text = PlayerPrefs.GetString("Minor3");
		if (PlayerPrefs.HasKey("Minor4"))
			_B4minor.text = PlayerPrefs.GetString("Minor4");
		/*if (PlayerPrefs.HasKey("myStr1"))
			_IFB1str.text = PlayerPrefs.GetString("myStr1");
		if (PlayerPrefs.HasKey("myStr2"))
			_IFB2str.text = PlayerPrefs.GetString("myStr2");
		if (PlayerPrefs.HasKey("myStr3"))
			_IFB3str.text = PlayerPrefs.GetString("myStr3");
		if (PlayerPrefs.HasKey("myStr4"))
			_IFB4str.text = PlayerPrefs.GetString("myStr4");*/
		if (PlayerPrefs.HasKey("envfac"))
			_IFEV.text = PlayerPrefs.GetString("envfac");
		else
			_IFEV.text = "2.0";
		/*if (PlayerPrefs.HasKey("sheight"))
			_IFHT.text = PlayerPrefs.GetString("sheight");
		else
			_IFHT.text = "2.0";
		if (PlayerPrefs.HasKey("POS12"))
			_POS12.text = PlayerPrefs.GetString("POS12");
		if (PlayerPrefs.HasKey("POS23"))
			_POS23.text = PlayerPrefs.GetString("POS23");*/
		/*if (PlayerPrefs.HasKey("POS34"))
		_POS34.text = PlayerPrefs.GetString("POS34");
		if (PlayerPrefs.HasKey("POS14"))
			_POS14.text = PlayerPrefs.GetString("POS14");*/
		//if (PlayerPrefs.HasKey("Type"))
		//	bt_Type = (BeaconType)PlayerPrefs.GetInt("Type");
		//if (PlayerPrefs.HasKey("BroadcastMode"))
		//	bm_Mode = (BroadcastMode)PlayerPrefs.GetInt("BroadcastMode");
		//else 
		//	bm_Mode = BroadcastMode.unknown;
	}
}