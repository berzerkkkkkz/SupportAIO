using EnsoulSharp.SDK;
using System;

namespace SupportAIO
{
    class Bootstrap
    {
        public static void Init()
        {
            try
            {                      
                GameEvent.OnGameLoad += delegate
                {
                    switch (GameObjects.Player.CharacterName)
                    {
                        case "Alistar":
                            new Champions.Alistar();
                            break;
                        case "Blitzcrank":
                            new Champions.Blitzcrank();
                            break;
                        case "Brand":
                            new Champions.Brand();
                            break;
                        case "Janna":
                            new Champions.Janna();
                            break;
                        case "Karma":
                            new Champions.Karma();
                            break;
                        case "Leona":
                            new Champions.Leona();
                            break;
                        case "Lulu":
                            new Champions.Lulu();
                            break;
                        case "Nami":
                            new Champions.Nami();
                            break;
                        case "Rakan":
                            new Champions.Rakan();
                            break;
                        case "Soraka":
                            new Champions.Soraka();
                            break;
                        case "Zilean":
                            new Champions.Zilean();
                            break;
                        case "Zyra":
                            new Champions.Zyra();
                            break;
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in loading. Exception:"+ ex);
            }
        }
    }
}
