using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Pacijent
{
    public string Oib { get; set; }
    public string Mbo { get; set; }
    public string ImePrezime { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public string Spol { get; set; }
    public string Dijagnoza { get; set; }

    public void Ispisi()
    {
        Console.WriteLine("OIB: " + Oib);
        Console.WriteLine("MBO: " + Mbo);
        Console.WriteLine("Ime i prezime: " + ImePrezime);
        Console.WriteLine("Datum rođenja: " + DatumRodjenja.ToString("dd.MM.yyyy."));
        Console.WriteLine("Spol: " + Spol);
        Console.WriteLine("Dijagnoza: " + Dijagnoza);
    }
}

class Program
{
    static List<Pacijent> pacijenti = new List<Pacijent>();
    static string datoteka = "pacijenti.json";

    static void Main()
    {
        UcitajPodatkeIzDatoteke();

        string odabir;

        do
        {
            Console.WriteLine("Odaberite radnju:");
            Console.WriteLine("1. Zaprimi pacijenta");
            Console.WriteLine("2. Otpusti pacijenta");
            Console.WriteLine("3. Izmijeni podatke o pacijentu");
            Console.WriteLine("4. Ispiši podatke o svim pacijentima");
            Console.WriteLine("0. Izlaz");

            odabir = Console.ReadLine();

            switch (odabir)
            {
                case "1":
                    ZaprimiPacijenta();
                    break;
                case "2":
                    OtpustiPacijenta();
                    break;
                case "3":
                    IzmijeniPodatkeOPacijentu();
                    break;
                case "4":
                    IspisiPodatkeOSvimPacijentima();
                    break;
                case "0":
                    Console.WriteLine("Doviđenja!");
                    break;
                default:
                    Console.WriteLine("Neispravan odabir.");
                    break;
            }

            SpremiPodatkeUDatoteku();
        } while (odabir != "0");
    }

    static void ZaprimiPacijenta()
    {
        Pacijent noviPacijent = new Pacijent();

        Console.Write("Unesite OIB pacijenta: ");
        noviPacijent.Oib = Console.ReadLine();

        while (noviPacijent.Oib.Length != 11)
        {
            Console.Write("OIB mora imati 11 znamenki. Unesite ponovno: ");
            noviPacijent.Oib = Console.ReadLine();
        }

        Console.Write("Unesite MBO pacijenta: ");
        noviPacijent.Mbo = Console.ReadLine();

        while (noviPacijent.Mbo.Length != 9)
        {
            Console.Write("MBO mora imati 9 znamenki. Unesite ponovno: ");
            noviPacijent.Mbo = Console.ReadLine();
        }

        Console.Write("Unesite ime i prezime pacijenta: ");
        noviPacijent.ImePrezime = Console.ReadLine();

        Console.Write("Unesite datum rođenja pacijenta (dd.mm.yyyy.): ");
        string unosDatuma = Console.ReadLine();
        DateTime datum;

        while (!DateTime.TryParseExact(unosDatuma, "dd.MM.yyyy.", null, System.Globalization.DateTimeStyles.None, out datum))
        {
            Console.Write("Neispravan unos. Unesite ponovno (dd.mm.yyyy.): ");
            unosDatuma = Console.ReadLine();
        }
        noviPacijent.DatumRodjenja = datum;

        Console.Write("Unesite spol pacijenta (M ili Z): ");
        noviPacijent.Spol = Console.ReadLine();

        while (noviPacijent.Spol != "M" && noviPacijent.Spol != "Z")
        {
            Console.Write("Neispravan unos. Unesite ponovno (M ili Z): ");
            noviPacijent.Spol = Console.ReadLine();
        }

        Console.Write("Unesite dijagnozu pacijenta: ");
        noviPacijent.Dijagnoza = Console.ReadLine();

        pacijenti.Add(noviPacijent);

        Console.WriteLine("Pacijent uspješno zaprimljen.");
    }

    static void OtpustiPacijenta()
    {
        Console.Write("Unesite OIB pacijenta kojeg želite otpustiti: ");
        string oib = Console.ReadLine();

        Pacijent pacijent = pacijenti.Find(p => p.Oib == oib);

        if (pacijent == null)
        {
            Console.WriteLine("Pacijent nije pronađen.");
            return;
        }

        pacijenti.Remove(pacijent);

        Console.WriteLine("Pacijent uspješno otpušten.");
    }

    static void IzmijeniPodatkeOPacijentu()
    {
        Console.Write("Unesite OIB pacijenta čije podatke želite izmijeniti: ");
        string oib = Console.ReadLine();

        Pacijent pacijent = pacijenti.Find(p => p.Oib == oib);

        if (pacijent == null)
        {
            Console.WriteLine("Pacijent nije pronađen.");
            return;
        }

        Console.WriteLine("Unesite nove podatke:");

        Console.Write("OIB: ");
        string noviOib = Console.ReadLine();

        while (noviOib.Length != 11)
        {
            Console.Write("OIB mora imati 11 znamenki. Unesite ponovno: ");
            noviOib = Console.ReadLine();
        }

        Console.Write("MBO: ");
        string noviMbo = Console.ReadLine();

        while (noviMbo.Length != 9)
        {
            Console.Write("MBO mora imati 9 znamenki. Unesite ponovno: ");
            noviMbo = Console.ReadLine();
        }

        Console.Write("Ime i prezime: ");
        noviOib = noviOib.Trim();

        if (pacijenti.Exists(p => p.Oib == noviOib && p != pacijent))
        {
            Console.WriteLine("Pacijent s unesenim OIB-om već postoji.");
            return;
        }

        pacijent.Oib = noviOib;
        pacijent.Mbo = noviMbo;

        Console.Write("Datum rođenja (dd.MM.yyyy.): ");
        string datumRodjenja = Console.ReadLine();
        DateTime noviDatumRodjenja;

        while (!DateTime.TryParseExact(datumRodjenja, "dd.MM.yyyy.", null, System.Globalization.DateTimeStyles.None, out noviDatumRodjenja))
        {
            Console.Write("Neispravan format. Unesite ponovno (dd.MM.yyyy.): ");
            datumRodjenja = Console.ReadLine();
        }

        pacijent.DatumRodjenja = noviDatumRodjenja;

        Console.Write("Spol (M/Z): ");
        string noviSpol = Console.ReadLine();

        while (noviSpol != "M" && noviSpol != "Z")
        {
            Console.Write("Neispravan unos. Unesite ponovno (M/Z): ");
            noviSpol = Console.ReadLine();
        }

        pacijent.Spol = noviSpol;

        Console.Write("Dijagnoza: ");
        pacijent.Dijagnoza = Console.ReadLine();

        Console.WriteLine("Podaci su uspješno izmijenjeni.");
    }

    static void IspisiPodatkeOSvimPacijentima()
    {
        foreach (Pacijent pacijent in pacijenti)
        {
            pacijent.Ispisi();
            Console.WriteLine();
        }
    }

    static void UcitajPodatkeIzDatoteke()
    {
        if (File.Exists(datoteka))
        {
            string json = File.ReadAllText(datoteka);
            pacijenti = JsonSerializer.Deserialize<List<Pacijent>>(json);
        }
    }

    static void SpremiPodatkeUDatoteku()
    {
        string json = JsonSerializer.Serialize(pacijenti);
        File.WriteAllText(datoteka, json);
    }
}

