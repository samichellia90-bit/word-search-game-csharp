using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSearch
{
    class Joueur
    {

        private string name;
        private List<string> found;                 //Création des attributs
        private int score;

        //getters

        public string Name => name;

        public List<string> Found => found;

        public int Score => score;

        public Joueur(string name)
        {
            this.name = name;                   //On cree le constructeur avec de base le score nul car le joueur n'a trouvé aucun mot
                                                   //la liste de mots trouvés "found" est a l'origine vide car on a pas encore trouvé de mots
            score = 0;
            found = new List<string>();
        }

        public void Add_mot(string mot)                 //Ne renvoie rien donc void

        {
            found.Add(mot);                             //Ajoute le mot a la liste de mots trouvés qui est de base vide
        }

        public void Add_Score(int val)
        {
            score += val;                               ///Ne renvoie rien et ajoute une valeur au score du joueur lorsqu'il trouve les mots dans le plateau de mots croisés

        }

        public override string ToString()
        {
            return "Name :" + name + "\n Score :" + score;      //Renvoie le nom du joueur et son score
        }
    }
}
