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
    public enum enemyState
    {
        Patrol,
        Wander,
        Chase,
        Idle
    };

    class Enemy : GameObject
    {
        static public Vector3[] nodes;
        static public int[,] pathTable;

        public int startNode;
        public int currentNode;
        public int targetNode;
        public int finalNode;

        public enemyState state;

        public Enemy(Vector3 _position, Model _model)
            : base(_position, _model)
        {

        }

        public void setState(enemyState _state)
        {
            state = _state;
        }

        public void initNodeSet(int _start, int _current, int _final)
        {
            startNode = _start;
            currentNode = _current;
            finalNode = _final;
        }

        public void update(Vector3 player_position)
        {
            if (state == enemyState.Patrol)
            {
                targetNode = pathTable[currentNode,finalNode];

                Vector3 movement = (nodes[targetNode].X - position.X) * xSpeed + (nodes[targetNode].Z - position.Z) * zSpeed;
                updatePosition(movement);

                if ((nodes[targetNode].X - position.X < 2) && (nodes[targetNode].Z - position.Z < 2))
                {
                    currentNode = targetNode;

                    if (currentNode == finalNode)
                    {
                        finalNode = startNode;
                        startNode = currentNode;

                        //Console.WriteLine("Start: " + (startNode + 1) + ", Current: " + (currentNode + 1) + ", Target: " + (targetNode + 1) + ", End: " + (finalNode + 1));
                    }
                }
            }
            else if (state == enemyState.Wander)
            {
                targetNode = pathTable[currentNode, finalNode];

                if (targetNode == -1)
                    targetNode = currentNode;

                Vector3 movement = (nodes[targetNode].X - position.X) * xSpeed + (nodes[targetNode].Z - position.Z) * zSpeed;
                updatePosition(movement);

                if ((nodes[targetNode].X - position.X < 2) && (nodes[targetNode].Z - position.Z < 2))
                {
                    currentNode = targetNode;

                    //Console.WriteLine("Start: " + (startNode + 1) + ", Current: " + (currentNode + 1) + ", Target: " + (targetNode + 1) + ", End: " + (finalNode + 1));

                    if (currentNode == finalNode)
                    {
                        Random rand = new Random();

                        finalNode = rand.Next(0,46);
                        startNode = currentNode;
                    }
                }
            }
            else if (state == enemyState.Idle)
            {
                if ((Math.Abs(position.X - player_position.X) < 300) && (Math.Abs(position.Z - player_position.Z) < 300) && (player_position.Z > 0))
                {
                    state = enemyState.Chase;
                }
            }
            else if (state == enemyState.Chase)
            {
                if (player_position.Z < 0 || (player_position.Z > 250 && player_position.X > 325) )
                {
                    state = enemyState.Idle;
                    xSpeed = new Vector3(0.03f, 0, 0);
                    zSpeed = new Vector3(0, 0, 0.03f);
                }
                else
                {
                    Vector3 movement = (player_position.X - position.X) * xSpeed + (player_position.Z - position.Z) * zSpeed;
                    updatePosition(movement);

                    if ((Math.Abs(position.X - player_position.X) < 75) && (Math.Abs(position.Z - player_position.Z) < 75))
                    {
                        xSpeed = new Vector3(0.08f, 0, 0);
                        zSpeed = new Vector3(0, 0, 0.08f);
                    }
                }
            }
        }

        public void initNodes()
        {
            nodes = new Vector3[46];

            nodes[0].X = -225; nodes[0].Z = -525; nodes[0].Y = 0;       //1
            nodes[1].X = -75; nodes[1].Z = -525; nodes[1].Y = 0;        //2
            nodes[2].X = 75; nodes[2].Z = -525; nodes[2].Y = 0;         //3
            nodes[3].X = 225; nodes[3].Z = -525; nodes[3].Y = 0;        //4
            nodes[4].X = 525; nodes[4].Z = -525; nodes[4].Y = 0;        //5

            nodes[5].X = 375; nodes[5].Z = -400; nodes[5].Y = 0;        //6

            nodes[6].X = -150; nodes[6].Z = -375; nodes[6].Y = 0;        //7
            nodes[7].X = 75; nodes[7].Z = -375; nodes[7].Y = 0;        //8
            nodes[8].X = 225; nodes[8].Z = -375; nodes[8].Y = 0;        //9
            nodes[9].X = 525; nodes[9].Z = -375; nodes[9].Y = 0;        //10

            nodes[10].X = -525; nodes[10].Z = -225; nodes[10].Y = 0;        //11
            nodes[11].X = -225; nodes[11].Z = -225; nodes[11].Y = 0;        //12
            nodes[12].X = -75; nodes[12].Z = -225; nodes[12].Y = 0;        //13
            nodes[13].X = 325; nodes[13].Z = -225; nodes[13].Y = 0;        //14
            nodes[14].X = 525; nodes[14].Z = -225; nodes[14].Y = 0;        //15

            nodes[15].X = -375; nodes[15].Z = -150; nodes[15].Y = 0;        //16
            nodes[16].X = 125; nodes[16].Z = -150; nodes[16].Y = 0;        //17

            nodes[17].X = -525; nodes[17].Z = -75; nodes[17].Y = 0;        //18
            nodes[18].X = -225; nodes[18].Z = -75; nodes[18].Y = 0;        //19
            nodes[19].X = -75; nodes[19].Z = -75; nodes[19].Y = 0;        //20
            nodes[20].X = 325; nodes[20].Z = -75; nodes[20].Y = 0;        //21
            nodes[21].X = 525; nodes[21].Z = -75; nodes[21].Y = 0;        //22

            nodes[22].X = -525; nodes[22].Z = 75; nodes[22].Y = 0;        //23
            nodes[23].X = -375; nodes[23].Z = 75; nodes[23].Y = 0;        //24
            nodes[24].X = -75; nodes[24].Z = 75; nodes[24].Y = 0;        //25
            nodes[25].X = 75; nodes[25].Z = 75; nodes[25].Y = 0;        //26
            nodes[26].X = 225; nodes[26].Z = 75; nodes[26].Y = 0;        //27
            nodes[27].X = 525; nodes[27].Z = 75; nodes[27].Y = 0;        //28

            nodes[28].X = -225; nodes[28].Z = 150; nodes[28].Y = 0;        //29
            nodes[29].X = 375; nodes[29].Z = 150; nodes[29].Y = 0;        //30

            nodes[30].X = -525; nodes[30].Z = 225; nodes[30].Y = 0;        //31
            nodes[31].X = -375; nodes[31].Z = 225; nodes[31].Y = 0;        //32
            nodes[32].X = -75; nodes[32].Z = 225; nodes[32].Y = 0;        //33
            nodes[33].X = 75; nodes[33].Z = 225; nodes[33].Y = 0;        //34
            nodes[34].X = 225; nodes[34].Z = 225; nodes[34].Y = 0;        //35
            nodes[35].X = 525; nodes[35].Z = 225; nodes[35].Y = 0;        //36

            nodes[36].X = -525; nodes[36].Z = 375; nodes[36].Y = 0;        //37
            nodes[37].X = -225; nodes[37].Z = 375; nodes[37].Y = 0;        //38
            nodes[38].X = -75; nodes[38].Z = 375; nodes[38].Y = 0;        //39
            nodes[39].X = 225; nodes[39].Z = 375; nodes[39].Y = 0;        //40

            nodes[40].X = -375; nodes[40].Z = 450; nodes[40].Y = 0;        //41
            nodes[41].X = 75; nodes[41].Z = 450; nodes[41].Y = 0;        //42

            nodes[42].X = -525; nodes[42].Z = 525; nodes[42].Y = 0;        //43
            nodes[43].X = -225; nodes[43].Z = 525; nodes[43].Y = 0;        //44
            nodes[44].X = -75; nodes[44].Z = 525; nodes[44].Y = 0;        //45
            nodes[45].X = 225; nodes[45].Z = 525; nodes[45].Y = 0;        //46

            pathTable = new int[46, 46];

            //fill out the table...

            //node 1

            pathTable[0, 0] = -1;
            pathTable[0, 1] = 1;

            for (int i = 2; i < 10; i++)
                pathTable[0, i] = 6;

            pathTable[0, 10] = 11;
            pathTable[0, 11] = 11;

            for (int i = 12; i < 15; i++)
                pathTable[0, i] = 6;

            pathTable[0, 15] = 11;
            pathTable[0, 16] = 6;

            pathTable[0, 17] = 11;
            pathTable[0, 18] = 11;

            for (int i = 19; i < 22; i++)
                pathTable[0, i] = 6;

            for (int i = 22; i < 46; i++)
                pathTable[0, i] = 11;

            //node 2

            pathTable[1, 0] = 0;
            pathTable[1, 1] = -1;

            for (int i = 2; i < 6; i++)
                pathTable[1, i] = 12;

            pathTable[1, 6] = 6;

            for (int i = 7; i < 10; i++)
                pathTable[1, i] = 12;

            pathTable[1, 10] = 6;
            pathTable[1, 11] = 6;

            for (int i = 12; i < 15; i++)
                pathTable[1, i] = 12;

            pathTable[1, 15] = 6;
            pathTable[1, 16] = 12;
            pathTable[1, 17] = 6;
            pathTable[1, 18] = 6;

            for (int i = 19; i < 22; i++)
                pathTable[1, i] = 12;

            for (int i = 22; i < 46; i++)
                pathTable[1, i] = 6;

            //node 3

            pathTable[2, 0] = 3;
            pathTable[2, 1] = 3;
            pathTable[2, 2] = -1;

            for (int i = 3; i < 7; i++)
                pathTable[2, i] = 3;

            pathTable[2, 7] = 7;
            pathTable[2, 8] = 7;

            for (int i = 9; i < 46; i++)
                pathTable[2, i] = 3;

            //node 4

            pathTable[3, 0] = 5;
            pathTable[3, 1] = 5;
            pathTable[3, 2] = 2;
            pathTable[3, 3] = -1;
            pathTable[3, 4] = 4;
            pathTable[3, 5] = 5;
            pathTable[3, 6] = 5;
            pathTable[3, 7] = 8;
            pathTable[3, 8] = 8;

            for (int i = 9; i < 46; i++)
                pathTable[3, i] = 5;

            //node 5

            pathTable[4, 0] = 9;
            pathTable[4, 1] = 9;
            pathTable[4, 2] = 3;
            pathTable[4, 3] = 3;
            pathTable[4, 4] = -1;
            pathTable[4, 5] = 5;
            pathTable[4, 6] = 9;
            pathTable[4, 7] = 5;
            pathTable[4, 8] = 5;

            for (int i = 9; i < 46; i++)
                pathTable[4, i] = 9;

            //node 6

            pathTable[5, 0] = 13;
            pathTable[5, 1] = 13;
            pathTable[5, 2] = 3;
            pathTable[5, 3] = 3;
            pathTable[5, 4] = 4;
            pathTable[5, 5] = -1;
            pathTable[5, 6] = 13;
            pathTable[5, 7] = 8;
            pathTable[5, 8] = 8;
            pathTable[5, 9] = 9;

            for (int i = 10; i < 14; i++)
                pathTable[5, i] = 13;

            pathTable[5, 14] = 9;

            for (int i = 15; i < 46; i++)
                pathTable[5, i] = 13;

            //node 7

            pathTable[6, 0] = 0;
            pathTable[6, 1] = 1;

            for (int i = 2; i < 6; i++)
                pathTable[6, i] = 12;

            pathTable[6, 6] = -1;

            for (int i = 7; i < 10; i++)
                pathTable[6, i] = 12;

            pathTable[6, 10] = 11;
            pathTable[6, 11] = 11;

            for (int i = 12; i < 15; i++)
                pathTable[6, i] = 12;

            pathTable[6, 15] = 11;
            pathTable[6, 16] = 12;
            pathTable[6, 17] = 11;
            pathTable[6, 18] = 11;

            for (int i = 19; i < 22; i++)
                pathTable[6, i] = 12;

            for (int i = 22; i < 46; i++)
                pathTable[6, i] = 11;

            //node 8

            pathTable[7, 0] = 8;
            pathTable[7, 1] = 8;
            pathTable[7, 2] = 2;
            pathTable[7, 3] = 2;

            for (int i = 4; i < 7; i++)
                pathTable[7, i] = 8;

            pathTable[7, 7] = -1;

            for (int i = 8; i < 46; i++)
                pathTable[7, i] = 8;

            //node 9

            pathTable[8, 0] = 5;
            pathTable[8, 1] = 5;
            pathTable[8, 2] = 3;
            pathTable[8, 3] = 3;

            for (int i = 4; i < 7; i++)
                pathTable[8, i] = 5;

            pathTable[8, 7] = 7;
            pathTable[8, 8] = -1;
            pathTable[8, 9] = 9;

            for (int i = 10; i < 14; i++)
                pathTable[8, i] = 5;

            pathTable[8, 14] = 9;

            for (int i = 15; i < 21; i++)
                pathTable[8, i] = 5;

            pathTable[8, 21] = 9;

            for (int i = 22; i < 46; i++)
                pathTable[8, i] = 5;

            //node 10

            pathTable[9, 0] = 13;
            pathTable[9, 1] = 13;
            pathTable[9, 2] = 5;
            pathTable[9, 3] = 5;
            pathTable[9, 4] = 4;
            pathTable[9, 5] = 5;
            pathTable[9, 6] = 13;
            pathTable[9, 7] = 8;
            pathTable[9, 8] = 8;
            pathTable[9, 9] = -1;

            for (int i = 10; i < 14; i++)
                pathTable[9, i] = 13;

            pathTable[9, 14] = 14;

            for (int i = 15; i < 21; i++)
                pathTable[9, i] = 13;

            pathTable[9, 21] = 14;

            for (int i = 22; i < 46; i++)
                pathTable[9, i] = 13;

            //node 11

            for (int i = 0; i < 10; i++)
                pathTable[10, i] = 11;

            pathTable[10, 10] = -1;

            for (int i = 11; i < 15; i++)
                pathTable[10, i] = 11;

            pathTable[10, 15] = 15;
            pathTable[10, 16] = 11;
            pathTable[10, 17] = 17;

            for (int i = 18; i < 22; i++)
                pathTable[10, i] = 15;

            pathTable[10, 22] = 17;
            pathTable[10, 23] = 15;
            pathTable[10, 24] = 15;

            for (int i = 25; i < 45; i+=2)
            {
                pathTable[10, i] = 15;
                pathTable[10, i+1] = 17;
            }

            pathTable[10, 45] = 15;

            //node 12

            pathTable[11, 0] = 0;
            pathTable[11, 1] = 6;

            for (int i = 2; i < 6; i++)
                pathTable[11, i] = 12;

            pathTable[11, 6] = 6;

            for (int i = 7; i < 10; i++)
                pathTable[11, i] = 12;

            pathTable[11, 10] = 10;
            pathTable[11, 11] = -1;

            for (int i = 12; i < 15; i++)
                pathTable[11, i] = 12;

            pathTable[11, 15] = 15;
            pathTable[11, 16] = 12;
            pathTable[11, 17] = 15;
            pathTable[11, 18] = 18;
            pathTable[11, 19] = 18;
            pathTable[11, 20] = 12;
            pathTable[11, 21] = 12;

            for (int i = 22; i < 46; i++)
                pathTable[11, i] = 15;

            //node 13

            pathTable[12, 0] = 6;
            pathTable[12, 1] = 1;

            for (int i = 2; i < 6; i++)
                pathTable[12, i] = 13;

            pathTable[12, 6] = 6;

            for (int i = 7; i < 10; i++)
                pathTable[12, i] = 13;

            pathTable[12, 10] = 11;
            pathTable[12, 11] = 11;
            pathTable[12, 12] = -1;
            pathTable[12, 13] = 13;
            pathTable[12, 14] = 13;
            pathTable[12, 15] = 11;
            pathTable[12, 16] = 16;
            pathTable[12, 17] = 11;
            pathTable[12, 18] = 19;
            pathTable[12, 19] = 19;
            pathTable[12, 20] = 16;
            pathTable[12, 21] = 16;

            for (int i = 22; i < 46; i++)
                pathTable[12, i] = 11;

            //node 14

            pathTable[13, 0] = 12;
            pathTable[13, 1] = 12;
            pathTable[13, 2] = 5;
            pathTable[13, 3] = 5;
            pathTable[13, 4] = 9;
            pathTable[13, 5] = 5;
            pathTable[13, 6] = 12;
            pathTable[13, 7] = 5;
            pathTable[13, 8] = 5;
            pathTable[13, 9] = 9;

            for (int i = 10; i < 13; i++)
                pathTable[13, i] = 12;

            pathTable[13, 13] = -1;
            pathTable[13, 14] = 14;
            pathTable[13, 15] = 12;

            for (int i = 16; i < 20; i++)
                pathTable[13, i] = 16;

            pathTable[13, 20] = 20;
            pathTable[13, 21] = 20;

            for (int i = 22; i < 46; i++)
                pathTable[13, i] = 16;

            //node 15

            pathTable[14, 0] = 13;
            pathTable[14, 1] = 13;

            for (int i = 2; i < 6; i++)
                pathTable[14, i] = 9;

            pathTable[14, 6] = 13;

            for (int i = 7; i < 10; i++)
                pathTable[14, i] = 9;

            for (int i = 10; i < 14; i++)
                pathTable[14, i] = 13;

            pathTable[14, 14] = -1;
            pathTable[14, 15] = 13;
            pathTable[14, 16] = 13;

            for (int i = 17; i < 46; i++)
                pathTable[14, i] = 21;

            //node 16

            for (int i = 0; i < 10; i++)
                pathTable[15, i] = 11;

            pathTable[15, 10] = 10;

            for (int i = 11; i < 15; i++)
                pathTable[15, i] = 11;

            pathTable[15, 15] = -1;
            pathTable[15, 16] = 11;
            pathTable[15, 17] = 17;

            for (int i = 18; i < 22; i++)
                pathTable[15, i] = 18;

            pathTable[15, 22] = 17;

            for (int i = 23; i < 46; i++)
                pathTable[15, i] = 23;

            pathTable[15, 30] = 17;

            //node 17

            pathTable[16, 0] = 12;
            pathTable[16, 1] = 12;

            for (int i = 2; i < 6; i++)
                pathTable[16, i] = 13;

            pathTable[16, 6] = 12;

            for (int i = 7; i < 10; i++)
                pathTable[16, i] = 13;

            for (int i = 10; i < 13; i++)
                pathTable[16, i] = 12;

            pathTable[16, 13] = 13;
            pathTable[16, 14] = 14;
            pathTable[16, 15] = 12;
            pathTable[16, 16] = -1;

            for (int i = 17; i < 20; i++)
                pathTable[16, i] = 19;

            pathTable[16, 20] = 20;
            pathTable[16, 21] = 20;

            for (int i = 22; i < 46; i++)
                pathTable[16, i] = 19;

            //node 18

            for (int i = 0; i < 10; i++)
                pathTable[17, i] = 15;

            pathTable[17, 10] = 10;

            for (int i = 11; i < 16; i++)
                pathTable[17, i] = 15;

            pathTable[17, 16] = 18;
            pathTable[17, 17] = -1;

            for (int i = 18; i < 22; i++)
                pathTable[17, i] = 18;

            pathTable[17, 22] = 22;

            for (int i = 23; i < 30; i++)
                pathTable[17, i] = 23;

            pathTable[17, 30] = 22;

            for (int i = 31; i < 46; i++)
                pathTable[17, i] = 23;

            //node 19

            pathTable[18, 0] = 11;
            pathTable[18, 1] = 11;

            for (int i = 2; i < 6; i++)
                pathTable[18, i] = 19;

            pathTable[18, 6] = 11;

            for (int i = 7; i < 10; i++)
                pathTable[18, i] = 19;

            pathTable[18, 10] = 15;
            pathTable[18, 11] = 11;

            for (int i = 12; i < 15; i++)
                pathTable[18, i] = 19;

            pathTable[18, 15] = 15;
            pathTable[18, 16] = 19;
            pathTable[18, 17] = 17;
            pathTable[18, 18] = -1;

            for (int i = 19; i < 22; i++)
                pathTable[18, i] = 19;

            for (int i = 22; i < 46; i++)
                pathTable[18, i] = 17;

            //node 20

            pathTable[19, 0] = 12;
            pathTable[19, 1] = 12;

            for (int i = 2; i < 6; i++)
                pathTable[19, i] = 16;

            pathTable[19, 6] = 12;

            for (int i = 7; i < 10; i++)
                pathTable[19, i] = 16;

            pathTable[19, 10] = 18;
            pathTable[19, 11] = 12;
            pathTable[19, 12] = 12;
            pathTable[19, 13] = 16;
            pathTable[19, 14] = 16;
            pathTable[19, 15] = 18;
            pathTable[19, 16] = 16;
            pathTable[19, 17] = 18;
            pathTable[19, 18] = 18;
            pathTable[19, 19] = -1;
            pathTable[19, 20] = 20;
            pathTable[19, 21] = 20;

            for (int i = 22; i < 46; i++)
                pathTable[19, i] = 18;

            //node 21

            pathTable[20, 0] = 16;
            pathTable[20, 1] = 16;

            for (int i = 2; i < 6; i++)
                pathTable[20, i] = 13;

            pathTable[20, 6] = 16;

            for (int i = 7; i < 10; i++)
                pathTable[20, i] = 13;

            for (int i = 10; i < 13; i++)
                pathTable[20, i] = 16;

            pathTable[20, 13] = 13;
            pathTable[20, 14] = 21;
            pathTable[20, 15] = 19;
            pathTable[20, 16] = 16;

            for (int i = 17; i < 20; i++)
                pathTable[20, i] = 19;

            pathTable[20, 20] = -1;
            pathTable[20, 21] = 21;

            for (int i = 22; i < 46; i++)
                pathTable[20, i] = 19;

            //node 22

            pathTable[21, 0] = 20;
            pathTable[21, 1] = 20;

            for (int i = 2; i < 6; i++)
                pathTable[21, i] = 14;

            pathTable[21, 6] = 20;

            for (int i = 7; i < 10; i++)
                pathTable[21, i] = 14;

            for (int i = 10; i < 14; i++)
                pathTable[21, i] = 20;

            pathTable[21, 14] = 13;

            for (int i = 15; i < 46; i++)
                pathTable[21, i] = 20;

            pathTable[21, 21] = -1;

            //node 23

            for (int i = 0; i < 22; i++)
                pathTable[22, i] = 17;

            pathTable[22, 22] = -1;

            for (int i = 23; i < 30; i++)
                pathTable[22, i] = 23;

            pathTable[22, 30] = 30;
            pathTable[22, 31] = 30;

            for (int i = 32; i < 46; i++)
                pathTable[22, i] = 23;

            //node 24

            for (int i = 0; i < 17; i++)
                pathTable[23, i] = 15;

            pathTable[23, 17] = 17;

            for (int i = 18; i < 22; i++)
                pathTable[23, i] = 15;

            pathTable[23, 22] = 22;
            pathTable[23, 23] = -1;
            pathTable[23, 24] = 24;

            for (int i = 25; i < 30; i++)
                pathTable[23, i] = 28;

            pathTable[23, 30] = 22;
            pathTable[23, 31] = 31;

            for (int i = 32; i < 46; i++)
                pathTable[23, i] = 28;

            //node 25

            for (int i = 0; i < 24; i++)
                pathTable[24, i] = 23;

            pathTable[24, 24] = -1;

            for (int i = 25; i < 28; i++)
                pathTable[24, i] = 32;

            pathTable[24, 28] = 28;
            pathTable[24, 29] = 32;
            pathTable[24, 30] = 28;
            pathTable[24, 31] = 28;

            for (int i = 32; i < 36; i++)
                pathTable[24, i] = 32;

            pathTable[24, 36] = 28;
            pathTable[24, 37] = 28;
            pathTable[24, 38] = 32;
            pathTable[24, 39] = 32;
            pathTable[24, 40] = 28;
            pathTable[24, 41] = 32;
            pathTable[24, 42] = 28;
            pathTable[24, 43] = 28;
            pathTable[24, 44] = 32;
            pathTable[24, 45] = 32;

            //node 26

            for (int i = 0; i < 25; i++)
                pathTable[25, i] = 33;

            pathTable[25, 25] = -1;
            pathTable[25, 26] = 26;
            pathTable[25, 27] = 26;
            pathTable[25, 28] = 33;
            pathTable[25, 29] = 26;

            for (int i = 30; i < 34; i++)
                pathTable[25, i] = 33;

            pathTable[25, 34] = 26;
            pathTable[25, 35] = 26;

            for (int i = 36; i < 46; i++)
                pathTable[25, i] = 33;

            //node 27

            for (int i = 0; i < 25; i++)
                pathTable[26, i] = 34;

            pathTable[26, 25] = 25;
            pathTable[26, 26] = -1;
            pathTable[26, 27] = 27;
            pathTable[26, 28] = 34;
            pathTable[26, 29] = 29;

            for (int i = 30; i < 46; i++)
                pathTable[26, i] = 34;

            pathTable[26, 35] = 29;

            //node 28

            for (int i = 0; i < 46; i++)
                pathTable[27, i] = 29;

            pathTable[27, 25] = 28;
            pathTable[27, 26] = 28;
            pathTable[27, 27] = -1;
            pathTable[27, 35] = 35;

            //node 29

            for (int i = 0; i < 24; i++)
                pathTable[28, i] = 23;

            pathTable[28, 24] = 24;

            for (int i = 25; i < 28; i++)
                pathTable[28, i] = 32;

            pathTable[28, 28] = -1;
            pathTable[28, 29] = 32;
            pathTable[28, 30] = 31;
            pathTable[28, 31] = 31;

            for (int i = 32; i < 36; i++)
                pathTable[28, i] = 32;

            pathTable[28, 36] = 37;
            pathTable[28, 37] = 37;
            pathTable[28, 38] = 32;
            pathTable[28, 39] = 32;
            pathTable[28, 40] = 37;
            pathTable[28, 41] = 32;
            pathTable[28, 42] = 37;
            pathTable[28, 43] = 37;
            pathTable[28, 44] = 32;
            pathTable[28, 45] = 32;

            //node 30

            for (int i = 0; i < 46; i++)
                pathTable[29, i] = 34;

            pathTable[29, 25] = 26;
            pathTable[29, 26] = 26;
            pathTable[29, 27] = 27;
            pathTable[29, 29] = -1;

            pathTable[29, 35] = 35;

            //node 31

            for (int i = 0; i < 24; i++)
                pathTable[30, i] = 22;

            for (int i = 24; i < 46; i++)
                pathTable[30, i] = 31;

            pathTable[30, 30] = -1;

            //node 32

            for (int i = 0; i < 25; i++)
                pathTable[31, i] = 23;

            pathTable[31, 25] = 28;
            pathTable[31, 26] = 32;
            pathTable[31, 27] = 32;
            pathTable[31, 28] = 28;
            pathTable[31, 29] = 32;
            pathTable[31, 30] = 30;
            pathTable[31, 31] = -1;

            for (int i = 32; i < 46; i++)
                pathTable[31, i] = 32;

            pathTable[31, 36] = 28;
            pathTable[31, 37] = 28;
            pathTable[31, 40] = 28;
            pathTable[31, 42] = 28;
            pathTable[31, 43] = 28;

            //node 33

            for (int i = 0; i < 24; i++)
                pathTable[32, i] = 28;

            pathTable[32, 24] = 24;

            for (int i = 25; i < 28; i++)
                pathTable[32, i] = 38;

            pathTable[32, 28] = 28;
            pathTable[32, 29] = 38;
            pathTable[32, 30] = 31;
            pathTable[32, 31] = 31;
            pathTable[32, 32] = -1;

            for (int i = 33; i < 46; i++)
                pathTable[32, i] = 38;

            //node 34

            for (int i = 0; i < 25; i++)
                pathTable[33, i] = 41;

            pathTable[33, 25] = 25;
            pathTable[33, 26] = 25;
            pathTable[33, 27] = 34;
            pathTable[33, 28] = 41;
            pathTable[33, 29] = 24;
            pathTable[33, 30] = 24;
            pathTable[33, 31] = 41;
            pathTable[33, 32] = 41;
            pathTable[33, 33] = -1;
            pathTable[33, 34] = 34;
            pathTable[33, 35] = 34;

            for (int i = 36; i < 46; i++)
                pathTable[33, i] = 41;

            //node 35

            for (int i = 0; i < 26; i++)
                pathTable[34, i] = 33;

            pathTable[34, 26] = 26;
            pathTable[34, 27] = 29;

            for (int i = 28; i < 46; i++)
                pathTable[34, i] = 39;

            pathTable[34, 29] = 29;
            pathTable[34, 33] = 33;
            pathTable[34, 34] = -1;
            pathTable[34, 35] = 35;

            //node 36

            for (int i = 0; i < 46; i++)
                pathTable[35, i] = 34;

            pathTable[35, 25] = 29;
            pathTable[35, 26] = 27;
            pathTable[35, 29] = 29;
            pathTable[35, 35] = -1;

            //node 37

            for (int i = 0; i < 46; i++)
                pathTable[36, i] = 37;

            pathTable[36, 36] = -1;
            pathTable[36, 40] = 40;
            pathTable[36, 42] = 42;

            for (int i = 43; i < 46; i++)
                pathTable[36, i] = 40;

            //node 38

            for (int i = 0; i < 25; i++)
                pathTable[37, i] = 28;

            for (int i = 25; i < 28; i++)
                pathTable[37, i] = 38;

            pathTable[37, 28] = 28;
            pathTable[37, 29] = 38;
            pathTable[37, 30] = 28;
            pathTable[37, 31] = 28;

            for (int i = 32; i < 36; i++)
                pathTable[37, i] = 38;

            pathTable[37, 36] = 36;
            pathTable[37, 37] = -1;
            pathTable[37, 38] = 38;
            pathTable[37, 39] = 38;
            pathTable[37, 40] = 40;
            pathTable[37, 41] = 38;
            pathTable[37, 42] = 40;
            pathTable[37, 43] = 43;
            pathTable[37, 44] = 38;
            pathTable[37, 45] = 38;

            //node 39

            for (int i = 0; i < 25; i++)
                pathTable[38, i] = 32;

            pathTable[38, 25] = 41;
            pathTable[38, 26] = 39;
            pathTable[38, 27] = 39;
            pathTable[38, 28] = 33;
            pathTable[38, 29] = 39;

            for (int i = 30; i < 33; i++)
                pathTable[38, i] = 32;

            pathTable[38, 33] = 41;
            pathTable[38, 34] = 39;
            pathTable[38, 35] = 39;
            pathTable[38, 36] = 37;
            pathTable[38, 37] = 37;
            pathTable[38, 38] = -1;
            pathTable[38, 39] = 39;
            pathTable[38, 40] = 37;
            pathTable[38, 41] = 41;
            pathTable[38, 42] = 37;
            pathTable[38, 43] = 44;
            pathTable[38, 44] = 44;
            pathTable[38, 45] = 41;

            //node 40

            for (int i = 0; i < 41; i++)
                pathTable[39, i] = 38;

            for (int i = 25; i < 31; i++)
                pathTable[39, i] = 34;

            for (int i = 33; i < 36; i++)
                pathTable[39, i] = 34;

            pathTable[39, 39] = -1;

            for (int i = 41; i < 45; i++)
                pathTable[39, i] = 41;

            pathTable[39, 45] = 45;
            pathTable[39, 29] = 34;

            //node 41

            for (int i = 0; i < 36; i++)
                pathTable[40, i] = 37;

            pathTable[40, 36] = 36;

            for (int i = 37; i < 42; i++)
                pathTable[40, i] = 37;

            pathTable[40, 40] = -1;
            pathTable[40, 42] = 42;

            for (int i = 43; i < 46; i++)
                pathTable[40, i] = 43;

            //node 42

            for (int i = 0; i < 41; i++)
                pathTable[41, i] = 38;

            pathTable[41, 25] = 33;
            pathTable[41, 26] = 39;
            pathTable[41, 27] = 39;
            pathTable[41, 29] = 39;
            pathTable[41, 33] = 33;
            pathTable[41, 34] = 39;
            pathTable[41, 35] = 39;
            pathTable[41, 39] = 39;
            pathTable[41, 41] = -1;

            for (int i = 42; i < 45; i++)
                pathTable[41, i] = 44;

            pathTable[41, 45] = 45;

            //node 43

            for (int i = 0; i < 41; i++)
                pathTable[42, i] = 40;

            pathTable[42, 36] = 36;

            for (int i = 41; i < 46; i++)
                pathTable[42, i] = 43;

            pathTable[42, 42] = -1;

            //node 44

            for (int i = 0; i < 38; i++)
                pathTable[43, i] = 37;

            for (int i = 25; i < 28; i++)
                pathTable[43, i] = 44;

            pathTable[43, 29] = 44;

            for (int i = 33; i < 36; i++)
                pathTable[43, i] = 44;

            pathTable[43, 36] = 40;
            pathTable[43, 38] = 44;
            pathTable[43, 39] = 44;
            pathTable[43, 40] = 40;
            pathTable[43, 41] = 44;
            pathTable[43, 42] = 42;
            pathTable[43, 43] = -1;
            pathTable[43, 41] = 44;
            pathTable[43, 42] = 44;

            //node 45

            for (int i = 0; i < 38; i++)
                pathTable[44, i] = 38;

            for (int i = 25; i < 28; i++)
                pathTable[44, i] = 41;

            pathTable[44, 29] = 41;

            for (int i = 33; i < 36; i++)
                pathTable[44, i] = 41;

            pathTable[44, 36] = 43;
            pathTable[44, 37] = 43;
            pathTable[44, 39] = 41;
            pathTable[44, 40] = 43;
            pathTable[44, 41] = 41;
            pathTable[44, 42] = 43;
            pathTable[44, 43] = 43;
            pathTable[44, 44] = -1;
            pathTable[44, 45] = 45;

            //node 46

            for (int i = 0; i < 41; i++)
                pathTable[45, i] = 41;

            pathTable[45, 26] = 39;
            pathTable[45, 27] = 39;
            pathTable[45, 29] = 39;
            pathTable[45, 34] = 39;
            pathTable[45, 35] = 39;
            pathTable[45, 36] = 44;
            pathTable[45, 37] = 44;
            pathTable[45, 39] = 39;
            pathTable[45, 40] = 44;

            for (int i = 42; i < 45; i++)
                pathTable[45, i] = 44;

            pathTable[45, 45] = -1;

            // Table

            //        1   2   3   4   5   6   7   8   9   10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26   27   28   29   30   31   32   33   34   35   36   37   38   39   40   41   42   43   44   45   46

            //1       x   2   7   7   7   7   7   7   7    7   12   12    7    7    7   12    7   12   12    7    7    7   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12
            //2       1   x  13  13  13  13   7  13  13   13    7    7   13   13   13    7   13    7    7   13   13   13    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7    7
            //3       4   4   x   4   4   4   4   8   8    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4    4
            //4       6   6   3   x   5   6   6   9   9    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6
            //5      10  10   4   4   x   6  10   6   6   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10   10
            //6      14  14   4   4   5   x  14   9   9   10   14   14   14   14   10   14   14   14   14   14   14   10   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14
            //7       1   2  13  13  13  13   x  13  13   13   12   12   13   13   13   12   13   12   12   13   13   13   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12
            //8       9   9   3   3   9   9   9   x   9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9    9
            //9       6   6   4   4   6   6   6   8   x   10    6    6    6    6   10    6    6    6    6    6    6   10    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6    6
            //10     14  14   6   6   5   6  14   9   9    x   14   14   14   14   15   14   14   14   14   14   14   15   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14   14
            //11     12  12  12  12  12  12  12  12  12   12    x   12   12   12   12   16   12   18   16   16   16   16   18   16   16   16   18   16   18   16   18   16   18   16   18   16   18   16   18   16   18   16   18   16   18   16
            //12      1   7  13  13  13  13   7  13  13   13   11    x   13   13   13   16   13   16   19   19   13   13   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16   16
            //13      7   2  14  14  14  14   7  14  14   14   12   12    x   14   14   12   17   12   20   20   17   17   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12   12
            //14     13  13   6   6  10   6  13   6   6   10   13   13   13    x   15   13   17   17   17   17   21   21   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17   17
            //15     14  14  10  10  10  10  14  10  10   10   14   14   14   14    x   14   14   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22   22
            //16     12  12  12  12  12  12  12  12  12   12   11   12   12   12   12    x   12   18   19   19   19   19   18   24   24   24   24   24   24   24   18   24   24   24   24   24   24   24   24   24   24   24   24   24   24   24
            //17     13  13  14  14  14  14  13  14  14   14   13   13   13   14   15   13    x   20   20   20   21   21   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20
            //18     16  16  16  16  16  16  16  16  16   16   11   16   16   16   16   16   19    x   19   19   19   19   23   24   24   24   24   24   24   24   23   24   24   24   24   24   24   24   24   24   24   24   24   24   24   24
            //19     12  12  20  20  20  20  12  20  20   20   16   12   20   20   20   16   20   18    x   20   20   20   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18   18
            //20     13  13  17  17  17  17  13  17  17   17   19   13   13   17   17   19   17   19   19    x   21   21   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19   19
            //21     17  17  14  14  14  14  17  14  14   14   17   17   17   14   22   20   17   20   20   20    x   22   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20   20
            //22     21  21  15  15  15  15  21  15  15   15   21   21   21   21   14   21   21   21   21   21   21    x   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21   21
            //23     18  18  18  18  18  18  18  18  18   18   18   18   18   18   18   18   18   18   18   18   18   18    x   24   24   24   24   24   24   24   31   31   24   24   24   24   24   24   24   24   24   24   24   24   24   24
            //24     16  16  16  16  16  16  16  16  16   16   16   16   16   16   16   16   16   18   16   16   16   16   23    x   25   29   29   29   29   29   23   32   29   29   29   29   29   29   29   29   29   29   29   29   29   29
            //25     24  24  24  24  24  24  24  24  24   24   24   24   24   24   24   24   24   24   24   24   24   24   24   24    x   33   33   33   29   33   29   29   33   33   33   33   29   29   33   33   29   33   29   29   33   33
            //26     34  34  34  34  34  34  34  34  34   34   34   34   34   34   34   34   34   34   34   34   34   34   34   34   34    x   27   27   34   27   34   34   34   34   27   27   34   34   34   34   34   34   34   34   34   34
            //27     35  35  35  35  35  35  35  35  35   35   35   35   35   35   35   35   35   35   35   35   35   35   35   35   35   26    x   28   35   30   35   35   35   35   35   30   35   35   35   35   35   35   35   35   35   35
            //28     30  30  30  30  30  30  30  30  30   30   30   30   30   30   30   30   30   30   30   30   30   30   30   30   30   27   27    x   30   30   30   30   30   30   30   36   30   30   30   30   30   30   30   30   30   30
            //29     24  24  24  24  24  24  24  24  24   24   24   24   24   24   24   24   24   24   24   24   24   24   24   24   25   33   33   33    x   33   32   32   33   33   33   33   38   38   33   33   38   33   38   38   33   33
            //30     35  35  35  35  35  35  35  35  35   35   35   35   35   35   35   35   35   35   35   35   35   35   35   35   35   27   27   28   35    x   35   35   35   35   35   36   35   35   35   35   35   35   35   35   35   35
            //31     23  23  23  23  23  23  23  23  23   23   23   23   23   23   23   23   23   23   23   23   23   23   23   23   32   32   32   32   32   32    x   32   32   32   32   32   32   32   32   32   32   32   32   32   32   32
            //32     24  24  24  24  24  24  24  24  24   24   24   24   24   24   24   24   24   24   24   24   24   24   24   24   24   29   33   33   29   33   31    x   33   33   33   33   29   29   33   33   29   33   29   29   33   33
            //33     29  29  29  29  29  29  29  29  29   29   29   29   29   29   29   29   29   29   29   29   29   29   29   29   25   39   39   39   29   39   32   32    x   39   39   39   39   39   39   39   39   39   39   39   39   39
            //34     42  42  42  42  42  42  42  42  42   42   42   42   42   42   42   42   42   42   42   42   42   42   42   42   42   26   26   35   42   25   25   42   42    x   35   35   42   42   42   42   42   42   42   42   42   42
            //35     34  34  34  34  34  34  34  34  34   34   34   34   34   34   34   34   34   34   34   34   34   34   34   34   34   34   27   30   40   30   40   40   40   34    x   36   40   40   40   40   40   40   40   40   40   40
            //36     35  35  35  35  35  35  35  35  35   35   35   35   35   35   35   35   35   35   35   35   35   35   35   35   35   30   28   35   35   30   35   35   35   35   35    x   35   35   35   35   35   35   35   35   35   35
            //37     38  38  38  38  38  38  38  38  38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38    x   38   38   38   41   38   43   41   41   41
            //38     29  29  29  29  29  29  29  29  29   29   29   29   29   29   29   29   29   29   29   29   29   29   29   29   29   39   39   39   29   39   29   29   39   39   39   39   37    x   39   39   41   39   41   44   39   39
            //39     33  33  33  33  33  33  33  33  33   33   33   33   33   33   33   33   33   33   33   33   33   33   33   33   33   42   40   40   33   40   33   33   33   42   40   40   38   38    x   40   38   42   38   45   45   42
            //40     39  39  39  39  39  39  39  39  39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   35   35   35   35   35   35   39   39   35   35   35   39   39   39    x   39   42   42   42   42   46
            //41     39  39  39  39  39  39  39  39  39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   37   38   38   38    x   38   43   44   44   44
            //42     39  39  39  39  39  39  39  39  39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   34   40   40   39   40   39   39   39   34   40   40   39   39   39   40   39    x   45   45   45   46
            //43     41  41  41  41  41  41  41  41  41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   41   37   41   41   41   41   44    x   44   44   44
            //44     38  38  38  38  38  38  38  38  38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   38   45   45   45   38   45   38   38   38   45   45   45   41   38   45   45   41   45   43    x   45   45
            //45     39  39  39  39  39  39  39  39  39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   39   42   42   42   39   42   39   39   39   42   42   42   44   44   39   42   44   42   44   44    x   46
            //46     42  42  42  42  42  42  42  42  42   42   42   42   42   42   42   42   42   42   42   42   42   42   42   42   42   42   40   40   42   40   42   42   42   42   40   40   45   45   42   40   45   42   45   45   45    x

        }
    }
}
