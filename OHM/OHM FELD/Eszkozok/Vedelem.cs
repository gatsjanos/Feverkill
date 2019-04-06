using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Management;
using System.Security.Cryptography;
using System.Net.Http;

namespace OpenHardwareMonitor
{
    public struct BiztKommInfo
    {
        public byte[] AESKulcs, AESIV, TitkositottAESKulcs, TitkositottAESIV;
        public int AESBlokkMeret, AESKulcsMeret;
    }
    public class Vedelem
    {
        public static bool SzerverIPMentes = true;
        public static void IndulasiHitelesites()
        {

            // return;//TODO: DEBUG VÉDELEM KIKAPCSOLVA

            try//Internetes hitelesítés
            {
                cookieJar = new CookieContainer();
                string[] sorok = Onlinehitelesites(4).Split('\n');

                switch (sorok[0])
                {
                    case "ervenyes_nullmach":
                    case "ervenyes_egyezomach":
                    case "ervenyes_nemkellmach":
                        {
                            try
                            {
                                if (Program.LICENSZERVENYESSEG != sorok[1] || Program.LICENSZTipus != Convert.ToInt32(sorok[3]))
                                {
                                    Program.LICENSZERVENYESSEG = sorok[1];
                                    Program.LICENSZTipus = Convert.ToInt32(sorok[3]);
                                    Fajlkezelo.HitelesitoAdatTitkositATLANIr();
                                }
                            }
                            catch
                            {
                                Program.LICENSZTipus = 10;
                                Fajlkezelo.HitelesitoAdatTitkositATLANIr();
                            }

                            try
                            {
                                if (sorok.Length > 4 && sorok[4] == "1")
                                    Program.LICENSZProbTeljVerz = true;
                                else
                                    Program.LICENSZProbTeljVerz = false;
                            }
                            catch
                            { }

                            byte[] bt = HitfajlTitkosit(Program.LICENSZJelszo + GetMachID(), Program.LICENSZNev + Program.LICENSZEmail + Program.LICENSZID + Program.LICENSZJelszo + Program.LICENSZERVENYESSEG + Program.LICENSZTipus.ToString());
                            if (!Fajlkezelo.HitelesitoAdatTitkositOTTBeolvas().SequenceEqual(bt))
                                Fajlkezelo.HitelesitoAdatTitkositOTTIr(bt);

                            SzerverIPMentes = false;

                            break;
                        }
                    case "lejart":
                        {
                            try
                            {
                                File.Delete("lic\\hitcr.ved");
                            }
                            catch { }
                            Program.FoAblak.SysTrayicon.ShowBalloonTip(8000, "Feverkill", "Your licence is deprecated!\nLejárt a licensze!", ToolTipIcon.Warning);
                            Environment.Exit(19981001);
                            break;
                        }
                    case "foglaltmach":
                        {
                            try
                            {
                                File.Delete("lic\\hitcr.ved");
                            }
                            catch { }
                            Program.FoAblak.SysTrayicon.ShowBalloonTip(30000, "Feverkill", "You have to reactivate your licence!\nÚjra kell aktiválnia a licenszét!\nReserved machine ID.", ToolTipIcon.Error);
                            try
                            {
                                System.Diagnostics.Process.Start("FeverkillSupervisor.exe");
                            }
                            catch { }

                            SzerverIPMentes = false;

                            Environment.Exit(19981001);

                            break;
                        }
                    case "nincsilyen1":
                    case "nincsilyen2":
                        {
                            try
                            {
                                File.Delete("lic\\hitcr.ved");
                            }
                            catch { }
                            Program.FoAblak.SysTrayicon.ShowBalloonTip(8000, "Feverkill", "Invalid licence!\nÉrvénytelen licensz!\n#3", ToolTipIcon.Error);
                            Environment.Exit(19981001);
                            break;
                        }
                    case "akttullepes":
                        {
                            try
                            {
                                File.Delete("lic\\hitcr.ved");
                            }
                            catch { }
                            Program.FoAblak.SysTrayicon.ShowBalloonTip(8000, "Feverkill", "Reactivation limit reached!\nÚjraaktiválási limit elérve!", ToolTipIcon.Error);
                            Environment.Exit(19981001);
                            break;
                        }

                    default:
                        {
                            throw new Exception();
                            break;
                        }
                }
            }
            catch//Offline hitelesítés
            {
                try
                {
                    string valasz = Offlinehitelesites(4);

                    switch (valasz)
                    {
                        case "ervenyes":
                            {
                                break;
                            }
                        case "ervenytelen#1":
                            {
                                Program.FoAblak.SysTrayicon.ShowBalloonTip(8000, "Feverkill", "Invalid licence!\nÉrvénytelen licensz!\n#1", ToolTipIcon.Error);
                                try
                                {
                                    File.Delete("lic\\hitcr.ved");
                                }
                                catch { }
                                Environment.Exit(19981001);

                                break;
                            }
                        case "lejart":
                            {
                                Program.FoAblak.SysTrayicon.ShowBalloonTip(8000, "Feverkill", "Your licence is deprecated!\nLejárt a licensze!", ToolTipIcon.Warning);
                                try
                                {
                                    File.Delete("lic\\hitcr.ved");
                                }
                                catch { }
                                Environment.Exit(19981001);

                                break;
                            }
                        case "ervenytelen#2":
                            {
                                Program.FoAblak.SysTrayicon.ShowBalloonTip(8000, "Feverkill", "Invalid licence!\nÉrvénytelen licensz!\n#2", ToolTipIcon.Error);
                                try
                                {
                                    File.Delete("lic\\hitcr.ved");
                                }
                                catch { }
                                Environment.Exit(19981001);

                                break;
                            }

                        case "HIBA":
                        default:
                            {
                                Program.FoAblak.SysTrayicon.ShowBalloonTip(8000, "Feverkill", "Invalid licence!\nÉrvénytelen licensz!\n#HIBA", ToolTipIcon.Error);
                                try
                                {
                                    File.Delete("lic\\hitcr.ved");
                                }
                                catch { }
                                Environment.Exit(19981001);

                                break;
                            }
                    }
                }
                catch
                {
                    try
                    {
                        File.Delete("lic\\hitcr.ved");
                    }
                    catch { }
                    Environment.Exit(19981001);
                }
            }
        }
        public static string Onlinehitelesites(int probszam)
        {
            try
            {
                List<string> valaszok = new List<string>();
                for (int i = 0; i < probszam; ++i)
                {
                    valaszok.Add(HitAdatFeltolto());


                    string[] sorok = valaszok[i].Split('\n');

                    if (sorok.Length == 0)
                    {
                        valaszok[i] = "HIBA";
                        continue;
                    }
                    switch (sorok[0])
                    {
                        case "HIBA":
                            {
                                break;
                            }

                        case "ervenyes_nullmach":
                        case "ervenyes_egyezomach":
                        case "ervenyes_nemkellmach":
                            {
                                if (sorok.Length < 3)
                                    valaszok[i] = "HIBA";
                                else
                                {
                                    i = 999999;//Kilép a for-ciklusból
                                }
                                break;
                            }
                        case "lejart":
                            {
                                break;
                            }
                        case "foglaltmach":
                            {
                                break;
                            }
                        case "nincsilyen1":
                        case "nincsilyen2":
                            {
                                break;
                            }
                        case "akttullepes":
                            {
                                break;
                            }

                        default:
                            {
                                valaszok[i] = "HIBA";
                                break;
                            }
                    }

                }

                try
                {
                    Dictionary<string, List<string>> ValaszDic = new Dictionary<string, List<string>>();

                    foreach (string item in valaszok)
                    {
                        string s = item.Split('\n')[0];

                        if (!ValaszDic.ContainsKey(s))
                        {
                            ValaszDic.Add(s, new List<string>());
                            ValaszDic[s].Add(item);
                        }
                        else
                        {
                            ValaszDic[s].Add(item);
                        }
                    }

                    if (ValaszDic.ContainsKey("ervenyes_egyezomach"))
                    {
                        return ValaszDic["ervenyes_egyezomach"][ValaszDic["ervenyes_egyezomach"].Count - 1];
                    }
                    if (ValaszDic.ContainsKey("ervenyes_nullmach"))
                    {
                        return ValaszDic["ervenyes_nullmach"][ValaszDic["ervenyes_nullmach"].Count - 1];
                    }
                    if (ValaszDic.ContainsKey("ervenyes_nemkellmach"))
                    {
                        return ValaszDic["ervenyes_nemkellmach"][ValaszDic["ervenyes_nemkellmach"].Count - 1];
                    }

                    string kimenovalasz = "HIBA";
                    int maxCount = 0;
                    foreach (KeyValuePair<string, List<string>> item in ValaszDic)
                    {
                        if (item.Value.Count > maxCount || kimenovalasz == "HIBA")
                        {
                            maxCount = item.Value.Count;
                            kimenovalasz = item.Value[item.Value.Count - 1];
                        }
                    }

                    return kimenovalasz;
                }
                catch
                {
                    return "HIBA";
                }
            }
            catch
            {
                return "HIBA";
            }
        }
        public static string Offlinehitelesites(int probszam)
        {
            List<string> valaszok = new List<string>();
            for (int i = 0; i < probszam; ++i)
            {
                valaszok.Add(HitAdatBeolvaso());

                if (valaszok[i] == "ervenyes")
                    return "ervenyes";
            }

            try
            {
                Dictionary<string, int> ValaszDic = new Dictionary<string, int>();

                foreach (string item in valaszok)
                {
                    if (!ValaszDic.ContainsKey(item))
                    {
                        ValaszDic.Add(item, 1);
                    }
                    else
                    {
                        ++ValaszDic[item];
                    }
                }

                string kimenovalasz = "HIBA";
                int maxCount = 0;
                foreach (KeyValuePair<string, int> item in ValaszDic)
                {
                    if (item.Value > maxCount || kimenovalasz == "HIBA")
                    {
                        maxCount = item.Value;
                        kimenovalasz = item.Key;
                    }
                }

                return kimenovalasz;
            }
            catch
            {
                return "HIBA";
            }

        }
        public static string HitAdatBeolvaso()
        {
            try
            {
                string visszafejtett = "c";
                byte[] titkositott = new byte[] { 1, 1 };
                int i = 0;
                for (; i < 3; i++)
                {
                    try
                    {
                        titkositott = Fajlkezelo.HitelesitoAdatTitkositOTTBeolvas();
                        visszafejtett = HitfajlVisszafejt(Program.LICENSZJelszo + GetMachID(), titkositott);
                        break;
                    }
                    catch
                    {
                    }
                }


                if (i >= 3)
                {
                    return "ervenytelen#1";
                }

                string titkositatlan = Program.LICENSZNev + Program.LICENSZEmail + Program.LICENSZID + Program.LICENSZJelszo + Program.LICENSZERVENYESSEG + Program.LICENSZTipus.ToString();

                if (visszafejtett == titkositatlan)
                {
                    string[] erv = Program.LICENSZERVENYESSEG.Split('.');

                    DateTime ervdt = new DateTime(int.Parse(erv[0]), int.Parse(erv[1]), int.Parse(erv[2]));

                    try
                    {
                        DateTime szerverDT = GetSzerverDatum();
                        if (ervdt < szerverDT)
                        {
                            return "lejart";
                        }
                    }
                    catch//Ha nem sikerült a szerver idejét lekérni (Exception volt), akkor ellenőrzi a gép idejével
                    {
                        if (ervdt < DateTime.Now)
                        {
                            return "lejart";
                        }
                    }
                }
                else
                {
                    return "ervenytelen#2";
                }

                return "ervenyes";
            }
            catch
            {
                return "HIBA";
            }
        }
        public static string HitAdatFeltolto()
        {
            try
            {
                string valasz = "HIBA";
                try
                {
                    BiztKommLetrehoz();

                    string postdata = "hostmuv=" + "licenszteszt" + "&ipmentes=" + ((SzerverIPMentes) ? "1" : "0") +
                                "&licid=" + Uri.EscapeDataString(EncodeBase64(AESTitkosit(Encoding.UTF8.GetBytes(Program.LICENSZID), BKommInfo.AESKulcs, BKommInfo.AESIV, BKommInfo.AESBlokkMeret, BKommInfo.AESKulcsMeret))) +
                                "&nev=" + Uri.EscapeDataString(EncodeBase64(AESTitkosit(Encoding.UTF8.GetBytes(Program.LICENSZNev), BKommInfo.AESKulcs, BKommInfo.AESIV, BKommInfo.AESBlokkMeret, BKommInfo.AESKulcsMeret))) +
                                "&email=" + Uri.EscapeDataString(EncodeBase64(AESTitkosit(Encoding.UTF8.GetBytes(Program.LICENSZEmail), BKommInfo.AESKulcs, BKommInfo.AESIV, BKommInfo.AESBlokkMeret, BKommInfo.AESKulcsMeret))) +
                                "&jelszo=" + Uri.EscapeDataString(EncodeBase64(AESTitkosit(Encoding.UTF8.GetBytes(Program.LICENSZJelszo), BKommInfo.AESKulcs, BKommInfo.AESIV, BKommInfo.AESBlokkMeret, BKommInfo.AESKulcsMeret))) +
                                "&machid=" + Uri.EscapeDataString(EncodeBase64(AESTitkosit(Encoding.UTF8.GetBytes(EncodeBase64(GetMachID())), BKommInfo.AESKulcs, BKommInfo.AESIV, BKommInfo.AESBlokkMeret, BKommInfo.AESKulcsMeret))) +
                                "&aeskulcs=" + Uri.EscapeDataString(EncodeBase64(BKommInfo.TitkositottAESKulcs)) +
                                "&aesiv=" + Uri.EscapeDataString(EncodeBase64(BKommInfo.TitkositottAESIV)) + 
                                "&verzio=" + Uri.EscapeDataString(Program.Verzioszam);
                    valasz = RequestPOST(Program.SzerverDomain + "/SZCSHost/LicenszHost.php", postdata, 30000);
                }
                catch { }

                return valasz;
            }
            catch
            {
                return "HIBA";
            }
        }

        public static DateTime GetSzerverDatum()
        {
            string s = RequestGET(Program.SzerverDomain + "/SZCSHost/LicenszHost.php?hostmuv=datumleker");
            string[] erv = s.Split('.');

            return new DateTime(int.Parse(erv[0]), int.Parse(erv[1]), int.Parse(erv[2]));
        }

        public static BiztKommInfo BKommInfo;
        public static void BiztKommLetrehoz()
        {
            string ServerPubKey = RequestPOST(Program.SzerverDomain + "/SZCSHost/LicenszHost.php", "hostmuv=biztkapcsrsakulcs", 21000);

            RSACryptoServiceProvider RSAobj = new RSACryptoServiceProvider();
            RSAobj.FromXmlString(ServerPubKey);

            SymmetricAlgorithm AESGeneralo = SymmetricAlgorithm.Create("AES");
            AESGeneralo.KeySize = 128;
            AESGeneralo.BlockSize = 128;
            AESGeneralo.GenerateKey();
            AESGeneralo.GenerateIV();
            AESGeneralo.Mode = CipherMode.CBC;
            AESGeneralo.Padding = PaddingMode.PKCS7;

            BiztKommInfo bKommInfo = new BiztKommInfo();
            bKommInfo.AESKulcs = AESGeneralo.Key;
            bKommInfo.AESIV = AESGeneralo.IV;
            bKommInfo.TitkositottAESKulcs = RSAobj.Encrypt(AESGeneralo.Key, true);
            bKommInfo.TitkositottAESIV = RSAobj.Encrypt(AESGeneralo.IV, true);
            bKommInfo.AESBlokkMeret = AESGeneralo.BlockSize;
            bKommInfo.AESKulcsMeret = AESGeneralo.KeySize;
            BKommInfo = bKommInfo;
        }
        public static byte[] AESTitkosit(byte[] adat, byte[] kulcs, byte[] iv, int blokkMeret, int kulcsMeret)
        {

            byte[] titkositott = null;

            using (var symmetricAlgorithm = SymmetricAlgorithm.Create("AES"))
            {
                symmetricAlgorithm.BlockSize = blokkMeret;
                symmetricAlgorithm.KeySize = kulcsMeret;
                symmetricAlgorithm.Key = kulcs;
                symmetricAlgorithm.IV = iv;
                symmetricAlgorithm.Mode = CipherMode.CBC;
                symmetricAlgorithm.Padding = PaddingMode.PKCS7;

                using (var transformation = symmetricAlgorithm.CreateEncryptor())
                {
                    titkositott = transformation.TransformFinalBlock(adat, 0, adat.Length);
                }
            }

            return titkositott;
        }
        public static byte[] AESVisszafejt(byte[] titkositottadat, byte[] kulcs, byte[] iv, int blokkMeret, int kulcsMeret)
        {

            byte[] visszafejtett = null;

            using (var symmetricAlgorithm = SymmetricAlgorithm.Create("AES"))
            {
                symmetricAlgorithm.BlockSize = blokkMeret;
                symmetricAlgorithm.KeySize = kulcsMeret;
                symmetricAlgorithm.Key = kulcs;
                symmetricAlgorithm.IV = iv;
                symmetricAlgorithm.Mode = CipherMode.CBC;
                symmetricAlgorithm.Padding = PaddingMode.PKCS7;

                using (var transformation = symmetricAlgorithm.CreateDecryptor())
                {
                    visszafejtett = transformation.TransformFinalBlock(titkositottadat, 0, titkositottadat.Length);
                }

            }

            return visszafejtett;
        }

        public static byte[] HitfajlTitkosit(string kulcs, string adat)
        {
            var textToBeCiphered = adat;

            byte[] key = new byte[32];
            byte[] iv = new byte[16];

            int q = 1;
            if (kulcs.Length > 90)
            {
                q = 2;
            }

            for (int i = 0; i < 32; ++i)
            {
                key[i] = (byte)kulcs[(q * i + i / kulcs.Length) % kulcs.Length];
            }

            for (int i = 0; i < 16; ++i)
            {
                iv[i] = (byte)kulcs[(q * (i + 32) + i / kulcs.Length) % kulcs.Length]; ;
            }

            byte[] cipherText = null;

            using (var symmetricAlgorithm = SymmetricAlgorithm.Create("AES"))
            {
                symmetricAlgorithm.BlockSize = 128;
                //symmetricAlgorithm.GenerateKey();
                //symmetricAlgorithm.GenerateIV();
                symmetricAlgorithm.Key = key;
                symmetricAlgorithm.IV = iv;
                symmetricAlgorithm.Mode = CipherMode.CBC;
                symmetricAlgorithm.Padding = PaddingMode.PKCS7;


                var bytesToBeCiphered = Encoding.UTF8.GetBytes(textToBeCiphered);

                using (var transformation = symmetricAlgorithm.CreateEncryptor())
                {
                    cipherText
                        = transformation.TransformFinalBlock(
                                bytesToBeCiphered, 0, bytesToBeCiphered.Length);
                } // using

                //key = symmetricAlgorithm.Key;
                //iv = symmetricAlgorithm.IV;
            } // using

            return cipherText;
        }
        public static string HitfajlVisszafejt(string kulcs, byte[] adat)
        {
            byte[] key = new byte[32];
            byte[] iv = new byte[16];

            int q = 1;
            if (kulcs.Length > 90)
            {
                q = 2;
            }

            for (int i = 0; i < 32; ++i)
            {
                key[i] = (byte)kulcs[(q * i + i / kulcs.Length) % kulcs.Length];
            }

            for (int i = 0; i < 16; ++i)
            {
                iv[i] = (byte)kulcs[(q * (i + 32) + i / kulcs.Length) % kulcs.Length]; ;
            }


            byte[] cipherText = adat;
            string plainText = null;

            using (var symmetricAlgorithm = SymmetricAlgorithm.Create("AES"))
            {
                symmetricAlgorithm.BlockSize = 128;
                symmetricAlgorithm.Key = key;
                symmetricAlgorithm.IV = iv;
                symmetricAlgorithm.Mode = CipherMode.CBC;
                symmetricAlgorithm.Padding = PaddingMode.PKCS7;

                using (var transformation = symmetricAlgorithm.CreateDecryptor())
                {
                    var deciphered
                        = transformation.TransformFinalBlock(
                                cipherText, 0, cipherText.Length);

                    plainText = Encoding.UTF8.GetString(deciphered);
                } // using

            } // using

            return plainText;
        }
        public static string EncodeBase64(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string EncodeBase64(byte[] eredeti)
        {
            return System.Convert.ToBase64String(eredeti);
        }
        public static string DecodeBase64(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static byte[] DecodeBase64ToByteTomb(string base64EncodedData)
        {
            return System.Convert.FromBase64String(base64EncodedData);
        }
        public static string GetMachID()
        {
            string CPU_ID = "", HDD_ID = "", MB_ID = "";

            try
            {
                ManagementObjectCollection mbsList = null;
                ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
                mbsList = mbs.Get();
                foreach (ManagementObject mo in mbsList)
                {
                    CPU_ID = mo["ProcessorID"].ToString();
                }

            }
            catch { }
            try
            {
                ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                dsk.Get();
                HDD_ID = dsk["VolumeSerialNumber"].ToString();
            }
            catch { }
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                ManagementObjectCollection moc = mos.Get();
                foreach (ManagementObject mo in moc)
                {
                    MB_ID = (string)mo["SerialNumber"];
                }
            }
            catch { }


            return CPU_ID + HDD_ID + MB_ID;
        }
        public static string RequestGET(String URL)
        {
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(Uri.EscapeUriString(URL));

            StreamReader input;
            HttpWebResponse response;

            response = (HttpWebResponse)request.GetResponse();
            input = new StreamReader(response.GetResponseStream());
            return input.ReadToEnd().Replace("<br>", "\n").Replace("<br/>", "\n");
        }
        public static CookieContainer cookieJar = new CookieContainer();
        public static string RequestPOST(String URL, string postData, int timeout)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Uri.EscapeUriString(URL));
            request.CookieContainer = cookieJar;

            var data = Encoding.UTF8.GetBytes(postData);

            request.Timeout = timeout;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString.Replace("<br>", "\n").Replace("<br/>", "\n");
        }
        public static string GetLicenszTipStringnevFromInt()
        {
            if(Program.LICENSZTipus == 10 && Program.LICENSZProbTeljVerz == true)
            {
                return "Trial Full Version";
            }

            switch (Program.LICENSZTipus)
            {
                case 10:
                    {
                        return "Freemium";
                        break;
                    }
                case 20:
                    {
                        return "Full Version";
                        break;
                    }
            }

            return "N/A";
        }
    }
}
