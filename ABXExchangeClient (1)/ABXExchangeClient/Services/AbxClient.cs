using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ABXExchangeClient.Models;
using ABXExchangeClient.Utils;

namespace ABXExchangeClient.Services
{
    public class AbxClient
    {
        private const string HOST = "127.0.0.1";
        private const int PORT = 3000;
        private const int PACKET_SIZE = 17; // 4 + 1 + 4 + 4 + 4

        public async Task<List<AbxPacket>> FetchAllPacketsAsync()
        {
            var packets = new List<AbxPacket>();
            var receivedSequences = new HashSet<int>();

            using TcpClient client = new TcpClient();
            await client.ConnectAsync(HOST, PORT);

            using NetworkStream stream = client.GetStream();
            await stream.WriteAsync(new byte[] { 1, 0 }); // callType = 1, dummy resendSeq = 0

            var buffer = new byte[PACKET_SIZE];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, PACKET_SIZE)) > 0)
            {
                if (bytesRead < PACKET_SIZE) continue;

                var packet = ParsePacket(buffer);
                packets.Add(packet);
                receivedSequences.Add(packet.Sequence);
            }

            // Detect missing
            var maxSeq = packets.Max(p => p.Sequence);
            var missing = Enumerable.Range(1, maxSeq).Where(seq => !receivedSequences.Contains(seq)).ToList();

            foreach (var seq in missing)
            {
                var missed = await ResendPacket(seq);
                if (missed != null)
                    packets.Add(missed);
            }

            return packets.OrderBy(p => p.Sequence).ToList();
        }

        private async Task<AbxPacket?> ResendPacket(int sequence)
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(HOST, PORT);

            using NetworkStream stream = client.GetStream();
            await stream.WriteAsync(new byte[] { 2, (byte)sequence });

            var buffer = new byte[PACKET_SIZE];
            int bytesRead = await stream.ReadAsync(buffer, 0, PACKET_SIZE);

            if (bytesRead < PACKET_SIZE) return null;

            return ParsePacket(buffer);
        }

        private AbxPacket ParsePacket(byte[] buffer)
        {
            return new AbxPacket
            {
                Symbol = BinaryParser.ReadAsciiString(buffer, 0, 4),
                BuySellIndicator = (char)buffer[4],
                Quantity = BinaryParser.ReadInt32BigEndian(buffer, 5),
                Price = BinaryParser.ReadInt32BigEndian(buffer, 9),
                Sequence = BinaryParser.ReadInt32BigEndian(buffer, 13)
            };
        }

        public async Task SaveToJsonAsync(List<AbxPacket> packets, string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(packets, options);
            string fullPath = Path.Combine("../../../", path);
            await File.WriteAllTextAsync(fullPath, json);
        }
    }
}
