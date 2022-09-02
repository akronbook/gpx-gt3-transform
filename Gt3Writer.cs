using System.Xml;
using Gpx;

namespace GPX_GT3_Transform
{
    public class Gt3Writer
    {
        public void Write(GpxTrack track, string outputFile) {
            var doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
            doc.AppendChild(xmlDeclaration);
            XmlElement gpxNode = doc.CreateElement("gpx");
            XmlAttribute xmlnsAttribute = doc.CreateAttribute("xmlns");
            xmlnsAttribute.Value = "http://www.topografix.com/GPX/1/1";
            gpxNode.Attributes.Append(xmlnsAttribute);

            XmlAttribute creatorAttribute = doc.CreateAttribute("creator");
            creatorAttribute.Value = track.Source;
            gpxNode.Attributes.Append(creatorAttribute);    

            XmlAttribute versionAttribute = doc.CreateAttribute("version");
            versionAttribute.Value = "1.1"; // TODO: avoid hard-coding
            gpxNode.Attributes.Append(versionAttribute); 
            
            XmlAttribute xsiAttribute = doc.CreateAttribute("xmlns:xsi");
            xsiAttribute.Value = "http://www.w3.org/2001/XMLSchema-instance"; // TODO: avoid hard-coding
            gpxNode.Attributes.Append(xsiAttribute);        

            XmlAttribute schemaLocationAttribute = doc.CreateAttribute("xsi:schemaLocation");
            schemaLocationAttribute.Value = "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd"; // TODO: avoid hard-coding
            gpxNode.Attributes.Append(schemaLocationAttribute);  

            XmlElement trackNode = doc.CreateElement("trk");
            gpxNode.AppendChild(trackNode);

            XmlElement nameNode = doc.CreateElement("name");   
            XmlText nameText = doc.CreateTextNode(track.Name);
            nameNode.AppendChild(nameText);
            trackNode.AppendChild(nameNode);


            XmlElement typeNode = doc.CreateElement("type");   
            XmlText typeText = doc.CreateTextNode(String.IsNullOrWhiteSpace(track.Type) ? "Hiking" : track.Type);
            typeNode.AppendChild(typeText);
            trackNode.AppendChild(typeNode);

            XmlElement? currentNode = null;
            foreach(var segment in track.Segments)
            {
                XmlElement trackSegmentNode = doc.CreateElement("trkseg"); 
                trackNode.AppendChild(trackSegmentNode); 

                foreach(var point in segment.TrackPoints) {
                    XmlElement pointNode = doc.CreateElement("trkpt");  
                    trackSegmentNode.AppendChild(pointNode);

                    XmlAttribute latAttribute = doc.CreateAttribute("lat");
                    latAttribute.Value = point.Latitude.Degrees.ToString("0.#####");
                    pointNode.Attributes.Append(latAttribute); 

                    XmlAttribute longAttribute = doc.CreateAttribute("lon");
                    longAttribute.Value = point.Longitude.Degrees.ToString("0.#####");
                    pointNode.Attributes.Append(longAttribute); 

                    XmlElement elevNode = doc.CreateElement("ele");
                    XmlText elevationText = doc.CreateTextNode(point.Elevation?.ToString("0.#"));
                    elevNode.AppendChild(elevationText); 
                    pointNode.AppendChild(elevNode); 
                    
                    currentNode = pointNode;
                }

                if (currentNode != null) {
                    trackSegmentNode.RemoveChild(currentNode);
                }
            }


            doc.AppendChild(gpxNode);
            doc.Save(outputFile);
        }
    }
}