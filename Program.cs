using Gpx;
using Gpx.Implementation;
using Gpx.Tests;

namespace GPX_GT3_Transform
{
    class Program
    {
        private static GpxTrack readTrack<TTrackPoint>(string path, IGpxTrackPointReader<TTrackPoint> trackPointReader = null)
            where TTrackPoint : GpxTrackPoint, new()
        {
            GpxTrack track = null;

            using (GpxIOFactory.CreateReader(path, trackPointReader, out IGpxReader reader, out _))
            {
                while (reader.Read(out GpxObjectType type))
                {
                    switch (type)
                    {
                        case GpxObjectType.Metadata:
                            break;
                        case GpxObjectType.WayPoint:
                            break;
                        case GpxObjectType.Route:
                            break;
                        case GpxObjectType.Track:
                            {
                                if (track == null)
                                    track = reader.Track;
                                else
                                    throw new InvalidOperationException("Track is already read");
                                break;
                            }
                    }
                }
            }

            if (track == null)
                throw new NullReferenceException("Track was not read");

            return track;

        }

        static void Main(string[] args)
        {
            var inputGpxFile = args[0];
            var outputGpxFile = args[1];
            GpxTrack track = readTrack(inputGpxFile, new ProximityTrackPointReader());
            Console.WriteLine($"Total # of segments={track.Segments.Count}, points={track.Segments[0].TrackPoints.Count}");
            var writer = new Gt3Writer();
            writer.Write(track, outputGpxFile);
        }
    }
}