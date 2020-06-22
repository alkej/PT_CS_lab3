using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PT_lab3
{
    [Serializable]
    public class Engine
    {
        public double displacement;
        public double horsePower;
        [XmlAttribute]
        public string model;

        //public double Displacement { get => displacement; set => displacement = value; }
        //public double HorsePower { get => horsePower; set => horsePower = value; }
        //public string Model { get => model; set => model = value; }

        public Engine() { }
        public Engine(double displacement, double hp, string model)
        {
            this.displacement = displacement;
            this.horsePower = hp;
            this.model = model;
        }

    }
}
