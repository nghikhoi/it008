using System;
using CNetwork;
using UI.Network;

namespace UI.Utils
{
    public class Packets
    {
        public static void SendPacket<T>() where T : IPacket
        {
            try
            {
                T data = Activator.CreateInstance<T>();
                _ = ChatConnection.Instance.Send(data);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
