using System;

namespace TrimEdge
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length < 1) {
				Console.WriteLine("Specify input image");
				return;
			}
				
			var fac = new ImageProcessor.ImageFactory(true);
			fac.Load(args[0]);

			var fb = new ImageProcessor.Imaging.FastBitmap(fac.Image);

			int iw = fb.Width;
			int ih = fb.Height;
			double min = double.MaxValue;
			double max = double.MinValue;
			int samples = 100;
			int skip = Math.Min(iw,ih)/samples;

			/*
			for(int y=0; y<ih; y++) {
				for(int x=0; x<iw; x++) {
					var c = fb.GetPixel(x,y);
					double dist = Math.Sqrt(c.R*c.R + c.G*c.G + c.B*c.B);
					if (dist < min) { min = dist; }
					if (dist > max) { max = dist; }
				}
			}

			Console.WriteLine("max= "+max+" min= "+min);
			*/

			var topsamp = new double[samples];
			var topcord = new int[samples];
			int topmax = ih/2;
			for (int s=0; s<samples; s++) {
				int x = s * iw / samples;
				double maxdiff = double.MinValue;
				double lastdist = 0;
				for(var y=0; y<topmax; y++) {
					var c = fb.GetPixel(x,y);
					double dist = Math.Sqrt(c.R*c.R + c.G*c.G + c.B*c.B);
					if (y == 0) {
						lastdist = dist;
						continue;
					}
					double diff = Math.Abs(lastdist - dist);
					lastdist = dist;
					if (diff > maxdiff) {
						maxdiff = diff;
						topsamp[s] = diff;
						topcord[s] = y;
					}
				}
			}

			for(int s=0; s<samples; s++) {
				//Console.WriteLine(s+" ["+topcord[s]+","+topsamp[s]+"]");
				Console.WriteLine(topcord[s]+","+topsamp[s]);
			}
		}
	}
}
