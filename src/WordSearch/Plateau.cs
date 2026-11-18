using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace WordSearch
{
    public class Plateau
    {

        public static string[] dirs = { "N-S", "O-E", "S-N", "E-O", "NE-SO", "NO-SE", "SO-NE", "SE-NO" };
        public static int[] diff = { 2, 4, 5, 6, 8 };////Correspond au 5 difficultés, si on prend 2, on prned les deux premier au dessus en direction N-S et O-E etc...

        private string[,] matrix;
        private int difficulty;
        private string[] toFind;//tableau de Mots a trouvés, il y a un nombre fixes de mots a trouvés
        private Dictionnaire dico;////Dictionnaire associé au plateau

        public string[,] Matrix => matrix;
        public int Difficulty => difficulty;
        public string[] ToFind => toFind;
        public Dictionnaire Dico => dico;

        public Plateau(string file, Dictionnaire dico)
        {
            this.dico = dico;
            ToRead(file);
        }

        public Plateau(Dictionnaire dico, int difficulty)////Création du constructeur qui prend comme parametre un dictionnaire et la difficulté
        {
            this.difficulty = difficulty;
            this.dico = dico;

            int maxLength = 0;////correspond a la taille maximal qu'auront les mots

            switch (difficulty)
            {
                case 1:
                    toFind = new string[8];
                    matrix = new string[7, 6];           //On donne les differents parametres qui dependront de la difficulté choisie : si la difficulté est de 1, on aura 8 mots a trouver, une matrice de 7x6 et des mots avec une taille inferieur ou egale a 6 lettres
                    maxLength = 6;
                    break;
                case 2:
                    toFind = new string[13];
                    matrix = new string[8, 8];                  //Meme principe pour les autres cas...
                    maxLength = 8;
                    break;
                case 3:
                    toFind = new string[18];
                    matrix = new string[10, 10];
                    maxLength = 10;
                    break;
                case 4:
                    toFind = new string[23];
                    matrix = new string[12, 11];
                    maxLength = 11;
                    break;
                case 5:
                    toFind = new string[28];
                    matrix = new string[13, 13];
                    maxLength = 13;
                    break;
                default:
                    throw new Exception("Difficulty must be in [1;5]");//Dans le cas ou le joueur ne rentre pas une difficulté entre 1 et 5, alors qu'il y a que 5 niveau de difficulté

            }

            //On defini et place les mots
            int nb = 0;

            //On defini les directions possibles pour la difficulté de la grille
            List<string> possibleDirs = new List<string>();
            for (int i = 0; i < diff[difficulty - 1]; i++)          
            {
                possibleDirs.Add(dirs[i]);                  //on ajoute a la liste possibledirs les directions possibles en fonction de la difficulté choisie
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {                                                           //on remplit le plateau de "vide"
                    matrix[i, j] = "";
                }
            }


            Random rand = new Random();
            //on va vérifier si un mot donné peut être placé dans la matrice dans une des directions possibles.
            while (nb < toFind.Length)              //La boucle while continue tant que les mots a trouver n'ont pas ete place
            {
                string word = dico.GetWord(maxLength);      //La variable word est initialisée avec un mot du dictionnaire ayant une taille max

                //On defini les endroits possibles pour le mot;
                List<Tuple<int, int, string>> candidates = new List<Tuple<int, int, string>>();

                foreach (var dir in possibleDirs)//La boucle foreach itère sur toutes les directions possibles de la liste possibleDirs et vérifie pour chaque direction si le mot peut être placé dans la matrice.
                {
                    switch (dir)
                    {
                        case "N-S":
                            candidates.AddRange(CheckNS(word));
                            break;                                                //on ajoute un candidat possible dans la liste de tuple
                        case "O-E":
                            candidates.AddRange(CheckOE(word));
                            break;
                        case "S-N":
                            candidates.AddRange(CheckSN(word));                     ///la méthode utilise un switch pour appeler la méthode appropriée pour vérifier si le mot peut être placé dans la matrice dans cette direction. Ces méthodes retournent une liste de tuples avec les positions possibles pour le mot dans la direction donnée, qui sont ajoutées à la liste candidates.
                            break;
                        case "E-O":
                            candidates.AddRange(CheckEO(word));
                            break;
                        case "NE-SO":
                            candidates.AddRange(CheckNESO(word));
                            break;
                        case "NO-SE":
                            candidates.AddRange(CheckNOSE(word));
                            break;
                        case "SE-NO":
                            candidates.AddRange(CheckSENO(word));
                            break;
                        case "SO-NE":
                            candidates.AddRange(CheckSONE(word));
                            break;
                    }
                }


                //On place le mot si place possible
                if (candidates.Count > 0)
                {


                    Tuple<int, int, string> pos = candidates[rand.Next(0, candidates.Count)];   //Si la liste candidates contient au moins une position valide pour le mot, le code choisit une position au hasard dans la liste à l'aide de la méthode Next de l'objet rand.


                    switch (pos.Item3)//on va placer le mot trouver aleatoirement parmi les candidats selon sa direction parmis
                    {
                        case "N-S":
                            for (int i = 0; i < word.Length; i++)
                            {
                                matrix[pos.Item1 + i, pos.Item2] = word[i].ToString();// on place le mot 
                            }
                            break;
                        case "O-E":
                            for (int i = 0; i < word.Length; i++)
                            {
                                matrix[pos.Item1, pos.Item2 + i] = word[i].ToString();
                            }
                            break;                                                                              //utilise ensuite un switch pour déterminer la direction de la position choisie et placer le mot dans la matrice en utilisant une boucle for pour parcourir les caractères du mot et les ajouter à la matrice à la position correspondante.
                        case "S-N":
                            for (int i = 0; i < word.Length; i++)
                            {
                                matrix[pos.Item1 - i, pos.Item2] = word[i].ToString();
                            }
                            break;
                        case "E-O":
                            for (int i = 0; i < word.Length; i++)
                            {
                                matrix[pos.Item1, pos.Item2 - i] = word[i].ToString();
                            }
                            break;
                        case "NE-SO":
                            for (int i = 0; i < word.Length; i++)
                            {
                                matrix[pos.Item1 + i, pos.Item2 - i] = word[i].ToString();
                            }
                            break;
                        case "NO-SE":
                            for (int i = 0; i < word.Length; i++)
                            {
                                matrix[pos.Item1 + i, pos.Item2 + i] = word[i].ToString();
                            }
                            break;
                        case "SE-NO":
                            for (int i = 0; i < word.Length; i++)
                            {
                                matrix[pos.Item1 - i, pos.Item2 - i] = word[i].ToString();
                            }
                            break;
                        case "SO-NE":
                            for (int i = 0; i < word.Length; i++)
                            {
                                matrix[pos.Item1 - i, pos.Item2 + i] = word[i].ToString();
                            }
                            break;
                    }


                    toFind[nb] = word;      //Enfin, le mot est ajouté à la liste des mots à trouver et le compteur de mots trouvés est incrémenté.
                    nb++;
                }


            }


            //On rempli les cases vides

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    //Si vide on met une lettre entre A (65) et Z (90)
                    if (matrix[i, j] == "")                                                     //On met des lettres aleatoires a l'aide de la table ASCI pour les endroits ou aucune lettre des mots places n'est prese
                        matrix[i, j] = ((char)rand.Next(65, 91)).ToString();
                }
            }

        }



        private List<Tuple<int, int, string>> CheckNS(string word)// on va determiner toutes les positions possibles pour un mot 
        {
            int x_max = matrix.GetLength(0) - word.Length + 1;//détermine valeur de x maximal pour placer le mot 
            List<Tuple<int, int, string>> res = new List<Tuple<int, int, string>>();
            for (int i = 0; i < x_max; i++)
            {
                                                                            //Ensuite, la méthode utilise deux boucles imbriquées pour itérer sur toutes les positions de départ possibles pour le mot dans la matrice.
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    //POUR CHAQUE COORD POSSIBLES
                    //EN FONCTION DE LA DIRECTION
                    int index = 0;
                    while (index < word.Length && (matrix[i + index, j] == "" || matrix[i + index, j] == word[index].ToString()))           //vérifie si le mot peut être placé en comparant les caractères du mot aux caractères correspondants dans la matrice. Si tous les caractères correspondent ou si les cellules correspondantes dans la matrice sont vides, 
                    {
                        index++;
                    }

                    //Le mot est bon
                    if (index == word.Length)
                    {
                        res.Add(new Tuple<int, int, string>(i, j, "N-S"));      //la méthode ajoute un nouveau tuple à la liste res avec la position de départ, la position de fin et la direction ("N-S" dans ce cas) du mot.
                    }

                }
            }

            return res;
        }



        private List<Tuple<int, int, string>> CheckOE(string word)
        {
            int y_max = matrix.GetLength(1) - word.Length + 1;
            List<Tuple<int, int, string>> res = new List<Tuple<int, int, string>>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < y_max; j++)
                {                                                                                                                       //Meme raisonnement que CheckNS
                    //POUR CHAQUE COORD POSSIBLES
                    //EN FONCTION DE LA DIRECTION
                    int index = 0;
                    while (index < word.Length && (matrix[i, j + index] == "" || matrix[i, j + index] == word[index].ToString()))
                    {
                        index++;
                    }

                    //Le mot est bon
                    if (index == word.Length)
                    {
                        res.Add(new Tuple<int, int, string>(i, j, "O-E"));
                    }

                }
            }

            return res;
        }

        private List<Tuple<int, int, string>> CheckEO(string word)
        {
            int y_max = word.Length - 1;
            List<Tuple<int, int, string>> res = new List<Tuple<int, int, string>>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = matrix.GetLength(1) - 1; j >= y_max; j--)
                {
                    //POUR CHAQUE COORD POSSIBLES
                    //EN FONCTION DE LA DIRECTION
                    int index = 0;
                    while (index < word.Length && (matrix[i, j - index] == "" || matrix[i, j - index] == word[index].ToString()))               //meme raisonnement que precedent
                    {
                        index++;
                    }

                    //Le mot est bon
                    if (index == word.Length)
                    {
                        res.Add(new Tuple<int, int, string>(i, j, "E-O"));
                    }

                }
            }

            return res;
        }


        private List<Tuple<int, int, string>> CheckSN(string word)//determiner toutes les positions possible 
        {
            int x_max = word.Length - 1;
            List<Tuple<int, int, string>> res = new List<Tuple<int, int, string>>();
            for (int i = matrix.GetLength(0) - 1; i >= x_max; i--)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    //POUR CHAQUE COORD POSSIBLES
                    //EN FONCTION DE LA DIRECTION
                    int index = 0;
                    while (index < word.Length && (matrix[i - index, j] == "" || matrix[i - index, j] == word[index].ToString()))
                    {
                        index++;
                    }

                    //Le mot est bon
                    if (index == word.Length)
                    {
                        res.Add(new Tuple<int, int, string>(i, j, "S-N"));
                    }

                }
            }

            return res;
        }

        private List<Tuple<int, int, string>> CheckNOSE(string word)
        {
            int x_max = matrix.GetLength(0) - word.Length + 1;
            int y_max = matrix.GetLength(1) - word.Length + 1;
            List<Tuple<int, int, string>> res = new List<Tuple<int, int, string>>();
            for (int i = 0; i < x_max; i++)
            {
                for (int j = 0; j < y_max; j++)
                {
                    //POUR CHAQUE COORD POSSIBLES
                    //EN FONCTION DE LA DIRECTION
                    int index = 0;                                                                                                                                  //meme raisonnement
                    while (index < word.Length && (matrix[i + index, j + index] == "" || matrix[i + index, j + index] == word[index].ToString()))
                    {
                        index++;
                    }

                    //Le mot est bon
                    if (index == word.Length)
                    {
                        res.Add(new Tuple<int, int, string>(i, j, "NO-SE"));
                    }

                }
            }

            return res;
        }

        private List<Tuple<int, int, string>> CheckNESO(string word)
        {
            int x_max = matrix.GetLength(0) - word.Length + 1;
            int y_max = word.Length - 1;
            List<Tuple<int, int, string>> res = new List<Tuple<int, int, string>>();
            for (int i = 0; i < x_max; i++)
            {
                for (int j = matrix.GetLength(1) - 1; j >= y_max; j--)
                {
                    //POUR CHAQUE COORD POSSIBLES
                    //EN FONCTION DE LA DIRECTION
                    int index = 0;                                                                                                                      //meme raisonnement
                    while (index < word.Length && (matrix[i + index, j - index] == "" || matrix[i + index, j - index] == word[index].ToString()))
                    {
                        index++;
                    }

                    //Le mot est bon
                    if (index == word.Length)
                    {
                        res.Add(new Tuple<int, int, string>(i, j, "NE-SO"));
                    }

                }
            }

            return res;
        }


        private List<Tuple<int, int, string>> CheckSONE(string word)
        {
            int x_max = word.Length - 1;
            int y_max = matrix.GetLength(1) - word.Length + 1;
            List<Tuple<int, int, string>> res = new List<Tuple<int, int, string>>();
            for (int i = matrix.GetLength(0) - 1; i >= x_max; i--)
            {
                for (int j = 0; j < y_max; j++)
                {
                    //POUR CHAQUE COORD POSSIBLES
                    //EN FONCTION DE LA DIRECTION                                                                                                   //Idem
                    int index = 0;
                    while (index < word.Length && (matrix[i - index, j + index] == "" || matrix[i - index, j + index] == word[index].ToString()))
                    {
                        index++;
                    }

                    //Le mot est bon
                    if (index == word.Length)
                    {
                        res.Add(new Tuple<int, int, string>(i, j, "SO-NE"));
                    }

                }
            }

            return res;
        }

        private List<Tuple<int, int, string>> CheckSENO(string word)
        {
            int x_max = word.Length - 1;
            int y_max = word.Length - 1;
            List<Tuple<int, int, string>> res = new List<Tuple<int, int, string>>();
            for (int i = matrix.GetLength(0) - 1; i >= x_max; i--)
            {
                for (int j = matrix.GetLength(1) - 1; j >= y_max; j--)
                {
                    //POUR CHAQUE COORD POSSIBLES
                    //EN FONCTION DE LA DIRECTION
                    int index = 0;                                                                                                                  //idem
                    while (index < word.Length && (matrix[i - index, j - index] == "" || matrix[i - index, j - index] == word[index].ToString()))
                    {
                        index++;
                    }

                    //Le mot est bon
                    if (index == word.Length)
                    {
                        res.Add(new Tuple<int, int, string>(i, j, "SE-NO"));
                    }

                }
            }

            return res;
        }








        public void ToRead(string file)             //Permet de prendre un fichier csv et transformer en une instance de plateau
        {
            string[] lines = System.IO.File.ReadAllLines(file);
            string[] properties = lines[0].Split(";");

            difficulty = Int32.Parse(properties[0]);
            matrix = new string[Int32.Parse(properties[1]), Int32.Parse(properties[2])];
            toFind = new string[Int32.Parse(properties[3])];

            string[] tf = lines[1].Split(";");
            for (int j = 0; j < toFind.Length; j++)
            {
                toFind[j] = tf[j];
            }

            int index = 2;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                string[] line = lines[index].Split(";");
                index++;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = line[j];
                }
            }

        }

        public override string ToString()
        {

            string res = "";

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                res += "──";
            }
            res += "─\n";

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                res += "│";
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    res += (matrix[i, j] != "" ? matrix[i, j] : " ") + "│";                                         //C'est la mise en forme du plateau pour avoir une vision claire avec des barres
                }
                res += "\n";
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    res += "──";
                }
                res += "─\n";

            }
            return res;
        }

        public void ToFile(string name)     //prendre un plateau et le mettre en csv
        {
            using (StreamWriter sw = new StreamWriter(name + ".csv"))
            {
                sw.WriteLine(difficulty + ";" + matrix.GetLength(0) + ";" + matrix.GetLength(1) + ";" + toFind.Length);
                sw.Write(toFind[0]);
                for (int i = 1; i < toFind.Length; i++)
                {
                    sw.Write(";" + toFind[i]);
                }
                sw.WriteLine();

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    sw.Write(matrix[i, 0]);
                    for (int j = 1; j < matrix.GetLength(1); j++)
                    {
                        sw.Write(";" + matrix[i, j]);
                    }
                    sw.WriteLine();
                }
            }
        }

        public bool DirInDiff(string dir)
        {
            for (int i = 0; i < diff[difficulty - 1]; i++)
            {
                if (dirs[i] == dir)
                    return true;            //renvoie true si la direction donnée correspond au niveau de difficulté choisi
            }

            return false;
        }


        public bool HandleNS(string word, int x, int y)//La fonction prend en entrée un mot (une chaîne de caractères) et deux entiers (x et y).
        {
            int i = 0;
            while (x + i < matrix.GetLength(0) && i < word.Length && word[i].ToString() == matrix[x + i, y])//La fonction entre dans une boucle qui continuera tant que x + i est inférieur à la longueur de la première dimension(nbre de ligne de la matrice "matrix" et que i est inférieur à la longueur du mot et que le i-ème caractère du mot est égal au caractère situé à la position (x + i, y) dans la matrice.
            {
                i++;
            }

            return i == word.Length;//Une fois que la boucle a terminé, la fonction vérifie si i est égal à la longueur du mot. Si c'est le cas, la fonction renvoie vrai. Sinon, la fonction renvoie faux.
        }

        //cette fonction permet de vérifier si tous les caractères d'un mot se trouvent dans la matrice à partir de la position (x, y) dans la direction de la première dimension de la matrice. Si c'est le cas, la fonction renvoie vrai. Sinon, elle renvoie faux.

        public bool HandleOE(string word, int x, int y)
        {
            int i = 0;
            while (y + i < matrix.GetLength(1) && i < word.Length && word[i].ToString() == matrix[x, y + i])//La fonction entre dans une boucle qui continuera tant que y + i est inférieur à la longueur de la seconde dimension(nbre de colonne de la matrice "matrix" et que i est inférieur à la longueur du mot et que le i-ème caractère du mot est égal au caractère situé à la position (x + i, y) dans la matrice.
            {
                i++;
            }

            return i == word.Length;//Une fois que la boucle a terminé, la fonction vérifie si i est égal à la longueur du mot. Si c'est le cas, la fonction renvoie vrai. Sinon, la fonction renvoie faux.
        }

        public bool HandleSN(string word, int x, int y)
        {
            int i = 0;
            while (x - i >= 0 && i < word.Length && word[i].ToString() == matrix[x - i, y])//La fonction entre dans une boucle qui continuera tant que x - i est superieur a 0 et que i est inférieur à la longueur du mot et que le i-ème caractère du mot est égal au caractère situé à la position (x - i, y) dans la matrice.
            {
                i++;
            }

            return i == word.Length;//Une fois que la boucle a terminé, la fonction vérifie si i est égal à la longueur du mot. Si c'est le cas, la fonction renvoie vrai. Sinon, la fonction renvoie faux.
        }

        public bool HandleEO(string word, int x, int y)
        {
            int i = 0;
            while (y - i >= 0 && i < word.Length && word[i].ToString() == matrix[x, y - i])//La fonction entre dans une boucle qui continuera tant que y - i est superieur a 0 et que i est inférieur à la longueur du mot et que le i-ème caractère du mot est égal au caractère situé à la position (x , y-i) dans la matrice.
            {
                i++;
            }

            return i == word.Length;//Une fois que la boucle a terminé, la fonction vérifie si i est égal à la longueur du mot. Si c'est le cas, la fonction renvoie vrai. Sinon, la fonction renvoie faux.
        }


        public bool HandleNESO(string word, int x, int y)
        {
            int i = 0;
            while (x + i < matrix.GetLength(0) && y - i >= 0 && i < word.Length && word[i].ToString() == matrix[x + i, y - i])// la fonction vérifie que x + i est inférieur à la longueur de la première dimension de la matrice matrix(nombre de ligne), que y-i est supérieur ou égal à 0, que i est inférieur à la longueur du mot word, et que le caractère en position i dans le mot word est égal au caractère situé à la position (x+i, y-i) dans la matrice matrix. Si toutes ces conditions sont vraies, la boucle continue. Sinon, elle s'arrête.
            {
                i++;
            }

            return i == word.Length;//Une fois que la boucle a terminé, la fonction vérifie si i est égal à la longueur du mot. Si c'est le cas, la fonction renvoie vrai. Sinon, la fonction renvoie faux.
        }

        public bool HandleNOSE(string word, int x, int y)
        {
            int i = 0;
            while (x + i < matrix.GetLength(0) && y + i < matrix.GetLength(1) && i < word.Length && word[i].ToString() == matrix[x + i, y + i])
            {
                i++;
            }

            return i == word.Length;//Une fois que la boucle a terminé, la fonction vérifie si i est égal à la longueur du mot. Si c'est le cas, la fonction renvoie vrai. Sinon, la fonction renvoie faux.
        }

        public bool HandleSONE(string word, int x, int y)
        {
            int i = 0;
            while (x - i >= 0 && y + i < matrix.GetLength(1) && i < word.Length && word[i].ToString() == matrix[x - i, y + i])
            {
                i++;
            }

            return i == word.Length;//Une fois que la boucle a terminé, la fonction vérifie si i est égal à la longueur du mot. Si c'est le cas, la fonction renvoie vrai. Sinon, la fonction renvoie faux.
        }

        public bool HandleSENO(string word, int x, int y)
        {
            int i = 0;
            while (x - i >= 0 && y - i >= 0 && i < word.Length && word[i].ToString() == matrix[x - i, y - i])
            {
                i++;
            }

            return i == word.Length;//Une fois que la boucle a terminé, la fonction vérifie si i est égal à la longueur du mot. Si c'est le cas, la fonction renvoie vrai. Sinon, la fonction renvoie faux.
        }




        public bool Test_Plateau(string mot, int ligne, int colonne, string direction)
        {
            if (!DirInDiff(direction) || !dico.RechDicoRecursif(mot))////Vérifie si la direction donnée n'est pas présente dans l'ensemble des directions possibles, ou si le mot donné n'est pas présent dans le dictionnaire. Si une de ces conditions est vraie, la fonction retourne false.
                return false;

            switch (direction)
            {
                case "N-S":
                    return HandleNS(mot, ligne, colonne);
                case "O-E":
                    return HandleOE(mot, ligne, colonne);
                case "S-N":
                    return HandleSN(mot, ligne, colonne);               //on utilise les fonctions crées precedemment pour determiner si un mot est positionné oui ou non dans une matrice selon une direction donnéé
                case "E-O":
                    return HandleEO(mot, ligne, colonne);
                case "NE-SO":
                    return HandleNESO(mot, ligne, colonne);
                case "NO-SE":
                    return HandleNOSE(mot, ligne, colonne);
                case "SO-NE":
                    return HandleSONE(mot, ligne, colonne);
                case "SE-NO":
                    return HandleSENO(mot, ligne, colonne);
            }

            return false;
        }
    }
}



