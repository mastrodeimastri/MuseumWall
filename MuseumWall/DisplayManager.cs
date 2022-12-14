using System;
using System.Diagnostics;
namespace MuseumWall
{
	public partial class Common
	{
		protected void CreateSubProcess()
		{
			for (int i = 0; i < nScreens; i++)
			{
				displays[i] = new Process();
				displays[i].StartInfo.FileName = "/bin/omxplayer";
				displays[i].StartInfo.Arguments = $"--display {i*7} --no-osd --no-keys /home/pi/video/{i}.mp4";
				displays[i].StartInfo.CreateNoWindow = true;
			}
		}

		protected void StartDisplays()
		{
            for (int i = 0; i < nScreens; i++) displays[i].Start();
            
            displays[0].WaitForExit();
        }
    }
}

