/*
 * Created by SharpDevelop.
 * User: clinky
 * Date: 2010/10/24
 * Time: 0:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace CXGUI.External
{
	/// <summary>
	/// Description of WriteWavHeader.
	/// </summary>
	public class WriteWavHeader
	{
		public WriteWavHeader()
		{
		}
		
		public static void Write(Stream target, AviSynthClip a)
        {
            const uint FAAD_MAGIC_VALUE = 0xFFFFFF00;
            const uint WAV_HEADER_SIZE = 36;
            bool useFaadTrick = a.AudioSizeInBytes >= ((long)uint.MaxValue - WAV_HEADER_SIZE);
            target.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            target.Write(BitConverter.GetBytes(useFaadTrick ? FAAD_MAGIC_VALUE : (uint)(a.AudioSizeInBytes + WAV_HEADER_SIZE)), 0, 4);
            target.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "), 0, 8);
            target.Write(BitConverter.GetBytes((uint)0x10), 0, 4);
            target.Write(BitConverter.GetBytes((a.SampleType==AudioSampleType.FLOAT) ? (short)0x03 : (short)0x01), 0, 2);
            target.Write(BitConverter.GetBytes(a.ChannelsCount), 0, 2);
            target.Write(BitConverter.GetBytes(a.AudioSampleRate), 0, 4);
            target.Write(BitConverter.GetBytes(a.AvgBytesPerSec), 0, 4);
            target.Write(BitConverter.GetBytes(a.BytesPerSample*a.ChannelsCount), 0, 2);
            target.Write(BitConverter.GetBytes(a.BitsPerSample), 0, 2);
            target.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
            target.Write(BitConverter.GetBytes(useFaadTrick ? (FAAD_MAGIC_VALUE - WAV_HEADER_SIZE) : (uint)a.AudioSizeInBytes), 0, 4);
        }
	}
}
