using System.Linq;
using Verse;

namespace PawnSaveUtility
{
    public class PawnName
    {
        public string firstName;

        public string nickName;

        public string lastName;

        public PawnName()
        {

        }

        public PawnName(Name name)
        {
            ModLog.Log("Pawn full name: "+name.ToStringFull);

            if(name is NameTriple nameTriple)
            {
                firstName = nameTriple.First;

                if(nameTriple.NickSet)
                {
                    nickName = nameTriple.Nick;
                }

                lastName = nameTriple.Last;
            }
        }
    }
}