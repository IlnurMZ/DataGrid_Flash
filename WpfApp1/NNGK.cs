using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlashViewer
{
    internal class NNGK
    {
        public int numPacket { get; set; } // id пакета
        public string time { get; set; } // Дата
        public double GK1 { get; set; } // ННК1/ННК1(вода)
        public double GK2 { get; set; } // ННК2/ННК2(вода)
        public double GK3 { get; set; } // НГК/НГК(вода)
        public double GK4 { get; set; } // ННК1 [ед]
        public double GK5 { get; set; } // ННК2 [ед]
        public double GK6 { get; set; } // НГК [ед]
        public double temperatura { get; set; } // Температура НГК [°С]
        public double periodSHIM { get; set; } // Период ШИМ ННК
        public double currentWorkTime { get; set; } // Текущая наработка
        public double totalWorkTime { get; set; } // Общая наработка
        public double timeNakop { get; set; } // Время накопления [сек]
        public double error { get; set; } // Ошибка I2C
    }

    
}
