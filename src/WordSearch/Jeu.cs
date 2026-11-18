using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;




namespace WordSearch
{
    class Jeu
    {
        private Dictionnaire dico;
        private Joueur[] joueurs;
        private Plateau current;                    ///On crée les attributs dont nous aurons besoin
        private List<Plateau> old;
        private int ttotal;
        private int tpartie;
        private int tour = 1;

        public Jeu()
        {
            dico = new Dictionnaire(Jeu.GetString("Langue (FR ou EN): ", new string[] { "FR", "EN" }));///Initialise un objet dico de type Dictionnaire en utilisant la méthode GetString pour demander à l'utilisateur de choisir une langue (francais ou anglais).

            joueurs = new Joueur[Jeu.GetInt("Entrer le nombre de joueurs")];///Initialise un tableau joueurs de type Joueur en utilisant la méthode GetInt pour demander à l'utilisateur de saisir le nombre de joueurs.

            for (int i = 1; i <= joueurs.Length; i++)
            {
                joueurs[i - 1] = new Joueur(Jeu.GetString("Entrer le nom du joueur " + i + " :", new string[0]));///Pour chaque joueur dans le tableau, utilise la méthode GetString pour demander à l'utilisateur de saisir le nom du joueur. Créez un nouvel objet Joueur pour chaque nom saisi et le stocke dans le tableau joueurs.
            }
            old = new List<Plateau>();              ///Initialise une liste d'objets Plateau nommée old.
            ttotal = Jeu.GetInt("Entrer Le temps total de la partie (en min)");     ///Utilise la méthode GetInt pour demander à l'utilisateur de saisir la durée totale de la partie en minutes et la stocke dans une variable ttotal.
            tpartie = Jeu.GetInt("Entrer Le temps de jeu d'un plateau");       ///Utilise la méthode GetInt pour demander à l'utilisateur de saisir la durée d'un plateau de jeu en minutes et la stocje dans une variable tpartie.
        }

        public void Play()//methode qui implémente la logique du jeu ( déroulement des tours du jeu pour chaque et calcule les score des joueurs e focntion du temps et du nombre de mot trouvé
        {
            Stopwatch sw = new Stopwatch(); // Déclare un nouvel objet "sw" de type Stopwatch et démarre le chronomètre
            sw.Start();
            while (tour<=5 && sw.Elapsed< TimeSpan.FromMinutes(ttotal))  //Boucle qui s'exécute tant que le numéro du tour actuel est inférieur ou égal à 5  et que le temps écoulé depuis le début de la partie est inférieur à la durée totale autorisée
            {
                foreach(Joueur joueur in joueurs)// ... crée un nouveau plateau de jeu en utilisant le dictionnaire "dico" et le numéro du tour actuel
                {
                    Plateau plateau = new Plateau(dico, tour);
                    current = plateau; // Stocke le plateau dans la variable "current"
                    int found = 0;
                    Stopwatch pwatch = new Stopwatch();// Déclare un nouvel objet "pwatch" de type Stopwatch et démarre le chronomètre
                    pwatch.Start(); 

                    while (found < current.ToFind.Length && pwatch.Elapsed < TimeSpan.FromMinutes(tpartie))   // Affiche des informations sur l'état actuel de la partie


                    {
                        Console.Clear();
                        Console.WriteLine("Tour "+ tour+  " de "   + joueur.Name); // on donne le tour, et le nom du joueur




                        Console.WriteLine("Temps total passé " + sw.ElapsedMilliseconds / 1000 + " sec");
                        Console.WriteLine("Temps sur le plateau passé " + pwatch.ElapsedMilliseconds / 1000 + " sec");
                        Console.WriteLine("Mots trouvés : " + found);
                        Console.WriteLine(current.ToString());
                        for (int k = 0; k < current.ToFind.Length; k++)
                        {
                            Console.WriteLine(current.ToFind[k]);
                        }

                      
                        int x = Jeu.GetInt("Entrer ligne :");
                        x--;
                        int y = Jeu.GetInt("Entrer colonne : ");
                        y--;    
                        string dir = Jeu.GetString("Entrer direction : ", new string[] {"N","S","E","O","NE","NO","SO","SE"}); // Demande à l'utilisateur de saisir la direction dans laquelle il souhaite placer le mot
                                                                                                                                // et stocke la valeur saisie dans la variable "dir"
                        string mot = Jeu.GetString("Entrer un mot : ", new string[0]);
                        bool good = false;

                        switch (dir)// Demande à l'utilisateur de saisir la direction dans laquelle il souhaite placer le mot
                                    // et stocke la valeur saisie dans la variable "dir"
                        {
                            case "N":
                                good = current.Test_Plateau(mot,x,y,"S-N");
                                break;
                            case "S":
                                good = current.Test_Plateau(mot, x, y, "N-S");
                                break;
                            case "E":
                                good = current.Test_Plateau(mot, x, y, "O-E");                  //on teste si un mot donnée a une position est egligible a etre placé dans le plateau
                                break;
                            case "O":
                                good = current.Test_Plateau(mot, x, y, "E-O");
                                break;
                            case "NE":
                                good = current.Test_Plateau(mot, x, y, "SO-NE");
                                break;
                            case "NO":
                                good = current.Test_Plateau(mot, x, y, "SE-NO");
                                break;

                            case "SE":
                                good = current.Test_Plateau(mot, x, y, "NO-SE");
                                break;
                            case "SO":
                                good = current.Test_Plateau(mot, x, y, "NE-SO");
                                break;
                            default:
                                good = false;
                                break;


                        }

                        if (good && !joueur.Found.Contains(mot)) // Si la fonction "Test_Plateau" renvoie "true",  le mot peut être placé sur le plateau, on incrémente
                                  // le compteur "found" et on ajoute le mot à la liste des mots trouvés par le joueur en cours
                        {


                            found++;
                            joueur.Add_mot(mot);
                        }
                    }
                    Console.WriteLine("Fin du tour de " +joueur.Name); // Quand le joueur a trouvé tous les mots du plateau ou que le
                                                                      // temps imparti est écoulé, on affiche un message indiquant la fin du tour du joueur et on met à jour son score en fonction du temps restant

                    pwatch.Stop();
                    if(pwatch.Elapsed< TimeSpan.FromMinutes(tpartie))
                    {
                        joueur.Add_Score(tpartie * 60 - (int)(pwatch.ElapsedMilliseconds / 1000)); // On ajoute également au score du joueur le nombre de mots trouvés
                    }

                    joueur.Add_Score(found);
                    old.Add(plateau);
                }
                tour++; // A la fin de chaque tour, on incrémente le compteur "tour" pour passer au tour suivant
            }

            sw.Stop();
            Console.WriteLine("Fin de la partie !"); // Quand la partie est finie, on affiche un message indiquant la fin de la
                                                     // partie et les détails des scores de chaque joueur
            Console.WriteLine("Détail des scores");
            foreach(Joueur joueur in joueurs)
            {
                Console.WriteLine(joueur.ToString());
            }
            Console.WriteLine("Appuyer sur une touche pour quitter");
            Console.ReadLine();

        }


        public static int GetInt(string prompt) ///La méthode GetInt affiche un message à l'utilisateur et attend qu'il entre un nombre entier supérieur à zéro. 
        {
            int res = 0;
            bool good = false;
            do
            {
                Console.WriteLine(prompt + " ( nombre > 0) :"); ////Affiche le message prompt à l'utilisateur en lui demandant de saisir un nombre entier supérieur à zéro.
                good = Int32.TryParse(Console.ReadLine(), out res);///Lit la réponse de l'utilisateur à partir de la console et tente de la convertir en entier en utilisant la méthode TryParse 

            } while (res <= 0 || !good); 
            ///Si la conversion a réussi et que le nombre est supérieur à zéro, la méthode renvoie le nombre entier. Sinon, la boucle recommence et l'utilisateur est invité à entrer un nombre valide.

            return res;
        }

        public static string GetString(string prompt, string[] valid)           ///la methode GetString affiche un message et attend qu'un utilisateur entre une chaîne de caractères
        {
            string res = "";
            bool good = false;
            do
            {
                Console.WriteLine(prompt);///Affiche le message prompt à l'utilisateur en lui demandant de saisir une chaîne de caractères.
                res = Console.ReadLine();//Lit la réponse de l'utilisateur à partir de la console et la stocke dans une variable res.
                if (valid.Length == 0 && !String.IsNullOrEmpty(res))
                    good = true;                ///Si le tableau valid est vide et que la chaîne res n'est pas vide, la boucle se termine et la méthode renvoie la chaîne res. 
                else if (valid.Contains(res)) ///Sinon, si la chaîne res se trouve dans le tableau valid, la boucle se termine et la méthode renvoie la chaîne res. Sinon, la boucle recommence et l'utilisateur est invité à entrer une chaîne valide.
                    good = true;
            } while (!good);
            ///Sinon, la boucle recommence et l'utilisateur est invité à entrer une chaîne valide.
            return res;
        }
    }
}