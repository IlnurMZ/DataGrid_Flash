﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashViewer2
{
    internal class Packet
    {
        public byte idPacket { get; set; }
        public byte idDevice { get; set; }
        public List<byte> lengthParams { get; set; } = new(); // количество байт на параметр
        public List<string> typeParams { get; set; } = new();
        public List<string> headerColumn { get; set; } = new();
        public List<string> typeCalculate { get; set; } = new();
        public List<double[]> dataCalculation { get; set; } = new();
        public List<byte> countSign { get; set; } = new();
        public List<byte> widthColumn { get; set; } = new();

        public double badValue { get; set; } // пока не учавствует в работе
        public int lengthLine { get; set; } = 0;
        public byte[] endLine { get; set; } = new byte[2]; // 2 байта на конец строки
    }
}
