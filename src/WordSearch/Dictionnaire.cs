using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSearch
{
    public class Dictionnaire
    {
        private string lang;
        private List<string> words;                 //Création des attributs : langue / listes de mots dans le dictionnaire
        private List<Tuple<int, int>> carac;            //pas de getter pour carac car on en a besoin qu'a l'interieur de la classe

        //Getter

        public string Lang => lang;

        private List<string> Words => words;


        //CONSTRUCTEUR
        public Dictionnaire(string lang)
        {
            this.lang = lang;
            words = new List<string>();  //mots, c'est une liste vide, on va mettre les mots du dico dedans
            carac = new List<Tuple<int, int>>(); //carac, Liste de tuple vide, le premier "element" est la longueur des mots et le deuxieme est le nombre de mots de cette longueur

            string[] lines = Array.Empty<string>(); 
            if (lang == "FR")
                lines = System.IO.File.ReadAllLines(System.IO.Path.Combine(AppContext.BaseDirectory, "MotsPossiblesFR.txt"));                    //vérifie la valeur de lang pour déterminer quel fichier lire entre le dictionnaire francais qui est donné et le dictionnaire en anglais
            else if (lang == "EN")                                                              //Le contenu de ce fichier est chargé dans l'array lines.
                lines = System.IO.File.ReadAllLines(System.IO.Path.Combine(AppContext.BaseDirectory, "MotsPossiblesEN.txt"));

            int i = 1;

            int nb = Int32.Parse(lines[0]);  ///on transforme en entier la premiere ligne du fichier tableau lines, qui correspond aux fichier MotsDictionnaires
                                                
            int nbmem = nb;
            string[] line = Array.Empty<string>();                  


            while (i < lines.Length)                            //On parcourt les lignes du fichier qui a été choisi precedemment
            {
                bool sep = Int32.TryParse(lines[i], out nb);   //Pour chaque ligne, on regarde si c'est un separateur, ie un nombre, dans le fichier dictionnaire donné
                if (sep)
                {
                    carac.Add(new Tuple<int, int>(nbmem, line.Length)); //Si c'est un separateur, il ajoute un nouveau tuple à la liste carac en utilisant la taille des mots de la ligne et le nombre de mots de cette longueur. 
                    nbmem = nb;             
                    i++;
                }
                else
                {
                    line = lines[i].Split(" ");  
                    foreach (string w in line)    // si la ligne ne représente pas un séparateur, on ajoute chaque mot de cette ligne à la liste words et passe à la ligne suivante.
                        words.Add(w);
                    i++;
                }
       
            }
            carac.Add(new Tuple<int, int>(nbmem, line.Length)); //on ajoute le dernier tuple qui correspond aux mots de plus grande taille qui ne sont pas pris en compte sinon(voir fichier texte MotsDictionnaire)

            words.Sort((x, y) => string.Compare(x, y)); //On trie la liste word dans l'ordre alphabétique dans le but ensuite de l'utiliser pour faire de la dichitimie


        }

        public override string ToString()
        {
            string res = "";
            res += "Langue : " + lang + "\n";
            res += "Nombre total de mots : " + words.Count + "\n";
            foreach (Tuple<int, int> couple in carac)
            {
                res += "Taille " + couple.Item1 + " Nombre : " + couple.Item2 + "\n";  //Avec couple.Item1, on va prendre le premier element du couple (la taille des mots en nombre de lettres), et avec couple.Item2, on va prendre le deuxieme element du couple (le nombre de mots de meme longueur)
            }
            return res;
        }
        public int dicho(int left, int right, string word)
        {
            if (left == right)
                return left;
            int m = (left + right) / 2;
            if (word == words[m])                            //Si word est egale au mot a l'indice m, on retourne m car on tombe directement dessus
                return m;
            else if (String.Compare(words[m], word) <0)      //on regarde dans la partie de droite du tableau car le mots sur lequel nous sommes tombés  a l'indice m dans la liste word est avant (par rapport a l'ordre alphabétique)
            {
                return dicho(m+1,right, word); 
            }
            else 
                return dicho(left,m,word);                  //le mot trouvé a l'indice m dans word tombe après le mots que l'on recherche(par rapport a l'odre alphabétique), on regarde dans avant dans la liste words donc dans la partie gauche

        }

        public bool RechDicoRecursif(string mot)    //on veut trouver si le mot existe dans le dictionnaire
        {
            int index = dicho(0, words.Count, mot); // La fonction appelle la fonction dicho qui utilise un algorithme de recherche binaire pour trouver l'index où le mot peut être dans le dictionnaire.
            if (index >= words.Count)               
                return false;                   //si l'index est superieur alors c'est immediatement faux, out of band, le mots ne peut pas etre dans le dictionnaire
            return words[index] == mot;    //sinon on teste si dans la liste words a l'indice index, on tombe sur mot; si ce n'est pas le cas, la fonction renverra false, true sinon
        }

        public string GetWord(int length) // on veut obtenir un mot avec une certaine longueur maximum
        {
            Random rand = new Random();     //on initialise un random
            string res = "";                   //on crée une chaine de caractères vide
            while (res == "")                   //On va faire une boucle tant que cette chaine de caractere est vide
            {
                int i = rand.Next(0, words.Count); // on prend un indice aleatoire dans tous nos mots du dictionnaire donné par le professeur "MotsDictionnairesFR
                if (words[i].Length <= length)   //si la longueur du mots correspond a la longueur recherché donc de taille inferieur a length, notre resultat est le mot
                                                 /// sinon on fait rien, tant qu'on trouve pas un mot on reste dans la boucle
                    res= words[i];
                
            }
            return res;
        }

    }
}
