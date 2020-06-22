using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace PT_lab3
{
    class Program
    {
        static void SerializeCars(List<Car> values, string filePath)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, values);

            }
        }

        static List<Car> DeserializeCars(string filePath)
        {
            List<Car> cars = new List<Car>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            using (Stream reader = new FileStream(filePath, FileMode.Open))
            {
               cars = (List<Car>)serializer.Deserialize(reader);
            }
            return cars;
        }

        static void createXmlFromLinq(List<Car> myCars)
        {
            IEnumerable<XElement> nodes = myCars.Select(car =>
                new XElement("car",
                    new XElement("model", car.model),
                    new XElement("engine",
                        new XAttribute("model", car.motor.model),
                        new XElement("displacement", car.motor.displacement),
                        new XElement("horsePower", car.motor.horsePower)),
                    new XElement("year", car.year)));

            XElement rootNode = new XElement("cars", nodes);
            rootNode.Save("CarsFromLinq.xml");
        }


        static void Main(string[] args)
        {

            List<Car> myCars = new List<Car>(){
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            };


            // zadanie 1
            Console.WriteLine("Zadanie 1");
            var zad1_1 = from c in myCars
                         where c.model == "A6"
                         select new
                         {
                             engineType = c.motor.model == "TDI" ? "Diesel" : "Petrol",
                             hppl = c.motor.horsePower / c.motor.displacement
                         };


            var zad1_2 = from e in zad1_1
                         group e by e.engineType into tmp
                         select new
                         {
                             engineType = tmp.Key,
                             avgHppl = tmp.Average(v => v.hppl)
                         };

            foreach (var group in zad1_2)
            {
                Console.WriteLine(group.engineType + ": " + group.avgHppl);
               
            }


            // zadanie 2
            Console.WriteLine("\nZadanie 2");
            string filePath = "CarsCollection.xml";
            SerializeCars(myCars, filePath);
            List<Car> deserCars = DeserializeCars(filePath);

            foreach (var c in deserCars)
            {
                Console.WriteLine("Samochód: " + c.model + " " + c.year + 
                    " Silnik: " + c.motor.model + " " + c.motor.horsePower + " " + c.motor.displacement);
            }

            // zadanie 3
            Console.WriteLine("\nZadanie 3");
            XElement rootNode = XElement.Load(filePath);
            double avgHP = (double)rootNode.XPathEvaluate("sum(//car/engine[@model!='TDI']/horsePower) " +
                "div count(//car/engine[@model!='TDI']/horsePower)");
            Console.WriteLine("Przeciętna moc samochodów o silnikach innych niż TDI: " + avgHP);

            var models = rootNode.XPathSelectElements("//car/model[not(. = ../following-sibling::car/model)]");

            Console.WriteLine("Modele samochodów bez powtórzeń");
            foreach (var model in models)
            {
                Console.WriteLine(model.Value);
            }


            // zadanie 4
            createXmlFromLinq(myCars);


            // zadanie 5
            var style = new XAttribute("style", "border: 1px solid black");
            IEnumerable<XElement> tab = myCars.Select(car =>
                     new XElement("tr", style,
                     new XElement("td", style, car.model),
                     new XElement("td", style, car.motor.model),
                     new XElement("td", style, car.motor.displacement),
                     new XElement("td", style, car.motor.horsePower),
                     new XElement("td", style, car.year)));

            XDocument template = XDocument.Load("template.html");
            template.Element("{http://www.w3.org/1999/xhtml}html").Element("{http://www.w3.org/1999/xhtml}body").Add(new XElement("table", style, tab));
            template.Save("CarsTable.html");


            // zadanie 6
            XDocument xmlFile = XDocument.Load("CarsCollection.xml");
            var query = from c in xmlFile.Elements("cars").Elements("car")
                        select c;

            foreach (XElement el in query)
            {
                el.Element("engine").Element("horsePower").Name = "hp";
                var year = el.Element("year");
                el.Element("model").Add(new XAttribute("year", year.Value));
                year.Remove();
            }

            xmlFile.Save("CarsCollectionZad5.xml");


        }
    }
}
