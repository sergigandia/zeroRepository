using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomePrediction
{
    using System.Security.Permissions;

    using LeagueSharp;
    using LeagueSharp.Common;

    public static class main
    {
        private static Menu menu;
       public static void Initialize()
        {
           Game.PrintChat("Awesome pred loaded");
           InitializeMenu();
        }

        public static void InitializeMenu()
        {
            menu = new Menu("Awesome prediction", "Awesome prediction", true);
            menu.AddItem(new MenuItem("draws", "Drawings on").SetValue(true));
            menu.AddToMainMenu();
        }
    }
}
