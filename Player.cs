using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace sarahmarinoff_assignment7
{
    class Player : GameObject
    {
        public Player(Vector3 _position, Model _model)
            : base(_position, _model)
        {

        }

        public bool checkStaticCollisions(Vector3 temp) //temp = future position
        {
            //Wall 1: -325 to -275 (z), -575 to -525 (x)
            if ((-325 <= temp.Z) && (-275 > temp.Z) && (-575 <= temp.X) && (-525 > temp.X))
                return true;

            //Wall 2: -325 to -275 (z), -375 to -325 (x)
            if ((-325 <= temp.Z) && (-275 > temp.Z) && (-375 <= temp.X) && (-325 > temp.X))
                return true;

            //Wall 3: -575 to -275 (z), -325 to -275 (x)
            if ((-575 <= temp.Z) && (-275 > temp.Z) && (-325 <= temp.X) && (-275 > temp.X))
                return true;

            //Wall 4: -575 to -275 (z), -25 to 25 (x)
            if ((-575 <= temp.Z) && (-275 > temp.Z) && (-25 <= temp.X) && (25 > temp.X))
                return true;

            //Wall 5: -325 to -275 (z), 25 to 275 (x)
            if ((-325 <= temp.Z) && (-275 > temp.Z) && (25 <= temp.X) && (275 > temp.X))
                return true;

            //Wall 6: -25 to 25 (z), -325 to 575 (x)
            if ((-25 <= temp.Z) && (25 > temp.Z) && (-325 <= temp.X) && (575 > temp.X))
                return true;

            //Wall 7: 25 to 275 (z), -25 to 25 (x)
            if ((25 <= temp.Z) && (275 > temp.Z) && (-25 <= temp.X) && (25 > temp.X))
                return true;

            //Wall 8: 275 to 325 (z), -575 to -325 (x)
            if ((275 <= temp.Z) && (325 > temp.Z) && (-575 <= temp.X) && (-325 > temp.X))
                return true;

            //Wall 9: 275 to 325 (z), 275 to 375 (x)
            if ((275 <= temp.Z) && (325 > temp.Z) && (275 <= temp.X) && (375 > temp.X))
                return true;

            //Wall 10: 275 to 325 (z), 525 to 575 (x)
            if ((275 <= temp.Z) && (325 > temp.Z) && (525 <= temp.X) && (575 > temp.X))
                return true;

            //Wall 11: 325 to 575 (z), 275 to 325 (x)
            if ((325 <= temp.Z) && (575 > temp.Z) && (275 <= temp.X) && (325 > temp.X))
                return true;

            //Spike 1: -475 to -425 (z), 125 to 175 (x)
            if ((-475 <= temp.Z) && (-425 > temp.Z) && (125 <= temp.X) && (175 > temp.X))
                return true;

            //Spike 2: -175 to -125 (z), -175 to -125 (x)
            if ((-175 <= temp.Z) && (-125 > temp.Z) && (-175 <= temp.X) && (-125 > temp.X))
                return true;

            //Spike 3: -175 to -125 (z), 400 to 450 (x)
            if ((-175 <= temp.Z) && (-125 > temp.Z) && (400 <= temp.X) && (450 > temp.X))
                return true;

            //Spike 4: 125 to 175 (z), -475 to -425 (x)
            if ((125 <= temp.Z) && (175 > temp.Z) && (-475 <= temp.X) && (-425 > temp.X))
                return true;

            //Spike 5: 125 to 175 (z), 125 to 175 (x)
            if ((125 <= temp.Z) && (175 > temp.Z) && (125 <= temp.X) && (175 > temp.X))
                return true;

            //Spike 6: 425 to 475 (z), -175 to -125 (x)
            if ((425 <= temp.Z) && (475 > temp.Z) && (-175 <= temp.X) && (-125 > temp.X))
                return true;

            //Interior limits: -575 to 575 (z), -575 to 575 (x)
            if ((temp.Z <= -575) || (temp.Z >= 575) || (temp.X <= -575) || (temp.X >= 575))
                return true;

            return false;
        }
    }
}
