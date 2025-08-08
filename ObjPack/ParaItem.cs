using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//--------------------------------------------------------------------------------------
using GlobalClass;
using System.Diagnostics;
using ObjPack;
//--------------------------------------------------------------------------------------
namespace MGOA_Pack
{
    public class ParaItem : MGOA
    {
        //------------------------------------------------------------------------------
        public AttInteger Value;
        public int Min;
        public int Max;
        public AttFloat Low;
        public AttFloat High;
        private UnitItem Unit = null;
        public Alarm.Mode alaMode = Alarm.Mode.Normal;
        public AlarmPara CurrentAlarmPara = null;
        public string CurrentPara = string.Empty;

        public delegate void OnAlarm(int ViewID,string key,double curVal,AlarmPara para);
        public static event OnAlarm onAlarm = null;

        //------------------------------------------------------------------------------
        public ParaItem(MGOA Parent, DBReader dReader, int ViewID,List<AlarmPara> list)
            : base(Parent, dReader, ViewID, list)
        {
            //Childs = new MGOA[5];
            uint i = 0;
            Value = new AttInteger(this, dReader, i, "Val");
            Childs.Add(Value);
            Low = new AttFloat(this, dReader, i, "Low");
            Childs.Add(Low);
            High = new AttFloat(this, dReader, i, "High");
            Childs.Add(High);
            Min = dReader.GetInt("Min");
            Max = dReader.GetInt("Max");
            CurrentPara = dReader.GetStr("SN");
            if (list != null)
            {
                foreach (AlarmPara para in list)
                {
                    if (para.ParaName == CurrentPara)
                    {
                        CurrentAlarmPara = para;
                        break;
                    }
                }
            }
        }
        //------------------------------------------------------------------------------
        public String GetValue()
        {
            return GetValue(Value.Value);
            /*if (Value.Value < Min || Value.Value > Max)
                return (Unit == null) ? "---" : Unit.InvStr;
            
            return (Unit==null)? Value.Value.ToString(): Unit.GetString(Value.Value);*/
        }
        //------------------------------------------------------------------------------
        public String GetValue(int v)
        {
            if (v < Min || v > Max)
                return (Unit == null) ? "---" : Unit.InvStr;

            return (Unit == null) ? v.ToString() : Unit.GetString(v);
        }
        //------------------------------------------------------------------------------
        public String GetLow()
        {
            return (Unit == null) ? Low.Value.ToString() : Low.Value.ToString(Unit.DStr);
        }
        //------------------------------------------------------------------------------
        public String GetHigh()
        {
            return (Unit == null) ? High.Value.ToString() : High.Value.ToString(Unit.DStr);
        }
        //------------------------------------------------------------------------------
        public void BindUnit(UnitItem Unit)
        {
            this.Unit = Unit;
        }
        //------------------------------------------------------------------------------
        public void SetValue(Int16 Data)
        {
            Value.Value = Data;
            CheckAlarm();   //检查报警
        }
        //------------------------------------------------------------------------------
        public void AlarmSetting(Double LowValue, Double HighValue)
        {
            Low.Value = LowValue;
            High.Value = HighValue;
            CheckAlarm();
        }


        //------------------------------------------------------------------------------

        /// <summary>
        /// 检查告警
        /// </summary>
        private void CheckAlarm()
        {   //检查报警
            //Double dVal = (Unit == null) ? Value.Value : Unit.ToDouble(Value.Value);
            //if (dVal < Min || dVal > Max)
            //{
            //    alaMode = Alarm.Mode.Normal;
            //    return;
            //}
            //if (dVal < Low.Value) alaMode = Alarm.Mode.ToLow;
            //else if (dVal > High.Value) alaMode = Alarm.Mode.ToHigh;
            //else alaMode = Alarm.Mode.Normal;

            Double dVal = (Unit == null) ? Value.Value : Unit.ToDouble(Value.Value);
            if (dVal < 0) return;
            if (CurrentAlarmPara != null)
            {
                if (CurrentAlarmPara.isEnabled == 0)
                {
                    if (dVal > CurrentAlarmPara.High||dVal<CurrentAlarmPara.Low)
                    { 
                        if(onAlarm!=null)
                        {
                          onAlarm(ViewID,CurrentPara,dVal,CurrentAlarmPara);
                        }

                        this.alaMode = dVal > CurrentAlarmPara.High ? Alarm.Mode.ToHigh : Alarm.Mode.ToLow;
                    }
                    else
                    {
                        this.alaMode = Alarm.Mode.Normal;
                    }
                }
            }

        }
        //------------------------------------------------------------------------------
        public float GetDrawY(int Val, int Height)
        {
            return (float)((Max - Val) * Height) / (Max - Min);
        }
        //------------------------------------------------------------------------------            
        public Int16 FloatValueToInt(Double FloatValue)
        {
            if (Unit == null) return (short)GLB.ToInt(FloatValue);
            return Unit.ToInt(FloatValue);
        }
        //------------------------------------------------------------------------------

    }
}
//--------------------------------------------------------------------------------------
