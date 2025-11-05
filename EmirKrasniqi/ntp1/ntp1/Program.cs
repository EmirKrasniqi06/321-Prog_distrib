using System;
using System.Net;
using System.Net.Sockets;
class Program
{
    static void Main()
    {
        try
        {
            // 4
            // --- A. Récupération de l'heure actuelle à partir d'un serveur NTP ---
            string ntpServer = "0.ch.pool.ntp.org";

            // 5
            byte[] ntpData = new byte[48];
            ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            // 6
            IPEndPoint ntpReference = new IPEndPoint(Dns.GetHostAddresses(ntpServer)[0], 123);

            // 7
            UdpClient client = new UdpClient();
            client.Connect(ntpReference);

            //8
            client.Send(ntpData, ntpData.Length);

            // 9
            ntpData = client.Receive(ref ntpReference);

            // 10
            // Conversion des données NTP en DateTime
            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var ntpTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            // 11
            Console.WriteLine($"Heure actuelle (NTP) : {ntpTime}");

            // 12
            client.Close();

            // --- B. Formats de date ---
            Console.WriteLine("\n--- Formats de date ---");
            Console.WriteLine(ntpTime.ToString("dddd, dd MMMM yyyy"));
            Console.WriteLine(ntpTime.ToString("dd.MM.yyyy HH:mm:ss"));
            Console.WriteLine(ntpTime.ToString("dd.MM.yyyy"));

            // --- C. Conversions temporelles ---
            Console.WriteLine("\n--- Conversions temporelles ---");

            // 1. Différence entre l'heure locale et l'heure NTP
            TimeSpan timeDiff = DateTime.Now - ntpTime;
            Console.WriteLine($"Différence de temps : {timeDiff.TotalSeconds:F2} secondes");

            // 2. Corriger l'heure locale avec l'heure NTP
            DateTime localTime = ntpTime.Add(timeDiff);
            Console.WriteLine($"Heure locale corrigée : {localTime}");

            // 3. Convertir l'heure locale en UTC
            DateTime utcTime = localTime.ToUniversalTime();
            Console.WriteLine($"Heure UTC : {utcTime}");

            // 4. Convertir l'heure UTC en locale
            localTime = utcTime.ToLocalTime();
            Console.WriteLine($"Heure locale (depuis UTC) : {localTime}");

            // 5. Convertir en GMT (Suisse = GMT+1)
            DateTime gmtTime = localTime.ToUniversalTime().AddHours(-1);
            Console.WriteLine($"Heure GMT : {gmtTime}");

            // 6. Convertir GMT en heure locale
            localTime = gmtTime.AddHours(1);
            Console.WriteLine($"Heure locale (depuis GMT) : {localTime}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }
}