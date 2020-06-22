using PT_lab3;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace PT_lab3
{
    [Serializable]
    [XmlType(TypeName = "car")]
    public class Car
    {
        public string model;
        [XmlElement(ElementName = "engine")]
        public Engine motor;
        public int year;

        //public string Model { get => model; set => model = value; }
        //public Engine Motor { get => motor; set => motor = value; }
        //public int Year { get => year; set => year = value; }

        public Car() { }
        public Car(string model, Engine motor, int year) {
            this.model = model;
            this.motor = motor;
            this.year = year;
        }

    }
}
