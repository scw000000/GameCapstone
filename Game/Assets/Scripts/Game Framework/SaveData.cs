using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameCapstone
{


    [System.Serializable]
    public struct Level
    {
        public bool _unlocked;
        public bool _completed;
        public Level(bool unlocked, bool completed)
        {
            _unlocked = unlocked;
            _completed = completed;
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public static string _loadPrefName = "LoadFromSlot";
        public static string _saveSlotPrefName = "SaveSlot";
        public static string _saveSlotValidPrefName = "SaveSlotValid";
        public int _currentProgress;
        public int _year;
        public int _month;
        public int _day;
        public int _hour;
        public int _minute;
        public int _second;

        public string _currentLevel;
        public string _dayOfWeek;
        public List<Level> _levelProgress;

        public SaveData()
        {
            _levelProgress = new List<Level>() {
                    new Level ( true, false ),
                    new Level ( false, false ),
                    new Level ( false, true ),
                    new Level ( true, false )
                };
            _year = System.DateTime.Now.Year;
            _month = System.DateTime.Now.Month;
            _day = System.DateTime.Now.Day;
            _dayOfWeek = System.DateTime.Now.DayOfWeek.ToString();
            _hour = System.DateTime.Now.Hour;
            _minute = System.DateTime.Now.Minute;
            _second = System.DateTime.Now.Second;
        }

    }
}
