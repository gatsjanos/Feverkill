using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace OpenHardwareMonitor
{
    class Fajlkezelo
    {
        static public void HitelesitoAdatTitkositATLANBeolvas()
        {
            Program.LICENSZNev = Program.LICENSZEmail = Program.LICENSZID = Program.LICENSZJelszo = "-";

            try
            {
                string[] hitadatok = Vedelem.HitfajlVisszafejt("2UIdZHjF08o347qR7iuZ7w5dH", HitelesitoAdatBeolvas(false)).Split('\n');

                switch (BeolvasottHitAdatVerzio)
                {
                    case 1:
                        {

                            Program.LICENSZNev = hitadatok[0];
                            Program.LICENSZEmail = hitadatok[1];
                            Program.LICENSZID = hitadatok[2];
                            Program.LICENSZJelszo = hitadatok[3];
                            Program.LICENSZERVENYESSEG = hitadatok[4];
                            break;
                        }

                    case 2:
                        {

                            Program.LICENSZNev = hitadatok[0];
                            Program.LICENSZEmail = hitadatok[1];
                            Program.LICENSZID = hitadatok[2];
                            Program.LICENSZJelszo = hitadatok[3];
                            Program.LICENSZERVENYESSEG = hitadatok[4];
                            Program.LICENSZTipus = Convert.ToInt32(hitadatok[5]);
                            break;
                        }

                    default:
                        {
                            Program.LICENSZNev = hitadatok[0];
                            Program.LICENSZEmail = hitadatok[1];
                            Program.LICENSZID = hitadatok[2];
                            Program.LICENSZJelszo = hitadatok[3];
                            Program.LICENSZERVENYESSEG = hitadatok[4];
                            Program.LICENSZTipus = Convert.ToInt32(hitadatok[5]);
                        }
                        break;
                }
            }
            catch { }
        }
        static public void HitelesitoAdatTitkositATLANIr()
        {

            byte[] bt = Vedelem.HitfajlTitkosit("2UIdZHjF08o347qR7iuZ7w5dH", Program.LICENSZNev + "\n" + Program.LICENSZEmail + "\n" + Program.LICENSZID + "\n" + Program.LICENSZJelszo + "\n" + Program.LICENSZERVENYESSEG + "\n" + Program.LICENSZTipus.ToString());
            Fajlkezelo.HitelesitoAdatIr(bt, false);
        }
        static public void HitelesitoAdatTitkositOTTIr(byte[] adat)
        {
            HitelesitoAdatIr(adat, true);
        }
        static public byte[] HitelesitoAdatTitkositOTTBeolvas()
        {
            return HitelesitoAdatBeolvas(true);
        }
        static public void HitelesitoAdatIr(byte[] adat, bool titkositott)
        {
            BinaryWriter bw = null;
            try
            {

                if (!Directory.Exists("lic"))
                {
                    Directory.CreateDirectory("lic");
                    Thread.Sleep(300);
                }

                if (titkositott)
                    bw = new BinaryWriter(new FileStream("lic\\hitcr.ved", FileMode.Create));
                else
                    bw = new BinaryWriter(new FileStream("lic\\hit.ved", FileMode.Create));

                bw.Write((byte)Program.MENTVERZSZAM_HitAdat);
                bw.Write(adat);
                Thread.Sleep(200);

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "File Error -- Fájlkezelési hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }
            try
            {
                bw.Close();
            }
            catch { }
        }
        static int BeolvasottHitAdatVerzio = Program.MENTVERZSZAM_HitAdat;
        static public byte[] HitelesitoAdatBeolvas(bool titkositott)
        {
            byte[] kitomb = new byte[] { 1, 1 };

            string fajlnev;
            if (titkositott)
                fajlnev = "lic\\hitcr.ved";
            else
            {
                fajlnev = "lic\\hit.ved";
                kitomb = new byte[] { 5, 5 };
            }

            List<byte> ki = new List<byte>();
            try
            {
                if (File.Exists(fajlnev))
                {
                    BinaryReader br = new BinaryReader(new FileStream(fajlnev, FileMode.Open));
                    try
                    {
                        BeolvasottHitAdatVerzio = 0;
                        try
                        {
                            BeolvasottHitAdatVerzio = br.ReadByte();
                        }
                        catch
                        { }

                        switch (BeolvasottHitAdatVerzio)
                        {
                            case 1:
                            case 2:
                                {
                                    while (br.BaseStream.Position != br.BaseStream.Length)
                                    {
                                        ki.Add(br.ReadByte());
                                    }
                                    break;
                                }

                            default:
                                {
                                    //MessageBox.Show("A hitelesítési adatakoat tartalmazó fájl verziója nem támogatott!\nA program megpróbálja beolvasni az adatokat, ám ezek feltehetően hibás működéshez vezetnek.", "Nem támogatott fájlverzió!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    br.Close(); br = new BinaryReader(new FileStream(fajlnev, FileMode.Open));

                                    while (br.BaseStream.Position != br.BaseStream.Length)
                                    {
                                        ki.Add(br.ReadByte());
                                    }

                                    break;
                                }
                        }


                        Thread.Sleep(200);
                    }
                    catch (Exception ex) { }// MessageBox.Show(ex.ToString() + "\n\nAmennyiben a probléma nem szűnik meg, megoldást jelenthet a felhasználói adatokat tartalmazó fájlok eltávolítása.\n(<Telepítési mappa>\\lstk\\*.*)", "Fájlkezelési hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }

                    br.Close();

                }
            }
            catch
            { }

            if (ki.Count != 0)
            {
                kitomb = new byte[ki.Count];
                for (int i = 0; i < ki.Count; i++)
                {
                    kitomb[i] = ki[i];
                }
            }
            return kitomb;
        }
        static public void NyelvBeolvas(string nyelv)
        {
            try
            {
                if (File.Exists("loc\\" + nyelv + ".loc"))
                {
                    StreamReader sr = new StreamReader("loc\\" + nyelv + ".loc", Encoding.GetEncoding("utf-8"));

                    while (!sr.EndOfStream)
                    {
                        Program.LocDic.Add(sr.ReadLine().Split('{')[0].Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t"), sr.ReadLine().Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t"));
                        if (!sr.EndOfStream)
                            sr.ReadLine();
                    }
                    sr.Close();
                }
                else
                    Program.KONFNyelv = "hun";
            }
            catch
            {
                Program.KONFNyelv = "hun";
            }

        }
        static public Program.SzabLista DeepCopySzablista(Program.SzabLista SzlistaBe)
        {
            Program.SzabLista SzlistKi = new Program.SzabLista();

            SzlistKi.VezTipListaalapu = SzlistaBe.VezTipListaalapu;
            SzlistKi.Nev = SzlistaBe.Nev;
            SzlistKi.Homero = SzlistaBe.Homero;
            SzlistKi.Csatornak = SzlistaBe.Csatornak;

            SzlistKi.PWM = new byte[SzlistaBe.PWM.Length];
            for (int x = 0; x < SzlistaBe.PWM.Length; x++)
            {
                SzlistKi.PWM[x] = SzlistaBe.PWM[x];
            }

            SzlistKi.PIDObjektum = new PIDKezelo(SzlistaBe.PIDObjektum.Setpoint, SzlistaBe.PIDObjektum.Kp, SzlistaBe.PIDObjektum.Ki, SzlistaBe.PIDObjektum.Kd, SzlistaBe.PIDObjektum.BeleszamitasIntegralVisszamenolegms, SzlistaBe.PIDObjektum.BeleszamitasDerivaltVisszamenolegms);

            return SzlistKi;
        }
        static public List<Program.SzabLista> DeepCopySzablista(List<Program.SzabLista> SzlistakBe)
        {
            List<Program.SzabLista> SzlistakKi = new List<Program.SzabLista>(); ;

            for (int i = 0; i < SzlistakBe.Count; i++)
            {
                SzlistakKi.Add(DeepCopySzablista(SzlistakBe[i]));
            }

            return SzlistakKi;
        }
        static public bool KiirtKONFTESZT()
        {
            if ((double)Program.KIIRTKONF[0] != Program.KONFOpacitas)
                return false;
            if ((bool)Program.KIIRTKONF[1] != Program.KONFFrissitesInditaskor)
                return false;
            if ((bool)Program.KIIRTKONF[2] != Program.KONFFelulMarado)
                return false;
            if ((bool)Program.KIIRTKONF[3] != Program.KONFHomersMutat)
                return false;
            if ((bool)Program.KIIRTKONF[4] != Program.KONFKittenyMutat)
                return false;
            if ((bool)Program.KIIRTKONF[5] != Program.KONFKismeretIndit)
                return false;
            if ((Size)Program.KIIRTKONF[6] != Program.KONFHomerokMeret)
                return false;
            if ((bool)Program.KIIRTKONF[7] != Program.KONFVanVezerlo)
                return false;
            if ((bool)Program.KIIRTKONF[8] != Program.KONFAutoIndul)
                return false;
            if ((int)Program.KIIRTKONF[9] != Program.KONFFrisIdo)
                return false;
            //if ((bool)Program.KIIRTKONF[10] != Program.KONFElsoInd)
            //return false;
            if ((bool)Program.KIIRTKONF[10] != Program.KONFAttekintMutat)
                return false;
            if ((float)Program.KIIRTKONF[11] != Program.KONFHiszterezis)
                return false;
            if ((bool)Program.KIIRTKONF[12] != Program.KONFUdvKeperny)
                return false;
            if ((string)Program.KIIRTKONF[13] != Program.KONFNyelv)
                return false;
            if ((bool)Program.KIIRTKONF[14] != Program.KONFHDDKernel32Tiltas)
                return false;
            if ((bool)Program.KIIRTKONF[15] != Program.KONFKellMegNyelvvalasztas)
                return false;
            if ((bool)Program.KIIRTKONF[16] != Program.KONFTutorialMegjelenit)
                return false;
            if ((bool)Program.KIIRTKONF[17] != Program.KONFBetekintoMutat)
                return false;
            if ((int)Program.KIIRTKONF[18] != Program.KONFAutoIndKesleltetes)
                return false;
            if ((bool)Program.KIIRTKONF[19] != Program.KONFMindenPCIEszkBetolt)
                return false;
            if ((int)Program.KIIRTKONF[20] != Program.KONFBootIteracioSzam)
                return false;
            if ((int)Program.KIIRTKONF[21] != Program.KONFBootFrisIdo)
                return false;

            return true;
        }
        static public void KiirtKONFSync()
        {
            Program.KIIRTKONF.Clear();
            Program.KIIRTKONF.Add(Program.KONFOpacitas);
            Program.KIIRTKONF.Add(Program.KONFFrissitesInditaskor);
            Program.KIIRTKONF.Add(Program.KONFFelulMarado);
            Program.KIIRTKONF.Add(Program.KONFHomersMutat);
            Program.KIIRTKONF.Add(Program.KONFKittenyMutat);
            Program.KIIRTKONF.Add(Program.KONFKismeretIndit);
            Program.KIIRTKONF.Add(Program.KONFHomerokMeret);
            Program.KIIRTKONF.Add(Program.KONFVanVezerlo);
            Program.KIIRTKONF.Add(Program.KONFAutoIndul);
            Program.KIIRTKONF.Add(Program.KONFFrisIdo);
            //Program.KIIRTKONF.Add(Program.KONFElsoInd);
            Program.KIIRTKONF.Add(Program.KONFAttekintMutat);
            Program.KIIRTKONF.Add(Program.KONFHiszterezis);
            Program.KIIRTKONF.Add(Program.KONFUdvKeperny);
            Program.KIIRTKONF.Add(Program.KONFNyelv);
            Program.KIIRTKONF.Add(Program.KONFHDDKernel32Tiltas);
            Program.KIIRTKONF.Add(Program.KONFKellMegNyelvvalasztas);
            Program.KIIRTKONF.Add(Program.KONFTutorialMegjelenit);
            Program.KIIRTKONF.Add(Program.KONFBetekintoMutat);
            Program.KIIRTKONF.Add(Program.KONFAutoIndKesleltetes);
            Program.KIIRTKONF.Add(Program.KONFMindenPCIEszkBetolt);
            Program.KIIRTKONF.Add(Program.KONFBootIteracioSzam);
            Program.KIIRTKONF.Add(Program.KONFBootFrisIdo);
        }
        static public void FoKonfBeolvas()
        {
            string FileNev = "lstk\\fokonf.kf";

            if (!File.Exists(FileNev))
                File.CreateText(FileNev);
            else
            {
                StreamReader sr = new StreamReader(FileNev);
                try
                {
                    if (!sr.EndOfStream)
                    {
                        int IverzBuff = 0;
                        string SverzBuff = "";
                        SverzBuff = sr.ReadLine();
                        try
                        {
                            IverzBuff = Convert.ToInt32(SverzBuff);
                        }
                        catch { }
                        switch (IverzBuff)
                        {
                            case 5:
                                {
                                    int KONFHomerokMeretWidth = -1;
                                    int KONFHomerokMeretHeight = -1;
                                    while (!sr.EndOfStream)
                                    {
                                        try
                                        {
                                            string sor = sr.ReadLine();
                                            if (sor.Contains("=") && sor.Split('=').Length > 1)
                                            {
                                                try
                                                {
                                                    string[] Tsor = sor.Split('=');
                                                    string kulcs = Tsor[0];
                                                    string ertek = "";
                                                    for (int i = 1; i < Tsor.Length; i++)
                                                    {
                                                        ertek += Tsor[i];
                                                    }
                                                    switch (kulcs)
                                                    {
                                                        case "KONFOpacitas":
                                                            {
                                                                Program.KONFOpacitas = Convert.ToDouble(ertek);
                                                                break;
                                                            }
                                                        case "KONFFrissitesInditaskor":
                                                            {
                                                                Program.KONFFrissitesInditaskor = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFFelulMarado":
                                                            {
                                                                Program.KONFFelulMarado = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFHomersMutat":
                                                            {
                                                                Program.KONFHomersMutat = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFKittenyMutat":
                                                            {
                                                                Program.KONFKittenyMutat = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFKismeretIndit":
                                                            {
                                                                Program.KONFKismeretIndit = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFVanVezerlo":
                                                            {
                                                                Program.KONFVanVezerlo = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFAutoIndul":
                                                            {
                                                                Program.KONFAutoIndul = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFAttekintMutat":
                                                            {
                                                                Program.KONFAttekintMutat = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFUdvKeperny":
                                                            {
                                                                Program.KONFUdvKeperny = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFHDDKernel32Tiltas":
                                                            {
                                                                Program.KONFHDDKernel32Tiltas = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFKellMegNyelvvalasztas":
                                                            {
                                                                Program.KONFKellMegNyelvvalasztas = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFTutorialMegjelenit":
                                                            {
                                                                Program.KONFTutorialMegjelenit = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFBetekintoMutat":
                                                            {
                                                                Program.KONFBetekintoMutat = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFMindenPCIEszkBetolt":
                                                            {
                                                                Program.KONFMindenPCIEszkBetolt = Convert.ToBoolean(ertek);
                                                                break;
                                                            }
                                                        case "KONFNyelv":
                                                            {
                                                                Program.KONFNyelv = Convert.ToString(ertek);
                                                                break;
                                                            }
                                                        case "KONFHiszterezis":
                                                            {
                                                                Program.KONFHiszterezis = float.Parse(ertek);
                                                                break;
                                                            }
                                                        case "KONFFrisIdo":
                                                            {
                                                                Program.KONFFrisIdo = Convert.ToInt32(ertek);
                                                                break;
                                                            }
                                                        case "KONFAutoIndKesleltetes":
                                                            {
                                                                Program.KONFAutoIndKesleltetes = Convert.ToInt32(ertek);
                                                                break;
                                                            }
                                                        case "KONFHomerokMeret.Height":
                                                            {
                                                                KONFHomerokMeretHeight = Convert.ToInt32(ertek);
                                                                break;
                                                            }
                                                        case "KONFHomerokMeret.Width":
                                                            {
                                                                KONFHomerokMeretWidth = Convert.ToInt32(ertek);
                                                                break;
                                                            }
                                                        case "KONFBootIteracioSzam":
                                                            {
                                                                Program.KONFBootIteracioSzam = Convert.ToInt32(ertek);
                                                                break;
                                                            }
                                                        case "KONFBootFrisIdo":
                                                            {
                                                                Program.KONFBootFrisIdo = Convert.ToInt32(ertek);
                                                                break;
                                                            }
                                                    }
                                                }
                                                catch { }
                                            }
                                        }
                                        catch { }
                                    }
                                    if (KONFHomerokMeretWidth > 0 && KONFHomerokMeretHeight > 0)
                                        Program.KONFHomerokMeret = new Size(KONFHomerokMeretWidth, KONFHomerokMeretHeight);

                                    break;
                                }
                            default:
                                {
                                    if (!sr.EndOfStream)
                                    { Program.KONFOpacitas = Convert.ToDouble(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFFrissitesInditaskor = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFFelulMarado = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFHomersMutat = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFKittenyMutat = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFKismeretIndit = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFHomerokMeret = new Size(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine())); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFVanVezerlo = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFAutoIndul = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFFrisIdo = Convert.ToInt32(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFAttekintMutat = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFHiszterezis = float.Parse(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFUdvKeperny = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFNyelv = Convert.ToString(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFHDDKernel32Tiltas = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFKellMegNyelvvalasztas = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFTutorialMegjelenit = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFBetekintoMutat = Convert.ToBoolean(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFAutoIndKesleltetes = Convert.ToInt32(sr.ReadLine()); }
                                    if (!sr.EndOfStream)
                                    { Program.KONFMindenPCIEszkBetolt = Convert.ToBoolean(sr.ReadLine()); }

                                    break;
                                }
                        }
                    }
                }
                catch { }
            }
        }
        static public void FoKonfMento()
        {
            try
            {

                if (!Directory.Exists("lstk"))
                {
                    Directory.CreateDirectory("lstk");
                    Thread.Sleep(300);
                }

                System.IO.StreamWriter sw = new System.IO.StreamWriter("lstk\\fokonf.kf");
                sw.WriteLine(Program.MENTVERZSZAM_fokonf);

                sw.WriteLine("KONFOpacitas=" + Program.KONFOpacitas);
                sw.WriteLine("KONFFrissitesInditaskor=" + Program.KONFFrissitesInditaskor);
                sw.WriteLine("KONFFelulMarado=" + Program.KONFFelulMarado);
                sw.WriteLine("KONFHomersMutat=" + Program.KONFHomersMutat);
                sw.WriteLine("KONFKittenyMutat=" + Program.KONFKittenyMutat);
                sw.WriteLine("KONFKismeretIndit=" + Program.KONFKismeretIndit);
                sw.WriteLine("KONFHomerokMeret.Width=" + Program.KONFHomerokMeret.Width);
                sw.WriteLine("KONFHomerokMeret.Height=" + Program.KONFHomerokMeret.Height);
                sw.WriteLine("KONFVanVezerlo=" + Program.KONFVanVezerlo);
                sw.WriteLine("KONFAutoIndul=" + Program.KONFAutoIndul);
                sw.WriteLine("KONFFrisIdo=" + Program.KONFFrisIdo);
                //sw.WriteLine("KONFElsoInd=" + Program.KONFElsoInd);
                sw.WriteLine("KONFAttekintMutat=" + Program.KONFAttekintMutat);
                sw.WriteLine("KONFHiszterezis=" + Program.KONFHiszterezis);
                sw.WriteLine("KONFUdvKeperny=" + Program.KONFUdvKeperny);
                sw.WriteLine("KONFNyelv=" + Program.KONFNyelv);
                sw.WriteLine("KONFHDDKernel32Tiltas=" + Program.KONFHDDKernel32Tiltas);
                sw.WriteLine("KONFKellMegNyelvvalasztas=" + Program.KONFKellMegNyelvvalasztas);
                sw.WriteLine("KONFTutorialMegjelenit=" + Program.KONFTutorialMegjelenit);
                sw.WriteLine("KONFBetekintoMutat=" + Program.KONFBetekintoMutat);
                sw.WriteLine("KONFAutoIndKesleltetes=" + Program.KONFAutoIndKesleltetes);
                sw.WriteLine("KONFMindenPCIEszkBetolt=" + Program.KONFMindenPCIEszkBetolt);
                sw.WriteLine("KONFBootIteracioSzam=" + Program.KONFBootIteracioSzam);
                sw.WriteLine("KONFBootFrisIdo=" + Program.KONFBootFrisIdo);

                System.Threading.Thread.Sleep(10);
                sw.Close();
                System.Threading.Thread.Sleep(10);

                Fajlkezelo.KiirtKONFSync();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString() + "\n\nAmennyiben a probléma nem szűnik meg, megoldást jelenthet a felhasználói adatokat tartalmazó fájlok eltávolítása.\n(<Telepítési mappa>\\lstk\\*.*)", "Fájlkezelési hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }

        }
        static public void LstkTeszt()
        {
            if (!Directory.Exists("lstk"))
            {
                Directory.CreateDirectory("lstk");
                Thread.Sleep(300);
            }

            if (!Directory.Exists("lic"))
            {
                Directory.CreateDirectory("lic");
                Thread.Sleep(300);
            }

            if (!File.Exists("lstk\\listak.lstk"))
            {
                File.CreateText("lstk\\listak.lstk").Close();
                Thread.Sleep(10);
            }

            if (!File.Exists("lstk\\ervenyesek.lstk"))
            {
                File.CreateText("lstk\\ervenyesek.lstk").Close();
                Thread.Sleep(10);
            }

            if (!File.Exists("lstk\\riaszt.hrf"))
            {
                File.CreateText("lstk\\riaszt.hrf").Close();
                Thread.Sleep(10);
            }

            if (!File.Exists("lstk\\fokonf.kf"))
            {
                File.CreateText("lstk\\fokonf.kf").Close();
                Thread.Sleep(10);
            }

            if (!File.Exists("lstk\\cscimk.slst"))
            {
                File.CreateText("lstk\\cscimk.slst").Close();
                Thread.Sleep(10);
            }
        }
        static public List<Program.SzabLista> SZabListBeolvas()
        {
            return SZListFajlBeolvas("lstk\\listak.lstk");
        }
        static public List<Program.SzabLista> ErvenyListBeolvas()
        {
            return SZListFajlBeolvas("lstk\\ervenyesek.lstk");
        }

        private static List<Program.SzabLista> SZListFajlBeolvas(string fajlnev)
        {

            string FileNev = fajlnev;
            List<Program.SzabLista> SzabListak = new List<Program.SzabLista>();

            if (!File.Exists(FileNev))
                File.CreateText(FileNev);
            else
            {
                StreamReader sr = new StreamReader(FileNev, Encoding.GetEncoding("utf-8"));
                try
                {
                    if (!sr.EndOfStream)
                    {
                        int IverzBuff = 0;
                        string SverzBuff = "";
                        SverzBuff = sr.ReadLine();
                        try
                        {
                            IverzBuff = Convert.ToInt32(SverzBuff);
                        }
                        catch { }

                        switch (IverzBuff)
                        {
                            case 4:
                                {
                                    while (!sr.EndOfStream)
                                    {
                                        Program.SzabLista SzabListx = new Program.SzabLista();

                                        SzabListx.Nev = sr.ReadLine();
                                        //SzabListx.Homero = byte.Parse(sr.ReadLine());
                                        SzabListx.Homero = sr.ReadLine();
                                        SzabListx.Csatornak = sr.ReadLine();

                                        SzabListx.PWM = new byte[46];

                                        for (int i = 0; i < 46; i++)
                                        {
                                            SzabListx.PWM[i] = byte.Parse(sr.ReadLine());
                                        }
                                        SzabListak.Add(SzabListx);

                                    }
                                    break;
                                }
                            case 5:
                                {
                                    while (!sr.EndOfStream)
                                    {
                                        Program.SzabLista SzabListx = new Program.SzabLista();

                                        SzabListx.Nev = sr.ReadLine();
                                        SzabListx.VezTipListaalapu = Convert.ToBoolean(sr.ReadLine());
                                        SzabListx.Homero = sr.ReadLine();
                                        SzabListx.Csatornak = sr.ReadLine();

                                        SzabListx.PWM = new byte[46];

                                        if (SzabListx.VezTipListaalapu)
                                        {
                                            for (int i = 0; i < 46; i++)
                                            {
                                                SzabListx.PWM[i] = byte.Parse(sr.ReadLine());
                                            }
                                        }
                                        else
                                        {
                                            SzabListx.PIDObjektum = new PIDKezelo();
                                            SzabListx.PIDObjektum.Setpoint = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.Kp = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.Ki = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.Kd = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.BeleszamitasIntegralVisszamenolegms = SzabListx.PIDObjektum.BeleszamitasDerivaltVisszamenolegms = Convert.ToInt32(sr.ReadLine());
                                        }
                                        SzabListak.Add(SzabListx);


                                    }
                                    break;
                                }
                            case 6:
                                {
                                    while (!sr.EndOfStream)
                                    {
                                        Program.SzabLista SzabListx = new Program.SzabLista();

                                        SzabListx.Nev = sr.ReadLine();
                                        SzabListx.VezTipListaalapu = Convert.ToBoolean(sr.ReadLine());
                                        SzabListx.Homero = sr.ReadLine();
                                        SzabListx.Csatornak = sr.ReadLine();

                                        SzabListx.PWM = new byte[46];

                                        if (SzabListx.VezTipListaalapu)
                                        {
                                            for (int i = 0; i < 46; i++)
                                            {
                                                SzabListx.PWM[i] = byte.Parse(sr.ReadLine());
                                            }
                                        }
                                        else
                                        {
                                            SzabListx.PIDObjektum = new PIDKezelo();
                                            SzabListx.PIDObjektum.Setpoint = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.Kp = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.Ki = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.Kd = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.BeleszamitasIntegralVisszamenolegms = Convert.ToInt32(sr.ReadLine());
                                            SzabListx.PIDObjektum.BeleszamitasDerivaltVisszamenolegms = Convert.ToInt32(sr.ReadLine());
                                        }
                                        SzabListak.Add(SzabListx);

                                    }
                                    break;
                                }
                            default:
                                {
                                    MessageBox.Show("A felhasználói adatakoat tartalmazó fájlok verziója nem támogatott!\nA program megpróbálja beolvasni az adatokat, ám ezek az adatok feltehetően hibás működéshez vezetnek.\n\nAmennyiben lehetősége van rá, távolítsa el ezeket a fájlokat!\n(<Telepítési mappa>\\lstk\\*.*)", "Nem támogatott fájlverzió!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    sr.Close(); sr = new StreamReader(FileNev, Encoding.GetEncoding("utf-8"));

                                    while (!sr.EndOfStream)
                                    {
                                        Program.SzabLista SzabListx = new Program.SzabLista();

                                        SzabListx.Nev = sr.ReadLine();
                                        SzabListx.VezTipListaalapu = Convert.ToBoolean(sr.ReadLine());
                                        SzabListx.Homero = sr.ReadLine();
                                        SzabListx.Csatornak = sr.ReadLine();

                                        SzabListx.PWM = new byte[46];

                                        if (SzabListx.VezTipListaalapu)
                                        {
                                            for (int i = 0; i < 46; i++)
                                            {
                                                SzabListx.PWM[i] = byte.Parse(sr.ReadLine());
                                            }
                                        }
                                        else
                                        {
                                            SzabListx.PIDObjektum = new PIDKezelo();
                                            SzabListx.PIDObjektum.Setpoint = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.Kp = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.Ki = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.Kd = Convert.ToDouble(sr.ReadLine());
                                            SzabListx.PIDObjektum.BeleszamitasIntegralVisszamenolegms = Convert.ToInt32(sr.ReadLine());
                                            SzabListx.PIDObjektum.BeleszamitasDerivaltVisszamenolegms = Convert.ToInt32(sr.ReadLine());
                                        }
                                        SzabListak.Add(SzabListx);

                                    }
                                    break;
                                }
                        }

                    }
                    Thread.Sleep(200);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString() + "\n\nAmennyiben a probléma nem szűnik meg, megoldást jelenthet a felhasználói adatokat tartalmazó fájlok eltávolítása.\n(<Telepítési mappa>\\lstk\\*.*)", "Fájlkezelési hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }

                sr.Close();
            }
            return SzabListak;
        }

        static public void ErvenyListIr(List<Program.SzabLista> SzabListak)
        {
            SZListFajlKiir(SzabListak, "lstk\\ervenyesek.lstk");
        }
        static public void SZabListIr(List<Program.SzabLista> SzabListak)
        {
            SZListFajlKiir(SzabListak, "lstk\\listak.lstk");
        }
        private static void SZListFajlKiir(List<Program.SzabLista> SzabListak, string fajlnev)
        {
            if (!Directory.Exists("lstk"))
            {
                Directory.CreateDirectory("lstk");
                Thread.Sleep(300);
            }

            StreamWriter sw = new StreamWriter(new FileStream(fajlnev, FileMode.Create), Encoding.GetEncoding("utf-8"));
            try
            {
                sw.WriteLine(Program.MENTVERZSZAM_listak);

                foreach (Program.SzabLista item in SzabListak)
                {
                    sw.WriteLine(item.Nev);
                    sw.WriteLine(item.VezTipListaalapu);
                    sw.WriteLine(item.Homero);
                    sw.WriteLine(item.Csatornak);

                    if (item.VezTipListaalapu)
                    {
                        for (int i = 0; i < 46; i++)
                        {
                            sw.WriteLine(item.PWM[i]);
                        }
                    }
                    else
                    {
                        sw.WriteLine(item.PIDObjektum.Setpoint);
                        sw.WriteLine(item.PIDObjektum.Kp);
                        sw.WriteLine(item.PIDObjektum.Ki);
                        sw.WriteLine(item.PIDObjektum.Kd);
                        sw.WriteLine(item.PIDObjektum.BeleszamitasIntegralVisszamenolegms);
                        sw.WriteLine(item.PIDObjektum.BeleszamitasDerivaltVisszamenolegms);
                    }
                }
                Thread.Sleep(200);

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString() + "\n\nAmennyiben a probléma nem szűnik meg, megoldást jelenthet a felhasználói adatokat tartalmazó fájlok eltávolítása.\n(<Telepítési mappa>\\lstk\\*.*)", "Fájlkezelési hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }
            sw.Close();
        }
        static public List<Program.Riasztas> RiasztasBeolvas()
        {
            string FileNev = "lstk\\riaszt.hrf";
            List<Program.Riasztas> Riasztasok = new List<Program.Riasztas>();

            if (!File.Exists(FileNev))
                File.CreateText(FileNev);
            else
            {
                StreamReader sr = new StreamReader(FileNev, Encoding.GetEncoding("utf-8"));
                try
                {
                    if (!sr.EndOfStream)
                    {
                        int IverzBuff = 0;
                        string SverzBuff = "";
                        SverzBuff = sr.ReadLine();
                        try
                        {
                            IverzBuff = Convert.ToInt32(SverzBuff);
                        }
                        catch { }

                        switch (IverzBuff)
                        {
                            case 4:
                                {
                                    Program.Riasztas Riasztasx;
                                    while (!sr.EndOfStream)
                                    {
                                        Riasztasx = new Program.Riasztas();

                                        Riasztasx.Homero = sr.ReadLine();
                                        Riasztasx.RiasztPont = int.Parse(sr.ReadLine());
                                        Riasztasx.Muvelet = sr.ReadLine();
                                        Riasztasx.Uzenet = sr.ReadLine();
                                        Riasztasx.Hangjelzes = (sr.ReadLine() == "1") ? true : false;
                                        Riasztasx.SpecMuvelet = sr.ReadLine();
                                        Riasztasx.EbresztIdo = int.Parse(sr.ReadLine());
                                        Riasztasok.Add(Riasztasx);
                                    }
                                    break;
                                }

                            default:
                                {
                                    MessageBox.Show("A felhasználói adatakoat tartalmazó fájlok verziója nem támogatott!\nA program megpróbálja beolvasni az adatokat, ám ezek az adatok feltehetően hibás működéshez vezetnek.\n\nAmennyiben lehetősége van rá, távolítsa el ezeket a fájlokat!\n(<Telepítési mappa>\\lstk\\*.*)", "Nem támogatott fájlverzió!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    sr.Close(); sr = new StreamReader(FileNev, Encoding.GetEncoding("utf-8"));
                                    Program.Riasztas Riasztasx;
                                    while (!sr.EndOfStream)
                                    {
                                        Riasztasx = new Program.Riasztas();

                                        Riasztasx.Homero = sr.ReadLine();
                                        Riasztasx.RiasztPont = int.Parse(sr.ReadLine());
                                        Riasztasx.Muvelet = sr.ReadLine();
                                        Riasztasx.Uzenet = sr.ReadLine();
                                        Riasztasx.Hangjelzes = (sr.ReadLine() == "1") ? true : false;
                                        Riasztasx.SpecMuvelet = sr.ReadLine();
                                        Riasztasx.EbresztIdo = int.Parse(sr.ReadLine());
                                        Riasztasok.Add(Riasztasx);
                                    }

                                    break;
                                }
                        }

                    }


                    Thread.Sleep(200);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString() + "\n\nAmennyiben a probléma nem szűnik meg, megoldást jelenthet a felhasználói adatokat tartalmazó fájlok eltávolítása.\n(<Telepítési mappa>\\lstk\\*.*)", "Fájlkezelési hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }

                sr.Close();
            }
            return Riasztasok;

        }
        static public void RiasztasIr(List<Program.Riasztas> Riasztasok)
        {
            if (!Directory.Exists("lstk"))
            {
                Directory.CreateDirectory("lstk");
                Thread.Sleep(300);
            }
            StreamWriter sw = new StreamWriter(new FileStream("lstk\\riaszt.hrf", FileMode.Create), Encoding.GetEncoding("utf-8"));
            try
            {
                sw.WriteLine(Program.MENTVERZSZAM_riaszt);

                foreach (Program.Riasztas item in Riasztasok)
                {
                    sw.WriteLine(item.Homero);
                    sw.WriteLine(item.RiasztPont);
                    sw.WriteLine(item.Muvelet);
                    sw.WriteLine(item.Uzenet);
                    sw.WriteLine(item.Hangjelzes ? "1" : "0");
                    sw.WriteLine(item.SpecMuvelet);
                    sw.WriteLine(item.EbresztIdo);

                }
                Thread.Sleep(200);

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString() + "\n\nAmennyiben a probléma nem szűnik meg, megoldást jelenthet a felhasználói adatokat tartalmazó fájlok eltávolítása.\n(<Telepítési mappa>\\lstk\\*.*)", "Fájlkezelési hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }
            sw.Close();
        }
        static public void CsCimkeIr()
        {
            if (!Directory.Exists("lstk"))
            {
                Directory.CreateDirectory("lstk");
                Thread.Sleep(300);
            }
            StreamWriter sw = new StreamWriter(new FileStream("lstk\\cscimk.slst", FileMode.Create), Encoding.GetEncoding("utf-8"));
            try
            {
                sw.WriteLine(Program.MENTVERZSZAM_csatcimke);

                for (int i = 0; i < Program.CsatCimkekCelh.Length; i++)
                {
                    sw.WriteLine(Program.CsatCimkekCelh[i]);
                }
                foreach (var item in Program.CsatCimkekBelso.Keys)
                {
                    if (Program.CsatCimkekBelso[item] != "")
                    {
                        sw.WriteLine(item);
                        sw.WriteLine(Program.CsatCimkekBelso[item]);
                    }
                }
                Thread.Sleep(200);

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString() + "\n\nAmennyiben a probléma nem szűnik meg, megoldást jelenthet a felhasználói adatokat tartalmazó fájlok eltávolítása.\n(<Telepítési mappa>\\lstk\\*.*)", "Fájlkezelési hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }
            sw.Close();
        }
        static public string[] CsCimkeBeolvas()
        {
            string FileNev = "lstk\\cscimk.slst";

            string[] CsCimkek = new string[8];
            for (int i = 0; i < CsCimkek.Length; i++)
            {
                CsCimkek[i] = "--";
            }

            if (!File.Exists(FileNev))
                File.CreateText(FileNev);
            else
            {
                StreamReader sr = new StreamReader(FileNev, Encoding.GetEncoding("utf-8"));
                try
                {
                    if (!sr.EndOfStream)
                    {
                        int IverzBuff = 0;
                        string SverzBuff = "";
                        SverzBuff = sr.ReadLine();
                        try
                        {
                            IverzBuff = Convert.ToInt32(SverzBuff);
                        }
                        catch { }

                        switch (IverzBuff)
                        {
                            case 1:
                                {
                                    for (int i = 0; i < CsCimkek.Length && !sr.EndOfStream; i++)
                                    {
                                        CsCimkek[i] = sr.ReadLine();
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    for (int i = 0; i < CsCimkek.Length && !sr.EndOfStream; i++)
                                    {
                                        CsCimkek[i] = sr.ReadLine();
                                    }

                                    while (!sr.EndOfStream)
                                    {
                                        string skey = sr.ReadLine();
                                        if (!sr.EndOfStream)
                                        {
                                            if (!Program.CsatCimkekBelso.ContainsKey(skey))
                                                Program.CsatCimkekBelso.Add(skey, sr.ReadLine());
                                        }
                                    }
                                    break;
                                }

                            default:
                                {
                                    MessageBox.Show("A felhasználói adatakoat tartalmazó fájlok verziója nem támogatott!\nA program megpróbálja beolvasni az adatokat, ám ezek az adatok feltehetően hibás működéshez vezetnek.\n\nAmennyiben lehetősége van rá, távolítsa el ezeket a fájlokat!\n(<Telepítési mappa>\\lstk\\*.*)", "Nem támogatott fájlverzió!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    sr.Close(); sr = new StreamReader(FileNev, Encoding.GetEncoding("utf-8"));
                                    for (int i = 0; i < CsCimkek.Length && !sr.EndOfStream; i++)
                                    {
                                        CsCimkek[i] = sr.ReadLine();
                                    }
                                    break;
                                }
                        }

                    }


                    Thread.Sleep(200);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString() + "\n\nAmennyiben a probléma nem szűnik meg, megoldást jelenthet a felhasználói adatokat tartalmazó fájlok eltávolítása.\n(<Telepítési mappa>\\lstk\\*.*)", "Fájlkezelési hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }

                sr.Close();
            }
            return CsCimkek;

        }
        static public string CsatbolString(string BeCsatorna)
        {
            string Ki = "";
            if (BeCsatorna.Contains(",8,"))
            { Ki = Ki.Insert(0, "8, "); }
            if (BeCsatorna.Contains(",7,"))
            { Ki = Ki.Insert(0, "7, "); }
            if (BeCsatorna.Contains(",6,"))
            { Ki = Ki.Insert(0, "6, "); }
            if (BeCsatorna.Contains(",5,"))
            { Ki = Ki.Insert(0, "5, "); }
            if (BeCsatorna.Contains(",4,"))
            { Ki = Ki.Insert(0, "4, "); }
            if (BeCsatorna.Contains(",3,"))
            { Ki = Ki.Insert(0, "3, "); }
            if (BeCsatorna.Contains(",2,"))
            { Ki = Ki.Insert(0, "2, "); }
            if (BeCsatorna.Contains(",1,"))
            { Ki = Ki.Insert(0, "1, "); }

            if (Ki.Length > 1)
                Ki = Ki.Remove(Ki.Length - 2, 2);


            string[] alaplapi = BeCsatorna.Split(',')[BeCsatorna.Split(',').Length - 1].Split('|');

            if (Ki.Length != 0 && alaplapi.Length > 1)
                Ki += " + ";

            for (int i = 1; i < alaplapi.Length; i++)
            {
                Ki += alaplapi[i] + "          ";
            }


            return Ki;
        }
        //static public void SZenzorBetolto(OpenHardwareMonitor.GUI.MainForm MF)
        //{
        //    string[] Ertekek = MF.computer.Szenzorertekek();



        //    Program.Szenzorok.Clear();
        //    for (int i = 0; i < Ertekek.Length; i++)
        //    {
        //        if (Ertekek[i].ToLower().Contains("temperature") || Ertekek[i].ToLower().Contains("hőmérséklet") || Ertekek[i].ToLower().Contains("homérséklet") || Ertekek[i].ToLower().Contains("homerseklet") || Ertekek[i].ToLower().Contains("hőmerseklet"))
        //        {
        //            for (int x = i; x >= 0; --x)
        //            {
        //                if (Ertekek[x].Contains("\\"))
        //                {
        //                    Program.Szenzorok.Add(Ertekek[x].Split('\\')[Ertekek[x].Split('\\').Length - 1] + " =->> " + Ertekek[i].Split('|')[Ertekek[i].Split('|').Length - 1].Split(':')[0]);
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //}

        //static List<byte> NyugtaLista = new List<byte>();
        static public bool UARTbajtKuldo(byte kod, byte[] adat)
        {
            try
            {
                Program.SorosPort.ReadExisting();
            }
            catch { }

            try
            {
                // NyugtaLista.Clear();
                Program.SorosPort.Write(new byte[] { (byte)(kod + Program.UARTKod100) }, 0, 1);
                Thread.Sleep(2);
                Program.SorosPort.Write(adat, 0, adat.Length);
                Program.SorosPort.WriteLine("");
                Thread.Sleep(100);

                return UARTNyugtazasFogad(kod, adat);
            }
            catch
            {
                return false;
            }
        }
        static public bool UARTbajtKuldo(byte[] adat)
        {
            try
            {
                Program.SorosPort.Write(adat, 0, adat.Length);
                Program.SorosPort.WriteLine("");
                Thread.Sleep(5);
                return true;
            }
            catch
            {
                return false;
            }
        }
        static public bool UARTbajtKuldo(byte adat)
        {
            try
            {
                Program.SorosPort.ReadExisting();
            }
            catch { }
            try
            {
                Program.SorosPort.Write(new byte[] { adat }, 0, 1);
                Program.SorosPort.WriteLine("");
                Thread.Sleep(5);

                //string ss = Program.SorosPort.ReadExisting();
                //if(ss[0] == adat)
                return true;
            }
            catch { }

            return false;
        }
        static public bool UARTNyugtazasFogad(byte kod, byte[] kuldott)
        {
            try
            {
                string ss = Program.SorosPort.ReadExisting();
                char x = ss[0];
                if (ss[0] != (byte)(kod + Program.UARTKod100))
                    return false;

                for (int i = 0; i < kuldott.Length - 1; ++i)
                {
                    if (ss[i + 1] != kuldott[i])
                        return false;
                }

                if (ss[ss.Length - 2] != 220 && ss[ss.Length - 2] != 120) //utolsó küldött érték elé beszúrt byte: régi célhardver:120, új célhardver: 220
                    return false;

                if (ss[ss.Length - 1] != kuldott[kuldott.Length - 1]) //utolsó küldött érték
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
            //Ha egy bizonyos időlimiten belül nem érkezik meg a kód érték és a 8 db tényező, ill ezek nem egyeznek, akkor a visszatérési érték: false
            //try
            //{
            //    string kiolvasott = Program.SorosPort.ReadExisting();

            //    if (kiolvasott[0] != kod)
            //        return false;

            //    for (int i = 1; i < kiolvasott.Length - 1; ++i)
            //    {
            //        if (kiolvasott[i] != kuldott[i - 1])
            //            return false;
            //    }

            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }
        static public void COM_AdatFogad(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string s = sp.ReadExisting();
            //NyugtaLista.Add(Convert.ToByte(sp.ReadExisting()[0]));
        }
        static public int HanyadikEzAzElem(string Nev, List<Program.SzabLista> Szlistak)
        {
            for (int i = 0; i < Szlistak.Count; i++)
            {
                if (Szlistak[i].Nev == Nev)
                    return i;
            }

            return 0;
        }
        static public int HomeroIndexMegallapito(List<Program.HoMers> HoMersek, string HoMero)
        {
            for (int i = 0; i < HoMersek.Count; ++i)
            {
                if (HoMero == HoMersek[i].Csop + " =->> " + HoMersek[i].Nev)
                    return i;
            }
            return 0;
        }

        static public int HanyadikEzAzElemSZERKESZTES(string Nev, List<Program.SzabLista> Szlistak)
        {
            for (int i = 0; i < Szlistak.Count; i++)
            {
                if (Szlistak[i].Nev == Nev)
                    return i;
            }

            return 0;
        }
    }
}
