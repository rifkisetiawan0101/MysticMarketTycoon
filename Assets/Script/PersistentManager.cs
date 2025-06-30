using System;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManager : MonoBehaviour {
    public static PersistentManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        } else {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    public static event Action OnTotalKoinChanged;  // Tambahkan event ini
    public float dataKoin = 4000;  // Nilai awal koin
    public void UpdateKoin(float amount) {
        dataKoin += amount;
        OnTotalKoinChanged?.Invoke();
        Debug.Log("Persistent Koin saat ini: " + dataKoin);
    }

    public static event Action OnTotalBatuAkikChanged;
    public float dataBatuAkik = 3;
    public void UpdateBatuAkik(float amount) {
        dataBatuAkik += amount;
        OnTotalBatuAkikChanged?.Invoke();
        Debug.Log("Persistent Batu akik saat ini " + dataBatuAkik);    
    }

    public static event Action OnTotalMerchantChanged;
    public float dataTotalMerchant = 0;
    public void UpdateTotalMerchant(float amount) {
        dataTotalMerchant += amount;
        OnTotalMerchantChanged?.Invoke();
        Debug.Log("Persistent total merchant saat ini: " + dataTotalMerchant);
    }

    public static event Action OnTotalNpcChanged;
    public float dataMaxNpc = 0;
    public float dataTotalSpawnNpc = 0;
    public void UpdateTotalNpc(float amount) {
        dataMaxNpc += amount;
        OnTotalNpcChanged?.Invoke();
    }

    public static event Action OnDayCounterChanged;
    public int dayCounter = 0;
    public void UpdateDayCounter(int amount) {
        dayCounter += amount;
        OnDayCounterChanged?.Invoke();
    }

    public static event Action OnNightCounterChanged;
    public int nightCounter = 1;
    public void UpdateNightCounter(int amount) {
        nightCounter += amount;
        OnNightCounterChanged?.Invoke();
    }

    public bool isPlayTimer = true;
    public bool isUIOpen = false;
    public bool isActivateUI = true;
    public bool isNowMalam = true;
    public bool isBossDefeated = false;

    public bool isBattleBoss;
    public bool isBattleUto;

    public static event Action OnUtoDefeated;
    public bool isUtoDefeated = false;
    public void UpdateUtoDefeated(bool state) {
        isUtoDefeated = state;
        OnUtoDefeated?.Invoke();
    }

    public static event Action<string, float> OnTotalCollectableChanged; 
    public void UpdateCollectable(float amount, string namaCollectable) {
        switch (namaCollectable) {
            case "Batu":
                dataBatu += amount;
                isBatuCollected = true;
                break;
            case "Kayu":
                dataKayu += amount;
                isKayuCollected = true;
                break;
            case "TanahLiat":
                dataTanahLiat += amount;
                isTanahLiatCollected = true;
                break;
            case "Akik":
                dataBatuAkik += amount;
                isAkikCollected = true;
                break;
        }
        OnTotalCollectableChanged?.Invoke(namaCollectable, amount);
    }

    [Header("-- Merchant Data --")]
    public List<MerchantData> dataMerchantList = new List<MerchantData>();
    [System.Serializable]
    public class MerchantData {
        public GameObject merchantObject;
        public MerchantTypeSO merchantTypeSO;
        public Vector3 merchantPosition;
        public int rotateIndex;

        public int levelMerchant;
        public float hargaDagangan;
        public float penghasilanMerchant;

        public float stokDagangan;
        public float maxStokDagangan;
        
        public int costUpBatu;
        public int costUpKayu;
        public int costUpTanahLiat;
        public int costUpBatuAkik;
        public int hargaUpgrade;
    }

    [Header("-- Restok & Collectable Data --")] 
    public float dataStokSayur = 2;
    public float dataStokRempah = 2;
    public float dataStokDaging = 2;

    public float dataBatu = 30;
    public float dataKayu = 30;
    public float dataTanahLiat = 30;


    [Header("-- Penghargaan Data --")]
    public bool isSayurPlaced = false;
    public bool isRempahPlaced = false;
    public bool isDagingPlaced = false;

    [Header("---")]

    public bool isBatuCollected = false;
    public bool isKayuCollected = false;
    public bool isTanahLiatCollected = false;
    public bool isAkikCollected = false;

    [Header("---")]

    public bool isAwalEkonomiClaimed = false;
    public bool isAwalEkonomiPlayed = false;

    public bool isProyekBesarClaimed = false;
    public bool isProyekBesarPlayed = false;

    public bool isJuraganPasarClaimed = false;
    public bool isJuraganPasarPlayed = false;

    public bool isCuanDiHutanClaimed = false;
    public bool isCuanDiHutanPlayed = false;

    public bool isHinggaTerbitFajarClaimed = false;
    public bool isHinggaTerbitFajarPlayed = false;

    public bool isAkuDukunClaimed = false;
    public bool isAkuDukunPlayed = false;

    public bool isPengepulAlamClaimed = false;
    public bool isPengepulAlamPlayed = false;

    public bool isKuliSaktiClaimed = false;
    public bool isKuliSaktiPlayed = false;

    public bool isMalamKliwonClaimed = false;
    public bool isMalamKliwonPlayed = false;

    public bool isPebisnisGhaibClaimed = false;
    public bool isPebisnisGhaibPlayed = false;


    [Header("---")]
    
    [Header("-- Invoice & Buka Pasar --")]
    public bool isInvoiceShown = false;


    [Header("-- Furnitur Data --")]
    public List<FurniturData> dataFurniturList = new List<FurniturData>();
    [System.Serializable]
    public class FurniturData {
        public FurniturTypeSO furniturTypeSO;
        public Vector3 furniturPosition;
    }


    [Header("-- Spesial Data --")]
    public List<SpesialData> dataSpesialList = new List<SpesialData>();
    [System.Serializable]
    public class SpesialData {
        public SpesialTypeSO spesialTypeSO;
        public Vector3 spesialPosition;
    }
}
