using System;
using System.Collections.Generic;

namespace TrickyAnimals
{
    /// <summary>
    /// Klasse Program. Bevat de methode Main en LosOp.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// De Main-methode. Het beginpunt van het programma, waar de programmabesturing begint en eindigt. Hier wordt het lees- en schrijfwerk van en naar de console gedaan.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        static void Main(string[] args)
        {
            // het aantal strings
            int aantal = int.Parse(Console.ReadLine());

            // inlezer en schrijver
            for (int x = 0; x < aantal; x++)
            {
                string s = Console.ReadLine();
                Console.WriteLine(LosOp(s));
            }
        }

        /// <summary>
        /// Deze methode lost de puzzel op met de gegeven string als startpunt.
        /// </summary>
        /// <returns>De oplossing.</returns>
        /// <param name="input">de string die wordt opgelost.</param>
        static string LosOp(string input)
        {
            //declaratie van de string met aantal uitgevoerde acties
            string aantalstappen;

            // oplossing bepalen
            char[] tesorteren = input.ToCharArray();
            Array.Sort(tesorteren);
            string oplossing = new string(tesorteren);

            // Initialisatie HashSet waarmee we kunnen bepalen of de string al is voorgekomen
            HashSet<string> algezien = new HashSet<string>();
            // Initialisatie van een Queue van nodes bestaande uit toestanden
            Queue<Toestand> nodes = new Queue<Toestand>();

            // De (begin)toestand "Root" bestaat uit de string die wordt meegegeven aan de LosOp-methode en een lege route
            Toestand Root = new Toestand(input, "");
            // Is de de string van de begintoestand de oplossing? Zo ja, return 0. Anders, ga verder.
            if (Root.Letters == oplossing)
                return "0";

            nodes.Enqueue(Root); // Voeg de begintoestand toe aan de Queue.

            // Opdrachten worden uitgevoerd zolang en de Queue niet leeg is.
            while (nodes.Count != 0)
            {
                // De (volgende) toestand wordt uit de queue gehaald. Dit is de voorste node in de Queue.
                Toestand voorstenodeinrij = nodes.Dequeue();

                /*
                Er worden nieuwe toestanden (afstammelingen) gemaakt door acties(a, b, x) uit te voeren met de bovenstaande node. 
                Hiervoor wordt de methode MaakAfstammeling aangeroepen.
                Bij aanroep van MaakAfstammeling wordt de letter van de actie meegegeven aan de methode.               
                Als de nieuwe toestand de oplossing bevat, dan worden het aantal acties (stappen) en de route (oftewel de stappen zelf) gereturnt.
                De nieuwe toestanden worden toegevoegd aan de Queue en de HashSet als de lettervolgorde nieuw is.
                */
                Toestand avanafvoorste = voorstenodeinrij.MaakAfstammeling('a');
                if (avanafvoorste.Letters == oplossing)
                {
                    aantalstappen = avanafvoorste.Route.Length.ToString();
                    return aantalstappen + " " + avanafvoorste.Route;
                }

                if (!algezien.Contains(avanafvoorste.Letters))
                {
                    algezien.Add(avanafvoorste.Letters);
                    nodes.Enqueue(avanafvoorste);
                }

                Toestand bvanafvoorste = voorstenodeinrij.MaakAfstammeling('b');
                if (bvanafvoorste.Letters == oplossing)
                {
                    aantalstappen = bvanafvoorste.Route.Length.ToString();
                    return aantalstappen + " " + bvanafvoorste.Route;
                }

                if (!algezien.Contains(bvanafvoorste.Letters))
                {
                    algezien.Add(bvanafvoorste.Letters);
                    nodes.Enqueue(bvanafvoorste);
                }

                Toestand xvanafvoorste = voorstenodeinrij.MaakAfstammeling('x');
                if (xvanafvoorste.Letters == oplossing)
                {
                    aantalstappen = xvanafvoorste.Route.Length.ToString();
                    return aantalstappen + " " + xvanafvoorste.Route;
                }
                if (!algezien.Contains(xvanafvoorste.Letters))
                {
                    algezien.Add(xvanafvoorste.Letters);
                    nodes.Enqueue(xvanafvoorste);
                }
            }
            return "geen oplossing gevonden";
        }
    }

    /// <summary>
    /// De klasse Toestand. Objecten met type worden gebruikt als de knopen in de boom. 
    /// Toestand heeft de membervariabelen letters en route. 
    /// Een object van dit type beschrijft de letters en de route (van een node).
    /// Bovendien bevat deze klasse een methode die nieuwe toestanden returnt 
    /// door acties uit te voeren op een kopie van de waarden van het toestand-object die de aanroeper is.
    /// </summary>
    public class Toestand
    {
        // membervariabelen(properties)
        public string Letters { get; private set; }
        public string Route { get; private set; }

        // constructormethode
        public Toestand(string mijnletters, string mijnroute)
        {
            Letters = mijnletters;
            Route = mijnroute;
        }

        /// <summary>
        /// Voert een actie (a, b, of x) uit met de waarden van een toestand-object
        /// en returnt een nieuw toestand-object(dit is een afstammeling van het vorige toestand-object/node). 
        /// De cases stellen acties voor. Bij aanroep van de methode moet er een actieletter worden meegegeven.
        /// </summary>
        /// <returns>Een nieuw toestand-object</returns>
        /// <param name="actieletter">Actieletter.</param>
        public Toestand MaakAfstammeling(char actieletter)
        {
            string letterreeks = Letters; // kopie van de letters
            char[] characters = letterreeks.ToCharArray();  // hier wordt een array van characters gemaakt van de letterreeks
            string nieuwetoestandletters = "";
            string nieuwetoestandroute = Route; // kopie van de route
            int lengte = letterreeks.Length;

            switch (actieletter)
            {
                case 'a': // de eerste en tweede letter worden omgewisseld

                    characters[1] = letterreeks[0];
                    characters[0] = letterreeks[1];

                    nieuwetoestandletters = new string(characters); // maakt een nieuwe string
                    nieuwetoestandroute += "a"; // voegt de actieletter toe aan de route

                    break;

                case 'b': // de eennalaatste en laatste letter worden omgewisseld

                    characters[lengte - 1] = letterreeks[lengte - 2];
                    characters[lengte - 2] = letterreeks[lengte - 1];

                    nieuwetoestandletters = new string(characters);
                    nieuwetoestandroute += "b";

                    break;

                case 'x': // de waarden van de middelste characters (1 t/m n-1) worden een plek naar rechts opgeschoven
                    for (int n = 2; n <= lengte - 2; n++)
                        characters[n] = letterreeks[n - 1];
                    // de meest linker plaats van het midden krijgt de letter van de meest rechter plaats van het midden
                    characters[1] = letterreeks[lengte - 2];

                    nieuwetoestandletters = new string(characters);
                    nieuwetoestandroute += "x";

                    break;
            }
            return new Toestand(nieuwetoestandletters, nieuwetoestandroute);
        }
    }
}

