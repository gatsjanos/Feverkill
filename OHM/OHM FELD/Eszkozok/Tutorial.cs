using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OpenHardwareMonitor.GUI
{
    class Tutorial
    {
        public enum TStat { NINCS = 0, Inditas, Szablistak_Letrehoz, Szablistak_Szerkeszt, Riasztasok, Direkt_Vez, Alapert_Ford, SzenzorGrafikon, Alaplap, Kiegeszites}
        public static TStat Statusz = TStat.NINCS;
        public enum Mboxok {Inditas, Feverkill, Szabalyzolistak_Letreh, Szabalyzolistak_Szerk, Szabalyzolistak_Osszevetese, Direkt_Vezerles, Alapert_Fordszam, Riasztasok, Alaplapi_Vez, Felul_Maradas, Gyorsbillentyuk, Celhardver_Elso_Haszn, Celhardver, Tutorial};
        static public void MBShow(Mboxok be)
        {
            switch (be)
            {
                case Mboxok.Inditas:
                    Tutorial.Statusz = Tutorial.TStat.Inditas;
                    break;
                case Mboxok.Feverkill:
                    MessageBox.Show("      A Feverkill két fő része: egy PC-n futó (továbbiakban Vezérlőszoftver) program és egy, a NEM PC-be épített hardverkomponensekre csatlakoztatott ventilátorok szabályzásához szükséges, a PC-vel összeköttetésben lévő, kiegészítő elektronika (továbbiakban célhardver). Ez a rendszer felhasználói beavatkozás nélkül, automatizáltan szabályozza a számítógép hűtését az aktuális hőmérsékletek alapján.", "Feverkill");
       
                    break;
                case Mboxok.Szabalyzolistak_Letreh:
                    MessageBox.Show("A vezérlőszoftver legfontosabb feladata a hőmérsékletfüggő vezérlés.\nEhhez Szabályzólistákat, vagy PID vezérlést használ, melyekben a következő adatok vannak\neltárolva:\n\t-A kívánt hőmérő\n\t-A vezérlésre kiválasztott csatornák\n\t-Az egyes hőmérsékletekhez tartozó fordulatszámok (Szabályzólista)\n\t-Célhőmérséklet + erősítő tényezők + vizsgálat intervallum (PID)\n\n\n   A kiválasztott hőmérőhöz beállítható, hogy a célhardver a kiválasztott csatornákra kötött ventilátorokat a kiválasztott hőmérő által mért hömérséklet nagyságától függően mekkora fordulatszámmal hajtsa.\n   A listákat, vagy PID vezérléseket a \"VEZÉRLÉS>>Szabályzólista Létrehozása\" menüpontban hozhatja létre.\n\n\nTUTORIAL: Kattintson a \"Haladó Ablak: VEZÉRLES>>Szabályzólista Létrehozása\" menüpontra.", "Szabályzólisták");
        
                    break;
                case Mboxok.Szabalyzolistak_Szerk:
                    MessageBox.Show("A szabályzólistákkal a \"VEZÉRLÉS>>Szabályzólisták Kezelése\" menüpontban végezhet további műveleteket.\n\n\nTUTORIAL: Kattintson a \"VEZÉRLÉS>>Szabályzólisták Kezelése\" menüpontra.");
                    
                    break;
                case Mboxok.Szabalyzolistak_Osszevetese:
                    MessageBox.Show("-Egy listán belül, ha a hőmérséklet két olyan érték között van,\n melyekhez be van álllítva eltérő fordulatszám, akkor a nagyobbik hőértékhez tartozó fordulatszám érvényesül.\n\n-Ha ugyanarra a csatornára több lista is vonatkozik eltérő fordulatszámmal,\n akkor a magasabb fordulatszám érvényesül.\n Ez akkor is így van, ha az egyazon csatornára\n vonatkozó listák nem ugyan azt a hőmérőt figyelik.", "Szabályzólisták Összevetése");
        
                    break;
                case Mboxok.Direkt_Vezerles:
                    MessageBox.Show("Ha egy csatornát manuálisan vezérel akár a Célhardveren, akár a belső ventilátorok közül, akkor a vezérlőszoftver az adott csatorná(ko)n nem szabályozza a ventilátor(ok) teljesítményét a hőmérséklet függvényében, hanem az adott csatornához megadott értéken tartja a fordulatszámot.\n A beállítás megadható, ha az Áttekintő ablakban rákattint egy adott csatornára az aktuális fordulatszámokat kijelző listában (balra fent).", "Manuális Vezérlés");
                    MessageBox.Show("A \"VEZÉRLÉS>>Manuális Vezérlés\" menüpontra kattintva egyszerre állíthatja az összes csatorna fordulatszámát a Célhardveren, 1%-os felbontásban.\n(Ezek az értékek a beállító ablakban található gombra kattintva lépnek életbe.)", "Manuális Vezérlés");
                    break;
                case Mboxok.Alapert_Fordszam:
                    MessageBox.Show("Ezzel a beállítással megadhatja, hogy a ventillátorok milyen sebességgel\npörögjenek a PC indításakor, míg a vezérlőszoftver jelet nem küld a hardvernek\na hömérséklet függvényében, vagyis míg a szoftver el nem indul.\n\n\nTUTORIAL: Kattintson a \"VEZÉRLÉS>>Indítási Fordulatszámok Beállítása\" menüpontra!", "Indítási Fordulatszám");
        
                    break;
                case Mboxok.Riasztasok:
                    MessageBox.Show("A program beállított hőmérsékleteknél képes riasztásokat generálni, akár hangjelzéssel is.\n\n\nTUTORIAL: Kattintson a \"VEZÉRLÉS>>Riasztás Létrehozása\" menüpontra.", "Riasztások");

                    break;
                case Mboxok.Alaplapi_Vez:
                    MessageBox.Show("A Haladó ablak fájában látható ventillátorvezérlés és fordulatszámok az alaplapra és egyéb hardverekre (pl. videókártya)\nkötött ventillátorokra vonatkoznak, és nem a Célhardverre kötöttekre!\n\nEzen ventilátorok manuálisan a Haladó ablakból, vagy az Áttekintő ablakból a Célhardver csatornáihoz hasonlatosan, a %-os teljesítményt tartalmazó sorra kattintva vezérelhetők.", "Alaplapi Vezérlés");
        
                    break;
                case Mboxok.Felul_Maradas:
                    MessageBox.Show("A \"Nézet->Felül Maradó\" menüpontra kattintva, vagy az Alt+3 billentyűkombinációt leütve állítható, hogy a szoftver ablakai mindvégig láthatóak maradjanak-e az egyéb alkalmazások ablakai felett is, vagy normál ablakként viselkedjenek.", "Felül Maradás");
        
                    break;
                case Mboxok.Gyorsbillentyuk:
                    MessageBox.Show("A Haladó ablak menüjében feltüntetett billentyűparancsok a program minden ablakában működnek,\n(az Áttekintő ablak kivételével)\nannak ellenére, hogy nem mindenhol jelenik meg a menü.\n\nAz Alt+F4 billentyűkombináció az aktuális ablakot zárja be, de nem lép ki a programból.\n(Utóbbit az Alt+F5 lenyomásával teheti meg.)", "Gyorsbillentyűk", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
                    break;
                case Mboxok.Celhardver_Elso_Haszn:
                    MessageBox.Show("\tCsatlakoztassa a Célhardver bemeneti tápcsatlakozóját a PC tápjának tetszőleges kimenetére, a ventilátorokat a célhardveren lévő tetszőleges kimenetekre és az USB kábelt egy tetszőleges USB portra. Ehhez a PC-t NEM szükséges leállítani, de ekkor az első újraindításig felmerülhetnek csatlakozási, stabilitási problémák.\n\tAz első csatlakoztatás alkalmával a számítógép\ntelepíteni fogja a kommunikációhoz szükséges drivert.", "Célhardver Első Használata", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    break;
                case Mboxok.Celhardver:
                    MessageBox.Show("A célhardver feladata, hogy a PC által az egyes csatornákra meghatározott sebességeken pörgesse a rájuk csatlakoztatott ventilátorokat.\nA célhardver csatlakozóin számok jelzik az egyes csatornákhoz tartozó kimeneteket."/* + "\n\n\n      KIEGÉSZÍTÉS: Egy csatornára több ventilátor is köthető, amíg ezek együttes áramfelvétele nem haladja meg a 2,75A-t (2750mA). Amennyiben olyan ventilátort akar használni, melynek önálló áramfelvétele meghaladja a 2,75A-t, úgy az azonos teljesítményt leadó csatornák minden további nélkül hidalhatók, hogy maximális teherbírásuk összeadódjon. (Hídba kötéshez két, vagy több csatorna NEGATÍV kimenetét kell összekötni, a POZITÍV kimenetet elegendő használni egyetlen csatornáról is (,ugyanis a Célhardver a 0 potenciált szaggatja))."*/, "Célhardver", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case Mboxok.Tutorial:
                    MessageBox.Show("Üdvözöljük!\n\nEz a tutoriál végig fogja vezetni önt a program és a mellékelt hardver használatának lépésein, részletekre való kitérés nélkül.\nA tutoriál használata során lehetősége van nem a javasolt sorrendben haladni és kihagyni lépéseket, bár ez nem ajánlott.\nAz ajánlott sorrend betartásához egyszerűen kövesse a megjelenő üzenetekben leírt utasításokat.\n\nA tutorial a későbbiekben is bármikor elindítható a \"Segítség>>Tutorial Mód Aktív\" menüpontra való kattintással, továbbá bármikor leállítható ugyanazon menüpont használatával.\n\nA tutorial során megjelenő utasítások mindig egy adott funkció használatára fogják felhívni a figyelmet. Miután egy funkció kezelését elsajátította, kattintson a \"Segítség>>Tutorial Léptetése\" menüpontra, vagy nyomja le a Ctrl+F6 billentyűkombinációt, hogy megismerkedhessen a következő funkcióval.\nAmikor egy adott funkciót mutat be a tutorial, akkor az ahhoz a funkcióhoz tartozó ablakok használata során folyamatosan segítő üzeneteket fog kapni, amig nem lépteti tovább a tutoriált(, vagy nem állítja le azt).", "Tutorial");
                    break;
                default:
                    break;
            }
        }

        static public void Leptet(FoAblak MFbe)
        {
            ++Statusz;

            switch (Statusz)
            {
                case TStat.NINCS:
                    break;
                case TStat.Inditas:
                    Tutorial.MBShow(Tutorial.Mboxok.Tutorial);
                    Tutorial.MBShow(Mboxok.Feverkill);
                    Tutorial.MBShow(Tutorial.Mboxok.Celhardver);
                    Tutorial.MBShow(Tutorial.Mboxok.Celhardver_Elso_Haszn);
                    MessageBox.Show("A célhardver csatlakoztatása mellőzhető a tutorial alatt, de ajánlott a jobb megértés érdekében.\nAmennyiben akár most, akár a későbbiekben célhardver nélkül akarja használni ezt az alkalmazást rendszermonitorozásra, alaplapi ventilátorvezérlésre és riasztások (későbbiekben kitérünk rájuk) generálására, úgy kattintson a \"Beállítások>>Célhardver Csatlakoztatva\" menüpontra a Haladó ablakban, vagy a Célhardver státuszát jelző címkére az Áttekintő ablakban!\n\nA következő lépésekben megkezdi a program funkcióinak megismerését.\n\nNe feledje, ha készen áll, kattintson a \"Segítség>>Tutorial Léptetése\" menüpontra.\n\n       A menüt is tartalmazó Haladó ablak megnyitásához az Áttekintő ablakban kattintson a jobbra fent található Ház ikonra!", "Átvezetés");
                    break;
                case TStat.Szablistak_Letrehoz:
                    MBShow(Mboxok.Szabalyzolistak_Letreh);
                    break;
                case TStat.Szablistak_Szerkeszt:
                    MBShow(Mboxok.Szabalyzolistak_Osszevetese);
                    MBShow(Mboxok.Szabalyzolistak_Szerk);
                    break;
                case TStat.Riasztasok:
                    MessageBox.Show("Ha látni akraja a célharver csatornáinak aktuális fordulatszámait, kattintson a \"Nézet>>Aktuális Fordulatszámok Mutatása/Rejtése\" menüpontra.", "Aktuális Fordulatszámok");
                    MBShow(Mboxok.Riasztasok);
                    break;
                case TStat.Direkt_Vez:
                    MessageBox.Show("A \"VEZÉRLÉS>>Riasztások Kezelése\" ablakban a <Del> billentyű megnyomásával törölhet kijelölt riasztásokat.", "Riasztások");
                    MBShow(Mboxok.Direkt_Vezerles);
                    break;
                case TStat.Alapert_Ford:
                    MessageBox.Show("A a célhardver összes csatornájának manuális vezérlésének feloldásához kattintson a \"VEZÉRLÉS>>Manuális Vezérlés Feloldása\" menüpontra.", "Manuális Vezérlés");
                    MBShow(Mboxok.Alapert_Fordszam);
                    break;
                case TStat.SzenzorGrafikon:
                    MessageBox.Show("A szenzorgrafikon képes vizuálisan megjeleníteni az egyes szenzorok által mért értékeket az idő függvényében.\nMegjelenítéséhez kattintson a \"Nézet>>Szenzorgrafikon Mutatása\" menüpontra.\n\nA megjeleníteni kívánt szenzorokat a Haladó ablak fájában megjelenő jelölőnégyzetekkel jelölheti ki.\nKattintson jobb egérgombbal a Szenzorgrafikon ablakán további lehetőségekért!\n\n(A Haladó Ablak:Beállítások menüsorban lehetőség van a szenzorok értékeinek naplózására is, megadható időközzel.)\n\n\nTUTORIAL: Kattintson a \"Nézet>>Szenzorgrafikon Mutatása\" menüpontra!", "Szenzorgrafikon");
                    break;
                case TStat.Alaplap:
                     MBShow(Mboxok.Alaplapi_Vez);
                    break;
                case TStat.Kiegeszites:
                    MBShow(Mboxok.Gyorsbillentyuk);
                    MBShow(Mboxok.Felul_Maradas);
                    MessageBox.Show("Ez a beállítás adja meg, hogy a program mekkora\nidőközönként kérje le a hőmérsékleti értékeket\nés állítsa a fordulatszámokat.\n\nA kisebb időköz nagyobb érzékenységet,\nugyanakkor nagyobb erőforrásigényt is jelent.\n\nA frissítési időközt a \"Beállítások>>Frissítési Időköz\" menüpont alatt, vagy az Áttelintő ablakban az ezt jelző címkére (balra lent) kattintva változtathatja.", "Frissítési időköz");
                    MessageBox.Show("A \"Segítség>>Játszótér\" menüpont alatt virtuális környezetet szimulálva tesztelheti az aktuálisan beállított Szabályzólisták, Riasztások, stb... együttes működését.", "Játszótér");
                    MessageBox.Show("Ez a beállítás megadja, hogy a program 3db monitorozó ablaka (Haladó ablak, Aktuális hőmérsékletek listája, Célhardver fordulatszámait jelző ablak) mennyire legyen átlátszatlan az egyéb alkalmazások ablakai felett.\nEzt a lehetőséget a \"Felül Maradás\" beállítás aktiválásával érdemes használni, így kis helyet foglaló, mindig látható, tetszőlegesen áttetsző ablakokkal folyamatosan megfigyelhetjük az egyes értékeket.\n\nAz opacitást a \"Nézet>>Opacitás\" menüpont alatt változtathatja.", "Opacitás");
                    MessageBox.Show("Opcionálisan csatlakoztathat külső hőszenzorokat a Célhardverhez, melyekhez a \"VEZÉRLÉS>>Biztonsági Hőszenzorok Beállítása\" menüpont alatt megadhat határértékeket. Amennyiben valamelyik szenzor túllépi a határértékét, a Célhardver az összes csatornáját 100%-ra kapcsolja, továbbá a Célhardver sípoló hangjelzést ad.\n\nÍly módon megelőzhető a károsodás, ha megszakadna a kapcsolat a Vezérlőszoftverrel.", "Biztonsági Hőszenzorok");
                    MessageBox.Show("Lehetőség van a Célhardver csatornáinak felcímkézésére, az egyes ventilátorok könyebb beazonosítása végett (például, hogy melyik csatornára kötött ventilátor hol helyezkedik el a számítógépben).\n A csatornák felcímkézhetők az Áttekintő ablakban egy adott csatornára kattintva az aktuális fordulatszámokat kijelző listában (balra fent).", "Csatornák Felcímkézése");
                    MessageBox.Show("Végére ért az ismertetőnek.\nRészletes beállításokért nézze át a Haladó ablakban található menüket, de általános használathoz elegendő az Áttekintő ablak használata, kiegészítve a Villanykörtéket ábrázoló gyorsgombokkal és a balra lent található három kattintható státuszjelző címkével.\n\n A tutorial bármikor újraindítható a \"Segítség>>Tutorial Mód Aktív\", vagy a \"Segítség>>Tutorial Léptetése\" menüpontokra, vagy az Áttekintő ablakban látható \"?\" gombra (jobbra fent) kattintva.\n\nAmennyiben további kérdése van, lépjen kapcsolatba a fejlesztő\ncsapattal a \"Haladó ablak: Segítség>>Visszajelzés Küldése\" menüpont alatt\n(adjon meg elérhetőséget az E-mail cím mezőben).");
                    MessageBox.Show("Köszönjük, hogy vásárlásával támogatta munkánkat!\n\nMagas Órajeleket és Alacsony Fordulatszámokat Kívánunk az elkövetkezendő időre!\n\t\t\tTisztelettel: A Fejlesztő Csapat Tagjai!");
                    MFbe.menuItem86.Checked = false;
                   // MFbe.menuItem86.PerformClick();
                    try { MFbe.TutorTH.Suspend(); } catch { try { MFbe.TutorTH.Suspend(); } catch { } }
                    MFbe.menuItemSegitseg.Text = "Segítség>";
                    MFbe.menuItem87.Text = "Tutorial Léptetése";
                    break;
                default:
                    break;
            }
        }
    }
}
